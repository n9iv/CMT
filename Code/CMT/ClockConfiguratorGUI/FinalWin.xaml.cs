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

        public FinalWin(int val)
        {
            InitializeComponent();
            _val = val;
            _win = (MainWindow)Application.Current.Windows[0];
            UCstruct.isNxtEnabled = false;

        }

        private void _btnConfig_Click(object sender, RoutedEventArgs e)
        {
            int res = -1;
            string str;
            Process clockProc = new Process();

            clockProc.StartInfo.FileName = "ClockConfigurator.exe";
            clockProc.StartInfo.Arguments = _val.ToString() + " Scripts\\ClockScript.txt";
            clockProc.StartInfo.RedirectStandardInput = true;
            clockProc.StartInfo.RedirectStandardOutput = true;
            clockProc.StartInfo.RedirectStandardError = true;
            clockProc.StartInfo.UseShellExecute = false;
            clockProc.StartInfo.CreateNoWindow = true;
            clockProc.Start();
            clockProc.WaitForExit();
            res = clockProc.ExitCode;
            StreamReader read = clockProc.StandardOutput;

            clockProc.Close();

            if (res != -1)
            {
                _tbConf.Foreground = Brushes.Green;
                str = "Configuration succeeded!";
            }

            else
            {
                str = "Configuration failed!";
                _tbConf.Foreground = Brushes.Red;
            }

            _tbConf.Text = str;
        }

        private void _btnConfig_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _tbConf.Text = "";
        }

    }
}
