namespace Binance.Api.Broker;

internal partial class BinanceBrokerRestClientLinkAndTrade
{
    /*
    /// <summary>
    /// Query Client If The New User.
    /// <para><a href="https://binance-docs.github.io/apiAgent-API-EN/api_rebate_endpoints_spot_EN/" /></para>
    /// <para><a href="https://binance-docs.github.io/apiAgent-API-CN/api_rebate_endpoints_spot_CN/" /></para>
    /// </summary>
    /// <param name="apiAgentCode">Api Agent Code</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>USER DATA</returns>
    public async Task<WebCallResult<BinanceIfNewUser>> GetIfNewUserAsync(string apiAgentCode, int? receiveWindow = null, CancellationToken ct = default)
    {
        apiAgentCode.ValidateNotNull(nameof(apiAgentCode));

        var parameters = new ParameterCollection
            {
                { "apiAgentCode", apiAgentCode }
            };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var request = _definitions.GetOrCreate(HttpMethod.Get, "sapi/v1/apiReferral/ifNewUser", BinanceExchange.RateLimiter.SpotRestIp, 100, true);
        return await _baseClient.SendAsync<BinanceIfNewUser>(request, parameters, ct).ConfigureAwait(false);
    }

    // TODO: Customize Id For Client (USER DATA) （For Partner）
    // TODO: Get Client Email Customized Id (USER DATA) （For Partner）
    // TODO: Customize Id For Client (USER DATA)(For client)
    // TODO: Get User’s Customize Id (USER DATA)
    // TODO: Query Rebate Recent Record （USER DATA）(For Partner)
    // TODO: Query Rebate Recent Record(For Client)
    */
}