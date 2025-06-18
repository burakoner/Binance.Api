namespace Binance.Api.PortfolioMargin;

public record BinancePortfolioMarginAmendment
{
    [JsonProperty("amendmentId")]
    public long AmendmentId { get; set; }

    [JsonProperty("symbol")]
    public string Symbol { get; set; }

    [JsonProperty("pair")]
    public string Pair { get; set; }

    [JsonProperty("orderId")]
    public long OrderId { get; set; }

    [JsonProperty("clientOrderId")]
    public string ClientOrderId { get; set; }

    [JsonProperty("time")]
    public long Time { get; set; }

    [JsonProperty("amendment")]
    public BinancePortfolioMarginAmendmentData Amendment { get; set; }

    [JsonProperty("priceMatch")]
    public string PriceMatch { get; set; }
}

public record BinancePortfolioMarginAmendmentData
{
    [JsonProperty("price")]
    public BinancePortfolioMarginAmendmentDataPrice Price { get; set; }

    [JsonProperty("origQty")]
    public BinancePortfolioMarginAmendmentDataQuantity OrigQty { get; set; }

    [JsonProperty("count")]
    public int Count { get; set; }
}

public record BinancePortfolioMarginAmendmentDataPrice
{
    [JsonProperty("before")]
    public decimal? Before { get; set; }

    [JsonProperty("after")]
    public decimal? After { get; set; }
}

public record BinancePortfolioMarginAmendmentDataQuantity
{
    [JsonProperty("before")]
    public decimal Before { get; set; }

    [JsonProperty("after")]
    public decimal After { get; set; }
}

public  record BinancePortfolioMarginAmendmentCM: BinancePortfolioMarginAmendment
{
}

public record BinancePortfolioMarginAmendmentUM : BinancePortfolioMarginAmendment
{
}