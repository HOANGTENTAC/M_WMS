using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using M_WMS.Consts;
using M_WMS.Enums;
using M_WMS.Helpers;
using M_WMS.Model;
using M_WMS.Resources.Languages;
using M_WMS.Services.Popups;
using M_WMS.Utils;
using System.Collections.ObjectModel;
using System.Globalization;

namespace M_WMS.ViewModel
{
    public partial class SettingViewModel : ObservableObject
    {
        private readonly IPopupDialogService _popupService;
        [ObservableProperty]
        private List<WmsSelectOption> stockLocationLists;
        [ObservableProperty]
        private List<WmsSelectOption> languageLists;
        public ObservableCollection<GrpItem> GrpList { get; set; } = new();
        [ObservableProperty]
        private string stockLocation;
        [ObservableProperty]
        private string language;
        [ObservableProperty]
        private string userCode;
        [ObservableProperty]
        private string kyotenCd;
        [ObservableProperty]
        private string userName;
        [ObservableProperty]
        private string userNameAlias;
        [ObservableProperty]
        private string salesTanCd;
        [ObservableProperty]
        private string email;
        public SettingViewModel(IPopupDialogService popupService)
        {
            _popupService = popupService;
        }
        public async Task InitializeAsync()
        {
            await LoadStockType();
            UserCode = LoginInfo.UserCd;
            UserName = LoginInfo.UserName;
            KyotenCd = LoginInfo.KyotenCd;
            UserNameAlias = LoginInfo.UserNameAlias;
            SalesTanCd = LoginInfo.SalesTanCd;
            Email = LoginInfo.Email;

            GrpList.Clear();
            foreach (string item in LoginInfo.GrpCdList)
            {
                GrpList.Add(new GrpItem { Grp = item });
            }
        }
        public async Task LoadStockType()
        {
            StockLocationLists = CurrentErpUtil.StockLocations(LoginInfo.KyotenCd);
            StockLocation = LoginInfo.StockLocation;

            LanguageLists = CurrentErpUtil.SelectLanguage();
            Language = AppResources.Culture.TwoLetterISOLanguageName;
            await Task.CompletedTask;
        }
        [RelayCommand]
        private async Task Ok()
        {
            LoginInfo.StockLocation = StockLocation;
            Preferences.Default.Set("StockLocation", StockLocation);
            LocalizationResourceManager.Instance.SetCulture(new CultureInfo(Language));
            await _popupService.ShowPopupAsync($"{AppResources.RequestCompleted}", "Đã cập nhật thành công", PopupType.Info, "OK");

        }
    }
    public class GrpItem()
    {
        public string Grp { get; set; }
    }
}
