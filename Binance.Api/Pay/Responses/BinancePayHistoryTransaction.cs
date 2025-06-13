namespace Binance.Api.Pay;

/// <summary>
/// Binance Pay Transaction
/// </summary>
public record BinancePayHistoryTransaction
{
    /// <summary>
    /// Order Type
    /// </summary>
    public string OrderType { get; set; } = "";

    /// <summary>
    /// Transaction Id
    /// </summary>
    public string TransactionId { get; set; } = "";

    /// <summary>
    /// Transaction Time
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime TransactionTime { get; set; }

    /// <summary>
    /// Quantity
    /// </summary>
    [JsonProperty("amount")]
    public decimal Quantity { get; set; }

    /// <summary>
    /// Currency
    /// </summary>
    public string Currency { get; set; } = "";

    /// <summary>
    /// Main Wallet Type
    /// </summary>
    public BinancePayWalletType WalletType { get; set; }

    /// <summary>
    /// Wallet Types
    /// </summary>
    public List<BinancePayWalletType> WalletTypes { get; set; } = [];

    /// <summary>
    /// Details of the funds used in the transaction
    /// </summary>
    [JsonProperty("fundsDetail")]
    public List<BinancePayTransactionFundDetails> FundsDetails { get; set; } = [];

    /// <summary>
    /// Payer Information
    /// </summary>
    [JsonProperty("payerInfo")]
    public BinancePayTransactionPayerInfo payerInfo { get; set; } = default!;

    /// <summary>
    /// Receiver Information
    /// </summary>
    [JsonProperty("receiverInfo")]
    public BinancePayTransactionReceiverInfo receiverInfo { get; set; } = default!;
}

/// <summary>
/// Binance Pay Transaction Fund Details
/// </summary>
public record BinancePayTransactionFundDetails
{
    /// <summary>
    /// Currency
    /// </summary>
    public string Currency { get; set; } = "";

    /// <summary>
    /// Quantity
    /// </summary>
    [JsonProperty("amount")]
    public decimal Quantity { get; set; }

    /// <summary>
    /// WalletAssetCost
    /// </summary>
    [JsonProperty("walletAssetCost")]
    public Dictionary<BinancePayWalletType, decimal> WalletAssetCosts { get; set; } = [];
}

/// <summary>
/// Binance Pay Transaction Payer Information
/// </summary>
public record BinancePayTransactionPayerInfo
{
    /// <summary>
    /// Name
    /// </summary>
    public string Name { get; set; } = "";

    /// <summary>
    /// Type
    /// </summary>
    public string Type { get; set; } = "";

    /// <summary>
    /// Binance Id
    /// </summary>
    public long BinanceId { get; set; }

    /// <summary>
    /// Account Id
    /// </summary>
    public long AccountId { get; set; }
}

/// <summary>
/// Binance Pay Transaction Receiver Information
/// </summary>
public record BinancePayTransactionReceiverInfo
{
    /// <summary>
    /// Name
    /// </summary>
    public string Name { get; set; } = "";

    /// <summary>
    /// Type
    /// </summary>
    public string Type { get; set; } = "";

    /// <summary>
    /// Email
    /// </summary>
    public string Email { get; set; } = "";

    /// <summary>
    /// Binance Id
    /// </summary>
    public long BinanceId { get; set; }

    /// <summary>
    /// Account Id
    /// </summary>
    public long AccountId { get; set; }

    /// <summary>
    /// Country Code
    /// </summary>
    public string CountryCode { get; set; } = "";

    /// <summary>
    /// Mobile Code
    /// </summary>
    public string MobileCode { get; set; } = "";

    /// <summary>
    /// Phone Number
    /// </summary>
    public string PhoneNumber { get; set; } = "";

    /// <summary>
    /// Extended Receiver Information
    /// </summary>
    public BinancePayTransactionReceiverInfoExtended? Extend { get; set; }
}

/// <summary>
/// Binance Pay Transaction Receiver Information Extended
/// </summary>
public record BinancePayTransactionReceiverInfoExtended
{
    /// <summary>
    /// Institution Name
    /// </summary>
    public string InstitutionName { get; set; } = "";

    /// <summary>
    /// Card Number
    /// </summary>
    public string CardNumber { get; set; } = "";

    /// <summary>
    /// Digital Wallet Id
    /// </summary>
    public string DigitalWalletId { get; set; } = "";
}
