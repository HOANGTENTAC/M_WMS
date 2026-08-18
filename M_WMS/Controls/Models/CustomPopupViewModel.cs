using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using M_WMS.Enums;
using M_WMS.FontAwesomeIs;

namespace M_WMS.Controls.Models
{
    public partial class CustomPopupViewModel : ObservableObject
    {
        private CancellationTokenSource? _autoCloseCts;
        // Action dùng để đóng Popup từ ViewModel và trả về kết quả (bool)
        public Action<bool>? CloseAction { get; set; }

        [ObservableProperty] private string _title = string.Empty;
        [ObservableProperty] private string _message = string.Empty;

        // Icon & Màu sắc chính cho Theme
        [ObservableProperty] private string _headerIconGlyph = FontAwesomeIcons.InfoCircle;
        [ObservableProperty] private Color _headerIconTintColor = Colors.DodgerBlue;

        // Nút OK
        [ObservableProperty] private string _okButtonText = "OK";
        [ObservableProperty] private Color _okButtonBackgroundColor = Colors.DodgerBlue;
        [ObservableProperty] private string _okIconGlyph = FontAwesomeIcons.Check;

        // Nút Cancel
        [ObservableProperty] private string _cancelButtonText = "Bỏ qua";
        [ObservableProperty] private string _cancelIconGlyph = FontAwesomeIcons.Times;

        [ObservableProperty] private double _progressValue = 1.0;

        [ObservableProperty] private bool _isCancelButtonVisible = false;
        // Hàm kích hoạt tự động đóng sau x giây (ms)
        public void StartAutoCloseTimer(int durationInMilliseconds = 3000)
        {
            //_autoCloseCts = new CancellationTokenSource();
            var token = _autoCloseCts.Token;

            Task.Run(async () =>
            {
                try
                {
                    int interval = 30; // Cập nhật giao diện mỗi 30ms cho mượt
                    int totalSteps = durationInMilliseconds / interval;
                    double stepDecrement = 1.0 / totalSteps;

                    for (int i = 0; i < totalSteps; i++)
                    {
                        await Task.Delay(interval, token);

                        // Cập nhật thanh ProgressBar trên UI Thread
                        MainThread.BeginInvokeOnMainThread(() =>
                        {
                            ProgressValue = Math.Max(0, ProgressValue - stepDecrement);
                        });
                    }

                    // Khi chạy hết thời gian mà người dùng chưa bấm gì
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        Cancel();
                    });
                }
                catch (TaskCanceledException)
                {
                    // Người dùng đã bấm nút OK hoặc Cancel trước khi hết thời gian
                }
            }, token);
        }

        [RelayCommand]
        private void Confirm()
        {
            _autoCloseCts?.Cancel();
            CloseAction?.Invoke(true); // Trả về true khi nhấn OK
        }

        [RelayCommand]
        private void Cancel()
        {
            _autoCloseCts?.Cancel();
            CloseAction?.Invoke(false); // Trả về false khi nhấn Cancel
        }
        public void SetPopupType(PopupType type, string? customOkText = null)
        {
            IsCancelButtonVisible = (type == PopupType.Question);
            switch (type)
            {
                case PopupType.Question:
                    HeaderIconGlyph = FontAwesomeIcons.QuestionCircle;
                    HeaderIconTintColor = Color.FromArgb("#1976D2"); // Màu tím (Purple) hoặc bạn có thể dùng màu Cam (#F59E0B)
                    OkButtonBackgroundColor = Color.FromArgb("#1976D2");
                    OkIconGlyph = FontAwesomeIcons.Check;
                    OkButtonText = customOkText ?? "Đồng ý";
                    CancelButtonText = "Cancel";
                    break;

                case PopupType.Success:
                    HeaderIconGlyph = FontAwesomeIcons.CheckCircle;
                    HeaderIconTintColor = Color.FromArgb("#10B981");
                    OkButtonBackgroundColor = Color.FromArgb("#10B981");
                    OkIconGlyph = FontAwesomeIcons.Check;
                    OkButtonText = customOkText ?? "Hoàn tất";
                    break;

                case PopupType.Warning:
                    HeaderIconGlyph = FontAwesomeIcons.ExclamationTriangle;
                    HeaderIconTintColor = Color.FromArgb("#EF4444");
                    OkButtonBackgroundColor = Color.FromArgb("#EF4444");
                    OkIconGlyph = FontAwesomeIcons.ExclamationTriangle;
                    OkButtonText = customOkText ?? "Xóa ngay";
                    break;

                case PopupType.Info:
                default:
                    HeaderIconGlyph = FontAwesomeIcons.InfoCircle;
                    HeaderIconTintColor = Color.FromArgb("#3B82F6");
                    OkButtonBackgroundColor = Color.FromArgb("#3B82F6");
                    OkIconGlyph = FontAwesomeIcons.Check;
                    OkButtonText = customOkText ?? "Close";
                    break;
            }
        }
    }
}
