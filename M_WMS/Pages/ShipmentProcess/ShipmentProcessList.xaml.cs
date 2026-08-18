using M_WMS.Services;
using M_WMS.ViewModel.ShipmentProcessViewModels;

namespace M_WMS.Pages.ShipmentProcess;

public partial class ShipmentProcessList : ContentPage//, IQueryAttributable
{
    private readonly ShipmentProcessListViewModel _viewModel;
    public ShipmentProcessList(ShipmentProcessListViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
        _viewModel = vm;

        WmsPopupService.Initialize(PopupHost, PopupContainer);
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        Dispatcher.DispatchDelayed(TimeSpan.FromMilliseconds(150), async () =>
        {
            await _viewModel.InitializeAsync();
        });

    }
    private async void PopupBackground_Tapped(object sender, TappedEventArgs e)
    {
        await WmsPopupService.CloseAsync();
    }

    private void OnEditorFocused(object sender, FocusEventArgs e)
    {
        ClearButton.IsVisible = true;
    }

    private void OnEditorUnfocused(object sender, FocusEventArgs e)
    {
        ClearButton.IsVisible = false;
    }

    private void OnClearTapped(object sender, EventArgs e)
    {
        InstrNo.Text = string.Empty;
    }

    //private void BarcodesDetected(object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs e)
    //{

    //}
    //public void ApplyQueryAttributes(IDictionary<string, object> query)
    //{
    //    if (query.TryGetValue("Barcode", out var barcode))
    //    {
    //        List<string> barcodeScan = new List<string>();
    //        if(barcode != null && barcode.ToString() != "")
    //        {
    //            barcodeScan = barcode.ToString().Split('|').ToList();
    //        }

    //        if(!string.IsNullOrEmpty(_viewModel.InstNo.Trim()))
    //        {
    //            if (barcodeScan.Count > 1)
    //            {
    //                _viewModel.InstNo += "," + barcodeScan[1].ToString();
    //            }
    //            else
    //            {
    //                _viewModel.InstNo += "," + barcodeScan[0].ToString();
    //            }
    //        }
    //        else
    //        {
    //            if (barcodeScan.Count > 1)
    //            {
    //                _viewModel.InstNo = barcodeScan[1].ToString();
    //            }
    //            else
    //            {
    //                _viewModel.InstNo = barcodeScan[0].ToString();
    //            }
    //        }
            
    //        //_ = _viewModel.SearchCommand.ExecuteAsync(null);
    //    }
    //}
}