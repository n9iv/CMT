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
            catch (FileNotFoundException)
            {
                Console.WriteLine("SerialConfiguration.txt does not exist");
                return -1;
            }
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
                Console.WriteLine(tmp);
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
                    Console.WriteLine(data);
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
