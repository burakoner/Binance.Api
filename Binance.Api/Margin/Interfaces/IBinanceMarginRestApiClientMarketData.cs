using Binance.Api.Margin.Responses;
using Binance.Net.Objects.Models.Spot.Margin;

namespace Binance.Api.Margin;

/// <summary>
/// Interface for the Binance Margin REST API Client Market Data Methods
/// </summary>
public interface IBinanceMarginRestApiClientMarketData
{
    Task<RestCallResult<IEnumerable<BinanceCrossMarginCollateralRatio>>> GetCrossMarginCollateralRatioAsync(int? receiveWindow = null, CancellationToken ct = default);
    Task<RestCallResult<IEnumerable<BinanceMarginSymbol>>> GetMarginSymbolsAsync(string? symbol = null, CancellationToken ct = default);
    Task<RestCallResult<IEnumerable<BinanceIsolatedMarginSymbol>>> GetIsolatedMarginSymbolsAsync(string? symbol = null, int? receiveWindow = null, CancellationToken ct = default);
    Task<RestCallResult<IEnumerable<BinanceMarginAsset>>> GetMarginAssetsAsync(string? asset = null, CancellationToken ct = default);
    Task<RestCallResult<IEnumerable<BinanceMarginDelistSchedule>>> GetMarginDelistScheduleAsync(int? receiveWindow = null, CancellationToken ct = default);
    Task<RestCallResult<IEnumerable<BinanceIsolatedMarginTier>>> GetIsolatedMarginTierDataAsync(string symbol, int? tier = null, int? receiveWindow = null, CancellationToken ct = default);
    Task<RestCallResult<BinanceMarginPriceIndex>> GetMarginPriceIndexAsync(string symbol, CancellationToken ct = default);
    Task<RestCallResult<BinanceMarginAvailableInventory>> GetMarginAvaliableInventoryAsync(BinanceMarginInventoryType type, CancellationToken ct = default);
    Task<RestCallResult<IEnumerable<BinanceCrossMarginProLiabilityCoinLeverageBracket>>> GetLiabilityCoinLeverageBracketInCrossMarginProModeAsync(CancellationToken ct = default);
}