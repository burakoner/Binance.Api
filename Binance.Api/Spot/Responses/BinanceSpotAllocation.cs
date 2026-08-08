namespace Binance.Api.Spot;

/// <summary>
/// Allocation resulting from Smart Order Routing.
/// </summary>
public record BinanceSpotAllocation
{
    /// <summary>The symbol.</summary>
    public string Symbol { get; set; } = string.Empty;

    /// <summary>The allocation identifier.</summary>
    public long AllocationId { get; set; }

    /// <summary>The allocation type.</summary>
    public string AllocationType { get; set; } = string.Empty;

    /// <summary>The order identifier.</summary>
    public long OrderId { get; set; }

    /// <summary>The order-list identifier.</summary>
    public long OrderListId { get; set; }

    /// <summary>The allocation price.</summary>
    public decimal Price { get; set; }

    /// <summary>The base-asset quantity.</summary>
    [JsonProperty("qty")]
    public decimal Quantity { get; set; }

    /// <summary>The quote-asset quantity.</summary>
    [JsonProperty("quoteQty")]
    public decimal QuoteQuantity { get; set; }

    /// <summary>The commission charged.</summary>
    public decimal Commission { get; set; }

    /// <summary>The commission asset.</summary>
    public string CommissionAsset { get; set; } = string.Empty;

    /// <summary>The allocation time.</summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime Time { get; set; }

    /// <summary>Whether the account was the buyer.</summary>
    public bool IsBuyer { get; set; }

    /// <summary>Whether the account was the maker.</summary>
    public bool IsMaker { get; set; }

    /// <summary>Whether the account was the allocator.</summary>
    public bool IsAllocator { get; set; }
}
