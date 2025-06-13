namespace Binance.Api.Link;

/// <summary>
/// IpRestriction Status
/// </summary>
public enum BinanceLinkApiKeyIpRestriction : byte
{
    /// <summary> 
    /// IP Unrestricted 
    /// </summary>
    [Map("1")]
    IpUnrestricted = 1,

    /// <summary> 
    /// Restrict access to trusted IPs only
    /// </summary>
    [Map("2")]
    TrustedIpAddressesOnly = 2,

    /// <summary>
    /// Restrict access to trusted IPs and third-party IPs only
    /// </summary>
    [Map("3")]
    TrustedThirdPartyIpAddressesOnly = 3,
}