using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Ports;
using System.Threading;

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
                _port = XMLparser.portName;
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
                this.Flush();
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
            _spClock.Dispose();
            _spClock.Close();
            Console.WriteLine("Clock communication is closed\n");
        }

        public void ReadData(out String data, string token)
        {
            string tmp = null;
            try
            {
                tmp = _spClock.ReadExisting();
                tmp = tmp.Replace(token + "\n\r\n", "");
                tmp = tmp.Replace("\r", "");
                tmp = tmp.Replace("\n", "");
                tmp = tmp.Replace("> ", "");
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
            char[] dataArray = data.ToCharArray();
            try
            {
                foreach(char ch in dataArray)
                {
                    _spClock.Write(ch.ToString());
                    Thread.Sleep(100);
                }
                _spClock.Write("\n");
                Console.WriteLine("{0} is written", data);
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine("Check the data to be send or serial connection.");
            }
        }

        public void Flush()
        {
            _spClock.BaseStream.Flush();
            _spClock.DiscardOutBuffer();
            _spClock.DiscardInBuffer();
        }
    }
}
