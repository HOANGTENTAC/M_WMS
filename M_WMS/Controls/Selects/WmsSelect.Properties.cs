using M_WMS.Controls.Enums;
using System.Collections;
using System.Collections.ObjectModel;

namespace M_WMS.Controls.Selects
{
    public partial class WmsSelect
    {
        private readonly ObservableCollection<object> _items = new();

        public IList Items => _items;
        protected override void OnPropertyChanged(string? propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == nameof(IsEnabled))
            {
                SetState(IsEnabled
                    ? WmsSelectState.Normal
                    : WmsSelectState.Disabled);
            }

            if (propertyName == nameof(IsReadOnly))
            {
                SetState(IsReadOnly
                    ? WmsSelectState.ReadOnly
                    : WmsSelectState.Normal);
            }
        }
        #region Placeholder
        public static readonly BindableProperty PlaceholderProperty =
        BindableProperty.Create(
            nameof(Placeholder),
            typeof(string),
            typeof(WmsSelect),
            "Select...",
            propertyChanged: OnPlaceholderChanged);

        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }
        private static void OnPlaceholderChanged(
        BindableObject bindable,
        object oldValue,
        object newValue)
        {
            ((WmsSelect)bindable).UpdateDisplay();
        }
        #endregion
        #region PlaceholderColor
        public static readonly BindableProperty PlaceholderColorProperty =
        BindableProperty.Create(
        nameof(PlaceholderColor),
        typeof(Color),
        typeof(WmsSelect),
        Colors.Gray,
        propertyChanged: OnPlaceholderColorChanged);

        public Color PlaceholderColor
        {
            get => (Color)GetValue(PlaceholderColorProperty);
            set => SetValue(PlaceholderColorProperty, value);
        }
        private static void OnPlaceholderColorChanged(
        BindableObject bindable,
        object oldValue,
        object newValue)
        {
            ((WmsSelect)bindable).UpdateDisplay();
        }
        #endregion
        #region DisplayText
        public static readonly BindableProperty DisplayTextProperty =
            BindableProperty.Create(
            nameof(DisplayText),
            typeof(string),
            typeof(WmsSelect),
            string.Empty,
            propertyChanged: OnDisplayTextChanged);

        public string DisplayText
        {
            get => (string)GetValue(DisplayTextProperty);
            set => SetValue(DisplayTextProperty, value);
        }
        private static void OnDisplayTextChanged(
            BindableObject bindable,
            object oldValue,
            object newValue)
        {
            ((WmsSelect)bindable).UpdateDisplay();
        }
        #endregion
        #region DisplayTextColor
        public static readonly BindableProperty DisplayTextColorProperty =
        BindableProperty.Create(
        nameof(DisplayTextColor),
        typeof(Color),
        typeof(WmsSelect),
        Colors.Black,
        propertyChanged: OnDisplayTextColorChanged);

        public Color DisplayTextColor
        {
            get => (Color)GetValue(DisplayTextColorProperty);
            set => SetValue(DisplayTextColorProperty, value);
        }
        private static void OnDisplayTextColorChanged(
        BindableObject bindable,
        object oldValue,
        object newValue)
        {
            ((WmsSelect)bindable).ApplyDisplay();
        }
        #endregion
        //#region TextColor
        //public static readonly BindableProperty TextColorProperty =
        //    BindableProperty.Create(
        //        nameof(TextColor),
        //        typeof(Color),
        //        typeof(WmsSelect),
        //        Colors.Black,
        //        propertyChanged: OnTextColorChanged);

        //public new Color TextColor
        //{
        //    get => (Color)GetValue(TextColorProperty);
        //    set => SetValue(TextColorProperty, value);
        //}
        //private static void OnTextColorChanged(
        //BindableObject bindable,
        //object oldValue,
        //object newValue)
        //{
        //    ((WmsSelect)bindable).ApplyDisplay();
        //}
        //#endregion
        #region FontSize
        public static readonly BindableProperty FontSizeProperty =
        BindableProperty.Create(
        nameof(FontSize),
        typeof(double),
        typeof(WmsSelect),
        14d,
        propertyChanged: OnFontChanged);

        public double FontSize
        {
            get => (double)GetValue(FontSizeProperty);
            set => SetValue(FontSizeProperty, value);
        }
        public static readonly BindableProperty FontAttributesProperty =
        BindableProperty.Create(
        nameof(FontAttributes),
        typeof(FontAttributes),
        typeof(WmsSelect),
        FontAttributes.None,
        propertyChanged: OnFontChanged);

        public FontAttributes FontAttributes
        {
            get => (FontAttributes)GetValue(FontAttributesProperty);
            set => SetValue(FontAttributesProperty, value);
        }
        private static void OnFontChanged(
        BindableObject bindable,
        object oldValue,
        object newValue)
        {
            ((WmsSelect)bindable).ApplyFont();
        }
        private void ApplyFont()
        {
            PART_Display.FontSize = FontSize;

            PART_Display.FontAttributes = FontAttributes;
        }
        #endregion
        //#region Border
        //public static readonly BindableProperty BorderBrushProperty =
        //BindableProperty.Create(
        //nameof(BorderBrush),
        //typeof(Brush),
        //typeof(WmsSelect),
        //new SolidColorBrush(Color.FromArgb("#C8C8C8")),
        //propertyChanged: OnBorderChanged);

        //public Brush BorderBrush
        //{
        //    get => (Brush)GetValue(BorderBrushProperty);
        //    set => SetValue(BorderBrushProperty, value);
        //}
        //public static readonly BindableProperty BorderThicknessProperty =
        //BindableProperty.Create(
        //nameof(BorderThickness),
        //typeof(double),
        //typeof(WmsSelect),
        //1d,
        //propertyChanged: OnBorderChanged);

        //public double BorderThickness
        //{
        //    get => (double)GetValue(BorderThicknessProperty);
        //    set => SetValue(BorderThicknessProperty, value);
        //}
        //public static readonly BindableProperty CornerRadiusProperty =
        //BindableProperty.Create(
        //nameof(CornerRadius),
        //typeof(double),
        //typeof(WmsSelect),
        //0d,
        //propertyChanged: OnCornerRadiusChanged);

        //public double CornerRadius
        //{
        //    get => (double)GetValue(CornerRadiusProperty);
        //    set => SetValue(CornerRadiusProperty, value);
        //}
        //private static void OnBorderChanged(
        //BindableObject bindable,
        //object oldValue,
        //object newValue)
        //{
        //    ((WmsSelect)bindable).ApplyBorder();
        //}

        //private static void OnCornerRadiusChanged(
        //    BindableObject bindable,
        //    object oldValue,
        //    object newValue)
        //{
        //    ((WmsSelect)bindable).ApplyCornerRadius();
        //}
        //#endregion
        #region BackgroundBrush
        public static readonly BindableProperty BackgroundBrushProperty =
        BindableProperty.Create(
        nameof(BackgroundBrush),
        typeof(Brush),
        typeof(WmsSelect),
        Brush.White,
        propertyChanged: OnBackgroundChanged);

        public Brush BackgroundBrush
        {
            get => (Brush)GetValue(BackgroundBrushProperty);
            set => SetValue(BackgroundBrushProperty, value);
        }
        private static void OnBackgroundChanged(
        BindableObject bindable,
        object oldValue,
        object newValue)
        {
            ((WmsSelect)bindable).ApplyBackground();
        }
        #endregion
        #region Padding
        public static readonly BindableProperty ContentPaddingProperty =
        BindableProperty.Create(
        nameof(ContentPadding),
        typeof(Thickness),
        typeof(WmsSelect),
        new Thickness(12, 8),
        propertyChanged: OnContentPaddingChanged);

        public Thickness ContentPadding
        {
            get => (Thickness)GetValue(ContentPaddingProperty);
            set => SetValue(ContentPaddingProperty, value);
        }
        private static void OnContentPaddingChanged(
        BindableObject bindable,
        object oldValue,
        object newValue)
        {
            ((WmsSelect)bindable).ApplyContentPadding();
        }
        #endregion
        #region Arrow
        /// <summary>
        /// ArrowSource
        /// </summary>
        public static readonly BindableProperty ArrowSourceProperty =
        BindableProperty.Create(
        nameof(ArrowSource),
        typeof(ImageSource),
        typeof(WmsSelect),
        defaultValue: null,
        propertyChanged: OnArrowChanged);

        public ImageSource ArrowSource
        {
            get => (ImageSource)GetValue(ArrowSourceProperty);
            set => SetValue(ArrowSourceProperty, value);
        }
        /// <summary>
        /// ArrowWidth
        /// </summary>
        public static readonly BindableProperty ArrowWidthProperty =
        BindableProperty.Create(
        nameof(ArrowWidth),
        typeof(double),
        typeof(WmsSelect),
        14d,
        propertyChanged: OnArrowChanged);

        public double ArrowWidth
        {
            get => (double)GetValue(ArrowWidthProperty);
            set => SetValue(ArrowWidthProperty, value);
        }
        /// <summary>
        /// ArrowHeight
        /// </summary>
        public static readonly BindableProperty ArrowHeightProperty =
        BindableProperty.Create(
        nameof(ArrowHeight),
        typeof(double),
        typeof(WmsSelect),
        14d,
        propertyChanged: OnArrowChanged);

        public double ArrowHeight
        {
            get => (double)GetValue(ArrowHeightProperty);
            set => SetValue(ArrowHeightProperty, value);
        }
        /// <summary>
        /// ArrowMargin
        /// </summary>
        public static readonly BindableProperty ArrowMarginProperty =
        BindableProperty.Create(
        nameof(ArrowMargin),
        typeof(Thickness),
        typeof(WmsSelect),
        new Thickness(10, 0, 0, 0),
        propertyChanged: OnArrowChanged);

        public Thickness ArrowMargin
        {
            get => (Thickness)GetValue(ArrowMarginProperty);
            set => SetValue(ArrowMarginProperty, value);
        }
        private static void OnArrowChanged(
        BindableObject bindable,
        object oldValue,
        object newValue)
        {
            ((WmsSelect)bindable).ApplyArrow();
        }
        /// <summary>
        /// ShowArrow
        /// </summary>
        public static readonly BindableProperty ShowArrowProperty =
        BindableProperty.Create(
        nameof(ShowArrow),
        typeof(bool),
        typeof(WmsSelect),
        true,
        propertyChanged: OnArrowPropertyChanged);

        public bool ShowArrow
        {
            get => (bool)GetValue(ShowArrowProperty);
            set => SetValue(ShowArrowProperty, value);
        }
        private static void OnArrowPropertyChanged(
        BindableObject bindable,
        object oldValue,
        object newValue)
        {
            ((WmsSelect)bindable).UpdateArrow();
        }
        /// <summary>
        /// ArrowMode
        /// </summary>
        public static readonly BindableProperty ArrowModeProperty =
        BindableProperty.Create(
        nameof(ArrowMode),
        typeof(WmsSelectArrowMode),
        typeof(WmsSelect),
        WmsSelectArrowMode.Auto,
        propertyChanged: OnArrowChanged);

        public WmsSelectArrowMode ArrowMode
        {
            get => (WmsSelectArrowMode)GetValue(ArrowModeProperty);
            set => SetValue(ArrowModeProperty, value);
        }
        #endregion
        #region IsReadOnly
        public static readonly BindableProperty IsReadOnlyProperty =
        BindableProperty.Create(
        nameof(IsReadOnly),
        typeof(bool),
        typeof(WmsSelect),
        false);

        public bool IsReadOnly
        {
            get => (bool)GetValue(IsReadOnlyProperty);
            set => SetValue(IsReadOnlyProperty, value);
        }
        #endregion
        #region PressedBackgroundBrush
        public static readonly BindableProperty PressedBackgroundBrushProperty =
        BindableProperty.Create(
        nameof(PressedBackgroundBrush),
        typeof(Brush),
        typeof(WmsSelect),
        new SolidColorBrush(Color.FromArgb("#F0F0F0")));

        public Brush PressedBackgroundBrush
        {
            get => (Brush)GetValue(PressedBackgroundBrushProperty);
            set => SetValue(PressedBackgroundBrushProperty, value);
        }
        #endregion
        #region DisplayMargin
        public static readonly BindableProperty DisplayMarginProperty =
        BindableProperty.Create(
        nameof(DisplayMargin),
        typeof(Thickness),
        typeof(WmsSelect),
        new Thickness(4, 2, 0, 2),
        propertyChanged: OnDisplayMarginChanged);

        public Thickness DisplayMargin
        {
            get => (Thickness)GetValue(DisplayMarginProperty);
            set => SetValue(DisplayMarginProperty, value);
        }
        private static void OnDisplayMarginChanged(
        BindableObject bindable,
        object oldValue,
        object newValue)
        {
            ((WmsSelect)bindable).ApplyDisplay();
        }
        #endregion
        #region ItemsSource
        public static readonly BindableProperty ItemsSourceProperty =
        BindableProperty.Create(
        nameof(ItemsSource),
        typeof(IList),
        typeof(WmsSelect),
        null, propertyChanged: OnItemsSourcePropertyChanged);

        public IList? ItemsSource
        {
            get => (IList?)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }
        private static void OnItemsSourcePropertyChanged(
        BindableObject bindable,
        object? oldValue,
        object? newValue)
        {
            var control = (WmsSelect)bindable;

            control.UpdateDisplay();
            control.ApplySelectedIndex();
        }
        #endregion
        #region SelectedItem
        public static readonly BindableProperty SelectedItemProperty =
        BindableProperty.Create(
        nameof(SelectedItem),
        typeof(object),
        typeof(WmsSelect),
        null,
        propertyChanged: OnSelectedItemPropertyChanged);

        private static void OnSelectedItemPropertyChanged(
            BindableObject bindable,
            object oldValue,
            object newValue)
        {
            var control = (WmsSelect)bindable;

            control.ApplySelectedItem();
        }
        public object? SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }
        #endregion
        #region SelectedIndex
        public static readonly BindableProperty SelectedIndexProperty =
        BindableProperty.Create(
        nameof(SelectedIndex),
        typeof(int),
        typeof(WmsSelect),
        -1,
        BindingMode.TwoWay,
        propertyChanged: OnSelectedIndexPropertyChanged);

        public int SelectedIndex
        {
            get => (int)GetValue(SelectedIndexProperty);
            set => SetValue(SelectedIndexProperty, value);
        }

        private static void OnSelectedIndexPropertyChanged(
            BindableObject bindable,
            object oldValue,
            object newValue)
        {
            var control = (WmsSelect)bindable;

            control.ApplySelectedIndex();
        }
        #endregion
        #region Title
        public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(
        nameof(Title),
        typeof(string),
        typeof(WmsSelect),
        "Select",
        propertyChanged: OnTitleChanged);

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        private static void OnTitleChanged(
            BindableObject bindable,
            object oldValue,
            object newValue)
        {
        }
        #endregion
        #region SearchPlaceholder
        public static readonly BindableProperty SearchPlaceholderProperty =
        BindableProperty.Create(
        nameof(SearchPlaceholder),
        typeof(string),
        typeof(WmsSelect),
        "Select",
        propertyChanged: OnSearchPlaceholderChanged);

        public string SearchPlaceholder
        {
            get => (string)GetValue(SearchPlaceholderProperty);
            set => SetValue(SearchPlaceholderProperty, value);
        }

        private static void OnSearchPlaceholderChanged(
            BindableObject bindable,
            object oldValue,
            object newValue)
        {
        }
        #endregion
        #region DisplayMemberPath
        public static readonly BindableProperty DisplayMemberPathProperty =
    BindableProperty.Create(
        nameof(DisplayMemberPath),
        typeof(string),
        typeof(WmsSelect),
        string.Empty,
        propertyChanged: OnDisplayMemberPathChanged);

        public string DisplayMemberPath
        {
            get => (string)GetValue(DisplayMemberPathProperty);
            set => SetValue(DisplayMemberPathProperty, value);
        }

        private static void OnDisplayMemberPathChanged(
            BindableObject bindable,
            object oldValue,
            object newValue)
        {
            var control = (WmsSelect)bindable;

            control.ApplyDisplayMemberPath();
        }
        #endregion
        #region ItemTemplate
        public static readonly BindableProperty ItemTemplateProperty =
            BindableProperty.Create(
                nameof(ItemTemplate),
                typeof(DataTemplate),
                typeof(WmsSelect));

        public DataTemplate? ItemTemplate
        {
            get => (DataTemplate?)GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }
        #endregion

        public static readonly BindableProperty EmptyTextProperty =
        BindableProperty.Create(
        nameof(EmptyText),
        typeof(string),
        typeof(WmsSelect),
        "No data found");

        public string EmptyText
        {
            get => (string)GetValue(EmptyTextProperty);
            set => SetValue(EmptyTextProperty, value);
        }
        public static readonly BindableProperty SelectedValuePathProperty =
        BindableProperty.Create(
        nameof(SelectedValuePath),
        typeof(string),
        typeof(WmsSelect));

        public string? SelectedValuePath
        {
            get => (string?)GetValue(SelectedValuePathProperty);
            set => SetValue(SelectedValuePathProperty, value);
        }
        public static readonly BindableProperty SelectedValueProperty =
        BindableProperty.Create(
        nameof(SelectedValue),
        typeof(object),
        typeof(WmsSelect),
        default,
        BindingMode.TwoWay,
        propertyChanged: OnSelectedValueChanged);

        public object? SelectedValue
        {
            get => GetValue(SelectedValueProperty);
            set => SetValue(SelectedValueProperty, value);
        }
        private static void OnSelectedValueChanged(
        BindableObject bindable,
        object oldValue,
        object newValue)
        {
            ((WmsSelect)bindable).UpdateSelectedItemByValue();
        }
        //public static readonly BindableProperty IsSearchVisibleProperty =
        //BindableProperty.Create(
        //nameof(IsSearchVisible),
        //typeof(bool),
        //typeof(WmsSelect),
        //true,
        //propertyChanged: OnIsSearchVisibleChanged);

        //public bool IsSearchVisible
        //{
        //    get => (bool)GetValue(IsSearchVisibleProperty);
        //    set => SetValue(IsSearchVisibleProperty, value);
        //}

        //private static void OnIsSearchVisibleChanged(
        //    BindableObject bindable,
        //    object oldValue,
        //    object newValue)
        //{
        //    if (bindable is WmsSelect select)
        //    {
        //        select.UpdateSearchVisibility();
        //    }
        //}
        #region Leading Icon

        public static readonly BindableProperty ShowLeadingIconProperty =
            BindableProperty.Create(
                nameof(ShowLeadingIcon),
                typeof(bool),
                typeof(WmsSelect),
                false,
                propertyChanged: OnLeadingIconChanged);

        public bool ShowLeadingIcon
        {
            get => (bool)GetValue(ShowLeadingIconProperty);
            set => SetValue(ShowLeadingIconProperty, value);
        }

        public static readonly BindableProperty LeadingIconSourceProperty =
            BindableProperty.Create(
                nameof(LeadingIconSource),
                typeof(ImageSource),
                typeof(WmsSelect),
                default(ImageSource),
                propertyChanged: OnLeadingIconChanged);

        public ImageSource? LeadingIconSource
        {
            get => (ImageSource?)GetValue(LeadingIconSourceProperty);
            set => SetValue(LeadingIconSourceProperty, value);
        }

        public static readonly BindableProperty LeadingFontAwesomeIconProperty =
            BindableProperty.Create(
                nameof(LeadingFontAwesomeIcon),
                typeof(string),
                typeof(WmsSelect),
                string.Empty,
                propertyChanged: OnLeadingIconChanged);

        public string LeadingFontAwesomeIcon
        {
            get => (string)GetValue(LeadingFontAwesomeIconProperty);
            set => SetValue(LeadingFontAwesomeIconProperty, value);
        }

        public static readonly BindableProperty LeadingFontAwesomeIconColorProperty =
            BindableProperty.Create(
                nameof(LeadingFontAwesomeIconColor),
                typeof(Color),
                typeof(WmsSelect),
                Colors.Black,
                propertyChanged: OnLeadingIconChanged);

        public Color LeadingFontAwesomeIconColor
        {
            get => (Color)GetValue(LeadingFontAwesomeIconColorProperty);
            set => SetValue(LeadingFontAwesomeIconColorProperty, value);
        }

        public static readonly BindableProperty LeadingIconSizeProperty =
            BindableProperty.Create(
                nameof(LeadingIconSize),
                typeof(double),
                typeof(WmsSelect),
                18d,
                propertyChanged: OnLeadingIconChanged);

        public double LeadingIconSize
        {
            get => (double)GetValue(LeadingIconSizeProperty);
            set => SetValue(LeadingIconSizeProperty, value);
        }

        private static void OnLeadingIconChanged(
            BindableObject bindable,
            object oldValue,
            object newValue)
        {
            if (bindable is WmsSelect select)
            {
                select.UpdateLeadingIcon();
            }
        }
        public static readonly BindableProperty LeadingIconSpacingProperty =
            BindableProperty.Create(
            nameof(LeadingIconSpacing),
            typeof(double),
            typeof(WmsSelect),
            8d,
            propertyChanged: OnLeadingIconChanged);

        public double LeadingIconSpacing
        {
            get => (double)GetValue(LeadingIconSpacingProperty);
            set => SetValue(LeadingIconSpacingProperty, value);
        }
        #endregion
    }
}
