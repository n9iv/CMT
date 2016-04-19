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
using Microsoft.Win32;
using System.Windows.Xps.Packaging;
using System.IO;
using System.Threading;

namespace CMT.ClockConfiguratorGUI
{
    /// <summary>
    /// Interaction logic for UCinstruction.xaml
    /// </summary>
    public partial class UCinstruction : UserControl
    {
        private int _val;
        private string _fileName = "ClockInstructions.html";

        public UCinstruction(int val)
        {
            InitializeComponent();
            _val = val;
            MainWindow.val[0] = _val;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var process = Process.GetCurrentProcess(); // Or whatever method you are using
            string fullPath = process.MainModule.FileName;
            string fileName = "Instructions\\" + _fileName;
            fullPath = fullPath.Replace("CMT.exe", fileName);
            if (!File.Exists(fullPath))
            {
                MainWindow main = (MainWindow)Application.Current.Windows[0];
                MessageBox.Show("There is no file instruction.");
                main._btnNext.IsEnabled = false;
                return;
            }
            this._wbInstruction.Navigate(fullPath);
        }
    }
}
