namespace Binance.Api.SubAccount;

/// <summary>
/// Sub-account Transaction Statistics
/// </summary>
public record BinanceSubAccountTransactionStatistics
{
    /// <summary>
    /// Recent 30-Days Btc Total
    /// </summary>
    [JsonProperty("recent30BtcTotal")]
    public decimal Recent30DaysBtcTotal { get; set; }

    /// <summary>
    /// Recent 30-Days Btc Futures Total
    /// </summary>
    [JsonProperty("recent30BtcFuturesTotal")]
    public decimal Recent30DaysBtcFuturesTotal { get; set; }

    /// <summary>
    /// Recent 30-Days Btc Margin Total
    /// </summary>
    [JsonProperty("recent30BtcMarginTotal")]
    public decimal Recent30DaysBtcMarginTotal { get; set; }

    /// <summary>
    /// Recent 30-Days BUSD Total
    /// </summary>
    [JsonProperty("recent30BusdTotal")]
    public decimal Recent30DaysBusdTotal { get; set; }

    /// <summary>
    /// Recent 30-Days BUSD Futures Total
    /// </summary>
    [JsonProperty("recent30BusdFuturesTotal")]
    public decimal Recent30DaysBusdFuturesTotal { get; set; }

    /// <summary>
    /// Recent 30-Days BUSD Margin Total
    /// </summary>
    [JsonProperty("recent30BusdMarginTotal")]
    public decimal Recent30DaysBusdMarginTotal { get; set; }

    /// <summary>
    /// Trade Info
    /// </summary>
    [JsonProperty("tradeInfoVos")]
    public IEnumerable<BinanceSubAccountTransactionStatisticsTradeInfoVos> TradeInfoVos { get; set; } = [];
}

/// <summary>
/// Sub-account Transaction Statistics Trade Info
/// </summary>
public record BinanceSubAccountTransactionStatisticsTradeInfoVos
{
    /// <summary>
    /// User ID
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// BTC
    /// </summary>
    public decimal Btc { get; set; }

    /// <summary>
    /// BTC Futures
    /// </summary>
    public decimal BtcFutures { get; set; }

    /// <summary>
    /// BTC Margin
    /// </summary>
    public decimal BtcMargin { get; set; }

    /// <summary>
    /// BUSD
    /// </summary>
    public decimal Busd { get; set; }

    /// <summary>
    /// BUSD Futures
    /// </summary>
    public decimal BusdFutures { get; set; }

    /// <summary>
    /// BUSD Margin
    /// </summary>
    public decimal BusdMargin { get; set; }

    /// <summary>
    /// Date
    /// </summary>
    public DateTime Date { get; set; }
}
