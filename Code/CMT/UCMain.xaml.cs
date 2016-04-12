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
using System.Collections;

namespace CMT
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : UserControl
    {
        private BitmapImage _image;
        private Dictionary<string, string> _imagesPath;

        public Main()
        {
            _imagesPath = new Dictionary<string, string>();
            SetImagesPath();
            InitializeComponent();
            LoadInitImage();
        }

        private void LoadInitImage()
        {
            _image = new BitmapImage(new Uri(@"Resources\Rafael_pic.png", UriKind.Relative));
            _image.CacheOption = BitmapCacheOption.OnLoad;
            _iImageMain.Source = _image;
        }

        private void _rbClockConf_Checked(object sender, RoutedEventArgs e)
        {
            UCstruct.usNext = new UCclockConfigurator();
            RadioButton r = (RadioButton)sender;
            string s;
            if (r.Content != null)
            {
                s = r.Content.ToString();
                DisplayImage(s);
            }
            EnableNext();
            UCstruct.isNxtEnabled = false;
        }

        private void _rbTswitchConf_Checked(object sender, RoutedEventArgs e)
        {
            UCstruct.usNext = new TswitchConfiguratorGUI.UCtsOptionChoose();
            RadioButton r = (RadioButton)sender;
            string s;

            if (r.Content != null)
            {
                s = r.Content.ToString();
                DisplayImage(s);
            }
            EnableNext();
            UCstruct.isNxtEnabled = false;
        }

        private void DisplayImage(string str)
        {
            string imagePath = _imagesPath[str];

            _image = new BitmapImage(new Uri(@imagePath, UriKind.Relative));
            _iImageMain.Source = _image;
        }

        private void SetImagesPath()
        {
            _imagesPath.Add("Clock Configurator","Resources\\Rafael_pic.png");
            _imagesPath.Add("T-Switch Configurator","Resources\\Desert.png");
        }

        private void EnableNext()
        {
            Window win = Application.Current.Windows[0];
            MainWindow main = (MainWindow)win;
            main._btnNext.IsEnabled = true;
        }
    }
}
