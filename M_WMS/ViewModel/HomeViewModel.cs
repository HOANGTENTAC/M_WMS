using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using M_WMS.Consts;
using M_WMS.Controls.Popups;
using M_WMS.Controls.Selects;
using M_WMS.Helpers;
using M_WMS.Model;
using M_WMS.Pages;
using M_WMS.Pages.ArrivalProcess;
using M_WMS.Pages.ShipmentProcess;
using System.Collections.ObjectModel;

namespace M_WMS.ViewModel
{
    public partial class HomeViewModel : ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotLoading))]
        private bool _isLoading = true;
        public bool IsNotLoading => !IsLoading;

        [ObservableProperty]
        private string userName;

        //private string _userName;
        //public string UserName
        //{
        //    get => _userName;
        //    set => SetProperty(ref _userName, value); // Tự động kiểm tra và bắn PropertyChanged
        //}

        [ObservableProperty]
        private bool isBusy;
        [ObservableProperty]
        private string iconCountry = "vietnam.png";
        public ObservableCollection<HomeMenu> Menus { get; set; } = new();
        public HomeViewModel()
        {
        }
        [RelayCommand]
        private async Task OpenMenu(HomeMenu item)
        {
            if (item == null)
                return;

            if (IsBusy) return;

            try
            {
                IsBusy = true;
                await Task.Delay(50);
                await Shell.Current.GoToAsync(item.Route);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
        [RelayCommand]
        private void ChangeLanguageTapped()
        {
            //Shell.Current.DisplayAlert("Thông báo", "Bạn đã bấm vào cờ Việt Nam!", "OK");
        }
        public async Task InitializeAsync()
        {
            switch (LoginInfo.KyotenCd)
            {
                case "HN":
                case "HC":
                    IconCountry = "vietnam.png";
                    break;
                case "KZ":
                    IconCountry = "japan.png";
                    break;
                case "TH":
                    IconCountry = "thailand.png";
                    break;
                case "SH":
                case "QD":
                    IconCountry = "china.png";
                    break;
                default:
                    break;
            }

            Menus.Clear();
            IsLoading = true;
            var user = await SecureStorage.GetAsync("UserCode");
            UserName = LocalizationResourceManager.Instance["User"] + ": " + user;

            //await Task.Delay(300);

            Menus.Add(new HomeMenu { Title = LocalizationResourceManager.Instance["ArrivalProcess"], Icon = "stock_arrival.png", Route = nameof(ArrivalProcessList) });
            Menus.Add(new HomeMenu { Title = LocalizationResourceManager.Instance["ShipmentProcess"], Icon = "stock_shipment.png", Route = nameof(ShipmentProcessList) });
            Menus.Add(new HomeMenu { Title = LocalizationResourceManager.Instance["InventoryManagement"], Icon = "history_stock.png" });
            Menus.Add(new HomeMenu { Title = "VIETNAM", Icon = "map.png" });
            Menus.Add(new HomeMenu { Title = LocalizationResourceManager.Instance["StockTransfer"], Icon = "stock_transfer.png" });
            Menus.Add(new HomeMenu { Title = LocalizationResourceManager.Instance["Disposal"], Icon = "bin.png" });
            Menus.Add(new HomeMenu { Title = "Đọc file Excel", Icon = "bin.png", Route = nameof(ReadFileExcel) });

            IsLoading = false;
        }
    }
}
