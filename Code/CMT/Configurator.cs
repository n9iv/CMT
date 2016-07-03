using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Diagnostics;
using System.Windows;

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
    }

    /// <summary>
    /// Parse ErrorMessages XML file and returns the corresponding error message according to error code
    /// </summary>
    class Configurator
    {
        public static Process ConfiguratorProc;
        public static bool Terminated = false;
        private static XmlDocument _doc;
        private static Dictionary<ErrorCodes, string> _errorMessages;

        static Configurator()
        {
            _doc = new XmlDocument();
            _errorMessages = new Dictionary<ErrorCodes, string>();
        }

        public static string GetErrorMsg(ErrorCodes errorCode)
        {
            if (Enum.IsDefined(typeof(ErrorCodes), errorCode))
                return _errorMessages[errorCode];
            return _errorMessages[ErrorCodes.Failed];
        }

        public static void Init()
        {
            string filePath = "ErrorMessages.xml";

            _doc.Load(filePath);
            ParseErrorMessageFile();
        }

        private static void ParseErrorMessageFile()
        {
            XmlNode errorMessages = _doc.DocumentElement.SelectSingleNode("/ErrorMessages");
            string message = null, code = null;
            ErrorCodes erroCode = ErrorCodes.Failed;

            foreach (XmlNode errorMessage in errorMessages.ChildNodes)
            {
                message = errorMessage.InnerText;
                code = errorMessage.Attributes["Code"].Value;
                erroCode = (ErrorCodes)Enum.Parse(typeof(ErrorCodes), code);

                if (!Enum.IsDefined(typeof(ErrorCodes), erroCode))
                    throw new Exception(string.Format("Code of the message \"{0}\" is not corresponding to configurator Error Code.", message));

                _errorMessages.Add(erroCode, message);
            }
        }

        public static void TerminateRunningConfigurator()
        {
            string msg = "This will terminate the configuration process.\nWould you like to continue?";

            if (ConfiguratorProc == null) 
                return;
            try
            {
                Process[] temp = Process.GetProcessesByName(ConfiguratorProc.ProcessName);
                if (temp.Length > 0)
                {
                    var res = MessageBox.Show(msg, "Configurator Termination Warning", MessageBoxButton.YesNo);
                    if (res == MessageBoxResult.Yes)
                    {
                        Terminated = true;
                        ConfiguratorProc.Kill();
                    }
                }
            }
            catch
            {
                return;
            }

        }
    }
}

