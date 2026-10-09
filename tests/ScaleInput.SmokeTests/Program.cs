using FarmPlus.ScaleInput;
using System.Globalization;

int count = 0;
void Check(string digits, string expected)
{
    string actual = WeightEntryFormatter.FormatDigits(digits);
    if (actual != expected) throw new Exception($"FormatDigits({digits}) expected {expected}, got {actual}");
    if (digits.Length > 0)
    {
        decimal expectedValue = decimal.Parse(expected, CultureInfo.InvariantCulture);
        decimal actualValue = WeightEntryFormatter.ParseDigits(digits);
        if (expectedValue != actualValue) throw new Exception($"ParseDigits({digits}) expected {expectedValue}, got {actualValue}");
    }
    count++;
}
Check("", "");
Check("1", "0.1");
Check("12", "1.2");
Check("123", "12.3");
Check("1234", "123.4");
Check("12560", "1256.0");
Check("0004", "000.4");
Check("0", "0.0");

try
{
    WeightEntryFormatter.FormatDigits("12.3");
    throw new Exception("Expected an invalid-input exception");
}
catch (ArgumentException) { count++; }

Console.WriteLine($"PASS: {count} formatting and parsing checks. UI keyboard integration, persistence and printing are NOT tested.");
