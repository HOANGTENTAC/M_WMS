using M_WMS.Controls.Models;
using M_WMS.Controls.Popups;
using M_WMS.Enums;

namespace M_WMS.Services.Popups
{
    public static class PopupService
    {
        public static async Task<bool> ShowAsync(
        string title,
        string message,
        PopupType type = PopupType.Info,
        string okButtonText = "Đồng ý",
        int autoCloseMs = 0)
        {
            // 1. Lấy trang đang hiển thị trên màn hình
            if (Shell.Current?.CurrentPage is not ContentPage currentPage)
                return false;

            //var vm = new CustomPopupViewModel_New
            //{
            //    Title = title,
            //    Message = message,
            //    OkButtonText = okButtonText,
            //    CancelButtonText = cancelText,
            //    IsCancelButtonVisible = showCancel
            //};

            var popupVm = new CustomPopupViewModel_New
            {
                Title = title,
                Message = message
            };


            if (autoCloseMs > 0)
            {
                popupVm.StartAutoCloseTimer(autoCloseMs);
            }

            popupVm.SetPopupType(type, okButtonText);

            var popupView = new CustomPopupView
            {
                BindingContext = popupVm
            };

            // 3. Đảm bảo Content gốc của Trang là một Grid để thêm Overlay
            Grid? rootGrid = currentPage.Content as Grid;
            bool isTemporaryGrid = false;

            if (rootGrid == null)
            {
                // Nếu trang chưa bọc bằng Grid, bọc tạm thời Content vào Grid
                var originalContent = currentPage.Content;
                currentPage.Content = null;

                rootGrid = new Grid();
                if (originalContent != null)
                {
                    rootGrid.Children.Add(originalContent);
                }
                currentPage.Content = rootGrid;
                isTemporaryGrid = true;
            }

            // 4. Thêm Popup đè lên trên cùng
            rootGrid.Children.Add(popupView);

            _ = popupView.AnimateInAsync();

            // 5. Chờ người dùng tương tác
            bool result = await popupVm.ShowAsync(title, message);

            await popupView.AnimateOutAsync();

            // 6. Dọn dẹp Popup sau khi đóng
            rootGrid.Children.Remove(popupView);

            if (isTemporaryGrid && rootGrid.Children.Count > 0)
            {
                var originalContent = rootGrid.Children[0] as View;
                rootGrid.Children.Clear();
                currentPage.Content = originalContent;
            }

            return result;
        }
    }
}
