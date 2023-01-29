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
/// Spot Asset Info
/// </summary>
public class BinanceBrokerageSpotAssetInfo
{
    /// <summary>
    /// Data
    /// </summary>
    public IEnumerable<BinanceBrokerageSubAccountSpotAssetInfo> Data { get; set; } = Array.Empty<BinanceBrokerageSubAccountSpotAssetInfo>();

    /// <summary>
    /// Timestamp
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime Timestamp { get; set; }
}

/// <summary>
/// Account Spot Asset Info
/// </summary>
public class BinanceBrokerageSubAccountSpotAssetInfo
{
    /// <summary>
    /// Sub Account Id
    /// </summary>
    public string SubAccountId { get; set; }

    /// <summary>
    /// Total Balance Of Btc
    /// </summary>
    public decimal TotalBalanceOfBtc { get; set; }
}