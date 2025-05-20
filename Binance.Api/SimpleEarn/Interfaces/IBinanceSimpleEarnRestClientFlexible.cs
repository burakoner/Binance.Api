namespace Binance.Api.SimpleEarn;

/// <summary>
/// Interface for the Binance Flexible Simple Earn Rest API client.
/// </summary>
public interface IBinanceSimpleEarnRestClientFlexible :
    IBinanceSimpleEarnRestClientFlexibleAccount,
    IBinanceSimpleEarnRestClientFlexibleEarn,
    IBinanceSimpleEarnRestClientFlexibleHistory;