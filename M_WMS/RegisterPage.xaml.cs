using System.Threading.Tasks;

namespace M_WMS;

public partial class RegisterPage : ContentPage
{
	public RegisterPage()
	{
		InitializeComponent();
	}

    private async void OnRegisterButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void OnLoginLabelTapped(object sender, TappedEventArgs e)
    {
        await Navigation.PopAsync();
    }
}