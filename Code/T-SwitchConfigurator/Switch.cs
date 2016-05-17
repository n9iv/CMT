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
            int res = (int)ErrorCodes.Success;

            if ((res = base.Init()) != (int)ErrorCodes.Success)
                return res;
            while (true)
            {
                if ((res = base.LogIn("s")) != (int)ErrorCodes.Success)
                    break;
                if ((res = base.RunScript(_val, _type, false)) != (int)ErrorCodes.Success)
                    break;
                if ((res = base.SaveSettings()) != (int)ErrorCodes.Success)
                    break;
                break;
            }
            Tswitch.Close();
            return res;
        }

        public int RouterConfig()
        {
            int res = 0;

            return res;
        }
    }
}
