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

        /// <summary>
        /// Sets all the attributes for serial communication
        /// </summary>
        /// <returns></returns>
        public int init()
        {
            int resVal = (int)ErrorCodes.Success;

            if (_port == "")
            {
                _port = XMLparser.portName;

                if (_port == "")
                    return (int)ErrorCodes.XMLFieldMissing;
            }

            _spCswitch = new SerialPort();
            _spCswitch.BaudRate = _boudRate;
            _spCswitch.PortName = _port.ToUpper();
            _spCswitch.DataBits = 8;
            _spCswitch.StopBits = StopBits.One;

            return resVal;
        }

        /// <summary>
        /// Open serial communication
        /// </summary>
        /// <returns></returns>
        public int Open()
        {
            try
            {
                _spCswitch.Open();
                Log.Write("C-Switch communication is opened\n");
                this.Flush();
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
                    Log.Write("The serial port is already opened");
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
            _spCswitch.Dispose();
            _spCswitch.Close();
            Log.Write("C-Switch communication is closed\n");
        }

        /// <summary>
        /// Reads line from serial.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public ErrorCodes ReadData(out String data, string token)
        {
            string tmp = null;
            try
            {
                tmp = _spCswitch.ReadExisting();
                //tmp = tmp.Replace(token + "\n\r\n", "");
                tmp = tmp.Replace("\r", "");
                tmp = tmp.Replace("\n", "");
                if (token.Length > 0)
                    tmp = tmp.Replace(token, "");
                if ((tmp != "\n") && (tmp != "\r\n") && (tmp != ""))
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

        /// <summary>
        /// Writes to serial.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public ErrorCodes SendData(string data)
        {
            char[] dataArray = data.ToCharArray();
            try
            {
                if (data != "\n")
                {
                    foreach (char ch in dataArray)
                    {
                        _spCswitch.Write(ch.ToString());
                        Thread.Sleep(20);
                    }
                }
                _spCswitch.Write("\n");
                if ((data != "\n") && (data != "\r\n") && (data != ""))
                    Log.Write(data + " - written");
            }
            catch (NullReferenceException ex)
            {
                Log.Write("Check the data to be send or serial connection.");
                return ErrorCodes.WritreSerialFailed;
            }
            return ErrorCodes.Success;
        }

        /// <summary>
        /// Erases the serial buffer.
        /// </summary>
        public void Flush()
        {
            _spCswitch.BaseStream.Flush();
            _spCswitch.DiscardOutBuffer();
            _spCswitch.DiscardInBuffer();
        }
    }
}
