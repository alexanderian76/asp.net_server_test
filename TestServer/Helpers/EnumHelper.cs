using System.Reflection;
using TestServer.Attributes;


public static class EnumHelper
{
    public static string GetName(Enum value)
    {
        var name = value.GetType()?
                        .GetMember(value.ToString())?
                        .FirstOrDefault()?
                        .GetCustomAttribute<NameAttribute>()?
                        .Name;
        if (name == null)
            name = value.ToString();
        return name;
    }

    public static T GetEnumFromName<T>(string name) where T : Enum
    {
        foreach (var value in Enum.GetValues(typeof(T)))
        {
            if (value == null)
                continue;

            var strValue = value.ToString();

            if (strValue == null)
                continue;

            var attribute = value.GetType()?.GetMember(strValue)?.FirstOrDefault()?.GetCustomAttribute<NameAttribute>();
            var fieldName = attribute == null ? value.GetType()?.GetMember(strValue)?.FirstOrDefault()?.Name : attribute.Name;
            if (fieldName == name)
                return (T)value;
        }
        throw new Exception("There is no enum with this name");
    }
}

