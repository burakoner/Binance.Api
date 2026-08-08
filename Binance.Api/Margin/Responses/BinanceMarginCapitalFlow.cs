namespace Binance.Api.Margin;

/// <summary>Cross or Isolated Margin capital-flow record.</summary>
public record BinanceMarginCapitalFlow
{
    /// <summary>Record identifier used for forward pagination.</summary>
    public long Id { get; set; }

    /// <summary>Transaction identifier.</summary>
    [JsonProperty("tranId")]
    public long TransactionId { get; set; }

    /// <summary>Record time.</summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime Timestamp { get; set; }

    /// <summary>Asset affected by the flow.</summary>
    public string Asset { get; set; } = string.Empty;

    /// <summary>Isolated Margin symbol; omitted for Cross Margin.</summary>
    public string? Symbol { get; set; }

    /// <summary>Capital-flow type.</summary>
    [JsonConverter(typeof(MapConverter))]
    public BinanceMarginCapitalFlowType Type { get; set; }

    /// <summary>Flow amount.</summary>
    public decimal Amount { get; set; }

    /// <summary>Institutional-loan origin, when applicable.</summary>
    [JsonConverter(typeof(MapConverter))]
    public BinanceMarginCapitalFlowNote? Note { get; set; }
}
