namespace Binance.Api.Spot;

public class BinanceSpotRestApiAccountClient(BinanceSpotRestApiClient parent)
{
    // Api
    private const string api = "api";
    private const string v1 = "1";
    private const string v3 = "3";

    // Parent Clients
    internal BinanceRestApiClient _ { get; } = parent._;
    internal BinanceSpotRestApiClient __ { get; } = parent;
    internal ILogger? Logger { get; } = parent.Logger;
    internal BinanceRestApiClientOptions ClientOptions { get; } = parent.ClientOptions;

    #region Account Information
    public async Task<RestCallResult<BinanceAccountInfo>> GetAccountInfoAsync(long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await __.SendRequestInternal<BinanceAccountInfo>(__.GetUrl(api, v3, "account"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10).ConfigureAwait(false);
    }
    #endregion

    #region Account Trade List
    public async Task<RestCallResult<IEnumerable<BinanceTrade>>> GetUserTradesAsync(string symbol, long? orderId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, long? fromId = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        symbol.ValidateBinanceSymbol();
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);

        var parameters = new ParameterCollection
            {
                { "symbol", symbol }
            };
        parameters.AddOptionalParameter("orderId", orderId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("fromId", fromId?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await __.SendRequestInternal<IEnumerable<BinanceTrade>>(__.GetUrl(api, v3, "myTrades"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10).ConfigureAwait(false);
    }
    #endregion

    #region Query Current Order Count Usage
    public async Task<RestCallResult<IEnumerable<BinanceOrderRateLimit>>> GetOrderRateLimitStatusAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await __.SendRequestInternal<IEnumerable<BinanceOrderRateLimit>>(__.GetUrl(api, v3, "rateLimit/order"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 20).ConfigureAwait(false);
    }
    #endregion

    // TODO: Query Prevented Matches (USER_DATA)

}