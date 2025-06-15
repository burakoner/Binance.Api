using Binance.Api.Options;

namespace Binance.Api.DualInvestment;

/// <summary>
/// Binance Dual Investment Product
/// </summary>
public record BinanceDualInvestmentProduct
{
    /// <summary>
    /// Id
    /// </summary>
    [JsonProperty("id")]
    public long Id { get; set; }

    /// <summary>
    /// Invest Coin
    /// </summary>
    [JsonProperty("investCoin")]
    public string InvestCoin { get; set; } = "";

    /// <summary>
    /// Exercised Coin
    /// </summary>
    [JsonProperty("exercisedCoin")]
    public string ExercisedCoin { get; set; } = "";

    /// <summary>
    /// Strike Price
    /// </summary>
    [JsonProperty("strikePrice")]
    public decimal StrikePrice { get; set; }

    /// <summary>
    /// Duration in seconds
    /// </summary>
    [JsonProperty("duration")]
    public int Duration { get; set; }

    /// <summary>
    /// Settle Date
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime SettleDate { get; set; }

    /// <summary>
    /// Purchase Decimal Places
    /// </summary>
    [JsonProperty("purchaseDecimal")]
    public int PurchaseDecimal { get; set; }

    /// <summary>
    /// Purchase End Time
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime PurchaseEndTime { get; set; }

    /// <summary>
    /// Can Purchase
    /// </summary>
    [JsonProperty("canPurchase")]
    public bool CanPurchase { get; set; } 

    /// <summary>
    /// The annual percentage rate (APR) for the financial product.
    /// </summary>
    [JsonProperty("apr")]
    public decimal APR { get; set; }

    /// <summary>
    /// Order Id
    /// </summary>
    [JsonProperty("orderId")]
    public long OrderId { get; set; }

    /// <summary>
    /// Minimum Quantity
    /// </summary>
    [JsonProperty("minAmount")]
    public decimal MinimumQuantity { get; set; }

    /// <summary>
    /// Maximum Quantity
    /// </summary>
    [JsonProperty("maxAmount")]
    public decimal MaximumQuantity { get; set; }

    /// <summary>
    /// Create Time
    /// </summary>
    [JsonProperty("createTimestamp")]
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// Options Side
    /// </summary>
    [JsonProperty("optionType")]
    public BinanceOptionsSide OptionsSide { get; set; }

    /// <summary>
    /// Is Auto Compound Enable
    /// </summary>
    [JsonProperty("isAutoCompoundEnable")]
    public bool IsAutoCompoundEnable { get; set; }

    /// <summary>
    /// Auto Compound Plan List
    /// </summary>
    [JsonProperty("autoCompoundPlanList")]
    public List<string> AutoCompoundPlanList { get; set; } = [];
}