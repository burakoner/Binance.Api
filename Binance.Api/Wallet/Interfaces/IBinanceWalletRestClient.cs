namespace Binance.Api.Wallet;

/// <summary>
/// Interface for the Binance Wallet REST API Client
/// </summary>
public interface IBinanceWalletRestClient :
    IBinanceWalletRestClientCapital,
    IBinanceWalletRestClientAsset,
    IBinanceWalletRestClientAccount,
    IBinanceWalletRestClientTravelRule,
    IBinanceWalletRestClientOthers;