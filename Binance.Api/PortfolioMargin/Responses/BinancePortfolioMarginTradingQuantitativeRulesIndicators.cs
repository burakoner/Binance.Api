namespace Binance.Api.PortfolioMargin;

public record BinancePortfolioMarginTradingQuantitativeRulesIndicators
{
    [JsonProperty("indicators")]
    public Dictionary<string, BinancePortfolioMarginTradingQuantitativeRulesIndicator> Indicators { get; set; }

    [JsonProperty("updateTime")]
    public DateTime UpdateTime { get; set; }
}

public record BinancePortfolioMarginTradingQuantitativeRulesIndicator
{
    [JsonProperty("isLocked")]
    public bool IsLocked { get; set; }

    [JsonProperty("plannedRecoverTime")]
    public DateTime PlannedRecoverTime { get; set; }

    [JsonProperty("indicator")]
    public string Indicator { get; set; } = "";

    [JsonProperty("value")]
    public decimal Value { get; set; }

    [JsonProperty("triggerValue")]
    public decimal TriggerValue { get; set; }
}

public record BinancePortfolioMarginTradingQuantitativeRulesIndicatorsCM : BinancePortfolioMarginTradingQuantitativeRulesIndicators
{
}

public record BinancePortfolioMarginTradingQuantitativeRulesIndicatorsUM : BinancePortfolioMarginTradingQuantitativeRulesIndicators
{
}