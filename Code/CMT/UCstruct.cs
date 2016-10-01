using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
namespace CMT
{
    /// <summary>
    /// Each UserControl is attached to the structure below.
    /// To navigate from page to page, the application uses this structure.
    /// </summary>
    public class UCstruct
    {
        private Type _userControl;
        private bool _isEnabled;

        public Type userControl
        {
            get
            {
                return _userControl;
            }
            set
            {
                _userControl = value;
            }
        }

        public bool isEnabled
        {
            get
            {
                return _isEnabled;
            }
            set
            {
                _isEnabled = value;
            }
        }

        public UCstruct(Type uc, bool enable)
        {
            _isEnabled = enable;
            _userControl = uc;
        }
    }
}
