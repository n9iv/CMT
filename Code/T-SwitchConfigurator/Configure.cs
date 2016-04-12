using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace T_SwitchConfigurator
{
    class Configure
    {
        private string _path;
        private string _userName;
        private SerialConfiguration _Tswitch;

        protected Configure(string port, string path) 
        {
            _path = path;
            _Tswitch = new SerialConfiguration(port);
        }

        public int Init()
        {
            int resVal;
            resVal = _Tswitch.init();

            return resVal;

        }

        protected virtual int RunScript(int MN, int BN)
        {
            int resVal = 0;
            StreamReader script = File.OpenText(_path);
            string line, rcv = null;
            string[] tokens;
            // _clock.Open();

            while ((line = script.ReadLine()) != null)
            {
                //configure
               line = line.Replace("MN", MN.ToString());
               line = line.Replace("BN", BN.ToString());
              //  _Tswitch.SendData(line);
                Thread.Sleep(200);

                //Check configured value
                tokens = line.Split(' ');
                //_Tswitch.SendData(tokens[0]);
               // _Tswitch.ReadData(out rcv);
                if (rcv != line)
                {
                    resVal = -1;
                    Console.WriteLine("{0}: configured value - {1}  received value - {2}", tokens[0], line, rcv);
                }
                else
                {
                    Console.WriteLine("value is properly configured.");
                }
            }
            script.Close();
            //_Tswitch.Close();
            return resVal;
        }

        protected bool LogIn()
        {
            bool isLogIn = true;
            string rcv;

            _Tswitch.SendData("");
            _Tswitch.ReadData(out rcv);
            Console.Write(rcv);
            if (rcv != "User:")
                isLogIn = false;
            _Tswitch.SendData(_userName);
            Console.WriteLine(_userName);
            _Tswitch.ReadData(out rcv);
            if (rcv != "Consol#")
                isLogIn = false;

            return isLogIn;
        }

        protected bool SaveSettings()
        {
            bool isSaved = false;
            string rcv;
            UInt16 attempts = 0;

            while ((attempts < 3) && (!isSaved))
            {
                _Tswitch.SendData("WR");
                _Tswitch.SendData("Y");
                _Tswitch.ReadData(out rcv);

                if (rcv == "Copy Succeded")
                    isSaved = true;

                attempts++;
            }

            return isSaved;
        }
    }
}
