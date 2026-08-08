namespace Binance.Api.Margin;

/// <summary>
/// Interface for the Binance Margin REST API Client Market Data Methods
/// </summary>
public interface IBinanceMarginRestClientMarketData
{
    /// <summary>
    /// Get cross margin collateral ratio
    /// <para><a href="https://developers.binance.com/docs/margin_trading/market-data" /></para>
    /// </summary>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceCrossMarginCollateralRatio>>> GetCrossMarginCollateralRatioAsync(int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get all asset pairs available for margin trading
    /// <para><a href="https://developers.binance.com/docs/margin_trading/market-data/Get-All-Cross-Margin-Pairs" /></para>
    /// </summary>
    /// <param name="symbol">Filter by symbol, for example `ETHUSDT`</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of margin pairs</returns>
    Task<RestCallResult<List<BinanceMarginSymbol>>> GetMarginSymbolsAsync(string? symbol = null, CancellationToken ct = default);

    /// <summary>
    /// Isolated margin symbol info
    /// <para><a href="https://developers.binance.com/docs/margin_trading/market-data/Get-All-Isolated-Margin-Symbol" /></para>
    /// </summary>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="symbol">Filter by symbol, for example `ETHUSDT`</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceIsolatedMarginSymbol>>> GetIsolatedMarginSymbolsAsync(string? symbol = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get all assets available for margin trading
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#get-all-margin-assets-market_data" /></para>
    /// </summary>
    /// <param name="asset">Filter by asset, for example `ETH`</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of margin assets</returns>
    Task<RestCallResult<List<BinanceMarginAsset>>> GetMarginAssetsAsync(string? asset = null, CancellationToken ct = default);

    /// <summary>
    /// Get cross and isolated delist schedule
    /// <para><a href="https://developers.binance.com/docs/margin_trading/market-data/Get-Delist-Schedule" /></para>
    /// </summary>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
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
    /// Get Isolated Margin Tier Data
    /// <para><a href="https://developers.binance.com/docs/margin_trading/market-data/Query-Isolated-Margin-Tier-Data" /></para>
    /// </summary>
    /// <param name="symbol">The symbol to get, for example `ETHUSDT`</param>
    /// <param name="tier">Tier level</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceIsolatedMarginTier>>> GetIsolatedMarginTierDataAsync(string symbol, int? tier = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get margin price index
    /// <para><a href="https://developers.binance.com/docs/margin_trading/market-data/Query-Margin-PriceIndex" /></para>
    /// </summary>
    /// <param name="symbol">The symbol to get, for example `ETHUSDT`</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Margin price index</returns>
    Task<RestCallResult<BinanceMarginPriceIndex>> GetMarginPriceIndexAsync(string symbol, CancellationToken ct = default);

    /// <summary>
    /// Get Margin Available Inventory
    /// <para><a href="https://developers.binance.com/docs/margin_trading/market-data/Query-margin-avaliable-inventory" /></para>
    /// </summary>
    /// <param name="type">The margin type to query for</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceMarginAvailableInventory>> GetMarginAvaliableInventoryAsync(BinanceMarginInventoryType type, CancellationToken ct = default);

}
