using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T_SwitchConfigurator
{
    class Switch : Configure
    {
        private int _MN, _BN;

        public Switch(string port,int MN, int BN, string path)
            : base(port, path)
        {
            _MN = MN;
            _BN = BN;
        }

        public int SwitchConfig()
        {
            int res = 0;

            if (base.LogIn() == false)
                return -1;
            if (base.RunScript(_MN, _BN) < 0)
                return -1;
            if (base.SaveSettings() == false)
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
