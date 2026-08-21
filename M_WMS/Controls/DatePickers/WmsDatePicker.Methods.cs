using M_WMS.Controls.Borders;
using M_WMS.Controls.Popups;

namespace M_WMS.Controls.DatePickers
{
    public partial class WmsDatePicker
    {
        private void UpdateText()
        {
            //if (Date.HasValue)
            //{
            //    PART_Text.Text = Date.Value.ToString(Format);
            //}
            //else
            //{
            //    PART_Text.Text = Placeholder;
            //}

            //UpdateTextAppearance();
            if (Date == null)
            {
                PART_Text.Text = Placeholder;
                PART_Text.TextColor = PlaceholderColor;
                return;
            }

            PART_Text.Text = Date.Value.ToString(Format);
            PART_Text.TextColor = TextColor;
        }
        private void UpdateFont()
        {
            PART_Text.FontSize = FontSize;
            PART_Text.FontFamily = FontFamily;
            PART_Text.FontAttributes = FontAttributes;
        }
        private void UpdateTextAppearance()
        {
            PART_Text.TextColor = Date == null
                ? PlaceholderColor
                : TextColor;
        }
        private void RootGrid_Tapped(object? sender, TappedEventArgs e)
        {
            //RaiseFocused();

            Clicked?.Invoke(this, EventArgs.Empty);
        }

        private async void OnTapped(object? sender, TappedEventArgs e)
        {
            //RaiseFocused();
            var border = FindParentBorder();
            //Clicked?.Invoke(this, EventArgs.Empty);
            //await M_WMS.Services.WmsPopupService.ShowAsync(new WmsCalendarPopup(this));
            var popup = new WmsCalendarPopup(this);

            await WmsPopupService.ShowAsync(popup, border);
        }
        private WmsBorder? FindParentBorder()
        {
            Element? parent = Parent;

            while (parent != null)
            {
                if (parent is WmsBorder border)
                    return border;

                parent = parent.Parent;
            }

            return null;
        }

        //public void RaiseFocused()
        //{
        //    if (_isFocused)
        //        return;

        //    _isFocused = true;

        //    Focused?.Invoke(this, EventArgs.Empty);
        //}

        //public void RaiseUnfocused()
        //{
        //    if (!_isFocused)
        //        return;

        //    _isFocused = false;

        //    Unfocused?.Invoke(this, EventArgs.Empty);
        //}
        private void UpdateIcon()
        {
            PART_Icon.Source = Icon;
        }
        private void UpdateClearButton()
        {
            PART_Clear.IsVisible =
                AllowClear &&
                Date.HasValue;
        }
        internal void ClearDate()
        {
            Date = null;

            //RaiseUnfocused();
        }
        internal bool HasTime => !string.IsNullOrWhiteSpace(Format) && (Format.Contains("H") || Format.Contains("h"));

        internal bool HasSecond => !string.IsNullOrWhiteSpace(Format) && Format.Contains("ss");
    }
}
