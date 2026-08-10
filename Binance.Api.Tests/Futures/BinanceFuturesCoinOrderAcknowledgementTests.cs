using ApiSharp.Models;
using Binance.Api.Futures;

namespace Binance.Api.Tests.Futures;

public class BinanceFuturesCoinOrderAcknowledgementTests
{
    [Fact]
    public void ImmediateAcknowledgements_DoNotExposeQueryOnlyFillOrTimeFields()
    {
        var removedProperties = new[]
        {
            nameof(BinanceFuturesOrder.AveragePrice),
            nameof(BinanceFuturesOrder.QuoteQuantityFilled),
            nameof(BinanceFuturesOrder.BaseQuantityFilled),
            nameof(BinanceFuturesOrder.CreateTime),
            nameof(BinanceFuturesOrder.GoodTillDate)
        };

        foreach (var acknowledgementType in new[]
                 {
                     typeof(BinanceFuturesCoinRestOrderAcknowledgement),
                     typeof(BinanceFuturesCoinSocketOrderAcknowledgement)
                 })
        {
            var propertyNames = acknowledgementType.GetProperties().Select(property => property.Name).ToHashSet();
            Assert.All(removedProperties, propertyName => Assert.DoesNotContain(propertyName, propertyNames));
            Assert.DoesNotContain(nameof(BinanceFuturesCoinRestOrderModificationAcknowledgement.ModifyId), propertyNames);
        }

        var queryPropertyNames = typeof(BinanceFuturesOrder).GetProperties().Select(property => property.Name).ToHashSet();
        Assert.All(removedProperties, propertyName => Assert.Contains(propertyName, queryPropertyNames));

        AssertReturnType<IBinanceFuturesRestClientCoinTrade>(
            nameof(IBinanceFuturesRestClientCoinTrade.GetOrderAsync),
            typeof(Task<RestCallResult<BinanceFuturesOrder>>));
        AssertReturnType<IBinanceFuturesSocketClientCoinQueryTrade>(
            nameof(IBinanceFuturesSocketClientCoinQueryTrade.GetOrderAsync),
            typeof(Task<CallResult<BinanceFuturesCoinSocketOrder>>));
    }

    [Fact]
    public void RestAndSocketAcknowledgements_ExposeOnlyTheirPublishedTransportFields()
    {
        var restProperties = typeof(BinanceFuturesCoinRestOrderAcknowledgement)
            .GetProperties()
            .Select(property => property.Name)
            .ToHashSet();
        var socketProperties = typeof(BinanceFuturesCoinSocketOrderAcknowledgement)
            .GetProperties()
            .Select(property => property.Name)
            .ToHashSet();
        var restModificationProperties = typeof(BinanceFuturesCoinRestOrderModificationAcknowledgement)
            .GetProperties()
            .Select(property => property.Name)
            .ToHashSet();
        var socketModificationProperties = typeof(BinanceFuturesCoinSocketOrderModificationAcknowledgement)
            .GetProperties()
            .Select(property => property.Name)
            .ToHashSet();

        Assert.Contains(nameof(BinanceFuturesCoinRestOrderAcknowledgement.ActivatePrice), restProperties);
        Assert.Contains(nameof(BinanceFuturesCoinRestOrderAcknowledgement.CallbackRate), restProperties);
        Assert.Contains(nameof(BinanceFuturesCoinRestOrderAcknowledgement.PriceMatch), restProperties);
        Assert.Contains(nameof(BinanceFuturesCoinRestOrderAcknowledgement.SelfTradePreventionMode), restProperties);

        Assert.DoesNotContain(nameof(BinanceFuturesCoinRestOrderAcknowledgement.ActivatePrice), socketProperties);
        Assert.DoesNotContain(nameof(BinanceFuturesCoinRestOrderAcknowledgement.CallbackRate), socketProperties);
        Assert.DoesNotContain(nameof(BinanceFuturesCoinRestOrderAcknowledgement.PriceMatch), socketProperties);
        Assert.DoesNotContain(nameof(BinanceFuturesCoinRestOrderAcknowledgement.SelfTradePreventionMode), socketProperties);
        Assert.DoesNotContain(nameof(BinanceFuturesCoinRestOrderModificationAcknowledgement.ModifyId), restProperties);
        Assert.DoesNotContain(nameof(BinanceFuturesCoinSocketOrderModificationAcknowledgement.ModifyId), socketProperties);
        Assert.Contains(nameof(BinanceFuturesCoinRestOrderModificationAcknowledgement.ModifyId), restModificationProperties);
        Assert.Contains(nameof(BinanceFuturesCoinSocketOrderModificationAcknowledgement.ModifyId), socketModificationProperties);
    }

    [Fact]
    public void RestMutationMethods_ReturnDedicatedAcknowledgements()
    {
        AssertReturnType<IBinanceFuturesRestClientCoinTrade>(
            nameof(IBinanceFuturesRestClientCoinTrade.PlaceOrderAsync),
            typeof(Task<RestCallResult<BinanceFuturesCoinRestOrderAcknowledgement>>));
        AssertReturnType<IBinanceFuturesRestClientCoinTrade>(
            nameof(IBinanceFuturesRestClientCoinTrade.PlaceOrdersAsync),
            typeof(Task<RestCallResult<List<CallResult<BinanceFuturesCoinRestOrderAcknowledgement>>>>));
        AssertReturnType<IBinanceFuturesRestClientCoinTrade>(
            nameof(IBinanceFuturesRestClientCoinTrade.ModifyOrderAsync),
            typeof(Task<RestCallResult<BinanceFuturesCoinRestOrderModificationAcknowledgement>>));
        AssertReturnType<IBinanceFuturesRestClientCoinTrade>(
            nameof(IBinanceFuturesRestClientCoinTrade.ModifyOrdersAsync),
            typeof(Task<RestCallResult<List<CallResult<BinanceFuturesCoinRestOrderModificationAcknowledgement>>>>));
        AssertReturnType<IBinanceFuturesRestClientCoinTrade>(
            nameof(IBinanceFuturesRestClientCoinTrade.CancelOrderAsync),
            typeof(Task<RestCallResult<BinanceFuturesCoinRestOrderAcknowledgement>>));
        AssertReturnType<IBinanceFuturesRestClientCoinTrade>(
            nameof(IBinanceFuturesRestClientCoinTrade.CancelOrdersAsync),
            typeof(Task<RestCallResult<List<CallResult<BinanceFuturesCoinRestOrderAcknowledgement>>>>));
    }

    [Fact]
    public void SocketMutationMethods_ReturnDedicatedAcknowledgements()
    {
        AssertReturnType<IBinanceFuturesSocketClientCoinQueryTrade>(
            nameof(IBinanceFuturesSocketClientCoinQueryTrade.PlaceOrderAsync),
            typeof(Task<CallResult<BinanceFuturesCoinSocketOrderAcknowledgement>>));
        AssertReturnType<IBinanceFuturesSocketClientCoinQueryTrade>(
            nameof(IBinanceFuturesSocketClientCoinQueryTrade.ModifyOrderAsync),
            typeof(Task<CallResult<BinanceFuturesCoinSocketOrderModificationAcknowledgement>>));
        AssertReturnType<IBinanceFuturesSocketClientCoinQueryTrade>(
            nameof(IBinanceFuturesSocketClientCoinQueryTrade.CancelOrderAsync),
            typeof(Task<CallResult<BinanceFuturesCoinSocketOrderAcknowledgement>>));
    }

    private static void AssertReturnType<TClient>(string methodName, Type expectedReturnType)
    {
        var method = typeof(TClient).GetMethod(methodName);
        Assert.NotNull(method);
        Assert.Equal(expectedReturnType, method.ReturnType);
    }
}
