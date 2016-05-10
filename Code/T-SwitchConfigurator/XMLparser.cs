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
        private static string _xmlFileName = "Configurator.xml";
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

        private static void ParseInfo(string type)
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

        private static int CreateXML()
        {
            try
            {
                _xmlDoc.Load(_xmlFileName);
            }
            catch (System.IO.FileNotFoundException)
            {
                Console.WriteLine("Configurator.xml file does not exist");
                return -1;
            }

            return 0;
        }

        public static int Parse(string type)
        {
            if (CreateXML() == -1)
                return -1;
            ParseSerial();
            ParseInfo(type);
            return 0;
        }
    }
}
