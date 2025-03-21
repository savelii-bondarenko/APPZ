using System.Globalization;
using System.Text.RegularExpressions;

namespace Lab1_3.Services;

public class ValidationService
{
    private static readonly Regex NameRegex = new(@"^[a-zA-Z0-9а-яА-ЯіІїЇєЄґҐ\s\-_]{3,50}$", RegexOptions.Compiled);
    private static readonly Regex NumberRegex = new(@"^[0-9]+$", RegexOptions.Compiled);
    private static readonly Regex DecimalRegex = new(@"^[0-9]+(\.[0-9]+)?$", RegexOptions.Compiled);


    public static readonly Dictionary<string, string> ValidationErrors = new()
    {
        { "name_required", "Назва не може бути порожньою" },
        { "name_invalid", "Назва містить неприпустимі символи або занадто коротка/довга (допустимо 3-50 символів)" },
        { "number_required", "Необхідно ввести число" },
        { "number_invalid", "Введене значення не є числом" },
        { "decimal_required", "Необхідно ввести десяткове число" },
        { "decimal_invalid", "Введене значення не є десятковим числом" },
        { "out_of_range", "Число виходить за допустимі межі" },
        { "option_invalid", "Виберіть один з доступних варіантів" },
        { "genre_invalid", "Вибраний жанр не існує" },
        { "device_type_invalid", "Вибраний тип пристрою не існує" },
        { "boolean_invalid", "Введіть 'Так' або 'Ні'" }
    };

    public bool IsValidName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return false;

        return NameRegex.IsMatch(name);
    }

    // Перевірка валідності цілого числа
    public bool TryParseInt(string input, out int number)
    {
        if (string.IsNullOrWhiteSpace(input) || !NumberRegex.IsMatch(input))
        {
            number = 0;
            return false;
        }

        return int.TryParse(input, out number);
    }

    public bool TryParseIntInRange(string input, int min, int max, out int number)
    {
        if (!TryParseInt(input, out number))
            return false;

        return number >= min && number <= max;
    }

    public int ParseInt(string input, int min, int max, int defaultValue = 0)
    {
        if (TryParseIntInRange(input, min, max, out var number))
            return number;

        return defaultValue;
    }

    public bool TryParseDouble(string input, out double number)
    {
        input = input?.Replace(',', '.') ?? string.Empty;
        if (string.IsNullOrWhiteSpace(input) || !DecimalRegex.IsMatch(input))
        {
            number = 0;
            return false;
        }

        return double.TryParse(input, NumberStyles.Any,
            CultureInfo.InvariantCulture, out number);
    }

    public bool TryParseDoubleInRange(string input, double min, double max, out double number)
    {
        if (!TryParseDouble(input, out number))
            return false;

        return number >= min && number <= max;
    }

    public bool TryParseBoolean(string input, out bool result)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            result = false;
            return false;
        }

        input = input.Trim().ToLower();
        if (input == "так" || input == "yes" || input == "y" || input == "1")
        {
            result = true;
            return true;
        }

        if (input == "ні" || input == "no" || input == "n" || input == "0")
        {
            result = false;
            return true;
        }

        result = false;
        return false;
    }

    public bool ParseBool(string input, bool defaultValue = false)
    {
        if (TryParseBoolean(input, out var result)) return result;

        return defaultValue;
    }

    public bool IsValidMenuOption(string input, int minOption, int maxOption, out int option)
    {
        if (string.IsNullOrWhiteSpace(input) || !NumberRegex.IsMatch(input))
        {
            option = -1;
            return false;
        }

        if (!int.TryParse(input, out option)) return false;

        return option >= minOption && option <= maxOption;
    }
}