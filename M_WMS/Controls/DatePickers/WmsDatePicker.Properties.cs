using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M_WMS.Controls.DatePickers
{
    public partial class WmsDatePicker
    {
        #region Date
        public static readonly BindableProperty DateProperty =
            BindableProperty.Create(
                nameof(Date),
                typeof(DateTime?),
                typeof(WmsDatePicker),
                null,
                BindingMode.TwoWay,
                propertyChanged: OnDateChanged);

        public DateTime? Date
        {
            get => (DateTime?)GetValue(DateProperty);
            set => SetValue(DateProperty, value);
        }
        private static void OnDateChanged(
            BindableObject bindable,
            object oldValue,
            object newValue)
        {
            var control = (WmsDatePicker)bindable;

            control.UpdateText();
            control.UpdateClearButton();
            control.DateChanged?.Invoke(
                control,
                new WmsDateChangedEventArgs(
                    oldValue as DateTime?,
                    newValue as DateTime?));
        }
        #endregion

        #region Format
        public static readonly BindableProperty FormatProperty =
        BindableProperty.Create(
        nameof(Format),
        typeof(string),
        typeof(WmsDatePicker),
        "yyyy/MM/dd",
        propertyChanged: OnFormatChanged);

        public string Format
        {
            get => (string)GetValue(FormatProperty);
            set => SetValue(FormatProperty, value);
        }
        private static void OnFormatChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is WmsDatePicker picker)
            {
                picker.UpdateText();
            }
        }
        #endregion

        #region Placeholder
        public static readonly BindableProperty PlaceholderProperty =
        BindableProperty.Create(
        nameof(Placeholder),
        typeof(string),
        typeof(WmsDatePicker),
        "Select date",
        propertyChanged: OnPlaceholderChanged);

        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }
        private static void OnPlaceholderChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is WmsDatePicker picker)
            {
                picker.UpdateText();
            }
        }
        #endregion

        #region Icon
        public static readonly BindableProperty IconProperty =
            BindableProperty.Create(
                nameof(Icon),
                typeof(ImageSource),
                typeof(WmsDatePicker),
                default(ImageSource),
                propertyChanged: OnIconChanged);

        public ImageSource? Icon
        {
            get => (ImageSource?)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }
        private static void OnIconChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((WmsDatePicker)bindable).UpdateIcon();
        }
        #endregion

        #region TextColor
        public static readonly BindableProperty TextColorProperty =
    BindableProperty.Create(
        nameof(TextColor),
        typeof(Color),
        typeof(WmsDatePicker),
        Colors.Black,
        propertyChanged: OnTextStyleChanged);

        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }
        #endregion

        #region PlaceholderColor
        public static readonly BindableProperty PlaceholderColorProperty =
    BindableProperty.Create(
        nameof(PlaceholderColor),
        typeof(Color),
        typeof(WmsDatePicker),
        Colors.Gray,
        propertyChanged: OnTextStyleChanged);

        public Color PlaceholderColor
        {
            get => (Color)GetValue(PlaceholderColorProperty);
            set => SetValue(PlaceholderColorProperty, value);
        }
        #endregion

        #region FontSize
        public static readonly BindableProperty FontSizeProperty =
    BindableProperty.Create(
        nameof(FontSize),
        typeof(double),
        typeof(WmsDatePicker),
        14d,
        propertyChanged: OnFontChanged);

        public new double FontSize
        {
            get => (double)GetValue(FontSizeProperty);
            set => SetValue(FontSizeProperty, value);
        }
        #endregion

        #region FontFamily
        public static readonly BindableProperty FontFamilyProperty =
    BindableProperty.Create(
        nameof(FontFamily),
        typeof(string),
        typeof(WmsDatePicker),
        default(string),
        propertyChanged: OnFontChanged);

        public string? FontFamily
        {
            get => (string?)GetValue(FontFamilyProperty);
            set => SetValue(FontFamilyProperty, value);
        }
        #endregion

        #region FontAttributes
        public static readonly BindableProperty FontAttributesProperty =
    BindableProperty.Create(
        nameof(FontAttributes),
        typeof(FontAttributes),
        typeof(WmsDatePicker),
        FontAttributes.None,
        propertyChanged: OnFontChanged);

        public FontAttributes FontAttributes
        {
            get => (FontAttributes)GetValue(FontAttributesProperty);
            set => SetValue(FontAttributesProperty, value);
        }
        #endregion

        #region AllowClear
        public static readonly BindableProperty AllowClearProperty =
        BindableProperty.Create(
        nameof(AllowClear),
        typeof(bool),
        typeof(WmsDatePicker),
        true);

        public bool AllowClear
        {
            get => (bool)GetValue(AllowClearProperty);
            set => SetValue(AllowClearProperty, value);
        }
        #endregion
        private static void OnFontChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((WmsDatePicker)bindable).UpdateFont();
        }
        private static void OnTextStyleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ((WmsDatePicker)bindable).UpdateTextAppearance();
        }
    }
}
