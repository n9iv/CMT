using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMT
{
    public enum ErrorCodes
    {
        Success = 0,
        Failed = -1,
        SPConnectionFailed = -2,
        ReadSerialFailed = -3,
        WritreSerialFailed = -4,
        ConfigurationFailed = -5,
        XMLFieldMissing = -6,
        XMLFileMissing = -7,
        LoginFailed = -8,
        SaveDataFailed = -9
    };

    class Configurator
    {
        public static string GetErrorMsg(ErrorCodes errorCode)
        {
            string str = null;

            switch (errorCode)
            {
                case ErrorCodes.Success:
                    str = "Configuration succeeded";
                    break;
                case ErrorCodes.ConfigurationFailed:
                case ErrorCodes.Failed:
                    str = "Configuration failed";
                    break;
                case ErrorCodes.LoginFailed:
                    str = "Login failed";
                    break;
                case ErrorCodes.ReadSerialFailed:
                    str = "Read data via serial failed";
                    break;
                case ErrorCodes.SaveDataFailed:
                    str = "Configuration failed - Save settings failed";
                    break;
                case ErrorCodes.SPConnectionFailed:
                    str = "Connection via serial port failed";
                    break;
                case ErrorCodes.WritreSerialFailed:
                    str = "Write data via serial failed";
                    break;
                case ErrorCodes.XMLFieldMissing:
                    str = "The port name field in XML file is empty";
                    break;
                case ErrorCodes.XMLFileMissing:
                    str = "XML file is missing";
                    break;
            }
            return str;
        }
    }
}
