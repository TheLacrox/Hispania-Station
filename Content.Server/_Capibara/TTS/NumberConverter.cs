// SPDX-FileCopyrightText: 2025 Capibara Station Contributors
// SPDX-License-Identifier: AGPL-3.0-or-later

using System.Text.RegularExpressions;

namespace Content.Server._Capibara.TTS;

/// <summary>
/// Converts numbers in text to their word representations for more natural TTS output.
/// </summary>
public static partial class NumberConverter
{
    private static readonly string[] Ones =
    {
        "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine",
        "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen",
        "seventeen", "eighteen", "nineteen"
    };

    private static readonly string[] Tens =
    {
        "", "", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety"
    };

    /// <summary>
    /// Replace numeric sequences in text with word equivalents.
    /// Only converts numbers up to 999 to keep output reasonable.
    /// </summary>
    public static string ConvertNumbersToWords(string text)
    {
        return NumberRegex().Replace(text, match =>
        {
            if (!int.TryParse(match.Value, out var number))
                return match.Value;

            if (number is < 0 or > 999)
                return match.Value;

            return NumberToWords(number);
        });
    }

    private static string NumberToWords(int number)
    {
        if (number < 20)
            return Ones[number];

        if (number < 100)
        {
            var result = Tens[number / 10];
            if (number % 10 != 0)
                result += " " + Ones[number % 10];
            return result;
        }

        // 100-999
        var hundreds = Ones[number / 100] + " hundred";
        var remainder = number % 100;
        if (remainder == 0)
            return hundreds;
        return hundreds + " and " + NumberToWords(remainder);
    }

    [GeneratedRegex(@"\b\d+\b")]
    private static partial Regex NumberRegex();
}
