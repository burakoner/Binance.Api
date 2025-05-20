namespace Binance.Api.Staking;

internal partial class BinanceStakingRestClientEth
{
    public Task<RestCallResult<BinanceEthStakingAccount>> GetAccountAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return __.RequestAsync<BinanceEthStakingAccount>(GetUrl(sapi, v2, "eth-staking/account"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 150);
    }

    public Task<RestCallResult<BinanceEthStakingQuota>> GetQuotaAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return __.RequestAsync<BinanceEthStakingQuota>(GetUrl(sapi, v1, "eth-staking/eth/quota"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 150);
    }
}