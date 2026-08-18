using M_WMS.Helpers;
using M_WMS.Model;
using M_WMS.Services;
using System.Text.RegularExpressions;

namespace M_WMS.Utils
{
    public static class CurrentErpUtil
    {
        public static List<WmsSelectOption> GetStockType(int ArrivalOrShipment)
        {
            try
            {
                List<WmsSelectOption> list = new List<WmsSelectOption>();
                if (ArrivalOrShipment == 1)
                {
                    list.Add(new WmsSelectOption
                    {
                        //Name = "外部から仕入れたものを入庫する時",
                        //Name = "Input from outside bought items",
                        Name = LocalizationResourceManager.Instance["InOut_Ku_2_1_1"],
                        Value = "1"
                    });
                    list.Add(new WmsSelectOption
                    {
                        //Name = "製造したものを倉庫に入れるとき（製造入庫）",
                        //Name = "Input goods made from production",
                        Name = LocalizationResourceManager.Instance["InOut_Ku_2_1_2"],
                        Value = "2"
                    });
                    list.Add(new WmsSelectOption
                    {
                        //Name = "棚卸し等の在庫調整",
                        //Name = "Inventory adjustments",
                        Name = LocalizationResourceManager.Instance["InOut_Ku_2_1_4"],
                        Value = "4"
                    });
                }
                else if (ArrivalOrShipment == 2)
                {
                    list.Add(new WmsSelectOption
                    {
                        //Name = "倉庫から売り上げるために出庫する時",
                        //Name = "Output goods from the stock for sale",
                        Name = LocalizationResourceManager.Instance["InOut_Ku_2_2_1"],
                        Value = "1"
                    });
                    list.Add(new WmsSelectOption
                    {
                        //Name = "加工するの構成商品を出庫する",
                        //Name = "Output material for production process",
                        Name = LocalizationResourceManager.Instance["InOut_Ku_2_2_2"],
                        Value = "2"
                    });
                    list.Add(new WmsSelectOption
                    {
                        //Name = "サンプル出荷または、棚卸し等の在庫調整",
                        //Name = "Sample shipment or inventory adjustments",
                        Name = LocalizationResourceManager.Instance["InOut_Ku_2_2_4"],
                        Value = "4"
                    });
                }
                return list;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static List<WmsSelectOption> StockLocations(string kyoten)
        {
            List<WmsSelectOption> list = new List<WmsSelectOption>();
            switch (kyoten)
            {
                case "All":
                    list.Add(new WmsSelectOption{Name = "加須工場",Value = "加須工場"});
                    list.Add(new WmsSelectOption{Name = "越谷第二工場", Value = "越谷第二工場" });
                    list.Add(new WmsSelectOption{Name = "信濃町", Value = "信濃町" });
                    list.Add(new WmsSelectOption{Name = "VIETNAM", Value = "VIETNAM" });
                    list.Add(new WmsSelectOption{Name = "HANOI", Value = "HANOI" });
                    list.Add(new WmsSelectOption{Name = "THAILAND", Value = "THAILAND" });
                    list.Add(new WmsSelectOption{Name = "QINGDAO", Value = "QINGDAO" });
                    list.Add(new WmsSelectOption{Name = "SHANGHAI", Value = "SHANGHAI" });
                    break;
                case "HC":
                    list.Add(new WmsSelectOption { Name = "VIETNAM", Value = "VIETNAM" });
                    break;
                case "HN":
                    list.Add(new WmsSelectOption { Name = "HANOI", Value = "HANOI" });
                    break;
                case "TH":
                    list.Add(new WmsSelectOption { Name = "THAILAND", Value = "THAILAND" });
                    break;
                case "QD":
                    list.Add(new WmsSelectOption { Name = "QINGDAO", Value = "QINGDAO" });
                    break;
                case "SH":
                    list.Add(new WmsSelectOption { Name = "SHANGHAI", Value = "SHANGHAI" });
                    break;
                default:
                    list.Add(new WmsSelectOption { Name = "加須工場", Value = "加須工場" });
                    list.Add(new WmsSelectOption { Name = "越谷第二工場", Value = "越谷第二工場" });
                    list.Add(new WmsSelectOption { Name = "信濃町", Value = "信濃町" });
                    break;
            }
            return list;
        }
        public static List<WmsSelectOption> SelectLanguage()
        {
            List<WmsSelectOption> list = new List<WmsSelectOption>();
            list.Add(new WmsSelectOption { Name = "English", Value = "en" });
            list.Add(new WmsSelectOption { Name = "Tiếng Việt", Value = "vi" });
            list.Add(new WmsSelectOption { Name = "日本語", Value = "ja" });
            return list;
        }
        public static async Task<List<Dictionary<string,string>>> LoadReason(string? kyoten, ApiService _apiService)
        {
            string getReasonSql = $@"SELECT reason_cd as id, {(IsEnTable(kyoten) ? "reason_name_en" : "reason_name_jp")} as name
                    FROM mst_inout_reason";
            var resultTest = await _apiService.SelectDbDictionaryErp(getReasonSql);
            return resultTest ?? new List<Dictionary<string, string>>();
        }
        public static bool IsEnTable(string? receiverCd)
        {
            switch (receiverCd)
            {
                case "HK":
                case "TH":
                case "HC":
                case "HN":
                    // 発注先CDが、中国（香港）、タイ、ベトナム（ホーチミン、ハノイ）の場合
                    return true;
                case "JP":
                case "SH":
                case "QD":
                case "KZ":
                default:
                    // 発注先CDが、日本、中国（上海、青島）の場合
                    return false;
            }
        }
        public static string ReplaceSpace(string text)
        {
            return Regex.Replace(text, @"[^a-zA-Z0-9_+-]", "");
        }
        public static string LoadSheetInput(string kyotencd)
        {
            switch (kyotencd)
            {
                case "HC":
                case "HN":
                case "TH":
                    return "INPUT";
                default:
                    return "入庫";
            }
        }
        public static string LoadSheetOutput(string kyotencd)
        {
            switch (kyotencd)
            {
                case "HC":
                case "HN":
                case "TH":
                    return "OUTPUT";
                default:
                    return "出庫";
            }
        }
    }
}
