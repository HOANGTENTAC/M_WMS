using M_WMS.Model;
using System.Collections.ObjectModel;

namespace M_WMS.Helpers
{
    public static class DictionaryExtensions
    {
        public static ObservableCollection<WmsSelectOption> ToSelectOptions(
                        this List<Dictionary<string, string>>? source,
                        string valueKey = "id",
                        string textKey = "name")
        {
            return new ObservableCollection<WmsSelectOption>(
                source?.Select(x => new WmsSelectOption
                {
                    Value = x.TryGetValue(valueKey, out var value) ? value : string.Empty,
                    Name = x.TryGetValue(textKey, out var text) ? text : string.Empty
                }) ?? Enumerable.Empty<WmsSelectOption>());
        }
    }
}
