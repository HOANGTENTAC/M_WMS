namespace M_WMS.Controls.Helpers
{
    internal static class DisplayHelper
    {
        public static string GetDisplayText(object? item, string displayMemberPath)
        {
            if (item == null)
                return string.Empty;

            if (string.IsNullOrWhiteSpace(displayMemberPath))
                return item.ToString() ?? string.Empty;

            var property = item.GetType().GetProperty(displayMemberPath);

            if (property == null)
                return item.ToString() ?? string.Empty;

            return property.GetValue(item)?.ToString() ?? string.Empty;
        }
        public static object? GetPropertyValue(object? item,string? propertyName)
        {
            if (item == null)
                return null;

            if (string.IsNullOrWhiteSpace(propertyName))
                return item;

            var property = item
                .GetType()
                .GetProperty(propertyName);

            return property?.GetValue(item);
        }
        public static bool PropertyEquals(
        object? item,
        string? propertyName,
        object? value)
        {
            var propertyValue = GetPropertyValue(item, propertyName);

            return Equals(propertyValue, value);
        }
    }
}
