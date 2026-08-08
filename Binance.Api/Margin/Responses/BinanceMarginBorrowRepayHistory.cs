namespace Binance.Api.Margin;

/// <summary>One Margin borrow or repayment record.</summary>
public record BinanceMarginBorrowRepayRecord
{
    /// <summary>How the operation was initiated, as reported by Binance.</summary>
    [JsonProperty("type")]
    public string ActionType { get; set; } = string.Empty;

    /// <summary>Isolated Margin symbol; omitted for Cross Margin.</summary>
    public string? IsolatedSymbol { get; set; }

    /// <summary>Total amount borrowed or repaid.</summary>
    [JsonProperty("amount")]
    public decimal Quantity { get; set; }

    /// <summary>Borrowed or repaid asset.</summary>
    public string Asset { get; set; } = string.Empty;

    /// <summary>Interest included in a repayment.</summary>
    public decimal Interest { get; set; }

    /// <summary>Principal included in a repayment.</summary>
    public decimal Principal { get; set; }

    /// <summary>Execution status.</summary>
    [JsonConverter(typeof(MapConverter))]
    public BinanceMarginStatus Status { get; set; }

    /// <summary>Operation time.</summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime Timestamp { get; set; }

    /// <summary>Transaction identifier.</summary>
    [JsonProperty("txId")]
    public long TransactionId { get; set; }
}

/// <summary>Paginated Margin borrow and repayment history.</summary>
public record BinanceMarginBorrowRepayHistory
{
    /// <summary>Borrow or repayment records.</summary>
    public List<BinanceMarginBorrowRepayRecord> Rows { get; set; } = [];

    /// <summary>Total number of records.</summary>
    public long Total { get; set; }
}
