﻿using Binance.ApiClient.Models.RestApi;
using Binance.ApiClient.Models.RestApi.Pay;

namespace Binance.ApiClient.Clients.RestApi.General;

public class BinanceRestApiPayClient
{
    // Api
    private const string marginApi = "sapi";
    private const string marginVersion = "1";

    // Pay
    private const string payTradeHistoryEndpoint = "pay/transactions";

    // Internal References
    internal BinanceRestApiClient RootClient { get; }
    internal BinanceRestApiGeneralClient GeneralClient { get; }
    internal BinanceRestApiClientOptions Options { get => RootClient.Options; }
    internal Uri GetUrl(string endpoint, string api, string version = null) => GeneralClient.GetUrl(endpoint, api, version);
    internal async Task<RestCallResult<T>> SendRequestInternal<T>(
    Uri uri, HttpMethod method, CancellationToken cancellationToken, Dictionary<string, object> parameters = null, bool signed = false,
    HttpMethodParameterPosition? postPosition = null, ArraySerialization? arraySerialization = null, int weight = 1, bool ignoreRateLimit = false) where T : class
        => await GeneralClient.SendRequestInternal<T>(uri, method, cancellationToken, parameters, signed, postPosition, arraySerialization, weight, ignoreRateLimit);

    internal BinanceRestApiPayClient(BinanceRestApiClient root, BinanceRestApiGeneralClient general)
    {
        RootClient = root;
        GeneralClient = general;
    }

    #region Get Pay Trade History
    public async Task<RestCallResult<IEnumerable<BinancePayTrade>>> GetPayTradeHistoryAsync(DateTime? startTime = null, DateTime? endTime = null, int? limit = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("limit", limit);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var result = await SendRequestInternal<BinanceResult<IEnumerable<BinancePayTrade>>>(GetUrl(payTradeHistoryEndpoint, marginApi, marginVersion), HttpMethod.Get, ct, parameters, true, weight: 3000).ConfigureAwait(false);
        if (!result.Success)
            return result.As<IEnumerable<BinancePayTrade>>(default);

        if (result.Data?.Code != 0)
            return result.AsError<IEnumerable<BinancePayTrade>>(new ServerError(result.Data!.Code, result.Data!.Message));

        return result.As(result.Data.Data);
    }
    #endregion
}