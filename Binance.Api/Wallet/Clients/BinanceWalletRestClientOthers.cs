namespace Binance.Api.Wallet;

internal partial class BinanceWalletRestClient
{
    public Task<RestCallResult<BinanceWalletSystemStatus>> GetSystemStatusAsync(CancellationToken ct = default)
    {
        return RequestAsync<BinanceWalletSystemStatus>(GetUrl(sapi, v1, "system/status"), HttpMethod.Get, ct, false, requestWeight: 1);
    }

}