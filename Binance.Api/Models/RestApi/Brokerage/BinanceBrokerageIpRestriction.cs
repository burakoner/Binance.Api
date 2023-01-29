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
/// IP Restriction
/// </summary>
public class BinanceBrokerageIpRestrictionBase
{
    /// <summary>
    /// Sub Account Id
    /// </summary>
    public string SubAccountId { get; set; }

    /// <summary>
    /// Api key
    /// </summary>
    public string ApiKey { get; set; }

    /// <summary>
    /// IP list
    /// </summary>
    public IEnumerable<string> IpList { get; set; } = Array.Empty<string>();

    /// <summary>
    /// Timestamp
    /// </summary>
    [JsonProperty("updateTime"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime UpdateDate { get; set; }
}

/// <summary>
/// IP Restriction
/// </summary>
public class BinanceBrokerageIpRestriction : BinanceBrokerageIpRestrictionBase
{
    /// <summary>
    /// Ip Restrict
    /// </summary>
    public bool IpRestrict { get; set; }
}