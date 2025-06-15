using Binance.Api.Options;

namespace Binance.Api.DualInvestment;

/// <summary>
/// Binance Dual Investment Subscription
/// </summary>
public record BinanceDualInvestmentSubscription
{
    /// <summary>
    /// Position Id
    /// </summary>
    [JsonProperty("positionId")]
    public long PositionId { get; set; }

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
    /// Subscription Quantity
    /// </summary>
    [JsonProperty("subscriptionAmount")]
    public decimal SubscriptionQuantity { get; set; }

    /// <summary>
    /// Duration in seconds
    /// </summary>
    [JsonProperty("duration")]
    public int Duration { get; set; }

    /// <summary>
    /// Auto Compound Plan
    /// </summary>
    [JsonProperty("autoCompoundPlan")]
    public BinanceDualInvestmentAutoCompoundPlan AutoCompoundPlan { get; set; }

    /// <summary>
    /// Strike Price
    /// </summary>
    [JsonProperty("strikePrice")]
    public decimal StrikePrice { get; set; }

    /// <summary>
    /// Settle Date
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime SettleDate { get; set; }

    /// <summary>
    /// Purchase Status
    /// </summary>
    [JsonProperty("purchaseStatus")]
    public BinanceDualInvestmentStatus Status { get; set; }

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
    /// Purchase Time
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime PurchaseTime { get; set; }

    /// <summary>
    /// Options Side
    /// </summary>
    [JsonProperty("optionType")]
    public BinanceOptionsSide OptionsSide { get; set; }
}