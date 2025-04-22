namespace Binance.Api.Wallet;

internal partial class BinanceWalletRestApiClient
{
    public Task<RestCallResult<BinanceSystemStatus>> GetSystemStatusAsync(CancellationToken ct = default)
    {
        return RequestAsync<BinanceSystemStatus>(GetUrl(sapi, v1, "system/status"), HttpMethod.Get, ct, false, requestWeight: 1);
    }

}