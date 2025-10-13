namespace Binance.Api.Options;

internal partial class BinanceOptionsRestClient
{
    public Task<RestCallResult<BinanceOptionsAccount>> GetAccountAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceOptionsAccount>(GetUrl(eapi, v1, "account"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 20);
    }

    // TODO: Option Margin Account Information (USER_DATA)

    public Task<RestCallResult<List<BinanceOptionsAccountFundingFlow>>> GetAccountFundingFlowAsync(string currency, long? recordId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);

        var parameters = new ParameterCollection
        {
            { "symbol", currency }
        };
        parameters.AddOptional("recordId", recordId?.ToString(BinanceConstants.CI));
        parameters.AddOptional("limit", limit?.ToString(BinanceConstants.CI));
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

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