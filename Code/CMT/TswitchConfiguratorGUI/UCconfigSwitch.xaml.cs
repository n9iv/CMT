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
        private CircularProgressBar _circularPB;

        public UCconfigSwitch(int mn, int bn)
        {
            InitializeComponent();
            _MN = mn;
            _BN = bn;

            if (MainWindow.IsAdmin)
                _chkbReset.Visibility = System.Windows.Visibility.Visible;
        }

        private void _btnConfig_Click(object sender, RoutedEventArgs e)
        {
            _switchProc = new Process();
            ThreadStart start = new ThreadStart(WaitConfigurator);
            _tbConf.Text = "";
            _ConfThread = new Thread(start);
            SetEnableNavigators(false);

            if (_rstFlag)
                RunResetScript();
            else
            {
                _tbPb.Text = "Applying new configuration";
            }
            _ConfThread.Start();
            _btnConfig.IsEnabled = false;
            _tbPb.Visibility = System.Windows.Visibility.Visible;
            _circularPB = new CircularProgressBar();
            _ucPb.Content = _circularPB;
            _ucPb.Visibility = System.Windows.Visibility.Visible;
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
                res = _switchProc.ExitCode;
                if (res != (int)ErrorCodes.Success)
                {
                    str = Configurator.GetErrorMsg((ErrorCodes)res);
                    EndConfigurate(b, str);
                    return;
                }
                this.Dispatcher.Invoke((Action)(() =>
             {
                 _tbPb.Text = "Applying new configuration";
             }));
            }

            RunProc(false);
            _switchProc.WaitForExit();
            res = _switchProc.ExitCode;
            _switchProc.Close();
            str = Configurator.GetErrorMsg((ErrorCodes)res);
            if (res == (int)ErrorCodes.Success)
            {
                b = Brushes.Green;
            }

            else
            {
                b = Brushes.Red;
            }
            EndConfigurate(b, str);
        }

        private void EndConfigurate(Brush color, String msg)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                _tbConf.Foreground = color;
                _tbConf.Text = msg;
                _btnConfig.IsEnabled = true;
                _circularPB.StopProgress();
                _ucPb.Visibility = System.Windows.Visibility.Hidden;
                _tbPb.Visibility = System.Windows.Visibility.Hidden;
                SetEnableNavigators(true);
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

        private void SetEnableNavigators(bool enable)
        {
            Window win = Application.Current.Windows[0];
            MainWindow m = (MainWindow)win;
            m.SetNavigateBar(enable);
        }
    }
}
