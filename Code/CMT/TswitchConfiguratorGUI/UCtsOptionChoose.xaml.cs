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

namespace CMT.TswitchConfiguratorGUI
{
    /// <summary>
    /// Interaction logic for UCtsOptionChoose.xaml
    /// </summary>
    public partial class UCtsOptionChoose : UserControl
    {
        private int _MN, _BN;

        public UCtsOptionChoose()
        {
            InitializeComponent();
            _MN = Int32.Parse(_tbMN.Text);
            _BN = Int32.Parse(_tbBN.Text);
        }

        private void _cmdUpMN_Click(object sender, RoutedEventArgs e)
        {
            int val = Int32.Parse(_tbMN.Text);
            if (val <= 99)
                val += 1;
            _tbMN.Text = val.ToString();
        }

        private void _cmdDownMN_Click(object sender, RoutedEventArgs e)
        {
            int val = Int32.Parse(_tbMN.Text);
            if (val > 0)
                val -= 1;
            _tbMN.Text = val.ToString();
        }

        private void _cmdUpBN_Click(object sender, RoutedEventArgs e)
        {
            int val = Int32.Parse(_tbBN.Text);
            if (val <= 99)
                val += 1;
            _tbBN.Text = val.ToString();
        }

        private void _cmdDownBN_Click(object sender, RoutedEventArgs e)
        {
            int val = Int32.Parse(_tbBN.Text);
            if (val > 0)
                val -= 1;
            _tbBN.Text = val.ToString();
        }

        private void _rbaaa_Checked(object sender, RoutedEventArgs e)
        {
            if ((_spBtnBN != null) && (_spBtnMN != null) && (_tbBN != null) && (_tbMN != null))
            {
                _spBtnBN.IsEnabled = true;
                _spBtnMN.IsEnabled = true;
                _tbMN.IsEnabled = true;
                _tbBN.IsEnabled = true;
                EnableNext();
                Next();
            }
        }

        private void EnableNext()
        {
            Window win = Application.Current.Windows[0];
            MainWindow main = (MainWindow)win;
            main._btnNext.IsEnabled = true;
        }

        public void Next()
        {
            UCstruct.usNext = new UCconfigSwitch(_MN, _BN);
            UCstruct.isNxtEnabled = true;
        }

        private void _tbMN_TextChanged(object sender, TextChangedEventArgs e)
        {
            _MN = int.Parse(_tbMN.Text);
            Next();
        }

        private void _tbBN_TextChanged(object sender, TextChangedEventArgs e)
        {
            _BN = int.Parse(_tbBN.Text);
            Next();
        }

    }
}