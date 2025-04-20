namespace Binance.Api.Models.RestApi.BSwap;

/// <summary>
/// Swap pool info
/// </summary>
public record BinanceBSwapPool
{
    /// <summary>
    /// Id
    /// </summary>
    public int PoolId { get; set; }
    /// <summary>
    /// Name
    /// </summary>
    public string PoolName { get; set; } = "";
    /// <summary>
    /// Assets in the pool
    /// </summary>
    public IEnumerable<string> Assets { get; set; } = [];
}
