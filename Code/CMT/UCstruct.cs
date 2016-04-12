using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
namespace CMT
{
    class UCstruct
    {
        public static UserControl usNext;
        public delegate void _func();
        public static _func func;
        public static bool isNxtEnabled = true;
    }
}
