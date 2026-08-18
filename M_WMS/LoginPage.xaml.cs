using M_WMS.ViewModel;

namespace M_WMS;

public partial class LoginPage : ContentPage
{
    private readonly LoginViewModel _viewModel;
    public LoginPage(LoginViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
        _viewModel = vm;
    }

    private async void OnRegisterLabelTapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new RegisterPage());
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _viewModel.InitializeAsync();
    }
    //private async void OnLoginButtonClicked(object sender, EventArgs e)
    //{
    //    if (string.IsNullOrEmpty(UsernameEntry.Text) || string.IsNullOrEmpty(PasswordEntry.Text))
    //    {
    //        await DisplayAlert("Error", "Please enter both username and password.", "OK");
    //        return;
    //    }
    //    try
    //    {
    //        if (UsernameEntry.Text == "user" && PasswordEntry.Text == "password")
    //        {
    //            await DisplayAlert("Success", "Login successful!", "OK");
    //            await Navigation.PushAsync(new MainPage());
    //        }
    //        else
    //        {
    //            await DisplayAlert("Error", "Invalid username or password", "OK");
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        await DisplayAlert("Error", $"Login failed: {ex.Message}", "OK");
    //        return;
    //    }
    //}
}