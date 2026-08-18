using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Views;
using M_WMS.Controls.Models;
using M_WMS.Controls.Popups;
using M_WMS.Enums;

namespace M_WMS.Services.Popups
{
    public class PopupDialogService : IPopupDialogService
    {
        public async Task<bool> ShowPopupAsync(
        string title,
        string message,
        PopupType type = PopupType.Info,
        string? okButtonText = null,
        int autoCloseMs = 0)
        {
            var popupVm = new CustomPopupViewModel
            {
                Title = title,
                Message = message
            };

            // Truyền okButtonText vào hàm SetPopupType
            popupVm.SetPopupType(type, okButtonText);

            if (autoCloseMs > 0)
            {
                popupVm.StartAutoCloseTimer(autoCloseMs);
            }

            var popup = new CustomPopup(popupVm);
            var currentPage = Shell.Current?.CurrentPage ?? Application.Current?.MainPage;

            if (currentPage != null)
            {
                var options = new PopupOptions
                {
                    Shape = null
                };

                //var popupResult = await currentPage.ShowPopupAsync<bool>(popup);
                IPopupResult<bool> popupResult = await currentPage.ShowPopupAsync<bool>(popup, options);
                //return result is bool isSuccess && isSuccess;
                return popupResult?.Result ?? false;
            }

            return false;
        }
    }
}
