namespace Binance.Api.CryptoLoan;

internal partial class BinanceCryptoLoanRestClientStable(BinanceCryptoLoanRestClient parent) : IBinanceCryptoLoanRestClientStable
{
    // Api
    private const string v1 = "1";
    private const string v3 = "3";
    private const string api = "api";
    private const string sapi = "sapi";

    // Parent
    internal BinanceCryptoLoanRestClient _ { get; } = parent;
}