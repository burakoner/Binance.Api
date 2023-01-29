/* Unmerged change from project 'Binance.ApiClient (netstandard2.1)'
Before:
namespace Binance.ApiClient.Models.RestApi.Brokerage.SubAccountData;
After:
using Binance;
using Binance.ApiClient;
using Binance.ApiClient.Models;
using Binance.ApiClient.Models.RestApi;
using Binance.ApiClient.Models.RestApi.Brokerage;
using Binance.ApiClient.Models.RestApi.Brokerage;
using Binance.ApiClient.Models.RestApi.Brokerage.SubAccountData;
*/

namespace Binance.ApiClient.Models.RestApi.Brokerage;

/// <summary>
/// Transfer Futures Transactions
/// </summary>
public class BinanceBrokerageTransferFuturesTransactions
{
    /// <summary>
    /// Success
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Futures type
    /// </summary>
    public BinanceBrokerageFuturesType FuturesType { get; set; }

    /// <summary>
    /// Transfer
    /// </summary>
    [JsonProperty("transfer")]
    public IEnumerable<BinanceBrokerageTransferFuturesTransaction> Transactions { get; set; } = Array.Empty<BinanceBrokerageTransferFuturesTransaction>();
}

/// <summary>
/// Transfer Futures Transaction
/// </summary>
public class BinanceBrokerageTransferFuturesTransaction
{
    /// <summary>
    /// From Id
    /// </summary>
    public string FromId { get; set; }

    /// <summary>
    /// To Id
    /// </summary>
    public string ToId { get; set; }

    /// <summary>
    /// Asset
    /// </summary>
    public string Asset { get; set; }

    /// <summary>
    /// Quantity
    /// </summary>
    [JsonProperty("qty")]
    public decimal Quantity { get; set; }

    /// <summary>
    /// Transaction Id
    /// </summary>
    [JsonProperty("tranId")]
    public string Id { get; set; }

    /// <summary>
    /// Client Transfer Id
    /// </summary>
    [JsonProperty("clientTranId")]
    public string ClientTransferId { get; set; }

    /// <summary>
    /// Date
    /// </summary>
    [JsonProperty("time"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime Date { get; set; }
}