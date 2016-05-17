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
        private string _type;
        private bool _rstFlag = false;

        public UCconfigCSwitch(int type, int val)
        {
            InitializeComponent();
            _val = val;
            if (type == 0)
                _type = "r";
            else
                _type = "m";
        }

        private void _btnConfig_Click(object sender, RoutedEventArgs e)
        {
            ThreadStart start = new ThreadStart(WaitConfigurator);
            _ConfThread = new Thread(start);
            _cSwitchProc = new Process();

            if (_rstFlag)
                RunResetScript();
            else
            {
                RunProc(false);
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
                _cSwitchProc.WaitForExit();
                this.Dispatcher.Invoke((Action)(() =>
                {
                    _tbPb.Text = "Applying new configuration";
                }));
            }

            RunProc(false);
            _cSwitchProc.WaitForExit();
            res = _cSwitchProc.ExitCode;
            _cSwitchProc.Close();

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
                _cSwitchProc.StartInfo.FileName = "C-SwitchConfigurator.exe";
                if (flag)
                    _cSwitchProc.StartInfo.Arguments = "reset " + _type.ToString() + " " + _val.ToString() + @" C-SwitchConfigurator\Scripts\";
                else
                    _cSwitchProc.StartInfo.Arguments = _type.ToString() + " " + _val.ToString() + @" C-SwitchConfigurator\Scripts\";
                _cSwitchProc.StartInfo.RedirectStandardInput = true;
                _cSwitchProc.StartInfo.RedirectStandardOutput = true;
                _cSwitchProc.StartInfo.RedirectStandardError = true;
                _cSwitchProc.StartInfo.UseShellExecute = false;
                _cSwitchProc.StartInfo.CreateNoWindow = true;
                _cSwitchProc.Start();
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

        private void _chkbReset_Checked(object sender, RoutedEventArgs e)
        {
            _rstFlag = true;
        }

        private void _chkbReset_Unchecked(object sender, RoutedEventArgs e)
        {
            _rstFlag = false;
        }
    }
}
