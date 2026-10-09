namespace FarmPlus.ScaleInput;

/// <summary>Formats an ASCII-only sequence of digits with exactly one implied decimal digit.</summary>
public static class WeightEntryFormatter
{
    public static string FormatDigits(string digits)
    {
        ArgumentNullException.ThrowIfNull(digits);
        if (digits.Length == 0) return string.Empty;
        foreach (char c in digits)
        {
            if (c < '0' || c > '9') throw new ArgumentException("Input must contain ASCII digits only.", nameof(digits));
        }
        if (digits.Length == 1) return "0." + digits;
        return digits[..^1] + "." + digits[^1];
    }

    public static decimal ParseDigits(string digits)
    {
        string formatted = FormatDigits(digits);
        if (formatted.Length == 0) throw new ArgumentException("Weight is empty.", nameof(digits));
        return decimal.Parse(formatted, System.Globalization.CultureInfo.InvariantCulture);
    }
}
