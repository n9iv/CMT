using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace ClockConfigurator
{
    class Configure
    {
        private string _path;
        private SerialConfiguration _clock;

        public Configure(string port, string path)
        {
            _path = path;
            _clock = new SerialConfiguration(port);
        }

        public int Init()
        {
            return _clock.init();
        }

        public int RunScript(int val)
        {
            int resVal = 0;
            bool isMFU = true, skip = false;
            StreamReader script = File.OpenText(_path);
            string line, rcv = null;
            string[] tokens;
            if (_clock.Open() == -1)
                return -1;
            if (val > 0)
            {
                isMFU = false;
            }
            while ((line = script.ReadLine()) != null)
            {
                //configure

                if (isMFU)
                {
                    if (line == "[MFU]")
                        continue;
                    if (line == "[else]")
                        skip = true;
                }
                else
                {
                    if (line == "[MFU]")
                        skip = true;
                    if (line == "[else]")
                    {
                        skip = false;
                        continue;
                    }

                }
                if (line == "[end]")
                {
                    skip = false;
                    continue;
                }

                if (skip)
                    continue;

                if (!isMFU)
                {
                    line = line.Replace("{0}", (70 + val).ToString());
                }
                else
                {
                    line = line.Replace("{0}", val.ToString());
                }
                _clock.SendData(line);
                // _clock.Flush();

                //Check configured value
                tokens = line.Split(' ');

                // if the line is RTR do not check the configuration
                if (tokens[0] == "RTR")
                {
                    _clock.Flush();
                    continue;
                }

                _clock.Flush();
                _clock.SendData(tokens[0]);
                Thread.Sleep(200);
                _clock.ReadData(out rcv, tokens[0]);
                if (!isMFU)
                {
                    rcv = rcv.Replace("0" + (70 + val).ToString(), (70 + val).ToString());
                    if (tokens.Length > 3)
                        line = tokens[0] + " " + tokens[1] + tokens[2] + tokens[3];
                }
                if (rcv != line)
                {
                    resVal = -1;
                    Console.WriteLine("{0}: configured value - {1}  received value - {2}", tokens[0], line, rcv);
                }
                else
                {
                    Console.WriteLine("value is properly configured.\n");

                }
            }
            script.Close();
            _clock.Close();
            return resVal;
        }
    }
}
