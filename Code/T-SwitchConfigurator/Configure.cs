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
        private const int TIMEINTERVAL = 100;

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

        protected virtual int RunScript(int val, string type, bool router)
        {
            int resVal = 0;
            string line, rcv = null;
            string[] tokens;
            _path = GetFilePath(type, router);
            StreamReader script = File.OpenText(_path);
            Log.Write("\nStart write script:");
            while ((line = script.ReadLine()) != null)
            {
                //configure
                line = line.Replace(type, val.ToString());
                if (_Tswitch.SendData(line) != ErrorCodes.Success)
                {
                    resVal = (int)ErrorCodes.WritreSerialFailed;
                    break;
                }
                Thread.Sleep(TIMEINTERVAL);
                if (line.Contains("ip "))
                {
                    tokens = line.Split(' ');
                    _switchIP = tokens[2];
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

        protected int LogIn(string type)
        {

            int isLogIn = (int)ErrorCodes.Success;
            string rcv, data;
            Log.Write("Login:");

            _Tswitch.Flush();
            if (_Tswitch.SendData("\n") != ErrorCodes.Success)
                return (int)ErrorCodes.WritreSerialFailed;
            Thread.Sleep(TIMEINTERVAL);
            //_Tswitch.SendData("\n");
            //Thread.Sleep(200);
            if (_Tswitch.ReadData(out rcv, "") != ErrorCodes.Success)
                return (int)ErrorCodes.ReadSerialFailed;
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

            return isLogIn;
        }

        protected int SaveSettings()
        {
            string rcv;
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
            {
                Thread.Sleep(TIMEINTERVAL);
                if (_Tswitch.ReadData(out rcv, "") != ErrorCodes.Success)
                    return (int)ErrorCodes.ReadSerialFailed;
            }

            if (VerifyConfig() != (int)ErrorCodes.Success)
                return (int)ErrorCodes.SaveDataFailed;
            return (int)ErrorCodes.Success;
        }

        protected int VerifyConfig()
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

            char[] sep = { ',', ' ' };
            tokens = rcv.Split(sep);
            int.TryParse(tokens[17], out packetsRcv);
            Log.Write("Received packets: " + packetsRcv);
            if (packetsRcv < 4 - PACKLOSS)
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
