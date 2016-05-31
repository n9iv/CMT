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

namespace CMT.ClockConfiguratorGUI
{
    /// <summary>
    /// Interaction logic for FinalWin.xaml
    /// </summary>
    public partial class FinalWin : UserControl
    {
        private int _val;
        private MainWindow _win;
        private Process _clockProc;
        private Thread _ConfThread;

        public FinalWin(int val)
        {
            InitializeComponent();
            _val = val;
            _win = (MainWindow)Application.Current.Windows[0];
        }

        private void _btnConfig_Click(object sender, RoutedEventArgs e)
        {
            ThreadStart start = new ThreadStart(WaitConfigurator);
            _ConfThread = new Thread(start);
            _clockProc = new Process();

            _tbConf.Text = "";
            _clockProc.StartInfo.FileName = "ClockConfigurator.exe";
            _clockProc.StartInfo.Arguments = _val.ToString() + " Scripts\\ClockScript.txt";
            _clockProc.StartInfo.RedirectStandardInput = true;
            _clockProc.StartInfo.RedirectStandardOutput = true;
            _clockProc.StartInfo.RedirectStandardError = true;
            _clockProc.StartInfo.UseShellExecute = false;
            _clockProc.StartInfo.CreateNoWindow = true;
            _clockProc.Start();
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

            _clockProc.WaitForExit();
            res = _clockProc.ExitCode;
            _clockProc.Close();

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
    }
}
