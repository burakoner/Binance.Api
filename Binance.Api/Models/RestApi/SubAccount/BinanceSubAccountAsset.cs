using Binance.Api.Wallet;

namespace Binance.Api.Models.RestApi.SubAccount;

internal class BinanceSubAccountAsset
{
    public bool Success { get; set; } = true;
    [JsonProperty("msg")]
    public string? Message { get; set; }
    public IEnumerable<BinanceBalance> Balances { get; set; } = [];
}
