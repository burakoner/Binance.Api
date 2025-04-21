namespace Binance.Api.Wallet;

/// <summary>
/// Binance Wallet Rest API Client
/// </summary>
/// <param name="root">Parent</param>
public class BinanceWalletRestApiClient(BinanceRestApiClient root)
{
    // Api
    private const string v1 = "1";
    private const string v3 = "3";
    private const string api = "api";
    private const string sapi = "sapi";

    // Parent
    internal BinanceRestApiClient _ { get; } = root;

    // Internal
    internal ILogger Logger => Logger;
    internal BinanceRestApiClientOptions Options => Options;
    internal DateTime? LastExchangeInfoUpdate { get; private set; }
    internal BinanceExchangeInfo? ExchangeInfo { get; private set; }
}