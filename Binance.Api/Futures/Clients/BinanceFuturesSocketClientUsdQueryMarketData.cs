namespace Binance.Api.Futures;

internal partial class BinanceFuturesSocketClientUsd
{
    public async Task<CallResult<BinanceFuturesOrderBook>> GetOrderBookAsync(string symbol, int? limit = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddParameter("symbol", symbol);
        parameters.AddOptional("limit", limit);

        int weight = limit <= 50 ? 2 : limit <= 100 ? 5 : limit <= 500 ? 10 : 20;
        var result = await RequestAsync<BinanceFuturesOrderBook>("ws-fapi/v1", $"depth", parameters, weight: weight, ct: ct).ConfigureAwait(false);
        if (result) result.Data.Symbol = symbol;

        return result;
    }

    public async Task<CallResult<BinanceFuturesPrice>> GetPriceAsync(string symbol, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddParameter("symbol", symbol);
        return await RequestAsync<BinanceFuturesPrice>("ws-fapi/v1", $"ticker.price", parameters, weight: 1, ct: ct);
    }

    public async Task<CallResult<List<BinanceFuturesPrice>>> GetPricesAsync(CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        return await RequestAsync<List<BinanceFuturesPrice>>("ws-fapi/v1", $"ticker.price", parameters, weight: 2, ct: ct);
    }

    public async Task<CallResult<BinanceFuturesBookTicker>> GetBookPriceAsync(string symbol, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddParameter("symbol", symbol);
        return await RequestAsync<BinanceFuturesBookTicker>("ws-fapi/v1", $"ticker.book", parameters, weight: 2, ct: ct);
    }

    public async Task<CallResult<List<BinanceFuturesBookTicker>>> GetBookPricesAsync(CancellationToken ct = default)
    {
        return await RequestAsync<List<BinanceFuturesBookTicker>>("ws-fapi/v1", $"ticker.book", [], weight: 5, ct: ct);
    }
}