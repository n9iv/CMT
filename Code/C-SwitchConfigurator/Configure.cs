using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace C_SwitchConfigurator
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
        SaveDataFaiuled = -9
    };

    class Configure
    {
        private string _path;
        private SerialConfiguration _cSwitch;
        private string _userName = "cisco";
        private string _password = "cisco";
        private const int DELAYTIME = 200;
        private string _mainPath = @"\ccu_sw1.txt";
        private string _redundancyPath = @"\ccu_sw2.txt";
        private string _resetPath;
        private string _switchIP;
        private const string _userNameRep = "USER_NAME";
        private const string _passRep = "USER_PASS";

        public Configure(string port, string path, bool isMain)
        {
            _path = path;
            if (isMain)
                _path += _mainPath;
            else
                _path += _redundancyPath;
            _cSwitch = new SerialConfiguration(port);
        }

        public int Init()
        {
            return _cSwitch.init();
        }

        public int RunScript(int val)
        {
            int resVal = (int)ErrorCodes.Success;
            StreamReader script = File.OpenText(_path);
            string line, rcv = null;
            string[] tokens;

            if ((resVal = _cSwitch.Open()) != (int)ErrorCodes.Success)
                return resVal;

            if ((resVal = this.LogIn()) != (int)ErrorCodes.Success)
            {
                Log.Write("Login to C-Switch failed");
                _cSwitch.Close();
                return resVal;
            }

            while ((line = script.ReadLine()) != null)
            {
                //configure
                line = line.Replace("BN", (70 + val).ToString());
                line = line.Replace(_userNameRep, _userName);
                line = line.Replace(_passRep, _password);
                if (_cSwitch.SendData(line) != ErrorCodes.WritreSerialFailed)
                {
                    resVal = (int)ErrorCodes.WritreSerialFailed;
                    break;
                }
                Thread.Sleep(DELAYTIME);
            }

            script.Close();
            if (resVal != (int)ErrorCodes.Success)
                _cSwitch.Close();
            return resVal;
        }

        private int LogIn()
        {
            int isLogIn = (int)ErrorCodes.Success;
            string rcv;

            _cSwitch.Flush();
            if (_cSwitch.SendData("\r\n") != ErrorCodes.Success)
                return (int)ErrorCodes.WritreSerialFailed;
            Thread.Sleep(3 * DELAYTIME);
            //_cSwitch.ReadData(out rcv, "");
            //if (rcv != "Username: ")
            //    return -1;
            //_cSwitch.SendData(_userName);
            //Thread.Sleep(200);
            //_cSwitch.ReadData(out rcv, _userName);
            //if (rcv != "Password: ")
            //    return -1;
            //_cSwitch.SendData(_password);
            //Thread.Sleep(200);
            //_cSwitch.ReadData(out rcv, "");
            if (_cSwitch.ReadData(out rcv, "") != ErrorCodes.Success)
                return (int)ErrorCodes.ReadSerialFailed;
            if (rcv.Contains(">") == false)
                return (int)ErrorCodes.LoginFailed;
            _cSwitch.Flush();
            if (_cSwitch.SendData("en") != ErrorCodes.Success)
                return (int)ErrorCodes.WritreSerialFailed;
            Thread.Sleep(DELAYTIME);
            if (_cSwitch.ReadData(out rcv, "en") != ErrorCodes.Success)
                return (int)ErrorCodes.ReadSerialFailed;
            if (rcv.Contains("Password: ") == false)
                return (int)ErrorCodes.LoginFailed;
            if (_cSwitch.SendData(_password) != ErrorCodes.Success)
                return (int)ErrorCodes.WritreSerialFailed;
            Thread.Sleep(DELAYTIME);
            if (_cSwitch.ReadData(out rcv, "") != ErrorCodes.Success)
                return (int)ErrorCodes.ReadSerialFailed;
            if (rcv.Contains("#") == false)
                return (int)ErrorCodes.LoginFailed;
            _cSwitch.SendData("conf t");
            Thread.Sleep(DELAYTIME);
            if (_cSwitch.ReadData(out rcv, "conf t") != ErrorCodes.Success)
                return (int)ErrorCodes.ReadSerialFailed;
            if (rcv.Contains("(config)#") == false)
                return (int)ErrorCodes.LoginFailed;

            return isLogIn;
        }

        private int SaveSettings()
        {
            int isSaved = (int)ErrorCodes.Success;
            string rcv;
            UInt16 attempts = 0;

            while ((attempts < 3) && (isSaved == -1))
            {
                _cSwitch.SendData("WR");
                _cSwitch.SendData("Y");
                _cSwitch.ReadData(out rcv, "");

                if (rcv == "Copy Succeded")
                    isSaved = 0;

                attempts++;
            }

            return isSaved;
        }

        protected int ResetRouter()
        {
            string rcv;

            _cSwitch.Flush();
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
            //LogInRouter("r");
            if (_cSwitch.SendData("write erase") != ErrorCodes.Success)
                return (int)ErrorCodes.WritreSerialFailed;
            _cSwitch.SendData("\n");
            Thread.Sleep(2000);
            _cSwitch.SendData("reload");
            Thread.Sleep(2000);
            _cSwitch.SendData("\n");
            Thread.Sleep(5000);
            _cSwitch.ReadData(out rcv, "");
            while (!rcv.Contains("initial configuration dialog? [yes/no]:"))
            {
                _cSwitch.SendData("\r\n");
                Thread.Sleep(3000);
                _cSwitch.ReadData(out rcv, "");
            }

            if (_cSwitch.SendData("no") != ErrorCodes.Success)
                return (int)ErrorCodes.WritreSerialFailed;
            Thread.Sleep(150000);
            Thread.Sleep(DELAYTIME);
            _cSwitch.SendData("\r\n");
            Thread.Sleep(DELAYTIME);
            if (_cSwitch.ReadData(out rcv, "") != ErrorCodes.Success)
                return (int)ErrorCodes.ReadSerialFailed;
            while (rcv.Contains("Router>") == false)
            {
                return (int)ErrorCodes.Failed;
            }
            _cSwitch.SendData("en");
            return (int)ErrorCodes.Success;
        }

        protected int VerifyConfigForRouter()
        {
            int packetsRcv;
            string rcv;
            string[] tokens;

            _cSwitch.Flush();
            if (_cSwitch.SendData("ping " + _switchIP) != ErrorCodes.Success)
                return (int)ErrorCodes.WritreSerialFailed;
            Thread.Sleep(2000);
            if (_cSwitch.ReadData(out rcv, "") != ErrorCodes.Success)
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
    }
}
