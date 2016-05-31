using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using CMT.BN_App;
using CMT.CswitchConfiguratorGUI;
using CMT.ClockConfiguratorGUI;
using CMT.TswitchConfiguratorGUI;
using System.Windows;

namespace CMT
{
    class Navigate
    {
        private static List<UCstruct> _typeList;

        static Navigate()
        {
            _typeList = new List<UCstruct>();
        }

        public static void SetClockList()
        {
            _typeList.Clear();
            _typeList.Add(new UCstruct(typeof(UCclockConfigurator), false));
            _typeList.Add(new UCstruct(typeof(FinalWin), false));

            Main._typeList = _typeList;
        }

        public static void SetTswitchList()
        {
            _typeList.Clear();
            _typeList.Add(new UCstruct(typeof(UCtsOptionChoose), false));
            _typeList.Add(new UCstruct(typeof(USswitchInstruction), true));
            _typeList.Add(new UCstruct(typeof(UCconfigSwitch), true));
            _typeList.Add(new UCstruct(typeof(UCrouterInstruction), true));
            _typeList.Add(new UCstruct(typeof(UCconfigRouter), false));

            Main._typeList = _typeList;
        }

        public static void SetCswitchList()
        {
            _typeList.Clear();
            _typeList.Add(new UCstruct(typeof(UCcSwitchconfigOptions), false));
            _typeList.Add(new UCstruct(typeof(UCcSwitchInstruction), true));
            _typeList.Add(new UCstruct(typeof(UCconfigCSwitch), false));

            Main._typeList = _typeList;
        }

        public static void SetBNapp()
        {
            _typeList.Clear();
            _typeList.Add(new UCstruct(typeof(UCrunBNapp), false));

            Main._typeList = _typeList;
        }

        public static void SetNextEnable(UserControl uc, bool enable)
        {
            Main._typeList.FirstOrDefault(x => x.userControl == uc.GetType()).isEnabled = enable;
            EnableNext(enable);
        }

        private static void EnableNext(bool enable)
        {
            Window win = Application.Current.Windows[0];
            MainWindow main = (MainWindow)win;
            main._btnNext.IsEnabled = enable;
        }
    }
}
