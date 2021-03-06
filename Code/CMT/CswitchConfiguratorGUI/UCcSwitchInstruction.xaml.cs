﻿using System;
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
using System.IO;
using System.Diagnostics;
using mshtml;

namespace CMT.CswitchConfiguratorGUI
{
    /// <summary>
    /// Loads HTML file instructions.
    /// </summary>
    public partial class UCcSwitchInstruction : UserControl
    {
        private int _val;
        private string _fileName = "CSwitchInstructions.html";
        private string _fileName1 = "CSwitchInstructions.htm";

        public UCcSwitchInstruction(int type, int val)
        {
            InitializeComponent();
            _val = val;
            MainWindow.val[0] = type;
            MainWindow.val[1] = _val;
        }

         private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            string fullPath = GetFullPath(_fileName);

            if (!File.Exists(fullPath))
            {
                fullPath = GetFullPath(_fileName1);
                if (!File.Exists(fullPath))
                {
                    MainWindow main = (MainWindow)Application.Current.Windows[0];
                    MessageBox.Show("There is no file instruction.");
                    main._btnNext.IsEnabled = false;
                    return;
                }
            }
            this._wbInstruction.Navigate(fullPath);
        }

        private string GetFullPath(string name)
        {
            var process = Process.GetCurrentProcess(); // Or whatever method you are using
            string fullPath = process.MainModule.FileName;
            string fileName = "C-SwitchConfigurator\\Instructions\\" + name;
            fullPath = fullPath.Replace("CMT.exe", fileName);
            return fullPath;
        }

        private void _wbInstruction_LoadCompleted(object sender, NavigationEventArgs e)
        {
            mshtml.IHTMLDocument2 dom = (mshtml.IHTMLDocument2)_wbInstruction.Document;
            dom.body.style.overflow = "hidden";
        }
    }
}
