namespace Binance.Api.SimpleEarn;

internal partial class BinanceSimpleEarnRestClientFlexible
{
    public Task<RestCallResult<BinanceSimpleEarnFlexiblePurchase>> SubscribeAsync(string productId, decimal quantity, bool? autoSubscribe = null, BinanceSimpleEarnSourceAccount? sourceAccount = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection()
        {
            { "productId", productId },
            { "amount", quantity }
        };
        parameters.AddOptional("autoSubscribe", autoSubscribe);
        parameters.AddOptionalEnum("sourceAccount", sourceAccount);
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceSimpleEarnFlexiblePurchase>(GetUrl(sapi, v1, "simple-earn/flexible/subscribe"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceSimpleEarnFlexibleRedemption>> RedeemAsync(string productId, bool? redeemAll = null, decimal? quantity = null, BinanceSimpleEarnSourceAccount? destinationAccount = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection()
        {
            { "productId", productId },
        };
        parameters.AddOptional("redeemAll", redeemAll);
        parameters.AddOptional("amount", quantity);
        parameters.AddOptionalEnum("destAccount", destinationAccount);
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceSimpleEarnFlexibleRedemption>(GetUrl(sapi, v1, "simple-earn/flexible/redeem"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceSimpleEarnResult>> SetAutoSubscribeAsync(string productId, bool autoSubscribe, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection()
        {
            { "productId", productId },
            { "autoSubscribe", autoSubscribe }
        };
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceSimpleEarnResult>(GetUrl(sapi, v1, "simple-earn/flexible/setAutoSubscribe"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 150);
    }
    public Task<RestCallResult<BinanceSimpleEarnFlexiblePreview>> GetSubscriptionPreviewAsync(string productId, decimal quantity, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "productId", productId },
            { "amount", quantity }
        };
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceSimpleEarnFlexiblePreview>(GetUrl(sapi, v1, "simple-earn/flexible/subscriptionPreview"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 150);
    }
}