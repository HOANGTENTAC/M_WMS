using CommunityToolkit.Mvvm.ComponentModel;

namespace M_WMS.Controls.Models
{
    internal partial class WmsSelectItem : ObservableObject
    {
        public object? Value { get; set; }

        [ObservableProperty]
        private string text = string.Empty;

        [ObservableProperty]
        private bool isSelected;

        [ObservableProperty]
        private Brush backgroundBrush = new SolidColorBrush(Colors.Transparent);

        [ObservableProperty]
        private Color textColor = Colors.Black;

        [ObservableProperty]
        private FontAttributes fontAttributes = FontAttributes.None;

        public object? Data => Value;
        public DataTemplate? Template { get; set; }
    }
}
