using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace C_SwitchConfigurator
{
    /// <summary>
    /// Enum for codes the program returns.
    /// </summary>
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
        private string _userName = XMLparser.switchUserName; //stores user name from XML file
        private string _password = XMLparser.switcPassword; //stores password from XML file
        private const int DELAYTIME = 200;
        private string _mainPath = @"\ccu_sw1.txt";
        private string _redundancyPath = @"\ccu_sw2.txt";
        private string _resetPath;
        private string _switchIP;
        private const string _userNameRep = "USER_NAME";
        private const string _passRep = "USER_PASS";
        private const string DEFAULTPASS = "cisco";

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

        /// <summary>
        /// Reads the script file line by line and sends each line to clock via serial.
        /// In each line:
        /// The BN, if exists, is replaced by value.
        /// The USER_PASS and USER_PASS, if exist, are replaced by username and password from the XML file.
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public int RunScript(int val)
        {
            StreamReader script = null;
            int resVal = (int)ErrorCodes.Success;
            try
            {
                script = File.OpenText(_path);
            }
            catch (Exception ex)
            {
                Log.Write(ex.Message);
                return (int)ErrorCodes.Failed;
            }

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

            Log.Write("\nRun Script");

            //Enter the unit to configure mode.
            _cSwitch.SendData("conf t");
            Thread.Sleep(DELAYTIME);
            if (_cSwitch.ReadData(out rcv, "conf t") != ErrorCodes.Success)
                return (int)ErrorCodes.ReadSerialFailed;
            if (rcv.Contains("(config)#") == false)
                return (int)ErrorCodes.LoginFailed;

            while ((line = script.ReadLine()) != null)
            {
                //configure
                if (line == "")
                    continue;
                line = line.Replace("BN", (70 + val).ToString());
                line = line.Replace(_userNameRep, _userName);
                line = line.Replace(_passRep, _password);
                if (_cSwitch.SendData(line) != ErrorCodes.Success)
                {
                    resVal = (int)ErrorCodes.WritreSerialFailed;
                    break;
                }
                if (line.Contains("ip address "))
                {
                    tokens = line.Split(' ');
                    _switchIP = tokens[3];
                }

            }

            script.Close();
            if (resVal != (int)ErrorCodes.Success)
                _cSwitch.Close();
            else
                resVal = SaveSettings();
            return resVal;
        }

        /// <summary>
        /// Login the unit
        /// </summary>
        /// <returns></returns>
        private int LogIn()
        {
            int isLogIn = (int)ErrorCodes.Success, cnt = 0; // cnt - counts number of loop retries
            string rcv;
            Log.Write("\nLogIn");
            _cSwitch.Flush();
            if (_cSwitch.SendData("\r\n") != ErrorCodes.Success)
                return (int)ErrorCodes.WritreSerialFailed;

            if (_cSwitch.ReadData(out rcv, "") != ErrorCodes.Success)
                return (int)ErrorCodes.ReadSerialFailed;
            if (rcv.Contains("#"))
            {
                return (int)ErrorCodes.Success;
            }
            while ((rcv.Contains(">") == false) && (rcv.Contains("Password") == false) && (cnt < 40))
            {
                cnt++;
                Thread.Sleep(15000);
                _cSwitch.SendData("\r\n");
                Thread.Sleep(DELAYTIME);
                _cSwitch.ReadData(out rcv, "");
            }

            if (cnt >= 40)
                return (int)ErrorCodes.LoginFailed;

            if (rcv.Contains("Password"))
                _cSwitch.SendData(DEFAULTPASS);
            Thread.Sleep(DELAYTIME);
            if (_cSwitch.SendData("en") != ErrorCodes.Success)
                return (int)ErrorCodes.WritreSerialFailed;
            Thread.Sleep(DELAYTIME);
            if (_cSwitch.ReadData(out rcv, "en") != ErrorCodes.Success)
                return (int)ErrorCodes.ReadSerialFailed;
            cnt = 0;
            while ((rcv.Contains("Password:") == false) && (cnt < 20))
            {
                cnt++;
                _cSwitch.Flush();
                _cSwitch.SendData("\r\n");
                Thread.Sleep(DELAYTIME);
                _cSwitch.ReadData(out rcv, "");
            }

            if (rcv.Contains("Password:"))
            {
                _cSwitch.SendData(_password);
                Thread.Sleep(DELAYTIME);
                _cSwitch.ReadData(out rcv, "");
            }

            cnt = 0;
            while ((rcv.Contains("#") == false) && (cnt < 20))
            {
                cnt++;
                _cSwitch.Flush();
                _cSwitch.SendData("\r\n");
                Thread.Sleep(DELAYTIME);
                _cSwitch.ReadData(out rcv, "");
            }

            if (cnt >= 20)
                return (int)ErrorCodes.LoginFailed;
            if(rcv.Contains("#") == false) 
                return (int)ErrorCodes.LoginFailed;
            return isLogIn;
        }

        /// <summary>
        /// After the configuration is succeeded, saves the configured settings
        /// </summary>
        /// <returns></returns>
        protected int SaveSettings()
        {
            string rcv;
            int cnt = 0;
            Log.Write("\nSave settings:");
            _cSwitch.Flush();
            if (_cSwitch.SendData("wr") != ErrorCodes.Success)
                return (int)ErrorCodes.WritreSerialFailed;
            Thread.Sleep(2000);
            _cSwitch.ReadData(out rcv, "");
            while ((rcv.Contains("#")) && (cnt < 120))
            {
                cnt++;
                Thread.Sleep(DELAYTIME);
                _cSwitch.ReadData(out rcv, "");
            }
            if (cnt >= 120)
                return (int)ErrorCodes.SaveDataFaiuled;
            return VerifyConfigForRouter();
        }

        /// <summary>
        /// In case the reset option is chosen, reset the unit configuration
        /// </summary>
        /// <returns></returns>
        public int ResetRouter()
        {
            string rcv;
            int resVal, cnt = 0;

            if ((resVal = _cSwitch.Open()) != (int)ErrorCodes.Success)
                return resVal;

            if (LogIn() != (int)ErrorCodes.Success)
                return (int)ErrorCodes.Failed;
            Log.Write("\nReset");
            if (_cSwitch.SendData("write erase") != ErrorCodes.Success)
                return (int)ErrorCodes.WritreSerialFailed;
            Thread.Sleep(2000);
            _cSwitch.SendData("\n");
            Thread.Sleep(2000);
            _cSwitch.SendData("reload");
            Thread.Sleep(2000);
            _cSwitch.SendData("yes");
            Thread.Sleep(1000);
            _cSwitch.ReadData(out rcv, "");
            while ((rcv.Contains("[confirm]") == false) && (cnt < 120))
            {
                cnt++;
                Thread.Sleep(DELAYTIME);
                _cSwitch.ReadData(out rcv, "");
            }
            if (cnt >= 120)
                return (int)ErrorCodes.Failed;
            _cSwitch.SendData("\n");
            Thread.Sleep(5000);
            _cSwitch.ReadData(out rcv, "");
            cnt = 0;
            while ((!rcv.Contains("initial configuration dialog? [yes/no]:")) && (cnt < 400))
            {
                cnt++;
                _cSwitch.SendData("\r\n");
                Thread.Sleep(3000);
                _cSwitch.ReadData(out rcv, "");
            }
            if (cnt >= 400)
                return (int)ErrorCodes.Failed;
            if (_cSwitch.SendData("no") != ErrorCodes.Success)
                return (int)ErrorCodes.WritreSerialFailed;
            Thread.Sleep(1000);
            _cSwitch.ReadData(out rcv, "");
            if (rcv.Contains("Would you like to terminate autoinstall"))
            {
                _cSwitch.SendData("\n");
            }
            Thread.Sleep(30000);
            Thread.Sleep(DELAYTIME);
            _cSwitch.SendData("\r\n");
            Thread.Sleep(DELAYTIME);
            if (_cSwitch.ReadData(out rcv, "") != ErrorCodes.Success)
                return (int)ErrorCodes.ReadSerialFailed;
            cnt = 0;
            while ((rcv.Contains(">") == false) && (rcv.Contains("Password") == false) && (cnt < 850))
            {
                cnt++;
                _cSwitch.SendData("\r\n");
                Thread.Sleep(DELAYTIME);
                _cSwitch.ReadData(out rcv, "");
            }
            if (cnt >= 850)
                return (int)ErrorCodes.Failed;

            if (rcv.Contains("Password"))
                _cSwitch.SendData(DEFAULTPASS);
            Thread.Sleep(DELAYTIME);
            _cSwitch.SendData("en");
            Thread.Sleep(DELAYTIME);
            _cSwitch.ReadData(out rcv, "");
            _cSwitch.Close();
            if (rcv.Contains("Switch#"))
            {
                Log.Write("Reset Succeeded");
                return (int)ErrorCodes.Success;
            }

            return (int)ErrorCodes.Failed;
        }

        protected int VerifyConfigForRouter()
        {
            int packetsRcv;
            string rcv;
            string[] tokens;

            _cSwitch.Flush();
            if (_cSwitch.SendData("ping " + _switchIP) != ErrorCodes.Success)
                return (int)ErrorCodes.WritreSerialFailed;
            Thread.Sleep(10000);
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
