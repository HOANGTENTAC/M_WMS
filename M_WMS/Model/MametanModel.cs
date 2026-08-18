using CommunityToolkit.Mvvm.ComponentModel;
using M_WMS.Enums;

namespace M_WMS.Model
{
    public partial class MametanModel : ObservableObject
    {
        [ObservableProperty]
        public string id;
        [ObservableProperty]
        public string instrNo;
        [ObservableProperty]
        public string shoCd;
        [ObservableProperty]
        public int suu;
        [ObservableProperty]
        public string stockType;
        [ObservableProperty]
        public string stockTypeName;
        [ObservableProperty]
        public string statusShoCd;
        [ObservableProperty]
        private bool isSelected;
        [ObservableProperty]
        private Brush rowBackground = Brush.Transparent;
        [ObservableProperty]
        public int instrgyou;
        [ObservableProperty]
        public int? kyotensId;
        [ObservableProperty]
        public string keepLocation;
        [ObservableProperty]
        private int? shoId;
        [ObservableProperty]
        private int? stockQty;
        [ObservableProperty]
        private string poNo;
        [ObservableProperty]
        private string goodsName;
        [ObservableProperty]
        private string unit;
        [ObservableProperty]
        private string grpCd;
    }
}
