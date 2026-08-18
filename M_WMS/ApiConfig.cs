namespace M_WMS
{
    public class ApiConfig
    {
#if !DEBUG
        public const string CPOSApi = "https://p01-wap01.azurewebsites.net/";
        public const string ERPApi = "http://localhost:44357/";
#else
        public const string CPOSApi = "https://p01-wap01.azurewebsites.net/";
        public const string ERPApi = "https://p01-wap10-bubecrd4ezgbfrhj.japaneast-01.azurewebsites.net/";
#endif

        public static string GetUserInfoUrl => $"{CPOSApi}api/OCA0206/GetUserInfo";
        public static string SelectDbErpDictionary => $"{ERPApi}api/WSA0101/SelectDBDictionary";
        public static string SelectDbCmmsDictionary => $"{ERPApi}api/WSA0102/SelectCmmsDBDictionary";
        public static string GetOrderNumberUrl => $"{ERPApi}api/WSA0301/UpdateNumber";
        public static string RegistArrivalShipmentUrl => $"{ERPApi}api/WSA0201/RegistArrivalShipment";

    }
}
