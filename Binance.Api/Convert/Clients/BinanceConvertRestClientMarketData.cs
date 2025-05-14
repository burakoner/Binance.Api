namespace Binance.Api.Convert;

internal partial class BinanceConvertRestClient
{
    public Task<RestCallResult<IEnumerable<BinanceConvertPair>>> GetPairsAsync(string? fromAsset = null, string? toAsset = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("fromAsset", fromAsset);
        parameters.AddOptional("toAsset", toAsset);

        return RequestAsync<IEnumerable<BinanceConvertPair>>(GetUrl(sapi, v1, "convert/exchangeInfo"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 3000);
    }

    public Task<RestCallResult<IEnumerable<BinanceConvertAsset>>> GetAssetsAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<IEnumerable<BinanceConvertAsset>>(GetUrl(sapi, v1, "convert/assetInfo"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 100);
    }
}