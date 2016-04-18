using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Ports;

namespace ClockConfigurator
{
    class SerialConfiguration
    {
        private string _port;
        private SerialPort _spClock;
        private int _boudRate = 19200;

        public SerialConfiguration(string port)
        {
           _port = port.ToUpper();
        }

        public int init()
        {
            int resVal = 1;

            if (_port == "")
            {
                if (ParsePortFile() == -1)
                    return -1;
            }

            _spClock = new SerialPort();
            _spClock.BaudRate = _boudRate;
            _spClock.PortName = _port.ToUpper();
            _spClock.DataBits = 8;
            _spClock.StopBits = StopBits.One;

            return resVal;
        }

        public int Open()
        {
            try
            {
                _spClock.Open();
                Console.WriteLine("Clock communication is oppened\n");
                return 0;
            }
            catch (Exception ex)
            {
                if (ex is ArgumentException)
                {
                    Console.WriteLine("The name does not begin with 'COM'");
                }

                if (ex is InvalidOperationException)
                {
                    Console.WriteLine("The serial port is already oppened");
                }

                if (ex is IOException)
                {
                    Console.WriteLine("Serial port Gets invalid parameters");
                }

                return -1;
            }

        }

        public void Close()
        {
            _spClock.Close();
            Console.WriteLine("Clock communication is oppened\n");
        }

        private int ParsePortFile()
        {
            string port, tmp;
            int portNum;
            bool isNumeric;
            try
            {
                using (StreamReader fileRead = File.OpenText("SerialConfiguration.txt"))
                {
                    port = fileRead.ReadLine();
                    tmp = port.Substring(0, 3);

                    if ((tmp != "com") && (tmp != "COM") && (tmp != "Com"))
                    {
                        Console.WriteLine("Check syntex in file");
                        return -1;
                    }
                    tmp = port.Substring(3);
                    isNumeric = Int32.TryParse(tmp, out portNum);
                    if (!isNumeric)
                    {
                        Console.WriteLine("Syntex error, after the word 'com' comes numeric value");
                        return -1;
                    }
                }

                _port = port.ToUpper();
                return 1;
            }
            catch(FileNotFoundException)
            {
                Console.WriteLine("SerialConfiguration.txt does not exist");
                return -1;
            }
        }

        public void ReadData(out String data)
        {
            string tmp = null;
            try
            {
                tmp = _spClock.ReadLine();
                Console.WriteLine("{0} is read", tmp);
            }
            catch (TimeoutException exp)
            {
                Console.WriteLine("Read from serial is failed!");
            }
            data = tmp;
        }

        public void SendData(string data)
        {
            try
            {
                _spClock.WriteLine(data);
                Console.WriteLine("{0} is written", data);
            }
            catch (NullReferenceException exc)
            {
                Console.WriteLine("Check the data to be send or serial connection.");
            }
        }
    }
}
