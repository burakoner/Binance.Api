namespace Binance.Api.CryptoLoan;

internal class BinanceCryptoLoanRestClient : IBinanceCryptoLoanRestClient
{
    // Parent
    internal BinanceRestApiClient _ { get; }

    // Internal
    internal ILogger Logger => _.Logger;
    internal BinanceRestApiClientOptions RestOptions => _.RestOptions;

    // Interface Properties
    public IBinanceCryptoLoanRestClientFlexible Flexible { get; }
    public IBinanceCryptoLoanRestClientStable Stable { get; }

    // Constructor
    internal BinanceCryptoLoanRestClient(BinanceRestApiClient root)
    {
        _ = root;
        Flexible = new BinanceCryptoLoanRestClientFlexible(this);
        Stable = new BinanceCryptoLoanRestClientStable(this);
    }
}