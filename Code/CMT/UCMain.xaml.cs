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
using CMT.BN_App;

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

        private void _rbCswitchConf_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton r = (RadioButton)sender;
            string s;

            MainWindow.val = null;

            if (r.Content != null)
            {
                s = r.Content.ToString();
                DisplayImage(s);
            }
            EnableNext();
            UCstruct.isNxtEnabled = false;
        }

        private void _rbClientSN_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton r = (RadioButton)sender;
            string s;

            MainWindow.val = null;
            SetBNapp();

            if (r.Content != null)
            {
                s = r.Content.ToString();
                DisplayImage(s);
            }
            EnableNext();
            UCstruct.isNxtEnabled = true;
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
            _imagesPath.Add("Clock Configurator","Clock.png");
            _imagesPath.Add("T-Switch Configurator","T-Switch.png");
            _imagesPath.Add("C-Switch Configurator", "C-Switch.png");
            _imagesPath.Add("Client BN-APP", "BN-App.png");
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
            _typeList.Add(typeof(USswitchInstruction));
            _typeList.Add(typeof(UCconfigSwitch));
            _typeList.Add(typeof(UCrouterInstruction));
            _typeList.Add(typeof(UCconfigRouter));
        }

        private void SetBNapp()
        {
            _typeList.Clear();
            _typeList.Add(typeof(UCbnAppInstruction));
            _typeList.Add(typeof(UCrunBNapp));
        }
    }
}
