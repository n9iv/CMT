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
            int resVal = (int)ErrorCodes.Success;

            if (_port == "")
            {
                _port = XMLparser.portName;

                if (_port == "")
                {
                    return (int)ErrorCodes.XMLFieldMissing;
                }
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
                Log.Write("T-Switch communication is oppened\n");
                return (int)ErrorCodes.Success;

            }
            catch (Exception ex)
            {
                if (ex is ArgumentException)
                {
                    Log.Write("The name does not begin with 'COM'");
                }

                if (ex is InvalidOperationException)
                {
                    Log.Write("The serial port is already oppened");
                }

                if (ex is IOException)
                {
                    Log.Write("Serial port Gets invalid parameters");
                }
                return (int)ErrorCodes.SPConnectionFailed;
            }

        }

        public void Close()
        {
            _spSwitch.Close();
        }

        public ErrorCodes ReadData(out String data, string token)
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
                    Log.Write(tmp + " - read");
            }
            catch (TimeoutException exp)
            {
                data = null;
                Log.Write("Read from serial is failed!");
                return ErrorCodes.ReadSerialFailed;
            }

            data = tmp;
            return ErrorCodes.Success;
        }

        public ErrorCodes SendData(string data, bool sendChsr = false)
        {
            char[] dataArray = data.ToCharArray();

            try
            {
                if (sendChsr)
                {
                    _spSwitch.Write(dataArray[0].ToString());
                    Log.Write(data + " - written");
                    return ErrorCodes.Success;
                }

                if (data != "\n")
                {
                    foreach (char ch in dataArray)
                    {
                        _spSwitch.Write(ch.ToString());
                        Thread.Sleep(100);
                    }
                }
                _spSwitch.Write("\n");
                if ((data != "\n") && (data != "\r\n"))
                    Log.Write(data + " - written");
            }
            catch (NullReferenceException ex)
            {
                Log.Write("Check the data to be send or serial connection.");
                return ErrorCodes.WritreSerialFailed;
            }
            return ErrorCodes.Success;
        }

        public void Flush()
        {
            _spSwitch.BaseStream.Flush();
            _spSwitch.DiscardOutBuffer();
            _spSwitch.DiscardInBuffer();
        }
    }
}
