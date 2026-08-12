using System;
using System.Text;

namespace Binance.FIX.Api;

internal static class BinanceFixLogRedactor
{
    internal const string RedactedValue = "[REDACTED]";

    internal static string Redact(string fixMessage)
    {
        ArgumentNullException.ThrowIfNull(fixMessage);

        if (fixMessage.Length == 0)
        {
            return fixMessage;
        }

        var redacted = new StringBuilder(fixMessage.Length);
        var fieldStart = 0;

        for (var index = 0; index <= fixMessage.Length; index++)
        {
            if (index < fixMessage.Length && fixMessage[index] != '\u0001')
            {
                continue;
            }

            AppendField(redacted, fixMessage.AsSpan(fieldStart, index - fieldStart));
            if (index < fixMessage.Length)
            {
                redacted.Append('\u0001');
            }

            fieldStart = index + 1;
        }

        return redacted.ToString();
    }

    private static void AppendField(StringBuilder destination, ReadOnlySpan<char> field)
    {
        var equalsIndex = field.IndexOf('=');
        if (equalsIndex <= 0 || !IsSensitiveTag(field[..equalsIndex]))
        {
            destination.Append(field);
            return;
        }

        destination.Append(field[..(equalsIndex + 1)]);
        destination.Append(RedactedValue);
    }

    private static bool IsSensitiveTag(ReadOnlySpan<char> tag)
        => tag.SequenceEqual("96") || tag.SequenceEqual("553");
}
