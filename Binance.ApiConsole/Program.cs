using Binance.Api;
using System;
using System.Threading.Tasks;

namespace Binance.ApiConsole
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var api = new BinanceRestApiClient();

            // Server (Public)
            var server_01 = await api.Spot.PingAsync();
            var server_02 = await api.Spot.GetServerTimeAsync();
            var server_03 = await api.Spot.GetExchangeInfoAsync();
            var server_04 = await api.Spot.GetSystemStatusAsync();

            /**/
            // Market Data (Public)
            var mdata_01 = await api.Spot.GetOrderBookAsync("BTCUSDT");
            var mdata_02 = await api.Spot.GetRecentTradesAsync("BTCUSDT");
            var mdata_03 = await api.Spot.GetTradeHistoryAsync("BTCUSDT");
            var mdata_04 = await api.Spot.GetAggregatedTradeHistoryAsync("BTCUSDT");
            var mdata_05 = await api.Spot.GetKlinesAsync("BTCUSDT", Api.Enums.KlineInterval.OneDay);
            var mdata_06 = await api.Spot.GetUiKlinesAsync("BTCUSDT", Api.Enums.KlineInterval.OneDay);
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
            // Trading (Private)
            var trade_01 = await api.Spot.Trading.PlaceTestOrderAsync("BTCUSDT", OrderSide.Buy, SpotOrderType.Market, 0.01m);
            var trade_02 = await api.Spot.Trading.PlaceOrderAsync("BTCUSDT", OrderSide.Buy, SpotOrderType.Market, 0.01m);
            var trade_03 = await api.Spot.Trading.CancelOrderAsync("BTCUSDT", orderId: 100000001);
            var trade_04 = await api.Spot.Trading.CancelOrderAsync("BTCUSDT", origClientOrderId: "---CLIENT-ORDER-ID---");
            var trade_05 = await api.Spot.Trading.CancelAllOrdersAsync("BTCUSDT");
            var trade_06 = await api.Spot.Trading.ReplaceOrderAsync("BTCUSDT", OrderSide.Buy, SpotOrderType.Market, CancelReplaceMode.StopOnFailure, ...);
            var trade_07 = await api.Spot.Trading.GetOpenOrdersAsync("BTCUSDT");
            var trade_08 = await api.Spot.Trading.GetOrderAsync("BTCUSDT", orderId: 100000001);
            var trade_09 = await api.Spot.Trading.GetOrderAsync("BTCUSDT", origClientOrderId: "---CLIENT-ORDER-ID---");
            var trade_10 = await api.Spot.Trading.GetOrdersAsync("BTCUSDT");
            var trade_11 = await api.Spot.Trading.PlaceOcoOrderAsync("BTCUSDT", ...);
            var trade_12 = await api.Spot.Trading.CancelOcoOrderAsync("BTCUSDT", orderListId: 100000001);
            var trade_13 = await api.Spot.Trading.CancelOcoOrderAsync("BTCUSDT", listClientOrderId: "---CLIENT-ORDER-ID---");
            var trade_14 = await api.Spot.Trading.GetOcoOrderAsync(orderListId: 100000001);
            var trade_15 = await api.Spot.Trading.GetOcoOrderAsync(origClientOrderId: "---CLIENT-ORDER-ID---");
            var trade_16 = await api.Spot.Trading.GetOcoOrdersAsync();
            var trade_17 = await api.Spot.Trading.GetOpenOcoOrdersAsync();
            var trade_18 = await api.Spot.Trading.GetAccountInfoAsync();
            var trade_19 = await api.Spot.Trading.GetUserTradesAsync("BTCUSDT");
            var trade_20 = await api.Spot.Trading.GetOrderRateLimitStatusAsync();
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