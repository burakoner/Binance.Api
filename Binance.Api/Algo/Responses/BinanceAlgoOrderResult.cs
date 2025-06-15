namespace Binance.Api.Algo;

/// <summary>
/// Algo order result
/// </summary>
public record BinanceAlgoOrderResult: BinanceResponse
{
    /// <summary>
    /// Order id
    /// </summary>
    public string ClientAlgoId { get; set; } = string.Empty;

    /// <summary>
    /// The order id as assigned by the client without the prefix
    /// </summary>
    public string RequestClientAlgoId => ClientAlgoId
        .TrimStart(BinanceConstants.ClientOrderIdPrefixSpot.ToCharArray())
        .TrimStart(BinanceConstants.ClientOrderIdPrefixFutures.ToCharArray());

    /// <summary>
    /// Successful
    /// </summary>
    public bool Success { get; set; }
}
