using ClosedXML.Excel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using M_WMS.Consts;
using M_WMS.Enums;
using M_WMS.Logics.WSB0005;
using M_WMS.Model;
using M_WMS.Resources.Languages;
using M_WMS.Services;
using M_WMS.Services.Popups;
using M_WMS.Utils;
using System.Collections.ObjectModel;
using System.Data;
using System.Text.RegularExpressions;
using static M_WMS.Model.WSA0201_RegistArrivalShipment.WSA0201ReqModel;

namespace M_WMS.ViewModel
{
    public partial class ReadFileExcelViewModel : ObservableObject
    {
        private List<TblInoutHistoryH> tblInoutHistoryHLists = new List<TblInoutHistoryH>();
        private List<TblInoutHistoryD> tblInoutHistoryDLists = new List<TblInoutHistoryD>();
        string inoutNo = "", InstrNoOld = "", StockKuOld = "", zaikoKu = "";
        private readonly IPopupDialogService _popupService;
        private readonly ApiService _apiService;
        [ObservableProperty]
        private ObservableCollection<StockInoutTenplate> stockInoutTenplate = new();

        // Dòng thông báo trạng thái
        [ObservableProperty]
        private string statusText = "Chưa chọn file nào";

        [ObservableProperty]
        private bool isInputSelect = true;

        [ObservableProperty]
        private bool isOutputSelect;

        [ObservableProperty]
        private string fileName;

        [RelayCommand]
        private async Task PickAndReadExcelAsync()
        {
            try
            {
                if (IsInputSelect)
                {
                    //await ExecuteInput(StockInoutTenplate, _popupService, _apiService);
                    StatusText = $"Đã đọc thành công {StockInoutTenplate.Count} dòng từ Sheet '{CurrentErpUtil.LoadSheetInput(LoginInfo.KyotenCd)}'! File : {FileName}";
                }
                else
                {
                    WSB0005_RegistShipment res = new WSB0005_RegistShipment();
                    await res.Execute(StockInoutTenplate, _popupService, _apiService);
                    StatusText = $"Đã đọc thành công {StockInoutTenplate.Count} dòng từ Sheet '{CurrentErpUtil.LoadSheetOutput(LoginInfo.KyotenCd)}'!";
                }
            }
            catch (Exception ex)
            {
                await _popupService.ShowPopupAsync("Lỗi", ex.Message, PopupType.Question, "OK");
            }
        }

        public ReadFileExcelViewModel(ApiService apiService, IPopupDialogService popupService)
        {
            _apiService = apiService;
            _popupService = popupService;
        }

        //public async Task ExecuteInput(ObservableCollection<StockInoutTenplate> stockInoutTenplate, IPopupDialogService _popupService, ApiService _apiService)
        //{
        //    try
        //    {
        //        tblInoutHistoryHLists.Clear();
        //        tblInoutHistoryDLists.Clear();

        //        var InstrNo = "";
        //        int inoutGyou = 0;
        //        bool res = true;
        //        string StockTypeName = "";
        //        //var SUBTITLE = "";
        //        string PoNo = "";
        //        string sqlzaiko_ku = "select zaiko_ku_cd from mst_cr_zaiko_ku where void_flg = 0";
        //        DataTable dtzaiko_ku = WebApiCommon.GetCmmsDataTable(sqlzaiko_ku);

        //        // Cấu hình lọc file Excel
        //        var excelFileType = new FilePickerFileType(
        //            new Dictionary<DevicePlatform, IEnumerable<string>>
        //            {
        //                { DevicePlatform.iOS, new[] { "org.openxmlformats-officedocument.spreadsheetml.sheet", "com.microsoft.excel.xls" } },
        //                { DevicePlatform.Android, new[] { "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "application/vnd.ms-excel" } },
        //                { DevicePlatform.WinUI, new[] { ".xlsx", ".xls" } },
        //                { DevicePlatform.MacCatalyst, new[] { "xlsx", "xls" } },
        //            });

        //        var result = await FilePicker.Default.PickAsync(new PickOptions
        //        {
        //            PickerTitle = "Chọn file Excel",
        //            FileTypes = excelFileType
        //        });

        //        if (result == null) return;

        //        FileName = result.FileName;

        //        // Mở luồng đọc file mà không mở ứng dụng Excel
        //        using var stream = await result.OpenReadAsync();

        //        using (var workbook = new XLWorkbook(stream))
        //        {
        //            // Chọn Sheet theo tên hoặc dùng Worksheet(1)
        //            string targetSheetName = CurrentErpUtil.LoadSheetInput(LoginInfo.KyotenCd);
        //            var worksheet = workbook.Worksheets.FirstOrDefault(w => w.Name == targetSheetName)
        //                         ?? workbook.Worksheets.First();

        //            stockInoutTenplate.Clear();

        //            // Lặp qua tất cả dòng có chứa dữ liệu
        //            foreach (var row in worksheet.RowsUsed())
        //            {
        //                // Bỏ qua dòng bị ẩn trong Excel
        //                if (row.IsHidden) continue;

        //                // Bỏ qua dòng tiêu đề
        //                if (row.RowNumber() == 1) continue;

        //                string ShoCd = CurrentErpUtil.ReplaceSpace(row.Cell("F").GetValue<string>());
        //                string LineNo = row.Cell("D").GetValue<string>();
        //                string StockType = row.Cell("E").GetValue<string>();
        //                int Qty = int.Parse(row.Cell("H").GetValue<string>());
        //                InstrNo = CurrentErpUtil.ReplaceSpace(row.Cell("K").GetValue<string>());
        //                zaikoKu = string.IsNullOrEmpty(CurrentErpUtil.ReplaceSpace(StockType)) ? "" : StockType.Substring(0, 2);
        //                string Hinmei = "";
        //                string Unit = "";
        //                string Note = row.Cell("J").GetValue<string>();
        //                //string LotNo = Range("K" + j);
        //                string ExpirationDate = row.Cell("L").GetValue<string>() != "" ? DateTime.FromOADate(double.Parse(row.Cell("L").GetValue<string>())).ToString("yyyy/MM/dd") : "";
        //                string OptDivision = row.Cell("M").GetValue<string>();
        //                string Reason = string.IsNullOrEmpty(row.Cell("N").GetValue<string>()) ? "" : row.Cell("N").GetValue<string>().Substring(0, 4);

        //                string checkShoCd = $@"SELECT 
		      //              MCS.sho_cd,
        //                    MCSK.name as sho_name,
        //                    MCSK.tani as tani,
        //                    MCS.id shoId,
        //                    MCSK.id kyotenId,
        //                    MCS.grp_cd,
        //                    MCSST.id stockId,
        //                    MCSST.stock_basho,
        //                    MCSST.stock_suu
        //            FROM mst_cr_shos MCS
        //            JOIN mst_cr_sho_kyotens MCSK ON MCSK.mst_cr_shos_id = MCS.id AND MCSK.kyoten_cd = '{LoginInfo.KyotenCd}' AND MCSK.void_flg = 0
        //            LEFT JOIN mst_cr_sho_stock MCSST ON MCSST.sho_cd = MCS.sho_cd AND MCSST.stock_ku = '{(StockType == "" ? "" : StockType.Substring(0, 2))}' AND MCSST.void_flg = 0 and MCSST.status = 1 and MCSST.kyoten_cd = '{LoginInfo.KyotenCd}'
        //            WHERE MCS.void_flg = 0 AND MCS.sho_cd = '{ShoCd}'";

        //                if (LoginInfo.KyotenCd == "KZ")
        //                {
        //                    checkShoCd = $@"SELECT 
		      //              MCS.sho_cd,
        //                    MCSK.name as sho_name,
        //                    MCSK.tani as tani,
        //                    MCS.id shoId,
        //                    MCSK.id kyotenId,
        //                    MCS.grp_cd,
        //                    MCSST.id stockId,
        //                    MCSST.stock_basho,
        //                    MCSST.stock_suu
        //            FROM mst_cr_shos MCS
        //            JOIN mst_cr_sho_kyotens MCSK ON MCSK.mst_cr_shos_id = MCS.id AND MCSK.kyoten_cd = '{LoginInfo.KyotenCd}' AND MCSK.void_flg = 0
        //            LEFT JOIN mst_cr_sho_stock MCSST ON MCSST.sho_cd = MCS.sho_cd AND MCSST.stock_ku = '{(StockType == "" ? "" : StockType.Substring(0, 2))}' AND MCSST.void_flg = 0 and MCSST.status = 1 and MCSST.kyoten_cd = '{LoginInfo.KyotenCd}' and MCSST.stock_location = '{SettingINI.StockLocation}'
        //            WHERE MCS.void_flg = 0 AND MCS.sho_cd = '{ShoCd}'";
        //                }
        //                checkShoCdData = WebApiCommon.GetCmmsDataTable(checkShoCd);

        //                if (InstrNo != InstrNoOld || zaikoKu != StockKuOld)
        //                {
        //                    if (InstrNo == "")
        //                    {
        //                        return res;
        //                    }
        //                    InstrNoOld = InstrNo;
        //                    StockKuOld = zaikoKu;

        //                    if (dtzaiko_ku.AsEnumerable().Where(x => x["zaiko_ku_cd"].ToString() == zaikoKu).Count() == 0 || StockType == "")
        //                    {
        //                        res = false;
        //                        await _popupService.ShowPopupAsync("Lỗi", string.Format(AppResources.WMS9999CW0074, ShoCd) + "\r\n" + "Line : " + j, PopupType.Warning, "OK");
        //                        return res;
        //                    }

        //                    var stocktypeOld = CurrentErpUtil.load_stock_ku_storage_type(zaikoKu, culture: KyotenUtil.CultureKyoten(LoginInfo.KyotenCd), onlyname: true);
        //                    StockTypeName = stocktypeOld.Rows[0]["name"].ToString();
        //                    //SUBTITLE = _isEnTable ? (zaikoKu == "10" ? "RESOLD" : (zaikoKu == "11" ? "MAKE" : "")) : "";

        //                    if (checkShoCdData.Rows.Count == 0)
        //                    {
        //                        res = false;
        //                        await _popupService.ShowPopupAsync("Lỗi", string.Format(AppResources.WMS9999CW0056, ShoCd) + " - " +
        //                            AppResources.InstructionNo + ": " + InstrNo +
        //                            "\r\n" + "Line : " + j, PopupType.Warning, "OK");
        //                        return res;
        //                    }
        //                    string getposql = "";
        //                    if (_isEnTable)
        //                    {
        //                        getposql = $@"select _INSTRUCTION_NO as PONO
        //                                     from TBL_PROC_H
        //                                     where _PRESS_INST_NO = '{InstrNo}'";
        //                    }
        //                    else
        //                    {
        //                        getposql = $@"select 指示書番号 as PONO
        //                                     from TBL_PROC_H
        //                                     where 印刷指示書NO = '{InstrNo}'";
        //                    }
        //                    var dataTable = WebApiCommon.GetSQLTableValue(getposql, LoginInfo.KyotenCd, _testFlg);
        //                    if (dataTable.Rows.Count > 0)
        //                    {
        //                        PoNo = dataTable.Rows[0]["PONO"].ToString();
        //                    }
        //                    else
        //                    {
        //                        PoNo = "";
        //                    }

        //                    inoutNo = CurrentErpUtil.GetArrivalStockiOrderNumber(true, 0, LoginInfo.KyotenCd).ToString().PadLeft(8, '0');
        //                    string sequenceNo = CurrentErpUtil.GetSequenceNoNumber(0, LoginInfo.KyotenCd).ToString();

        //                    inoutGyou = 0;

        //                    var tblInoutHistoryHs = new TblInoutHistoryH
        //                    {
        //                        InoutKu1 = 1,
        //                        InoutKu2 = InstrNo.StartsWith("3") ? 2 : 1,
        //                        InoutNo = inoutNo,
        //                        InstructionNo = CurrentErpUtil.ReplaceSpace(InstrNo),
        //                        PoNo = PoNo,
        //                        EntryDate = DateTime.Now.ToString("yyyy/MM/dd"),
        //                        EntryTime = DateTime.Now.ToString("HH:mm:ss"),
        //                        UpdatedAt = DateTimeOffset.Now,
        //                        UpdatedUserCd = LoginInfo.UserCd,
        //                        UpdatedUserName = LoginInfo.UserName,
        //                        CreatedAt = DateTimeOffset.Now,
        //                        CreatedUserCd = LoginInfo.UserCd,
        //                        CreatedUserName = LoginInfo.UserName,
        //                        Bikou = Note,
        //                        KyotenCd = LoginInfo.KyotenCd,
        //                        ZaikoKu = zaikoKu,
        //                        OptDivision = OptDivision,
        //                        IdOutbox = Guid.NewGuid(),
        //                        TableName = "Tbl_Inout_History_H_InoutKu1",
        //                        Action = "Insert",
        //                        Status = "PENDING",
        //                        SequenceNo = int.Parse(sequenceNo),
        //                        TransactionId = Guid.NewGuid(),
        //                    };
        //                    tblInoutHistoryHLists.Add(tblInoutHistoryHs);
        //                }
        //                else if (InstrNo == "" && InstrNoOld == "")
        //                {
        //                    return res;
        //                }
        //                var regexItem = new Regex("^[a-zA-Z]*$");
        //                if (InstrNo.StartsWith("1") ||
        //                    InstrNo.StartsWith("2") ||
        //                    InstrNo.StartsWith("3") ||
        //                    regexItem.IsMatch(CurrentErpUtil.ReplaceSpace(InstrNo).Substring(0, 1)))
        //                {
        //                    string instruction_no = CurrentErpUtil.ReplaceSpace(InstrNoOld);
        //                    if (checkShoCdData.Rows.Count == 0)
        //                    {
        //                        res = false;
        //                        FormMsgBoxUtil.Error(MessageFormsConsts.WMS9999CW0056.SetReplaces(ShoCd).ToString() + " - " +
        //                            Resources.InstructionNo + ": " + InstrNo +
        //                            "\r\n" + "Line : " + j);
        //                        return res;
        //                    }

        //                    if (string.IsNullOrEmpty(Qty))
        //                    {
        //                        res = false;
        //                        FormMsgBoxUtil.Information(MessageFormsConsts.WMS9999CW0044.SetReplaces(ShoCd).ToString() + "\r\n" + "Line : " + j);
        //                        return res;
        //                    }
        //                    // check line no
        //                    if (string.IsNullOrEmpty(LineNo))
        //                    {
        //                        string sqlCheckLineNo = "";
        //                        string sqlGetLineNo = "";
        //                        if (_isEnTable)
        //                        {
        //                            if (instruction_no.StartsWith("3"))
        //                            {
        //                                sqlCheckLineNo = $@"SELECT count(ISNULL(mc._Goods_CD,0)) as countGoods_CD
        //                            FROM TBL_PROC_H h
        //                            left join TBL_PROC_MC mc on h._PRESS_INST_NO = mc._PRESS_INST_NO
        //                            where h._PRESS_INST_NO = '{instruction_no}' and ISNULL(mc._Goods_CD,h._Goods_CD) = '{ShoCd}'";

        //                                sqlGetLineNo = $@"SELECT ISNULL(mc._LINE_NO,0) as _LINE_NO
        //                            FROM TBL_PROC_H h
        //                            left join TBL_PROC_MC mc on h._PRESS_INST_NO = mc._PRESS_INST_NO
        //                            where h._PRESS_INST_NO = '{instruction_no}' and ISNULL(mc._Goods_CD,h._Goods_CD)  = '{ShoCd}'";
        //                            }
        //                            else if (instruction_no.StartsWith("1"))
        //                            {
        //                                sqlCheckLineNo = $@"SELECT count(_Goods_CD) as countGoods_CD
        //                             FROM TBL_PO_DOMESTIC_RECORD
        //                             where _Order_No = '{instruction_no}' and _GOODS_CD = '{ShoCd}'";

        //                                sqlGetLineNo = $@"SELECT _LINE_NO
        //                             FROM TBL_PO_DOMESTIC_RECORD
        //                             where _Order_No = '{instruction_no}' and _GOODS_CD = '{ShoCd}'";
        //                            }
        //                            else
        //                            {
        //                                sqlCheckLineNo = $@"SELECT count(_Goods_CD) as countGoods_CD
        //                             FROM TBL_PO_INTERNATIONAL_RECORD
        //                             where _Order_No = '{instruction_no}' and _GOODS_CD = '{ShoCd}'";

        //                                sqlGetLineNo = $@"SELECT _LINE_NO
        //                             FROM TBL_PO_INTERNATIONAL_RECORD
        //                             where _Order_No = '{instruction_no}' and _GOODS_CD = '{ShoCd}'";
        //                            }
        //                        }
        //                        else
        //                        {
        //                            if (instruction_no.StartsWith("3"))
        //                            {
        //                                sqlCheckLineNo = $@"SELECT count(商品CD) as countGoods_CD
        //                            FROM TBL_PROC_MC m
        //                            where 印刷指示書NO = '{instruction_no}' and 商品CD = '{ShoCd}'";

        //                                sqlGetLineNo = $@"SELECT 行番号 as _LINE_NO
        //                            FROM TBL_PROC_MC m
        //                            where 印刷指示書NO = '{instruction_no}' and 商品CD = '{ShoCd}'";
        //                            }
        //                            else if (instruction_no.StartsWith("1"))
        //                            {
        //                                sqlCheckLineNo = $@"SELECT count(商品CD) as countGoods_CD
        //                             FROM TBL_発注書国内_RECORD
        //                             where  発注書NO = '{instruction_no}' and 商品CD = '{ShoCd}'";

        //                                sqlGetLineNo = $@"SELECT 行番号 as _LINE_NO
        //                             FROM TBL_発注書国内_RECORD
        //                             where  発注書NO = '{instruction_no}' and 商品CD = '{ShoCd}'";
        //                            }
        //                            else
        //                            {
        //                                sqlCheckLineNo = $@"SELECT count(商品CD) as countGoods_CD
        //                             FROM TBL_発注書国際_RECORD
        //                             where 発注書NO = '{instruction_no}' and 商品CD = '{ShoCd}'";

        //                                sqlGetLineNo = $@"SELECT 行番号 as _LINE_NO
        //                             FROM TBL_発注書国際_RECORD
        //                             where 発注書NO = '{instruction_no}' and 商品CD = '{ShoCd}'";
        //                            }
        //                        }
        //                        DataTable dtCheckLineNo = WebApiCommon.GetSQLTableValue(sqlCheckLineNo, LoginInfo.KyotenCd, _testFlg);
        //                        if (int.Parse(dtCheckLineNo.Rows[0][0].ToString()) > 1)
        //                        {
        //                            res = false;
        //                            FormMsgBoxUtil.Information(MessageFormsConsts.WMS9999CW0078.SetReplaces(ShoCd).ToString() + "\r\n" + "Line : " + j);
        //                            return res;
        //                        }
        //                        else
        //                        {
        //                            DataTable dtGetLineNo = WebApiCommon.GetSQLTableValue(sqlGetLineNo, LoginInfo.KyotenCd, _testFlg);
        //                            if (dtGetLineNo.Rows.Count > 0)
        //                            {
        //                                LineNo = dtGetLineNo.Rows[0][0].ToString();
        //                            }
        //                            else
        //                            {
        //                                LineNo = "0";
        //                            }
        //                        }
        //                    }
        //                    if (string.IsNullOrEmpty(StockType))
        //                    {
        //                        res = false;
        //                        FormMsgBoxUtil.Information(MessageFormsConsts.WMS9999CW0074.SetReplaces(ShoCd).ToString() + "\r\n" + "Line : " + j);
        //                        return res;
        //                    }
        //                    if (string.IsNullOrEmpty(ShoCd))
        //                    {
        //                        res = false;
        //                        FormMsgBoxUtil.Information(MessageFormsConsts.WMS9999CW0056.SetReplaces(ShoCd).ToString() + "\r\n" + "Line : " + j);
        //                        return res;
        //                    }

        //                    Hinmei = checkShoCdData.Rows[0]["sho_name"].ToString();
        //                    Unit = checkShoCdData.Rows[0]["tani"].ToString();
        //                    int qtyPlan = 0;
        //                    int qtyHistory = 0;

        //                    string sqlHistory = $@"select sum(d.suu) as suu
        //                                    from tbl_inout_history_h h 
        //                                    join tbl_inout_history_d d on h.id = d.tbl_inout_history_h_id and d.void_flg = 0
        //                                    where h.instruction_no = '{instruction_no}' 
        //                                    and h.kyoten_cd = '{LoginInfo.KyotenCd}' 
        //                                    and d.sho_cd = '{ShoCd}' 
        //                                    and d.zaiko_ku = '{zaikoKu}'
        //                                    and d.instruction_gyou = '{LineNo}'
        //                                    {(LoginInfo.KyotenCd == "KZ" ? $"and d.stock_location = '{SettingINI.StockLocation}'" : "")}";

        //                    DataTable dthistory = WebApiCommon.GetErpDataTable(sqlHistory);

        //                    if (dthistory.Rows.Count == 0)
        //                    {
        //                        string sqlHistory1 = "";
        //                        if (_isEnTable)
        //                        {
        //                            sqlHistory1 = $@"select sum(m._QTY) as suu
        //                            from [TBL_ST_GOODS_IH] h
        //                            join [TBL_ST_GOODS_IM] m on h._STOCKING_SLIP_NO = m._SHIPMENT_SLIP_NO
        //                            where h._PRESS_INST_NO = '{instruction_no}' 
        //                            and m._GOODS_CD = '{ShoCd}' 
        //                            and h._STORAGE_TYPE = '{StockTypeName}' 
        //                            and m.SUBTITLE = ''
        //                            and m._LINE_NO = '{LineNo}'";
        //                        }
        //                        else
        //                        {
        //                            sqlHistory1 = $@"select sum(m.数量) as suu
        //                            from TBL_ST_GOODS_IH h
        //                            join TBL_ST_GOODS_IM m on h.入庫単NO = m.出庫単NO
        //                            where h.印刷指示書NO = '{instruction_no}' 
        //                            and m.商品CD = '{ShoCd}' 
        //                            and h.物別 = '{StockTypeName}' 
        //                            and m.SUBTITLE = ''
        //                            and m.行番号 = '{LineNo}'";
        //                            if (LoginInfo.KyotenCd == "KZ")
        //                            {
        //                                sqlHistory1 = $@"select sum(m.数量) as suu
        //                            from TBL_ST_GOODS_IH h
        //                            join TBL_ST_GOODS_IM m on h.入庫単NO = m.出庫単NO
        //                            where h.印刷指示書NO = '{instruction_no}' 
        //                            and m.商品CD = '{ShoCd}' 
        //                            and h.物別 = '{StockTypeName}' 
        //                            and m.SUBTITLE = ''
        //                            and m.行番号 = '{LineNo}'
        //                            and h.在庫場所 = '{SettingINI.StockLocation}'";
        //                            }
        //                        }
        //                        dthistory = WebApiCommon.GetSQLTableValue(sqlHistory1, LoginInfo.KyotenCd, _testFlg);
        //                    }

        //                    if (dthistory.Rows.Count > 0)
        //                    {
        //                        qtyHistory = int.Parse(string.IsNullOrEmpty(dthistory.Rows[0]["suu"].ToString()) ? "0" : dthistory.Rows[0]["suu"].ToString());
        //                    }
        //                    DataTable dtPlan = new DataTable();
        //                    string sqlPlan = "";

        //                    if (_isEnTable)
        //                    {
        //                        if (instruction_no.StartsWith("3"))
        //                        {
        //                            sqlPlan = $@"SELECT sum(ISNULL(mc._QTY,h._QTY)) as suu,ISNULL(mc._Goods_CD,h._Goods_CD) as sho_cd
        //                    FROM TBL_PROC_H h
        //                    left join TBL_PROC_MC mc on h._PRESS_INST_NO = mc._PRESS_INST_NO
        //                    where h._PRESS_INST_NO = '{instruction_no}' 
        //                        and ISNULL(mc._Goods_CD,h._Goods_CD) = '{ShoCd}' 
        //                        and ISNULL(mc._LINE_NO,0) = case when mc._LINE_NO is null then 0 else '{LineNo}' end
        //                    Group by ISNULL(mc._Goods_CD,h._Goods_CD)";
        //                        }
        //                        else if (instruction_no.StartsWith("1"))
        //                        {
        //                            sqlPlan = $@"SELECT sum(_QTY) as suu,_GOODS_CD as sho_cd
        //                     FROM TBL_PO_DOMESTIC_RECORD
        //                     where _Order_No = '{instruction_no}' and _GOODS_CD = '{ShoCd}' and _LINE_NO = '{LineNo}'
        //                     Group by _Goods_CD";
        //                        }
        //                        else
        //                        {
        //                            sqlPlan = $@"SELECT sum(_QTY) as suu,_GOODS_CD as sho_cd
        //                     FROM TBL_PO_INTERNATIONAL_RECORD
        //                     where _Order_No = '{instruction_no}' and _GOODS_CD = '{ShoCd}' and _LINE_NO = '{LineNo}'
        //                     Group by _Goods_CD";
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (instruction_no.StartsWith("3"))
        //                        {
        //                            sqlPlan = $@"SELECT sum(ISNULL(mc.数量,h.数量)) as suu, ISNULL(mc.商品CD,h.商品CD) as sho_cd
        //                    FROM TBL_PROC_H h
        //                    left join TBL_PROC_MC mc on h.印刷指示書NO = mc.印刷指示書NO
        //                    where h.印刷指示書NO = '{instruction_no}' 
        //                        and ISNULL(mc.商品CD,h.商品CD) = '{ShoCd}' 
        //                        and ISNULL(mc.行番号,0) = case when mc.行番号 is null then 0 else '{LineNo}' end
        //                    Group by ISNULL(mc.商品CD,h.商品CD)";
        //                        }
        //                        else if (instruction_no.StartsWith("1"))
        //                        {
        //                            sqlPlan = $@"SELECT sum(数量) as suu, 商品CD as sho_cd
        //                     FROM TBL_発注書国内_RECORD
        //                     where  発注書NO = '{instruction_no}' and 商品CD = '{ShoCd}'  and 行番号 = '{LineNo}'
        //                     Group by 商品CD";
        //                        }
        //                        else
        //                        {
        //                            sqlPlan = $@"SELECT sum(数量) as suu, 商品CD as sho_cd
        //                     FROM TBL_発注書国際_RECORD
        //                     where 発注書NO = '{instruction_no}' and 商品CD = '{ShoCd}' and 行番号 = '{LineNo}'
        //                     Group by 商品CD";
        //                        }
        //                    }

        //                    dtPlan = WebApiCommon.GetSQLTableValue(sqlPlan, LoginInfo.KyotenCd, _testFlg);
        //                    if (dtPlan.Rows.Count > 0)
        //                    {
        //                        qtyPlan = int.Parse(string.IsNullOrEmpty(dtPlan.Rows[0]["suu"].ToString()) ? "0" : dtPlan.Rows[0]["suu"].ToString());
        //                    }
        //                    else
        //                    {
        //                        res = false;
        //                        FormMsgBoxUtil.Information(MessageFormsConsts.WMS9999CW0056.SetReplaces(ShoCd).ToString() + "\r\n" + "Line : " + j);
        //                        return res;
        //                    }

        //                    int qtymax = qtyPlan + (qtyPlan * 10 / 100);
        //                    if (int.Parse(Qty) > qtymax)
        //                    {
        //                        var conf = FormMsgBoxUtil.YesNo(MessageFormsConsts.WMS9999CW0084.ToString() + "\r\n" + "Line : " + j);
        //                        if (conf == false)
        //                        {
        //                            res = false;
        //                            return res;
        //                        }
        //                    }

        //                    var tblInoutHistoryD = new TblInoutHistoryD
        //                    {
        //                        InoutKu1 = 1,
        //                        InoutKu2 = InstrNo.StartsWith("3") ? 2 : 1,
        //                        InoutGyou = inoutGyou,
        //                        InstructionNo = CurrentErpUtil.ReplaceSpace(InstrNo),
        //                        InstructionGyou = int.Parse(LineNo),
        //                        PoNo = PoNo,
        //                        PoGyou = null,
        //                        EntryDate = DateTime.Now.ToString("yyyy/MM/dd"),
        //                        EntryTime = DateTime.Now.ToString("HH:mm:ss"),
        //                        ShoCd = ShoCd,
        //                        Hinmei = Hinmei,
        //                        Unit = Unit,
        //                        Suu = int.Parse(Qty),
        //                        GrpCd = checkShoCdData.Rows[0]["grp_cd"].ToString(),
        //                        ZaikoKu = zaikoKu,
        //                        InoutReason = Reason,
        //                        UpdatedAt = DateTimeOffset.Now,
        //                        UpdatedUserCd = LoginInfo.UserCd,
        //                        UpdatedUserName = LoginInfo.UserName,
        //                        CreatedAt = DateTimeOffset.Now,
        //                        CreatedUserCd = LoginInfo.UserCd,
        //                        CreatedUserName = LoginInfo.UserName,
        //                        ExpirationDate = ExpirationDate,
        //                        PlanQty = qtyPlan - qtyHistory < 0 ? 0 : qtyPlan - qtyHistory,
        //                        InoutNo = inoutNo,
        //                        MstCrShoKyotensId = int.Parse(checkShoCdData.Rows[0]["kyotenId"].ToString()),
        //                        StockBasho = checkShoCdData.Rows[0]["stock_basho"].ToString(),
        //                        FirstReceivedDate = DateTimeOffset.Now.ToString("yyyy/MM/dd"),
        //                        FirstShippedDate = DateTimeOffset.Now.ToString("yyyy/MM/dd"),
        //                        LastReceivedDate = DateTimeOffset.Now.ToString("yyyy/MM/dd"),
        //                        LastShippedDate = DateTimeOffset.Now.ToString("yyyy/MM/dd"),
        //                        StockQty = int.Parse(checkShoCdData.Rows[0]["stock_suu"].ToString()),
        //                    };
        //                    tblInoutHistoryDLists.Add(tblInoutHistoryD);
        //                    inoutGyou++;
        //                }
        //                else
        //                {
        //                    res = false;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message, ex);
        //    }
        //}
    }
}
