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
        private bool _rstFlag = false;

        public UCconfigSwitch(int mn, int bn)
        {
            InitializeComponent();
            _MN = mn;
            _BN = bn;
        }

        private void _btnConfig_Click(object sender, RoutedEventArgs e)
        {
            _switchProc = new Process();
            ThreadStart start = new ThreadStart(WaitConfigurator);
            _ConfThread = new Thread(start);

            if (_rstFlag)
                RunResetScript();
            else
            {
                _tbPb.Text = "Applying new configuration";
            }
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
            if (_rstFlag)
            {
                _switchProc.WaitForExit();
                this.Dispatcher.Invoke((Action)(() =>
             {
                 _tbPb.Text = "Applying new configuration";
             }));
            }

            RunProc(false);
            _switchProc.WaitForExit();
            res = _switchProc.ExitCode;
            _switchProc.Close();

            if (res == (int)ErrorCodes.Success)
            {
                b = Brushes.Green;
                str = "Configuration succeeded!";
            }

            else
            {
                str = Configurator.GetErrorMsg((ErrorCodes)res);
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

        private void RunProc(bool flag)
        {
            this.Dispatcher.Invoke((Action)(() =>
       {
           _switchProc.StartInfo.FileName = "T-SwitchConfigurator.exe";
           if (flag)
               _switchProc.StartInfo.Arguments = "reset s " + _MN.ToString() + " " + _BN.ToString() + @" T-SwitchConfigurator\Scripts";
           else
               _switchProc.StartInfo.Arguments = "s " + _MN.ToString() + " " + _BN.ToString() + @" T-SwitchConfigurator\Scripts";
           _switchProc.StartInfo.RedirectStandardError = true;
           _switchProc.StartInfo.UseShellExecute = false;
           _switchProc.StartInfo.CreateNoWindow = true;
           _switchProc.Start();
       }));
        }

        private void RunResetScript()
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                _tbPb.Text = "Reseting configuration";
            }));

            RunProc(_rstFlag);
        }

        private void _chkbReset_Unchecked(object sender, RoutedEventArgs e)
        {
            _rstFlag = false;
        }

        private void _chkbReset_Checked(object sender, RoutedEventArgs e)
        {
            _rstFlag = true;
        }
    }
}
