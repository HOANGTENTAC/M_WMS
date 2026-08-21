using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using M_WMS.Consts;
using M_WMS.Enums;
using M_WMS.Helpers;
using M_WMS.Model;
using M_WMS.Pages;
using M_WMS.Pages.ShipmentProcess;
using M_WMS.Resources.Languages;
using M_WMS.Services;
using M_WMS.Services.Popups;
using System.Collections.ObjectModel;
using ZXing.Net.Maui;

namespace M_WMS.ViewModel.ShipmentProcessViewModels
{
    public enum Status
    {
        None,
        StockNull,
        StockNotNull
    }
    public partial class ShipmentProcessListViewModel : ObservableObject
    {
        private readonly IPopupDialogService _popupService;
        private readonly ApiService _apiService;
        //public ObservableCollection<WmsSelectOption> StockKuList { get; set; }
        [ObservableProperty]
        public ObservableCollection<MametanModel> listItems = new();
        public int ItemCount => ListItems.Count;
        [ObservableProperty]
        private string instrNo = string.Empty;
        [ObservableProperty]
        private DateTime? startDateSearch = DateTime.Now;
        [ObservableProperty]
        private DateTime? endDateSearch = DateTime.Now;
        [ObservableProperty]
        private bool isLoading;
        [ObservableProperty]
        private bool isLoadingMore;
        [ObservableProperty]
        private string stockLocation = LoginInfo.StockLocation;
        [ObservableProperty]
        private bool hasMore = true;
        private const int PageSize = 30;
        private int _pageIndex = 1;
        public ShipmentProcessListViewModel(ApiService apiService, IPopupDialogService popupService)
        {
            _apiService = apiService;
            _popupService = popupService;
        }
        #region Initialize
        public async Task InitializeAsync()
        {
            //await LoadStockKu();
            //await LoadDataAsync();
        }
        #endregion

        #region Command
        [RelayCommand]
        private async Task Search()
        {
            try
            {
                IsLoading = true;
                _pageIndex = 1;
                HasMore = true;
                ListItems.Clear();
                ListItems = new ObservableCollection<MametanModel>(await LoadDataAsync());
                OnPropertyChanged(nameof(ItemCount));
                if (ListItems.Count < PageSize)
                    HasMore = false;
            }
            finally
            {
                IsLoading = false;
            }
        }
        [RelayCommand]
        private async Task Back()
        {
            await Shell.Current.GoToAsync("..");
        }
        [RelayCommand]
        private async Task Ok()
        {
            var selected = ListItems
                   .Where(x => x.IsSelected)
                   .ToList();

            List<ArrivalItem> arrivalItems = new List<ArrivalItem>();
            string message = "";
            foreach (var item in selected)
            {
                arrivalItems.Add(new ArrivalItem
                {
                    InstrNo = item.InstrNo,
                    PoNo = item.PoNo,
                    GoodsCd = item.ShoCd,
                    Qty = item.Suu,
                    GoodsName = item.GoodsName,
                    StockType = item.StockType,
                    PlanQty = item.Suu,
                    Instrgyou = item.Instrgyou,
                    PoGyou = 0,
                    Unit = item.Unit,
                    GrpCd = item.GrpCd,
                    KyotenId = item.KyotensId,
                    KeepLocation = item.KeepLocation,
                    ShoId = item.ShoId,
                    StockQty = item.StockQty
                });
                message += $@"{AppResources.InstrNo}: {item.InstrNo}, {AppResources.GoodsCD}: {item.ShoCd}, {AppResources.StockType}: {item.StockTypeName}" + Environment.NewLine;
            }
            var navigationParameters = new Dictionary<string, object>
                {
                    { "MyList", arrivalItems }
                };

            if (arrivalItems.Count > 0)
            {
                bool isConfirmed = await _popupService.ShowPopupAsync($"{AppResources.PleaseConfirm}", message, PopupType.Question, "OK");
                if (!isConfirmed)
                {
                    return;
                }
            }

            await Shell.Current.GoToAsync(nameof(ShipmentProcessPage), navigationParameters);
        }
        [RelayCommand]
        private void SelectAll()
        {
            bool newValue = ListItems.Any(x => !x.IsSelected);

            foreach (var item in ListItems)
                item.IsSelected = newValue;
        }
        [RelayCommand]
        private async Task Scan()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.Camera>();

            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.Camera>();
            }

            if (status != PermissionStatus.Granted)
            {
                await Shell.Current.DisplayAlert(
                    $"{LocalizationResourceManager.Instance["Notification"]}",
                    $"{LocalizationResourceManager.Instance["TheAppNeedsCameraPermissionToScan"]}",
                    "OK");

                return;
            }

            var scannerPage = new ScannerPage();
            await Shell.Current.Navigation.PushAsync(scannerPage);
            string barcodeResult = await scannerPage.ScanResultTask.Task;

            List<string> barcodeScan = new List<string>();
            if (barcodeResult != null && barcodeResult.ToString() != "")
            {
                barcodeScan = barcodeResult.ToString().Split('|').ToList();
            }
            if (!string.IsNullOrEmpty(InstrNo.Trim()))
            {
                if (barcodeScan.Count > 1)
                {
                    InstrNo += "," + barcodeScan[1].ToString();
                }
                else
                {
                    InstrNo += "," + barcodeScan[0].ToString();
                }
            }
            else
            {
                if (barcodeScan.Count > 1)
                {
                    InstrNo = barcodeScan[1].ToString();
                }
                else
                {
                    InstrNo = barcodeScan[0].ToString();
                }
            }
        }
        [RelayCommand]
        private void SelectRow(MametanModel item)
        {
            item.IsSelected = !item.IsSelected;
            if (item.IsSelected)
            {
                item.RowBackground = new SolidColorBrush(Color.FromArgb("#E8F5E9"));
            }
            else if (item.StatusShoCd == "1")
            {
                item.RowBackground = new SolidColorBrush(Color.FromArgb("#F5A89A"));
            }
            else if (item.StatusShoCd == "2")
            {
                item.RowBackground = new SolidColorBrush(Color.FromArgb("#FFFAB3"));
            }
            else
            {
                item.RowBackground = Brush.Transparent;
            }
        }
        [RelayCommand]
        private async Task ScanBarcodeRow(MametanModel item)
        {
            if (item == null) return;

            var scannerPage = new ScannerPage();
            await Shell.Current.Navigation.PushAsync(scannerPage);
            string barcodeResult = await scannerPage.ScanResultTask.Task;
            if (!string.IsNullOrEmpty(barcodeResult))
            {
                if (barcodeResult == item.ShoCd)
                {
                    item.IsSelected = true;
                    item.RowBackground = new SolidColorBrush(Color.FromArgb("#E8F5E9"));
                }
            }
        }
        [RelayCommand]
        private async Task LoadMore()
        {
            if (ListItems.Count > 0)
            {
                if (IsLoadingMore || !HasMore)
                    return;

                IsLoadingMore = true;

                try
                {
                    _pageIndex++;

                    var result = new ObservableCollection<MametanModel>(await LoadDataAsync());

                    if (result.Count == 0)
                    {
                        HasMore = false;
                        return;
                    }

                    foreach (var item in result)
                        ListItems.Add(item);

                    OnPropertyChanged(nameof(ItemCount));
                    if (result.Count < PageSize)
                        HasMore = false;
                }
                finally
                {
                    IsLoadingMore = false;
                }
            }
        }
        #endregion
        private async void Camera_BarcodesDetected(object? sender, BarcodeDetectionEventArgs e)
        {
            var value = e.Results.FirstOrDefault()?.Value;

            if (string.IsNullOrEmpty(value))
                return;

            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await Shell.Current.GoToAsync("..",
                    new Dictionary<string, object>
                    {
                { "Barcode", value }
                    });
            });
        }
        public async Task LoadStockKu()
        {
            try
            {
                IsLoading = true;
                string nameZaikoKu = string.Empty;
                if (LoginInfo.KyotenCd == "KZ" || LoginInfo.KyotenCd == "SH" || LoginInfo.KyotenCd == "QD")
                {
                    nameZaikoKu = "zaiko_ku_name_jp";
                }
                else
                {
                    nameZaikoKu = "zaiko_ku_name_en";
                }
                string sql = $@"SELECT zaiko_ku_cd as id,{nameZaikoKu} as name
                    FROM public.mst_cr_zaiko_ku where void_flg = 0
                    ORDER BY zaiko_ku_cd";
                var resultTest = await _apiService.SelectDbDictionaryCmms(sql);

                //StockKuList = resultTest.ToSelectOptions();
            }
            finally
            {
                IsLoading = false;
            }
        }
        private async Task<List<MametanModel>> LoadDataAsync()
        {
            string sql = $@"select h.instruction_no,d.sho_cd,d.suu,d.instr_gyou,d.id,h.po_no
                            from tbl_proc_h h
                            join tbl_proc_d d on h.id = d.tbl_proc_h_id
                            where h.void_flg = 0 and
	                              d.void_flg = 0 and
	                              h.kyoten_cd = '{LoginInfo.KyotenCd}' and
                                  h.instr_ku_1 in(1,2) and 
                                  d.instr_ku_2 = 1
                            ";
            if (!string.IsNullOrEmpty(InstrNo))
            {
                sql += $@" and h.instruction_no in ({string.Join(",", InstrNo.Split(',').Select(x => $"'{x}'"))})" + Environment.NewLine;
            }
            else
            {
                if (StartDateSearch != null)
                {
                    sql += $@" and h.scheduled_ship_date >= '{StartDateSearch.Value.ToString("yyyy/MM/dd")}'" + Environment.NewLine;
                }
                if (EndDateSearch != null)
                {
                    sql += $@" and h.scheduled_ship_date <= '{EndDateSearch.Value.ToString("yyyy/MM/dd")}'" + Environment.NewLine;
                }
            }
            sql += $@"order by d.id desc LIMIT {PageSize} OFFSET {(_pageIndex - 1) * PageSize}";

            var resultErp = await _apiService.SelectDbDictionaryErp(sql);

            string listSho = string.Join(",", resultErp?.Select(x => $"'{x["sho_cd"]}'"));

            if (!string.IsNullOrEmpty(listSho))
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
                string sqlCmms = $@"select a.sho_cd,b.stock_ku,a.id as sho_id, b.id as idstock,b.mst_cr_sho_kyotens_id,b.stock_basho,c.{nameZaikoKu} as stock_ku_name,
                                    b.stock_suu,a.grp_cd,d.name,d.tani
                                from mst_cr_shos a
                                left join mst_cr_sho_stock b on a.sho_cd = b.sho_cd and b.kyoten_cd = '{LoginInfo.KyotenCd}' and b.void_flg = 0 and stock_location = 'VIETNAM' and b.status = 1
                                left join mst_cr_zaiko_ku c on b.stock_ku::integer  = c.zaiko_ku_cd and c.void_flg = 0
                                left join mst_cr_sho_kyotens d on a.id = d.mst_cr_shos_id and d.void_flg = 0 and d.kyoten_cd = '{LoginInfo.KyotenCd}'
                                where a.void_flg = 0 and a.sho_cd in ({listSho})
                                ";
                var resultCmms = await _apiService.SelectDbDictionaryCmms(sqlCmms);

                var res = (from a in resultErp
                           join b in resultCmms on a["sho_cd"].ToString() equals b["sho_cd"].ToString()
                           into b
                           from grp in b.DefaultIfEmpty()
                           let status = (grp == null ? "1" : grp["idstock"] == "" ? "2" : "0")
                           //let status = "2"
                           select new MametanModel
                           {
                               Id = a["id"].ToString(),
                               InstrNo = a["instruction_no"].ToString(),
                               ShoCd = a["sho_cd"].ToString(),
                               StockType = grp != null && grp["idstock"] != "" ? grp["stock_ku"].ToString() : "",
                               StockTypeName = grp != null && grp["stock_ku_name"] != "" ? grp["stock_ku_name"].ToString() : "",
                               Suu = int.Parse(a["suu"].ToString()),
                               StatusShoCd = status,
                               RowBackground = status == "1" ? new SolidColorBrush(Color.FromArgb("#F5A89A")) :
                                             status == "2" ? new SolidColorBrush(Color.FromArgb("#FFFAB3")) : Brush.Transparent,
                               Instrgyou = int.Parse(a["instr_gyou"].ToString()),
                               KyotensId = grp != null && grp["mst_cr_sho_kyotens_id"] != "" ? int.Parse(grp["mst_cr_sho_kyotens_id"].ToString()) : null,
                               KeepLocation = grp != null && grp["stock_basho"] != "" ? grp["stock_basho"].ToString() : "",
                               ShoId = grp != null && grp["sho_id"] != "" ? int.Parse(grp["sho_id"].ToString()) : null,
                               StockQty = grp != null && grp["stock_suu"] != "" ? int.Parse(grp["stock_suu"].ToString()) : 0,
                               PoNo = a["po_no"].ToString() ?? "",
                               GoodsName = grp != null ? grp["name"].ToString() : "",
                               Unit = grp != null ? grp["tani"].ToString() : "",
                               GrpCd = grp != null ? grp["grp_cd"].ToString() : "",
                           }).OrderByDescending(x => x.Id).ToList();
                return res;
            }
            else
            {
                return new List<MametanModel>();
            }
        }
    }
}
