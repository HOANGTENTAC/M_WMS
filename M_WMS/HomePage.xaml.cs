using M_WMS.Services;
using M_WMS.ViewModel;

namespace M_WMS
{
    public partial class HomePage : ContentView
    {
        //private readonly HomeViewModel _viewModel;
        int count = 0;

        public HomePage()
        {
            InitializeComponent();
            //BindingContext = vm;
            //_viewModel = vm;
            //Loaded += async (s, e) => await _viewModel.InitializeAsync();
        }
        //protected override async void OnAppearing()
        //{
        //    base.OnAppearing();

        //    await _viewModel.InitializeAsync();
        //}
        //private void OnCounterClicked(object? sender, EventArgs e)
        //{
        //    count++;

        //    if (count == 1)
        //        CounterBtn.Text = $"Clicked {count} time";
        //    else
        //        CounterBtn.Text = $"Clicked {count} times";

        //    SemanticScreenReader.Announce(CounterBtn.Text);
        //}
    }
}
