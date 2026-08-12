namespace Binance.FIX.Api;

/// <summary>
/// Controls the account execution reports delivered to an Order Entry or Drop Copy session.
/// </summary>
public enum BinanceFixResponseMode
{
    /// <summary>
    /// Receive all account execution reports and list statuses. This is the wrapper default.
    /// </summary>
    Everything = 1,

    /// <summary>
    /// Receive acknowledgements only and disable execution-report push.
    /// </summary>
    OnlyAcknowledgements = 2
}
