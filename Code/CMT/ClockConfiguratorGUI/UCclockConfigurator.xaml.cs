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
using CMT.ClockConfiguratorGUI;

namespace CMT
{
    /// <summary>
    /// Interaction logic for UCclockConfigurator.xaml
    /// </summary>
    public partial class UCclockConfigurator : UserControl
    {
        private Int16 _val;

        public UCclockConfigurator()
        {
            InitializeComponent();
            //LoadUPDownImage();
        }

        //private void LoadUPDownImage()
        //{
        //    //Load down image
        //    BitmapImage srcDown = new BitmapImage();
        //    srcDown.BeginInit();
        //    srcDown.UriSource = new Uri("../../Resources/Down.png", UriKind.Relative);
        //    srcDown.CacheOption = BitmapCacheOption.OnLoad;
        //    srcDown.EndInit();
        //    _iDown.Source = srcDown;

        //    //Load up image
        //    BitmapImage srcUP = new BitmapImage();
        //    srcUP.BeginInit();
        //    srcUP.UriSource = new Uri("../../Resources/UP.png", UriKind.Relative);
        //    srcUP.CacheOption = BitmapCacheOption.OnLoad;
        //    srcUP.EndInit();
        //    _iUP.Source = srcUP;
        //}

        private void _cmdUp_Click(object sender, RoutedEventArgs e)
        {
            int val = Int32.Parse(_tbBatVal.Text);
            if (val <= 99)
                val += 1;
            _tbBatVal.Text = val.ToString();
        }

        private void _cmdDown_Click(object sender, RoutedEventArgs e)
        {
            int val = Int32.Parse(_tbBatVal.Text);
            if (val > 0)
                val -= 1;
            _tbBatVal.Text = val.ToString();
        }

        private void _rbbbbb_Checked(object sender, RoutedEventArgs e)
        {
            _spBtn.IsEnabled = true;
            _tbBatVal.IsEnabled = true;
            EnableNext();
        }

        private void _rbaaa_Checked(object sender, RoutedEventArgs e)
        {
            if ((_tbBatVal != null) && (_spBtn != null))
            {
                _tbBatVal.IsEnabled = false;
                _spBtn.IsEnabled = false;
            }
            _val = 0;
            Next();
            EnableNext();
        }

        private void EnableNext()
        {
            Window win = Application.Current.Windows[0];
            MainWindow main = (MainWindow)win;
            main._btnNext.IsEnabled = true;
        }

        public void Next()
        {
            double height;
            UCinstruction ins = new UCinstruction(_val);
            height = System.Windows.SystemParameters.PrimaryScreenHeight;
            ins._dcDoc.Height = (500.0 / 768.0) * height;
            UCstruct.usNext = (UserControl)ins;
            UCstruct.func = ins.ReadInstructions;
            UCstruct.isNxtEnabled = true;
        }

        private void _tbBatVal_TextChanged(object sender, TextChangedEventArgs e)
        {
            _val = Int16.Parse(_tbBatVal.Text);
            Next();
        }
    }
}