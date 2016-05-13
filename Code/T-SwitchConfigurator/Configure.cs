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
    class Configure
    {
        private string _path;
        private string _userName;
        private string _password;
        private SerialConfiguration _Tswitch;
        private string _switchIP;
        private const int PACKLOSS = 3;
        private const int TIMEINTERVAL = 100;

        protected Configure(string port, string path)
        {
            _path = path;
            _Tswitch = new SerialConfiguration(port);
            _userName = XMLparser.switchUserName;
            _password = XMLparser.switchPassword;
        }

        public int Init()
        {
            if (_Tswitch.init() == -1)
                return -1;
            if (_Tswitch.Open() == -1)
                return -1;

            return 0;
        }

        protected virtual int RunScript(int val, string type, bool router)
        {
            int resVal = 0;
            string line, rcv = null;
            string[] tokens;
            _path = GetFilePath(type, router);
            StreamReader script = File.OpenText(_path);
            Console.WriteLine("\nStart run script:");
            while ((line = script.ReadLine()) != null)
            {
                //configure
                line = line.Replace(type, val.ToString());
                _Tswitch.SendData(line);
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
            return resVal;
        }

        protected int LogIn(string type)
        {

            int isLogIn = 0;
            string rcv, data;
            Console.WriteLine("Login:");

            _Tswitch.Flush();
            _Tswitch.SendData("\n");
            Thread.Sleep(200);
            //_Tswitch.SendData("\n");
            //Thread.Sleep(200);
            _Tswitch.ReadData(out rcv, "");
            if (rcv != "User Name:")
                return -1;
            _Tswitch.SendData(_userName);
            Thread.Sleep(200);
            data = _userName;
            if (type == "r")
            {
                _Tswitch.ReadData(out rcv, "_userName");
                if (rcv != "Password:")
                    return -1;
                _Tswitch.SendData(_password);
                Thread.Sleep(200);
                data = _password;
            }
            _Tswitch.ReadData(out rcv, data);
            if (rcv.Contains("#") == false)
                return -1;
            if (type == "r")
            {
                _Tswitch.SendData("en");
                if (rcv != "Password:")
                    return -1;
                _Tswitch.SendData(_password);
                Thread.Sleep(200);
                data = _password;
            }
            _Tswitch.SendData("conf t");
            Thread.Sleep(200);
            _Tswitch.ReadData(out rcv, "conf t");
            if (rcv.Contains("(config)#") == false)
                return -1;

            return isLogIn;
        }

        protected int SaveSettings()
        {
            string rcv;
            Console.WriteLine("\nSave settings:");
            _Tswitch.Flush();
            _Tswitch.SendData("wr");
            Thread.Sleep(TIMEINTERVAL);
            _Tswitch.ReadData(out rcv, "");
            if (rcv.Contains("?") == false)
                return -1;
            Thread.Sleep(TIMEINTERVAL);
            _Tswitch.SendData("y");
            Thread.Sleep(4000);
            _Tswitch.ReadData(out rcv, "");
            while (!rcv.Contains("#"))
            {
                Thread.Sleep(TIMEINTERVAL);
                _Tswitch.ReadData(out rcv, "");
            }

            return VerifyConfig();
        }

        protected int VerifyConfig()
        {
            int packetsRcv;
            string rcv;
            string[] tokens;

            _Tswitch.Flush();
            _Tswitch.SendData("ping " + _switchIP);
            Thread.Sleep(2000);
            _Tswitch.ReadData(out rcv, "");

            char[] sep = { ',', ' ' };
            tokens = rcv.Split(sep);
            int.TryParse(tokens[17], out packetsRcv);
            Console.WriteLine("Received packets: {0}", packetsRcv);
            if (packetsRcv < 4 - PACKLOSS)
            {
                Console.WriteLine("\nConfiguration failed");
                return -1;
            }
            Console.WriteLine("\nConfiguration succeeded");
            return 0;
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
