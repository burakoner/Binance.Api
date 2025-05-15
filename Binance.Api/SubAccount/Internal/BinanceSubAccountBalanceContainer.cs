namespace Binance.Api.SubAccount;

internal class BinanceSubAccountBalanceContainer
{
    [JsonProperty("balances")]
    public List<BinanceSubAccountBalance> Payload { get; set; } = [];
}