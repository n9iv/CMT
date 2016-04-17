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
using CMT.TswitchConfiguratorGUI;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Interop;

namespace CMT
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static UserControl nextUC;
        public List<UserControl> _backList;
        public static int[] val;
        public static List<UserControl> _userControlList;

        public MainWindow()
        {

            double height, width;
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();
            _backList = new List<UserControl>();
            _userControlList = new List<UserControl>();

            Main uc = new Main();
            height = System.Windows.SystemParameters.PrimaryScreenHeight;
            width = System.Windows.SystemParameters.PrimaryScreenWidth;
            uc.Width = _cUserCtrlMain.Width;
            uc._iImageMain.Width = (500.0 / 1366.0) * width;
            uc._iImageMain.Height = (500.0 / 768.0) * height;
            _cUserCtrlMain.Content = uc;

        }

        //private void btnNext_onClick(object sender, RoutedEventArgs e)
        //{
        //    UserControl tempUC = UCstruct.usNext;
        //    int index, flag = 0;

        //    if ((_cUserCtrlMain.Content is Main) && (_isBackClicked))
        //    {
        //        _backList.Clear();
        //        UCstruct.func = null;
        //        _isBackClicked = false;
        //    }
        //    if (_btnBack.IsEnabled == false)
        //        _btnBack.IsEnabled = true;
        //    tempUC.Height = _cUserCtrlMain.Height;
        //    tempUC.Width = _cUserCtrlMain.Width;
        //    if (_isBackClicked)
        //    {
        //        index = _backList.FindIndex(x => x == (UserControl)_cUserCtrlMain.Content) + 1;
        //        _cUserCtrlMain.Content = _backList[index];
        //        if (index == _backList.Count-1)
        //            _btnNext.IsEnabled = false;
        //        _isBackClicked = false;
        //        return;
        //    }
        //    if (_backList.Count == 0)
        //        _backList.Add((UserControl)_cUserCtrlMain.Content);
        //    else if (!IsExist((UserControl)_cUserCtrlMain.Content))
        //        _backList.Add((UserControl)_cUserCtrlMain.Content);
        //    else
        //    {
        //        index = _backList.FindIndex(x => x == (UserControl)_cUserCtrlMain.Content) + 1;
        //        _cUserCtrlMain.Content = _backList[index];
        //        flag++;
        //    }
        //    if (flag == 0)
        //        _cUserCtrlMain.Content = tempUC;
        //    flag = 0;
        //    if (UCstruct.func != null)
        //    {
        //        //UCstruct.func();
        //        UCstruct.func = null;
        //    }
        //    if (UCstruct.isNxtEnabled == false)
        //        _btnNext.IsEnabled = false;

        //    if (_cUserCtrlMain.Content is FinalWin)
        //        _btnNext.IsEnabled = false;
        //}

        //private void btnBack_onClick(object sender, RoutedEventArgs e)
        //{
        //    UserControl tempUC;
        //    int index = _backList.Count - 1;

        //    //UCsecks if the current window is in the list
        //    if(!IsExist((UserControl)_cUserCtrlMain.Content))
        //        _backList.Add((UserControl)_cUserCtrlMain.Content);
        //    //checks if the next window to display is in the list
        //    if (!IsExist(UCstruct.usNext))
        //        _backList.Add(UCstruct.usNext);

        //    //find the previous window of the one displayed
        //    if (IsExist((UserControl)_cUserCtrlMain.Content))
        //        index = _backList.FindIndex(x => x == (UserControl)_cUserCtrlMain.Content) - 1;
        //    tempUC = _backList[index];

        //    if (tempUC is Main)
        //    {
        //        double width, height;
        //        Main uc = new Main();

        //        height = System.Windows.SystemParameters.PrimaryScreenHeight;
        //        width = System.Windows.SystemParameters.PrimaryScreenWidth;
        //        uc.Width = _cUserCtrlMain.Width;
        //        uc._iImageMain.Width = (500.0 / 1366.0) * width;
        //        uc._iImageMain.Height = (500.0 / 768.0) * height;

        //        tempUC = uc;
        //    }
        //    _cUserCtrlMain.Content = tempUC;
        //    if ((_backList.Count == 0) || (_cUserCtrlMain.Content is Main))
        //        _btnBack.IsEnabled = false;
        //    _btnNext.IsEnabled = true;
        //    _isBackClicked = true;
        //}

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

            if (res == MessageBoxResult.Yes)
            {
                var psi = new ProcessStartInfo("shutdown", "/s /t 0");
                psi.CreateNoWindow = true;
                psi.UseShellExecute = false;
                Process.Start(psi);
            }
        }

        private object GetInstance(Type t, int[] num)
        {
            if (num == null)
                return Activator.CreateInstance(t);
            if (num.Length == 1)
            {
                if (num[0] == -1)
                    return Activator.CreateInstance(t);
                else
                    return Activator.CreateInstance(t, num[0]);
            }

            return Activator.CreateInstance(t, num[0], num[1]);
        }

        private void btnNext_onClick(object sender, RoutedEventArgs e)
        {
            Type objType = _cUserCtrlMain.Content.GetType();
            UserControl obj = null;
            int indx = -1, nxtIndx = -1;

            if (_cUserCtrlMain.Content is Main)
            {
                if (_userControlList.Count > 0)
                    if ((_userControlList[1].GetType() != Main._typeList[0]) && (_userControlList.Count > 1))
                    {
                        _userControlList.Clear();
                    }
                if (!_userControlList.Exists(x => x == _cUserCtrlMain.Content))
                {
                    _userControlList.Add((UserControl)_cUserCtrlMain.Content);
                }
            }

            nxtIndx = _userControlList.FindIndex(x => x == _cUserCtrlMain.Content);

            if (nxtIndx >= 0 && (nxtIndx < _userControlList.Count - 2))
            {
                obj = _userControlList[nxtIndx + 1];
            }

            else
            {
                indx = Main._typeList.FindIndex(x => x == objType);

                if (indx >= 0)
                {
                    objType = Main._typeList[indx + 1];
                }
                else
                {
                    objType = Main._typeList[0];
                }
                obj = (UserControl)GetInstance(objType, val);
            }

            obj.Height = _cUserCtrlMain.Height;
            obj.Width = _cUserCtrlMain.Width;
            _cUserCtrlMain.Content = (UserControl)obj;

            if (!_userControlList.Exists(x => x == obj))
                _userControlList.Add((UserControl)obj);
            if (indx == Main._typeList.Count - 1)
                _btnNext.IsEnabled = false;
            if (UCstruct.isNxtEnabled == false)
            {
                _btnNext.IsEnabled = false;
                UCstruct.isNxtEnabled = true;
            }
            if (_btnBack.IsEnabled == false)
                _btnBack.IsEnabled = true;
            if (_btnMainPage.IsEnabled == false)
                _btnMainPage.IsEnabled = true;
        }

        private void btnBack_onClick(object sender, RoutedEventArgs e)
        {
            UserControl obj = (UserControl)_cUserCtrlMain.Content;
            int indx = -1;

            indx = _userControlList.FindIndex(x => x == obj);

            _cUserCtrlMain.Content = _userControlList[indx - 1];
            if (indx - 1 == 0)
            {
                _btnBack.IsEnabled = false;
                _btnMainPage.IsEnabled = false;
            }
            if (_btnNext.IsEnabled == false)
                _btnNext.IsEnabled = true;
        }

        private void _btnMainPage_Click(object sender, RoutedEventArgs e)
        {
            _cUserCtrlMain.Content = _userControlList[0];
            _btnMainPage.IsEnabled = false;
        }

        private void SetDefaultList()
        {
            UserControl u = new Main();
            _userControlList.Clear();

            u.Height = _cUserCtrlMain.Height;
            u.Width = _cUserCtrlMain.Width;

            _userControlList.Add(u);
        }

        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;
        private const int WS_MAXIMIZEBOX = 0x10000;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var hwnd = new WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
        }

        private void Window_SourceInitialized(object sender, EventArgs e)
        {
            System.Windows.Interop.HwndSource source = System.Windows.Interop.HwndSource.FromHwnd(new System.Windows.Interop.WindowInteropHelper(this).Handle);
            source.AddHook(new System.Windows.Interop.HwndSourceHook(WndProc));
        }

        int WM_NCLBUTTONDBLCLK { get { return 0x00A3; } }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == WM_NCLBUTTONDBLCLK)
            {
                handled = true;  //prevent double click from maximizing the window.
            }

            return IntPtr.Zero;
        }
    }
}
