namespace MyBudgetly.Domain.Utility;

public static class ToStringUtility
{
    /// <summary>
    /// Generates a string representation of an object type and its properties.
    /// </summary>
    public static string ToString<T>(params (string propName, object? propValue)[] properties)
    {
        return ToString(typeof(T), properties);
    }

    public static string ToString(Type objectType, params (string propName, object? propValue)[] properties)
    {
        if (objectType == null)
            throw new ArgumentNullException(nameof(objectType));
        if (properties == null)
            throw new ArgumentNullException(nameof(properties));

        if (properties.Length == 0)
            return objectType.Name;

        var formattedProps = properties.Select(p =>
        {
            var value = FormatValue(p.propValue);
            return $"{p.propName}={value}";
        });

        return $"{objectType.Name}: {string.Join("; ", formattedProps)}";
    }

    private static string FormatValue(object? value)
    {
        return value switch
        {
            null => "null",
            string s => $"\"{s}\"",
            DateTime dt => dt.ToString("u"), // universal format
            _ => value.ToString() ?? "null"
        };
    }
}