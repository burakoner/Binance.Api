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
/// Margin Asset Info
/// </summary>
public class BinanceBrokerageMarginAssetInfo
{
    /// <summary>
    /// Data
    /// </summary>
    public IEnumerable<BinanceBrokerageSubAccountMarginAssetInfo> Data { get; set; } = Array.Empty<BinanceBrokerageSubAccountMarginAssetInfo>();

    /// <summary>
    /// Timestamp
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime Timestamp { get; set; }
}

/// <summary>
/// Account Margin Asset Info
/// </summary>
public class BinanceBrokerageSubAccountMarginAssetInfo
{
    /// <summary>
    /// Sub Account Id
    /// </summary>
    public string SubAccountId { get; set; }

    /// <summary>
    /// Margin enable
    /// </summary>
    [JsonProperty("marginEnable")]
    public bool IsMarginEnable { get; set; }

    /// <summary>
    /// Total Asset Of Btc
    /// </summary>
    public decimal TotalAssetOfBtc { get; set; }

    /// <summary>
    /// Total Liability Of Btc
    /// </summary>
    public decimal TotalLiabilityOfBtc { get; set; }

    /// <summary>
    /// Total Net Asset Of Btc
    /// </summary>
    public decimal TotalNetAssetOfBtc { get; set; }

    /// <summary>
    /// Margin level
    /// </summary>
    public decimal MarginLevel { get; set; }
}