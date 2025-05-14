namespace Binance.Api.SubAccount;

internal class BinanceSubAccountBalanceContainer
{
    [JsonProperty("balances")]
    public IEnumerable<BinanceSubAccountBalance> Payload { get; set; } = [];
}