using Binance.Api.Models.RestApi.Pay;

namespace Binance.Api.Clients.RestApi.General;

public class BinanceRestApiPayClient
{
    // Api
    private const string marginApi = "sapi";
    private const string marginVersion = "1";

    // Pay
    private const string payTradeHistoryEndpoint = "pay/transactions";

    // Internal References
    internal BinanceRestApiGeneralClient MainClient { get; }
    internal BinanceRestApiClientOptions ClientOptions { get => MainClient.RootClient.Options; }
    internal Uri GetUrl(string endpoint, string api, string version = null) => MainClient.GetUrl(endpoint, api, version);
    internal async Task<RestCallResult<T>> SendRequestInternal<T>(
        Uri uri, HttpMethod method, CancellationToken cancellationToken, bool signed = false,
        Dictionary<string, object> queryParameters = null, Dictionary<string, object> bodyParameters = null, Dictionary<string, string> headerParameters = null,
        ArraySerialization? serialization = null, JsonSerializer deserializer = null, bool ignoreRatelimit = false, int requestWeight = 1) where T : class
        => await MainClient.SendRequestInternal<T>(uri, method, cancellationToken, signed, queryParameters, bodyParameters, headerParameters, serialization, deserializer, ignoreRatelimit, requestWeight);

    internal BinanceRestApiPayClient(BinanceRestApiGeneralClient main)
    {
        MainClient = main;
    }

    #region Get Pay Trade History
    public async Task<RestCallResult<IEnumerable<BinancePayTrade>>> GetPayTradeHistoryAsync(DateTime? startTime = null, DateTime? endTime = null, int? limit = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("limit", limit);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var result = await SendRequestInternal<BinanceResult<IEnumerable<BinancePayTrade>>>(GetUrl(payTradeHistoryEndpoint, marginApi, marginVersion), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 3000).ConfigureAwait(false);
        if (!result.Success)
            return result.As<IEnumerable<BinancePayTrade>>(default);

        if (result.Data?.Code != 0)
            return result.AsError<IEnumerable<BinancePayTrade>>(new ServerError(result.Data!.Code, result.Data!.Message));

        return result.As(result.Data.Data);
    }
    #endregion
}