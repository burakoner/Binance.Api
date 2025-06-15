namespace Binance.Api.C2C;

/// <summary>
/// Binance C2C Order Order History Record
/// </summary>
public record BinanceC2CUserOrderRecord
{
    /// <summary>
    /// Order Number
    /// </summary>
    public long OrderNumber { get; set; }

    /// <summary>
    /// Advertisement Number
    /// </summary>
    [JsonProperty("advNo")]
    public long AdvertisementNumber { get; set; }

    /// <summary>
    /// Side
    /// </summary>
    [JsonProperty("tradeType")]
    public BinanceOrderSide Side { get; set; }

    /// <summary>
    /// Asset Name
    /// </summary>
    public string Asset { get; set; } = "";

    /// <summary>
    /// Fiat Currency
    /// </summary>
    public string Fiat { get; set; } = "";

    /// <summary>
    /// Fiat Symbol
    /// </summary>
    public string FiatSymbol { get; set; } = "";

    /// <summary>
    /// Quantity
    /// </summary>
    [JsonProperty("amount")]
    public decimal Quantity { get; set; }

    /// <summary>
    /// Total Price
    /// </summary>
    [JsonProperty("totalPrice")]
    public decimal TotalPrice { get; set; }

    /// <summary>
    /// Unit Price
    /// </summary>
    [JsonProperty("unitPrice")]
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Order Status
    /// </summary>
    public string OrderStatus { get; set; } = "";

    /// <summary>
    /// Create Time
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// Commission
    /// </summary>
    public decimal Commission { get; set; }

    /// <summary>
    /// Counterpart Nickname
    /// </summary>
    public string CounterPartNickName { get; set; } = "";

    /// <summary>
    /// Advertisement Role
    /// </summary>
    public string AdvertisementRole { get; set; } = "";
}