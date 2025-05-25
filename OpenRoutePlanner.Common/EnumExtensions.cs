using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace OpenRoutePlanner;

public static class EnumExtensions
{
    public static string? Display(this Enum @enum, bool @short = false)
    {
        string value = @enum.ToString();
        string[] spl = value.Split(", ", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        for (int i = 0; i < spl.Length; i++)
        {
            MemberInfo? member = @enum.GetType().GetMember(spl[i]).FirstOrDefault();

            if (member == null)
            {
                continue;
            }

            DisplayAttribute? attribute = member.GetCustomAttribute<DisplayAttribute>(true);
            
            if (attribute == null)
            {
                continue;
            }

            spl[i] = @short ? attribute.GetShortName()! : attribute.GetName()!;
        }

        return string.Join(", ", spl);
    }

    public static IEnumerable<(Enum value, string name)> GetFlagValues<T>(bool @short = false) where T : Enum
    {
        return Enum.GetValues(typeof(T))
            .Cast<Enum>()
            .Where(v => Convert.ToInt32(v) != 0)
            .Select(v => (v, Display(v, @short)));
    }
}
