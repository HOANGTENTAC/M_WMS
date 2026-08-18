using M_WMS.Services.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace M_WMS.Extensions
{
    public static class ModelExtensions
    {
        public static string ToStringObject<T>(this T obj)
        {
            try
            {
                if (obj == null)
                {
                    return "null";
                }

                if (obj is string)
                {
                    return obj as string;
                }

                if (obj is DateTime || obj is TimeSpan)
                {
                    return $"\"{obj}\"";
                }

                if (obj is byte[])
                {
                    byte[] array = obj as byte[];
                    return $"binary({array.Length}bytes)";
                }

                if (obj is IEnumerable)
                {
                    IEnumerable enumerable = obj as IEnumerable;
                    List<string> list = new List<string>();
                    foreach (object item in enumerable)
                    {
                        if (list.Count > 100)
                        {
                            list.Add($"...({list.Count}items)");
                            break;
                        }

                        list.Add(item.ToStringObject());
                    }

                    return "[" + string.Join(",", list) + "]";
                }

                if (obj.GetType().Namespace.StartsWith("TENTAC") || obj is BaseModel)
                {
                    IEnumerable<PropertyInfo> enumerable2 = from c in obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                                            where c.CanRead
                                                            select c;
                    List<string> list2 = new List<string>();
                    foreach (PropertyInfo item2 in enumerable2)
                    {
                        list2.Add("\"" + item2.Name + "\":" + item2.GetValue(obj).ToStringObject());
                    }

                    return "{" + string.Join(",", list2) + "}";
                }
            }
            catch
            {
            }

            return obj.ToString();
        }
    }
    public class NumberFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (int.TryParse(value?.ToString(), out int number))
                return number.ToString("N0"); // format 1,000
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (int.TryParse(value?.ToString(), out var number))
                return number;
            return 0;
        }
    }
}
