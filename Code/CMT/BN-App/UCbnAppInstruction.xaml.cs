﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using System.Windows.Xps.Packaging;

namespace CMT.BN_App
{
    /// <summary>
    /// Interaction logic for UCbnAppInstruction.xaml
    /// </summary>
    public partial class UCbnAppInstruction : UserControl
    {
        private string _fileName = "B-NetInstruction.html";

        public UCbnAppInstruction()
        {
            InitializeComponent();
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
