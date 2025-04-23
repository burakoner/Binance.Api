namespace Binance.Api.Futures;

public interface IBinanceRestApiClientFutures
{
    IBinanceRestApiClientFuturesCoin Coin { get; }
    IBinanceRestApiClientFuturesUsd USD { get; }
}