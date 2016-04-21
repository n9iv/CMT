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
using CMT.ClockConfiguratorGUI;

namespace CMT
{
    /// <summary>
    /// Interaction logic for UCclockConfigurator.xaml
    /// </summary>
    public partial class UCclockConfigurator : UserControl
    {
        private Int16 _val;

        public UCclockConfigurator()
        {
            InitializeComponent();
        }

        private void _cmdUp_Click(object sender, RoutedEventArgs e)
        {
            int val = Int32.Parse(_tbBatVal.Text);
            if (val < 3)
                val += 1;
            _tbBatVal.Text = val.ToString();
        }

        private void _cmdDown_Click(object sender, RoutedEventArgs e)
        {
            int val = Int32.Parse(_tbBatVal.Text);
            if (val > 1)
                val -= 1;
            _tbBatVal.Text = val.ToString();
        }

        private void _rbCCU_Checked(object sender, RoutedEventArgs e)
        {
            _spBtn.IsEnabled = true;
            _tbBatVal.IsEnabled = true;
            _val = Int16.Parse(_tbBatVal.Text);
            MainWindow.val[0] = _val;
            EnableNext();
        }

        private void _rbMFU_Checked(object sender, RoutedEventArgs e)
        {
            if ((_tbBatVal != null) && (_spBtn != null))
            {
                _tbBatVal.IsEnabled = false;
                _spBtn.IsEnabled = false;
            }
            _val = 0;
            MainWindow.val[0] = _val;
            EnableNext();
        }

        private void EnableNext()
        {
            Window win = Application.Current.Windows[0];
            MainWindow main = (MainWindow)win;
            main._btnNext.IsEnabled = true;
        }

        private void _tbBatVal_TextChanged(object sender, TextChangedEventArgs e)
        {
            _val = Int16.Parse(_tbBatVal.Text);
            MainWindow.val[0] = _val;
        }
    }
}