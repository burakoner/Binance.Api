using Binance.Api;
using Binance.Api.Shared;
using Binance.Api.Spot;
using Binance.Api.Wallet;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Binance.ApiConsole
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var api = new BinanceRestApiClient(new BinanceRestApiClientOptions
            {
                RawResponse = true,
            });

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
            var spot_301 = await api.Spot.PlaceOrderAsync("BTCUSDT", BinanceSpotOrderSide.Buy, BinanceSpotOrderType.Market, 0.01m);
            var spot_302 = await api.Spot.PlaceTestOrderAsync("BTCUSDT", BinanceSpotOrderSide.Buy, BinanceSpotOrderType.Market, 0.01m);
            var spot_303 = await api.Spot.GetOrderAsync("BTCUSDT", orderId: 100000001);
            var spot_304 = await api.Spot.GetOrderAsync("BTCUSDT", origClientOrderId: "---CLIENT-ORDER-ID---");
            var spot_305 = await api.Spot.CancelOrderAsync("BTCUSDT", orderId: 100000001);
            var spot_306 = await api.Spot.CancelOrderAsync("BTCUSDT", origClientOrderId: "---CLIENT-ORDER-ID---");
            var spot_307 = await api.Spot.CancelOrdersAsync("BTCUSDT");
            var spot_308 = await api.Spot.ReplaceOrderAsync("BTCUSDT", BinanceSpotOrderSide.Buy, BinanceSpotOrderType.Market, BinanceSpotOrderCancelReplaceMode.StopOnFailure, cancelOrderId: 100000001, quantity: 0.1m);
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

            // Wallet > Capital Methods (PRIVATE)
            var wallet_101= await api.Wallet.GetUserAssetsAsync();
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
















            /*
            var account_30 = await api.Wallet.ConvertTransferAsync("---CLIENT-ORDER-ID---", "USDT", 10.0m, "BNB");
            var account_31 = await api.Wallet.GetConvertTransferHistoryAsync(new DateTime(2022, 7, 1), new DateTime(2022, 12, 31, 23, 59, 59));
            */


            /** /
            // Market Data (Public)
            var mdata_01 = await api.Spot.GetOrderBookAsync("BTCUSDT");
            var mdata_02 = await api.Spot.GetRecentTradesAsync("BTCUSDT");
            var mdata_03 = await api.Spot.GetTradeHistoryAsync("BTCUSDT");
            var mdata_04 = await api.Spot.GetAggregatedTradeHistoryAsync("BTCUSDT");
            var mdata_05 = await api.Spot.GetKlinesAsync("BTCUSDT", BinanceKlineInterval.OneDay);
            var mdata_06 = await api.Spot.GetUiKlinesAsync("BTCUSDT", BinanceKlineInterval.OneDay);
            var mdata_07 = await api.Spot.GetCurrentAvgPriceAsync("BTCUSDT");
            var mdata_08 = await api.Spot.GetTickerAsync("BTCUSDT");
            var mdata_09 = await api.Spot.GetTickersAsync();
            var mdata_10 = await api.Spot.GetPriceAsync("BTCUSDT");
            var mdata_11 = await api.Spot.GetPricesAsync();
            var mdata_12 = await api.Spot.GetBookPriceAsync("BTCUSDT");
            var mdata_13 = await api.Spot.GetBookPricesAsync();
            var mdata_14 = await api.Spot.GetRollingWindowTickerAsync("BTCUSDT");
            /**/

            /*
            // User Stream (Private)
            var stream_01 = await api.Spot.UserStream.CreateSpotUserStreamListenKeyAsync();
            var stream_02 = await api.Spot.UserStream.KeepAliveSpotUserStreamAsync(stream_01.Data);
            var stream_03 = await api.Spot.UserStream.StopSpotUserStreamAsync(stream_01.Data);
            */

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
}