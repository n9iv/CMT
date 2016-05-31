using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T_SwitchConfigurator
{
    class Switch : Configure
    {
        public static string routeMN = "MFU_T_Rauter.txt";
        public static string routeBN = "CCU_T_Rauter.txt";
        public static string switchMN = "MFU_T_Switch.txt";
        public static string switchBN = "CCU_T_Switch.txt";
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

        public int SwitchConfig(bool reset)
        {
            int res = (int)ErrorCodes.Success;

            if ((res = base.Init()) != (int)ErrorCodes.Success)
                return res;

            while (true)
            {
                if ((res = base.LogInSwitch("s")) != (int)ErrorCodes.Success)
                    break;
                if (reset)
                {
                    res = base.ResetSwitch();
                    break;
                }
                if ((res = base.RunScriptSwitch(_val, _type, false)) != (int)ErrorCodes.Success)
                    break;
                if ((res = base.SaveSettings(false)) != (int)ErrorCodes.Success)
                    break;
                break;
            }
            Tswitch.Close();
            return res;
        }

        public int RouterConfig(bool reset)
        {
            int res = 0;
            if ((res = base.Init()) != (int)ErrorCodes.Success)
                return res;
            if (reset)
            {
                res = base.ResetRouter();
            }
            else
            {
                while (true)
                {
                    if ((res = base.LogInRouter("r")) != (int)ErrorCodes.Success)
                        break;
                    if ((res = base.RunScriptRouter(_val, _type, true)) != (int)ErrorCodes.Success)
                        break;
                    if ((res = base.SaveSettings(true)) != (int)ErrorCodes.Success)
                        break;
                    break;
                }
            }
            return res;
        }
    }
}
