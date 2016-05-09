using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CMT.TswitchConfiguratorGUI
{
    /// <summary>
    /// Interaction logic for UCtsOptionChoose.xaml
    /// </summary>
    public partial class UCtsOptionChoose : UserControl
    {
        private int _MN, _BN;
        private string[] _MNarray = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "101", "102", "103", "104", "105", "106", "107", "108", "109" };
        private string[] _BNarray = { "1", "2", "3" };

        public UCtsOptionChoose()
        {
            MainWindow.val = new int[2];
            InitializeComponent();
            _cbMN.ItemsSource = _MNarray;
            _cbBN.ItemsSource = _BNarray;
        }

        private void _rbaaa_Checked(object sender, RoutedEventArgs e)
        {
            if ((_cbMN != null) && (_cbBN != null))
            {
                _cbMN.IsEnabled = true;
                _cbBN.IsEnabled = false;
                MainWindow.val[1] = 0;
            }

            if (_cbMN.SelectedItem == null)
            {
                EnableNext(false);
            }
            else
            {
                EnableNext(true);
                int.TryParse((string)_cbMN.SelectedItem, out MainWindow.val[0]);
            }
        }

        private void _rbbbbb_Checked(object sender, RoutedEventArgs e)
        {
            if ((_cbMN != null) && (_cbBN != null))
            {
                _cbMN.IsEnabled = false;
                _cbBN.IsEnabled = true;
                MainWindow.val[0] = 0;
            }

            if (_cbBN.SelectedItem == null)
            {
                EnableNext(false);
            }
            else
            {
                EnableNext(true);
                int.TryParse((string)_cbBN.SelectedItem, out MainWindow.val[1]);
            }
        }

        private void EnableNext(bool enable)
        {
            Window win = Application.Current.Windows[0];
            MainWindow main = (MainWindow)win;
            main._btnNext.IsEnabled = enable;
        }

        public void Next()
        {
            UCstruct.usNext = new UCconfigSwitch(_MN, _BN);
            UCstruct.isNxtEnabled = true;
        }

        private void _cbMN_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_cbMN.SelectedItem != null)
            {
                string s = (string)_cbMN.SelectedItem.ToString();
                int.TryParse(s, out _MN);
                MainWindow.val[0] = _MN;
                EnableNext(true);
            }
        }

        private void _cbBN_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_cbBN.SelectedItem != null)
            {
                string s = (string)_cbBN.SelectedItem.ToString();
                int.TryParse(s, out _BN);
                MainWindow.val[1] = _BN;
                EnableNext(true);
            }
        }
    }
}
