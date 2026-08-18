namespace M_WMS.Controls.TabPages;

public partial class CustomTabBar : ContentView
{
    public CustomTabBar()
    {
        InitializeComponent();
        BackgroundColor = Colors.Transparent;
    }
    private async void OnTabTapped(object sender, EventArgs e)
    {
        if (sender is VisualElement element)
        {
            // Hiệu ứng nhún nhẹ khi chạm vào nút Tab
            await element.ScaleTo(0.85, 80, Easing.CubicOut);
            await element.ScaleTo(1.0, 80, Easing.CubicIn);
        }
    }
}