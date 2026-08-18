using CommunityToolkit.Mvvm.ComponentModel;

namespace M_WMS.Model
{
    public partial class ArrivalItem : ObservableObject
    {
        [ObservableProperty]
        private string instrNo = string.Empty;

        [ObservableProperty]
        private string poNo = string.Empty;

        [ObservableProperty]
        private string goodsCd = string.Empty;
        [ObservableProperty]
        private string goodsName = string.Empty;

        [ObservableProperty]
        private string stockType = string.Empty;

        [ObservableProperty]
        private int qty;

        [ObservableProperty]
        private int planQty;

        [ObservableProperty]
        private string remark = "";

        [ObservableProperty]
        private string status = "";

        [ObservableProperty]
        private int instrgyou;

        [ObservableProperty]
        private int instructionGyou;

        [ObservableProperty]
        private int? poGyou;

        [ObservableProperty]
        private string unit;

        [ObservableProperty]
        private string grpCd;

        [ObservableProperty]
        private int? kyotenId;

        [ObservableProperty]
        private string keepLocation;

        [ObservableProperty]
        private int? shoId;

        [ObservableProperty]
        private int? stockQty;
        //private string PoNo {  get; set; }
        //private string InstNo { get; set; }
        //public string GoodsCd { get; set; }
        //public string Status { get; set; }
        //public int Qty { get; set; }
        //public string StockType { get; set; }
        //public string Remark { get; set; }
    }
}
