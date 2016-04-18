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
        private int _MN;
        private int _BN;

        public UCconfigRouter(int mn, int bn)
        {
            InitializeComponent();
            _MN = mn;
            _BN = bn;
            UCstruct.isNxtEnabled = false;
        }

        private void _btnConfig_Click(object sender, RoutedEventArgs e)
        {
            int res = -1;
            string str;
            Process routerProc = new Process();

            routerProc.StartInfo.FileName = "T-SwitchConfigurator.exe";
            routerProc.StartInfo.Arguments = "r " + _MN.ToString() + " " + _BN.ToString() + " Scripts\\T-SwitchScript.txt";
            routerProc.StartInfo.RedirectStandardInput = true;
            routerProc.StartInfo.RedirectStandardOutput = true;
            routerProc.StartInfo.RedirectStandardError = true;
            routerProc.StartInfo.UseShellExecute = false;
            routerProc.StartInfo.CreateNoWindow = true;
            routerProc.Start();
            routerProc.WaitForExit();
            res = routerProc.ExitCode;
            StreamReader read = routerProc.StandardOutput;
            routerProc.Close();


            Debug.WriteLine(read.ReadToEnd());
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
    }
}
