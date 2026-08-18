using M_WMS.Enums;

namespace M_WMS.Services.Popups
{
    public interface IPopupDialogService
    {
        Task<bool> ShowPopupAsync(
                string title,
                string message,
                PopupType type = PopupType.Info,
                string? okButtonText = null,
                int autoCloseMs = 0);
    }
}
