using ApiSharp;

namespace Binance.Api.Broker;

internal partial class BinanceBrokerRestClientLinkAndTrade
{
    /*
    /// <summary>
    /// Query Client If The New User.
    /// <para><a href="https://binance-docs.github.io/apiAgent-API-EN/api_rebate_endpoints_futures_EN/" /></para>
    /// <para><a href="https://binance-docs.github.io/apiAgent-API-CN/api_rebate_endpoints_futures_CN/" /></para>
    /// </summary>
    /// <param name="brokerId">Api Broker Id</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>USER DATA</returns>
    public async Task<WebCallResult<BinanceFuturesIfNewUser>> GetIfNewUserAsync(string brokerId, int? receiveWindow = null, CancellationToken ct = default)
    {
        brokerId.ValidateNotNull(nameof(brokerId));

        var parameters = new ParameterCollection
            {
                { "brokerId", brokerId }
            };
        parameters.AddEnum("type", IfNewUserMarginedFuturesType.UsdtMarginedFutures);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? _baseClient.ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var request = _definitions.GetOrCreate(HttpMethod.Get, "fapi/v1/apiReferral/ifNewUser", BinanceExchange.RateLimiter.FuturesRest, 100, true);
        return await _baseClient.SendAsync<BinanceFuturesIfNewUser>(request, parameters, ct).ConfigureAwait(false);
    }

    // TODO: Customize Id For Client (USER DATA)(For Partner)
    // TODO: Get Client Email Customized Id (USER DATA)
    // TODO: Customize Id For Client (USER DATA)(For client)
    // TODO: Get User’s Customize Id (USER DATA)
    // TODO: Get Income History(USER DATA)
    // TODO: Get Trader Number (USER DATA)
    // TODO: Get Rebate Data Overview (USER DATA)
    // TODO: Get User Trade Volume (USER DATA)
    // TODO: Get Rebate Volume (USER DATA)
    // TODO: Get Trader Detail (USER DATA)
    // TODO: Query Client If The New User (USER DATA)(PAPI)
    // TODO: Customize Id For Client (USER DATA)(For client)(PAPI)
    // TODO: Get User’s Customize Id (USER DATA)(PAPI)
    */
}