namespace Binance.Api.NFT;

/// <summary>
/// Order type for a NFT order
/// </summary>
public enum BinanceNftOrderType : byte
{
    /// <summary>
    /// Purchase order made by the user to buy assets.
    /// </summary>
    [Map("0")]
    PurchaseOrder = 0,

    /// <summary>
    /// Sell order made by the user to sell assets.
    /// </summary>
    [Map("1")]
    SellOrder = 1,

    /// <summary>
    /// Income received as royalty from assets.
    /// </summary>
    [Map("2")]
    RoyaltyIncome = 2,

    /// <summary>
    /// Order placed during the primary market sale.
    /// </summary>
    [Map("3")]
    PrimaryMarketOrder = 3,

    /// <summary>
    /// Fee paid for minting assets.
    /// </summary>
    [Map("4")]
    MintFee
}
