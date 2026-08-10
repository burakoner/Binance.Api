namespace Binance.Api.Futures;

internal partial class BinanceFuturesSocketClientUsd
{
    public async Task<CallResult<BinanceFuturesOrderBook>> GetOrderBookAsync(string symbol, int? limit = null, CancellationToken ct = default)
    {
        ValidateMarketDataSymbol(symbol);
        var parameters = new ParameterCollection();
        parameters.AddParameter("symbol", symbol);
        parameters.AddOptional("limit", limit);

        var weight = DepthRequestWeight(limit);
        var result = await RequestAsync<BinanceFuturesOrderBook>("ws-fapi/v1", $"depth", parameters, weight: weight, ct: ct).ConfigureAwait(false);
        if (result) result.Data.Symbol = symbol;

        return result;
    }

    public async Task<CallResult<BinanceFuturesPrice>> GetPriceAsync(string symbol, CancellationToken ct = default)
    {
        ValidateMarketDataSymbol(symbol);
        var parameters = new ParameterCollection();
        parameters.AddParameter("symbol", symbol);
        return await RequestAsync<BinanceFuturesPrice>("ws-fapi/v1", $"ticker.price", parameters, weight: 1, ct: ct);
    }

    public async Task<CallResult<List<BinanceFuturesPrice>>> GetPricesAsync(CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        return await RequestAsync<List<BinanceFuturesPrice>>("ws-fapi/v1", $"ticker.price", parameters, weight: 2, ct: ct);
    }

    public async Task<CallResult<BinanceFuturesWebSocketBookTicker>> GetBookPriceAsync(string symbol, CancellationToken ct = default)
    {
        ValidateMarketDataSymbol(symbol);
        var parameters = new ParameterCollection();
        parameters.AddParameter("symbol", symbol);
        return await RequestAsync<BinanceFuturesWebSocketBookTicker>("ws-fapi/v1", $"ticker.book", parameters, weight: 2, ct: ct);
    }

    public async Task<CallResult<List<BinanceFuturesWebSocketBookTicker>>> GetBookPricesAsync(CancellationToken ct = default)
    {
        return await RequestAsync<List<BinanceFuturesWebSocketBookTicker>>("ws-fapi/v1", $"ticker.book", [], weight: 5, ct: ct);
    }

    internal static int DepthRequestWeight(int? limit)
        => limit switch
        {
            5 or 10 or 20 or 50 => 2,
            100 => 5,
            null or 500 => 10,
            1000 => 20,
            _ => throw new ArgumentOutOfRangeException(nameof(limit), "Limit must be one of 5, 10, 20, 50, 100, 500, or 1000.")
        };

    private static void ValidateMarketDataSymbol(string symbol)
    {
        if (string.IsNullOrWhiteSpace(symbol))
            throw new ArgumentException("Symbol is required.", nameof(symbol));
    }
}
