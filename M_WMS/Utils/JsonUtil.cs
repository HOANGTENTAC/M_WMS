using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M_WMS.Utils
{
    public class JsonUtil
    {
        public static string SerializeJson(object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.None, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }
        public static T DeserializeJson<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
