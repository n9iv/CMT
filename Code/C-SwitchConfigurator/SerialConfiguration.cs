using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace C_SwitchConfigurator
{
    class SerialConfiguration
    {
        private string _port;
        private SerialPort _spCswitch;
        private int _boudRate = 9600;

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

            _spCswitch = new SerialPort();
            _spCswitch.BaudRate = _boudRate;
            _spCswitch.PortName = _port.ToUpper();
            _spCswitch.DataBits = 8;
            _spCswitch.StopBits = StopBits.One;

            return resVal;
        }

        public int Open()
        {
            try
            {
                _spCswitch.Open();
                Console.WriteLine("C-Switch communication is oppened\n");
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
            _spCswitch.Dispose();
            _spCswitch.Close();
            Console.WriteLine("C-Switch communication is closed\n");
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
                tmp = _spCswitch.ReadExisting();
                //tmp = tmp.Replace(token + "\n\r\n", "");
                tmp = tmp.Replace("\r", "");
                tmp = tmp.Replace("\n", "");
                if(token.Length > 0)
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
                        _spCswitch.Write(ch.ToString());
                        Thread.Sleep(100);
                    }
                }
                _spCswitch.Write("\n");
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
            _spCswitch.BaseStream.Flush();
            _spCswitch.DiscardOutBuffer();
            _spCswitch.DiscardInBuffer();
        }
    }
}
