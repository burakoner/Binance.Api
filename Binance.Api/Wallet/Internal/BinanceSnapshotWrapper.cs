namespace Binance.Api.Wallet.Internal;

internal class BinanceSnapshotWrapper<T>
{
    public int Code { get; set; }

    [JsonProperty("msg")]
    public string? Message { get; set; }

    [JsonProperty("snapshotVos")]
    public T SnapshotData { get; set; } = default!;
}
