using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace CMT.BN_App
{
    /// <summary>
    /// Interaction logic for UCrunBNapp.xaml
    /// </summary>
    public partial class UCrunBNapp : UserControl
    {
        public UCrunBNapp()
        {
            InitializeComponent();
            UCstruct.isNxtEnabled = false;
        }

        private void _btnRun_Click(object sender, RoutedEventArgs e)
        {
            int res = -1;
            string str;
            Process BNappProc = new Process();

            BNappProc.StartInfo.FileName = "ClockConfigurator.exe";
            BNappProc.StartInfo.Arguments = "";
            BNappProc.StartInfo.RedirectStandardInput = true;
            BNappProc.StartInfo.RedirectStandardOutput = true;
            BNappProc.StartInfo.RedirectStandardError = true;
            BNappProc.StartInfo.UseShellExecute = false;
            BNappProc.StartInfo.CreateNoWindow = true;
            //BNappProc.Start();
            //BNappProc.WaitForExit();
            //res = BNappProc.ExitCode;

            //BNappProc.Close();

            if (res != -1)
            {
                _tbConf.Foreground = Brushes.Green;
                str = "BN-APP succeeded!";
            }

            else
            {
                str = "BN-APP failed!";
                _tbConf.Foreground = Brushes.Red;
            }

            _tbConf.Text = str;
        }
    }
}
