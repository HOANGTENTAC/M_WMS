using Microsoft.Maui.Controls.Shapes;

namespace M_WMS.Controls.Pickers
{
    public partial class WmsPicker
    {
        private void InitializeStyle()
        {
            ApplyBorder();
            ApplyCornerRadius();
            ApplyPicker();
            ApplyDropDownIcon();
            ApplyTextColor();
        }
        private void ApplyBorder()
        {
            PART_Border.Stroke = Colors.DodgerBlue;
            PART_Border.StrokeThickness = 1;
        }

        private void ApplyCornerRadius()
        {
            PART_Border.StrokeShape = new RoundRectangle
            {
                CornerRadius = new CornerRadius(12)
            };
        }

        private void ApplyPicker()
        {
            //PART_Picker.Opacity = 0.01;
            PART_Picker.Margin = 0;

            PART_Picker.TextColor = Colors.Black;

            PART_Picker.HorizontalOptions = LayoutOptions.Fill;

            PART_Picker.VerticalOptions = LayoutOptions.Fill;
            //PART_Picker.ZIndex = 0;

            //PART_Picker.BackgroundColor = Colors.Transparent;
            //PART_Picker.Title = Placeholder;
        }

        private void ApplyDropDownIcon()
        {
            PART_DropDownIcon.Source = "down.png";

            PART_DropDownIcon.WidthRequest = 14;

            PART_DropDownIcon.HeightRequest = 14;

            PART_DropDownIcon.HorizontalOptions = LayoutOptions.End;

            PART_DropDownIcon.VerticalOptions = LayoutOptions.Center;

            PART_DropDownIcon.Margin = new Thickness(0, 0, 5, 0);

            PART_DropDownIcon.InputTransparent = true;
            PART_DropDownIcon.ZIndex = 1;
        }
    }
}
