using Microsoft.Maui.Controls;

namespace M_WMS.Controls.Borders
{
    public partial class WmsBorder
    {
        #region Border Thickness

        public static readonly BindableProperty BorderTopThicknessProperty =
            BindableProperty.Create(
                nameof(BorderTopThickness),
                typeof(double),
                typeof(WmsBorder),
                0d,
                propertyChanged: OnBorderPropertyChanged);

        public double BorderTopThickness
        {
            get => (double)GetValue(BorderTopThicknessProperty);
            set => SetValue(BorderTopThicknessProperty, value);
        }

        public static readonly BindableProperty BorderRightThicknessProperty =
            BindableProperty.Create(
                nameof(BorderRightThickness),
                typeof(double),
                typeof(WmsBorder),
                0d,
                propertyChanged: OnBorderPropertyChanged);

        public double BorderRightThickness
        {
            get => (double)GetValue(BorderRightThicknessProperty);
            set => SetValue(BorderRightThicknessProperty, value);
        }

        public static readonly BindableProperty BorderBottomThicknessProperty =
            BindableProperty.Create(
                nameof(BorderBottomThickness),
                typeof(double),
                typeof(WmsBorder),
                0d,
                propertyChanged: OnBorderPropertyChanged);

        public double BorderBottomThickness
        {
            get => (double)GetValue(BorderBottomThicknessProperty);
            set => SetValue(BorderBottomThicknessProperty, value);
        }

        public static readonly BindableProperty BorderLeftThicknessProperty =
            BindableProperty.Create(
                nameof(BorderLeftThickness),
                typeof(double),
                typeof(WmsBorder),
                0d,
                propertyChanged: OnBorderPropertyChanged);

        public double BorderLeftThickness
        {
            get => (double)GetValue(BorderLeftThicknessProperty);
            set => SetValue(BorderLeftThicknessProperty, value);
        }

        #endregion

        #region Border Brush

        public static readonly BindableProperty BorderTopBrushProperty =
            BindableProperty.Create(
                nameof(BorderTopBrush),
                typeof(Brush),
                typeof(WmsBorder),
                Brush.Gray,
                propertyChanged: OnBorderPropertyChanged);

        public Brush BorderTopBrush
        {
            get => (Brush)GetValue(BorderTopBrushProperty);
            set => SetValue(BorderTopBrushProperty, value);
        }

        public static readonly BindableProperty BorderRightBrushProperty =
            BindableProperty.Create(
                nameof(BorderRightBrush),
                typeof(Brush),
                typeof(WmsBorder),
                Brush.Gray,
                propertyChanged: OnBorderPropertyChanged);

        public Brush BorderRightBrush
        {
            get => (Brush)GetValue(BorderRightBrushProperty);
            set => SetValue(BorderRightBrushProperty, value);
        }

        public static readonly BindableProperty BorderBottomBrushProperty =
            BindableProperty.Create(
                nameof(BorderBottomBrush),
                typeof(Brush),
                typeof(WmsBorder),
                Brush.Gray,
                propertyChanged: OnBorderPropertyChanged);

        public Brush BorderBottomBrush
        {
            get => (Brush)GetValue(BorderBottomBrushProperty);
            set => SetValue(BorderBottomBrushProperty, value);
        }

        public static readonly BindableProperty BorderLeftBrushProperty =
            BindableProperty.Create(
                nameof(BorderLeftBrush),
                typeof(Brush),
                typeof(WmsBorder),
                Brush.Gray,
                propertyChanged: OnBorderPropertyChanged);

        public Brush BorderLeftBrush
        {
            get => (Brush)GetValue(BorderLeftBrushProperty);
            set => SetValue(BorderLeftBrushProperty, value);
        }

        #endregion

        private static void OnBorderPropertyChanged(
            BindableObject bindable,
            object oldValue,
            object newValue)
        {
            ((WmsBorder)bindable).UpdateBorder();
        }

        //#region Content
        //public new static readonly BindableProperty ContentProperty =
        //BindableProperty.Create(
        //nameof(Content),
        //typeof(View),
        //typeof(WmsBorder),
        //null,
        //propertyChanged: OnContentChanged);

        //public new View? Content
        //{
        //    get => (View?)GetValue(ContentProperty);
        //    set => SetValue(ContentProperty, value);
        //}

        //private static void OnContentChanged(
        //BindableObject bindable,
        //object? oldValue,
        //object? newValue)
        //{
        //    var control = (WmsBorder)bindable;

        //    if (control.PART_ContentHost == null)
        //    {
        //        control._pendingContent = newValue as View;
        //        return;
        //    }

        //    control.PART_ContentHost.Content = newValue as View;
        //}
        //#endregion
        #region Child
        public static readonly BindableProperty ChildProperty =
        BindableProperty.Create(
        nameof(Child),
        typeof(View),
        typeof(WmsBorder),
        null,
        propertyChanged: OnChildChanged);

        public View? Child
        {
            get => (View?)GetValue(ChildProperty);
            set => SetValue(ChildProperty, value);
        }

        private static void OnChildChanged(
            BindableObject bindable,
            object? oldValue,
            object? newValue)
        {
            var control = (WmsBorder)bindable;

            control.DetachEvents(oldValue as View);

            control.PART_ContentHost.Content = newValue as View;

            control.AttachEvents(newValue as View);
        }
        #endregion
        #region CornerRadius

        public static readonly BindableProperty CornerRadiusProperty =
        BindableProperty.Create(
        nameof(CornerRadius),
        typeof(CornerRadius),
        typeof(WmsBorder),
        new CornerRadius(0),
        propertyChanged: OnBorderPropertyChanged);

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        #endregion
        //#region BackgroundBrush

        //public static readonly BindableProperty BackgroundBrushProperty =
        //    BindableProperty.Create(
        //        nameof(BackgroundBrush),
        //        typeof(Brush),
        //        typeof(WmsBorder),
        //        Brush.Transparent,
        //        propertyChanged: OnBorderPropertyChanged);

        //public Brush BackgroundBrush
        //{
        //    get => (Brush)GetValue(BackgroundBrushProperty);
        //    set => SetValue(BackgroundBrushProperty, value);
        //}

        //#endregion
        //#region Padding

        //public static readonly BindableProperty BorderPaddingProperty =
        //    BindableProperty.Create(
        //        nameof(BorderPadding),
        //        typeof(Thickness),
        //        typeof(WmsBorder),
        //        new Thickness(0),
        //        propertyChanged: OnBorderPropertyChanged);

        //public Thickness BorderPadding
        //{
        //    get => (Thickness)GetValue(BorderPaddingProperty);
        //    set => SetValue(BorderPaddingProperty, value);
        //}

        //#endregion
        #region Border

        public static readonly BindableProperty BorderThicknessProperty =
            BindableProperty.Create(
                nameof(BorderThickness),
                typeof(double),
                typeof(WmsBorder),
                0d,
                propertyChanged: OnBorderPropertyChanged);

        public double BorderThickness
        {
            get => (double)GetValue(BorderThicknessProperty);
            set => SetValue(BorderThicknessProperty, value);
        }

        public static readonly BindableProperty BorderBrushProperty =
            BindableProperty.Create(
                nameof(BorderBrush),
                typeof(Brush),
                typeof(WmsBorder),
                Brush.Gray,
                propertyChanged: OnBorderPropertyChanged);

        public Brush BorderBrush
        {
            get => (Brush)GetValue(BorderBrushProperty);
            set => SetValue(BorderBrushProperty, value);
        }

        #endregion

        #region FocusedBorderBottomBrush
        public static readonly BindableProperty FocusedBorderBottomBrushProperty =
        BindableProperty.Create(
        nameof(FocusedBorderBottomBrush),
        typeof(Brush),
        typeof(WmsBorder),
        new SolidColorBrush(Colors.DodgerBlue));

        public Brush FocusedBorderBottomBrush
        {
            get => (Brush)GetValue(FocusedBorderBottomBrushProperty);
            set => SetValue(FocusedBorderBottomBrushProperty, value);
        }
        #endregion

        #region NormalBorderBottomBrush
        public static readonly BindableProperty NormalBorderBottomBrushProperty =
        BindableProperty.Create(
        nameof(NormalBorderBottomBrush),
        typeof(Brush),
        typeof(WmsBorder),
        new SolidColorBrush(Color.FromArgb("#DADADA")));

        public Brush NormalBorderBottomBrush
        {
            get => (Brush)GetValue(NormalBorderBottomBrushProperty);
            set => SetValue(NormalBorderBottomBrushProperty, value);
        }
        #endregion
    }
}
