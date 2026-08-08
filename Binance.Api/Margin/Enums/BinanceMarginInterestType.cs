namespace Binance.Api.Margin;

/// <summary>Reason Margin interest was charged.</summary>
public enum BinanceMarginInterestType : byte
{
    /// <summary>Hourly interest charge.</summary>
    [Map("PERIODIC")]
    Periodic = 1,

    /// <summary>Initial interest charged when borrowing.</summary>
    [Map("ON_BORROW")]
    OnBorrow = 2,

    /// <summary>Hourly interest converted into BNB.</summary>
    [Map("PERIODIC_CONVERTED")]
    PeriodicConverted = 3,

    /// <summary>Initial borrowing interest converted into BNB.</summary>
    [Map("ON_BORROW_CONVERTED")]
    OnBorrowConverted = 4,

    /// <summary>Daily interest on a Portfolio Margin negative balance.</summary>
    [Map("PORTFOLIO")]
    Portfolio = 5
}
