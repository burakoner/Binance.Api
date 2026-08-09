namespace Binance.Api.Convert;

internal partial class BinanceConvertRestClient
{
    public Task<RestCallResult<List<BinanceConvertPair>>> GetPairsAsync(string? fromAsset = null, string? toAsset = null, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(fromAsset) && string.IsNullOrWhiteSpace(toAsset))
            throw new ArgumentException("Either fromAsset or toAsset must be provided.");
        if (fromAsset != null && string.IsNullOrWhiteSpace(fromAsset))
            throw new ArgumentException("fromAsset cannot be empty.", nameof(fromAsset));
        if (toAsset != null && string.IsNullOrWhiteSpace(toAsset))
            throw new ArgumentException("toAsset cannot be empty.", nameof(toAsset));

        var parameters = new ParameterCollection();
        parameters.AddOptional("fromAsset", fromAsset);
        parameters.AddOptional("toAsset", toAsset);

        return RequestAsync<List<BinanceConvertPair>>(GetUrl(sapi, v1, "convert/exchangeInfo"), HttpMethod.Get, ct, false, queryParameters: parameters, requestWeight: 3000);
    }

    public Task<RestCallResult<List<BinanceConvertAsset>>> GetAssetsAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", ValidateReceiveWindow(receiveWindow));

        return RequestAsync<List<BinanceConvertAsset>>(GetUrl(sapi, v1, "convert/assetInfo"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 100);
    }
}
