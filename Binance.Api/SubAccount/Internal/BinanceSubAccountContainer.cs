namespace Binance.Api.SubAccount;

internal record BinanceSubAccountContainer
{
    [JsonProperty("subAccounts")]
    public IEnumerable<BinanceSubAccount> Payload { get; set; } = [];
}