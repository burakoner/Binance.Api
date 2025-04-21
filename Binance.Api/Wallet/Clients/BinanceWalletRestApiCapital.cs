namespace Binance.Api.Wallet;

public class BinanceWalletRestApiCapital(BinanceWalletRestApi parent)
{
    // Api
    private const string api = "api";
    private const string v1 = "1";
    private const string v3 = "3";

    // Parent Objects
    private BinanceRestApiClient _ => __._;
    private BinanceWalletRestApi __ { get; } = parent;
    private BinanceRestApiClientOptions _options => _.ClientOptions;
    private ILogger _logger => _.Logger;
}