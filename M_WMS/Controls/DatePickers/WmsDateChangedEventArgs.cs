using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M_WMS.Controls.DatePickers
{
    public class WmsDateChangedEventArgs : EventArgs
    {
        public DateTime? OldDate { get; }

        public DateTime? NewDate { get; }

        public WmsDateChangedEventArgs(DateTime? oldDate, DateTime? newDate)
        {
            OldDate = oldDate;
            NewDate = newDate;
        }
    }
}
