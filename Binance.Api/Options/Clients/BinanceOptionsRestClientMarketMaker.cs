namespace Binance.Api.Options;

internal partial class BinanceOptionsRestClientMarketMaker(BinanceOptionsRestClient parent) : IBinanceOptionsRestClientMarketMaker
{
    // Api
    private const string v1 = "1";
    private const string eapi = "eapi";

    // Parent
    private BinanceRestApiClient __ { get; } = parent._;
    private BinanceOptionsRestClient _ { get; } = parent;

    // Internal
    private ILogger Logger => __.Logger;
    private BinanceRestApiClientOptions RestOptions => __.ApiOptions;


}