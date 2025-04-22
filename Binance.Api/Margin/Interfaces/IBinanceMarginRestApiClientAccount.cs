using Binance.Api.Margin.Responses;
using Binance.Api.Wallet;
using Binance.Net.Objects.Models.Spot.IsolatedMargin;
using Binance.Net.Objects.Models.Spot.Margin;

namespace Binance.Api.Margin;

/// <summary>
/// Interface for the Binance Margin REST API Client Account Methods
/// </summary>
public interface IBinanceMarginRestApiClientAccount
{
    Task<RestCallResult<BinanceCrossMarginLeverageResult>> CrossMarginAdjustMaxLeverageAsync(int maxLeverage, int? receiveWindow = null, CancellationToken ct = default);
    Task<RestCallResult<CreateIsolatedMarginAccountResult>> DisableIsolatedMarginAccountAsync(string symbol, int? receiveWindow = null, CancellationToken ct = default);
    Task<RestCallResult<CreateIsolatedMarginAccountResult>> EnableIsolatedMarginAccountAsync(string symbol, int? receiveWindow = null, CancellationToken ct = default);
    Task<RestCallResult<BinanceBnbBurnStatus>> GetBnbBurnStatusAsync(int? receiveWindow = null, CancellationToken ct = default);
    Task<RestCallResult<BinanceMarginLevel>> GetMarginLevelInformationAsync(int? receiveWindow = null, CancellationToken ct = default);
    Task<RestCallResult<BinanceMarginAccount>> GetMarginAccountInfoAsync(int? receiveWindow = null, CancellationToken ct = default);
    Task<RestCallResult<IEnumerable<BinanceInterestMarginData>>> GetInterestMarginDataAsync(string? asset = null, string? vipLevel = null, int? receiveWindow = null, CancellationToken ct = default);
    Task<RestCallResult<IsolatedMarginAccountLimit>> GetEnabledIsolatedMarginAccountLimitAsync(int? receiveWindow = null, CancellationToken ct = default);
    Task<RestCallResult<BinanceIsolatedMarginAccount>> GetIsolatedMarginAccountAsync(int? receiveWindow = null, CancellationToken ct = default);
    Task<RestCallResult<IEnumerable<BinanceIsolatedMarginFeeData>>> GetIsolatedMarginFeeDataAsync(string? symbol = null, int? vipLevel = null, int? receiveWindow = null, CancellationToken ct = default);
    // TODO: Query Cross Isolated Margin Capital Flow (USER_DATA)
}