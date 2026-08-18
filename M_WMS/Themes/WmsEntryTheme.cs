namespace M_WMS.Themes
{
    public class WmsEntryTheme
    {
        public Brush Background { get; set; } = Brush.White;

        public Brush FocusedBackground { get; set; } = Brush.White;

        public Brush DisabledBackground { get; set; }
            = new SolidColorBrush(Color.FromArgb("#F5F5F5"));

        public Brush ErrorBackground { get; set; }
            = Brush.White;

        public Color TextColor { get; set; } = Colors.Black;

        public Color PlaceholderColor { get; set; }
            = Colors.Gray;
    }
}
