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
        private int _boudRate = 19200; //Default boud rate

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
            int resVal = 0;

            if (_port == "")
            {
                _port = XMLparser.portName;
                if (_port == "")
                    return (int)Configure.ErrorCodes.XMLPortNameMissing;
            }

            _spClock = new SerialPort();
            _spClock.BaudRate = _boudRate;
            _spClock.PortName = _port.ToUpper();
            _spClock.DataBits = 8;
            _spClock.StopBits = StopBits.One;

            return resVal;
        }

        /// <summary>
        /// Open serial communication
        /// </summary>
        /// <returns></returns>
        public Configure.ErrorCodes Open()
        {
            try
            {
                _spClock.Open();
                Log.Write("Clock communication is opened\n");
                this.Flush();
                return 0;
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

                return Configure.ErrorCodes.SPConnectionFailed;
            }

        }

        public void Close()
        {
            _spClock.Dispose();
            _spClock.Close();
            Log.Write("Clock communication is closed\n");
        }

        /// <summary>
        /// Reads line from serial.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Configure.ErrorCodes ReadData(out String data, string token)
        {
            string tmp = null;
            Configure.ErrorCodes res = Configure.ErrorCodes.Success;
            try
            {
                tmp = _spClock.ReadExisting();
                tmp = tmp.Replace(token + "\n\r\n", "");
                tmp = tmp.Replace("\r", "");
                tmp = tmp.Replace("\n", "");
                tmp = tmp.Replace("> ", "");
                Log.Write(tmp + " - read");

            }
            catch (TimeoutException exp)
            {
                Log.Write("Read from serial is failed!");
                data = null;
                return Configure.ErrorCodes.ReadSerialFailed;
            }
            data = tmp;
            return Configure.ErrorCodes.Success;
        }

        /// <summary>
        /// Writes to serial.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public Configure.ErrorCodes SendData(string data)
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
                Log.Write( data + " - written");
                return Configure.ErrorCodes.Success;
            }
            catch (NullReferenceException ex)
            {
                Log.Write("Check the data to be send or serial connection.");
                return Configure.ErrorCodes.WritreSerialFailed;
            }
        }

        /// <summary>
        /// Erases the serial buffer.
        /// </summary>
        public void Flush()
        {
            _spClock.BaseStream.Flush();
            _spClock.DiscardOutBuffer();
            _spClock.DiscardInBuffer();
        }
    }
}
