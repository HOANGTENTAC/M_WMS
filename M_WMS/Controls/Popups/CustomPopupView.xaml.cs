namespace M_WMS.Controls.Popups;

public partial class CustomPopupView : ContentView
{
	public CustomPopupView()
	{
		InitializeComponent();
	}
    public async Task AnimateInAsync()
    {
        // Đặt kích thước ban đầu nhỏ lại và trong suốt
        PopupContainer.Scale = 0.6;
        PopupContainer.Opacity = 0;

        // Phóng to đồng thời làm rõ nét với hiệu ứng SpringOut (nảy nhẹ)
        await Task.WhenAll(
            PopupContainer.ScaleTo(1.0, 250, Easing.SpringOut),
            PopupContainer.FadeTo(1.0, 200, Easing.CubicOut)
        );
    }

    // Hiệu ứng thu nhỏ khi đóng
    public async Task AnimateOutAsync()
    {
        await Task.WhenAll(
            PopupContainer.ScaleTo(0.8, 150, Easing.CubicIn),
            PopupContainer.FadeTo(0, 150, Easing.CubicIn)
        );
    }
}