using M_WMS.Controls.Enums;
using M_WMS.Controls.Helpers;

namespace M_WMS.Controls.Selects
{
    public partial class WmsSelect
    {
        private void InitializeStyle()
        {
            ApplyBackground();
            ApplyDisplay();
            ApplyFont();
            ApplyArrow();
        }

        private void ApplyDisplay()
        {
            PART_Display.Margin = DisplayMargin;

            PART_Display.TextColor = DisplayTextColor;

            PART_Display.VerticalOptions = LayoutOptions.Center;

            PART_Display.LineBreakMode = LineBreakMode.TailTruncation;

            UpdateDisplay();
        }

        private void ApplyArrow()
        {
            switch (ArrowMode)
            {
                case WmsSelectArrowMode.None:

                    PART_Arrow.IsVisible = false;
                    break;

                case WmsSelectArrowMode.Auto:

                    PART_Arrow.IsVisible = true;

                    PART_Arrow.Source =
                        IsPopupOpen
                            ? ImageSource.FromFile("up.png")
                            : ImageSource.FromFile("down.png");

                    break;

                case WmsSelectArrowMode.Custom:

                    PART_Arrow.IsVisible = true;
                    PART_Arrow.Source = ArrowSource;
                    break;
            }
            PART_Arrow.WidthRequest = ArrowWidth;
            PART_Arrow.HeightRequest = ArrowHeight;
            PART_Arrow.Margin = ArrowMargin;
            PART_Arrow.HorizontalOptions = LayoutOptions.End;
            PART_Arrow.VerticalOptions = LayoutOptions.Center;
        }
        private void ApplyBackground()
        {
            PART_Grid.Background = BackgroundBrush;
        }
        private void ApplyContentPadding()
        {
            PART_Grid.Padding = ContentPadding;
        }
        private void ApplyEnabled()
        {
            Opacity = IsEnabled ? 1 : 0.5;
        }
        private void UpdateDisplay()
        {
            if (SelectedItem == null)
            {
                PART_Display.Text = Placeholder;
                PART_Display.TextColor = PlaceholderColor;
                return;
            }

            PART_Display.Text = DisplayHelper.GetDisplayText(SelectedItem, DisplayMemberPath);
            PART_Display.TextColor = DisplayTextColor;
        }
        private void UpdateArrow()
        {
            if (PART_Arrow == null)
                return;

            PART_Arrow.IsVisible = ShowArrow;
            if (ShowArrow)
            {
                PART_Grid.ColumnDefinitions[2].Width = GridLength.Auto;
            }
            else
            {
                PART_Grid.ColumnDefinitions[2].Width = new GridLength(0);
            }
        }
        private void Press()
        {
            _normalBackground ??= BackgroundBrush;

            PART_Grid.Background = PressedBackgroundBrush;
        }
        private void Release()
        {
            PART_Grid.Background = _normalBackground;
        }
        private void UpdateSelectedIndex()
        {
            if (ItemsSource == null)
            {
                SelectedIndex = -1;
                return;
            }

            int index = 0;

            foreach (var item in ItemsSource)
            {
                if (Equals(item, SelectedItem))
                {
                    SelectedIndex = index;
                    return;
                }

                index++;
            }

            SelectedIndex = -1;
        }
        private void UpdateLeadingIcon()
        {
            bool hasFontAwesome =
                ShowLeadingIcon &&
                !string.IsNullOrWhiteSpace(LeadingFontAwesomeIcon);

            bool hasImage =
                ShowLeadingIcon &&
                LeadingIconSource != null &&
                !hasFontAwesome;

            PART_LeadingIcon.Source = LeadingIconSource;
            PART_LeadingIcon.WidthRequest = LeadingIconSize;
            PART_LeadingIcon.HeightRequest = LeadingIconSize;
            PART_LeadingIcon.IsVisible = hasImage;

            PART_LeadingFontAwesomeIcon.Text =
                LeadingFontAwesomeIcon;

            PART_LeadingFontAwesomeIcon.FontSize =
                LeadingIconSize;

            PART_LeadingFontAwesomeIcon.TextColor =
                LeadingFontAwesomeIconColor;

            PART_LeadingFontAwesomeIcon.WidthRequest =
                LeadingIconSize;

            PART_LeadingFontAwesomeIcon.HeightRequest =
                LeadingIconSize;

            PART_LeadingFontAwesomeIcon.IsVisible =
                hasFontAwesome;

            UpdateDisplayMargin();
        }
        private void UpdateDisplayMargin()
        {
            PART_Display.Margin =
                ShowLeadingIcon
                    ? new Thickness(LeadingIconSpacing, 0, 0, 0)
                    : Thickness.Zero;
        }
    }
}
