namespace Binance.Api.Models.RestApi.SubAccount;

/// <summary>
/// Sub account futures summary
/// </summary>
public class BinanceSubAccountFuturesSummaryV2
{
    /// <summary>
    /// Futures account response (USDT margined)
    /// </summary>
    [JsonProperty("futureAccountSummaryResp")]
    public BinanceSubAccountFuturesSummary UsdtMarginedFutures { get; set; } = default!;

    /// <summary>
    /// Delivery account response (COIN margined)
    /// </summary>
    [JsonProperty("deliveryAccountSummaryResp")]
    public BinanceSubAccountFuturesDeliverySummary CoinMarginedFutures { get; set; } = default!;
}

public class BinanceSubAccountFuturesDeliverySummary
{
    public decimal TotalMarginBalanceOfBTC { get; set; }
    public decimal TotalUnrealizedProfitOfBTC { get; set; }
    public decimal TotalWalletBalanceOfBTC { get; set; }
    public string Asset { get; set; }
    public IEnumerable<BinanceSubAccountFuturesDeliveryAccount> SubAccountList { get; set; } = Array.Empty<BinanceSubAccountFuturesDeliveryAccount>();
}

public class BinanceSubAccountFuturesDeliveryAccount
{
    /// <summary>
    /// Email of the sub account
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Total margin balance
    /// </summary>
    public decimal TotalMarginBalance { get; set; }

    /// <summary>
    /// Total unrealized profit
    /// </summary>
    public decimal TotalUnrealizedProfit { get; set; }

    /// <summary>
    /// Total wallet balance
    /// </summary>
    public decimal TotalWalletBalance { get; set; }

    /// <summary>
    /// Asset
    /// </summary>
    public string Asset { get; set; }
}
