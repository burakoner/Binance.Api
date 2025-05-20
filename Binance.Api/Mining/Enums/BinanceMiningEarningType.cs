namespace Binance.Api.Mining;

/// <summary>
/// Mining earnings type
/// </summary>
[JsonConverter(typeof(MapConverter))]
public enum BinanceMiningEarningType : byte
{
    /// <summary>
    /// Mining wallet
    /// </summary>
    [Map("0")]
    MiningWallet = 0,

    /// <summary>
    /// Merged mining
    /// </summary>
    [Map("1")]
    MergedMining = 1,

    /// <summary>
    /// Activity bonus
    /// </summary>
    [Map("2")]
    ActivityBonus = 2,

    /// <summary>
    /// Rebate
    /// </summary>
    [Map("3")]
    Rebate = 3,

    /// <summary>
    /// Smart pool
    /// </summary>
    [Map("4")]
    SmartPool = 4,

    /// <summary>
    /// Mining address
    /// </summary>
    [Map("5")]
    MiningAddress = 5,

    /// <summary>
    /// Pool savings
    /// </summary>
    [Map("7")]
    PoolSavings = 7,

    /// <summary>
    /// Transferred
    /// </summary>
    [Map("8")]
    Transferred = 8,

    /// <summary>
    /// Income transfer
    /// </summary>
    [Map("6", "31")]
    IncomeTransfer = 31,

    /// <summary>
    /// Hashrate resale - mining wallet
    /// </summary>
    [Map("32")]
    HashrateResaleMiningWallet = 32,

    /// <summary>
    /// Hashrate resale - pool savings
    /// </summary>
    [Map("33")]
    HashrateResalePoolSavings = 33
}
