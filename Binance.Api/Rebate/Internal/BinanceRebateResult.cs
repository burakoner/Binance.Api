namespace Binance.Api.Rebate;

/// <summary>
/// Binance Rebate Result
/// </summary>
/// <typeparam name="T"></typeparam>
internal record BinanceRebateResult<T>
{
    /// <summary>
    /// Status
    /// </summary>
    public string Status { get; set; } = "";

    /// <summary>
    /// Type
    /// </summary>
    public string Type { get; set; } = "";

    /// <summary>
    /// Code
    /// </summary>
    public string Code { get; set; } = "";

    /// <summary>
    /// Data
    /// </summary>
    public T Data { get; set; }= default!;
}
