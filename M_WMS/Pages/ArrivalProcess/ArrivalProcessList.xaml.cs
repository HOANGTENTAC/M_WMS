using M_WMS.Services;
using M_WMS.ViewModel.ArrivalProcessViewModels;

namespace M_WMS.Pages.ArrivalProcess;

public partial class ArrivalProcessList : ContentPage
{
    private readonly ArrivalProcessListViewModel _viewModel;
    public ArrivalProcessList(ArrivalProcessListViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
        _viewModel = vm;

        WmsPopupService.Initialize(PopupHost, PopupContainer);
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.InitializeAsync();
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
}