namespace Binance.Api.Wallet;

internal class BinanceWalletSnapshotWrapper<T>
{
    public int Code { get; set; }

    [JsonProperty("msg")]
    public string? Message { get; set; }

    [JsonProperty("snapshotVos")]
    public T SnapshotData { get; set; } = default!;
}
