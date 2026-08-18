namespace M_WMS.Consts
{
    public class AppConsts
    {
        public static string AppName { get; set; } = "T-WMS";
        public static string Version { get; set; }
        public static string AppNameAndVersion { get; set; }
    }
    public class LoginInfo
    {
        public static List<object> GrpCdList { get; set; }
        /// <summary>
        /// 担当者CD
        /// </summary>
        public static string UserCd { get; set; }

        /// <summary>
        /// 担当者名
        /// </summary>
        public static string UserName { get; set; }

        /// <summary>
        /// 拠点CD
        /// </summary>
        public static string KyotenCd { get; set; }

        /// <summary>
        /// カルチャ
        /// </summary>
        public static string Culture { get; set; }

        /// <summary>
        /// 営業担当者CD
        /// </summary>
        public static string SalesTanCd { get; set; }

        public static string Email { get; set; }

        public static string StockLocation { get; set; }

        public static string UserNameAlias { get; set; }
    }
}
