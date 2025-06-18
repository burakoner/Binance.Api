namespace Binance.Api.PortfolioMargin;

public record BinancePortfolioMarginLeverageBracket
{
    [JsonProperty("symbol")]
    public string Symbol { get; set; }
}

public record BinancePortfolioMarginLeverageBracketItem
{
    [JsonProperty("bracket")]
    public decimal Bracket { get; set; }

    [JsonProperty("initialLeverage")]
    public decimal InitialLeverage { get; set; }

    [JsonProperty("maintMarginRatio")]
    public decimal MaintenanceMarginRatio { get; set; }

    [JsonProperty("cum")]
    public decimal Cumulated { get; set; }
}

public record BinancePortfolioMarginLeverageBracketItemCM : BinancePortfolioMarginLeverageBracketItem
{
    [JsonProperty("qtyCap")]
    public decimal QuantityCap { get; set; }

    [JsonProperty("qtyCap")]
    public decimal QuantityFloor { get; set; }
}

public record BinancePortfolioMarginLeverageBracketItemUM : BinancePortfolioMarginLeverageBracketItem
{
    [JsonProperty("notionalCap")]
    public decimal NotionalCap { get; set; }

    [JsonProperty("notionalFloor")]
    public decimal NotionalFloor { get; set; }
}

public record BinancePortfolioMarginLeverageBracketCM : BinancePortfolioMarginLeverageBracket
{
    [JsonProperty("brackets")]
    public List<BinancePortfolioMarginLeverageBracketItemCM> Brackets { get; set; } = [];
}

public record BinancePortfolioMarginLeverageBracketUM : BinancePortfolioMarginLeverageBracket
{
    [JsonProperty("notionalCoef")]
    public decimal NotionalCoefficient { get; set; }

    [JsonProperty("brackets")]
    public List<BinancePortfolioMarginLeverageBracketItemUM> Brackets { get; set; } = [];
}