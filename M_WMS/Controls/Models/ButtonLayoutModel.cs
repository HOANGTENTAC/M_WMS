namespace M_WMS.Controls.Models
{
    internal class ButtonLayoutModel
    {
        public List<GridLength> Columns { get; } = new();

        public List<GridLength> Rows { get; } = new();

        public bool ShowIcon { get; set; }

        public int IconRow { get; set; }

        public int IconColumn { get; set; }

        public int TextRow { get; set; }

        public int TextColumn { get; set; }

        public LayoutOptions ContentHorizontal { get; set; } = LayoutOptions.Fill;

        public LayoutOptions ContentVertical { get; set; } = LayoutOptions.Fill;

        public LayoutOptions TextHorizontal { get; set; } = LayoutOptions.Center;

        public TextAlignment TextAlignment { get; set; } = TextAlignment.Center;

    }
}
