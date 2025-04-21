using Binance.Api.Models.RestApi.Blvt;

namespace Binance.Api.Clients.RestApi.General;

public class BinanceRestApiBlvtClient
{
    // Api
    private const string blvtApi = "sapi";
    private const string blvtVersion = "1";
    private const string marginApi = "sapi";
    private const string marginVersion = "1";

    // BLVT
    private const string blvtInfoEndpoint = "blvt/tokenInfo";
    private const string blvtHistoricalKlinesEndpoint = "lvtKlines";
    private const string blvtSubscribeEndpoint = "blvt/subscribe";
    private const string blvtSubscriptionRecordsEndpoint = "blvt/subscribe/record";
    private const string blvtRedeemEndpoint = "blvt/redeem";
    private const string blvtRedeemRecordsEndpoint = "blvt/redeem/record";
    private const string blvtUserLimitEndpoint = "blvt/userLimit";

    // Internal References
    internal BinanceRestApiGeneralClient MainClient { get; }
    internal BinanceRestApiClientOptions ClientOptions { get => MainClient.RootClient.RestOptions; }
    internal Uri GetUrl(string endpoint, string api, string version = null) => MainClient.GetUrl(endpoint, api, version);
    internal async Task<RestCallResult<T>> SendRequestInternal<T>(
        Uri uri, HttpMethod method, CancellationToken cancellationToken, bool signed = false,
        Dictionary<string, object> queryParameters = null, Dictionary<string, object> bodyParameters = null, Dictionary<string, string> headerParameters = null,
        ArraySerialization? serialization = null, JsonSerializer deserializer = null, bool ignoreRatelimit = false, int requestWeight = 1) where T : class
        => await MainClient.SendRequestInternal<T>(uri, method, cancellationToken, signed, queryParameters, bodyParameters, headerParameters, serialization, deserializer, ignoreRatelimit, requestWeight);

    internal BinanceRestApiBlvtClient(BinanceRestApiGeneralClient main)
    {
        MainClient = main;
    }

    #region Get BLVT Info
    public async Task<RestCallResult<IEnumerable<BinanceBlvtInfo>>> GetLeveragedTokenInfoAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceBlvtInfo>>(GetUrl(blvtInfoEndpoint, blvtApi, blvtVersion), HttpMethod.Get, ct, false, queryParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Historical BLVT NAV Kline/Candlestick
    public async Task<RestCallResult<IEnumerable<BinanceBlvtKline>>> GetLeveragedTokensHistoricalKlinesAsync(string symbol, BinanceKlineInterval interval, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        // TODO check if URL works
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);

        var parameters = new ParameterCollection
            {
                { "symbol", symbol },
            };
        parameters.AddEnum("interval", interval);
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceBlvtKline>>(GetUrl(blvtHistoricalKlinesEndpoint, "fapi", blvtVersion), HttpMethod.Get, ct, false, queryParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Subscribe BLVT
    public async Task<RestCallResult<BinanceBlvtSubscribeResult>> SubscribeLeveragedTokenAsync(string tokenName, decimal cost, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
            {
                { "tokenName", tokenName },
                { "cost", cost.ToString(CultureInfo.InvariantCulture) }
            };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceBlvtSubscribeResult>(GetUrl(blvtSubscribeEndpoint, blvtApi, blvtVersion), HttpMethod.Post, ct, true, bodyParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Query Subscription Record
    public async Task<RestCallResult<IEnumerable<BinanceBlvtSubscription>>> GetLeveragedTokensSubscriptionRecordsAsync(string tokenName = null, long? id = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);

        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("tokenName", tokenName);
        parameters.AddOptionalParameter("id", id?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceBlvtSubscription>>(GetUrl(blvtSubscriptionRecordsEndpoint, blvtApi, blvtVersion), HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Redeem BLVT
    public async Task<RestCallResult<BinanceBlvtRedeemResult>> RedeemLeveragedTokenAsync(string tokenName, decimal quantity, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
            {
                { "tokenName", tokenName },
                { "amount", quantity.ToString(CultureInfo.InvariantCulture) }
            };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceBlvtRedeemResult>(GetUrl(blvtRedeemEndpoint, blvtApi, blvtVersion), HttpMethod.Post, ct, true, bodyParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Query Redemption Record
    public async Task<RestCallResult<IEnumerable<BinanceBlvtRedemption>>> GetLeveragedTokensRedemptionRecordsAsync(string tokenName = null, long? id = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 1000);

        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("tokenName", tokenName);
        parameters.AddOptionalParameter("id", id?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceBlvtRedemption>>(GetUrl(blvtRedeemRecordsEndpoint, blvtApi, blvtVersion), HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Get BLVT User Limit Info
    public async Task<RestCallResult<IEnumerable<BinanceBlvtUserLimit>>> GetLeveragedTokensUserLimitAsync(string tokenName = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("tokenName", tokenName);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceBlvtUserLimit>>(GetUrl(blvtUserLimitEndpoint, marginApi, marginVersion), HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
    }
    #endregion
}