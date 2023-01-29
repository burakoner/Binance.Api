﻿using Binance.Api.Models.RestApi.Rebate;

namespace Binance.Api.Clients.RestApi.General;

public class BinanceRestApiRebateClient
{
    // Api
    private const string marginApi = "sapi";
    private const string marginVersion = "1";

    // Rebate
    private const string rebateHistoryEndpoint = "rebate/taxQuery";

    // Internal References
    internal BinanceRestApiGeneralClient MainClient { get; }
    internal BinanceRestApiClientOptions Options { get => MainClient.RootClient.Options; }
    internal Uri GetUrl(string endpoint, string api, string version = null) => MainClient.GetUrl(endpoint, api, version);
    internal async Task<RestCallResult<T>> SendRequestInternal<T>(
    Uri uri, HttpMethod method, CancellationToken cancellationToken, Dictionary<string, object> parameters = null, bool signed = false,
    HttpMethodParameterPosition? postPosition = null, ArraySerialization? arraySerialization = null, int weight = 1, bool ignoreRateLimit = false) where T : class
        => await MainClient.SendRequestInternal<T>(uri, method, cancellationToken, parameters, signed, postPosition, arraySerialization, weight, ignoreRateLimit);

    internal BinanceRestApiRebateClient(BinanceRestApiGeneralClient main)
    {
        MainClient = main;
    }

    #region Get Spot Rebate History Records
    public async Task<RestCallResult<BinanceRebateWrapper>> GetRebateHistoryAsync(DateTime? startTime = null, DateTime? endTime = null, int? page = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("page", page);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var result = await SendRequestInternal<BinanceResult<BinanceRebateWrapper>>(GetUrl(rebateHistoryEndpoint, marginApi, marginVersion), HttpMethod.Get, ct, parameters, true, weight: 3000).ConfigureAwait(false);
        if (!result.Success)
            return result.As<BinanceRebateWrapper>(default);

        if (result.Data?.Code != 0)
            return result.AsError<BinanceRebateWrapper>(new ServerError(result.Data!.Code, result.Data!.Message));

        return result.As(result.Data.Data);
    }
    #endregion
}