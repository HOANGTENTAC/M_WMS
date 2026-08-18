using M_WMS.ViewModel;

namespace M_WMS.Pages;

public partial class SettingView : ContentView
{
    private readonly SettingViewModel _viewModel;
    public SettingView(SettingViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
        _viewModel = vm;
    }
    protected override async void OnParentSet()
    {
        base.OnParentSet();
        await _viewModel.InitializeAsync();
    }
}