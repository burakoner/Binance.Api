namespace Binance.Api.PortfolioMargin;

public  record BinancePortfolioMarginAccountInformation
{
    [JsonProperty("uniMMR")]
    public decimal UniMMR { get; set; }

    [JsonProperty("accountEquity")]
    public decimal AccountEquity { get; set; }

    [JsonProperty("actualEquity")]
    public decimal ActualEquity { get; set; }

    [JsonProperty("accountInitialMargin")]
    public decimal AccountInitialMargin { get; set; }

    [JsonProperty("accountMaintMargin")]
    public decimal AccountMaintenanceMargin { get; set; }

    [JsonProperty("accountStatus")]
    public string AccountStatus { get; set; } // TODO: Enum

    [JsonProperty("virtualMaxWithdrawAmount")]
    public decimal? VirtualMaxWithdrawAmount { get; set; }

    [JsonProperty("totalAvailableBalance")]
    public decimal? TotalAvailableBalance { get; set; }

    [JsonProperty("totalMarginOpenLoss")]
    public decimal? TotalMarginOpenLoss { get; set; }

    [JsonProperty("updateTime")]
    public DateTime? UpdateTime { get; set; }
}