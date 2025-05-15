namespace Binance.Api.SubAccount;

internal record BinanceSubAccountContainer
{
    [JsonProperty("subAccounts")]
    public List<BinanceSubAccount> Payload { get; set; } = [];
}