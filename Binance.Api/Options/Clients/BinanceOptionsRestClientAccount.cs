namespace Binance.Api.Options;

internal partial class BinanceOptionsRestClient
{
    public Task<RestCallResult<BinanceOptionsMarginAccount>> GetMarginAccountAsync(long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", NormalizeReceiveWindow(receiveWindow));

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
        parameters.AddOptional("recvWindow", NormalizeReceiveWindow(receiveWindow));

        return RequestAsync<List<BinanceOptionsAccountFundingFlow>>(GetUrl(eapi, v1, "bill"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }
}
