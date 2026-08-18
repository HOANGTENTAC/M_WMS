using M_WMS.Services.Model;
using M_WMS.Services.Model.WSA0101_SelectDB;
using M_WMS.Services.Model.WSA0301_UpdateNumber;
using M_WMS.Utils;
using Newtonsoft.Json;
using System.Data;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Text;
using static M_WMS.Model.UserInfoModel;

namespace M_WMS.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<UserInfoResponse?> GetUserInfoAsync(string userCode)
        {
            var response = await _httpClient.PostAsJsonAsync(
                ApiConfig.GetUserInfoUrl,
                new { UserCode = userCode });

            return await response.Content.ReadFromJsonAsync<UserInfoResponse>();
        }
        public async Task<List<Dictionary<string, string>>?> SelectDbDictionary(string sql, DbEnum db)
        {
            string ApiSelectApi = GetSelectApiUrl(db);
            List<Dictionary<string, string>> DictionaryValue = new List<Dictionary<string, string>>();
            try
            {
                var response = await _httpClient.PostAsJsonAsync(
                ApiSelectApi,
                new { Sql = sql });
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<WSA0101OutputModel>(json);
                DictionaryValue = result.SelectResultDictionary;

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            return DictionaryValue;
        }
        public async Task<List<Dictionary<string, string>>?> SelectDbDictionaryErp(string sql)
        {
            List<Dictionary<string, string>>? DictionaryValue = new List<Dictionary<string, string>>();
            DictionaryValue = await SelectDbDictionary(sql, DbEnum.ERP);
            return DictionaryValue;
        }
        public async Task<List<Dictionary<string, string>>?> SelectDbDictionaryCmms(string sql)
        {
            List<Dictionary<string, string>>? DictionaryValue = new List<Dictionary<string, string>>();
            DictionaryValue = await SelectDbDictionary(sql, DbEnum.CMMS);
            return DictionaryValue;
        }
        public Task<string> GetOrderNumberAsync(string orderKubun, string KotenCd = "")
        {
            var number = string.Empty;
            var reqData = new WSA0301InputModel
            {
                MstKey = orderKubun,
                SubKey = KotenCd
            };
            var url = $"{ApiConfig.GetOrderNumberUrl}";
            var resData = PostSync<WSA0301OutputModel>(url, reqData);
            switch (resData.Result)
            {
                case 0:
                    // 成功の場合
                    number = resData.UpdateNumber.ToString();
                    break;
                default:
                    // エラーの場合
                    //サーバでエラーが発生しました。処理結果
                    throw new Exception($"A server error occurred. Processing result：{resData.Result}");
            }
            return Task.FromResult(number);
        }

        public TResponseModel PostSync<TResponseModel>(string url, BaseModel requestModel, double? timeout = null) where TResponseModel : BaseModel
        {
            try
            {
                var reqJson = JsonConvert.SerializeObject(requestModel);
                var res = SyncUtil.RunSync(() => _httpClient.PostAsync(url, new StringContent(reqJson, Encoding.UTF8, "application/json")));
                var resJson = SyncUtil.RunSync(() => res.Content.ReadAsStringAsync());
                var resData = JsonUtil.DeserializeJson<TResponseModel>(resJson);

                // HTTPステータスコードエラー発生時
                if ((int)res.StatusCode >= 400)
                {
                    var status = (int)res.StatusCode;
                    //HTTP通信が失敗しました。HTTPステータスコード：{0}
                    throw new Exception($"HTTP request failed with status code {status}");
                }
                return resData;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private static string GetSelectApiUrl(DbEnum db)
        {
            switch (db)
            {
                case DbEnum.ERP:
                    // 取得対象がERPの場合
                    return ApiConfig.SelectDbErpDictionary;
                case DbEnum.CMMS:
                default:
                    // 取得対象がCMMSの場合
                    return ApiConfig.SelectDbCmmsDictionary;
            }
        }
        public DataTable ConvertToDataTable(string xml)
        {
            DataSet ds = new DataSet();

            using (StringReader sr = new StringReader(xml))
            {
                ds.ReadXml(sr);
            }

            if (ds.Tables.Count > 0)
                return ds.Tables[0];

            return new DataTable();
        }
    }
}
