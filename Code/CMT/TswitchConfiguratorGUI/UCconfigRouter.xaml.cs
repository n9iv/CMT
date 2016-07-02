using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for UCconfigRouter.xaml
    /// </summary>
    public partial class UCconfigRouter : UserControl
    {
        private bool _rstFlag = false;
        private int _MN;
        private int _BN;
        private Process _routerProc;
        private Thread _ConfThread;
        private CircularProgressBar _circularPB;

        public UCconfigRouter(int mn, int bn)
        {
            InitializeComponent();
            _MN = mn;
            _BN = bn;

            if (MainWindow.IsAdmin)
                _cbReset.Visibility = System.Windows.Visibility.Visible;
        }

        private void _btnConfig_Click(object sender, RoutedEventArgs e)
        {
            ThreadStart start = new ThreadStart(WaitConfigurator);
            _tbConf.Text = "";
            _ConfThread = new Thread(start);
            _routerProc = new Process();
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
                _routerProc.WaitForExit();
                res = _routerProc.ExitCode;
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
            _routerProc.WaitForExit();
            res = _routerProc.ExitCode;
            _routerProc.Close();

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
            if (Configurator.Terminated)
                return;
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
                _routerProc.StartInfo.FileName = "T-SwitchConfigurator.exe";
                if (flag)
                    _routerProc.StartInfo.Arguments = "reset r " + _MN.ToString() + " " + _BN.ToString() + @" T-SwitchConfigurator\Scripts";
                else
                    _routerProc.StartInfo.Arguments = "r " + _MN.ToString() + " " + _BN.ToString() + @" T-SwitchConfigurator\Scripts";
                _routerProc.StartInfo.RedirectStandardError = true;
                _routerProc.StartInfo.UseShellExecute = false;
                _routerProc.StartInfo.CreateNoWindow = true;
                Configurator.ConfiguratorProc = _routerProc;
                _routerProc.Start();
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

        private void _cbReset_Unchecked(object sender, RoutedEventArgs e)
        {
            _rstFlag = false;
        }

        private void _cbReset_Checked(object sender, RoutedEventArgs e)
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
