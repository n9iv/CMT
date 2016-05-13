using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Ports;
using System.Threading;

namespace T_SwitchConfigurator
{
    class SerialConfiguration
    {
        private string _port;
        private SerialPort _spSwitch;
        private int _boudRate = 115200;

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

            _spSwitch = new SerialPort();
            _spSwitch.BaudRate = _boudRate;
            _spSwitch.PortName = _port.ToUpper();
            _spSwitch.DataBits = 8;
            _spSwitch.StopBits = StopBits.One;

            return resVal;
        }

        public int Open()
        {
            try
            {
                _spSwitch.Open();
                Console.WriteLine("T-Switch communication is oppened\n");
                return 0;
   
            }
            catch(Exception ex)
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
            _spSwitch.Close();
        }

        public void ReadData(out String data, string token)
        {
            string tmp = null;
            try
            {
                tmp = _spSwitch.ReadExisting();
                //tmp = tmp.Replace(token + "\n\r\n", "");
                tmp = tmp.Replace("\r", "");
                tmp = tmp.Replace("\n", "");
                if (token.Length > 0)
                    tmp = tmp.Replace(token, "");
                if ((tmp != "") && (tmp != "\n"))
                    Console.WriteLine(tmp + " - read");
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
                if (data != "\n")
                {
                    foreach (char ch in dataArray)
                    {
                        _spSwitch.Write(ch.ToString());
                        Thread.Sleep(100);
                    }
                }
                _spSwitch.Write("\n");
                if (data != "\n")
                    Console.WriteLine(data + " - written");
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine("Check the data to be send or serial connection.");
            }
        }

        public void Flush()
        {
            _spSwitch.BaseStream.Flush();
            _spSwitch.DiscardOutBuffer();
            _spSwitch.DiscardInBuffer();
        }
    }
}
