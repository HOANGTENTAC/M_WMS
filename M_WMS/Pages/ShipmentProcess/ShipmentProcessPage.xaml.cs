using M_WMS.Model;
using M_WMS.Services;
using M_WMS.ViewModel.ShipmentProcessViewModels;

namespace M_WMS.Pages.ShipmentProcess;

public partial class ShipmentProcessPage : ContentPage
{
    private readonly ShipmentProcessViewModel _viewModel;
    public ShipmentProcessPage(ShipmentProcessViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
        _viewModel = vm;
        //Loaded += (s, e) =>
        //{
        //    TestPicker.SelectedIndex = 0;
        //};
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

    private void QtyEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (sender is Entry entry && string.IsNullOrWhiteSpace(entry.Text))
        {
            entry.Text = "0";
        }
    }

    private void OnGoodsCdUnfocused(object sender, FocusEventArgs e)
    {
        if (sender is not Entry entry) return;

        if (entry.BindingContext is not ArrivalItem currentItem) return;

        if (this.BindingContext is not ShipmentProcessViewModel viewModel) return;

        if (viewModel.OnEntryUnfocusedCommand.CanExecute(currentItem))
        {
            viewModel.OnEntryUnfocusedCommand.Execute(currentItem);
        }
    }

    private void OnGoodsCdFocused(object sender, FocusEventArgs e)
    {
        if (sender is not Entry entry) return;

        if (entry.BindingContext is not ArrivalItem currentItem) return;

        if (this.BindingContext is not ShipmentProcessViewModel viewModel) return;

        if (viewModel.OnEntryFocusedCommand.CanExecute(currentItem))
        {
            viewModel.OnEntryFocusedCommand.Execute(currentItem);
        }
    }
}