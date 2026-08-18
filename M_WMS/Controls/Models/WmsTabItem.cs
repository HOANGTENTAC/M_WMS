using CommunityToolkit.Mvvm.ComponentModel;
using System.Windows.Input;

namespace M_WMS.Controls.Models
{
    public partial class WmsTabItem
    {
        /// <summary>
        /// Tiêu đề tab
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Icon
        /// </summary>
        public ImageSource? Icon { get; set; }

        /// <summary>
        /// Hàm tạo View
        /// </summary>
        public Func<View>? ViewFactory { get; set; }

        /// <summary>
        /// Cache View (internal)
        /// </summary>
        internal View? CachedView { get; set; }
    }
}
