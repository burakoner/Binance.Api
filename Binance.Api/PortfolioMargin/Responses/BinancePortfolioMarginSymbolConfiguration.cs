namespace Binance.Api.PortfolioMargin;

public record BinancePortfolioMarginSymbolConfiguration
{
    [JsonProperty("symbol")]
    public string Symbol { get; set; }

    [JsonProperty("marginType")]
    public string MarginType { get; set; } // TODO: Enum

    [JsonProperty("isAutoAddMargin")]
    public bool IsAutoAddMargin { get; set; }

    [JsonProperty("leverage")]
    public decimal Leverage { get; set; }

    [JsonProperty("maxNotionalValue")]
    public decimal MaximumNotional { get; set; }
}

public record BinancePortfolioMarginSymbolConfigurationCM : BinancePortfolioMarginSymbolConfiguration
{
}

public record BinancePortfolioMarginSymbolConfigurationUM : BinancePortfolioMarginSymbolConfiguration
{
}