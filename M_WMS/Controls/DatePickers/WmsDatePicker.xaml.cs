using M_WMS.Services;

namespace M_WMS.Controls.DatePickers;

public partial class WmsDatePicker : ContentView, IWmsFocusable
{
    public WmsDatePicker()
    {
        InitializeComponent();

        var tap = new TapGestureRecognizer();
        tap.Tapped += OnTapped;

        RootGrid.GestureRecognizers.Add(tap);

        UpdateFont();
        UpdateText();
    }

    private void Clear_Tapped(object sender, TappedEventArgs e)
    {
        ClearDate();
    }
}