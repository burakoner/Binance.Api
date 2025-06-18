namespace Binance.Api.PortfolioMargin;

public  record BinancePortfolioMarginCrossTrade
{
    [JsonProperty("symbol")]
    public string Symbol { get; set; }

    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("orderId")]
    public long OrderId { get; set; }

    [JsonProperty("price")]
    public decimal Price { get; set; }

    [JsonProperty("qty")]
    public string Quantity { get; set; }

    [JsonProperty("commission")]
    public decimal Commission { get; set; }

    [JsonProperty("commissionAsset")]
    public string CommissionAsset { get; set; }

    [JsonProperty("time")]
    public DateTime Time { get; set; }

    [JsonProperty("isBestMatch")]
    public bool IsBestMatch { get; set; }

    [JsonProperty("isBuyer")]
    public bool IsBuyer { get; set; }

    [JsonProperty("isMaker")]
    public bool IsMaker { get; set; }
}