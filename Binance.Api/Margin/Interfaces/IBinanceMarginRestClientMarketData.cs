namespace Binance.Api.Margin;

/// <summary>
/// Interface for the Binance Margin REST API Client Market Data Methods
/// </summary>
public interface IBinanceMarginRestClientMarketData
{
    /// <summary>
    /// Gets Cross Margin collateral ratios.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-margin-trading/api/rest-api/market-data#cross-margin-collateral-ratio" /></para>
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    Task<RestCallResult<List<BinanceCrossMarginCollateralRatio>>> GetCrossMarginCollateralRatioAsync(CancellationToken ct = default);

    /// <summary>
    /// Gets all Cross Margin pairs, optionally filtered by symbol.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-margin-trading/api/rest-api/market-data#get-all-cross-margin-pairs" /></para>
    /// </summary>
    /// <param name="symbol">Optional symbol filter, for example <c>ETHUSDT</c>.</param>
    /// <param name="ct">Cancellation token.</param>
    Task<RestCallResult<List<BinanceMarginSymbol>>> GetMarginSymbolsAsync(string? symbol = null, CancellationToken ct = default);

    /// <summary>
    /// Gets all Isolated Margin pairs, optionally filtered by symbol.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-margin-trading/api/rest-api/market-data#get-all-isolated-margin-symbol" /></para>
    /// </summary>
    /// <param name="symbol">Optional symbol filter, for example <c>ETHUSDT</c>.</param>
    /// <param name="receiveWindow">Optional receive window in milliseconds, maximum 60000.</param>
    /// <param name="ct">Cancellation token.</param>
    Task<RestCallResult<List<BinanceIsolatedMarginSymbol>>> GetIsolatedMarginSymbolsAsync(string? symbol = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Gets all Margin assets, optionally filtered by asset.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-margin-trading/api/rest-api/market-data#get-all-margin-assets" /></para>
    /// </summary>
    /// <param name="asset">Optional asset filter, for example <c>ETH</c>.</param>
    /// <param name="ct">Cancellation token.</param>
    Task<RestCallResult<List<BinanceMarginAsset>>> GetMarginAssetsAsync(string? asset = null, CancellationToken ct = default);

    /// <summary>
    /// Gets the Cross and Isolated Margin delisting schedule.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-margin-trading/api/rest-api/market-data#get-delist-schedule" /></para>
    /// </summary>
    /// <param name="receiveWindow">Optional receive window in milliseconds, maximum 60000.</param>
    /// <param name="ct">Cancellation token.</param>
    Task<RestCallResult<List<BinanceMarginDelistSchedule>>> GetMarginDelistScheduleAsync(int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get Cross Margin symbols subject to the current index-price limit-order restriction.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-margin-trading/api/rest-api/market-data#get-limit-price-pairs" /></para>
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    Task<RestCallResult<BinanceMarginLimitPricePairs>> GetMarginLimitPricePairsAsync(CancellationToken ct = default);

    /// <summary>
    /// Get the upcoming Cross and Isolated Margin listing schedule.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-margin-trading/api/rest-api/market-data#get-list-schedule" /></para>
    /// </summary>
    /// <param name="receiveWindow">Optional receive window in milliseconds. The maximum is 60000.</param>
    /// <param name="ct">Cancellation token.</param>
    Task<RestCallResult<List<BinanceMarginListSchedule>>> GetMarginListScheduleAsync(int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get asset-specific risk-based liquidation ratios.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-margin-trading/api/rest-api/market-data#get-margin-asset-risk-based-liquidation-ratio" /></para>
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    Task<RestCallResult<List<BinanceMarginRiskBasedLiquidationRatio>>> GetMarginRiskBasedLiquidationRatiosAsync(CancellationToken ct = default);

    /// <summary>
    /// Get assets restricted from opening long positions or exceeding maximum collateral.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-margin-trading/api/rest-api/market-data#get-margin-restricted-assets" /></para>
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    Task<RestCallResult<BinanceMarginRestrictedAssets>> GetMarginRestrictedAssetsAsync(CancellationToken ct = default);

    /// <summary>
    /// Gets Isolated Margin tier data for a symbol.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-margin-trading/api/rest-api/market-data#query-isolated-margin-tier-data" /></para>
    /// </summary>
    /// <param name="symbol">Symbol to query, for example <c>ETHUSDT</c>.</param>
    /// <param name="tier">Optional tier filter; omitting it returns all tiers.</param>
    /// <param name="receiveWindow">Optional receive window in milliseconds, maximum 60000.</param>
    /// <param name="ct">Cancellation token.</param>
    Task<RestCallResult<List<BinanceIsolatedMarginTier>>> GetIsolatedMarginTierDataAsync(string symbol, long? tier = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Gets the Margin price index for a symbol.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-margin-trading/api/rest-api/market-data#query-margin-priceindex" /></para>
    /// </summary>
    /// <param name="symbol">Symbol to query, for example <c>ETHUSDT</c>.</param>
    /// <param name="ct">Cancellation token.</param>
    Task<RestCallResult<BinanceMarginPriceIndex>> GetMarginPriceIndexAsync(string symbol, CancellationToken ct = default);

    /// <summary>
    /// Gets the available Cross or Isolated Margin inventory.
    /// <para><a href="https://developers.binance.com/en/docs/catalog/core-trading-margin-trading/api/rest-api/market-data#query-margin-available-inventory" /></para>
    /// </summary>
    /// <param name="type">Margin scope to query.</param>
    /// <param name="ct">Cancellation token.</param>
    Task<RestCallResult<BinanceMarginAvailableInventory>> GetMarginAvailableInventoryAsync(BinanceMarginInventoryType type, CancellationToken ct = default);

}
