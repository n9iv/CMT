using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace C_SwitchConfigurator
{
    class Configure
    {
        private string _path;
        private SerialConfiguration _cSwitch;
        private string _userName = "cisco";
        private string _password = "cisco";

        public Configure(string port, string path)
        {
            _path = path;
            _cSwitch = new SerialConfiguration(port);
        }

        public int Init()
        {
            return _cSwitch.init();
        }

        public int RunScript(int val)
        {
            int resVal = 0;
            bool isMFU = true, skip = false;
            StreamReader script = File.OpenText(_path);
            string line, rcv = null;
            string[] tokens;
            if (_cSwitch.Open() == -1)
                return -1;

            if (this.LogIn() == -1)
            {
                Console.WriteLine("Login to C-Switch failed");
                return -1;
            }
                
            if (val > 0)
            {
                isMFU = false;
            }
            while ((line = script.ReadLine()) != null)
            {
                //configure
                line = line.Replace("BN", (70 + val).ToString());
                _cSwitch.SendData(line);
                Thread.Sleep(100);
            }

            script.Close();
            _cSwitch.Close();
            return resVal;
        }

        private int LogIn()
        {
            int isLogIn = 0;
            string rcv;

            _cSwitch.Flush();
            _cSwitch.SendData("\r\n");
            Thread.Sleep(500);
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
            _cSwitch.ReadData(out rcv, "");
            if (rcv.Contains(">") == false)
                return -1;
            _cSwitch.Flush();
            _cSwitch.SendData("en");
            Thread.Sleep(200);
            _cSwitch.ReadData(out rcv, "en");
            if (rcv.Contains("Password: ") == false)
                return -1;
            _cSwitch.SendData(_password);
            Thread.Sleep(200);
            _cSwitch.ReadData(out rcv, "");
            if (rcv.Contains("#") == false)
                return -1;
            _cSwitch.SendData("conf t");
            Thread.Sleep(200);
            _cSwitch.ReadData(out rcv, "conf t");
            if (rcv.Contains("(config)#") == false)
                return -1;

            return isLogIn;
        }

        private int SaveSettings()
        {
            int isSaved = -1;
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
    }
}
