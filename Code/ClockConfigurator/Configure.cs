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
            StreamReader script = File.OpenText(_path);
            string line, rcv = null;
            string[] tokens;
            if (_clock.Open() == -1)
                return -1;

            while((line = script.ReadLine()) != null)
            {
                //configure
                line = line.Replace("{0}",val.ToString());
                _clock.SendData(line);
                Thread.Sleep(200);

                //Check configured value
                tokens = line.Split(' ');
                _clock.SendData(tokens[0]);
                _clock.ReadData(out rcv);
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
            _clock.Close();
            return resVal;
        }
    }
}
