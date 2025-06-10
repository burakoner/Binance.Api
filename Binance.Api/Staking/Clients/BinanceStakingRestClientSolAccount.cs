namespace Binance.Api.Staking;

internal partial class BinanceStakingRestClientSol
{
    public  Task<RestCallResult<BinanceSolStakingAccount>> GetAccountAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceSolStakingAccount>(GetUrl(sapi, v1, "sol-staking/account"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 150);
    }

    public  Task<RestCallResult<BinanceSolStakingQuota>> GetQuotaAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceSolStakingQuota>(GetUrl(sapi, v1, "sol-staking/sol/quota"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 150);
    }
}