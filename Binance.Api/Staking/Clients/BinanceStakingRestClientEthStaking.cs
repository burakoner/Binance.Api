namespace Binance.Api.Staking;

internal partial class BinanceStakingRestClientEth
{
    public Task<RestCallResult<BinanceEthStakingStake>> StakeAsync(decimal quantity, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "amount", quantity.ToString(CultureInfo.InvariantCulture) },
        };
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceEthStakingStake>(GetUrl(sapi, v2, "eth-staking/eth/stake"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 150);
    }

    public Task<RestCallResult<BinanceEthStakingRedeem>> RedeemAsync(decimal quantity, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection()
        {
            { "amount", quantity.ToString(CultureInfo.InvariantCulture) },
        };
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceEthStakingRedeem>(GetUrl(sapi, v1, "eth-staking/eth/redeem"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 150);
    }

    public Task<RestCallResult<BinanceEthStakingWrap>> WrapAsync(decimal quantity, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection()
        {
            { "amount", quantity.ToString(CultureInfo.InvariantCulture) }
        };
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceEthStakingWrap>(GetUrl(sapi, v1, "eth-staking/wbeth/wrap"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 150);
    }

}