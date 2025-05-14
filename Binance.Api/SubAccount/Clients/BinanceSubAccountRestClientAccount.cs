namespace Binance.Api.SubAccount;

internal partial class BinanceSubAccountRestClient
{
    public Task<RestCallResult<BinanceSubAccountEmail>> CreateVirtualSubAccountAsync(string subAccountString, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
            {
                { "subAccountString", subAccountString }
            };
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceSubAccountEmail>(GetUrl(sapi, v1, "sub-account/virtualSubAccount"), HttpMethod.Post, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public async Task<RestCallResult<IEnumerable<BinanceSubAccount>>> GetSubAccountsAsync(string? email = null, int? page = null, int? limit = null, int? receiveWindow = null, bool? isFreeze = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("email", email);
        parameters.AddOptional("page", page?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptional("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));
        parameters.AddOptional("isFreeze", isFreeze);

        var result = await RequestAsync<BinanceSubAccountContainer>(GetUrl(sapi, v1, "sub-account/list"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
        return result ? result.As(result.Data.Payload) : result.As<IEnumerable<BinanceSubAccount>>([]);
    }

    public Task<RestCallResult<BinanceSubAccountFuturesEnabled>> EnableFuturesAsync(string email, int? receiveWindow = null, CancellationToken ct = default)
    {
        email.ValidateNotNull(nameof(email));
        var parameters = new ParameterCollection
        {
            { "email", email }
        };
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceSubAccountFuturesEnabled>(GetUrl(sapi, v1, "sub-account/futures/enable"), HttpMethod.Post, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceSubAccountMarginEnabled>> EnableMarginAsync(string email, int? receiveWindow = null, CancellationToken ct = default)
    {
        email.ValidateNotNull(nameof(email));

        var parameters = new ParameterCollection
        {
            { "email", email }
        };
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceSubAccountMarginEnabled>(GetUrl(sapi, v1, "sub-account/margin/enable"), HttpMethod.Post, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceSubAccountOptionsEnabled>> EnableOptionsAsync(string email, int? receiveWindow = null, CancellationToken ct = default)
    {
        email.ValidateNotNull(nameof(email));

        var parameters = new ParameterCollection
        {
            { "email", email }
        };
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceSubAccountOptionsEnabled>(GetUrl(sapi, v1, "sub-account/margin/enable"), HttpMethod.Post, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceSubAccountBlvt>> EnableBlvtAsync(string email, bool enable, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "email", email },
            { "enableBlvt", enable }
        };
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceSubAccountBlvt>(GetUrl(sapi, v1, "sub-account/blvt/enable"), HttpMethod.Post, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<IEnumerable<BinanceSubAccountStatus>>> GetSubAccountStatusAsync(string? email = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("email", email);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<IEnumerable<BinanceSubAccountStatus>>(GetUrl(sapi, v1, "sub-account/status"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10);
    }

    public Task<RestCallResult<IEnumerable<BinanceSubAccountFuturesPositionRisk>>> GetFuturesPositionRiskAsync(string email, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "email", email }
        };
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<IEnumerable<BinanceSubAccountFuturesPositionRisk>>(GetUrl(sapi, v1, "sub-account/futures/positionRisk"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10);
    }

    public Task<RestCallResult<BinanceSubAccountFuturesPositionRiskV2>> GetFuturesPositionRiskAsync(BinanceSubAccountFuturesType futuresType, string email, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "email", email },
        };
        parameters.AddEnum("futuresType", futuresType);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceSubAccountFuturesPositionRiskV2>(GetUrl(sapi, v2, "sub-account/futures/positionRisk"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceSubAccountTransactionStatistics>> GetTransactionStatisticsAsync(string email, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "email", email },
        };
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceSubAccountTransactionStatistics>(GetUrl(sapi, v2, "sub-account/transaction-statistics"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 60);
    }
}
