using M_WMS.Controls.Popups;
using M_WMS.Controls.Selects;
using M_WMS.ViewModel;

namespace M_WMS;

public partial class MainPage : ContentPage
{
    private readonly MainViewModel _viewModel;
    public MainPage(MainViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        _viewModel = vm;
        this.Opacity = 0;
    }
    //TabControl.ItemsSource = new()
    //{ 
    //    new WmsTabItem
    //    {
    //        Title = "Home",
    //        ViewFactory = () => new HomeView()
    //    },

    //    new WmsTabItem
    //    {
    //        Title = "Setting",
    //        ViewFactory = () => new SettingView()
    //    }
    //};
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Hiệu ứng hiện dần trong 250ms cực mượt
        await this.FadeTo(1, 250, Easing.CubicIn);
    }
}