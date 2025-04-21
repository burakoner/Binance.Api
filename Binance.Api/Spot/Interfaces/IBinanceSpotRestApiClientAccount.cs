namespace Binance.Api.Spot;

/// <summary>
/// Interface for the Binance Spot REST API Client Account Methods
/// </summary>
public interface IBinanceSpotRestApiClientAccount
{
    Task<RestCallResult<BinanceSpotAccount>> GetAccountAsync(bool? omitZeroBalances = null, int? receiveWindow = null, CancellationToken ct = default);
    Task<RestCallResult<IEnumerable<BinanceSpotUserTrade>>> GetUserTradesAsync(string symbol, long? orderId = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, long? fromId = null, int? receiveWindow = null, CancellationToken ct = default);
    Task<RestCallResult<IEnumerable<BinanceOrderRateLimit>>> GetOrderRateLimitStatusAsync(int? receiveWindow = null, CancellationToken ct = default);
    Task<RestCallResult<IEnumerable<BinancePreventedTrade>>> GetPreventedTradesAsync(string symbol, long? orderId = null, long? preventedMatchId = null, long? fromPreventedMatchId = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default);

    // TODO: Query Allocations (USER_DATA)
    // TODO: Query Commission Rates (USER_DATA)
    // TODO: Query Order Amendments (USER_DATA)
}