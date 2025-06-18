namespace Binance.Api.PortfolioMargin;

public  record BinancePortfolioMarginCrossInterestRecord
{
    [JsonProperty("txId")]
    public long TransactionId { get; set; }

    [JsonProperty("interestAccuredTime")]
    public DateTime InterestAccuredTime { get; set; }

    [JsonProperty("asset")]
    public string Asset { get; set; } = "";

    [JsonProperty("rawAsset")]
    public string RawAsset { get; set; } = "";

    [JsonProperty("principal")]
    public decimal Principal { get; set; }

    [JsonProperty("interest")]
    public decimal Interest { get; set; }

    [JsonProperty("interestRate")]
    public decimal InterestRate { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; } = ""; // TODO: Enum
}