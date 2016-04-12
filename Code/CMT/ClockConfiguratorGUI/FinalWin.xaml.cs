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
        private Int16 _val;
        private MainWindow _win;

        public FinalWin(Int16 val)
        {
            InitializeComponent();
            _val = val;
            _win = (MainWindow)Application.Current.Windows[0];
           // _win._btnNext.IsEnabled = false;
        }

        private void _btnConfig_Click(object sender, RoutedEventArgs e)
        {
            int res;
            string str;
            Process clockProc = new Process();

            clockProc.StartInfo.FileName = "ClockConfigurator.exe";
            clockProc.StartInfo.Arguments = _val.ToString() + "ClockScript.txt";
            clockProc.StartInfo.RedirectStandardInput = true;
            clockProc.StartInfo.RedirectStandardOutput = true;
            clockProc.StartInfo.RedirectStandardError = true;
            clockProc.StartInfo.UseShellExecute = false;
            clockProc.StartInfo.CreateNoWindow = true;
            clockProc.Start();
            Thread.Sleep(1000);
            res = clockProc.ExitCode;
            StreamReader read = clockProc.StandardOutput;

            clockProc.Close();

            if (res != -1)
            {
                _tbConf.Foreground = Brushes.Green;
                str = "Configuration succeded\n" + read.ReadToEnd();
                _win._btnShtDown.IsEnabled = true;
            }

            else
            {
                str = "Configuration failed!";
                _tbConf.Foreground = Brushes.Red;
            }

            _tbConf.Text = str;
        }

        private void _btnReturn_Click(object sender, RoutedEventArgs e)
        {
            double width, height;
            Main uc = new Main();

            height = System.Windows.SystemParameters.PrimaryScreenHeight;
            width = System.Windows.SystemParameters.PrimaryScreenWidth;
            uc.Width = _win._cUserCtrlMain.Width;
            uc._iImageMain.Width = (500.0 / 1366.0) * width;
            uc._iImageMain.Height = (500.0 / 768.0) * height;
            _win._cUserCtrlMain.Content = uc;
            _win._backList.Clear();
        }
    }
}
