namespace Binance.Api.SubAccount;

internal record BinanceSubAccountUniversalTransfersContainer
{
    /// <summary>
    /// Transactions
    /// </summary>
    [JsonProperty("result")]
    public IEnumerable<BinanceSubAccountUniversalTransfer> Payload { get; set; } = [];
}