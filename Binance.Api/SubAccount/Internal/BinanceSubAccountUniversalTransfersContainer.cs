namespace Binance.Api.SubAccount;

internal record BinanceSubAccountUniversalTransfersContainer
{
    /// <summary>
    /// Transactions
    /// </summary>
    [JsonProperty("result")]
    public List<BinanceSubAccountUniversalTransfer> Payload { get; set; } = [];
}