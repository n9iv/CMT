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
        private int _val;
        private string[] _batteryValues = { "1", "2", "3" };

        public UCclockConfigurator()
        {
            InitializeComponent();
            _cbBatVal.ItemsSource = _batteryValues;
        }

        private void _rbCCU_Checked(object sender, RoutedEventArgs e)
        {
            _sp.Visibility = System.Windows.Visibility.Visible;
            if(_cbBatVal.SelectedItem == null)
            {
                Navigate.SetNextEnable(this, false);
            }
            else
            {
                string selectedVal = (string)_cbBatVal.SelectedItem.ToString();
                int.TryParse(selectedVal, out _val);
                MainWindow.val[0] = _val;
            }
        }

        private void _rbMFU_Checked(object sender, RoutedEventArgs e)
        {
            _sp.Visibility = System.Windows.Visibility.Hidden;
            _val = 0;
            MainWindow.val[0] = _val;
            Navigate.SetNextEnable(this, true);
        }

        private void _cbBatVal_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedVal = (string)_cbBatVal.SelectedItem.ToString();
            int.TryParse(selectedVal, out _val);
            MainWindow.val[0] = _val;
            Navigate.SetNextEnable(this, true);
        }
    }
}