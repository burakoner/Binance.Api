using System;

namespace Binance.FIX.Api;

/// <summary>
/// The current published Binance Spot FIX limits for one isolated session role.
/// </summary>
/// <remarks>
/// Binance enforces these limits authoritatively at the exchange. Concurrent connection limits
/// apply across the account, including connections created by other processes or clients.
/// </remarks>
public sealed class BinanceFixSessionLimits
{
    private static readonly BinanceFixSessionLimits OrderEntry = new(
        new BinanceFixWindowLimit(10_000, TimeSpan.FromSeconds(10)),
        new BinanceFixWindowLimit(15, TimeSpan.FromSeconds(30)),
        maximumConcurrentConnectionsPerAccount: 10,
        maximumMarketDataStreamsPerConnection: null);

    private static readonly BinanceFixSessionLimits DropCopy = new(
        new BinanceFixWindowLimit(60, TimeSpan.FromSeconds(60)),
        new BinanceFixWindowLimit(15, TimeSpan.FromSeconds(30)),
        maximumConcurrentConnectionsPerAccount: 10,
        maximumMarketDataStreamsPerConnection: null);

    private static readonly BinanceFixSessionLimits MarketData = new(
        new BinanceFixWindowLimit(2_000, TimeSpan.FromSeconds(60)),
        new BinanceFixWindowLimit(300, TimeSpan.FromSeconds(300)),
        maximumConcurrentConnectionsPerAccount: 100,
        maximumMarketDataStreamsPerConnection: 1_000);

    private BinanceFixSessionLimits(
        BinanceFixWindowLimit outboundMessages,
        BinanceFixWindowLimit connectionAttempts,
        int maximumConcurrentConnectionsPerAccount,
        int? maximumMarketDataStreamsPerConnection)
    {
        OutboundMessages = outboundMessages;
        ConnectionAttempts = connectionAttempts;
        MaximumConcurrentConnectionsPerAccount = maximumConcurrentConnectionsPerAccount;
        MaximumMarketDataStreamsPerConnection = maximumMarketDataStreamsPerConnection;
    }

    /// <summary>
    /// Gets the client-to-exchange message limit for one connection. Exchange responses do not consume it.
    /// </summary>
    public BinanceFixWindowLimit OutboundMessages { get; }

    /// <summary>
    /// Gets the connection-attempt limit for the account and role.
    /// </summary>
    public BinanceFixWindowLimit ConnectionAttempts { get; }

    /// <summary>
    /// Gets the maximum concurrent TCP connections for the account and role.
    /// </summary>
    public int MaximumConcurrentConnectionsPerAccount { get; }

    /// <summary>
    /// Gets the maximum market-data streams on one connection, or <see langword="null"/> for roles without streams.
    /// </summary>
    public int? MaximumMarketDataStreamsPerConnection { get; }

    internal static BinanceFixSessionLimits ForRole(BinanceFixSessionRole role)
        => role switch
        {
            BinanceFixSessionRole.OrderEntry => OrderEntry,
            BinanceFixSessionRole.DropCopy => DropCopy,
            BinanceFixSessionRole.MarketData => MarketData,
            _ => throw new ArgumentOutOfRangeException(nameof(role), role, "Unsupported FIX session role.")
        };
}
