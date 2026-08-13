using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Xml.Linq;

namespace Binance.FIX.Api;

internal static class BinanceFixDataDictionaryVerifier
{
    internal const long OrderEntryLength = 24_513;
    internal const string OrderEntrySha256 = "55891d2ae2c7b5a5e9dbec0003ddcfd3034ba5846e0b7fa6561defa8f44797e9";
    internal const long MarketDataLength = 12_374;
    internal const string MarketDataSha256 = "b18432105ae64f24acc49e1ff1e357811ccfb5a84c7475cfba7347ad9e30a2ec";

    internal static BinanceFixVerifiedDataDictionary VerifyRequired(
        BinanceFixSessionRole role,
        string? path)
    {
        if (path is null)
        {
            throw new InvalidOperationException(
                "A caller-owned official Binance QuickFIX data dictionary is required before constructing a FIX session.");
        }

        return role switch
        {
            BinanceFixSessionRole.OrderEntry or BinanceFixSessionRole.DropCopy =>
                CreateOrderEntryRuntimeDictionary(Verify(path, OrderEntryLength, OrderEntrySha256)),
            BinanceFixSessionRole.MarketData =>
                Verify(path, MarketDataLength, MarketDataSha256),
            _ => throw new ArgumentOutOfRangeException(nameof(role), role, "Unsupported FIX session role.")
        };
    }

    internal static BinanceFixVerifiedDataDictionary Verify(
        string path,
        long expectedLength,
        string expectedSha256)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        if (expectedLength < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(expectedLength));
        }

        if (expectedSha256.Length != 64 ||
            expectedSha256.Any(character =>
                character is not (>= '0' and <= '9') and not (>= 'a' and <= 'f')))
        {
            throw new ArgumentException("Expected SHA-256 must be 64 lowercase hexadecimal characters.", nameof(expectedSha256));
        }

        var fullPath = Path.GetFullPath(path);
        var stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read);
        try
        {
            if (stream.Length != expectedLength)
            {
                throw new InvalidDataException(
                    $"Binance FIX data dictionary length mismatch. Expected {expectedLength.ToString(CultureInfo.InvariantCulture)} bytes but found {stream.Length.ToString(CultureInfo.InvariantCulture)} bytes.");
            }

            var actualSha256 = Convert.ToHexString(SHA256.HashData(stream)).ToLowerInvariant();
            if (!string.Equals(actualSha256, expectedSha256, StringComparison.Ordinal))
            {
                throw new InvalidDataException(
                    $"Binance FIX data dictionary SHA-256 mismatch. Expected {expectedSha256} but found {actualSha256}.");
            }

            return new BinanceFixVerifiedDataDictionary(fullPath, stream);
        }
        catch
        {
            stream.Dispose();
            throw;
        }
    }

    internal static BinanceFixVerifiedDataDictionary CreateOrderEntryRuntimeDictionary(
        BinanceFixVerifiedDataDictionary verifiedSource)
    {
        ArgumentNullException.ThrowIfNull(verifiedSource);

        try
        {
            var document = XDocument.Load(verifiedSource.Content, LoadOptions.PreserveWhitespace);
            var messages = document.Root?.Element("messages")?.Elements("message")
                .Where(element => string.Equals((string?)element.Attribute("name"), "ListStatus", StringComparison.Ordinal))
                .ToArray() ?? [];
            if (messages.Length != 1)
            {
                throw new InvalidDataException("The verified Order Entry dictionary must contain exactly one ListStatus message.");
            }

            var message = messages[0];
            var orderGroups = message.Elements("group")
                .Where(element => string.Equals((string?)element.Attribute("name"), "NoOrders", StringComparison.Ordinal))
                .ToArray();
            if (orderGroups.Length != 1)
            {
                throw new InvalidDataException("The verified ListStatus definition must contain exactly one NoOrders group.");
            }

            var orderGroup = orderGroups[0];
            var actualOrderMembers = orderGroup.Elements()
                .Select(GetNamedElementIdentity)
                .ToArray();
            string[] expectedOrderMembers =
            [
                "field:ClOrdID",
                "field:Symbol",
                "field:OrderID",
                "component:ListTriggeringInstruction",
                "field:OrdRejReason",
                "field:ErrorCode",
                "field:Text"
            ];
            if (!actualOrderMembers.SequenceEqual(expectedOrderMembers, StringComparer.Ordinal))
            {
                throw new InvalidDataException("The verified ListStatus NoOrders definition no longer matches the reviewed source shape.");
            }

            var membersByIdentity = orderGroup.Elements()
                .ToDictionary(GetNamedElementIdentity, element => element, StringComparer.Ordinal);
            orderGroup.ReplaceNodes(
                membersByIdentity["field:Symbol"],
                membersByIdentity["field:OrderID"],
                membersByIdentity["field:ClOrdID"],
                membersByIdentity["component:ListTriggeringInstruction"],
                membersByIdentity["field:OrdRejReason"],
                membersByIdentity["field:ErrorCode"],
                membersByIdentity["field:Text"]);

            var transactionTime = message.Elements("field").SingleOrDefault(element =>
                string.Equals((string?)element.Attribute("name"), "TransactTime", StringComparison.Ordinal));
            if (transactionTime is null)
            {
                throw new InvalidDataException("The verified ListStatus definition is missing TransactTime.");
            }

            transactionTime.AddBeforeSelf(
                new XElement("field", new XAttribute("name", "OrdRejReason"), new XAttribute("required", "N")));
            transactionTime.AddAfterSelf(
                new XElement("field", new XAttribute("name", "ErrorCode"), new XAttribute("required", "N")),
                new XElement("field", new XAttribute("name", "Text"), new XAttribute("required", "N")));

            var listIdDefinitions = document.Root?.Element("fields")?.Elements("field")
                .Where(element =>
                    string.Equals((string?)element.Attribute("number"), "66", StringComparison.Ordinal) &&
                    string.Equals((string?)element.Attribute("name"), "ListID", StringComparison.Ordinal))
                .ToArray() ?? [];
            if (listIdDefinitions.Length != 1 ||
                !string.Equals((string?)listIdDefinitions[0].Attribute("type"), "INT", StringComparison.Ordinal))
            {
                throw new InvalidDataException("The verified ListID definition no longer matches the reviewed INT source shape.");
            }

            listIdDefinitions[0].SetAttributeValue("type", "STRING");

            byte[] runtimeBytes;
            using (var runtimeContent = new MemoryStream())
            {
                document.Save(runtimeContent, SaveOptions.DisableFormatting);
                runtimeBytes = runtimeContent.ToArray();
            }

            var runtimeSha256 = Convert.ToHexString(SHA256.HashData(runtimeBytes)).ToLowerInvariant();
            var runtimeDirectory = System.IO.Path.Combine(
                System.IO.Path.GetTempPath(),
                "Binance.Api",
                "fix-dictionaries");
            Directory.CreateDirectory(runtimeDirectory);
            var runtimePath = System.IO.Path.Combine(
                runtimeDirectory,
                $"spot-fix-oe-{runtimeSha256}.xml");
            var stagingPath = System.IO.Path.Combine(
                runtimeDirectory,
                $".{Guid.NewGuid():N}.tmp");
            try
            {
                if (!File.Exists(runtimePath))
                {
                    using (var staging = new FileStream(stagingPath, FileMode.CreateNew, FileAccess.Write, FileShare.None))
                    {
                        staging.Write(runtimeBytes);
                        staging.Flush(flushToDisk: true);
                    }

                    try
                    {
                        File.Move(stagingPath, runtimePath);
                    }
                    catch (IOException) when (File.Exists(runtimePath))
                    {
                        TryDelete(stagingPath);
                    }
                }

                verifiedSource.Dispose();
                return Verify(runtimePath, runtimeBytes.LongLength, runtimeSha256);
            }
            catch
            {
                TryDelete(stagingPath);
                throw;
            }
        }
        catch
        {
            verifiedSource.Dispose();
            throw;
        }
    }

    private static string GetNamedElementIdentity(XElement element)
        => $"{element.Name.LocalName}:{(string?)element.Attribute("name")}";

    private static void TryDelete(string path)
    {
        try
        {
            File.Delete(path);
        }
        catch (IOException)
        {
        }
        catch (UnauthorizedAccessException)
        {
        }
    }
}

internal sealed class BinanceFixVerifiedDataDictionary : IDisposable
{
    private readonly FileStream verificationLease;

    internal BinanceFixVerifiedDataDictionary(string path, FileStream verificationLease)
    {
        Path = path;
        this.verificationLease = verificationLease;
    }

    internal string Path { get; }

    internal Stream Content
    {
        get
        {
            verificationLease.Position = 0;
            return verificationLease;
        }
    }

    public void Dispose() => verificationLease.Dispose();
}
