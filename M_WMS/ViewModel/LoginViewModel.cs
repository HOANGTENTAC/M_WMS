using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using M_WMS.Consts;
using M_WMS.Helpers;
using M_WMS.Services;
using M_WMS.Utils;
using System.Globalization;
using M_WMS.Consts;

namespace M_WMS.ViewModel
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly ApiService _apiService;
        //public ICommand LoginCommand { get; }
        [ObservableProperty]
        private string userCode;
        partial void OnUserCodeChanged(string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                userCode = value.ToUpperInvariant();
            }
        }
        [ObservableProperty]
        private string password;
        [ObservableProperty]
        private bool rememberMe;
        [ObservableProperty]
        private bool hasSavedAccount;
        [ObservableProperty]
        private bool isBusy;
        public LoginViewModel(ApiService apiService)
        {
            _apiService = apiService;
            //LoginCommand = new Command(async () => await GetUser());
        }
        [RelayCommand]
        private async Task Login()
        {

            if (string.IsNullOrWhiteSpace(UserCode))
            {
                await Shell.Current.DisplayAlert("Thông báo", "Vui lòng nhập UserCode", "OK");
                return;
            }
            if (IsBusy) return;
            try
            {
                IsBusy = true;
                //var resultTest = await _apiService.SelectDbDictionary("select * from outbox_sync limit 1");
                var result = await _apiService.GetUserInfoAsync(UserCode);

                if (result?.MstUserList?.Any() == true)
                {
                    if (RememberMe)
                    {
                        await SecureStorage.SetAsync("UserCode", UserCode);
                    }
                    else
                    {
                        SecureStorage.Default.Remove("UserCode");
                    }
                    Preferences.Default.Set("RememberMe", RememberMe);

                    LoginInfo.UserName = result.MstUserList[0].User_Name;
                    LoginInfo.KyotenCd = result.MstUserList[0].Kyoten_Cd;
                    LoginInfo.UserCd = result.MstUserList[0].User_Cd;
                    LoginInfo.Culture = result.MstUserList[0].Culture;
                    LoginInfo.UserNameAlias = result.MstUserList[0].User_Name_Alias;
                    LoginInfo.SalesTanCd = result.MstUserList[0].Sales_Tan_Cd;
                    LoginInfo.Email = result.MstUserList[0].EMail;
                    LoginInfo.GrpCdList = result.GrpCdList;

                    switch (LoginInfo.KyotenCd)
                    {
                        case "HC":
                            Preferences.Default.Set("StockLocation", "VIETNAM");
                            break;
                        case "HN":
                            Preferences.Default.Set("StockLocation", "HANOI");
                            break;
                        case "TH":
                            Preferences.Default.Set("StockLocation", "THAILAND");
                            break;
                        case "QD":
                            Preferences.Default.Set("StockLocation", "QINGDAO");
                            break;
                        case "SH":
                            Preferences.Default.Set("StockLocation", "SHANGHAI");
                            break;
                        case "KZ":
                            string stockLocation = Preferences.Default.Get("StockLocation", "");
                            var listStockLocation = CurrentErpUtil.StockLocations("KZ");
                            if (listStockLocation.FirstOrDefault(x=>x.Value == stockLocation) == null)
                            {
                                Preferences.Default.Set("StockLocation", "");
                            }
                            break;
                        default:
                            Preferences.Default.Set("StockLocation", "");
                            break;
                    }
                    LoginInfo.StockLocation = Preferences.Default.Get("StockLocation", "");
                    await Task.Delay(50);

                    //await Shell.Current.GoToAsync(nameof(HomePage));
                    await Shell.Current.GoToAsync("//MainPage");
                    //MainThread.BeginInvokeOnMainThread(() =>
                    //{
                    //    Application.Current.MainPage = new MainPage();
                    //});
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Không tìm thấy dữ liệu", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                if (Shell.Current?.CurrentState?.Location?.OriginalString?.Contains("MainPage") != true)
                {
                    IsBusy = false; // TẮT LOADING
                }
            }
        }
        [RelayCommand]
        public async Task ChangeLanguageTapped()
        {
            string action = await Shell.Current.DisplayActionSheet(
                LocalizationResourceManager.Instance["SelectLanguage"],
                "Cancel",
                null,
                "English",
                "Tiếng Việt",
                "日本語");

            if (action == "English")
            {
                LocalizationResourceManager.Instance.SetCulture(new CultureInfo("en"));
            }
            else if (action == "Tiếng Việt")
            {
                LocalizationResourceManager.Instance.SetCulture(new CultureInfo("vi"));
            } else if (action == "日本語")
            {
                LocalizationResourceManager.Instance.SetCulture(new CultureInfo("ja"));
            } 
        }
        public async Task InitializeAsync()
        {
            var user = await SecureStorage.GetAsync("UserCode");

            HasSavedAccount = !string.IsNullOrEmpty(user);

            if (HasSavedAccount)
            {
                RememberMe = Preferences.Default.Get("RememberMe", false);
                UserCode = user;
                RememberMe = true;
            }
        }
        //[RelayCommand]
        //private void ClearUser()
        //{
        //    UserCode = "";
        //}
    }
}
