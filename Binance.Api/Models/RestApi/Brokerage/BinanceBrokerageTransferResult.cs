namespace Binance.ApiClient.Models.RestApi.Brokerage;

/// <summary>
/// Transfer Result
/// </summary>
public class BinanceBrokerageTransferResult
{
    /// <summary>
    /// Transaction Id
    /// </summary>
    [JsonProperty("txnId")]
    public string Id { get; set; }

    /// <summary>
    /// Client Transfer Id
    /// </summary>
    [JsonProperty("clientTranId")]
    public string ClientTransferId { get; set; }
}