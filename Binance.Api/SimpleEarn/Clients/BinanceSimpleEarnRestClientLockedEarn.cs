namespace Binance.Api.SimpleEarn;

internal partial class BinanceSimpleEarnRestClientLocked
{
    public Task<RestCallResult<BinanceSimpleEarnLockedPurchase>> SubscribeAsync(string projectId, decimal quantity, bool? autoSubscribe = null, BinanceSimpleEarnSourceAccount? sourceAccount = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection()
        {
            { "projectId", projectId },
            { "amount", quantity }
        };
        parameters.AddOptional("autoSubscribe", autoSubscribe);
        parameters.AddOptionalEnum("sourceAccount", sourceAccount);
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceSimpleEarnLockedPurchase>(GetUrl(sapi, v1, "simple-earn/locked/subscribe"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceSimpleEarnLockedRedemption>> RedeemAsync(string positionId, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection()
        {
            { "positionId", positionId },
        };
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceSimpleEarnLockedRedemption>(GetUrl(sapi, v1, "simple-earn/locked/redeem"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceSimpleEarnResult>> SetAutoSubscribeAsync(string positionId, bool autoSubscribe, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection()
        {
            { "positionId", positionId },
            { "autoSubscribe", autoSubscribe }
        };
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceSimpleEarnResult>(GetUrl(sapi, v1, "simple-earn/locked/setAutoSubscribe"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 150);
    }

    public Task<RestCallResult<List<BinanceSimpleEarnLockedPreview>>> GetSubscriptionPreviewAsync(string projectId, decimal quantity, bool? autoSubscribe = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "projectId", projectId },
            { "amount", quantity }
        };
        parameters.AddOptional("autoSubscribe", autoSubscribe);
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<List<BinanceSimpleEarnLockedPreview>>(GetUrl(sapi, v1, "simple-earn/locked/subscriptionPreview"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 150);
    }

    public Task<RestCallResult<BinanceSimpleEarnResult>> SetRedeemOptionAsync(string positionId, BinanceSimpleEarnRedeemOption redeemTo, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection()
        {
            { "positionId", positionId },
        };
        parameters.AddEnum("redeemTo", redeemTo);
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceSimpleEarnResult>(GetUrl(sapi, v1, "simple-earn/locked/setRedeemOption"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 150);
    }
}