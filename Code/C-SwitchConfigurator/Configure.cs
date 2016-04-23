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
                Console.WriteLine("Login to C-Switch is failed");
                return -1;
            }
                
            if (val > 0)
            {
                isMFU = false;
            }
            while ((line = script.ReadLine()) != null)
            {
                //configure

                //if (isMFU)
                //{
                //    if (line == "[MFU]")
                //        continue;
                //    if (line == "[else]")
                //        skip = true;
                //}
                //else
                //{
                //    if (line == "[MFU]")
                //        skip = true;
                //    if (line == "[else]")
                //    {
                //        skip = false;
                //        continue;
                //    }

                //}
                //if (line == "[end]")
                //{
                //    skip = false;
                //    continue;
                //}

                //if (skip)
                //    continue;

                //if (!isMFU)
                //{
                //    line = line.Replace("{0}", (70 + val).ToString());
                //}
                //else
                //{
                //    line = line.Replace("{0}", val.ToString());
                //}
                //_cSwitch.SendData(line);
                //// _clock.Flush();

                ////Check configured value
                //tokens = line.Split(' ');

                //// if the line is RTR do not check the configuration
                //if (tokens[0] == "RTR")
                //{
                //    _cSwitch.Flush();
                //    continue;
                //}

                //_cSwitch.Flush();
                //_cSwitch.SendData(tokens[0]);
                //Thread.Sleep(200);
                //_cSwitch.ReadData(out rcv, tokens[0]);
                //if (!isMFU)
                //{
                //    rcv = rcv.Replace("0" + (70 + val).ToString(), (70 + val).ToString());
                //    if (tokens.Length > 3)
                //        line = tokens[0] + " " + tokens[1] + tokens[2] + tokens[3];
                //}
                //if (rcv != line)
                //{
                //    resVal = -1;
                //    Console.WriteLine("{0}: configured value - {1}  received value - {2}", tokens[0], line, rcv);
                //}
                //else
                //{
                //    Console.WriteLine("value is properly configured.\n");

                //}
                _cSwitch.SendData(line);
                _cSwitch.ReadData(out rcv);
                if (line == "exit")
                {
                    Thread.Sleep(200);
                    _cSwitch.SendData("\r\n");
                    //_cSwitch.Flush();
                }
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
            _cSwitch.SendData("\n");
            Thread.Sleep(200);
            _cSwitch.ReadData(out rcv);
            if (rcv != "Username: ")
                return -1;
            _cSwitch.SendData(_userName);
            _cSwitch.Flush();
            _cSwitch.SendData("\n");
            _cSwitch.ReadData(out rcv);
            if (rcv != "Password: ")
                return -1;
            _cSwitch.SendData(_password);
            _cSwitch.Flush();
            _cSwitch.SendData("\n");
            _cSwitch.ReadData(out rcv);
            if (rcv != "Switch#")
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
                _cSwitch.ReadData(out rcv);

                if (rcv == "Copy Succeded")
                    isSaved = 0;

                attempts++;
            }

            return isSaved;
        }
    }
}
