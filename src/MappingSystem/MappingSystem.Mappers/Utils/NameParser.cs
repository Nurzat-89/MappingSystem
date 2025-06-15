namespace MappingSystem.Mappers.Utils;

public class NameParser
{
    public static (string FirstName, string LastName) ParseFullName(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            return ("", "");

        // "LastName, FirstName"
        if (fullName.Contains(','))
        {
            var parts = fullName.Split(',', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length >= 2)
                return (parts[1].Trim(), parts[0].Trim());
            if (parts.Length == 1)
                return ("", parts[0].Trim());
        }

        // "FirstName LastName"
        var words = fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return words.Length switch
        {
            >= 2 => (words.First(), string.Join(' ', words.Skip(1))),
            1 => (words[0], ""),
            _ => ("", "")
        };
    }
}