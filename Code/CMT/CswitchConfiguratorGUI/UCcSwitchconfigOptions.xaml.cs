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

namespace CMT.CswitchConfiguratorGUI
{
    /// <summary>
    /// Interaction logic for UCcSwitchconfigOptions.xaml
    /// </summary>
    public partial class UCcSwitchconfigOptions : UserControl
    {
        private int _val;

        public UCcSwitchconfigOptions()
        {
            InitializeComponent();

        }

        private void _tbBN_TextChanged(object sender, TextChangedEventArgs e)
        {
            _val = Int16.Parse(_tbBN.Text);
            if (MainWindow.val == null)
                MainWindow.val = new int[1];
            MainWindow.val[0] = _val;
        }

        private void _cmdUp_Click(object sender, RoutedEventArgs e)
        {
            int val = Int32.Parse(_tbBN.Text);
            if (val < 3)
                val += 1;
            _tbBN.Text = val.ToString();
        }

        private void _cmdDown_Click(object sender, RoutedEventArgs e)
        {
            int val = Int32.Parse(_tbBN.Text);
            if (val > 1)
                val -= 1;
            _tbBN.Text = val.ToString();
        }
    }
}
