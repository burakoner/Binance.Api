namespace Binance.FIX.Api;

/// <summary>
/// One fee entry from an ExecutionReport NoMiscFees repeating group.
/// </summary>
public sealed class BinanceFixMiscFee
{
    internal BinanceFixMiscFee(decimal amount, string currency, BinanceFixMiscFeeType type)
    {
        Amount = amount;
        Currency = currency;
        Type = type;
    }

    /// <summary>Gets the fee amount.</summary>
    public decimal Amount { get; }

    /// <summary>Gets the fee asset.</summary>
    public string Currency { get; }

    /// <summary>Gets the fee type.</summary>
    public BinanceFixMiscFeeType Type { get; }
}
