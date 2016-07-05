using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace C_SwitchConfigurator
{
    class XMLparser
    {
        public static string portName;
        public static string switchUserName;
        public static string switcPassword;
        private static string _xmlFileName = @"C-SwitchConfigurator\C-SwitchConfigurator.xml";
        private static XmlDocument _xmlDoc;

        static XMLparser()
        {
            _xmlDoc = new XmlDocument();
        }

        private static void ParseSerial()
        {
            XmlNodeList nodes = _xmlDoc.DocumentElement.SelectNodes("/Configurator/Serial");

            foreach (XmlNode node in nodes)
            {
                portName = node.SelectSingleNode("PortName").InnerText;
            }
            portName = portName.ToUpper();
        }

        private static void ParseSwitch()
        {
            XmlNodeList nodes = _xmlDoc.DocumentElement.SelectNodes("/Configurator/Switch");

            foreach (XmlNode node in nodes)
            {
                switchUserName = node.SelectSingleNode("Username").InnerText;
                switcPassword = node.SelectSingleNode("Password").InnerText;
            }
        }

        private static void ParseLog()
        {
            XmlNodeList nodes = _xmlDoc.DocumentElement.SelectNodes("/Configurator/Log");

            foreach (XmlNode node in nodes)
            {
                int num;

                int.TryParse(node.SelectSingleNode("NumLogsToSave").InnerText, out num);
                Log.noLogToSave = num;
            }
        }

        private static ErrorCodes CreateXML()
        {
            ErrorCodes ret = ErrorCodes.Success;
            try
            {
                _xmlDoc.Load(_xmlFileName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ErrorCodes.XMLFileMissing;
            }

            return ret;
        }

        public static ErrorCodes Parse()
        {
            if (CreateXML() != ErrorCodes.Success)
                return ErrorCodes.XMLFileMissing;
            ParseSerial();
            ParseSwitch();
            ParseLog();
            return ErrorCodes.Success;
        }
    }
}
