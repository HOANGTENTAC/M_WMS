using M_WMS.Pages;
using M_WMS.Pages.ArrivalProcess;
using M_WMS.Pages.ShipmentProcess;

namespace M_WMS
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            //Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(HomeView), typeof(HomeView));
            Routing.RegisterRoute(nameof(SettingView), typeof(SettingView));
            Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
            Routing.RegisterRoute(nameof(ArrivalProcessPage), typeof(ArrivalProcessPage));
            Routing.RegisterRoute(nameof(ArrivalProcessList), typeof(ArrivalProcessList));
            Routing.RegisterRoute(nameof(ShipmentProcessPage), typeof(ShipmentProcessPage));
            Routing.RegisterRoute(nameof(ShipmentProcessList), typeof(ShipmentProcessList));
            Routing.RegisterRoute(nameof(ScannerPage), typeof(ScannerPage));
            Routing.RegisterRoute(nameof(ReadFileExcel), typeof(ReadFileExcel));
        }
    }
}
