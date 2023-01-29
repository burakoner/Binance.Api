﻿namespace Binance.Api.Enums;

/// <summary>
/// Borrow status
/// </summary>
public enum BorrowStatus
{
    /// <summary>
    /// Accruing interest
    /// </summary>
    [Map("Accuring_Interest")]
    AccruingInterest,
    /// <summary>
    /// Overdue
    /// </summary>
    [Map("Overdue")]
    Overdeu,
    /// <summary>
    /// Currently liquidating
    /// </summary>
    [Map("Liquidating")]
    Liquidating,
    /// <summary>
    /// Repaying
    /// </summary>
    [Map("Repaying")]
    Repaying,
    /// <summary>
    /// Repaid
    /// </summary>
    [Map("Repaid")]
    Repaid,
    /// <summary>
    /// Liquidated
    /// </summary>
    [Map("Liquidated")]
    Liquidated,
    /// <summary>
    /// Pending
    /// </summary>
    [Map("Pending")]
    Pending,
    /// <summary>
    /// Failed
    /// </summary>
    [Map("Failed")]
    Failed
}
