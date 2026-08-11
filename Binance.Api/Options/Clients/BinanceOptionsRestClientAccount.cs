namespace Binance.Api.Options;

internal partial class BinanceOptionsRestClient
{
    public Task<RestCallResult<BinanceOptionsAccount>> GetAccountAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceOptionsAccount>(GetUrl(eapi, v1, "account"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 20);
    }

    public Task<RestCallResult<BinanceOptionsMarginAccount>> GetMarginAccountAsync(long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        var normalizedReceiveWindow = receiveWindow ?? (RestOptions.ReceiveWindow == null
            ? null
            : System.Convert.ToInt64(RestOptions.ReceiveWindow.Value.TotalMilliseconds));
        parameters.AddOptional("recvWindow", normalizedReceiveWindow);

        return RequestAsync<BinanceOptionsMarginAccount>(GetUrl(eapi, v1, "marginAccount"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 3);
    }

    public Task<RestCallResult<List<BinanceOptionsAccountFundingFlow>>> GetAccountFundingFlowAsync(BinanceOptionsFundingFlowCurrency currency, long? recordId = null, DateTime? startTime = null, DateTime? endTime = null, long? limit = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        if (currency != BinanceOptionsFundingFlowCurrency.Usdt)
            throw new ArgumentOutOfRangeException(nameof(currency), currency, "currency must be USDT");
        if (limit > 1000)
            throw new ArgumentOutOfRangeException(nameof(limit), "limit cannot exceed 1000");

        var parameters = new ParameterCollection();
        parameters.AddEnum("currency", currency);
        parameters.AddOptional("recordId", recordId);
        parameters.AddOptional("limit", limit);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        var normalizedReceiveWindow = receiveWindow ?? (RestOptions.ReceiveWindow == null
            ? null
            : System.Convert.ToInt64(RestOptions.ReceiveWindow.Value.TotalMilliseconds));
        parameters.AddOptional("recvWindow", normalizedReceiveWindow);

        return RequestAsync<List<BinanceOptionsAccountFundingFlow>>(GetUrl(eapi, v1, "bill"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceOptionsDownloadId>> GetTransactionHistoryDownloadIdAsync(DateTime? startTime = null, DateTime? endTime = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceOptionsDownloadId>(GetUrl(eapi, v1, "income/asyn"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceOptionsDownloadLink>> GetTransactionHistoryDownloadLinkAsync(long downloadId, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddParameter("downloadId", downloadId);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceOptionsDownloadLink>(GetUrl(eapi, v1, "income/asyn/id"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }
}
