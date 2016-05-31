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
        private const int TIMEINTERVAL = 200;
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
            string line, rcv = null;
            string[] tokens;
            _path = GetFilePath(type, router);
            StreamReader script = File.OpenText(_path);
            Log.Write("\nStart write script:");
<<<<<<< HEAD

            Thread.Sleep(TIMEINTERVAL);
            if (_Tswitch.SendData("conf t") != ErrorCodes.Success)
                return (int)ErrorCodes.WritreSerialFailed;
            Thread.Sleep(TIMEINTERVAL);
            if (_Tswitch.ReadData(out rcv, "conf t") != ErrorCodes.Success)
                return (int)ErrorCodes.ReadSerialFailed;
            if (rcv.Contains("(config)#") == false)
                return (int)ErrorCodes.LoginFailed;

=======
>>>>>>> origin/master
            while ((line = script.ReadLine()) != null)
            {
                //configure
                line = line.Replace(type, val.ToString());
<<<<<<< HEAD
                line = line.Replace(_userNameRep, _userName);
                line = line.Replace(_passRep, _password);
=======
>>>>>>> origin/master
                if (_Tswitch.SendData(line) != ErrorCodes.Success)
                {
                    resVal = (int)ErrorCodes.WritreSerialFailed;
                    break;
                }
                Thread.Sleep(TIMEINTERVAL);
                if (line.Contains("ip address "))
                {
                    tokens = line.Split(' ');
                    _switchIP = tokens[3];
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

        protected int LogInSwitch(string type)
        {

            int isLogIn = (int)ErrorCodes.Success;
            string rcv, data;
<<<<<<< HEAD
            Log.Write("Login Switch:");

            _Tswitch.Flush();
            if (_Tswitch.SendData("\r\n") != ErrorCodes.Success)
=======
            Log.Write("Login:");

            _Tswitch.Flush();
            if (_Tswitch.SendData("\n") != ErrorCodes.Success)
>>>>>>> origin/master
                return (int)ErrorCodes.WritreSerialFailed;
            Thread.Sleep(TIMEINTERVAL);
            //_Tswitch.SendData("\n");
            //Thread.Sleep(200);
            if (_Tswitch.ReadData(out rcv, "") != ErrorCodes.Success)
                return (int)ErrorCodes.ReadSerialFailed;
<<<<<<< HEAD
            if (rcv.Contains("#"))
                return (int)ErrorCodes.Success;
            while (rcv.Contains("User Name:") == false)
            {
                _Tswitch.SendData("\r\n");
                Thread.Sleep(15000);
                _Tswitch.ReadData(out rcv, "");
            }
            if (_Tswitch.SendData("admin") != ErrorCodes.Success)
                return (int)ErrorCodes.WritreSerialFailed;
            Thread.Sleep(TIMEINTERVAL);
            if (_Tswitch.ReadData(out rcv, "") != ErrorCodes.Success)
                return (int)ErrorCodes.ReadSerialFailed;
            if (rcv.Contains("#") == false)
            {
                _Tswitch.SendData("\r\n");
                _Tswitch.SendData(_userName);
                Thread.Sleep(TIMEINTERVAL);
                _Tswitch.SendData(_password);
            }
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
            string line, rcv = null;
            string[] tokens;
            bool toTakeIp = false;

            _path = GetFilePath(type, router);
            StreamReader script = File.OpenText(_path);
            Log.Write("\nStart write script:");
            _Tswitch.SendData("conf t");
            Thread.Sleep(TIMEINTERVAL);
            if (_Tswitch.ReadData(out line, "") != ErrorCodes.Success)
                return (int)ErrorCodes.ReadSerialFailed;
            if (line.Contains("(config)#") == false)
                return (int)ErrorCodes.Failed;
            while ((line = script.ReadLine()) != null)
            {
                //configure
                line = line.Replace(type, val.ToString());
                line = line.Replace(_userNameRep, _userName);
                line = line.Replace(_passRep, _password);
                if (_Tswitch.SendData(line) != ErrorCodes.Success)
                {
                    resVal = (int)ErrorCodes.WritreSerialFailed;
                    break;
                }
                Thread.Sleep(TIMEINTERVAL);
                if (line.Contains("interface Ethernet0/0.99"))
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
            Log.Write("Login Router:");

            _Tswitch.Flush();
            if (_Tswitch.SendData("\r\n") != ErrorCodes.Success)
                return (int)ErrorCodes.WritreSerialFailed;
            Thread.Sleep(TIMEINTERVAL);
            //_Tswitch.SendData("\n");
            //Thread.Sleep(200);
            if (_Tswitch.ReadData(out rcv, "") != ErrorCodes.Success)
                return (int)ErrorCodes.ReadSerialFailed;
            while ((rcv.Contains("Router#") == false) && (rcv.Contains("MT9012C login:") == false))
            {
                Thread.Sleep(15000);
                _Tswitch.SendData("\r\n");
                Thread.Sleep(TIMEINTERVAL);
                _Tswitch.ReadData(out rcv, "");
            }
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
            while (rcv.Contains("[root@MT9012C ~]# ") == false)
            {
                Thread.Sleep(TIMEINTERVAL);
                _Tswitch.ReadData(out rcv, "");
            }
=======
            if (rcv != "User Name:")
                return (int)ErrorCodes.LoginFailed;
            if (_Tswitch.SendData(_userName) != ErrorCodes.Success)
                return (int)ErrorCodes.WritreSerialFailed;
            Thread.Sleep(TIMEINTERVAL);
            data = _userName;
            if (type == "r")
            {
                if (_Tswitch.ReadData(out rcv, "_userName") != ErrorCodes.Success)
                    return (int)ErrorCodes.ReadSerialFailed;
                if (rcv != "Password:")
                    return (int)ErrorCodes.LoginFailed;
                if (_Tswitch.SendData(_password) != ErrorCodes.Success)
                    return (int)ErrorCodes.WritreSerialFailed;
                Thread.Sleep(TIMEINTERVAL);
                data = _password;
            }
            if (_Tswitch.ReadData(out rcv, data) != ErrorCodes.Success)
                return (int)ErrorCodes.ReadSerialFailed;
            if (rcv.Contains("#") == false)
                return (int)ErrorCodes.LoginFailed;
            if (type == "r")
            {
                if (_Tswitch.SendData("en") != ErrorCodes.Success)
                    return (int)ErrorCodes.WritreSerialFailed;
                if (rcv != "Password:")
                    return (int)ErrorCodes.LoginFailed;
                if (_Tswitch.SendData(_password) != ErrorCodes.Success)
                    return (int)ErrorCodes.WritreSerialFailed;
                Thread.Sleep(TIMEINTERVAL);
                data = _password;
            }
            if (_Tswitch.SendData("conf t") != ErrorCodes.Success)
                return (int)ErrorCodes.WritreSerialFailed;
            Thread.Sleep(TIMEINTERVAL);
            if (_Tswitch.ReadData(out rcv, "conf t") != ErrorCodes.Success)
                return (int)ErrorCodes.ReadSerialFailed;
            if (rcv.Contains("(config)#") == false)
                return (int)ErrorCodes.LoginFailed;
>>>>>>> origin/master

            if (_Tswitch.SendData("cisco") != ErrorCodes.Success)
                return (int)ErrorCodes.WritreSerialFailed;
            Thread.Sleep(5000);
            _Tswitch.ReadData(out rcv, "");
            //Check if reset configuration is done
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
                while (rcv.Contains("Username:") == false)
                {
                    _Tswitch.SendData("\r\n");
                    Thread.Sleep(TIMEINTERVAL);
                    _Tswitch.ReadData(out rcv, "");
                }
                _Tswitch.SendData("\r\n");
                _Tswitch.SendData("\r\n");
                _Tswitch.SendData(_userName);
                Thread.Sleep(TIMEINTERVAL);
                _Tswitch.ReadData(out rcv, "");
                if (rcv.Contains("Password:") == false)
                    return (int)ErrorCodes.LoginFailed;
                _Tswitch.SendData(_password);
                Thread.Sleep(TIMEINTERVAL);
                if (_Tswitch.ReadData(out rcv, "") != ErrorCodes.Success)
                    return (int)ErrorCodes.ReadSerialFailed;

            }
            _Tswitch.SendData("\r\n");
            Thread.Sleep(5000);
            _Tswitch.ReadData(out rcv, "");
            if (rcv.Contains(">") == false)
                return (int)ErrorCodes.LoginFailed;
            _Tswitch.SendData("en");
            Thread.Sleep(TIMEINTERVAL);
            if (!isRst)
            {
                if (_Tswitch.ReadData(out rcv, "") != ErrorCodes.Success)
                    return (int)ErrorCodes.ReadSerialFailed;
                if (rcv.Contains("Password:") == false)
                    return (int)ErrorCodes.LoginFailed;
                _Tswitch.SendData("cisco");
            }
            Thread.Sleep(TIMEINTERVAL);
            if (_Tswitch.ReadData(out rcv, "") != ErrorCodes.Success)
                return (int)ErrorCodes.ReadSerialFailed;
            if (rcv.Contains("#") == false)
                return (int)ErrorCodes.LoginFailed;
            return isLogIn;
        }

        protected int ResetRouter()
        {
            string rcv;
<<<<<<< HEAD

            _Tswitch.Flush();
            //if (_Tswitch.SendData("\n") != ErrorCodes.Success)
            //    return (int)ErrorCodes.WritreSerialFailed;
            //Thread.Sleep(TIMEINTERVAL);
            ////_Tswitch.SendData("\n");
            ////Thread.Sleep(200);
            //if (_Tswitch.ReadData(out rcv, "") != ErrorCodes.Success)
            //    return (int)ErrorCodes.ReadSerialFailed;
            //while ((rcv.Contains("Router#") == false) && (rcv.Contains("MT9012C login:") == false))
            //{
            //    Thread.Sleep(15000);
            //    _Tswitch.SendData("\r\n");
            //    Thread.Sleep(TIMEINTERVAL);
            //    _Tswitch.ReadData(out rcv, "");
            //}

            //Thread.Sleep(TIMEINTERVAL);
            //if (rcv.Contains("MT9012C login:") == false)
            //    return (int)ErrorCodes.LoginFailed;
            //if (_Tswitch.SendData(_routerDefaultUser) != ErrorCodes.Success)
            //    return (int)ErrorCodes.WritreSerialFailed;
            //Thread.Sleep(TIMEINTERVAL);
            //if (_Tswitch.ReadData(out rcv, _routerDefaultUser) != ErrorCodes.Success)
            //    return (int)ErrorCodes.ReadSerialFailed;
            //if (rcv.Contains("Password:") == false)
            //    return (int)ErrorCodes.Failed;
            //if (_Tswitch.SendData(_routerDefaultPass) != ErrorCodes.Success)
            //    return (int)ErrorCodes.WritreSerialFailed;
            //Thread.Sleep(TIMEINTERVAL);
            //if (_Tswitch.ReadData(out rcv, "") != ErrorCodes.Success)
            //    return (int)ErrorCodes.ReadSerialFailed;
            //while(rcv.Contains("[root@MT9012C ~]# ") == false)
            //{
            //    _Tswitch.SendData("\r\n");
            //    _Tswitch.ReadData(out rcv, "");
            //}
            //Thread.Sleep(TIMEINTERVAL);
            //if (_Tswitch.SendData("cisco") != ErrorCodes.Success)
            //    return (int)ErrorCodes.WritreSerialFailed;
            //Thread.Sleep(TIMEINTERVAL);
            //while((rcv.Contains(">") == false) && (rcv.Contains("Username") == false))
            //{
            //    Thread.Sleep(2000);
            //    _Tswitch.SendData("\r\n");
            //    _Tswitch.ReadData(out rcv, "");
            //}
            //if (rcv.Contains("Username") == true)
            //{

            //}
            LogInRouter("r");
            if (_Tswitch.SendData("write erase") != ErrorCodes.Success)
                return (int)ErrorCodes.WritreSerialFailed;
            _Tswitch.SendData("\n");
            Thread.Sleep(2000);
            _Tswitch.SendData("reload");
            Thread.Sleep(2000);
            _Tswitch.SendData("\n");
            Thread.Sleep(5000);
            _Tswitch.ReadData(out rcv, "");
            while (!rcv.Contains("initial configuration dialog? [yes/no]:"))
            {
                _Tswitch.SendData("\r\n");
                Thread.Sleep(3000);
                _Tswitch.ReadData(out rcv, "");
            }

            if (_Tswitch.SendData("no") != ErrorCodes.Success)
                return (int)ErrorCodes.WritreSerialFailed;
            Thread.Sleep(150000);
            Thread.Sleep(TIMEINTERVAL);
            _Tswitch.SendData("\r\n");
            Thread.Sleep(TIMEINTERVAL);
            if (_Tswitch.ReadData(out rcv, "") != ErrorCodes.Success)
                return (int)ErrorCodes.ReadSerialFailed;
            while (rcv.Contains("Router>") == false)
            {
                return (int)ErrorCodes.Failed;
            }
            _Tswitch.SendData("en");
            return (int)ErrorCodes.Success;
        }

        protected int ResetSwitch()
        {
            string rcv;

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
            while (rcv.Contains("#") == false)
            {
                _Tswitch.SendData("\r\n");
                Thread.Sleep(TIMEINTERVAL);
                _Tswitch.ReadData(out rcv, "");
            }
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
            while (rcv.Contains("User Name:") == false)
=======
            Log.Write("\nSave settings:");
            _Tswitch.Flush();
            if (_Tswitch.SendData("wr") != ErrorCodes.Success)
                return (int)ErrorCodes.WritreSerialFailed;
            Thread.Sleep(TIMEINTERVAL);
            if (_Tswitch.ReadData(out rcv, "") != ErrorCodes.Success)
                return (int)ErrorCodes.ReadSerialFailed;
            if (rcv.Contains("?") == false)
                return (int)ErrorCodes.SaveDataFailed;
            Thread.Sleep(TIMEINTERVAL);
            if (_Tswitch.SendData("y") != ErrorCodes.Success)
                return (int)ErrorCodes.WritreSerialFailed;
            Thread.Sleep(4000);
            if (_Tswitch.ReadData(out rcv, "") != ErrorCodes.Success)
                return (int)ErrorCodes.ReadSerialFailed;
            while (!rcv.Contains("#"))
>>>>>>> origin/master
            {
                Thread.Sleep(10000);
                _Tswitch.SendData("\r\n");
                Thread.Sleep(TIMEINTERVAL);
                if (_Tswitch.ReadData(out rcv, "") != ErrorCodes.Success)
                    return (int)ErrorCodes.ReadSerialFailed;
            }

<<<<<<< HEAD
            _Tswitch.SendData("\r\n");
            _Tswitch.SendData("admin");
            Thread.Sleep(TIMEINTERVAL);
            _Tswitch.ReadData(out rcv, "");
            if (rcv.Contains("console#"))
                return (int)ErrorCodes.Success;

            return (int)ErrorCodes.Failed;

=======
            if (VerifyConfig() != (int)ErrorCodes.Success)
                return (int)ErrorCodes.SaveDataFailed;
            return (int)ErrorCodes.Success;
>>>>>>> origin/master
        }

        protected int SaveSettings(bool isRouter)
        {
            string rcv;
            Log.Write("\nSave settings:");
            _Tswitch.Flush();
            if (_Tswitch.SendData("wr") != ErrorCodes.Success)
                return (int)ErrorCodes.WritreSerialFailed;
            if (isRouter)
            {
                if (VerifyConfigForRouter() != (int)ErrorCodes.Success)
                    return (int)ErrorCodes.SaveDataFailed;
            }
            else
                if (VerifyConfigForSwitch() != (int)ErrorCodes.Success)
                    return (int)ErrorCodes.SaveDataFailed;
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

<<<<<<< HEAD
            if (rcv.Contains("Success rate is 100 percent (0/5)") == true)
=======
            char[] sep = { ',', ' ' };
            tokens = rcv.Split(sep);
            int.TryParse(tokens[17], out packetsRcv);
            Log.Write("Received packets: " + packetsRcv);
            if (packetsRcv < 4 - PACKLOSS)
>>>>>>> origin/master
            {
                Log.Write("\nConfiguration failed");
                return (int)ErrorCodes.ConfigurationFailed;
            }
<<<<<<< HEAD

            Log.Write("\nConfiguration succeeded");
            return (int)ErrorCodes.Success;
        }

        protected int VerifyConfigForSwitch()
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

            if (rcv.Contains("0 packets received,") == true)
            {
                Log.Write("\nConfiguration failed");
                return (int)ErrorCodes.ConfigurationFailed;
            }

=======
>>>>>>> origin/master
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
