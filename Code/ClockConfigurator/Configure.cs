using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace ClockConfigurator
{
    /// <summary>
    /// Consists of functions to configure the unit
    /// </summary>
    class Configure
    {
        /// <summary>
        /// Enum for codes the program returns.
        /// </summary>
        public enum ErrorCodes
        {
            Success = 0,
            Failed = -1,
            SPConnectionFailed = -2,
            ReadSerialFailed = -3,
            WritreSerialFailed = -4,
            ConfigurationFailed = -5,
            XMLPortNameMissing = -6,
            XMLFileMissing = -7
        };

        private string _path;
        private string _scriptPath = @"\ClockScript.txt";
        private SerialConfiguration _clock;

        public Configure(string port, string path)
        {
            _path = path + _scriptPath;
            _clock = new SerialConfiguration(port);
        }

        public int Init()
        {
            return _clock.init();
        }

        /// <summary>
        /// Reads the script file line by line and sends each line to clock via serial.
        /// After each sending, check if it was properly configured.
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public int RunScript(int val)
        {
            int resVal = 0;
            StreamReader script = null;
            bool isMFU = true, skip = false;

            try
            {
                script = File.OpenText(_path);
            }
            catch (Exception ex)
            {
                Log.Write(ex.Message);
                return (int)ErrorCodes.Failed;
            }
            Configure.ErrorCodes ret;
            string line, rcv = null;
            string[] tokens;
            if ((ret = _clock.Open()) != Configure.ErrorCodes.Success)
                return (int)ret;
            if (val > 0)
            {
                isMFU = false;
            }
            while ((line = script.ReadLine()) != null)
            {
                //configure

                //if the position of the clock is MFU, read the corresponding line in the script
                //In the script, read the line goes after the line "[MFU]"
                if (isMFU)
                {
                    if (line == "[MFU]")
                        continue;
                    if (line == "[else]")
                        skip = true;
                }

                //otherwise
                else
                {
                    if (line == "[MFU]")
                        skip = true;
                    if (line == "[else]")
                    {
                        skip = false;
                        continue;
                    }

                }
                if (line == "[end]")
                {
                    skip = false;
                    continue;
                }

                if (skip)
                    continue;

                //Replaces the Battery value where needed
                if (!isMFU)
                {
                    line = line.Replace("{0}", (70 + val).ToString());
                }
                else
                {
                    line = line.Replace("{0}", val.ToString());
                }
                if (_clock.SendData(line) != Configure.ErrorCodes.Success)
                {
                    resVal = (int)Configure.ErrorCodes.WritreSerialFailed;
                    break;
                }
                // _clock.Flush();

                //Check configured value
                tokens = line.Split(' ');

                // if the line is RTR do not check the configuration
                if (tokens[0] == "RTR")
                {
                    _clock.Flush();
                    continue;
                }

                _clock.Flush();
                if (_clock.SendData(tokens[0]) != Configure.ErrorCodes.Success)
                {
                    resVal = (int)Configure.ErrorCodes.WritreSerialFailed;
                    break;
                }
                Thread.Sleep(200);
                if (_clock.ReadData(out rcv, tokens[0]) != Configure.ErrorCodes.Success)
                {
                    resVal = (int)Configure.ErrorCodes.ReadSerialFailed;
                    break;
                }
                if (!isMFU)
                {
                 
                    rcv = rcv.Replace("0" + (70 + val).ToString(), (70 + val).ToString());
                    if (tokens.Length > 3)
                        line = tokens[0] + " " + tokens[1] + tokens[2] + tokens[3];
                }
                if (rcv != line)
                {
                    resVal = (int)Configure.ErrorCodes.ConfigurationFailed;
                    Log.Write(tokens[0] + ": configured value - " + line + "  received value - " + rcv);
                    break;
                }
                else
                {
                    Log.Write("value is properly configured.\n");

                }
            }
            script.Close();
            _clock.Close();
            return resVal;
        }
    }
}
