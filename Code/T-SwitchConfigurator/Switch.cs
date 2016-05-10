using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T_SwitchConfigurator
{
    class Switch : Configure
    {
        public static string routeMN = "T-SwitchRouterMNScript.txt";
        public static string routeBN = "T-SwitchRouterBNScript.txt";
        public static string switchMN = "T-SwitchSwitchMNScript.txt";
        public static string switchBN = "T-SwitchSwitchBNScript.txt";
        private int _val;
        private string _type;

        public Switch(string port, int MN, int BN, string path)
            : base(port, path)
        {
            if (MN != 0)
            {
                _type = "MN";
                _val = MN;
                if (MN > 100)
                {
                    _val = 63 - (MN % 100);
                }
            }
            if (BN != 0)
            {
                _val = 70 + BN;
                _type = "BN";

            }
        }

        public int SwitchConfig()
        {
            int res = 0;

            if (base.Init() == -1)
                return -1;
            if (base.LogIn("s") == -1)
                return -1;
            if (base.RunScript(_val, _type, false) < 0)
                return -1;
            if (base.SaveSettings() == -1)
                return -1;

            return res;
        }

        public int RouterConfig()
        {
            int res = 0;

            return res;
        }
    }
}
