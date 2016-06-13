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
using CMT.CswitchConfiguratorGUI;
using System.Security;

namespace CMT
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static int[] val;
        public static List<UserControl> _userControlList;
        private string _adminUserName = "Administrator";
        private static bool _isAdmin = false;

        public MainWindow()
        {

            double height, width;
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            InitializeComponent();
            _userControlList = new List<UserControl>();

            Main uc = new Main();
            height = System.Windows.SystemParameters.PrimaryScreenHeight;
            width = System.Windows.SystemParameters.PrimaryScreenWidth;
            uc.Width = _cUserCtrlMain.Width;
            uc._iImageMain.Width = (500.0 / 1366.0) * width;
            uc._iImageMain.Height = (500.0 / 768.0) * height;
            _cUserCtrlMain.Content = uc;

        }

        private void _btnShtDown_onClick(object sender, RoutedEventArgs e)
        {
            string msg = "The computer will shut down.\r\nDo you want to continue?";
            string cap = "Warning";

            FlushMouseMessages();

            MessageBoxButton button = MessageBoxButton.YesNo;

            var res = MessageBox.Show(msg, cap, button,MessageBoxImage.Warning,MessageBoxResult.No);

            if (res == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
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

            FlushMouseMessages();

            if (_cUserCtrlMain.Content is Main)
            {
                if (_userControlList.Count > 0)
                    //If another option is selected, clear the control list for new navigation
                    if ((_userControlList[1].GetType() != Main._typeList[0].userControl) && (_userControlList.Count > 1))
                    {
                        _userControlList.Clear();
                    }
                if (!_userControlList.Exists(x => x == _cUserCtrlMain.Content))
                {
                    _userControlList.Add((UserControl)_cUserCtrlMain.Content);
                }
            }

            nxtIndx = _userControlList.FindIndex(x => x == _cUserCtrlMain.Content);

            if (nxtIndx >= 0 && (nxtIndx < _userControlList.Count - 1))
            {
                if ((_userControlList[nxtIndx + 1] is FinalWin) || (_userControlList[nxtIndx + 1] is UCconfigRouter) || (_userControlList[nxtIndx + 1] is UCconfigSwitch) || (_userControlList[nxtIndx + 1] is UCconfigCSwitch))
                {
                    _userControlList[nxtIndx + 1] = (UserControl)GetInstance(_userControlList[nxtIndx + 1].GetType(), val);
                }
                obj = _userControlList[nxtIndx + 1];
            }

            else
            {
                indx = Main._typeList.FindIndex(x => x.userControl == objType);

                if (indx >= 0)
                {
                    objType = Main._typeList[indx + 1].userControl;
                }
                else
                {
                    objType = Main._typeList[0].userControl;
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

            
            _btnNext.IsEnabled = Main._typeList.Find(x => x.userControl == _cUserCtrlMain.Content.GetType()).isEnabled;
       
            if (_btnBack.IsEnabled == false)
                _btnBack.IsEnabled = true;
            if (_btnMainPage.IsEnabled == false)
                _btnMainPage.IsEnabled = true;
        }

        private void btnBack_onClick(object sender, RoutedEventArgs e)
        {
            UserControl obj = (UserControl)_cUserCtrlMain.Content;
            int indx = -1;

            FlushMouseMessages();

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
            FlushMouseMessages();

            _cUserCtrlMain.Content = _userControlList[0];
            _btnNext.IsEnabled = true;
            _btnMainPage.IsEnabled = false;
            _btnBack.IsEnabled = false;
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
        const int WM_SYSCOMMAND = 0x0112;
        const int SC_MOVE = 0xF010;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Environment.UserName == _adminUserName)
                _isAdmin = true;
            if (!_isAdmin)
            {
                var hwnd = new WindowInteropHelper(this).Handle;
                SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU);
                this.Topmost = true;
                ResizeMode = System.Windows.ResizeMode.NoResize;
            }
        }

        //Disable resize window
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

            switch (msg)
            {
                case WM_SYSCOMMAND:
                    int command = wParam.ToInt32() & 0xfff0;
                    if (command == SC_MOVE)
                    {
                        handled = true;
                    }
                    break;
                default:
                    break;
            }

            return IntPtr.Zero;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!_isAdmin)
            {
                e.Cancel = true;
            }
        }

        public static bool IsAdmin
        {
            get
            {
                return _isAdmin;
            }
        }

        public void SetNavigateBar(bool enable)
        {
            _cUserCtrlBtn.IsEnabled = enable;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct NativeMessage
        {
            public IntPtr handle;
            public uint msg;
            public IntPtr wParam;
            public IntPtr lParam;
            public uint time;
            public System.Drawing.Point p;
        }

        [SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("User32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool PeekMessage(out NativeMessage message,
            IntPtr handle, uint filterMin, uint filterMax, uint flags);
        private const UInt32 WM_MOUSEFIRST = 0x0200;
        private const UInt32 WM_MOUSELAST = 0x020D;
        public const int PM_REMOVE = 0x0001;

        // Flush all pending mouse events.
        private static void FlushMouseMessages()
        {
            NativeMessage msg;
            // Repeat until PeekMessage returns false.
            while (PeekMessage(out msg, IntPtr.Zero,
                WM_MOUSEFIRST, WM_MOUSELAST, PM_REMOVE))
                ;
        }

        public static void FlushClickEvent()
        {
            FlushMouseMessages();
        }
    }
}
