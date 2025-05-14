namespace Binance.Api.SubAccount;

internal record BinanceSubAccountFuturesSummaryV2
{
    [JsonProperty("futureAccountSummaryResp")]
    public BinanceSubAccountFuturesSummary Payload { get; set; } = default!;
}