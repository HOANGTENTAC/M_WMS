using Microsoft.Maui.Controls.Shapes;

namespace M_WMS.Controls.Borders
{
    public partial class WmsBorder
    {
        private void UpdateBorder()
        {
            PART_BackgroundBorder.Background = Background;

            PART_BackgroundBorder.Padding = Padding;

            PART_BackgroundBorder.StrokeShape =
                new RoundRectangle
                {
                    CornerRadius = CornerRadius
                };

            UpdateBorderPart(new BorderPart
            {
                View = PART_Top,
                Horizontal = true,
                Thickness = BorderTopThickness > 0 ? BorderTopThickness : BorderThickness,
                Brush = BorderTopBrush ?? BorderBrush
            });

            UpdateBorderPart(new BorderPart
            {
                View = PART_Right,
                Horizontal = false,
                Thickness = BorderRightThickness > 0 ? BorderRightThickness : BorderThickness,
                Brush = BorderRightBrush ?? BorderBrush
            });

            UpdateBorderPart(new BorderPart
            {
                View = PART_Bottom,
                Horizontal = true,
                Thickness = BorderBottomThickness > 0 ? BorderBottomThickness : BorderThickness,
                Brush = BorderBottomBrush ?? BorderBrush
            });

            UpdateBorderPart(new BorderPart
            {
                View = PART_Left,
                Horizontal = false,
                Thickness = BorderLeftThickness > 0 ? BorderLeftThickness : BorderThickness,
                Brush = BorderLeftBrush ?? BorderBrush
            });
        }
        private void UpdateTopBorder()
        {
            PART_Top.HeightRequest = BorderTopThickness;
            PART_Top.Background = BorderTopBrush;
        }

        private void UpdateRightBorder()
        {
            PART_Right.WidthRequest = BorderRightThickness;
            PART_Right.Background = BorderRightBrush;
        }

        private void UpdateBottomBorder()
        {
            PART_Bottom.HeightRequest = BorderBottomThickness;
            PART_Bottom.Background = BorderBottomBrush;
        }

        private void UpdateLeftBorder()
        {
            PART_Left.WidthRequest = BorderLeftThickness;
            PART_Left.Background = BorderLeftBrush;
        }
        private void UpdateBorderPart(BorderPart part)
        {
            if (part.Horizontal)
            {
                part.View.HeightRequest = part.Thickness;
            }
            else
            {
                part.View.WidthRequest = part.Thickness;
            }

            part.View.Background = part.Brush;

            part.View.IsVisible = part.Thickness > 0;
        }
        internal void SetFocused()
        {
            //BorderBottomBrush = FocusedBorderBottomBrush;
            PART_Bottom.Background = FocusedBorderBottomBrush;
        }

        internal void SetNormal()
        {
            //BorderBottomBrush = NormalBorderBottomBrush;
            PART_Bottom.Background = BorderBottomBrush ?? BorderBrush;
        }
        private void AttachEvents(View? child)
        {
            if (child == null)
                return;

            if (child is VisualElement visual)
            {
                visual.Focused += Child_Focused;
                visual.Unfocused += Child_Unfocused;
            }
            // Custom control
            if (child is IWmsFocusable focusable)
            {
                focusable.Focused += ChildCustom_Focused;
                focusable.Unfocused += ChildCustom_Unfocused;
            }
        }

        private void DetachEvents(View? child)
        {
            if (child == null)
                return;

            if (child is VisualElement visual)
            {
                visual.Focused -= Child_Focused;
                visual.Unfocused -= Child_Unfocused;
            }
            if (child is IWmsFocusable focusable)
            {
                focusable.Focused -= ChildCustom_Focused;
                focusable.Unfocused -= ChildCustom_Unfocused;
            }
        }

        private void Child_Focused(object? sender, FocusEventArgs e)
        {
            SetFocused();
        }

        private void Child_Unfocused(object? sender, FocusEventArgs e)
        {
            SetNormal();
        }
        private void ChildCustom_Focused(object? sender, EventArgs e)
        {
            SetFocused();
        }

        private void ChildCustom_Unfocused(object? sender, EventArgs e)
        {
            SetNormal();
        }
    }
    public sealed class BorderPart
    {
        public required BoxView View { get; init; }

        public required double Thickness { get; init; }

        public required Brush Brush { get; init; }

        public required bool Horizontal { get; init; }
    }
}
