namespace Binance.Api.PortfolioMargin;

public  record BinancePortfolioMarginCrossRepay
{
    [JsonProperty("amount")]
    public decimal Quantity { get; set; }

    [JsonProperty("asset")]
    public string Asset { get; set; } = "";

    [JsonProperty("specifyRepayAssets")]
    public List<string> RepayAssets { get; set; } = [];

    [JsonProperty("updateTime")]
    public DateTime? UpdateTime { get; set; }

    [JsonProperty("success")]
    public bool Success { get; set; }
}