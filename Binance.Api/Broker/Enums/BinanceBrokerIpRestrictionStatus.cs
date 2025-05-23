﻿namespace Binance.Api.Broker;

/// <summary>
/// Broker IpRestriction Status
/// </summary>
public enum BinanceBrokerIpRestrictionStatus : byte
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
    RestrictAccessToTrustedIpAddressesOnly = 2,
}