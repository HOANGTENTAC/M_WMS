using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M_WMS.Controls.DatePickers
{
    public partial class WmsDatePicker
    {
        private bool _isFocused;

        public bool IsControlFocused => _isFocused;

        public event EventHandler? Focused;
        public event EventHandler? Unfocused;
        public event EventHandler? Clicked;
        public event EventHandler<WmsDateChangedEventArgs>? DateChanged;
    }
}
