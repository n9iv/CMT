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

        public UCconfigSwitch(int mn, int bn)
        {
            InitializeComponent();
            _MN = mn;
            _BN = bn;
        }

        private void _btnConfig_Click(object sender, RoutedEventArgs e)
        {
            int res;
            string str;
            Process switchProc = new Process();

            switchProc.StartInfo.FileName = "T-SwitchConfigurator.exe";
            switchProc.StartInfo.Arguments = "s " + _MN.ToString() + " " + _BN.ToString() +  "TswitchScript.txt";
            switchProc.StartInfo.RedirectStandardInput = true;
            switchProc.StartInfo.RedirectStandardOutput = true;
            switchProc.StartInfo.RedirectStandardError = true;
            switchProc.StartInfo.UseShellExecute = false;
            switchProc.StartInfo.CreateNoWindow = true;
            switchProc.Start();
            Thread.Sleep(1000);
            res = switchProc.ExitCode;
            StreamReader read = switchProc.StandardOutput;
            switchProc.Close();


            Debug.WriteLine(read.ReadToEnd());
            if (res != -1)
            {
                _tbConf.Foreground = Brushes.Green;
                str = "Configuration succeded";
            }

            else
            {
                str = "Configuration failed!";
                _tbConf.Foreground = Brushes.Red;
            }

            _tbConf.Text = str;
        }
    }
}
