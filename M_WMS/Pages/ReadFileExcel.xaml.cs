using ExcelDataReader;
using M_WMS.Consts;
using M_WMS.Model;
using M_WMS.Utils;
using M_WMS.ViewModel;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace M_WMS.Pages;

public partial class ReadFileExcel : ContentPage
{
	public ReadFileExcel(ReadFileExcelViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
    //private async void OnPickAndReadExcelClicked(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        var excelFileType = new FilePickerFileType(
    //            new Dictionary<DevicePlatform, IEnumerable<string>>
    //            {
    //            { DevicePlatform.iOS, new[] { "org.openxmlformats.officedocument.spreadsheetml.sheet", "com.microsoft.excel.xls" } },
    //            { DevicePlatform.Android, new[] { "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "application/vnd.ms-excel" } },
    //            { DevicePlatform.WinUI, new[] { ".xlsx", ".xls" } },
    //            { DevicePlatform.MacCatalyst, new[] { "xlsx", "xls" } },
    //            });

    //        var pickResult = await FilePicker.Default.PickAsync(new PickOptions
    //        {
    //            PickerTitle = "Vui lòng chọn file Excel",
    //            FileTypes = excelFileType
    //        });

    //        if (pickResult == null) return;

    //        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

    //        var StockInouts = new List<StockInoutTenplate>();

    //        // Tên Sheet bạn muốn đọc (Thay đổi theo tên Sheet thực tế trong file Excel của bạn)
    //        string targetSheetName = CurrentErpUtil.LoadSheetInput(LoginInfo.KyotenCd);

    //        using (var stream = await pickResult.OpenReadAsync())
    //        {
    //            using (var reader = ExcelReaderFactory.CreateReader(stream))
    //            {
    //                // Chuyển toàn bộ file Excel thành DataSet
    //                var resultDataSet = reader.AsDataSet(new ExcelDataSetConfiguration()
    //                {
    //                    ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
    //                    {
    //                        UseHeaderRow = true // Tự động lấy dòng đầu tiên làm tên cột
    //                    }
    //                });

    //                // 1. Kiểm tra xem Sheet có tồn tại hay không
    //                if (!resultDataSet.Tables.Contains(targetSheetName))
    //                {
    //                    await DisplayAlert("Lỗi", $"Không tìm thấy Sheet có tên '{targetSheetName}' trong file!", "OK");
    //                    return;
    //                }

    //                // 2. Lấy đúng Sheet cần đọc theo tên
    //                DataTable targetTable = resultDataSet.Tables[targetSheetName];

    //                // 3. Duyệt qua từng dòng dữ liệu trong Sheet đó
    //                foreach (DataRow row in targetTable.Rows)
    //                {
    //                    // Đọc theo tên cột (Header) hoặc theo chỉ số cột row[0], row[1]...
    //                    string GoodsCD = row["Goods CD"]?.ToString();
    //                    string LineNo = row["Line No"]?.ToString();
    //                    string StockType = row["Stock Type"]?.ToString();
    //                    int Quantity = int.Parse(row["Quantity"].ToString());
    //                    string InstructionNo = row["Instruction No"]?.ToString();

    //                    if (!string.IsNullOrEmpty(InstructionNo))
    //                    {
    //                        StockInouts.Add(new StockInoutTenplate
    //                        {
    //                            StockType = StockType,
    //                            InstrNo = InstructionNo,
    //                            LineNo = null,
    //                            GoosdCd = GoodsCD,
    //                            Qty = Quantity
    //                        });
    //                    }
    //                }
    //            }
    //        }

    //        // Cập nhật lên giao diện
    //        NhanVienCollectionView.ItemsSource = StockInouts;
    //        StatusLabel.Text = $"Đã đọc thành công {StockInouts.Count} dòng từ Sheet '{targetSheetName}'!";
    //    }
    //    catch (Exception ex)
    //    {
    //        await DisplayAlert("Lỗi", ex.Message, "OK");
    //    }
    //}
}