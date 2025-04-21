namespace Binance.Api.Wallet;

/// <summary>
/// Binance Wallet Rest API Client
/// </summary>
public class BinanceWalletRestApi
{
    // Parent
    internal BinanceRestApiClient _ { get; }

    /// <summary>
    /// Capital Client
    /// <para><a href="https://developers.binance.com/docs/wallet/capital" /></para>
    /// </summary>
    public BinanceWalletRestApiCapital Capital { get; set; }

    /// <summary>
    /// Asset Client
    /// <para><a href="https://developers.binance.com/docs/wallet/asset" /></para>
    /// </summary>
    public BinanceWalletRestApiAsset Asset { get; set; }

    /// <summary>
    /// Account Client
    /// <para><a href="https://developers.binance.com/docs/wallet/account" /></para>
    /// </summary>
    public BinanceWalletRestApiAccount Account { get; set; }

    /// <summary>
    /// Travel Client
    /// <para><a href="https://developers.binance.com/docs/wallet/travel-rule" /></para>
    /// </summary>
    public BinanceWalletRestApiTravel Travel { get; set; }

    /// <summary>
    /// Others Client
    /// <para><a href="https://developers.binance.com/docs/wallet/others" /></para>
    /// </summary>
    public BinanceWalletRestApiOthers Others { get; set; }

    /// <summary>
    /// Binance Spot Rest API Client
    /// </summary>
    /// <param name="root">Parent</param>
    internal BinanceWalletRestApi(BinanceRestApiClient root)
    {
        _ = root;

        Capital = new BinanceWalletRestApiCapital(this);
        Asset = new BinanceWalletRestApiAsset(this);
        Account = new BinanceWalletRestApiAccount(this);
        Travel = new BinanceWalletRestApiTravel(this);
        Others = new BinanceWalletRestApiOthers(this);
    }

}