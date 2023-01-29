namespace Binance.ApiClient.Models.RestApi.FuturesAlgoOrders;

/// <summary>
/// Algo order result
/// </summary>
public class BinanceAlgoOrderResult : BinanceResult
{
    /// <summary>
    /// Order id
    /// </summary>
    public string ClientAlgoId { get; set; }
    /// <summary>
    /// Successful
    /// </summary>
    public bool Success { get; set; }
}
