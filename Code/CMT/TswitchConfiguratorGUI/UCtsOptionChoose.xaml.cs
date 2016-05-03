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

        public UCtsOptionChoose()
        {
            MainWindow.val = new int[2];
            InitializeComponent();
            _MN = Int32.Parse(_tbMN.Text);
            _BN = Int32.Parse(_tbBN.Text);
        }

        private void _cmdUpMN_Click(object sender, RoutedEventArgs e)
        {
            int val = Int32.Parse(_tbMN.Text);
            if (val <= 2)
                val += 1;
            _tbMN.Text = val.ToString();
        }

        private void _cmdDownMN_Click(object sender, RoutedEventArgs e)
        {
            int val = Int32.Parse(_tbMN.Text);
            if (val > 0)
                val -= 1;
            _tbMN.Text = val.ToString();
        }

        private void _cmdUpBN_Click(object sender, RoutedEventArgs e)
        {
            int val = Int32.Parse(_tbBN.Text);
            if (val <= 2)
                val += 1;
            _tbBN.Text = val.ToString();
        }

        private void _cmdDownBN_Click(object sender, RoutedEventArgs e)
        {
            int val = Int32.Parse(_tbBN.Text);
            if (val > 0)
                val -= 1;
            _tbBN.Text = val.ToString();
        }

        private void _rbaaa_Checked(object sender, RoutedEventArgs e)
        {
            if ((_spBtnBN != null) && (_spBtnMN != null) && (_tbBN != null) && (_tbMN != null))
            {
                _spBtnBN.IsEnabled = true;
                _spBtnMN.IsEnabled = true;
                _tbMN.IsEnabled = true;
                _tbBN.IsEnabled = true;
                EnableNext();
            }
        }

        private void EnableNext()
        {
            Window win = Application.Current.Windows[0];
            MainWindow main = (MainWindow)win;
            main._btnNext.IsEnabled = true;
        }

        public void Next()
        {
            UCstruct.usNext = new UCconfigSwitch(_MN, _BN);
            UCstruct.isNxtEnabled = true;
        }

        private void _tbMN_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if ((int.Parse(_tbMN.Text) < 1) || (int.Parse(_tbMN.Text) > 3))
                {
                    MessageBox.Show("Invalid value. The MN value should be between 1-3", "Invalid Argument", MessageBoxButton.OK);
                    _tbMN.Text = "1";
                    return;
                }
            }
            catch (FormatException ex)
            {
                MessageBox.Show("The value is not in correct format", "Invalid Argument", MessageBoxButton.OK);
                _tbMN.Text = "1";
                return;
            }
            _MN = int.Parse(_tbMN.Text);
            MainWindow.val[0] = _MN;
        }

        private void _tbBN_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if ((Int16.Parse(_tbBN.Text) < 1) || (Int16.Parse(_tbBN.Text) > 3))
                {
                    MessageBox.Show("Invalid value. The BN value should be between 1-3", "Invalid Argument", MessageBoxButton.OK);
                    _tbBN.Text = "1";
                    return;
                }
            }
            catch (FormatException ex)
            {
                MessageBox.Show("The value is not in correct format", "Invalid Argument", MessageBoxButton.OK);
                _tbBN.Text = "1";
                return;
            }
            _BN = int.Parse(_tbBN.Text);
            MainWindow.val[1] = _BN;
        }

    }
}
