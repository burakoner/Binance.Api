namespace Binance.Api.Models.RestApi.BSwap;

/// <summary>
/// Operation result
/// </summary>
public record BinanceBSwapOperationResult
{
    /// <summary>
    /// Id of the operation
    /// </summary>
    public long OperationId { get; set; }
}
