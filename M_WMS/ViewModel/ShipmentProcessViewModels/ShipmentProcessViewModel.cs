using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using M_WMS.Consts;
using M_WMS.Controls.Models;
using M_WMS.Enums;
using M_WMS.Helpers;
using M_WMS.Model;
using M_WMS.Model.WSA0201_RegistArrivalShipment;
using M_WMS.Resources.Languages;
using M_WMS.Services;
using M_WMS.Services.Popups;
using M_WMS.Utils;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using static M_WMS.Model.WSA0201_RegistArrivalShipment.WSA0201ReqModel;

namespace M_WMS.ViewModel.ShipmentProcessViewModels
{
    [QueryProperty(nameof(ItemList), "MyList")]
    public partial class ShipmentProcessViewModel : ObservableObject
    {
        private string shoCdOldValue;
        public ICommand OnEntryUnfocusedCommand { get; private set; }
        public ICommand OnEntryFocusedCommand { get; private set; }
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

        public ShipmentProcessViewModel(ApiService apiService)
        {
            _apiService = apiService;
            OnEntryUnfocusedCommand = new Command<ArrivalItem>(async (item) => await OnEntryUnfocused(item));
            OnEntryFocusedCommand = new Command<ArrivalItem>(OnEntryFocused);
        }

        #region Data
        public ObservableCollection<ArrivalItem> ShipmentItems { get; set; } = [];

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
                    ShipmentItems.Clear();

                    foreach (var item in itemList)
                    {
                        item.PropertyChanged += Item_PropertyChanged;
                        ShipmentItems.Add(item);
                    }

                    //OnPropertyChanged(nameof(ItemCount));
                    ItemCountText = $"{AppResources.Items}: {ShipmentItems.Count}";
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
            ShipmentItems.Add(newItem);
            newItem.PropertyChanged += Item_PropertyChanged;
            //OnPropertyChanged(nameof(ItemCount));
            ItemCountText = $"{AppResources.Items}: {ShipmentItems.Count}";
            UpdateTotal();
        }
        [RelayCommand]
        private void DeleteRow(ArrivalItem item)
        {
            item.PropertyChanged -= Item_PropertyChanged;
            ShipmentItems.Remove(item);
            ItemCountText = $"{AppResources.Items}: {ShipmentItems.Count}";
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
            await Shell.Current.DisplayAlert("Scan", "Scan Barcode", "OK");
        }
        [RelayCommand]
        private async Task Ok()
        {
            bool check = await CheckValidation();
            if (check)
            {
                DateTime currentDate = DateTime.Now;
                var ListShipmentItems = ShipmentItems.ToList();

                string inoutNo = await _apiService.GetOrderNumberAsync("Shipment_Stock", LoginInfo.KyotenCd);
                string sequenceNo = await _apiService.GetOrderNumberAsync("SequenceNo", LoginInfo.KyotenCd);

                var res = new WSAXX03Model();
                res.Payload = new OrderPayload();
                res.Payload.Header = new GoodsH();
                res.Payload.Details = new List<GoodsM>();

                List<TblInoutHistoryH> historyHs = new List<TblInoutHistoryH>();

                TblInoutHistoryH historyH = new TblInoutHistoryH
                {
                    InoutKu1 = 2,
                    InoutKu2 = int.Parse(InoutKu2),
                    InoutNo = inoutNo,
                    InstructionNo = CurrentErpUtil.ReplaceSpace(InstrNo),
                    PoNo = PoNo,
                    EntryDate = currentDate.ToString("yyyy/MM/dd"),
                    EntryTime = currentDate.ToString("HH:mm:ss"),
                    UpdatedAt = DateTimeOffset.Now,
                    UpdatedUserCd = LoginInfo.KyotenCd,
                    UpdatedUserName = LoginInfo.UserName,
                    CreatedAt = DateTimeOffset.Now,
                    CreatedUserCd = LoginInfo.UserCd,
                    CreatedUserName = LoginInfo.UserName,
                    Bikou = Note,
                    KyotenCd = LoginInfo.KyotenCd,
                    OptDivision = OptDivision,
                    ScheduledShipDate = (ScheduledShipDate != "" ? DateTime.Parse(ScheduledShipDate).ToString("yyyy/MM/dd") : ""),
                    Payloads = JsonConvert.SerializeObject(res.Payload),
                    TableName = "Tbl_Inout_History_H_InoutKu2",
                    Action = "Insert",
                    Status = "PENDING",
                    SequenceNo = int.Parse(sequenceNo),
                    TransactionId = Guid.NewGuid(),
                    IdOutbox = Guid.NewGuid()
                };
                historyHs.Add(historyH);

                var tblInoutHistoryDList = new List<TblInoutHistoryD>();
                int inoutGyou = 0;
                foreach (var row in ShipmentItems.ToList())
                {
                    var tblInoutHistoryD = new TblInoutHistoryD
                    {
                        InoutKu1 = 2,
                        InoutKu2 = int.Parse(InoutKu2),
                        InoutGyou = inoutGyou,
                        InstructionNo = CurrentErpUtil.ReplaceSpace(InstrNo),
                        InstructionGyou = row.InstructionGyou,
                        PoNo = row.PoNo,
                        PoGyou = row.PoGyou,
                        EntryDate = currentDate.ToString("yyyy/MM/dd"),
                        EntryTime = currentDate.ToString("HH:mm:ss"),
                        ShoCd = CurrentErpUtil.ReplaceSpace(row.GoodsCd),
                        Hinmei = row.GoodsName,
                        Unit = row.Unit,
                        Suu = row.Qty,
                        GrpCd = row.GrpCd,
                        ZaikoKu = row.StockType,
                        InoutReason = row.Remark,
                        UpdatedAt = DateTimeOffset.Now,
                        UpdatedUserCd = LoginInfo.UserCd,
                        UpdatedUserName = LoginInfo.UserName,
                        CreatedAt = DateTimeOffset.Now,
                        CreatedUserCd = LoginInfo.UserCd,
                        CreatedUserName = LoginInfo.UserName,
                        PlanQty = row.PlanQty,
                        InoutNo = inoutNo,
                        MstCrShoKyotensId = (int)row.KyotenId,
                        StockBasho = row.KeepLocation,
                        FirstReceivedDate = DateTimeOffset.Now.ToString("yyyy/MM/dd"),
                        FirstShippedDate = DateTimeOffset.Now.ToString("yyyy/MM/dd"),
                        LastReceivedDate = DateTimeOffset.Now.ToString("yyyy/MM/dd"),
                        LastShippedDate = DateTimeOffset.Now.ToString("yyyy/MM/dd"),
                    };
                    tblInoutHistoryDList.Add(tblInoutHistoryD);

                    inoutGyou++;
                }
                //var reqData = new WSA0201ReqModel
                //{
                //    InoutKu1 = 2,
                //    InoutKu2 = int.Parse(InoutKu2),
                //    TblInoutHistoryHs = historyHs,
                //    TblInoutHistoryDs = tblInoutHistoryDList,
                //    //StockLocation = SettingINI.StockLocation
                //};
                //var url = $"{ApiConfig.RegistArrivalShipmentUrl}";
                //var resData = _apiService.PostSync<WSA0201ResModel>(url, reqData);

                //switch (resData.Result)
                //{
                //    case 0:
                //        StockSlip = inoutNo;
                //        break;
                //}
            }
        }
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
        private async Task<bool> CheckValidation()
        {
            if (string.IsNullOrEmpty(InstrNo))
            {
                await PopupService.ShowAsync($"{AppResources.PleaseConfirm}", AppResources.EmptyMametan, PopupType.Warning, "OK");
                return false;
            }
            //if (string.IsNullOrEmpty(InoutKu2))
            //{
            //    throw new Exception(AppResources.ErrorInoutKu2Required);
            //}
            int countrow = 0;
            foreach (var item in ShipmentItems)
            {
                countrow++;
                if ((string.IsNullOrEmpty(item.GoodsCd) ||
                item.KyotenId == null ||
                item.ShoId == null ||
                item.StockQty == null) && item.Qty != 0)
                {
                    await PopupService.ShowAsync($"{AppResources.PleaseConfirm}", AppResources.ShoCdIsNotRegistered, PopupType.Warning, "OK");
                    return false;
                }
                if (item.Qty == 0)
                {
                    await PopupService.ShowAsync($"{AppResources.PleaseConfirm}", AppResources.WMS9999CW0057, PopupType.Warning, "OK");
                    return false;
                }
                if(item.StockQty < item.Qty)
                {
                    await PopupService.ShowAsync($"{AppResources.PleaseConfirm}", AppResources.WMS9999CW0061, PopupType.Warning, "OK");
                    return false;
                }
                if(string.IsNullOrEmpty(item.StockType))
                {
                    await PopupService.ShowAsync($"{AppResources.PleaseConfirm}", AppResources.WMS9999CW0058, PopupType.Warning, "OK");
                    return false;
                }
                if(string.IsNullOrEmpty(item.InstrNo))
                {
                    await PopupService.ShowAsync($"{AppResources.PleaseConfirm}", AppResources.EmptyMametan, PopupType.Warning, "OK");
                    return false;
                }
            }
            return await Task.FromResult(true);
        }
        #endregion
        #region Load Data
        private async Task LoadDataAsync()
        {
            ShipmentItems.Clear();

            ShipmentItems.Add(new ArrivalItem
            {
                GoodsCd = "MAT001",
                Status = "Arrival",
                Qty = 100,
                StockType = "10",
                Remark = "GOOD"
            });

            ShipmentItems.Add(new ArrivalItem
            {
                GoodsCd = "MAT002",
                Status = "Arrival",
                Qty = 50,
                StockType = "11",
                Remark = "NG"
            });
            ItemCountText = $"{LocalizationResourceManager.Instance["Items"]}: {ShipmentItems.Count}";
            //OnPropertyChanged(nameof(ItemCount));
            // Lắng nghe khi Qty thay đổi
            foreach (var item in ShipmentItems)
            {
                item.PropertyChanged += Item_PropertyChanged;
            }
            UpdateTotal();
            await Task.CompletedTask;
        }

        public void UpdateTotal()
        {
            TotalQty = ShipmentItems.Sum(item => item.Qty);
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
            StockTypeList = CurrentErpUtil.GetStockType(2);
            await Task.CompletedTask;
        }
        public async Task LoadStockKu()
        {
            string sql = $@"SELECT zaiko_ku_cd as id,zaiko_ku_name_en as name
                    FROM public.mst_cr_zaiko_ku where void_flg = 0
                    ORDER BY zaiko_ku_cd";
            var resultTest = await _apiService.SelectDbDictionaryCmms(sql);

            StockKuList = resultTest.ToSelectOptions();
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
        #endregion
    }
}
