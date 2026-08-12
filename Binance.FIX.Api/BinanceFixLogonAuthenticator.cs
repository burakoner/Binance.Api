using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using QuickFix;
using QuickFix.Fields;

namespace Binance.FIX.Api;

internal static class BinanceFixLogonAuthenticator
{
    internal const int MessageHandlingTag = 25035;
    internal const int ResponseModeTag = 25036;
    internal const int DropCopyFlagTag = 9406;

    private static readonly string[] SendingTimeFormats =
    [
        "yyyyMMdd-HH:mm:ss",
        "yyyyMMdd-HH:mm:ss.fff",
        "yyyyMMdd-HH:mm:ss.ffffff"
    ];

    internal static void Apply(
        Message logon,
        BinanceFixSessionOptions options,
        BinanceFixEd25519Credentials credentials)
    {
        ArgumentNullException.ThrowIfNull(logon);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(credentials);
        ClearOwnedFields(logon);

        var messageType = GetRequiredHeaderValue(logon, Tags.MsgType);
        if (!string.Equals(messageType, MsgType.LOGON, StringComparison.Ordinal))
        {
            throw new InvalidOperationException("Authentication fields can only be applied to a Logon (35=A) message.");
        }

        var senderCompId = GetRequiredHeaderValue(logon, Tags.SenderCompID);
        if (!string.Equals(senderCompId, options.SenderCompId, StringComparison.Ordinal))
        {
            throw new InvalidOperationException("Logon SenderCompID does not match the validated session options.");
        }

        var targetCompId = GetRequiredHeaderValue(logon, Tags.TargetCompID);
        if (!string.Equals(targetCompId, BinanceFixSessionOptions.TargetCompId, StringComparison.Ordinal))
        {
            throw new InvalidOperationException("Logon TargetCompID must be SPOT.");
        }

        var messageSequenceNumber = GetRequiredHeaderValue(logon, Tags.MsgSeqNum);
        if (!ulong.TryParse(
                messageSequenceNumber,
                NumberStyles.None,
                CultureInfo.InvariantCulture,
                out var sequenceNumber)
            || sequenceNumber == 0)
        {
            throw new InvalidOperationException("Logon MsgSeqNum must be a positive unsigned integer.");
        }

        var sendingTime = GetRequiredHeaderValue(logon, Tags.SendingTime);
        if (!DateTime.TryParseExact(
                sendingTime,
                SendingTimeFormats,
                CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal,
                out _))
        {
            throw new InvalidOperationException("Logon SendingTime must use a supported UTC FIX timestamp format.");
        }

        var payload = string.Join(
            '\u0001',
            messageType,
            senderCompId,
            targetCompId,
            messageSequenceNumber,
            sendingTime);
        var payloadBytes = Encoding.ASCII.GetBytes(payload);

        string signature;
        try
        {
            signature = credentials.SignBase64(payloadBytes);
        }
        finally
        {
            CryptographicOperations.ZeroMemory(payloadBytes);
        }

        var apiKey = credentials.GetApiKey();
        logon.SetField(new EncryptMethod(0));
        logon.SetField(new HeartBtInt(options.HeartbeatIntervalSeconds));
        logon.SetField(new RawDataLength(signature.Length));
        logon.SetField(new RawData(signature));
        logon.SetField(new ResetSeqNumFlag(true));
        logon.SetField(new Username(apiKey));
        logon.SetField(new IntField(MessageHandlingTag, (int)options.MessageHandling));

        if (options.ResponseMode is { } responseMode)
        {
            logon.SetField(new IntField(ResponseModeTag, (int)responseMode));
        }
        else
        {
            logon.RemoveField(ResponseModeTag);
        }

        if (options.Role is BinanceFixSessionRole.DropCopy)
        {
            logon.SetField(new BooleanField(DropCopyFlagTag, true));
        }
        else
        {
            logon.RemoveField(DropCopyFlagTag);
        }
    }

    private static string GetRequiredHeaderValue(Message message, int tag)
    {
        if (!message.Header.IsSetField(tag))
        {
            throw new InvalidOperationException($"QuickFIX must populate required Logon header tag {tag} before authentication.");
        }

        return message.Header.GetString(tag);
    }

    private static void ClearOwnedFields(Message logon)
    {
        logon.RemoveField(Tags.RawDataLength);
        logon.RemoveField(Tags.RawData);
        logon.RemoveField(Tags.EncryptMethod);
        logon.RemoveField(Tags.HeartBtInt);
        logon.RemoveField(Tags.ResetSeqNumFlag);
        logon.RemoveField(Tags.Username);
        logon.RemoveField(MessageHandlingTag);
        logon.RemoveField(ResponseModeTag);
        logon.RemoveField(DropCopyFlagTag);
    }
}
