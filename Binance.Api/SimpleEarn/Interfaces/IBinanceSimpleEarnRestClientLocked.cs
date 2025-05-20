namespace Binance.Api.SimpleEarn;

/// <summary>
/// Interface for the Binance Locked Simple Earn Rest API client.
/// </summary>
public interface IBinanceSimpleEarnRestClientLocked :
    IBinanceSimpleEarnRestClientLockedAccount,
    IBinanceSimpleEarnRestClientLockedEarn,
    IBinanceSimpleEarnRestClientLockedHistory;