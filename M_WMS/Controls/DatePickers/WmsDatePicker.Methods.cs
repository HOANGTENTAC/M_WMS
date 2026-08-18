using M_WMS.Controls.Popups;
using M_WMS.Services;

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
            RaiseFocused();

            Clicked?.Invoke(this, EventArgs.Empty);
        }

        private async void OnTapped(object? sender, TappedEventArgs e)
        {
            RaiseFocused();

            //Clicked?.Invoke(this, EventArgs.Empty);
            await WmsPopupService.ShowAsync(new WmsCalendarPopup(this));
        }

        public void RaiseFocused()
        {
            if (_isFocused)
                return;

            _isFocused = true;

            Focused?.Invoke(this, EventArgs.Empty);
        }

        public void RaiseUnfocused()
        {
            if (!_isFocused)
                return;

            _isFocused = false;

            Unfocused?.Invoke(this, EventArgs.Empty);
        }
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

            RaiseUnfocused();
        }
        internal bool HasTime => !string.IsNullOrWhiteSpace(Format) && (Format.Contains("H") || Format.Contains("h"));

        internal bool HasSecond => !string.IsNullOrWhiteSpace(Format) && Format.Contains("ss");
    }
}
