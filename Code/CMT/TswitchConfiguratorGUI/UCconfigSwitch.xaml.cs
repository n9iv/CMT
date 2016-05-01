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
using System.IO;

namespace CMT.TswitchConfiguratorGUI
{
    /// <summary>
    /// Interaction logic for UCconfigSwitch.xaml
    /// </summary>
    public partial class UCconfigSwitch : UserControl
    {
        private int _MN;
        private int _BN;
        private Process _switchProc;
        private Thread _ConfThread;

        public UCconfigSwitch(int mn, int bn)
        {
            InitializeComponent();
            _MN = mn;
            _BN = bn;
        }

        private void _btnConfig_Click(object sender, RoutedEventArgs e)
        {
            ThreadStart start = new ThreadStart(WaitConfigurator);
            _ConfThread = new Thread(start);
            _switchProc = new Process();

            _switchProc.StartInfo.FileName = "T-SwitchConfigurator.exe";
            _switchProc.StartInfo.Arguments = "s " + _MN.ToString() + " " + _BN.ToString() + " Scripts\\T-SwitchSwitchScript.txt";
            _switchProc.StartInfo.RedirectStandardInput = true;
            _switchProc.StartInfo.RedirectStandardOutput = true;
            _switchProc.StartInfo.RedirectStandardError = true;
            _switchProc.StartInfo.UseShellExecute = false;
            _switchProc.StartInfo.CreateNoWindow = true;
            _switchProc.Start();
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

            _switchProc.WaitForExit();
            res = _switchProc.ExitCode;
            _switchProc.Close();

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
