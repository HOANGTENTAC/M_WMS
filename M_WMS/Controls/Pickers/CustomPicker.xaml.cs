using System.Collections;

namespace M_WMS.Controls.Pickers;

public partial class CustomPicker : ContentView
{
    private bool _isOpen;
    public CustomPicker()
    {
        InitializeComponent();
        cvItems.SelectionChanged += CvItems_SelectionChanged;
    }
    public static readonly BindableProperty ItemsSourceProperty =
        BindableProperty.Create(
            nameof(ItemsSource),
            typeof(IEnumerable),
            typeof(CustomPicker),
            null,
            propertyChanged: OnItemsSourceChanged);

    public IEnumerable ItemsSource
    {
        get => (IEnumerable)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }
    private static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var picker = (CustomPicker)bindable;
        picker.cvItems.ItemsSource = newValue as IEnumerable;
    }
    private static void OnItemsChanged(
        BindableObject bindable,
        object oldValue,
        object newValue)
    {
        var picker = (CustomPicker)bindable;

        picker.cvItems.ItemsSource = newValue as IEnumerable;
    }

    private async void Header_Tapped(object sender, TappedEventArgs e)
    {
        _isOpen = !_isOpen;

        DropDownBorder.IsVisible = _isOpen;

        await imgArrow.RotateTo(_isOpen ? 180 : 0, 150);
    }

    private async void CvItems_SelectionChanged(
        object sender,
        SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count == 0)
            return;

        lblText.Text = e.CurrentSelection[0].ToString();

        _isOpen = false;

        DropDownBorder.IsVisible = false;

        await imgArrow.RotateTo(0, 150);
    }
}