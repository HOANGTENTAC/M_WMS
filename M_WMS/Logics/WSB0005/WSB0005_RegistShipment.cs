using ExcelDataReader;
using M_WMS.Consts;
using M_WMS.Enums;
using M_WMS.Model;
using M_WMS.Services;
using M_WMS.Services.Popups;
using M_WMS.Utils;
using System.Collections.ObjectModel;
using System.Data;

namespace M_WMS.Logics.WSB0005
{
    public class WSB0005_RegistShipment
    {
        public WSB0005_RegistShipment()
        {
        }

        public async Task Execute(ObservableCollection<StockInoutTenplate> stockInoutTenplate, IPopupDialogService _popupService, ApiService _apiService)
        {
            var excelFileType = new FilePickerFileType(
                    new Dictionary<DevicePlatform, IEnumerable<string>>
                    {
                        { DevicePlatform.iOS, new[] { "org.openxmlformats.officedocument.spreadsheetml.sheet", "com.microsoft.excel.xls" } },
                        { DevicePlatform.Android, new[] { "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "application/vnd.ms-excel" } },
                        { DevicePlatform.WinUI, new[] { ".xlsx", ".xls" } },
                        { DevicePlatform.MacCatalyst, new[] { "xlsx", "xls" } },
                    });

            var pickResult = await FilePicker.Default.PickAsync(new PickOptions
            {
                PickerTitle = "Vui lòng chọn file Excel",
                FileTypes = excelFileType
            });

            if (pickResult == null) return;

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            // Tên Sheet bạn muốn đọc (Thay đổi theo tên Sheet thực tế trong file Excel của bạn)
            string targetSheetName = CurrentErpUtil.LoadSheetOutput(LoginInfo.KyotenCd);

            using (var stream = await pickResult.OpenReadAsync())
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    // Chuyển toàn bộ file Excel thành DataSet
                    var resultDataSet = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = true // Tự động lấy dòng đầu tiên làm tên cột
                        }
                    });

                    // 1. Kiểm tra xem Sheet có tồn tại hay không
                    if (!resultDataSet.Tables.Contains(targetSheetName))
                    {
                        await _popupService.ShowPopupAsync("Lỗi", $"Không tìm thấy Sheet có tên '{targetSheetName}' trong file!", PopupType.Question, "OK");
                        return;
                    }

                    // 2. Lấy đúng Sheet cần đọc theo tên
                    DataTable targetTable = resultDataSet.Tables[targetSheetName];
                    stockInoutTenplate.Clear();

                    // 3. Duyệt qua từng dòng dữ liệu trong Sheet đó
                    foreach (DataRow row in targetTable.Rows)
                    {
                        // Đọc theo tên cột (Header) hoặc theo chỉ số cột row[0], row[1]...
                        string GoodsCD = row["Goods CD"]?.ToString();
                        string LineNo = row["Line No"]?.ToString();
                        string StockType = row["Stock Type"]?.ToString();
                        int Quantity = int.Parse(row["Quantity"].ToString());
                        string InstructionNo = row["Instruction No"]?.ToString();

                        if (!string.IsNullOrEmpty(InstructionNo))
                        {
                            stockInoutTenplate.Add(new StockInoutTenplate
                            {
                                StockType = StockType,
                                InstrNo = InstructionNo,
                                LineNo = null,
                                GoosdCd = GoodsCD,
                                Qty = Quantity
                            });
                        }
                    }
                }
            }
        }
    }
}
