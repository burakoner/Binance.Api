﻿namespace Binance.Api.Enums;

/// <summary>
/// Type of account
/// </summary>
public enum AccountType
{
    /// <summary>
    /// Spot account type
    /// </summary>
    [Map("SPOT")]
    Spot,
    /// <summary>
    /// Margin account type
    /// </summary>>
    [Map("MARGIN")]
    Margin,
    /// <summary>
    /// Futures account type
    /// </summary>
    [Map("FUTURES")]
    Futures,
    /// <summary>
    /// Leveraged account type
    /// </summary>
    [Map("LEVERAGED")]
    Leveraged,
    /// <summary>
    /// Trade group 2
    /// </summary>
    [Map("TRD_GRP_002")]
    TradeGroup002,
    /// <summary>
    /// Trade group 3
    /// </summary>
    [Map("TRD_GRP_003")]
    TradeGroup003,
    /// <summary>
    /// Trade group 4
    /// </summary>
    [Map("TRD_GRP_004")]
    TradeGroup004,
    /// <summary>
    /// Trade group 5
    /// </summary>
    [Map("TRD_GRP_005")]
    TradeGroup005,
    /// <summary>
    /// Trade group 6
    /// </summary>
    [Map("TRD_GRP_006")]
    TradeGroup006,
    /// <summary>
    /// Trade group 7
    /// </summary>
    [Map("TRD_GRP_007")]
    TradeGroup007,
    /// <summary>
    /// Trade group 8
    /// </summary>
    [Map("TRD_GRP_008")]
    TradeGroup008,
    /// <summary>
    /// Trade group 9
    /// </summary>
    [Map("TRD_GRP_009")]
    TradeGroup009
}
