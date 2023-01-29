﻿using Binance.ApiClient.Models.RestApi;
using Binance.ApiClient.Models.RestApi.Fiat;

namespace Binance.ApiClient.Clients.RestApi.General;

public class BinanceRestApiFiatClient
{
    // Fiat
    private const string fiatDepositWithdrawHistoryEndpoint = "fiat/orders";
    private const string fiatPaymentHistoryEndpoint = "fiat/payments";

    // Internal References
    internal BinanceRestApiClient RootClient { get; }
    internal BinanceRestApiGeneralClient GeneralClient { get; }
    internal BinanceRestApiClientOptions Options { get => RootClient.Options; }
    internal Uri GetUrl(string endpoint, string api, string version = null) => GeneralClient.GetUrl(endpoint, api, version);
    internal async Task<RestCallResult<T>> SendRequestInternal<T>(
    Uri uri, HttpMethod method, CancellationToken cancellationToken, Dictionary<string, object> parameters = null, bool signed = false,
    HttpMethodParameterPosition? postPosition = null, ArraySerialization? arraySerialization = null, int weight = 1, bool ignoreRateLimit = false) where T : class
        => await GeneralClient.SendRequestInternal<T>(uri, method, cancellationToken, parameters, signed, postPosition, arraySerialization, weight, ignoreRateLimit);

    internal BinanceRestApiFiatClient(BinanceRestApiClient root, BinanceRestApiGeneralClient general)
    {
        RootClient = root;
        GeneralClient = general;
    }

    #region Get Fiat Deposit/Withdraw History
    public async Task<RestCallResult<IEnumerable<BinanceFiatWithdrawDeposit>>> GetFiatDepositWithdrawHistoryAsync(TransactionType side, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
            {
                { "transactionType", side == TransactionType.Deposit ? "0": "1" }
            };
        parameters.AddOptionalParameter("beginTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("page", page?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var result = await SendRequestInternal<BinanceResult<IEnumerable<BinanceFiatWithdrawDeposit>>>(GetUrl(fiatDepositWithdrawHistoryEndpoint, "sapi", "1"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);

        return result.As(result.Data?.Data!);
    }
    #endregion

    #region Get Fiat Payments History 
    public async Task<RestCallResult<IEnumerable<BinanceFiatPayment>>> GetFiatPaymentHistoryAsync(OrderSide side, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
            {
                { "transactionType", side == OrderSide.Buy ? "0": "1" }
            };
        parameters.AddOptionalParameter("beginTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("page", page?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        var result = await SendRequestInternal<BinanceResult<IEnumerable<BinanceFiatPayment>>>(GetUrl(fiatPaymentHistoryEndpoint, "sapi", "1"), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);

        return result.As(result.Data?.Data!);
    }
    #endregion
}