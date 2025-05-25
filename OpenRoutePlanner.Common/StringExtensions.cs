using System.Collections;
using System.Net;
using System.Reflection;
using System.Text;

namespace OpenRoutePlanner;
public static class StringExtensions
{
    public static string ToQueryString(this string baseString, object? obj)
    {
        if (obj == null)
        {
            return baseString;
        }

        var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var builder = new StringBuilder(baseString)
            .Append('?');

        foreach (PropertyInfo prop in properties)
        {
            
            var value = prop.GetValue(obj);

            if (value == null)
            {
                continue;
            }

            string key = WebUtility.UrlEncode(ToCamelCase(prop.Name));

            if (value is IEnumerable enumerable && value is not string)
            {
                foreach (var item in enumerable)
                {
                    if (item == null) continue;
                    builder.Append($"{key}={WebUtility.UrlEncode(ConvertToString(item))}&");
                }
            }
            else
            {
                builder.Append($"{key}={WebUtility.UrlEncode(ConvertToString(value))}&");
            }
        }

        if (builder.Length > baseString.Length + 1)
        {
            builder.Length--;
        }

        return builder.ToString();
    }

    private static string ConvertToString(object value)
    {
        return value switch
        {
            DateTimeOffset dto => dto.ToString("o"),
            DateTime dt => dt.ToString("o"),
            Enum e => e.ToString(),
            Guid g => g.ToString(),
            _ => value.ToString() ?? string.Empty
        };
    }

    private static string ToCamelCase(string name)
    {
        if (string.IsNullOrWhiteSpace(name) || char.IsLower(name[0]))
        {
            return name;
        }

        return char.ToLowerInvariant(name[0]) + name.Substring(1);
    }
}
