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
using System.Diagnostics;
using System.Threading;

namespace CMT.CswitchConfiguratorGUI
{
    /// <summary>
    /// Interaction logic for UCconfigCSwitch.xaml
    /// </summary>
    public partial class UCconfigCSwitch : UserControl
    {
        private Process _cSwitchProc;
        private Thread _ConfThread;
        private int _val;

        public UCconfigCSwitch(int val)
        {
            InitializeComponent();
            _val = val;
            UCstruct.isNxtEnabled = false;
        }

        private void _btnConfig_Click(object sender, RoutedEventArgs e)
        {
            ThreadStart start = new ThreadStart(WaitConfigurator);
            _ConfThread = new Thread(start);
            _cSwitchProc = new Process();

            _cSwitchProc.StartInfo.FileName = "C-SwitchConfigurator.exe";
            _cSwitchProc.StartInfo.Arguments = _val.ToString() + " Scripts\\C-SwitchScript.txt";
            _cSwitchProc.StartInfo.RedirectStandardInput = true;
            _cSwitchProc.StartInfo.RedirectStandardOutput = true;
            _cSwitchProc.StartInfo.RedirectStandardError = true;
            _cSwitchProc.StartInfo.UseShellExecute = false;
            _cSwitchProc.StartInfo.CreateNoWindow = true;
            _cSwitchProc.Start();
            _ConfThread.Start();

            _btnConfig.IsEnabled = false;
            _tbPb.Visibility = System.Windows.Visibility.Visible;
            _pb.Visibility = System.Windows.Visibility.Visible;
            _pb.IsIndeterminate = true;
        }

        /// <summary>
        /// Thread that is waiting for configurator result.
        /// </summary>
        private void WaitConfigurator()
        {
            int res = -1;
            string str;
            Brush b = Brushes.Red;

            _cSwitchProc.WaitForExit();
            res = _cSwitchProc.ExitCode;
            _cSwitchProc.Close();

            if (res != -1)
            {
                b = Brushes.Green;
                str = "Configuration succeeded!";
            }

            else
            {
                str = "Configuration failed!";
                b = Brushes.Red;
            }
            this.Dispatcher.Invoke((Action)(() =>
            {
                _tbConf.Foreground = b;
                _tbConf.Text = str;
                _btnConfig.IsEnabled = true;
                _pb.IsIndeterminate = false;
                _tbPb.Visibility = System.Windows.Visibility.Hidden;
                _pb.Visibility = System.Windows.Visibility.Hidden;
            }));
        }
    }
}
