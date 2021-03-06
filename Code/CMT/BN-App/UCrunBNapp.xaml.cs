﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
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
        }

        private void _btnRun_Click(object sender, RoutedEventArgs e)
        {
            Process BNappProc = new Process();

            BNappProc.StartInfo.FileName = @"BN-APP\run_bn-app.lnk";
            BNappProc.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
            BNappProc.Start();
            Application.Current.Windows[0].Topmost = false;
            BNappProc.WaitForExit();
            Application.Current.Windows[0].Topmost = true;
            MainWindow.FlushClickEvent();
            BNappProc.Close();
        }
    }
}
