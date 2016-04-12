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
using System.Diagnostics;

namespace CMT
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _isBackClicked = false;
        public static UserControl nextUC;
        public List<UserControl> _backList;

        public MainWindow()
        {

            double height, width;
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();
            _backList = new List<UserControl>();
            Main uc = new Main();
            height = System.Windows.SystemParameters.PrimaryScreenHeight;
            width = System.Windows.SystemParameters.PrimaryScreenWidth;
            uc.Width = _cUserCtrlMain.Width;
            uc._iImageMain.Width = (500.0 / 1366.0) * width;
            uc._iImageMain.Height = (500.0 / 768.0) * height;
             _cUserCtrlMain.Content = uc;

        }

        private void btnNext_onClick(object sender, RoutedEventArgs e)
        {
            UserControl tempUC = UCstruct.usNext;
            int index, flag = 0;

            if ((_cUserCtrlMain.Content is Main) && (_isBackClicked))
            {
                _backList.Clear();
                UCstruct.func = null;
                _isBackClicked = false;
            }
            if (_btnBack.IsEnabled == false)
                _btnBack.IsEnabled = true;
            tempUC.Height = _cUserCtrlMain.Height;
            tempUC.Width = _cUserCtrlMain.Width;
            if (_isBackClicked)
            {
                index = _backList.FindIndex(x => x == (UserControl)_cUserCtrlMain.Content) + 1;
                _cUserCtrlMain.Content = _backList[index];
                if (index == _backList.Count-1)
                    _btnNext.IsEnabled = false;
                _isBackClicked = false;
                return;
            }
            if (_backList.Count == 0)
                _backList.Add((UserControl)_cUserCtrlMain.Content);
            else if (!IsExist((UserControl)_cUserCtrlMain.Content))
                _backList.Add((UserControl)_cUserCtrlMain.Content);
            else
            {
                index = _backList.FindIndex(x => x == (UserControl)_cUserCtrlMain.Content) + 1;
                _cUserCtrlMain.Content = _backList[index];
                flag++;
            }
            if (flag == 0)
                _cUserCtrlMain.Content = tempUC;
            flag = 0;
            if (UCstruct.func != null)
            {
                //UCstruct.func();
                UCstruct.func = null;
            }
            if (UCstruct.isNxtEnabled == false)
                _btnNext.IsEnabled = false;

            if (_cUserCtrlMain.Content is FinalWin)
                _btnNext.IsEnabled = false;
        }

        private void btnBack_onClick(object sender, RoutedEventArgs e)
        {
            UserControl tempUC;
            int index = _backList.Count - 1;

            //UCsecks if the current window is in the list
            if(!IsExist((UserControl)_cUserCtrlMain.Content))
                _backList.Add((UserControl)_cUserCtrlMain.Content);
            //checks if the next window to display is in the list
            if (!IsExist(UCstruct.usNext))
                _backList.Add(UCstruct.usNext);

            //find the previous window of the one displayed
            if (IsExist((UserControl)_cUserCtrlMain.Content))
                index = _backList.FindIndex(x => x == (UserControl)_cUserCtrlMain.Content) - 1;
            tempUC = _backList[index];

            if (tempUC is Main)
            {
                double width, height;
                Main uc = new Main();

                height = System.Windows.SystemParameters.PrimaryScreenHeight;
                width = System.Windows.SystemParameters.PrimaryScreenWidth;
                uc.Width = _cUserCtrlMain.Width;
                uc._iImageMain.Width = (500.0 / 1366.0) * width;
                uc._iImageMain.Height = (500.0 / 768.0) * height;

                tempUC = uc;
            }
            _cUserCtrlMain.Content = tempUC;
            if ((_backList.Count == 0) || (_cUserCtrlMain.Content is Main))
                _btnBack.IsEnabled = false;
            _btnNext.IsEnabled = true;
            _isBackClicked = true;
        }

        private bool IsExist(UserControl u)
        {
            bool isExist = false;
            
            foreach (UserControl uc in _backList)
            {
                if (uc.Equals(u))
                    return true;
            }

            return isExist;
        }

        private void _btnShtDown_onClick(object sender, RoutedEventArgs e)
        {
            string msg = "The computer will shut down.\r\nDo you want to continue?";
            string cap = "Warning";
            MessageBoxButton button = MessageBoxButton.YesNo;

            var res = MessageBox.Show(msg, cap, button);

            if(res == MessageBoxResult.Yes)
            {
                var psi = new ProcessStartInfo("shutdown", "/s /t 0");
                psi.CreateNoWindow = true;
                psi.UseShellExecute = false;
                Process.Start(psi);
            }
        }

    }
}
