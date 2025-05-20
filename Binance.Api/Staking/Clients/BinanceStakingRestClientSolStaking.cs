namespace Binance.Api.Staking;

internal partial class BinanceStakingRestClientSol
{
    public  Task<RestCallResult<BinanceSolStakingStake>> StakeAsync(decimal quantity, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "amount", quantity.ToString(CultureInfo.InvariantCulture) },
        };
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return __.RequestAsync<BinanceSolStakingStake>(GetUrl(sapi, v1, "sol-staking/sol/stake"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 150);
    }

    public  Task<RestCallResult<BinanceSolStakingRedeem>> RedeemAsync(decimal quantity, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection()
        {
            { "amount", quantity.ToString(CultureInfo.InvariantCulture) },
        };
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return __.RequestAsync<BinanceSolStakingRedeem>(GetUrl(sapi, v1, "sol-staking/sol/redeem"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 150);
    }

    public  Task<RestCallResult<BinanceSolStakingClaim>> ClaimAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return __.RequestAsync<BinanceSolStakingClaim>(GetUrl(sapi, v1, "sol-staking/sol/claim"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 150);
    }
}