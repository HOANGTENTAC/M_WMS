using M_WMS.Services;

namespace M_WMS.Controls.Popups;

public partial class WmsPopupHost : ContentView
{
	public WmsPopupHost()
	{
		InitializeComponent();
	}
    public void Show(View popup)
    {
        PopupPresenter.Content = popup;
        IsVisible = true;
    }

    public void Close()
    {
        PopupPresenter.Content = null;
        IsVisible = false;
    }
}