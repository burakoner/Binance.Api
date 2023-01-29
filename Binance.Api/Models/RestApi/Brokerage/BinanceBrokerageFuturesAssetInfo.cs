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
/// Futures Asset Info
/// </summary>
public class BinanceBrokerageFuturesAssetInfo
{
    /// <summary>
    /// Data
    /// </summary>
    public IEnumerable<BinanceBrokerageSubAccountFuturesAssetInfo> Data { get; set; } = Array.Empty<BinanceBrokerageSubAccountFuturesAssetInfo>();

    /// <summary>
    /// Timestamp
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime Timestamp { get; set; }
}

/// <summary>
/// Account Futures Asset Info
/// </summary>
public class BinanceBrokerageSubAccountFuturesAssetInfo
{
    /// <summary>
    /// Sub Account Id
    /// </summary>
    public string SubAccountId { get; set; }

    /// <summary>
    /// Futures enable
    /// </summary>
    [JsonProperty("futuresEnable")]
    public bool IsFuturesEnable { get; set; }

    /// <summary>
    /// Total Initial Margin Of Usdt
    /// </summary>
    public decimal TotalInitialMarginOfUsdt { get; set; }

    /// <summary>
    /// Total Maintenance Margin Of Usdt
    /// </summary>
    public decimal TotalMaintenanceMarginOfUsdt { get; set; }

    /// <summary>
    /// Total Wallet Balance Of Usdt
    /// </summary>
    public decimal TotalWalletBalanceOfUsdt { get; set; }

    /// <summary>
    /// Total Unrealized Profit Of Usdt
    /// </summary>
    public decimal TotalUnrealizedProfitOfUsdt { get; set; }

    /// <summary>
    /// Total Margin Balance Of Usdt
    /// </summary>
    public decimal TotalMarginBalanceOfUsdt { get; set; }

    /// <summary>
    /// Total Position Initial Margin Of Usdt
    /// </summary>
    public decimal TotalPositionInitialMarginOfUsdt { get; set; }

    /// <summary>
    /// Total Open Order Initial Margin Of Usdt
    /// </summary>
    public decimal TotalOpenOrderInitialMarginOfUsdt { get; set; }
}