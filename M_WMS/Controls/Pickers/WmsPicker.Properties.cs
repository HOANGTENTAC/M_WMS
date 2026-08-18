using System.Collections;

namespace M_WMS.Controls.Pickers
{
    public partial class WmsPicker
    {
        #region ItemsSource
        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create(
                nameof(ItemsSource),
                typeof(IList),
                typeof(WmsPicker),
                default(IList),
                propertyChanged: OnItemsSourceChanged);

        public IList ItemsSource
        {
            get => (IList)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        private static void OnItemsSourceChanged(
            BindableObject bindable,
            object oldValue,
            object newValue)
        {
            if (bindable is WmsPicker picker)
            {
                picker.ApplyItemsSource();
            }
        }
        #endregion
        
        #region SelectedIndex
        public static readonly BindableProperty SelectedIndexProperty =
            BindableProperty.Create(
        nameof(SelectedIndex),
        typeof(int),
        typeof(WmsPicker),
        -1,
        BindingMode.TwoWay,
        propertyChanged: OnSelectedIndexChanged);

        public int SelectedIndex
        {
            get => (int)GetValue(SelectedIndexProperty);
            set => SetValue(SelectedIndexProperty, value);
        }

        private static void OnSelectedIndexChanged(
            BindableObject bindable,
            object oldValue,
            object newValue)
        {
            if (bindable is WmsPicker picker)
            {
                picker.ApplySelectedIndex();
            }
        }
        #endregion
        //#region Placeholder
        //public static readonly BindableProperty PlaceholderProperty =
        //    BindableProperty.Create(
        //nameof(Placeholder),
        //typeof(string),
        //typeof(WmsPicker),
        //string.Empty,
        //propertyChanged: OnPlaceholderChanged);

        //public string Placeholder
        //{
        //    get => (string)GetValue(PlaceholderProperty);
        //    set => SetValue(PlaceholderProperty, value);
        //}

        //private static void OnPlaceholderChanged(
        //    BindableObject bindable,
        //    object oldValue,
        //    object newValue)
        //{
        //    if (bindable is WmsPicker picker)
        //    {
        //        picker.ApplyPlaceholder();
        //    }
        //}
        //#endregion
        #region SelectedItem
        public static readonly BindableProperty SelectedItemProperty =
        BindableProperty.Create(
        nameof(SelectedItem),
        typeof(object),
        typeof(WmsPicker),
        null,
        BindingMode.TwoWay,
        propertyChanged: OnSelectedItemChanged);

        public object? SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        private static void OnSelectedItemChanged(
            BindableObject bindable,
            object oldValue,
            object newValue)
        {
            if (bindable is WmsPicker picker)
            {
                picker.ApplySelectedItem();
            }
        }
        #endregion
        #region ItemDisplayBinding
        public static readonly BindableProperty ItemDisplayBindingProperty =
        BindableProperty.Create(
        nameof(ItemDisplayBinding),
        typeof(BindingBase),
        typeof(WmsPicker),
        default(BindingBase),
        propertyChanged: OnItemDisplayBindingChanged);

        public BindingBase? ItemDisplayBinding
        {
            get => (BindingBase?)GetValue(ItemDisplayBindingProperty);
            set => SetValue(ItemDisplayBindingProperty, value);
        }

        private static void OnItemDisplayBindingChanged(
            BindableObject bindable,
            object oldValue,
            object newValue)
        {
            if (bindable is WmsPicker picker)
            {
                picker.ApplyItemDisplayBinding();
            }
        }
        #endregion
        #region TextColor
        public static readonly BindableProperty TextColorProperty =
    BindableProperty.Create(
        nameof(TextColor),
        typeof(Color),
        typeof(WmsPicker),
        Colors.Black,
        propertyChanged: OnTextColorChanged);

        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }
        private static void OnTextColorChanged(
            BindableObject bindable,
            object oldValue,
            object newValue)
        {
            if (bindable is WmsPicker picker)
            {
                picker.ApplyTextColor();
            }
        }
        #endregion
    }
}
