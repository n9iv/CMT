using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ClockConfigurator
{
    class XMLparser
    {
        public static string portName;
        public static string switchUserName;
        public static string switcPassword;
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

        private static void ParseSwitch()
        {
            XmlNodeList nodes = _xmlDoc.DocumentElement.SelectNodes("/Configurator/Switch");

            foreach (XmlNode node in nodes)
            {
                switchUserName = node.SelectSingleNode("Username").InnerText;
                switcPassword = node.SelectSingleNode("Password").InnerText;
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

        public static int Parse()
        {
            if (CreateXML() == -1)
                return -1;
            ParseSerial();
            ParseSwitch();
            return 0;
        }
    }
}
