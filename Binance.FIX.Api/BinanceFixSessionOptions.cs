using System;
using System.IO;

namespace Binance.FIX.Api;

/// <summary>
/// Immutable, validated connection identity and logon-mode options for one Binance Spot FIX session.
/// </summary>
public sealed class BinanceFixSessionOptions
{
    /// <summary>
    /// The required FIX target component identifier.
    /// </summary>
    public const string TargetCompId = "SPOT";

    /// <summary>
    /// The default negotiated heartbeat interval in seconds.
    /// </summary>
    public const int DefaultHeartbeatIntervalSeconds = 30;

    /// <summary>
    /// Creates validated options for one explicitly selected environment and role.
    /// </summary>
    /// <param name="environment">The explicit production or Spot Testnet environment.</param>
    /// <param name="role">The isolated session role.</param>
    /// <param name="senderCompId">A 1-8 character ASCII session identifier. Binance requires it to be unique across active account sessions.</param>
    /// <param name="heartbeatIntervalSeconds">The negotiated heartbeat interval, from 5 through 60 seconds.</param>
    /// <param name="messageHandling">The message processing-order mode.</param>
    /// <param name="responseMode">The Order Entry/Drop Copy execution-report response mode. Market Data does not support this field.</param>
    /// <param name="dataDictionaryPath">The caller-owned official Binance QuickFIX dictionary path. Order Entry and Drop Copy use spot-fix-oe.xml; Market Data uses spot-fix-md.xml. The file is verified before a session is constructed.</param>
    /// <exception cref="ArgumentException"><paramref name="senderCompId"/> does not match Binance's published domain.</exception>
    /// <exception cref="ArgumentOutOfRangeException">An enum or heartbeat value is outside the published domain.</exception>
    public BinanceFixSessionOptions(
        BinanceFixEnvironment environment,
        BinanceFixSessionRole role,
        string senderCompId,
        int heartbeatIntervalSeconds = DefaultHeartbeatIntervalSeconds,
        BinanceFixMessageHandling messageHandling = BinanceFixMessageHandling.Sequential,
        BinanceFixResponseMode? responseMode = null,
        string? dataDictionaryPath = null)
    {
        Endpoint = ResolveEndpoint(environment, role);

        if (!IsValidSenderCompId(senderCompId))
        {
            throw new ArgumentException(
                "SenderCompId must contain 1-8 ASCII letters, digits, hyphens, or underscores.",
                nameof(senderCompId));
        }

        if (heartbeatIntervalSeconds is < 5 or > 60)
        {
            throw new ArgumentOutOfRangeException(
                nameof(heartbeatIntervalSeconds),
                heartbeatIntervalSeconds,
                "Heartbeat interval must be from 5 through 60 seconds.");
        }

        if (messageHandling is not BinanceFixMessageHandling.Unordered and not BinanceFixMessageHandling.Sequential)
        {
            throw new ArgumentOutOfRangeException(nameof(messageHandling), messageHandling, "Unsupported message handling mode.");
        }

        if (responseMode is not null &&
            responseMode is not BinanceFixResponseMode.Everything and not BinanceFixResponseMode.OnlyAcknowledgements)
        {
            throw new ArgumentOutOfRangeException(nameof(responseMode), responseMode, "Unsupported response mode.");
        }

        if (role is BinanceFixSessionRole.MarketData && responseMode is not null)
        {
            throw new ArgumentException("Market Data Logon does not support ResponseMode.", nameof(responseMode));
        }

        if (dataDictionaryPath is not null && string.IsNullOrWhiteSpace(dataDictionaryPath))
        {
            throw new ArgumentException("Data dictionary path must not be empty or whitespace.", nameof(dataDictionaryPath));
        }

        Environment = environment;
        Role = role;
        Limits = BinanceFixSessionLimits.ForRole(role);
        SenderCompId = senderCompId;
        HeartbeatIntervalSeconds = heartbeatIntervalSeconds;
        MessageHandling = messageHandling;
        ResponseMode = role is BinanceFixSessionRole.MarketData
            ? null
            : responseMode ?? BinanceFixResponseMode.Everything;
        DataDictionaryPath = dataDictionaryPath is null
            ? null
            : Path.GetFullPath(dataDictionaryPath);
    }

    /// <summary>
    /// Gets the explicitly selected environment.
    /// </summary>
    public BinanceFixEnvironment Environment { get; }

    /// <summary>
    /// Gets the isolated session role.
    /// </summary>
    public BinanceFixSessionRole Role { get; }

    /// <summary>
    /// Gets the current published limits for the isolated session role.
    /// </summary>
    public BinanceFixSessionLimits Limits { get; }

    /// <summary>
    /// Gets the immutable official TLS endpoint for the environment and role.
    /// </summary>
    public Uri Endpoint { get; }

    /// <summary>
    /// Gets the client session identifier used as FIX tag 49. Binance requires it to be unique across active account sessions.
    /// </summary>
    public string SenderCompId { get; }

    /// <summary>
    /// Gets the negotiated heartbeat interval in seconds.
    /// </summary>
    public int HeartbeatIntervalSeconds { get; }

    /// <summary>
    /// Gets the message processing-order mode used as FIX tag 25035.
    /// </summary>
    public BinanceFixMessageHandling MessageHandling { get; }

    /// <summary>
    /// Gets the execution-report response mode used as FIX tag 25036, or <see langword="null"/> for Market Data.
    /// </summary>
    public BinanceFixResponseMode? ResponseMode { get; }

    /// <summary>
    /// Gets the absolute path to the caller-owned official Binance QuickFIX dictionary.
    /// A production session cannot be constructed until this file passes the current size and SHA-256 lock.
    /// </summary>
    public string? DataDictionaryPath { get; }

    private static Uri ResolveEndpoint(BinanceFixEnvironment environment, BinanceFixSessionRole role)
    {
        var host = (environment, role) switch
        {
            (BinanceFixEnvironment.Production, BinanceFixSessionRole.OrderEntry) => "fix-oe.binance.com",
            (BinanceFixEnvironment.Production, BinanceFixSessionRole.DropCopy) => "fix-dc.binance.com",
            (BinanceFixEnvironment.Production, BinanceFixSessionRole.MarketData) => "fix-md.binance.com",
            (BinanceFixEnvironment.SpotTestnet, BinanceFixSessionRole.OrderEntry) => "fix-oe.testnet.binance.vision",
            (BinanceFixEnvironment.SpotTestnet, BinanceFixSessionRole.DropCopy) => "fix-dc.testnet.binance.vision",
            (BinanceFixEnvironment.SpotTestnet, BinanceFixSessionRole.MarketData) => "fix-md.testnet.binance.vision",
            _ when environment is not BinanceFixEnvironment.Production and not BinanceFixEnvironment.SpotTestnet =>
                throw new ArgumentOutOfRangeException(nameof(environment), environment, "Unsupported FIX environment."),
            _ => throw new ArgumentOutOfRangeException(nameof(role), role, "Unsupported FIX session role.")
        };

        return new Uri($"tcp+tls://{host}:9000", UriKind.Absolute);
    }

    private static bool IsValidSenderCompId(string? value)
    {
        if (value is not { Length: >= 1 and <= 8 })
        {
            return false;
        }

        foreach (var character in value)
        {
            if ((character is >= 'a' and <= 'z') ||
                (character is >= 'A' and <= 'Z') ||
                (character is >= '0' and <= '9') ||
                character is '-' or '_')
            {
                continue;
            }

            return false;
        }

        return true;
    }
}
