namespace Binance.Api.Staking;

internal partial class BinanceStakingRestClientSol
{
    public  Task<RestCallResult<BinanceQueryRecords<BinanceSolStakingRecord>>> GetStakingHistoryAsync(DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("current", page);
        parameters.AddOptional("size", pageSize);
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceQueryRecords<BinanceSolStakingRecord>>(GetUrl(sapi, v1, "sol-staking/sol/history/stakingHistory"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 150);
    }

    public  Task<RestCallResult<BinanceQueryRecords<BinanceSolStakingRedemption>>> GetRedemptionHistoryAsync(DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("current", page);
        parameters.AddOptional("size", pageSize);
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceQueryRecords<BinanceSolStakingRedemption>>(GetUrl(sapi, v1, "sol-staking/sol/history/redemptionHistory"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 150);
    }

    public  Task<RestCallResult<BinanceSolStakingRewards>> GetBnSolRewardsHistoryAsync(DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("current", page);
        parameters.AddOptional("size", pageSize);
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceSolStakingRewards>(GetUrl(sapi, v1, "sol-staking/sol/history/bnsolRewardsHistory"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 150);
    }

    public  Task<RestCallResult<BinanceQueryRecords<BinanceSolStakingBnSolRate>>> GetBnSolRateHistoryAsync(DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("current", page);
        parameters.AddOptional("size", pageSize);
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceQueryRecords<BinanceSolStakingBnSolRate>>(GetUrl(sapi, v1, "sol-staking/sol/history/rateHistory"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 150);
    }

    public  Task<RestCallResult<BinanceQueryRecords<BinanceSolStakingBnSolReward>>> GetBoostRewardsHistoryAsync(BinanceSolStakingRewardType type, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddEnum("type", type);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("current", page);
        parameters.AddOptional("size", pageSize);
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceQueryRecords<BinanceSolStakingBnSolReward>>(GetUrl(sapi, v1, "sol-staking/sol/history/boostRewardsHistory"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 150);
    }

    public  Task<RestCallResult<List<BinanceSolStakingUnclaimedReward>>> GetUnclaimedRewardsAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<List<BinanceSolStakingUnclaimedReward>>(GetUrl(sapi, v1, "sol-staking/sol/history/unclaimedRewards"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 150);
    }
}