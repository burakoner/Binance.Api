using Binance.Api;
using Binance.Api.Spot.Enums;
using System;
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

            /*
            // Spot General (Public)
            var spot_101 = await api.Spot.General.PingAsync();
            var spot_102 = await api.Spot.General.GetTimeAsync();
            var spot_103 = await api.Spot.General.GetExchangeInfoAsync();

            // Spot Market Data (Public)
            var spot_201 = await api.Spot.MarketData.GetOrderBookAsync("BTCUSDT");
            var spot_202 = await api.Spot.MarketData.GetRecentTradesAsync("BTCUSDT");
            var spot_203 = await api.Spot.MarketData.GetTradeHistoryAsync("BTCUSDT");
            var spot_204 = await api.Spot.MarketData.GetAggregatedTradeHistoryAsync("BTCUSDT");
            var spot_205 = await api.Spot.MarketData.GetKlinesAsync("BTCUSDT", BinanceKlineInterval.OneDay);
            var spot_206 = await api.Spot.MarketData.GetUiKlinesAsync("BTCUSDT", BinanceKlineInterval.OneDay);
            var spot_207 = await api.Spot.MarketData.GetAveragePriceAsync("BTCUSDT");
            var spot_211 = await api.Spot.MarketData.GetFullTickerAsync("BTCUSDT");
            var spot_212 = await api.Spot.MarketData.GetFullTickersAsync(["BTCUSDT", "ETHUSDT"]);
            var spot_213 = await api.Spot.MarketData.GetFullTickersAsync();
            var spot_214 = await api.Spot.MarketData.GetMiniTickerAsync("BTCUSDT");
            var spot_215 = await api.Spot.MarketData.GetMiniTickersAsync(["BTCUSDT", "ETHUSDT"]);
            var spot_216 = await api.Spot.MarketData.GetMiniTickersAsync();
            var spot_221 = await api.Spot.MarketData.GetTradingDayFullTickerAsync("BTCUSDT");
            var spot_222 = await api.Spot.MarketData.GetTradingDayFullTickersAsync(["BTCUSDT", "ETHUSDT"]);
            var spot_223 = await api.Spot.MarketData.GetTradingDayFullTickersAsync();
            var spot_224 = await api.Spot.MarketData.GetTradingDayMiniTickerAsync("BTCUSDT");
            var spot_225 = await api.Spot.MarketData.GetTradingDayMiniTickersAsync(["BTCUSDT", "ETHUSDT"]);
            var spot_226 = await api.Spot.MarketData.GetTradingDayMiniTickersAsync();
            var spot_231 = await api.Spot.MarketData.GetPriceAsync("BTCUSDT");
            var spot_232 = await api.Spot.MarketData.GetPricesAsync(["BTCUSDT", "ETHUSDT"]);
            var spot_233 = await api.Spot.MarketData.GetPricesAsync();
            var spot_241 = await api.Spot.MarketData.GetRollingWindowTickerAsync("BTCUSDT");
            var spot_242 = await api.Spot.MarketData.GetRollingWindowTickersAsync(["BTCUSDT", "ETHUSDT"], TimeSpan.FromHours(4));

            // Spot Trading (Private Signed)
            var spot_301 = await api.Spot.Trading.PlaceOrderAsync("BTCUSDT", BinanceSpotOrderSide.Buy, BinanceSpotOrderType.Market, 0.01m);
            var spot_302 = await api.Spot.Trading.PlaceTestOrderAsync("BTCUSDT", BinanceSpotOrderSide.Buy, BinanceSpotOrderType.Market, 0.01m);
            var spot_303 = await api.Spot.Trading.GetOrderAsync("BTCUSDT", orderId: 100000001);
            var spot_304 = await api.Spot.Trading.GetOrderAsync("BTCUSDT", origClientOrderId: "---CLIENT-ORDER-ID---");
            var spot_305 = await api.Spot.Trading.CancelOrderAsync("BTCUSDT", orderId: 100000001);
            var spot_306 = await api.Spot.Trading.CancelOrderAsync("BTCUSDT", origClientOrderId: "---CLIENT-ORDER-ID---");
            var spot_307 = await api.Spot.Trading.CancelOrdersAsync("BTCUSDT");
            var spot_308 = await api.Spot.Trading.ReplaceOrderAsync("BTCUSDT", BinanceSpotOrderSide.Buy, BinanceSpotOrderType.Market, BinanceSpotOrderCancelReplaceMode.StopOnFailure, cancelOrderId: 100000001, quantity:0.1m);
            var spot_309 = await api.Spot.Trading.GetOpenOrdersAsync("BTCUSDT");
            var spot_310 = await api.Spot.Trading.GetOrdersAsync("BTCUSDT");
            */


            /**/

            var server_04 = await api.Spot.GetSystemStatusAsync();

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
            // Account (Private)
            var account_01 = await api.Spot.Account.GetUserAssetsAsync();
            var btcBalance = account_01.Data.FirstOrDefault(x => x.Asset == "BTC");
            var usdtBalance = account_01.Data.FirstOrDefault(x => x.Asset == "USDT");
            var account_02 = await api.Spot.Account.GetDailySpotAccountSnapshotAsync();
            var account_03 = await api.Spot.Account.GetDailyMarginAccountSnapshotAsync();
            var account_04 = await api.Spot.Account.GetDailyFutureAccountSnapshotAsync();
            var account_05 = await api.Spot.Account.EnableFastWithdrawSwitchAsync();
            var account_06 = await api.Spot.Account.DisableFastWithdrawSwitchAsync();
            var account_07 = await api.Spot.Account.WithdrawAsync("---ASSET---", "---ADDRESS---", 0.01m);
            var account_08 = await api.Spot.Account.GetWithdrawalHistoryAsync("---ASSET---");
            var account_09 = await api.Spot.Account.GetWithdrawalHistoryAsync();
            var account_10 = await api.Spot.Account.GetDepositHistoryAsync("---ASSET---");
            var account_11 = await api.Spot.Account.GetDepositHistoryAsync();
            var account_12 = await api.Spot.Account.GetDepositAddressAsync("BTC");
            var account_13 = await api.Spot.Account.GetDepositAddressAsync("USDT");
            var account_14 = await api.Spot.Account.GetDepositAddressAsync("USDT", "TRX");
            var account_15 = await api.Spot.Account.GetAccountStatusAsync();
            var account_16 = await api.Spot.Account.GetTradingStatusAsync();
            var account_17 = await api.Spot.Account.GetDustLogAsync();
            var account_18 = await api.Spot.Account.GetAssetsForDustTransferAsync();
            var account_19 = await api.Spot.Account.DustTransferAsync(new List<string> { "ASSET01", "ASSET02", "ASSET03" });
            var account_20 = await api.Spot.Account.GetAssetDividendRecordsAsync();
            var account_21 = await api.Spot.Account.GetAssetDetailsAsync();
            var account_22 = await api.Spot.Account.GetTradeFeeAsync();
            var account_23 = await api.Spot.Account.GetTradeFeeAsync("BTCUSDT");
            var account_24 = await api.Spot.Account.TransferAsync(UniversalTransferType.FundingToUsdFutures, "USDT", 10.0m);
            var account_25 = await api.Spot.Account.GetTransfersAsync(UniversalTransferType.FundingToUsdFutures);
            var account_26 = await api.Spot.Account.GetFundingWalletAsync("USDT");
            var account_27 = await api.Spot.Account.GetFundingWalletAsync();
            var account_28 = await api.Spot.Account.GetBalancesAsync("USDT");
            var account_29 = await api.Spot.Account.GetBalancesAsync();
            var account_30 = await api.Spot.Account.ConvertTransferAsync("---CLIENT-ORDER-ID---", "USDT", 10.0m, "BNB");
            var account_31 = await api.Spot.Account.GetConvertTransferHistoryAsync(new DateTime(2022, 7, 1), new DateTime(2022, 12, 31, 23, 59, 59));
            var account_32 = await api.Spot.Account.GetAPIKeyPermissionsAsync();
            */

            /*
            // User Stream (Private)
            var stream_01 = await api.Spot.UserStream.CreateSpotUserStreamListenKeyAsync();
            var stream_02 = await api.Spot.UserStream.KeepAliveSpotUserStreamAsync(stream_01.Data);
            var stream_03 = await api.Spot.UserStream.StopSpotUserStreamAsync(stream_01.Data);
            */

            Console.WriteLine("Done!..");








            /** /
            var ws = new BinanceStreamClient();
            await ws.Spot.MarketData.SubscribeToAllMiniTickerUpdatesAsync((data) =>
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