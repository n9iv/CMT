using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace T_SwitchConfigurator
{
    class XMLparser
    {
        public static string portName;
        public static string switchUserName;
        public static string switchPassword;
        private static string _xmlFileName = @"T-SwitchConfigurator\T-SwitchConfigurator.xml";
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

        public static void ParseInfo(string type)
        {
            string str = "Switch";

            if (type == "r")
                str = "Router";
            XmlNodeList nodes = _xmlDoc.DocumentElement.SelectNodes("/Configurator/" + str);

            foreach (XmlNode node in nodes)
            {
                switchUserName = node.SelectSingleNode("Username").InnerText;
                switchPassword = node.SelectSingleNode("Password").InnerText;
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

        private static int CreateXML()
        {
            try
            {
                _xmlDoc.Load(_xmlFileName);
            }
            catch (Exception ex)
            {
               Log.Write(ex.Message);
                return (int)ErrorCodes.XMLFileMissing;
            }

            return (int)ErrorCodes.Success;
        }

        public static int Parse(string type)
        {
            if (CreateXML() != (int)ErrorCodes.Success)
                return (int)ErrorCodes.XMLFileMissing;
            ParseSerial();
            ParseInfo(type);
            ParseLog();
            return (int)ErrorCodes.Success;
        }
    }
}
