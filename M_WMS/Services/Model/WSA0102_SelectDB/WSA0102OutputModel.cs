namespace M_WMS.Services.Model.WSA0102_SelectDB
{
    public class WSA0102OutputModel : BaseModel
    {
        public List<Dictionary<string, string>> SelectResultDictionary { get; set; }
        /// <summary>
        /// DB検索結果(XML形式)
        /// </summary>
        public string SelectResult { get; set; }

        /// <summary>
        /// DB検索件数
        /// </summary>
        public int SelectCount { get; set; }

        /// <summary>
        /// エラーメッセージ
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// SQLSTATE
        /// </summary>
        public string SqlState { get; set; }
    }
}
