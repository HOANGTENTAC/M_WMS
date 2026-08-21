using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using M_WMS.Consts;
using M_WMS.Enums;
using M_WMS.Helpers;
using M_WMS.Model;
using M_WMS.Pages;
using M_WMS.Pages.ArrivalProcess;
using M_WMS.Resources.Languages;
using M_WMS.Services;
using M_WMS.Services.Popups;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;

namespace M_WMS.ViewModel.ArrivalProcessViewModels
{
    public partial class ArrivalProcessListViewModel : ObservableObject
    {
        private readonly IPopupDialogService _popupService;
        private readonly ApiService _apiService;
        public ObservableCollection<WmsSelectOption> StockKuList { get; set; }
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
        public ArrivalProcessListViewModel(ApiService apiService, IPopupDialogService popupService)
        {
            _apiService = apiService;
            _popupService = popupService;
        }
        #region Initialize
        public async Task InitializeAsync()
        {
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

            await Shell.Current.GoToAsync(nameof(ArrivalProcessPage), navigationParameters);
        }
        #endregion
        [RelayCommand]
        private void SelectAll()
        {
            bool newValue = ListItems.Any(x => !x.IsSelected);

            foreach (var item in ListItems)
                item.IsSelected = newValue;
        }
        [RelayCommand]
        private void SelectRow(MametanModel item)
        {
            if (item.StatusShoCd != "0") return;

            item.IsSelected = !item.IsSelected;
            if (item.IsSelected)
            {
                item.RowBackground = new SolidColorBrush(Color.FromArgb("#E8F5E9"));
            }
            //else if (item.StatusShoCd == "1")
            //{
            //    item.RowBackground = new SolidColorBrush(Color.FromArgb("#F5A89A"));
            //}
            //else if (item.StatusShoCd == "2")
            //{
            //    item.RowBackground = new SolidColorBrush(Color.FromArgb("#FFFAB3"));
            //}
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
            if (barcodeScan.Count > 0)
            {
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
        }

        public async Task LoadStockKu()
        {
            string sql = $@"SELECT zaiko_ku_cd as id,zaiko_ku_name_en as name
                    FROM public.mst_cr_zaiko_ku where void_flg = 0
                    ORDER BY zaiko_ku_cd";
            var resultTest = await _apiService.SelectDbDictionaryCmms(sql);

            StockKuList = resultTest.ToSelectOptions();
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
        private async Task<List<MametanModel>> LoadDataAsync()
        {
            string sqlSelect = string.Empty;
            string nameDatabase = "";
            if (LoginInfo.KyotenCd == "HC")
            {
                nameDatabase = "VNProc_Inst";
            }
            else if (LoginInfo.KyotenCd == "HN")
            {
                nameDatabase = "HNProc_Inst";
            }
            else
            {
                nameDatabase = "Proc_Inst";
            }
            if (LoginInfo.KyotenCd == "HC" || LoginInfo.KyotenCd == "HN" || LoginInfo.KyotenCd == "TH")
            {
                sqlSelect = $@"SELECT h._INSTRUCTION_NO as PoNo,h._PRESS_INST_NO as instruction_no,
                             CASE WHEN d._GOODS_CD IS NOT NULL AND mst._GOODS_CD IS NULL THEN d._GOODS_CD
                         	    ELSE ISNULL(mst._GOODS_CD,mst1._GOODS_CD) END as sho_cd,
                             ISNULL(ISNULL(d._QTY,h._QTY),0) suu,ISNULL(d._LINE_NO,0) as instr_gyou
                             FROM TBL_PROC_H h
                             left join TBL_PROC_MC d on h._PRESS_INST_NO = d._PRESS_INST_NO
                             left join TBL_MST_GOODS mst on d._GOODS_CD = mst._GOODS_CD
                             left join TBL_MST_GOODS mst1 on h._GOODS_CD = mst1._GOODS_CD
                            where 1=1 {(string.IsNullOrEmpty(InstrNo) ? $" and _ISSUE_DATE >= '{StartDateSearch.Value.ToString("yyyy/MM/dd")}' and _ISSUE_DATE <= '{EndDateSearch.Value.ToString("yyyy/MM/dd")}'" : "")}
                            {(!string.IsNullOrEmpty(InstrNo) ? $" and h._PRESS_INST_NO like '{InstrNo}%'" : "")}

union all
                         select '' as PoNo, h._Order_No as instruction_no,d._GOODS_CD as sho_cd, ISNULL(d._QTY,0) suu,d._LINE_NO as instr_gyou
                         from TBL_PO_INTERNATIONAL_Head h
                         join TBL_PO_INTERNATIONAL_RECORD d on h._Order_No = d._Order_No
                         join TBL_MST_GOODS mst on d._GOODS_CD = mst._GOODS_CD
                         where 1=1 {(string.IsNullOrEmpty(InstrNo) ? $" and _Order_Date >= '{StartDateSearch.Value.ToString("yyyy/MM/dd")}' and _Order_Date <= '{EndDateSearch.Value.ToString("yyyy/MM/dd")}'" : "")}
                         {(!string.IsNullOrEmpty(InstrNo) ? $" and h._Order_No like '{InstrNo}%'" : "")}

union all
                         select '' as PoNo, h._Order_No as instruction_no,d._GOODS_CD as sho_cd, ISNULL(d._QTY,0) suu,d._LINE_NO as instr_gyou
                         from TBL_PO_DOMESTIC_Head h
                         join TBL_PO_DOMESTIC_RECORD d on h._Order_No = d._Order_No
                         join TBL_MST_GOODS mst on d._GOODS_CD = mst._GOODS_CD
                         where 1=1 {(string.IsNullOrEmpty(InstrNo) ? $" and _Order_Date >= '{StartDateSearch.Value.ToString("yyyy/MM/dd")}' and _Order_Date <= '{EndDateSearch.Value.ToString("yyyy/MM/dd")}'" : "")}
                         {(!string.IsNullOrEmpty(InstrNo) ? $" and h._Order_No like '{InstrNo}%'" : "")}

union all
                         select '' as PoNo, HaNo as instruction_no,SyouCD as sho_cd, ISNULL(Suu,0) suu,0 as instr_gyou
                         FROM [{nameDatabase}].[dbo].[PR_NaITEM]
                         where 1 = 1 {(string.IsNullOrEmpty(InstrNo) ? $" and HakkouBi >= '{StartDateSearch.Value.ToString("yyyy/MM/dd")}' and HakkouBi <= '{EndDateSearch.Value.ToString("yyyy/MM/dd")}'" : "")}
                         {(!string.IsNullOrEmpty(InstrNo) ? $" and HaNo like '{InstrNo}%'" : "")}
";
            }
            else if(LoginInfo.KyotenCd != "KZ")
            {
                sqlSelect = $@"select h.指示書番号 as PoNo, h.印刷指示書NO as instruction_no,
                         case when d.商品CD is not null and mst.商品CD is null then d.商品CD 
                              else ISNULL(mst.商品CD,mst1.商品CD) end as sho_cd,
                         ISNULL(ISNULL(d.数量,h.数量),0) suu,
                         ISNULL(d.行番号,0) as instr_gyou
                         from TBL_PROC_H h
                         left join TBL_PROC_MC d on h.印刷指示書NO = d.印刷指示書NO
                         left join TBL_MST_GOODS mst on d.商品CD = mst.商品CD
                         left join TBL_MST_GOODS mst1 on h.商品CD = mst1.商品CD
                         where 1=1 {(string.IsNullOrEmpty(InstrNo) ? $" and h.発行日 >= '{StartDateSearch.Value.ToString("yyyy/MM/dd")}' and h.発行日 <= '{EndDateSearch.Value.ToString("yyyy/MM/dd")}'" : "")}
                         {(!string.IsNullOrEmpty(InstrNo) ? $" and h.印刷指示書NO like '{InstrNo}%'" : "")}

union all 
                         select '' as PoNo, h.発注書NO as instruction_no,d.商品CD as sho_cd, ISNULL(d.数量,0) suu,d.行番号 as instr_gyou
                         from TBL_発注書国際_Head h
                         join TBL_発注書国際_RECORD d on h.発注書NO = d.発注書NO
                         join TBL_MST_GOODS mst on d.商品CD = mst.商品CD
                         where 1=1 {(string.IsNullOrEmpty(InstrNo) ? $" and h.発注日 >= '{StartDateSearch.Value.ToString("yyyy/MM/dd")}' and h.発注日 <= '{EndDateSearch.Value.ToString("yyyy/MM/dd")}'" : "")}
                         {(!string.IsNullOrEmpty(InstrNo) ? $" and h.発注書NO like '{InstrNo}%'" : "")}

union all
                         select '' as PoNo, h.発注書NO as instruction_no,d.商品CD as sho_cd,ISNULL(d.数量,0) suu,d.行番号 as instr_gyou
                         from TBL_発注書国内_Head h
                         join TBL_発注書国内_RECORD d on h.発注書NO = d.発注書NO
                         join TBL_MST_GOODS mst on d.商品CD = mst.商品CD
                         where 1=1 {(string.IsNullOrEmpty(InstrNo) ? $" and h.発注日 >= '{StartDateSearch.Value.ToString("yyyy/MM/dd")}' and h.発注日 <= '{EndDateSearch.Value.ToString("yyyy/MM/dd")}'" : "")}
                         {(!string.IsNullOrEmpty(InstrNo) ? $" and h.発注書NO like '{InstrNo}%'" : "")}

union all
                         select '' as PoNo, HaNo as instruction_no,SyouCD as sho_cd,
                         ISNULL(Suu,0) suu,0 as instr_gyou, '' as unit
                         from [{nameDatabase}].[dbo].[PR_NaITEM]
                         where 1 = 1 {(string.IsNullOrEmpty(InstrNo) ? $" and HakkouBi >= '{StartDateSearch.Value.ToString("yyyy/MM/dd")}' and HakkouBi <= '{EndDateSearch.Value.ToString("yyyy/MM/dd")}'" : "")}
                         {(!string.IsNullOrEmpty(InstrNo) ? $" and HaNo like '{InstrNo}%'" : "")}
                         OPTION (MAXDOP 1) ";
            }

            DataTable dataSql = await SqlServerService.GetDataFromRegionAsync(sqlSelect, LoginInfo.KyotenCd);

            string listSho = string.Join(",", dataSql.AsEnumerable().Select(x => $"'{x["sho_cd"]}'"));

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

                var res = (from a in dataSql.AsEnumerable()
                           join b in resultCmms on a["sho_cd"].ToString() equals b["sho_cd"].ToString()
                           into b
                           from grp in b.DefaultIfEmpty()
                           let status = (grp == null ? "1" : grp["idstock"] == "" ? "2" : "0")
                           //let status = "2"
                           select new MametanModel
                           {
                               InstrNo = a["instruction_no"].ToString() ?? "",
                               ShoCd = a["sho_cd"].ToString() ?? "",
                               StockType = grp != null && grp["idstock"] != "" ? grp["stock_ku"].ToString() : "",
                               StockTypeName = grp != null && grp["stock_ku_name"] != "" ? grp["stock_ku_name"].ToString() : "",
                               Suu = int.Parse(a["suu"].ToString() ?? "0"),
                               StatusShoCd = status,
                               RowBackground = status == "1" ? new SolidColorBrush(Color.FromArgb("#F5A89A")) :
                                             status == "2" ? new SolidColorBrush(Color.FromArgb("#FFFAB3")) : Brush.Transparent,
                               Instrgyou = int.Parse(a["instr_gyou"].ToString() ?? "0"),
                               KyotensId = grp != null && grp["mst_cr_sho_kyotens_id"] != "" ? int.Parse(grp["mst_cr_sho_kyotens_id"].ToString()) : null,
                               KeepLocation = grp != null && grp["stock_basho"] != "" ? grp["stock_basho"].ToString() : "",
                               ShoId = grp != null && grp["sho_id"] != "" ? int.Parse(grp["sho_id"].ToString()) : null,
                               StockQty = grp != null && grp["stock_suu"] != "" ? int.Parse(grp["stock_suu"].ToString()) : 0,
                               PoNo = a["PoNo"].ToString() ?? "",
                               GoodsName = grp != null ? grp["name"].ToString() : "",
                               Unit = grp != null ? grp["tani"].ToString() : "",
                               GrpCd = grp != null ? grp["grp_cd"].ToString() : "",
                           }).OrderByDescending(x => x.InstrNo).ToList();
                return res;
            }
            else
            {
                return new List<MametanModel>();
            }
        }
    }
}
