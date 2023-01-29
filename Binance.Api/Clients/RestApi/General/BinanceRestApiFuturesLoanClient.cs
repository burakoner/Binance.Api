using Binance.Api.Models.RestApi.Futures;

namespace Binance.Api.Clients.RestApi.General;

public class BinanceRestApiFuturesLoanClient
{
    // Api
    private const string sapi = "sapi";

    // Futures
    private const string futuresBorrowHistoryEndpoint = "futures/loan/borrow/history";
    private const string futuresRepayHistoryEndpoint = "futures/loan/repay/history";
    private const string futuresWalletEndpoint = "futures/loan/wallet";
    private const string futuresAdjustCrossCollateralHistoryEndpoint = "futures/loan/adjustCollateral/history";
    private const string futuresCrossCollateralLiquidationHistoryEndpoint = "futures/loan/liquidationHistory";
    // TODO: Cross-Collateral Interest History

    // Internal References
    internal BinanceRestApiGeneralClient MainClient { get; }
    internal BinanceRestApiClientOptions Options { get => MainClient.RootClient.Options; }
    internal Uri GetUrl(string endpoint, string api, string version = null) => MainClient.GetUrl(endpoint, api, version);
    internal async Task<RestCallResult<T>> SendRequestInternal<T>(
    Uri uri, HttpMethod method, CancellationToken cancellationToken, Dictionary<string, object> parameters = null, bool signed = false,
    HttpMethodParameterPosition? postPosition = null, ArraySerialization? arraySerialization = null, int weight = 1, bool ignoreRateLimit = false) where T : class
        => await MainClient.SendRequestInternal<T>(uri, method, cancellationToken, parameters, signed, postPosition, arraySerialization, weight, ignoreRateLimit);

    internal BinanceRestApiFuturesLoanClient(BinanceRestApiGeneralClient main)
    {
        MainClient = main;
    }

    #region Cross-Collateral Borrow History
    public async Task<RestCallResult<BinanceQueryRecords<BinanceCrossCollateralBorrowHistory>>> GetCrossCollateralBorrowHistoryAsync(string asset = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("coin", asset);
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("size", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceQueryRecords<BinanceCrossCollateralBorrowHistory>>(GetUrl(futuresBorrowHistoryEndpoint, sapi, "1"), HttpMethod.Get, ct, parameters, true, weight: 10).ConfigureAwait(false);
    }
    #endregion

    #region Cross-Collateral Repayment History
    public async Task<RestCallResult<BinanceQueryRecords<BinanceCrossCollateralRepayHistory>>> GetCrossCollateralRepayHistoryAsync(string asset = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("coin", asset);
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("size", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceQueryRecords<BinanceCrossCollateralRepayHistory>>(GetUrl(futuresRepayHistoryEndpoint, sapi, "1"), HttpMethod.Get, ct, parameters, true, weight: 10).ConfigureAwait(false);
    }
    #endregion

    #region Cross-Collateral Wallet V2
    public async Task<RestCallResult<BinanceCrossCollateralWallet>> GetCrossCollateralWalletAsync(long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceCrossCollateralWallet>(GetUrl(futuresWalletEndpoint, sapi, "2"), HttpMethod.Get, ct, parameters, true, weight: 10).ConfigureAwait(false);
    }
    #endregion

    #region Adjust Cross-Collateral LTV History
    public async Task<RestCallResult<BinanceQueryRecords<BinanceCrossCollateralAdjustLtvHistory>>> GetAdjustCrossCollateralLoanToValueHistoryAsync(string collateralAsset = null, string loanAsset = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("loanCoin", loanAsset);
        parameters.AddOptionalParameter("collateralCoin", collateralAsset);
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("size", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceQueryRecords<BinanceCrossCollateralAdjustLtvHistory>>(GetUrl(futuresAdjustCrossCollateralHistoryEndpoint, sapi, "1"), HttpMethod.Get, ct, parameters, true, weight: 10).ConfigureAwait(false);
    }
    #endregion

    #region Cross-Collateral Liquidation History
    public async Task<RestCallResult<BinanceQueryRecords<BinanceCrossCollateralLiquidationHistory>>> GetCrossCollateralLiquidationHistoryAsync(string collateralAsset = null, string loanAsset = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("loanCoin", loanAsset);
        parameters.AddOptionalParameter("collateralCoin", collateralAsset);
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("size", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceQueryRecords<BinanceCrossCollateralLiquidationHistory>>(GetUrl(futuresCrossCollateralLiquidationHistoryEndpoint, sapi, "1"), HttpMethod.Get, ct, parameters, true, weight: 10).ConfigureAwait(false);
    }
    #endregion
}