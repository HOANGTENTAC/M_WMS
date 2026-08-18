using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using M_WMS.Consts;
using M_WMS.Helpers;
using M_WMS.Model;
using M_WMS.Resources.Languages;
using M_WMS.Services;
using M_WMS.Services.Popups;
using M_WMS.Utils;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace M_WMS.ViewModel.ArrivalProcessViewModels
{
    [QueryProperty(nameof(ItemList), "MyList")]
    public partial class ArrivalProcessViewModel : ObservableObject
    {
        private string shoCdOldValue;
        public ICommand OnEntryUnfocusedCommand { get; private set; }
        public ICommand OnEntryFocusedCommand { get; private set; }
        private readonly IPopupDialogService _popupService;
        [ObservableProperty]
        private List<ArrivalItem> itemList;
        private readonly ApiService _apiService;
        #region Search Condition

        [ObservableProperty]
        private string instrNo;

        [ObservableProperty]
        private string goodsCd;

        [ObservableProperty]
        private string scheduledShipDate;

        [ObservableProperty]
        private string optDivision;

        [ObservableProperty]
        private string note;

        [ObservableProperty]
        private string poNo;

        [ObservableProperty]
        private string stockSlip;

        [ObservableProperty]
        private string inoutKu2 = "1";
        #endregion

        #region Screen
        [ObservableProperty]
        private int totalQty;
        #endregion

        [ObservableProperty]
        private string itemCountText;

        [ObservableProperty]
        private bool isLoading;

        //public int ItemCount => ArrivalItems.Count;
        //public int TotalQty => ArrivalItems.Sum(x=>x.Qty);
        #region Combo
        public ObservableCollection<WmsSelectOption> ReasonList { get; set; }
        public ObservableCollection<WmsSelectOption> UnitList { get; set; }
        public ObservableCollection<WmsSelectOption> StockKuList { get; set; }
        [ObservableProperty]
        private List<WmsSelectOption> stockTypeList;
        #endregion

        public ArrivalProcessViewModel(ApiService apiService, IPopupDialogService popupService)
        {
            _apiService = apiService;
            _popupService = popupService;
            OnEntryUnfocusedCommand = new Command<ArrivalItem>(async (item) => await OnEntryUnfocused(item));
            OnEntryFocusedCommand = new Command<ArrivalItem>(OnEntryFocused);
        }

        #region Data
        //public ObservableCollection<ArrivalItem> Items { get; } = [];
        public ObservableCollection<ArrivalItem> ArrivalItems { get; set; } = [];

        #endregion

        #region Initialize
        public async Task InitializeAsync()
        {
            try
            {
                IsLoading = true;
                await LoadStockType();
                await LoadStockKu();
                await LoadUnit();
                await LoadReason();
                if (itemList.Count > 0)
                {
                    ArrivalItems.Clear();

                    foreach (var item in itemList)
                    {
                        item.PropertyChanged += Item_PropertyChanged;
                        ArrivalItems.Add(item);
                    }

                    ItemCountText = $"{AppResources.Items}: {ArrivalItems.Count}";
                    UpdateTotal();
                }
            }
            finally
            {
                IsLoading = false;
            }
        }
        #endregion
        #region Command
        [RelayCommand]
        private void AddRow()
        {
            var newItem = new ArrivalItem
            {
                GoodsCd = "",
                Status = "Arrival",
                StockType = "PRODUCT",
                Qty = 0,
                Remark = "GOOD"
            };
            ArrivalItems.Add(newItem);
            newItem.PropertyChanged += Item_PropertyChanged;
            //OnPropertyChanged(nameof(ItemCount));
            ItemCountText = $"{AppResources.Items}: {ArrivalItems.Count}";
            UpdateTotal();
        }
        [RelayCommand]
        private void DeleteRow(ArrivalItem item)
        {
            item.PropertyChanged -= Item_PropertyChanged;
            ArrivalItems.Remove(item);
            ItemCountText = $"{AppResources.Items}: {ArrivalItems.Count}";
            //OnPropertyChanged(nameof(ItemCount));
            UpdateTotal();
        }
        [RelayCommand]
        private async Task Back()
        {
            await Shell.Current.GoToAsync("..");
        }
        [RelayCommand]
        private async Task Scan()
        {
            await Shell.Current.DisplayAlert(
                "Scan",
                "Scan Barcode",
                "OK");
        }
        [RelayCommand]
        private void Ok()
        {
            //foreach (var item in ArrivalItems)
            //{
            //}
        }
        #endregion
        #region Load Data

        public void UpdateTotal()
        {
            TotalQty = ArrivalItems.Sum(item => item.Qty);
        }
        private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ArrivalItem.Qty))
            {
                UpdateTotal();
            }
        }
        public async Task LoadStockType()
        {
            StockTypeList = CurrentErpUtil.GetStockType(1);
            await Task.CompletedTask;
        }
        private async Task LoadUnit()
        {
            string getTaniSql =
                               $@"SELECT tani as id, tani as name
                    FROM mst_cr_sho_kyotens 
                    WHERE kyoten_cd = '{LoginInfo.KyotenCd}' AND 
                          void_flg = 0 AND 
                          tani is not null 
                    GROUP BY tani 
                    ORDER BY tani;";
            var resultTest = await _apiService.SelectDbDictionaryCmms(getTaniSql);
            UnitList = resultTest.ToSelectOptions();
        }

        private async Task LoadReason()
        {
            var result = await CurrentErpUtil.LoadReason(LoginInfo.KyotenCd, _apiService);
            ReasonList = result.ToSelectOptions();
        }
        public async Task LoadStockKu()
        {
            string sql = $@"SELECT zaiko_ku_cd as id,zaiko_ku_name_en as name
                    FROM public.mst_cr_zaiko_ku where void_flg = 0
                    ORDER BY zaiko_ku_cd";
            var resultTest = await _apiService.SelectDbDictionaryCmms(sql);

            StockKuList = resultTest.ToSelectOptions();
        }
        #endregion
        public async Task OnEntryUnfocused(ArrivalItem updatedItem)
        {
            if (updatedItem.GoodsCd != shoCdOldValue)
            {
                string nameZaikoKu = string.Empty;
                if (LoginInfo.KyotenCd == "KZ" || LoginInfo.KyotenCd == "SH" || LoginInfo.KyotenCd == "QD")
                {
                    nameZaikoKu = "zaiko_ku_name_jp";
                }
                else
                {
                    nameZaikoKu = "zaiko_ku_name_en";
                }
                string sqlCmms = $@"select a.sho_cd,b.stock_ku,a.id as sho_id, b.id as idstock,d.id as kyotens_id,b.stock_basho,c.{nameZaikoKu} as stock_ku_name,
                                    b.stock_suu,d.tani,d.name
                                from mst_cr_shos a
                                left join mst_cr_sho_kyotens d on a.id = d.mst_cr_shos_id and d.kyoten_cd = '{LoginInfo.KyotenCd}' and d.void_flg = 0
                                left join mst_cr_sho_stock b on a.sho_cd = b.sho_cd and b.kyoten_cd = '{LoginInfo.KyotenCd}' and b.void_flg = 0 and stock_location = 'VIETNAM' and b.status = 1
                                left join mst_cr_zaiko_ku c on b.stock_ku::integer  = c.zaiko_ku_cd and c.void_flg = 0
                                where a.void_flg = 0 and a.sho_cd = '{updatedItem.GoodsCd}'
                                limit 1";
                var resultCmms = await _apiService.SelectDbDictionaryCmms(sqlCmms);
                if (resultCmms.Count > 0)
                {
                    var item = resultCmms[0];
                    updatedItem.KyotenId = item["kyotens_id"] != null ? (int?)Convert.ToInt32(item["kyotens_id"]) : null;
                    updatedItem.ShoId = item["sho_id"] != null ? (int?)Convert.ToInt32(item["sho_id"]) : null;
                    updatedItem.StockQty = item["stock_suu"] != null ? (int?)Convert.ToInt32(item["stock_suu"]) : null;
                    updatedItem.KeepLocation = item["stock_basho"]?.ToString() ?? string.Empty;
                    updatedItem.Unit = item["tani"]?.ToString() ?? string.Empty;
                    updatedItem.GoodsName = item["name"]?.ToString() ?? string.Empty;
                }
                else
                {
                    updatedItem.KyotenId = null;
                    updatedItem.ShoId = null;
                    updatedItem.StockQty = null;
                    updatedItem.KeepLocation = string.Empty;
                }
            }
        }
        public void OnEntryFocused(ArrivalItem updatedItem)
        {
            shoCdOldValue = updatedItem.GoodsCd;
        }
    }
}