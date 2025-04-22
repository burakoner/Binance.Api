using Binance.Api.Margin.Responses;
using Binance.Api.Wallet;
using Binance.Net.Objects.Models.Spot.IsolatedMargin;
using Binance.Net.Objects.Models.Spot.Margin;

namespace Binance.Api.Margin;

internal partial class BinanceMarginRestApiClient
{
    public Task<RestCallResult<BinanceCrossMarginLeverageResult>> CrossMarginAdjustMaxLeverageAsync(int maxLeverage, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "maxLeverage", maxLeverage },
        };
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceCrossMarginLeverageResult>(GetUrl(sapi, v1, "margin/max-leverage"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 3000);
    }

    public Task<RestCallResult<CreateIsolatedMarginAccountResult>> DisableIsolatedMarginAccountAsync(string symbol,
        int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            {"symbol", symbol}
        };
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<CreateIsolatedMarginAccountResult>(GetUrl(sapi, v1, "margin/isolated/account"), HttpMethod.Delete, ct, true, bodyParameters: parameters, requestWeight: 300);
    }

    public Task<RestCallResult<CreateIsolatedMarginAccountResult>> EnableIsolatedMarginAccountAsync(string symbol,
        int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            {"symbol", symbol}
        };
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<CreateIsolatedMarginAccountResult>(GetUrl(sapi, v1, "margin/isolated/account"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 300);
    }

    public Task<RestCallResult<BinanceBnbBurnStatus>> GetBnbBurnStatusAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceBnbBurnStatus>(GetUrl(sapi, v1, "bnbBurn"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceMarginLevel>> GetMarginLevelInformationAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceMarginLevel>(GetUrl(sapi, v1, "margin/tradeCoeff"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10);
    }

    public Task<RestCallResult<BinanceMarginAccount>> GetMarginAccountInfoAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceMarginAccount>(GetUrl(sapi, v1, "margin/account"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10);
    }

    public Task<RestCallResult<IEnumerable<BinanceInterestMarginData>>> GetInterestMarginDataAsync(string? asset = null, string? vipLevel = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        asset?.ValidateNotNull(nameof(asset));

        var parameters = new ParameterCollection();

        parameters.AddOptional("coin", asset);
        parameters.AddOptional("vipLevel", vipLevel);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        var weight = asset == null ? 5 : 1;
        return RequestAsync<IEnumerable<BinanceInterestMarginData>>(GetUrl(sapi, v1, "margin/crossMarginData"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: weight);
    }

    public Task<RestCallResult<IsolatedMarginAccountLimit>> GetEnabledIsolatedMarginAccountLimitAsync(
        int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<IsolatedMarginAccountLimit>(GetUrl(sapi, v1, "margin/isolated/accountLimit"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceIsolatedMarginAccount>> GetIsolatedMarginAccountAsync(
        int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceIsolatedMarginAccount>(GetUrl(sapi, v1, "margin/isolated/account"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10);
    }

    public Task<RestCallResult<IEnumerable<BinanceIsolatedMarginFeeData>>> GetIsolatedMarginFeeDataAsync(string? symbol = null, int? vipLevel = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("symbol", symbol);
        parameters.AddOptional("vipLevel", vipLevel);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        var weight = symbol == null ? 10 : 1;
        return RequestAsync<IEnumerable<BinanceIsolatedMarginFeeData>>(GetUrl(sapi, v1, "margin/isolatedMarginData"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: weight);
    }


}