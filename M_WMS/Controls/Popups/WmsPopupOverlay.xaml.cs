namespace M_WMS.Controls.Popups;

public partial class WmsPopupOverlay : ContentView
{
	public WmsPopupOverlay()
	{
		InitializeComponent();
        var tap = new TapGestureRecognizer();
        tap.Tapped += (_, _) => Close();

        PART_Background.GestureRecognizers.Add(tap);
    }
    public View? PopupContent
    {
        get => PART_Content.Content;
        set => PART_Content.Content = value;
    }

    public void Open()
    {
        IsVisible = true;
    }

    public void Close()
    {
        IsVisible = false;
        PART_Content.Content = null;
    }
}