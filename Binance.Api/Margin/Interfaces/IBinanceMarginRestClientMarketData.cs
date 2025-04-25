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
    Task<RestCallResult<IEnumerable<BinanceCrossMarginCollateralRatio>>> GetCrossMarginCollateralRatioAsync(int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get all asset pairs available for margin trading
    /// <para><a href="https://developers.binance.com/docs/margin_trading/market-data/Get-All-Cross-Margin-Pairs" /></para>
    /// </summary>
    /// <param name="symbol">Filter by symbol, for example `ETHUSDT`</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of margin pairs</returns>
    Task<RestCallResult<IEnumerable<BinanceMarginSymbol>>> GetMarginSymbolsAsync(string? symbol = null, CancellationToken ct = default);

    /// <summary>
    /// Isolated margin symbol info
    /// <para><a href="https://developers.binance.com/docs/margin_trading/market-data/Get-All-Isolated-Margin-Symbol" /></para>
    /// </summary>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="symbol">Filter by symbol, for example `ETHUSDT`</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<IEnumerable<BinanceIsolatedMarginSymbol>>> GetIsolatedMarginSymbolsAsync(string? symbol = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get all assets available for margin trading
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#get-all-margin-assets-market_data" /></para>
    /// </summary>
    /// <param name="asset">Filter by asset, for example `ETH`</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of margin assets</returns>
    Task<RestCallResult<IEnumerable<BinanceMarginAsset>>> GetMarginAssetsAsync(string? asset = null, CancellationToken ct = default);

    /// <summary>
    /// Get cross and isolated delist schedule
    /// <para><a href="https://developers.binance.com/docs/margin_trading/market-data/Get-Delist-Schedule" /></para>
    /// </summary>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<IEnumerable<BinanceMarginDelistSchedule>>> GetMarginDelistScheduleAsync(int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get Isolated Margin Tier Data
    /// <para><a href="https://developers.binance.com/docs/margin_trading/market-data/Query-Isolated-Margin-Tier-Data" /></para>
    /// </summary>
    /// <param name="symbol">The symbol to get, for example `ETHUSDT`</param>
    /// <param name="tier">Tier level</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<IEnumerable<BinanceIsolatedMarginTier>>> GetIsolatedMarginTierDataAsync(string symbol, int? tier = null, int? receiveWindow = null, CancellationToken ct = default);

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

    /// <summary>
    /// Get Liability Coin Leverage Bracket in Cross Margin Pro Mode
    /// <para><a href="https://developers.binance.com/docs/margin_trading/market-data/Query-Liability-Coin-Leverage-Bracket-in-Cross-Margin-Pro-Mode" /></para>
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<IEnumerable<BinanceCrossMarginProLiabilityCoinLeverageBracket>>> GetLiabilityCoinLeverageBracketInCrossMarginProModeAsync(CancellationToken ct = default);
}