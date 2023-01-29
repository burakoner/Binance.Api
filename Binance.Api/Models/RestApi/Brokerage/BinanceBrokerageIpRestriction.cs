﻿namespace Binance.Api.Models.RestApi.Brokerage;

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