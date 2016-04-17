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
using System.Diagnostics;
using CMT.ClockConfiguratorGUI;
using CMT.TswitchConfiguratorGUI;

namespace CMT
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : UserControl
    {
        private BitmapImage _image;
        private Dictionary<string, string> _imagesPath;
        public static List<Type> _typeList;

        public Main()
        {
            _imagesPath = new Dictionary<string, string>();
            SetImagesPath();
            InitializeComponent();
            _typeList = new List<Type>();
            //LoadInitImage();
        }

        private void LoadInitImage()
        {
            _image = new BitmapImage(new Uri(@"Rafael_pic.png", UriKind.Relative));
            _image.CacheOption = BitmapCacheOption.OnLoad;
            _iImageMain.Source = _image;
        }

        private void _rbClockConf_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton r = (RadioButton)sender;
            string s;
            MainWindow.val = new int[1];
            MainWindow.val[0] = -1;
            SetClockList();

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
            RadioButton r = (RadioButton)sender;
            string s;

            MainWindow.val = null;
            SetTswitchList();

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
            var proc = Process.GetCurrentProcess();
            string path = proc.MainModule.FileName;

            imagePath = path.Replace("CMT.exe", "MainPagePics\\" + imagePath);
            _image = new BitmapImage(new Uri(imagePath, UriKind.RelativeOrAbsolute));
            _iImageMain.Source = _image;
        }

        private void SetImagesPath()
        {
            _imagesPath.Add("Clock Configurator","Rafael_pic.png");
            _imagesPath.Add("T-Switch Configurator","Desert.png");
        }

        private void EnableNext()
        {
            Window win = Application.Current.Windows[0];
            MainWindow main = (MainWindow)win;
            main._btnNext.IsEnabled = true;
        }

        private void SetClockList()
        {
            _typeList.Clear();
            _typeList.Add(typeof(UCclockConfigurator));
            _typeList.Add(typeof(UCinstruction));
            _typeList.Add(typeof(FinalWin));
        }

        private void SetTswitchList()
        {
            _typeList.Clear();
            _typeList.Add(typeof(UCtsOptionChoose));
            _typeList.Add(typeof(UCconfigSwitch));
        }
    }
}
