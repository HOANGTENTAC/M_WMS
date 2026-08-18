namespace M_WMS.Controls
{
    public class WmsSelectItemSelectedEventArgs : EventArgs
    {
        public object? SelectedItem { get; init; }

        public object? SelectedValue { get; init; }
    }
}
