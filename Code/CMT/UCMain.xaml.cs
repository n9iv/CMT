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
using System.IO;
using System.Xml;

namespace CMT
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : UserControl
    {
        private BitmapImage _image;
        private Dictionary<string, string> _imagesPath;
        private Dictionary<string, string> _foldersPath;
        public static List<UCstruct> _typeList;

        public Main()
        {
            _imagesPath = new Dictionary<string, string>();
            _foldersPath = new Dictionary<string, string>();  
            InitializeComponent();
            SetImagesPath();
            SetConfiguratorFolderPath();
        }

        private void _rbClockConf_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton r = (RadioButton)sender;
            string s;
            MainWindow.val = new int[1];
            MainWindow.val[0] = -1;
            Navigate.SetClockList();

            if (r.Content != null)
            {
                s = r.Content.ToString();
                DisplayImage(s);
            }
            
            EnableNext();
        }

        private void _rbTswitchConf_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton r = (RadioButton)sender;
            string s;

            MainWindow.val = null;
            Navigate.SetTswitchList();

            if (r.Content != null)
            {
                s = r.Content.ToString();
                DisplayImage(s);
            }
            EnableNext();
        }

        private void _rbCswitchConf_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton r = (RadioButton)sender;
            string s;

            MainWindow.val = null;
            Navigate.SetCswitchList();

            if (r.Content != null)
            {
                s = r.Content.ToString();
                DisplayImage(s);
            }
            EnableNext();
        }

        private void _rbClientSN_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton r = (RadioButton)sender;
            string s;

            MainWindow.val = null;
            Navigate.SetBNapp();

            if (r.Content != null)
            {
                s = r.Content.ToString();
                DisplayImage(s);
            }
            EnableNext();
        }

        private void DisplayImage(string str)
        {
            string imagePath = _imagesPath[str];
            var proc = Process.GetCurrentProcess();
            string confFolder = _foldersPath[str];
            string path = proc.MainModule.FileName;
            string fdPath;
            if (str.Contains("BNET") == true)
                fdPath = @"BN-APP\";
            else
                fdPath = confFolder.Replace(" ", "") + @"\";
            fdPath += @"MainPagePic\";
            imagePath = path.Replace("CMT.exe", fdPath + imagePath);
            if (File.Exists(imagePath))
                _image = new BitmapImage(new Uri(imagePath, UriKind.RelativeOrAbsolute));
            else
                _image = null;
            _iImageMain.Source = _image;
        }

        private void SetImagesPath()
        {
            _imagesPath.Add(_rbClockConf.Content.ToString(), "Clock.png");
            _imagesPath.Add(_rbTswitchConf.Content.ToString(), "T-Switch.png");
            _imagesPath.Add(_rbCswitchConf.Content.ToString(), "C-Switch.png");
            _imagesPath.Add(_rbClientSN.Content.ToString(), "BN-App.png");
        }

        private void EnableNext()
        {
            Window win = Application.Current.Windows[0];
            MainWindow main = (MainWindow)win;
            main._btnNext.IsEnabled = true;
        }

        private void SetConfiguratorFolderPath()
        {
            _foldersPath.Add(_rbClockConf.Content.ToString(), "ClockConfigurator");
            _foldersPath.Add(_rbTswitchConf.Content.ToString(), "T-SwitchConfigurator");
            _foldersPath.Add(_rbCswitchConf.Content.ToString(), "C-SwitchConfigurator");
            _foldersPath.Add(_rbClientSN.Content.ToString(), "BN-APP");
        }
    }
}
