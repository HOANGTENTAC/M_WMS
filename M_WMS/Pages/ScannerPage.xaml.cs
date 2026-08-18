using ZXing.Net.Maui;

namespace M_WMS.Pages;

public partial class ScannerPage : ContentPage
{
    public TaskCompletionSource<string> ScanResultTask { get; } = new();
    public ScannerPage()
	{
		InitializeComponent();

        barcodeReader.Options = new BarcodeReaderOptions
        {
            AutoRotate = true,
            Multiple = false,
            Formats = BarcodeFormats.All
        };
        //barcodeReader.BarcodesDetected += CameraView_BarcodesDetected;
    }
    private bool _isScanning = false;

    //private async void CameraView_BarcodesDetected(
    //object? sender,
    //BarcodeDetectionEventArgs e)
    //{
    //    var firstResult = e.Results?.FirstOrDefault();
    //    if (_isScanning)
    //        return;

    //    _isScanning = true;

    //    cameraView.IsDetecting = false;

    //    var result = e.Results.FirstOrDefault();

    //    if (result == null)
    //        return;

    //    await MainThread.InvokeOnMainThreadAsync(async () =>
    //    {
    //        ScanResultTask.TrySetResult(firstResult.Value);
    //        await Shell.Current.GoToAsync("..",
    //            new Dictionary<string, object>
    //            {
    //                ["Barcode"] = result.Value
    //            });
    //    });
    //}
    private void OnBarcodesDetected(object sender, BarcodeDetectionEventArgs e)
    {
        var firstResult = e.Results?.FirstOrDefault();
        if (firstResult != null)
        {
            // Tắt camera để tránh đọc lặp lại nhiều lần
            barcodeReader.IsDetecting = false;

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                // Trả kết quả về Task
                ScanResultTask.TrySetResult(firstResult.Value);

                // Đóng màn hình Scan
                await Navigation.PopAsync();
            });
        }
    }

    private async void OnCloseClicked(object sender, EventArgs e)
    {
        ScanResultTask.TrySetResult(null); // Hủy quét
        await Navigation.PopAsync();
    }
}