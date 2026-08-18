using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using M_WMS.Enums;
using M_WMS.FontAwesomeIs;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace M_WMS.Controls.Models
{
    public partial class CustomPopupViewModel_New : INotifyPropertyChanged
    {
        private CancellationTokenSource? _autoCloseCts;
        private bool _isVisible;
        public bool IsVisible
        {
            get => _isVisible;
            set { _isVisible = value; OnPropertyChanged(); }
        }

        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string HeaderIconGlyph { get; set; } = FontAwesomeIcons.InfoCircle;
        public Color HeaderIconTintColor { get; set; } = Colors.DodgerBlue;
        public string CancelButtonText { get; set; } = "Hủy";
        public string OkButtonText { get; set; } = "Đồng ý";
        public Color OkButtonBackgroundColor { get; set; } = Colors.DodgerBlue;
        public bool IsCancelButtonVisible { get; set; } = false;
        public string CancelIconGlyph = FontAwesomeIcons.Times;
        public string OkIconGlyph = FontAwesomeIcons.Check;
        public double ProgressValue = 1.0;
        // TaskCompletionSource giúp chờ kết quả trả về (Task<bool>) giống như ShowPopupAsync
        private TaskCompletionSource<bool>? _tcs;

        public ICommand CancelCommand { get; }
        public ICommand ConfirmCommand { get; }
        public ICommand OutsideTapCommand { get; }

        public CustomPopupViewModel_New()
        {
            CancelCommand = new Command(() => Close(false));
            ConfirmCommand = new Command(() => Close(true));
            OutsideTapCommand = new Command(() => Close(false));
        }
        public Task<bool> ShowAsync(string title, string message)
        {
            Title = title;
            Message = message;
            OnPropertyChanged(nameof(Title));
            OnPropertyChanged(nameof(Message));

            _tcs = new TaskCompletionSource<bool>();
            IsVisible = true; // Hiện Popup

            return _tcs.Task;
        }

        private void Close(bool result)
        {
            _autoCloseCts?.Cancel();
            IsVisible = false; // Ẩn Popup
            _tcs?.TrySetResult(result);
        }
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
                        Close(true);
                    });
                }
                catch (TaskCanceledException)
                {
                    // Người dùng đã bấm nút OK hoặc Cancel trước khi hết thời gian
                }
            }, token);
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
        
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
