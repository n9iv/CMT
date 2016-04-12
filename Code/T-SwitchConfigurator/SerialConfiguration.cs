using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Ports;

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
                if (ParsePortFile() == -1)
                    return -1;
            }

            _spSwitch = new SerialPort();
            _spSwitch.BaudRate = _boudRate;
            _spSwitch.PortName = _port.ToUpper();
            _spSwitch.DataBits = 8;
            _spSwitch.StopBits = StopBits.One;

            return resVal;
        }

        public void Open()
        {
            try
            {
                _spSwitch.Open();
   
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
            
            using (StreamReader fileRead = File.OpenText("SerialConfiguration.txt"))
            {
                port = fileRead.ReadLine();
                tmp = port.Substring(0,3);
                
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

        public void ReadData(out String data)
        {
            string tmp = null;
            try
            {
                tmp = _spSwitch.ReadLine();
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
            _spSwitch.WriteLine(data);
            Console.WriteLine("{0} is written", data);
        }
    }
}
