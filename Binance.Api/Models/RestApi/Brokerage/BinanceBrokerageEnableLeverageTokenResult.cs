namespace Binance.ApiClient.Models.RestApi.Brokerage;

/// <summary>
/// Enable Leverage Token Result
/// </summary>
public class BinanceBrokerageEnableLeverageTokenResult
{
    /// <summary>
    /// Sub Account Id
    /// </summary>
    public string SubAccountId { get; set; }

    /// <summary>
    /// Is Leverage Token Enabled
    /// </summary>
    [JsonProperty("enableBlvt")]
    public bool IsLeverageTokenEnabled { get; set; }
}