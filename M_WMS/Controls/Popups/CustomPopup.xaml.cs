namespace M_WMS.Controls.Popups;
using CommunityToolkit.Maui.Views;
using M_WMS.Controls.Models;

public partial class CustomPopup : Popup<bool>
{
    public CustomPopup(CustomPopupViewModel viewModel)
    {
        InitializeComponent();
        //Color = Colors.Transparent;
        // Gán BindingContext
        BindingContext = viewModel;

        //BorderContainer.SizeChanged += OnBorderContainerSizeChanged;
        // Bắt sự kiện khi Popup bắt đầu mở để chạy Animation
        Opened += OnPopupOpened;

        // Lắng nghe sự kiện đóng Popup từ ViewModel
        viewModel.CloseAction = async (result) => await CloseWithAnimationAsync(result);

    }
    private async void OnPopupOpened(object? sender, EventArgs e)
    {
        // Hiệu ứng Zoom & Fade In khi xuất hiện
        this.Content.Scale = 0.7;
        this.Content.Opacity = 0;

        await Task.WhenAll(
            this.Content.ScaleTo(1.0, 250, Easing.CubicOut),
            this.Content.FadeTo(1.0, 250, Easing.CubicOut)
        );
    }

    private async Task CloseWithAnimationAsync(bool result)
    {
        // Hiệu ứng Zoom Out & Fade Out khi đóng
        await Task.WhenAll(
            this.Content.ScaleTo(0.8, 150, Easing.CubicIn),
            this.Content.FadeTo(0, 150, Easing.CubicIn)
        );

        // Đóng Popup và trả về kết quả
        await CloseAsync();
    }
}