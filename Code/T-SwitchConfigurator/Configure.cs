using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Net.NetworkInformation;

namespace T_SwitchConfigurator
{
    public enum ErrorCodes
    {
        Success = 0,
        Failed = -1,
        SPConnectionFailed = -2,
        ReadSerialFailed = -3,
        WritreSerialFailed = -4,
        ConfigurationFailed = -5,
        XMLFieldMissing = -6,
        XMLFileMissing = -7,
        LoginFailed = -8,
        SaveDataFailed = -9
    };

    class Configure
    {
        private string _path;
        private string _userName;
        private string _password;
        private SerialConfiguration _Tswitch;
        private string _switchIP;
        private const int PACKLOSS = 3;
        private const int TIMEINTERVAL = 300;
        private const string _userNameRep = "USER_NAME";
        private const string _passRep = "USER_PASS";
        private const string _routerDefaultUser = "root";
        private const string _routerDefaultPass = "123456";
        public SerialConfiguration Tswitch
        {
            get
            {
                return _Tswitch;
            }
            set { }
        }

        protected Configure(string port, string path)
        {
            _path = path;
            _Tswitch = new SerialConfiguration(port);
            _userName = XMLparser.switchUserName;
            _password = XMLparser.switchPassword;
        }

        public int Init()
        {
            int ret = (int)ErrorCodes.Success;

            if ((ret = _Tswitch.init()) != (int)ErrorCodes.Success)
                return ret;
            if ((ret = _Tswitch.Open()) != (int)ErrorCodes.Success)
                return ret;

            return ret;
        }

        protected virtual int RunScriptSwitch(int val, string type, bool router)
        {
            int resVal = 0;
            string line, rcv = null, interfaceStr = null;
            string[] tokens;
            bool toTakeIp = false;
            StreamReader script = null;
            _path = GetFilePath(type, router);

            try
            {
                script = File.OpenText(_path);
            }
            catch (Exception ex)
            {
                Log.Write(ex.Message);
                return (int)ErrorCodes.Failed;
            }

            if (type == "BN")
                interfaceStr = "interface vlan 14";
            else
                interfaceStr = "interface vlan 99";

            Log.Write("\nStart write script:");

            Thread.Sleep(TIMEINTERVAL);
            if (_Tswitch.SendData("conf t") != ErrorCodes.Success)
                return (int)ErrorCodes.WritreSerialFailed;
            Thread.Sleep(TIMEINTERVAL);
            if (_Tswitch.ReadData(out rcv, "conf t") != ErrorCodes.Success)
                return (int)ErrorCodes.ReadSerialFailed;
            if (rcv.Contains("(config)#") == false)
                return (int)ErrorCodes.LoginFailed;

            while ((line = script.ReadLine()) != null)
            {
                //configure
                if (line == "")
                    continue;
                line = line.Replace(type, val.ToString());
                line = line.Replace(_userNameRep, _userName);
                line = line.Replace(_passRep, _password);
                if (_Tswitch.SendData(line) != ErrorCodes.Success)
                {
                    resVal = (int)ErrorCodes.WritreSerialFailed;
                    break;
                }
                Thread.Sleep(20);
                if (line.Contains(interfaceStr))
                    toTakeIp = true;
                if (line.Contains("ip address ") && toTakeIp)
                {
                    tokens = line.Split(' ');
                    _switchIP = tokens[3];
                    toTakeIp = false;
                }
            }
            script.Close();
            if (resVal != (int)ErrorCodes.Success)
                _Tswitch.Close();
            return resVal;
        }

        protected int LogInSwitch(string type)
        {

            int isLogIn = (int)ErrorCodes.Success;
            string rcv, data;
            int flag = 0;
            int cnt = 0, cnt1 = 0;

            Log.Write("Login Switch:");

            _Tswitch.Flush();
            if (_Tswitch.SendData("\r\n") != ErrorCodes.Success)
                return (int)ErrorCodes.WritreSerialFailed;
            Thread.Sleep(TIMEINTERVAL);
            //_Tswitch.SendData("\n");
            //Thread.Sleep(200);
            if (_Tswitch.ReadData(out rcv, "") != ErrorCodes.Success)
                return (int)ErrorCodes.ReadSerialFailed;
            if (rcv.Contains("#"))
                return (int)ErrorCodes.Success;
            while ((rcv.Contains("User Name:") == false) && (cnt < 15))
            {
                cnt++;
                Thread.Sleep(15000);
                _Tswitch.ReadData(out rcv, "");
            }
            if (cnt >= 15)
                return (int)ErrorCodes.LoginFailed;

            cnt = 0;
            while ((rcv.IndexOf("authentication failed") > rcv.IndexOf("User Name")) && (cnt < 100))
            {
                cnt++;
                _Tswitch.Flush();
                _Tswitch.SendData("\r\n");
                Thread.Sleep(TIMEINTERVAL);
                _Tswitch.ReadData(out rcv, "");
            }
            if (cnt >= 100)
                return (int)ErrorCodes.LoginFailed;
            Thread.Sleep(TIMEINTERVAL);
            if (rcv.Contains("User Name:"))
                _Tswitch.SendData("admin");
            else
            {
                _Tswitch.SendData("\r\n");
                Thread.Sleep(TIMEINTERVAL);
                _Tswitch.SendData("admin");
            }
            Thread.Sleep(TIMEINTERVAL);
            _Tswitch.ReadData(out rcv, "");
            cnt = 0;
            cnt1 = 0;
            while ((rcv.Contains("authentication failed") || rcv.Contains("Password")) && (cnt < 100))
            {
                cnt++;
                _Tswitch.SendData("\r\n");
                Thread.Sleep(TIMEINTERVAL);
                cnt1 = 0;
                while ((rcv.Contains("User Name:") == false) && (cnt1 < 100))
                {
                    cnt1++;
                    _Tswitch.Flush();
                    _Tswitch.SendData("\r\n");
                    Thread.Sleep(TIMEINTERVAL);
                    _Tswitch.ReadData(out rcv, "");
                }

                if (cnt1 >= 100)
                    return (int)ErrorCodes.LoginFailed;
                _Tswitch.SendData(_userName);
                Thread.Sleep(TIMEINTERVAL);
                _Tswitch.ReadData(out rcv, "");
                if (rcv.Contains("Password:"))
                {
                    _Tswitch.SendData(_password);
                    Thread.Sleep(TIMEINTERVAL);
                    _Tswitch.ReadData(out rcv, "");
                }

            }
            if (cnt >= 100)
                return (int)ErrorCodes.LoginFailed;
            _Tswitch.SendData("\r\n");
            Thread.Sleep(TIMEINTERVAL);
            if (_Tswitch.ReadData(out rcv, "") != ErrorCodes.Success)
                return (int)ErrorCodes.ReadSerialFailed;
            if (rcv.Contains("#") == false)
                return (int)ErrorCodes.LoginFailed;

            return isLogIn;
        }

        protected virtual int RunScriptRouter(int val, string type, bool router)
        {
            int resVal = 0;
            string line, rcv = null, interfaceStr = null;
            string[] tokens;
            StreamReader script = null;
            bool toTakeIp = false;

            _path = GetFilePath(type, router);
            try
            {
                script = File.OpenText(_path);
            }
            catch (Exception ex)
            {
                Log.Write(ex.Message);
                return (int)ErrorCodes.Failed;
            }

            if (type == "BN")
                interfaceStr = "interface Ethernet0/0.14";
            else
                interfaceStr = "interface Ethernet0/0.99";

            Log.Write("\nRun Script:");
            _Tswitch.SendData("conf t");
            Thread.Sleep(TIMEINTERVAL);
            if (_Tswitch.ReadData(out line, "") != ErrorCodes.Success)
                return (int)ErrorCodes.ReadSerialFailed;
            if (line.Contains("(config)#") == false)
                return (int)ErrorCodes.Failed;
            while ((line = script.ReadLine()) != null)
            {
                //configure
                if (line == "")
                    continue;
                line = line.Replace(type, val.ToString());
                line = line.Replace(_userNameRep, _userName);
                line = line.Replace(_passRep, _password);
                if (_Tswitch.SendData(line) != ErrorCodes.Success)
                {
                    resVal = (int)ErrorCodes.WritreSerialFailed;
                    break;
                }
                Thread.Sleep(20);
                if (line.Contains(interfaceStr))
                    toTakeIp = true;
                if ((line.Contains("ip address")) && (toTakeIp))
                {
                    tokens = line.Split(' ');
                    _switchIP = tokens[3];
                    toTakeIp = false;
                }
                //Check configured value
                //tokens = line.Split(' ');
                //_Tswitch.SendData(tokens[0]);
                //_Tswitch.ReadData(out rcv,"");

            }
            script.Close();
            if (resVal != (int)ErrorCodes.Success)
                _Tswitch.Close();
            return resVal;
        }

        protected int LogInRouter(string type)
        {
            bool isRst = false;
            int isLogIn = (int)ErrorCodes.Success;
            string rcv, data;
            int cnt = 0;

            Log.Write("Login Router:");

            _Tswitch.Flush();
            if (_Tswitch.SendData("\r\n") != ErrorCodes.Success)
                return (int)ErrorCodes.WritreSerialFailed;
            Thread.Sleep(TIMEINTERVAL);
            //_Tswitch.SendData("\n");
            //Thread.Sleep(200);
            if (_Tswitch.ReadData(out rcv, "") != ErrorCodes.Success)
                return (int)ErrorCodes.ReadSerialFailed;
            while ((rcv.Contains("Router#") == false) && (rcv.Contains("MT9012C login:") == false) && (cnt < 12))
            {
                cnt++;
                Thread.Sleep(15000);
                _Tswitch.SendData("\r\n");
                Thread.Sleep(TIMEINTERVAL);
                _Tswitch.ReadData(out rcv, "");
            }

            if (cnt >= 12)
                return (int)ErrorCodes.LoginFailed;
            _Tswitch.SendData("\r\n");
            Thread.Sleep(4000);
            _Tswitch.SendData("\r\n");
            Thread.Sleep(TIMEINTERVAL);
            _Tswitch.ReadData(out rcv, "");
            if (rcv.Contains("Router#") == true)
                return (int)ErrorCodes.Success;
            if (rcv.Contains("MT9012C login:") == false)
                return (int)ErrorCodes.LoginFailed;
            if (_Tswitch.SendData(_routerDefaultUser) != ErrorCodes.Success)
                return (int)ErrorCodes.WritreSerialFailed;
            Thread.Sleep(TIMEINTERVAL);
            data = _routerDefaultUser;
            if (_Tswitch.ReadData(out rcv, data) != ErrorCodes.Success)
                return (int)ErrorCodes.ReadSerialFailed;
            if (rcv.Contains("Password:") == false)
                return (int)ErrorCodes.LoginFailed;
            if (_Tswitch.SendData(_routerDefaultPass) != ErrorCodes.Success)
                return (int)ErrorCodes.WritreSerialFailed;
            Thread.Sleep(TIMEINTERVAL);
            if (_Tswitch.ReadData(out rcv, "") != ErrorCodes.Success)
                return (int)ErrorCodes.ReadSerialFailed;
            cnt = 0;
            while ((rcv.Contains("[root@MT9012C ~]# ") == false) && (cnt < 100))
            {
                cnt++;
                Thread.Sleep(TIMEINTERVAL);
                _Tswitch.ReadData(out rcv, "");
            }
            if (cnt >= 100)
                return (int)ErrorCodes.LoginFailed;
            if (_Tswitch.SendData("cisco") != ErrorCodes.Success)
                return (int)ErrorCodes.WritreSerialFailed;
            Thread.Sleep(20000);
            _Tswitch.ReadData(out rcv, "");
            //Check if reset configuration is done
            cnt = 0;
            while ((rcv.Contains("initial configuration dialog? [yes/no]:") == false) && (!rcv.Contains("Username:")) && (cnt < 10))
            {
                cnt++;
                _Tswitch.SendData("\r\n");
                Thread.Sleep(20000);
                _Tswitch.ReadData(out rcv, "");
            }
            if (cnt >= 10)
                return (int)ErrorCodes.LoginFailed;
            if (rcv.Contains("initial configuration dialog? [yes/no]:") == true)
                isRst = true;

            if (isRst)
            {
                if (_Tswitch.SendData("no") != ErrorCodes.Success)
                    return (int)ErrorCodes.WritreSerialFailed;
                Thread.Sleep(3000);
            }
            else
            {
                _Tswitch.SendData("\r\n");
                Thread.Sleep(TIMEINTERVAL);
                cnt = 0;
                while ((rcv.Contains("Username:") == false) && (cnt < 450))
                {
                    cnt++;
                    _Tswitch.SendData("\r\n");
                    Thread.Sleep(TIMEINTERVAL);
                    _Tswitch.ReadData(out rcv, "");
                }
                if (cnt >= 450)
                    return (int)ErrorCodes.LoginFailed;

                _Tswitch.Flush();
                _Tswitch.SendData("\r\n");
                Thread.Sleep(TIMEINTERVAL);
                _Tswitch.ReadData(out rcv, "");

                cnt = 0;
                while ((rcv != "User Access VerificationUsername: ") && (cnt < 20))
                {
                    cnt++;
                    _Tswitch.Flush();
                    _Tswitch.SendData("\r\n");
                    Thread.Sleep(TIMEINTERVAL);
                    _Tswitch.ReadData(out rcv, "");
                }

                if (cnt >= 20)
                    return (int)ErrorCodes.LoginFailed;
                _Tswitch.SendData(_userName);
                Thread.Sleep(600);
                _Tswitch.ReadData(out rcv, "");
                if (rcv.Contains("Password:") == false)
                    return (int)ErrorCodes.LoginFailed;
                _Tswitch.SendData(_password);
                Thread.Sleep(600);
                if (_Tswitch.ReadData(out rcv, "") != ErrorCodes.Success)
                    return (int)ErrorCodes.ReadSerialFailed;

            }
            _Tswitch.SendData("\r\n");
            Thread.Sleep(TIMEINTERVAL);
            if (_Tswitch.ReadData(out rcv, "") != ErrorCodes.Success)
                return (int)ErrorCodes.ReadSerialFailed;
            if (rcv.Contains(">") == true)
            {
                _Tswitch.SendData("en");
                Thread.Sleep(TIMEINTERVAL);
                _Tswitch.ReadData(out rcv, "");
            }

            if (rcv.Contains("#") == false)
                return (int)ErrorCodes.LoginFailed;
            return isLogIn;
        }

        protected int ResetRouter()
        {
            string rcv;
            int cnt = 0, res;
            _Tswitch.Flush();

            if ((res = LogInRouter("r")) != (int)ErrorCodes.Success)
                return res;
            Log.Write("\nReset Router:");
            if (_Tswitch.SendData("write erase") != ErrorCodes.Success)
                return (int)ErrorCodes.WritreSerialFailed;
            _Tswitch.SendData("\n");
            Thread.Sleep(2000);
            _Tswitch.SendData("reload");
            Thread.Sleep(2000);
            _Tswitch.SendData("\n");
            Thread.Sleep(5000);
            _Tswitch.ReadData(out rcv, "");
            Thread.Sleep(20000);
            while ((!rcv.Contains("initial configuration dialog? [yes/no]:")) && (!rcv.Contains("Router>")) && (cnt < 30))
            {
                cnt++;
                _Tswitch.SendData("\r\n");
                Thread.Sleep(3000);
                _Tswitch.ReadData(out rcv, "");
            }
            if (cnt >= 30)
                return (int)ErrorCodes.Failed;
            if (rcv.Contains("initial configuration dialog? [yes/no]:"))
            {
                if (_Tswitch.SendData("no") != ErrorCodes.Success)
                    return (int)ErrorCodes.WritreSerialFailed;
                Thread.Sleep(150000);
            }

            Thread.Sleep(TIMEINTERVAL);
            _Tswitch.SendData("\r\n");
            Thread.Sleep(TIMEINTERVAL);
            if (_Tswitch.ReadData(out rcv, "") != ErrorCodes.Success)
                return (int)ErrorCodes.ReadSerialFailed;
            cnt = 0;
            while ((rcv.Contains("Router>") == false) && (cnt < 100))
            {
                cnt++;
                _Tswitch.SendData("\r\n");
                Thread.Sleep(TIMEINTERVAL);
                _Tswitch.ReadData(out rcv, "");
            }
            if (cnt >= 100)
                return (int)ErrorCodes.Failed;
            _Tswitch.SendData("en");
            Log.Write("Reset succeeded:");
            return (int)ErrorCodes.Success;
        }

        protected int ResetSwitch()
        {
            string rcv;
            int cnt = 0;

            Log.Write("\nReset Router:");
            _Tswitch.SendData("\r\n");
            Thread.Sleep(TIMEINTERVAL);
            if (_Tswitch.ReadData(out rcv, "") != ErrorCodes.Success)
                return (int)ErrorCodes.ReadSerialFailed;
            if (rcv.Contains("#") == false)
                return (int)ErrorCodes.LoginFailed;
            if (_Tswitch.SendData("delete startup-config") != ErrorCodes.Success)
                return (int)ErrorCodes.WritreSerialFailed;
            Thread.Sleep(TIMEINTERVAL);
            _Tswitch.SendData("Y");
            while ((rcv.Contains("#") == false) && (cnt < 100))
            {
                cnt++;
                _Tswitch.SendData("\r\n");
                Thread.Sleep(TIMEINTERVAL);
                _Tswitch.ReadData(out rcv, "");
            }
            if (cnt >= 100)
                return (int)ErrorCodes.Failed;
            if (_Tswitch.SendData("reload") != ErrorCodes.Success)
                return (int)ErrorCodes.WritreSerialFailed;
            Thread.Sleep(TIMEINTERVAL);
            _Tswitch.ReadData(out rcv, "");
            if (rcv.Contains("Y"))
                _Tswitch.SendData("Y", true);
            Thread.Sleep(7000);
            _Tswitch.ReadData(out rcv, "");
            Thread.Sleep(TIMEINTERVAL);
            while (rcv.Contains("Y") == false)
            {
                Thread.Sleep(TIMEINTERVAL);
                _Tswitch.ReadData(out rcv, "");
            }
            _Tswitch.SendData("Y", true);
            Thread.Sleep(TIMEINTERVAL);
            _Tswitch.ReadData(out rcv, "");
            _Tswitch.Flush();
            cnt = 0;
            while ((rcv.Contains("User Name:") == false) && (cnt < 25))
            {
                cnt++;
                Thread.Sleep(10000);
                _Tswitch.SendData("\r\n");
                Thread.Sleep(TIMEINTERVAL);
                _Tswitch.ReadData(out rcv, "");
            }
            if (cnt >= 25)
                return (int)ErrorCodes.Failed;
            cnt = 0;
            while ((rcv.IndexOf("authentication failed") > rcv.IndexOf("User Name")) && (cnt < 120))
            {
                cnt++;
                _Tswitch.Flush();
                _Tswitch.SendData("\r\n");
                Thread.Sleep(TIMEINTERVAL);
                _Tswitch.ReadData(out rcv, "");
            }
            if (cnt >= 120)
                return (int)ErrorCodes.Failed;
            Thread.Sleep(TIMEINTERVAL);
            _Tswitch.SendData("admin");
            Thread.Sleep(TIMEINTERVAL);
            _Tswitch.ReadData(out rcv, "");
            if (rcv.Contains("console#"))
            {
                Log.Write("Reset succeeded:");
                return (int)ErrorCodes.Success;
            }

            else
            {
                _Tswitch.SendData("\r\n");
                _Tswitch.SendData("admin");
                Thread.Sleep(TIMEINTERVAL);
                _Tswitch.ReadData(out rcv, "");
                if (rcv.Contains("console#"))
                {
                    Log.Write("Reset succeeded:");
                    return (int)ErrorCodes.Success;
                }
            }
            return (int)ErrorCodes.Failed;

        }

        protected int SaveSettings(bool isRouter)
        {
            string rcv;
            Log.Write("\nSave settings:");
            _Tswitch.Flush();
            if (_Tswitch.SendData("wr") != ErrorCodes.Success)
                return (int)ErrorCodes.WritreSerialFailed;
            Thread.Sleep(2000);
            if (isRouter)
            {
                if (VerifyConfigForRouter() != (int)ErrorCodes.Success)
                    return (int)ErrorCodes.SaveDataFailed;
            }
            else
            {
                _Tswitch.ReadData(out rcv, "");
                if (rcv.Contains("Overwrite file [startup-config].... (Y/N)"))
                    _Tswitch.SendData("y", true);
                Thread.Sleep(20000);
                _Tswitch.ReadData(out rcv, "");

                if (rcv.Contains("Copy succeeded"))
                {
                    if (VerifyConfigForSwitch() != (int)ErrorCodes.Success)
                        return (int)ErrorCodes.SaveDataFailed;
                }
            }
            return (int)ErrorCodes.Success;
        }

        protected int VerifyConfigForRouter()
        {
            int packetsRcv;
            string rcv;
            string[] tokens;

            _Tswitch.Flush();
            if (_Tswitch.SendData("ping " + _switchIP) != ErrorCodes.Success)
                return (int)ErrorCodes.WritreSerialFailed;
            Thread.Sleep(2000);
            if (_Tswitch.ReadData(out rcv, "") != ErrorCodes.Success)
                return (int)ErrorCodes.ReadSerialFailed;

            if ((rcv.Contains("Success rate") == true))
            {
                if (rcv.Contains("Success rate is 100 percent (0/5)") == true)
                {
                    Log.Write("\nConfiguration failed");
                    return (int)ErrorCodes.ConfigurationFailed;
                }
            }
            else
            {
                Log.Write("\nConfiguration failed");
                return (int)ErrorCodes.ConfigurationFailed;
            }
            Log.Write("\nConfiguration succeeded");
            return (int)ErrorCodes.Success;
        }

        protected int VerifyConfigForSwitch()
        {
            int packetsRcv;
            string rcv;
            string[] tokens;

            _Tswitch.Flush();
            _Tswitch.SendData("\r\n");
            Thread.Sleep(TIMEINTERVAL);
            if (_Tswitch.SendData("ping " + _switchIP) != ErrorCodes.Success)
                return (int)ErrorCodes.WritreSerialFailed;
            Thread.Sleep(2000);
            if (_Tswitch.ReadData(out rcv, "") != ErrorCodes.Success)
                return (int)ErrorCodes.ReadSerialFailed;

            if (rcv.Contains("packets received,") == true)
            {
                if (rcv.Contains("0 packets received,") == true)
                {
                    Log.Write("\nConfiguration failed");
                    return (int)ErrorCodes.ConfigurationFailed;
                }
            }
            else
            {
                Log.Write("\nConfiguration failed");
                return (int)ErrorCodes.ConfigurationFailed;
            }
            Log.Write("\nConfiguration succeeded");
            return (int)ErrorCodes.Success;
        }

        private string GetFilePath(string type, bool router)
        {
            string scriptPath = _path;

            switch (type)
            {
                case "MN":
                    if (router)
                        scriptPath = scriptPath + "\\" + Switch.routeMN;
                    else
                        scriptPath = scriptPath + "\\" + Switch.switchMN;
                    break;
                case "BN":
                    if (router)
                        scriptPath = scriptPath + "\\" + Switch.routeBN;
                    else
                        scriptPath = scriptPath + "\\" + Switch.switchBN;
                    break;
            }

            return scriptPath;
        }
    }
}
