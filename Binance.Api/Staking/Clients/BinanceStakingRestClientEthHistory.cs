﻿namespace Binance.Api.Staking;

internal partial class BinanceStakingRestClientEth
{
    public Task<RestCallResult<BinanceQueryRecords<BinanceEthStakingRecord>>> GetStakingHistoryAsync(DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("current", page);
        parameters.AddOptional("size", pageSize);
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return __.RequestAsync<BinanceQueryRecords<BinanceEthStakingRecord>>(GetUrl(sapi, v1, "eth-staking/eth/history/stakingHistory"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 150);
    }

    public Task<RestCallResult<BinanceQueryRecords<BinanceEthStakingRedemption>>> GetRedemptionHistoryAsync(DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("current", page);
        parameters.AddOptional("size", pageSize);
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return __.RequestAsync<BinanceQueryRecords<BinanceEthStakingRedemption>>(GetUrl(sapi, v1, "eth-staking/eth/history/redemptionHistory"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 150);
    }

    public Task<RestCallResult<BinanceQueryRecords<BinanceEthStakingReward>>> GetRewardsHistoryAsync(DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("current", page);
        parameters.AddOptional("size", pageSize);
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return __.RequestAsync<BinanceQueryRecords<BinanceEthStakingReward>>(GetUrl(sapi, v1, "eth-staking/eth/history/rewardsHistory"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 150);
    }

    public Task<RestCallResult<BinanceEthStakingWbEthRewards>> GetWbEthRewardsHistoryAsync(DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("current", page);
        parameters.AddOptional("size", pageSize);
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return __.RequestAsync<BinanceEthStakingWbEthRewards>(GetUrl(sapi, v1, "eth-staking/eth/history/wbethRewardsHistory"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 150);
    }

    public Task<RestCallResult<BinanceQueryRecords<BinanceEthStakingWbEthRate>>> GetWbEthRateHistoryAsync(DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("current", page);
        parameters.AddOptional("size", pageSize);
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return __.RequestAsync<BinanceQueryRecords<BinanceEthStakingWbEthRate>>(GetUrl(sapi, v1, "eth-staking/eth/history/rateHistory"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 150);
    }

    public Task<RestCallResult<BinanceQueryRecords<BinanceEthStakingWbEthWrap>>> GetWbEthWrapHistoryAsync(DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("current", page);
        parameters.AddOptional("size", pageSize);
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return __.RequestAsync<BinanceQueryRecords<BinanceEthStakingWbEthWrap>>(GetUrl(sapi, v1, "eth-staking/wbeth/history/wrapHistory"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 150);
    }

    public Task<RestCallResult<BinanceQueryRecords<BinanceEthStakingWbEthWrap>>> GetWbEthUnwrapHistoryAsync(DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("current", page);
        parameters.AddOptional("size", pageSize);
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return __.RequestAsync<BinanceQueryRecords<BinanceEthStakingWbEthWrap>>(GetUrl(sapi, v1, "eth-staking/wbeth/history/unwrapHistory"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 150);
    }
}