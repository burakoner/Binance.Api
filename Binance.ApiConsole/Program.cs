using Binance.Api;
using Binance.Api.Futures;
using Binance.Api.Margin;
using Binance.Api.Shared;
using Binance.Api.Spot;
using Binance.Api.Wallet;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Binance.ApiConsole;

internal class Program
{
    static async Task Main2(string[] args)
    {
        var api = new BinanceRestApiClient();

        // Spot > General Methods (PUBLIC)
        //var spot_101 = await api.Spot.PingAsync();
        //var spot_102 = await api.Spot.GetTimeAsync();
        var spot_103 = await api.Spot.GetExchangeInfoAsync();

        //var futures_101 = await api.UsdFutures.PingAsync();
        //var futures_102 = await api.UsdFutures.GetTimeAsync();
        var futures_103 = await api.UsdFutures.GetExchangeInfoAsync();

        var a = 0;
    }

    static async Task Main(string[] args)
    {
        var api = new BinanceRestApiClient();

        // Spot > General Methods (PUBLIC)
        var spot_101 = await api.Spot.PingAsync();
        var spot_102 = await api.Spot.GetTimeAsync();
        var spot_103 = await api.Spot.GetExchangeInfoAsync();

        /**/
        // Spot > Market Data Methods (PUBLIC)
        var spot_201 = await api.Spot.GetOrderBookAsync("BTCUSDT");
        var spot_202 = await api.Spot.GetRecentTradesAsync("BTCUSDT");
        var spot_203 = await api.Spot.GetTradeHistoryAsync("BTCUSDT");
        var spot_204 = await api.Spot.GetAggregatedTradeHistoryAsync("BTCUSDT");
        var spot_205 = await api.Spot.GetKlinesAsync("BTCUSDT", BinanceKlineInterval.OneDay);
        var spot_206 = await api.Spot.GetUiKlinesAsync("BTCUSDT", BinanceKlineInterval.OneDay);
        var spot_207 = await api.Spot.GetAveragePriceAsync("BTCUSDT");
        var spot_211 = await api.Spot.GetFullTickerAsync("BTCUSDT");
        var spot_212 = await api.Spot.GetFullTickersAsync(["BTCUSDT", "ETHUSDT"]);
        var spot_213 = await api.Spot.GetFullTickersAsync();
        var spot_214 = await api.Spot.GetMiniTickerAsync("BTCUSDT");
        var spot_215 = await api.Spot.GetMiniTickersAsync(["BTCUSDT", "ETHUSDT"]);
        var spot_216 = await api.Spot.GetMiniTickersAsync();
        var spot_221 = await api.Spot.GetTradingDayFullTickerAsync("BTCUSDT");
        var spot_222 = await api.Spot.GetTradingDayFullTickersAsync(["BTCUSDT", "ETHUSDT"]);
        var spot_223 = await api.Spot.GetTradingDayFullTickersAsync();
        var spot_224 = await api.Spot.GetTradingDayMiniTickerAsync("BTCUSDT");
        var spot_225 = await api.Spot.GetTradingDayMiniTickersAsync(["BTCUSDT", "ETHUSDT"]);
        var spot_226 = await api.Spot.GetTradingDayMiniTickersAsync();
        var spot_231 = await api.Spot.GetPriceTickerAsync("BTCUSDT");
        var spot_232 = await api.Spot.GetPriceTickersAsync(["BTCUSDT", "ETHUSDT"]);
        var spot_233 = await api.Spot.GetPriceTickersAsync();
        var spot_241 = await api.Spot.GetBookTickerAsync("BTCUSDT");
        var spot_242 = await api.Spot.GetBookTickersAsync(["BTCUSDT", "ETHUSDT"]);
        var spot_243 = await api.Spot.GetBookTickersAsync();
        var spot_251 = await api.Spot.GetRollingWindowTickerAsync("BTCUSDT");
        var spot_252 = await api.Spot.GetRollingWindowTickersAsync(["BTCUSDT", "ETHUSDT"], TimeSpan.FromHours(4));

        // Spot > Trading Methods (PRIVATE)
        var spot_301 = await api.Spot.PlaceOrderAsync("BTCUSDT", BinanceOrderSide.Buy, BinanceSpotOrderType.Market, 0.01m);
        var spot_302 = await api.Spot.PlaceTestOrderAsync("BTCUSDT", BinanceOrderSide.Buy, BinanceSpotOrderType.Market, 0.01m);
        var spot_303 = await api.Spot.GetOrderAsync("BTCUSDT", orderId: 100000001);
        var spot_304 = await api.Spot.GetOrderAsync("BTCUSDT", origClientOrderId: "---CLIENT-ORDER-ID---");
        var spot_305 = await api.Spot.CancelOrderAsync("BTCUSDT", orderId: 100000001);
        var spot_306 = await api.Spot.CancelOrderAsync("BTCUSDT", origClientOrderId: "---CLIENT-ORDER-ID---");
        var spot_307 = await api.Spot.CancelOrdersAsync("BTCUSDT");
        var spot_308 = await api.Spot.ReplaceOrderAsync("BTCUSDT", BinanceOrderSide.Buy, BinanceSpotOrderType.Market, BinanceSpotOrderCancelReplaceMode.StopOnFailure, cancelOrderId: 100000001, quantity: 0.1m);
        var spot_309 = await api.Spot.GetOpenOrdersAsync("BTCUSDT");
        var spot_310 = await api.Spot.GetOrdersAsync("BTCUSDT");

        // Spot > Account Methods (PRIVATE)
        var spot_401 = await api.Spot.GetAccountAsync();
        var spot_402 = await api.Spot.GetUserTradesAsync("BTCUSDT");
        var spot_403 = await api.Spot.GetOrderRateLimitStatusAsync();
        var spot_404 = await api.Spot.GetPreventedTradesAsync("BTCUSDT", orderId: 100000001);
        var spot_405 = await api.Spot.StartUserStreamAsync();
        var spot_406 = await api.Spot.KeepAliveUserStreamAsync("---LISTEN-KEY---");
        var spot_407 = await api.Spot.StopUserStreamAsync("---LISTEN-KEY---");

        // Margin > General Market Data Methods (PUBLIC)
        var margin_101 = await api.Margin.GetCrossMarginCollateralRatioAsync();
        var margin_102 = await api.Margin.GetMarginSymbolsAsync();
        var margin_103 = await api.Margin.GetIsolatedMarginSymbolsAsync();
        var margin_104 = await api.Margin.GetMarginAssetsAsync();
        var margin_105 = await api.Margin.GetMarginDelistScheduleAsync();
        var margin_106 = await api.Margin.GetIsolatedMarginTierDataAsync("SYMBOL");
        var margin_107 = await api.Margin.GetMarginPriceIndexAsync("SYMBOL");
        var margin_108 = await api.Margin.GetMarginAvaliableInventoryAsync(BinanceMarginInventoryType.Margin);
        var margin_109 = await api.Margin.GetLiabilityCoinLeverageBracketInCrossMarginProModeAsync();

        // Margin > General Borrow and Repay Methods (PRIVATE)
        var margin_201 = await api.Margin.GetFutureHourlyInterestRateAsync(["ASSET"], true);
        var margin_202 = await api.Margin.GetMarginInterestHistoryAsync();
        var margin_203 = await api.Margin.MarginBorrowAsync("ASSET", 100.0m);
        var margin_204 = await api.Margin.MarginRepayAsync("ASSET", 95.0m);
        var margin_205 = await api.Margin.GetMarginLoansAsync("ASSET");
        var margin_206 = await api.Margin.GetMarginInterestRateHistoryAsync("ASSET");
        var margin_207 = await api.Margin.GetMarginMaxBorrowAmountAsync("ASSET");

        // Margin > General Trade Methods (PRIVATE)
        var margin_301 = await api.Margin.GetMarginForcedLiquidationHistoryAsync();
        var margin_302 = await api.Margin.GetCrossMarginSmallLiabilityExchangeAssetsAsync();
        var margin_303 = await api.Margin.GetCrossMarginSmallLiabilityExchangeHistoryAsync();
        var margin_304 = await api.Margin.CancelAllMarginOrdersAsync("SYMBOL");
        var margin_305 = await api.Margin.CancelMarginOcoOrderAsync("SYMBOL");
        var margin_306 = await api.Margin.CancelMarginOrderAsync("SYMBOL");
        var margin_307 = await api.Margin.PlaceMarginOCOOrderAsync("SYMBOL", BinanceOrderSide.Buy, 100.0m, 100.0m, 15.0m);
        var margin_308 = await api.Margin.PlaceMarginOrderAsync("SYMBOL", BinanceOrderSide.Buy, BinanceSpotOrderType.Market, 100.0m);
        var margin_309 = await api.Margin.GetMarginOrderRateLimitStatusAsync();
        var margin_310 = await api.Margin.GetMarginOcoOrdersAsync();
        var margin_311 = await api.Margin.GetMarginOrdersAsync("SYMBOL");
        var margin_312 = await api.Margin.GetMarginOcoOrderAsync();
        var margin_313 = await api.Margin.GetMarginOpenOcoOrdersAsync();
        var margin_314 = await api.Margin.GetOpenMarginOrdersAsync();
        var margin_315 = await api.Margin.GetMarginOrderAsync("SYMBOL");
        var margin_316 = await api.Margin.GetMarginUserTradesAsync("SYMBOL");
        var margin_317 = await api.Margin.CrossMarginSmallLiabilityExchangeAsync(["ASSET"]);

        // Margin > General Transfer Methods (PRIVATE)
        var margin_401 = await api.Margin.GetMarginTransferHistoryAsync(BinanceTransferDirection.RollIn);
        var margin_402 = await api.Margin.GetMarginMaxTransferAmountAsync("ASSET");

        // Margin > General Account Methods (PRIVATE)
        var margin_501 = await api.Margin.CrossMarginAdjustMaxLeverageAsync(20);
        var margin_502 = await api.Margin.DisableIsolatedMarginAccountAsync("SYMBOL");
        var margin_503 = await api.Margin.EnableIsolatedMarginAccountAsync("SYMBOL");
        var margin_504 = await api.Margin.GetBnbBurnStatusAsync();
        var margin_505 = await api.Margin.GetMarginLevelInformationAsync();
        var margin_506 = await api.Margin.GetMarginAccountInfoAsync();
        var margin_507 = await api.Margin.GetInterestMarginDataAsync();
        var margin_508 = await api.Margin.GetEnabledIsolatedMarginAccountLimitAsync();
        var margin_509 = await api.Margin.GetIsolatedMarginAccountAsync();
        var margin_510 = await api.Margin.GetIsolatedMarginFeeDataAsync();

        // Margin > General Trade Data Stream Methods (PRIVATE)
        var margin_601 = await api.Margin.StartMarginUserStreamAsync();
        var margin_602 = await api.Margin.KeepAliveMarginUserStreamAsync("---LISTEN-KEY---");
        var margin_603 = await api.Margin.StopMarginUserStreamAsync("---LISTEN-KEY---");
        var margin_604 = await api.Margin.StartIsolatedMarginUserStreamAsync("SYMBOL");
        var margin_605 = await api.Margin.KeepAliveIsolatedMarginUserStreamAsync("SYMBOL", "---LISTEN-KEY---");
        var margin_606 = await api.Margin.CloseIsolatedMarginUserStreamAsync("SYMBOL", "---LISTEN-KEY---");

        // TODO: Margin > General Risk Data Stream Methods (PRIVATE)

        // Wallet > Capital Methods (PRIVATE)
        var wallet_101 = await api.Wallet.GetUserAssetsAsync();
        var btcBalance = wallet_101.Data.FirstOrDefault(x => x.Asset == "BTC");
        var usdtBalance = wallet_101.Data.FirstOrDefault(x => x.Asset == "USDT");
        var wallet_102 = await api.Wallet.WithdrawAsync("---ASSET---", "---ADDRESS---", 0.01m);
        var wallet_103 = await api.Wallet.GetWithdrawalHistoryAsync();
        var wallet_104 = await api.Wallet.GetWithdrawalHistoryAsync("---ASSET---");
        var wallet_105 = await api.Wallet.GetWithdrawalAddressesAsync();
        var wallet_106 = await api.Wallet.GetDepositHistoryAsync();
        var wallet_107 = await api.Wallet.GetDepositHistoryAsync("---ASSET---");
        var wallet_108 = await api.Wallet.GetDepositAddressAsync("BTC");
        var wallet_109 = await api.Wallet.GetDepositAddressAsync("USDT");
        var wallet_110 = await api.Wallet.GetDepositAddressAsync("USDT", "TRX");

        // Wallet > Asset Methods (PRIVATE)
        var wallet_201 = await api.Wallet.GetAssetDetailsAsync();
        var wallet_202 = await api.Wallet.GetWalletBalancesAsync();
        var wallet_203 = await api.Wallet.GetWalletBalancesAsync("BTC");
        var wallet_204 = await api.Wallet.GetBalancesAsync();
        var wallet_205 = await api.Wallet.GetBalancesAsync("USDT");
        var wallet_206 = await api.Wallet.TransferAsync(BinanceUniversalTransferType.FundingToUsdFutures, "USDT", 10.0m);
        var wallet_207 = await api.Wallet.GetTransfersAsync(BinanceUniversalTransferType.FundingToUsdFutures);
        var wallet_208 = await api.Wallet.SetBnbBurnStatusAsync(true, true);
        var wallet_209 = await api.Wallet.GetAssetsForDustTransferAsync();
        var wallet_210 = await api.Wallet.DustTransferAsync(["ASSET01", "ASSET02", "ASSET03"]);
        var wallet_211 = await api.Wallet.GetDustLogAsync();
        var wallet_212 = await api.Wallet.GetAssetDividendRecordsAsync();
        var wallet_213 = await api.Wallet.GetTradeFeeAsync();
        var wallet_214 = await api.Wallet.GetTradeFeeAsync("BTCUSDT");
        var wallet_215 = await api.Wallet.GetFundingWalletAsync();
        var wallet_216 = await api.Wallet.GetFundingWalletAsync("USDT");
        var wallet_217 = await api.Wallet.GetCloudMiningHistoryAsync(DateTime.Now.AddDays(30), DateTime.Now);
        var wallet_218 = await api.Wallet.GetDelistScheduleAsync();

        // Wallet > Account Methods (PRIVATE)
        var wallet_301 = await api.Wallet.GetAccountVipLevelAndStatusAsync();
        var wallet_302 = await api.Wallet.GetDailySpotAccountSnapshotAsync();
        var wallet_303 = await api.Wallet.GetDailyMarginAccountSnapshotAsync();
        var wallet_304 = await api.Wallet.GetDailyFutureAccountSnapshotAsync();
        var wallet_305 = await api.Wallet.DisableFastWithdrawSwitchAsync();
        var wallet_306 = await api.Wallet.EnableFastWithdrawSwitchAsync();
        var wallet_307 = await api.Wallet.GetAccountStatusAsync();
        var wallet_308 = await api.Wallet.GetTradingStatusAsync();
        var wallet_309 = await api.Wallet.GetAPIKeyPermissionsAsync();

        // TODO: Wallet > Travel Rule (Local Entity) Methods (PRIVATE)

        // Wallet > Other Methods (PUBLIC)
        var wallet_501 = await api.Wallet.GetSystemStatusAsync();

        // Algo > Futures Methods (PRIVATE)
        var algo_101 = await api.Algo.Futures.PlaceVolumeParticipationOrderAsync("SYMBOL", BinanceOrderSide.Buy, 100.0m, BinanceUrgency.High);
        var algo_102 = await api.Algo.Futures.PlaceTimeWeightedAveragePriceOrderAsync("SYMBOL", BinanceOrderSide.Buy, 100.0m, 900);
        var algo_103 = await api.Algo.Futures.CancelAlgoOrderAsync(1_000_001L);
        var algo_104 = await api.Algo.Futures.GetAlgoSubOrdersAsync(1_000_001L);
        var algo_105 = await api.Algo.Futures.GetOpenAlgoOrdersAsync();
        var algo_106 = await api.Algo.Futures.GetClosedAlgoOrdersAsync();

        // Algo > Spot Methods (PRIVATE)
        var algo_202 = await api.Algo.Spot.PlaceTimeWeightedAveragePriceOrderAsync("SYMBOL", BinanceOrderSide.Buy, 100.0m, 900);
        var algo_203 = await api.Algo.Spot.CancelAlgoOrderAsync(1_000_001L);
        var algo_204 = await api.Algo.Spot.GetAlgoSubOrdersAsync(1_000_001L);
        var algo_205 = await api.Algo.Spot.GetOpenAlgoOrdersAsync();
        var algo_206 = await api.Algo.Spot.GetClosedAlgoOrdersAsync();

        // USDⓈ-M Futures -> Market Data Methods (PUBLIC)
        var futures_101 = await api.UsdFutures.PingAsync();
        var futures_102 = await api.UsdFutures.GetTimeAsync();
        var futures_103 = await api.UsdFutures.GetExchangeInfoAsync();
        var futures_104 = await api.UsdFutures.GetOrderBookAsync("SYMBOL");
        var futures_105 = await api.UsdFutures.GetRecentTradesAsync("SYMBOL");
        var futures_106 = await api.UsdFutures.GetTradeHistoryAsync("SYMBOL");
        var futures_107 = await api.UsdFutures.GetAggregatedTradeHistoryAsync("SYMBOL");
        var futures_108 = await api.UsdFutures.GetKlinesAsync("SYMBOL", BinanceKlineInterval.OneDay);
        var futures_109 = await api.UsdFutures.GetContinuousContractKlinesAsync("SYMBOL", BinanceContractType.Perpetual, BinanceKlineInterval.OneDay);
        var futures_110 = await api.UsdFutures.GetIndexPriceKlinesAsync("SYMBOL", BinanceKlineInterval.OneDay);
        var futures_111 = await api.UsdFutures.GetMarkPriceKlinesAsync("SYMBOL", BinanceKlineInterval.OneDay);
        var futures_112 = await api.UsdFutures.GetPremiumIndexKlinesAsync("SYMBOL", BinanceKlineInterval.OneDay);
        var futures_113 = await api.UsdFutures.GetMarkPriceAsync("SYMBOL");
        var futures_114 = await api.UsdFutures.GetMarkPricesAsync();
        var futures_115 = await api.UsdFutures.GetFundingRatesAsync("SYMBOL");
        var futures_116 = await api.UsdFutures.GetFundingInfoAsync();
        var futures_117 = await api.UsdFutures.GetTickerAsync("SYMBOL");
        var futures_118 = await api.UsdFutures.GetTickersAsync();
        var futures_119 = await api.UsdFutures.GetPriceAsync("SYMBOL");
        var futures_120 = await api.UsdFutures.GetPricesAsync();
        var futures_121 = await api.UsdFutures.GetBookPriceAsync("SYMBOL");
        var futures_122 = await api.UsdFutures.GetBookPricesAsync();
        var futures_123 = await api.UsdFutures.GetOpenInterestAsync("SYMBOL");
        var futures_124 = await api.UsdFutures.GetOpenInterestHistoryAsync("SYMBOL", BinancePeriodInterval.FourHour);
        var futures_125 = await api.UsdFutures.GetTopLongShortPositionRatioAsync("SYMBOL", BinancePeriodInterval.FourHour);
        var futures_126 = await api.UsdFutures.GetTopLongShortAccountRatioAsync("SYMBOL", BinancePeriodInterval.FourHour);
        var futures_127 = await api.UsdFutures.GetGlobalLongShortAccountRatioAsync("SYMBOL", BinancePeriodInterval.FourHour);
        var futures_128 = await api.UsdFutures.GetTakerBuySellVolumeRatioAsync("SYMBOL", BinancePeriodInterval.FourHour);
        var futures_129 = await api.UsdFutures.GetBasisAsync("SYMBOL", BinanceContractType.Perpetual, BinancePeriodInterval.FourHour);
        var futures_130 = await api.UsdFutures.GetCompositeIndexInfoAsync();
        var futures_131 = await api.UsdFutures.GetAssetIndexAsync("SYMBOL");
        var futures_132 = await api.UsdFutures.GetAssetIndexesAsync();
        var futures_133 = await api.UsdFutures.PingAsync();
        var futures_134 = await api.UsdFutures.PingAsync();

        // USDⓈ-M Futures -> Trading Methods (PRIVATE)
        var futures_201 = await api.UsdFutures.PlaceOrderAsync("SYMBOL", BinanceOrderSide.Buy, BinanceFuturesOrderType.Market, 100.0m);
        var futures_202 = await api.UsdFutures.PlaceMultipleOrdersAsync([]);
        var futures_203 = await api.UsdFutures.EditOrderAsync("SYMBOL", BinanceOrderSide.Buy, 110.0m, orderId:1_000_000L);
        var futures_204 = await api.UsdFutures.EditMultipleOrdersAsync([]);
        var futures_205 = await api.UsdFutures.GetOrderEditHistoryAsync("SYMBOL");
        var futures_206 = await api.UsdFutures.CancelOrderAsync("SYMBOL", orderId: 1_000_000L);
        var futures_207 = await api.UsdFutures.CancelMultipleOrdersAsync("SYMBOL", [1_000_000L]);
        var futures_208 = await api.UsdFutures.CancelAllOrdersAsync("SYMBOL");
        var futures_209 = await api.UsdFutures.CancelAllOrdersAfterTimeoutAsync("SYMBOL", TimeSpan.FromSeconds(15));
        var futures_210 = await api.UsdFutures.GetOrderAsync("SYMBOL", orderId: 1_000_000L);
        var futures_211 = await api.UsdFutures.GetOrdersAsync();
        var futures_212 = await api.UsdFutures.GetOpenOrdersAsync();
        var futures_213 = await api.UsdFutures.GetOpenOrderAsync("SYMBOL");
        var futures_214 = await api.UsdFutures.GetForcedOrdersAsync();
        var futures_215 = await api.UsdFutures.GetUserTradesAsync("SYMBOL");
        var futures_216 = await api.UsdFutures.ChangeMarginTypeAsync("SYMBOL", BinanceFuturesMarginType.Isolated);
        var futures_217 = await api.UsdFutures.ModifyPositionModeAsync(true);
        var futures_218 = await api.UsdFutures.ChangeInitialLeverageAsync("SYMBOL", 10);
        var futures_219 = await api.UsdFutures.SetMultiAssetsModeAsync(true);
        var futures_220 = await api.UsdFutures.ModifyPositionMarginAsync("SYMBOL", 100.0m, BinanceFuturesMarginChangeDirectionType.Add);
        var futures_221 = await api.UsdFutures.GetPositionInformationAsync();
        var futures_222 = await api.UsdFutures.GetPositionsAsync();
        var futures_223 = await api.UsdFutures.GetPositionAdlQuantileEstimationAsync();
        var futures_224 = await api.UsdFutures.GetMarginChangeHistoryAsync("SYMBOL");










        Console.WriteLine("Done!..");








        /** /
        var ws = new BinanceStreamClient();
        await ws.Spot.SubscribeToAllMiniTickerUpdatesAsync((data) =>
        {
            foreach (var d in data.Data)
            {
                Console.WriteLine($"{d.Symbol} O:{d.OpenPrice} H:{d.HighPrice} L:{d.LowPrice} C:{d.LastPrice} V:{d.Volume}");
            }
            Console.WriteLine($"Got {data.Data.Count()} rows");
        }, default);
        /**/

        //Console.ReadLine();
        //await ws.UnsubscribeAllAsync();

        /*
        var api = new BinanceRestApiClient(new BinanceRestApiClientOptions
        {
            OutputOriginalData= true,
        });
        await api.Spot.Server.GetServerTimeAsync();
        var i = 0;
        var sw01 = Stopwatch.StartNew();
        while (true)
        {
            i++;
            var date = await api.Spot.Server.GetServerTimeAsync();
            Console.WriteLine($"{i} Response:{date.Data}  -  Duration:{date.ResponseTime.Value.TotalMilliseconds}ms  -  Avg:{sw01.ElapsedMilliseconds / i}ms");
        }
        */

        Console.WriteLine("Done!..");
        Console.ReadLine();
    }
}