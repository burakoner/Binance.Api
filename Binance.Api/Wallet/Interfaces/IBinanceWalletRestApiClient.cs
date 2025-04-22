namespace Binance.Api.Wallet;

/// <summary>
/// Interface for the Binance Wallet REST API Client
/// </summary>
public interface IBinanceWalletRestApiClient :
    IBinanceWalletRestApiClientCapital,
    IBinanceWalletRestApiClientAsset,
    IBinanceWalletRestApiClientAccount,
    IBinanceWalletRestApiClientTravelRule,
    IBinanceWalletRestApiClientOthers;