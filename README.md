# Binance.Api

A .Net wrapper for the Binance API as described on [Binance Developer Center](https://developers.binance.com/en), including all features the API provides using clear and readable objects.

**If you think something is broken, something is missing or have any questions, please open an [Issue](https://github.com/burakoner/Binance.Api/issues)**

## Donations

Donations are greatly appreciated and a motivation to keep improving.

|Coin|Address|
|:--|:--|
|**BTC**|33WbRKqt7wXARVdAJSu1G1x3QnbyPtZ2bH|
|**ETH**|0x65b02db9b67b73f5f1e983ae10796f91ded57b64|
|**USDT (TRC-20)**|TXwqoD7doMESgitfWa8B2gHL7HuweMmNBJ|

## Changes & Release Notes

Please take a look to [ChangeLog](https://github.com/burakoner/Binance.Api/blob/master/CHANGELOG.md) for all changes.

## Supported Frameworks
The library is targeting both `.NET Standard 2.0` and `.NET Standard 2.1` for optimal compatibility, as well as dotnet 6.0, 7.0, 8.0 and 9.0 to use the latest framework features.

|.NET Implementation|Version Support|
|--|--|
|.NET Core|`2.0` and higher|
|.NET Framework|`4.6.1` and higher|
|.NET Standard|`2.0` and higher|
|Mono|`5.4` and higher|
|Unity|`2018.1` and higher|
|UWP|`10.0.16299` and higher|
|Xamarin.Android|`8.0` and higher|
|Xamarin.iOS|`10.14` and higher|

## Supported Endpoints

|Product|Rest API|WS Query|WS Stream|
|--|:--|:--|:--|
|Spot Trading|✅|✅|✅|
|Margin Trading|✅|✅|✅|
|Coin-M Futures|✅|✅|✅|
|USDⓈ-M Futures|✅|✅|✅|
|European Options|✅|-|⌛|
|Portfolio Margin|⏹|-|⏹|
|Portfolio Margin Pro|⏹|-|⏹|
|Algo Trading|✅|-|-|
|Copy Trading|✅|-|-|
|Wallet|✅|-|-|
|Auto Invest|✅|-|-|
|Convert|✅|-|-|
|Institutional Loan|✅|-|-|
|Sub Account|✅|-|-|
|Exchange Link|✅|-|-|
|Link-n-Trade|✅|-|-|
|Staking|✅|-|-|
|Dual Investment|✅|-|-|
|Mining|✅|-|-|
|Crypto Loan|✅|-|-|
|VIP Loan|⏹|-|-|
|C2C|✅|-|-|
|Fiat|✅|-|-|
|NFT|✅|-|-|
|Gift Card|✅|-|-|
|Rebate|✅|-|-|
|Simple Earn|✅|-|-|
|Binance Pay|✅|-|-|
|Binance Connect|⏹|-|-|

⏹Planned ⌛In Progress ✅Done [-] Not Supported

## Installation

[![NuGet Version](https://img.shields.io/nuget/v/Binance.Api.svg?style=for-the-badge)](https://www.nuget.org/packages/Binance.Api)  [![Nuget Downloads](https://img.shields.io/nuget/dt/Binance.Api.svg?style=for-the-badge)](https://www.nuget.org/packages/Binance.Api)


```console
PM> Install-Package Binance.Api
```

To get started with Binance.Api first you will need to get the library itself. The easiest way to do this is to install the package into your project using  [NuGet](https://www.nuget.org/packages/Binance.Api). Using Visual Studio this can be done in two ways.

### Using the package manager

In Visual Studio right click on your solution and select 'Manage NuGet Packages for solution...'. A screen will appear which initially shows the currently installed packages. In the top bit select 'Browse'. This will let you download net package from the NuGet server. In the search box type 'Binance.Api' and hit enter. The Binance.Api package should come up in the results. After selecting the package you can then on the right hand side select in which projects in your solution the package should install. After you've selected all project you wish to install and use Binance.Api in hit 'Install' and the package will be downloaded and added to you projects.

### Using the package manager console

In Visual Studio in the top menu select 'Tools' -> 'NuGet Package Manager' -> 'Package Manager Console'. This should open up a command line interface. On top of the interface there is a dropdown menu where you can select the Default Project. This is the project that Binance.Api will be installed in. After selecting the correct project type  `Install-Package Binance.Api`  in the command line interface. This should install the latest version of the package in your project.

After doing either of above steps you should now be ready to actually start using Binance.Api.

## Getting started

After installing it's time to actually use it. To get started we have to add the Binance.Api namespace:  `using Binance.Api;`.

Binance.Api provides two clients to interact with the Binance.Api. The  `BinanceRestApiClient`  provides all rest API calls. The  `BinanceSocketApiClient` provides functions to interact with the websocket provided by the Binance.Api. Both clients are disposable and as such can be used in a  `using`statement.

## Usage

You can find the basic usage of the methods in this library as below.
[Rest Api Methods](#rest-api-methods)
[WebSocket Api Query Methods](#websocket-api-query-methods)
[WebSocket Api Stream Methods](#rest-api-methods)

## Rest Api Methods

```csharp
// Rest API Client
var api = new BinanceRestApiClient();
api.SetApiCredentials("XXXXXXXX-API-KEY-XXXXXXXX", "XXXXXXXX-API-SECRET-XXXXXXXX");

// Spot > General Methods (PUBLIC)
var spot_101 = await api.Spot.PingAsync();
var spot_102 = await api.Spot.GetTimeAsync();
var spot_103 = await api.Spot.GetExchangeInfoAsync();

// Spot > Market Data Methods (PUBLIC)
var spot_201 = await api.Spot.GetOrderBookAsync("BTCUSDT");
var spot_202 = await api.Spot.GetRecentTradesAsync("BTCUSDT");
var spot_203 = await api.Spot.GetHistoricalTradesAsync("BTCUSDT");
var spot_204 = await api.Spot.GetAggregatedTradesAsync("BTCUSDT");
var spot_205 = await api.Spot.GetKlinesAsync("BTCUSDT", BinanceKlineInterval.OneDay);
var spot_206 = await api.Spot.GetUIKlinesAsync("BTCUSDT", BinanceKlineInterval.OneDay);
var spot_207 = await api.Spot.GetAveragePriceAsync("BTCUSDT");
var spot_211 = await api.Spot.GetTickerAsync("BTCUSDT");
var spot_212 = await api.Spot.GetTickersAsync(["BTCUSDT", "ETHUSDT"]);
var spot_213 = await api.Spot.GetTickersAsync();
var spot_214 = await api.Spot.GetMiniTickerAsync("BTCUSDT");
var spot_215 = await api.Spot.GetMiniTickersAsync(["BTCUSDT", "ETHUSDT"]);
var spot_216 = await api.Spot.GetMiniTickersAsync();
var spot_221 = await api.Spot.GetTradingDayTickerAsync("BTCUSDT");
var spot_222 = await api.Spot.GetTradingDayTickersAsync(["BTCUSDT", "ETHUSDT"]);
var spot_223 = await api.Spot.GetTradingDayTickersAsync();
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
var spot_403 = await api.Spot.GetRateLimitsAsync();
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
var margin_106 = await api.Margin.GetIsolatedMarginTierDataAsync("---SYMBOL---");
var margin_107 = await api.Margin.GetMarginPriceIndexAsync("---SYMBOL---");
var margin_108 = await api.Margin.GetMarginAvaliableInventoryAsync(BinanceMarginInventoryType.Margin);
var margin_109 = await api.Margin.GetLiabilityCoinLeverageBracketInCrossMarginProModeAsync();

// Margin > General Borrow and Repay Methods (PRIVATE)
var margin_201 = await api.Margin.GetFutureHourlyInterestRateAsync(["---ASSET---"], true);
var margin_202 = await api.Margin.GetMarginInterestHistoryAsync();
var margin_203 = await api.Margin.BorrowAsync("---ASSET---", 100.0m);
var margin_204 = await api.Margin.RepayAsync("---ASSET---", 95.0m);
var margin_205 = await api.Margin.GetMarginLoansAsync("---ASSET---");
var margin_206 = await api.Margin.GetMarginInterestRateHistoryAsync("---ASSET---");
var margin_207 = await api.Margin.GetMarginMaxBorrowAmountAsync("---ASSET---");

// Margin > General Trade Methods (PRIVATE)
var margin_301 = await api.Margin.GetMarginForcedLiquidationHistoryAsync();
var margin_302 = await api.Margin.GetSmallLiabilityExchangeAssetsAsync();
var margin_303 = await api.Margin.GetSmallLiabilityExchangeHistoryAsync();
var margin_304 = await api.Margin.CancelAllMarginOrdersAsync("---SYMBOL---");
var margin_305 = await api.Margin.CancelMarginOcoOrderAsync("---SYMBOL---");
var margin_306 = await api.Margin.CancelMarginOrderAsync("---SYMBOL---");
var margin_307 = await api.Margin.PlaceMarginOCOOrderAsync("---SYMBOL---", BinanceOrderSide.Buy, 100.0m, 100.0m, 15.0m);
var margin_308 = await api.Margin.PlaceMarginOrderAsync("---SYMBOL---", BinanceOrderSide.Buy, BinanceSpotOrderType.Market, 100.0m);
var margin_309 = await api.Margin.GetMarginRateLimitsAsync();
var margin_310 = await api.Margin.GetMarginOcoOrdersAsync();
var margin_311 = await api.Margin.GetMarginOrdersAsync("---SYMBOL---");
var margin_312 = await api.Margin.GetMarginOcoOrderAsync();
var margin_313 = await api.Margin.GetMarginOpenOcoOrdersAsync();
var margin_314 = await api.Margin.GetOpenMarginOrdersAsync();
var margin_315 = await api.Margin.GetMarginOrderAsync("---SYMBOL---");
var margin_316 = await api.Margin.GetMarginUserTradesAsync("---SYMBOL---");
var margin_317 = await api.Margin.SmallLiabilityExchangeAsync(["---ASSET---"]);

// Margin > General Transfer Methods (PRIVATE)
var margin_401 = await api.Margin.GetMarginTransfersAsync(BinanceMarginTransferDirection.RollIn);
var margin_402 = await api.Margin.GetMarginMaxTransferAmountAsync("---ASSET---");

// Margin > General Account Methods (PRIVATE)
var margin_501 = await api.Margin.AdjustMaximumLeverageAsync(20);
var margin_502 = await api.Margin.DisableIsolatedMarginAccountAsync("---SYMBOL---");
var margin_503 = await api.Margin.EnableIsolatedMarginAccountAsync("---SYMBOL---");
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
var margin_604 = await api.Margin.StartIsolatedMarginUserStreamAsync("---SYMBOL---");
var margin_605 = await api.Margin.KeepAliveIsolatedMarginUserStreamAsync("---SYMBOL---", "---LISTEN-KEY---");
var margin_606 = await api.Margin.CloseIsolatedMarginUserStreamAsync("---SYMBOL---", "---LISTEN-KEY---");

// TODO: Margin > General Risk Data Stream Methods (PRIVATE)

// Wallet > Capital Methods (PRIVATE)
var wallet_101 = await api.Wallet.GetUserAssetsAsync();
var btcBalance = wallet_101.Data.FirstOrDefault(x => x.Asset == "BTC");
var usdtBalance = wallet_101.Data.FirstOrDefault(x => x.Asset == "USDT");
var wallet_102 = await api.Wallet.WithdrawAsync("---ASSET---", "---ADDRESS---", 0.01m);
var wallet_103 = await api.Wallet.GetWithdrawalsAsync();
var wallet_104 = await api.Wallet.GetWithdrawalsAsync("---ASSET---");
var wallet_105 = await api.Wallet.GetWithdrawalAddressesAsync();
var wallet_106 = await api.Wallet.GetDepositsAsync();
var wallet_107 = await api.Wallet.GetDepositsAsync("---ASSET---");
var wallet_108 = await api.Wallet.GetDepositAddressAsync("BTC");
var wallet_109 = await api.Wallet.GetDepositAddressAsync("USDT");
var wallet_110 = await api.Wallet.GetDepositAddressAsync("USDT", "TRX");

// Wallet > Asset Methods (PRIVATE)
var wallet_201 = await api.Wallet.GetAssetDetailsAsync();
var wallet_202 = await api.Wallet.GetWalletBalancesAsync();
var wallet_203 = await api.Wallet.GetWalletBalancesAsync("BTC");
var wallet_204 = await api.Wallet.GetBalancesAsync();
var wallet_205 = await api.Wallet.GetBalancesAsync("USDT");
var wallet_206 = await api.Wallet.TransferAsync(BinanceWalletUniversalTransferType.FundingToUsdFutures, "USDT", 10.0m);
var wallet_207 = await api.Wallet.GetTransfersAsync(BinanceWalletUniversalTransferType.FundingToUsdFutures);
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
var algo_101 = await api.Algo.Futures.PlaceVolumeParticipationOrderAsync("---SYMBOL---", BinanceOrderSide.Buy, 100.0m, BinanceUrgency.High);
var algo_102 = await api.Algo.Futures.PlaceTimeWeightedAveragePriceOrderAsync("---SYMBOL---", BinanceOrderSide.Buy, 100.0m, 900);
var algo_103 = await api.Algo.Futures.CancelAlgoOrderAsync(1_000_001L);
var algo_104 = await api.Algo.Futures.GetAlgoSubOrdersAsync(1_000_001L);
var algo_105 = await api.Algo.Futures.GetOpenAlgoOrdersAsync();
var algo_106 = await api.Algo.Futures.GetClosedAlgoOrdersAsync();

// Algo > Spot Methods (PRIVATE)
var algo_202 = await api.Algo.Spot.PlaceTimeWeightedAveragePriceOrderAsync("---SYMBOL---", BinanceOrderSide.Buy, 100.0m, 900);
var algo_203 = await api.Algo.Spot.CancelAlgoOrderAsync(1_000_001L);
var algo_204 = await api.Algo.Spot.GetAlgoSubOrdersAsync(1_000_001L);
var algo_205 = await api.Algo.Spot.GetOpenAlgoOrdersAsync();
var algo_206 = await api.Algo.Spot.GetClosedAlgoOrdersAsync();

// USDⓈ-M Futures -> Market Data Methods (PUBLIC)
var futures_101 = await api.UsdFutures.PingAsync();
var futures_102 = await api.UsdFutures.GetTimeAsync();
var futures_103 = await api.UsdFutures.GetExchangeInfoAsync();
var futures_104 = await api.UsdFutures.GetOrderBookAsync("---SYMBOL---");
var futures_105 = await api.UsdFutures.GetRecentTradesAsync("---SYMBOL---");
var futures_106 = await api.UsdFutures.GetHistoricalTradesAsync("---SYMBOL---");
var futures_107 = await api.UsdFutures.GetAggregatedTradesAsync("---SYMBOL---");
var futures_108 = await api.UsdFutures.GetKlinesAsync("---SYMBOL---", BinanceKlineInterval.OneDay);
var futures_109 = await api.UsdFutures.GetContinuousContractKlinesAsync("---SYMBOL---", BinanceFuturesContractType.Perpetual, BinanceKlineInterval.OneDay);
var futures_110 = await api.UsdFutures.GetIndexPriceKlinesAsync("---SYMBOL---", BinanceKlineInterval.OneDay);
var futures_111 = await api.UsdFutures.GetMarkPriceKlinesAsync("---SYMBOL---", BinanceKlineInterval.OneDay);
var futures_112 = await api.UsdFutures.GetPremiumIndexKlinesAsync("---SYMBOL---", BinanceKlineInterval.OneDay);
var futures_113 = await api.UsdFutures.GetMarkPriceAsync("---SYMBOL---");
var futures_114 = await api.UsdFutures.GetMarkPricesAsync();
var futures_115 = await api.UsdFutures.GetFundingRatesAsync("---SYMBOL---");
var futures_116 = await api.UsdFutures.GetFundingInfoAsync();
var futures_117 = await api.UsdFutures.GetTickerAsync("---SYMBOL---");
var futures_118 = await api.UsdFutures.GetTickersAsync();
var futures_119 = await api.UsdFutures.GetPriceAsync("---SYMBOL---");
var futures_120 = await api.UsdFutures.GetPricesAsync();
var futures_121 = await api.UsdFutures.GetBookPriceAsync("---SYMBOL---");
var futures_122 = await api.UsdFutures.GetBookPricesAsync();
var futures_123 = await api.UsdFutures.GetOpenInterestAsync("---SYMBOL---");
var futures_124 = await api.UsdFutures.GetOpenInterestHistoryAsync("---SYMBOL---", BinancePeriodInterval.FourHour);
var futures_125 = await api.UsdFutures.GetTopLongShortPositionRatioAsync("---SYMBOL---", BinancePeriodInterval.FourHour);
var futures_126 = await api.UsdFutures.GetTopLongShortAccountRatioAsync("---SYMBOL---", BinancePeriodInterval.FourHour);
var futures_127 = await api.UsdFutures.GetGlobalLongShortAccountRatioAsync("---SYMBOL---", BinancePeriodInterval.FourHour);
var futures_128 = await api.UsdFutures.GetTakerBuySellVolumeRatioAsync("---SYMBOL---", BinancePeriodInterval.FourHour);
var futures_129 = await api.UsdFutures.GetBasisAsync("---SYMBOL---", BinanceFuturesContractType.Perpetual, BinancePeriodInterval.FourHour);
var futures_130 = await api.UsdFutures.GetCompositeIndexInfoAsync();
var futures_131 = await api.UsdFutures.GetAssetIndexAsync("---SYMBOL---");
var futures_132 = await api.UsdFutures.GetAssetIndexesAsync();

// USDⓈ-M Futures -> Trading Methods (PRIVATE)
var futures_201 = await api.UsdFutures.PlaceOrderAsync("---SYMBOL---", BinanceOrderSide.Buy, BinanceFuturesOrderType.Market, 100.0m);
var futures_202 = await api.UsdFutures.PlaceOrdersAsync([]);
var futures_203 = await api.UsdFutures.ModifyOrderAsync("---SYMBOL---", BinanceOrderSide.Buy, 110.0m, orderId: 1_000_000L);
var futures_204 = await api.UsdFutures.ModifyOrdersAsync([]);
var futures_205 = await api.UsdFutures.GetOrderEditHistoryAsync("---SYMBOL---");
var futures_206 = await api.UsdFutures.CancelOrderAsync("---SYMBOL---", orderId: 1_000_000L);
var futures_207 = await api.UsdFutures.CancelOrdersAsync("---SYMBOL---", [1_000_000L]);
var futures_208 = await api.UsdFutures.CancelAllOrdersAsync("---SYMBOL---");
var futures_209 = await api.UsdFutures.CancelAllOrdersAfterTimeoutAsync("---SYMBOL---", TimeSpan.FromSeconds(15));
var futures_210 = await api.UsdFutures.GetOrderAsync("---SYMBOL---", orderId: 1_000_000L);
var futures_211 = await api.UsdFutures.GetOrdersAsync();
var futures_212 = await api.UsdFutures.GetOpenOrdersAsync();
var futures_213 = await api.UsdFutures.GetOpenOrderAsync("---SYMBOL---");
var futures_214 = await api.UsdFutures.GetForcedOrdersAsync();
var futures_215 = await api.UsdFutures.GetUserTradesAsync("---SYMBOL---");
var futures_216 = await api.UsdFutures.SetMarginTypeAsync("---SYMBOL---", BinanceFuturesMarginType.Isolated);
var futures_217 = await api.UsdFutures.SetPositionModeAsync(true);
var futures_218 = await api.UsdFutures.SetInitialLeverageAsync("---SYMBOL---", 10);
var futures_219 = await api.UsdFutures.SetMultiAssetsModeAsync(true);
var futures_220 = await api.UsdFutures.SetPositionMarginAsync("---SYMBOL---", 100.0m, BinanceFuturesMarginChangeDirectionType.Add);
var futures_221 = await api.UsdFutures.GetPositionInformationAsync();
var futures_222 = await api.UsdFutures.GetPositionsAsync();
var futures_223 = await api.UsdFutures.GetPositionAdlQuantileEstimationAsync();
var futures_224 = await api.UsdFutures.GetMarginChangeHistoryAsync("---SYMBOL---");

// USDⓈ-M Futures -> User Data Stream Methods (PRIVATE)
var futures_301 = await api.UsdFutures.StartUserStreamAsync();
var futures_302 = await api.UsdFutures.KeepAliveUserStreamAsync("---LISTEN-KEY---");
var futures_303 = await api.UsdFutures.StopUserStreamAsync("---LISTEN-KEY---");

// USDⓈ-M Futures -> Account Methods (PRIVATE)
var futures_401 = await api.UsdFutures.GetBalancesAsync();
var futures_402 = await api.UsdFutures.GetAccountInfoV2Async();
var futures_403 = await api.UsdFutures.GetUserCommissionRateAsync("---SYMBOL---");
var futures_404 = await api.UsdFutures.GetAccountConfigurationAsync();
var futures_405 = await api.UsdFutures.GetSymbolConfigurationAsync();
var futures_406 = await api.UsdFutures.GetRateLimitsAsync();
var futures_407 = await api.UsdFutures.GetBracketsAsync();
var futures_408 = await api.UsdFutures.GetMultiAssetsModeAsync();
var futures_409 = await api.UsdFutures.GetPositionModeAsync();
var futures_410 = await api.UsdFutures.GetIncomeHistoryAsync();
var futures_411 = await api.UsdFutures.GetTradingStatusAsync();
var futures_412 = await api.UsdFutures.GetDownloadIdForTransactionHistoryAsync(DateTime.UtcNow.AddDays(-7), DateTime.UtcNow);
var futures_413 = await api.UsdFutures.GetDownloadLinkForTransactionHistoryAsync("---DOWNLOAD-ID---");
var futures_414 = await api.UsdFutures.GetDownloadIdForOrderHistoryAsync(DateTime.UtcNow.AddDays(-7), DateTime.UtcNow);
var futures_415 = await api.UsdFutures.GetDownloadLinkForOrderHistoryAsync("---DOWNLOAD-ID---");
var futures_416 = await api.UsdFutures.GetDownloadIdForTradeHistoryAsync(DateTime.UtcNow.AddDays(-7), DateTime.UtcNow);
var futures_417 = await api.UsdFutures.GetDownloadLinkForTradeHistoryAsync("---DOWNLOAD-ID---");
var futures_418 = await api.UsdFutures.SetBnbBurnStatusAsync(true);
var futures_419 = await api.UsdFutures.GetBnbBurnStatusAsync();

// USDⓈ-M Futures -> Convert Methods (PRIVATE)
var futures_501 = await api.UsdFutures.GetConvertSymbolsAsync();
var futures_502 = await api.UsdFutures.ConvertQuoteRequestAsync("---FROM-ASSET---", "---TO-ASSET---", 100.0m);
var futures_503 = await api.UsdFutures.ConvertAcceptQuoteAsync("---QUOTE-ID---");
var futures_504 = await api.UsdFutures.GetConvertOrderStatusAsync();

// Coin-M Futures -> Market Data Methods (PUBLIC)
var futures_601 = await api.CoinFutures.PingAsync();
var futures_602 = await api.CoinFutures.GetTimeAsync();
var futures_603 = await api.CoinFutures.GetExchangeInfoAsync();
var futures_604 = await api.CoinFutures.GetOrderBookAsync("---SYMBOL---");
var futures_605 = await api.CoinFutures.GetRecentTradesAsync("---SYMBOL---");
var futures_606 = await api.CoinFutures.GetHistoricalTradesAsync("---SYMBOL---");
var futures_607 = await api.CoinFutures.GetAggregatedTradesAsync("---SYMBOL---");
var futures_608 = await api.CoinFutures.GetMarkPricesAsync();
var futures_609 = await api.CoinFutures.GetFundingRatesAsync("---SYMBOL---");
var futures_610 = await api.CoinFutures.GetFundingInfoAsync();
var futures_611 = await api.CoinFutures.GetKlinesAsync("---SYMBOL---", BinanceKlineInterval.OneDay);
var futures_612 = await api.CoinFutures.GetContinuousContractKlinesAsync("---SYMBOL---", BinanceFuturesContractType.Perpetual, BinanceKlineInterval.OneDay);
var futures_613 = await api.CoinFutures.GetIndexPriceKlinesAsync("---SYMBOL---", BinanceKlineInterval.OneDay);
var futures_614 = await api.CoinFutures.GetMarkPriceKlinesAsync("---SYMBOL---", BinanceKlineInterval.OneDay);
var futures_615 = await api.CoinFutures.GetPremiumIndexKlinesAsync("---SYMBOL---", BinanceKlineInterval.OneDay);
var futures_616 = await api.CoinFutures.GetTickersAsync();
var futures_617 = await api.CoinFutures.GetPricesAsync();
var futures_618 = await api.CoinFutures.GetBookPricesAsync();
var futures_619 = await api.CoinFutures.GetOpenInterestAsync("---SYMBOL---");
var futures_620 = await api.CoinFutures.GetOpenInterestHistoryAsync("---SYMBOL---", BinanceFuturesContractType.PerpetualDelivering, BinancePeriodInterval.FourHour);
var futures_621 = await api.CoinFutures.GetTopLongShortPositionRatioAsync("---SYMBOL---", BinancePeriodInterval.FourHour);
var futures_622 = await api.CoinFutures.GetTopLongShortAccountRatioAsync("---SYMBOL---", BinancePeriodInterval.FourHour);
var futures_623 = await api.CoinFutures.GetGlobalLongShortAccountRatioAsync("---SYMBOL---", BinancePeriodInterval.FourHour);
var futures_624 = await api.CoinFutures.GetTakerBuySellVolumeRatioAsync("---SYMBOL---", BinanceFuturesContractType.PerpetualDelivering, BinancePeriodInterval.FourHour);
var futures_625 = await api.CoinFutures.GetBasisAsync("---SYMBOL---", BinanceFuturesContractType.Perpetual, BinancePeriodInterval.FourHour);

// Coin-M Futures -> Trading Methods (PRIVATE)
var futures_701 = await api.CoinFutures.PlaceOrderAsync("---SYMBOL---", BinanceOrderSide.Buy, BinanceFuturesOrderType.Market, 100.0m);
var futures_702 = await api.CoinFutures.PlaceOrdersAsync([]);
var futures_703 = await api.CoinFutures.CancelOrderAsync("---SYMBOL---", orderId: 1_000_000L);
var futures_704 = await api.CoinFutures.CancelOrdersAsync("---SYMBOL---", [1_000_000L]);
var futures_705 = await api.CoinFutures.CancelAllOrdersAsync("---SYMBOL---");
var futures_706 = await api.CoinFutures.CancelAllOrdersAfterTimeoutAsync("---SYMBOL---", TimeSpan.FromSeconds(15));
var futures_707 = await api.CoinFutures.GetOrderAsync("---SYMBOL---", orderId: 1_000_000L);
var futures_708 = await api.CoinFutures.GetOrdersAsync("---SYMBOL---");
var futures_709 = await api.CoinFutures.GetOpenOrdersAsync();
var futures_710 = await api.CoinFutures.GetOpenOrderAsync("---SYMBOL---");
var futures_711 = await api.CoinFutures.GetForcedOrdersAsync();
var futures_712 = await api.CoinFutures.GetUserTradesAsync("---SYMBOL---");
var futures_713 = await api.CoinFutures.GetPositionsAsync();
var futures_714 = await api.CoinFutures.SetPositionModeAsync(true);
var futures_715 = await api.CoinFutures.SetMarginTypeAsync("---SYMBOL---", BinanceFuturesMarginType.Isolated);
var futures_716 = await api.CoinFutures.SetInitialLeverageAsync("---SYMBOL---", 10);
var futures_717 = await api.CoinFutures.GetPositionAdlQuantileEstimationAsync();
var futures_718 = await api.CoinFutures.SetPositionMarginAsync("---SYMBOL---", 100.0m, BinanceFuturesMarginChangeDirectionType.Add);
var futures_719 = await api.CoinFutures.GetMarginChangeHistoryAsync("---SYMBOL---");

// Coin-M Futures -> User Data Stream Methods (PRIVATE)
var futures_801 = await api.CoinFutures.StartUserStreamAsync();
var futures_802 = await api.CoinFutures.KeepAliveUserStreamAsync("---LISTEN-KEY---");
var futures_803 = await api.CoinFutures.StopUserStreamAsync("---LISTEN-KEY---");

// Coin-M Futures -> Account Methods (PRIVATE)
var futures_901 = await api.CoinFutures.GetBalancesAsync();
var futures_902 = await api.CoinFutures.GetUserCommissionRateAsync("---SYMBOL---");
var futures_903 = await api.CoinFutures.GetAccountInfoAsync();
var futures_904 = await api.CoinFutures.GetPositionModeAsync();
var futures_905 = await api.CoinFutures.GetIncomeHistoryAsync();
var futures_906 = await api.CoinFutures.GetDownloadIdForTransactionHistoryAsync(DateTime.UtcNow.AddDays(-7), DateTime.UtcNow);
var futures_907 = await api.CoinFutures.GetDownloadLinkForTransactionHistoryAsync("---DOWNLOAD-ID---");
var futures_908 = await api.CoinFutures.GetDownloadIdForOrderHistoryAsync(DateTime.UtcNow.AddDays(-7), DateTime.UtcNow);
var futures_909 = await api.CoinFutures.GetDownloadLinkForOrderHistoryAsync("---DOWNLOAD-ID---");
var futures_910 = await api.CoinFutures.GetDownloadIdForTradeHistoryAsync(DateTime.UtcNow.AddDays(-7), DateTime.UtcNow);
var futures_911 = await api.CoinFutures.GetDownloadLinkForTradeHistoryAsync("---DOWNLOAD-ID---");

// Futures Data Methods (PRIVATE)
var futures_1001 = await api.FuturesData.GetFuturesDataLinkAsync("---SYMBOL---", BinanceFuturesDataType.TickData, DateTime.UtcNow.AddMonths(-3), DateTime.UtcNow);

// European Options -> General Methods (PUBLIC)
var options_101 = await api.Options.PingAsync();
var options_102 = await api.Options.GetTimeAsync();
var options_103 = await api.Options.GetExchangeInfoAsync();

// European Options -> Market Data Methods (PUBLIC)
var options_201 = await api.Options.GetTickersAsync();
var options_202 = await api.Options.GetTickersAsync("---SYMBOL---");
var options_203 = await api.Options.GetPublicExerciseRecordsAsync();
var options_204 = await api.Options.GetOpenInterestAsync("---UNDERLYING---", DateTime.UtcNow);
var options_205 = await api.Options.GetOrderBookAsync("---SYMBOL---");
var options_206 = await api.Options.GetRecentTradesAsync("---SYMBOL---");
var options_207 = await api.Options.GetRecentBlockTradesAsync("---SYMBOL---");
var options_208 = await api.Options.GetIndexPriceAsync("---UNDERLYING---");
var options_209 = await api.Options.GetKlinesAsync("---SYMBOL---", BinanceKlineInterval.OneDay);
var options_210 = await api.Options.GetHistoricalTradesAsync("---SYMBOL---");
var options_211 = await api.Options.GetMarkPriceAsync("---SYMBOL---");

// European Options -> Account Methods (PRIVATE)
var options_301 = await api.Options.GetAccountAsync();
var options_302 = await api.Options.GetAccountFundingFlowAsync("---CURRENCY---");
var options_303 = await api.Options.GetTransactionHistoryDownloadIdAsync();
var options_304 = await api.Options.GetTransactionHistoryDownloadLinkAsync(1_000_001);

// European Options -> Trade Methods (PRIVATE)
var options_401 = await api.Options.PlaceOrderAsync("---SYMBOL---", BinanceOrderSide.Buy, BinanceOptionsOrderType.Limit, 100.0m, 1.10m, BinanceTimeInForce.GoodTillCanceled);
var options_402 = await api.Options.PlaceOrdersAsync([]);
var options_403 = await api.Options.CancelOrderAsync("---SYMBOL---",1_000_001);
var options_404 = await api.Options.CancelOrdersAsync("---SYMBOL---", []);
var options_405 = await api.Options.CancelOrdersByUnderlyingAsync("---UNDERLYING---");
var options_406 = await api.Options.CancelOrdersBySymbolAsync("---SYMBOL---");
var options_407 = await api.Options.GetOrderAsync("---SYMBOL---", 1_000_001);
var options_408 = await api.Options.GetOrdersHistoryAsync("---SYMBOL---");
var options_409 = await api.Options.GetOpenOrdersAsync();
var options_410 = await api.Options.GetPositionsAsync();
var options_411 = await api.Options.GetUserExerciseRecordsAsync();
var options_412 = await api.Options.GetUserTradesAsync();

// TODO: European Options -> User Data Stream Methods (PRIVATE)
// TODO: European Options -> Market Maker -> Endpoints (PRIVATE)
// TODO: European Options -> Market Maker -> Block Trade Methods (PRIVATE)

// Copy Trading -> Futures Methods (PRIVATE)
var copy_101 = await api.CopyTrading.Futures.GetLeadTraderStatusAsync();
var copy_102 = await api.CopyTrading.Futures.GetLeadTradingSymbolsAsync();

// Sub-Account -> Account Methods (PRIVATE)
var subaccount_101 = await api.SubAccount.CreateVirtualSubAccountAsync("---SUBACCOUNT-STRING---");
var subaccount_102 = await api.SubAccount.GetSubAccountsAsync();
var subaccount_103 = await api.SubAccount.EnableFuturesAsync("---SUBACCOUNT-EMAIL---");
var subaccount_104 = await api.SubAccount.EnableMarginAsync("---SUBACCOUNT-EMAIL---");
var subaccount_105 = await api.SubAccount.EnableOptionsAsync("---SUBACCOUNT-EMAIL---");
var subaccount_106 = await api.SubAccount.EnableBlvtAsync("---SUBACCOUNT-EMAIL---", true);
var subaccount_107 = await api.SubAccount.GetSubAccountStatusAsync();
var subaccount_108 = await api.SubAccount.GetFuturesPositionRiskAsync("---SUBACCOUNT-EMAIL---");
var subaccount_109 = await api.SubAccount.GetFuturesPositionRiskAsync(BinanceFuturesType.UsdtMarginedFutures, "---SUBACCOUNT-EMAIL---");
var subaccount_110 = await api.SubAccount.GetTransactionStatisticsAsync("---SUBACCOUNT-EMAIL---");

// Sub-Account -> API Management Methods (PRIVATE)
var subaccount_201 = await api.SubAccount.GetApiKeyIpRestrictionAsync("---SUBACCOUNT-EMAIL---", "---SUBACCOUNT-APIKEY---");
var subaccount_202 = await api.SubAccount.RemoveApiKeyIpRestrictionAsync("---SUBACCOUNT-EMAIL---", "---SUBACCOUNT-APIKEY---", null);
var subaccount_203 = await api.SubAccount.SetApiKeyIpRestrictionAsync("---SUBACCOUNT-EMAIL---", "---SUBACCOUNT-APIKEY---", false, null);

// Sub-Account -> Asset Management Methods (PRIVATE)
var subaccount_301 = await api.SubAccount.FuturesTransferAsync("---SUBACCOUNT-EMAIL---", "---ASSET---", 100.0m, BinanceSubAccountFuturesTransferType.FromCoinFuturesToSpot);
var subaccount_302 = await api.SubAccount.GetFuturesDetailsAsync("---SUBACCOUNT-EMAIL---");
var subaccount_303 = await api.SubAccount.GetFuturesDetailsAsync(BinanceFuturesType.UsdtMarginedFutures, "---SUBACCOUNT-EMAIL---");
var subaccount_304 = await api.SubAccount.GetMarginDetailsAsync("---SUBACCOUNT-EMAIL---");
var subaccount_305 = await api.SubAccount.GetDepositAddressAsync("---SUBACCOUNT-EMAIL---", "---ASSET---");
var subaccount_306 = await api.SubAccount.GetDepositsAsync("---SUBACCOUNT-EMAIL---");
var subaccount_307 = await api.SubAccount.GetFuturesSummaryAsync();
var subaccount_308 = await api.SubAccount.GetFuturesSummaryAsync();
var subaccount_309 = await api.SubAccount.GetMarginSummaryAsync();
var subaccount_310 = await api.SubAccount.MarginTransferAsync("---SUBACCOUNT-EMAIL---", "---ASSET---", 100.0m, BinanceSubAccountMarginTransferType.FromSubAccountSpotToSubAccountMargin);
var subaccount_311 = await api.SubAccount.GetBalancesAsync("---SUBACCOUNT-EMAIL---");
var subaccount_312 = await api.SubAccount.GetFuturesTransferHistoryAsync("---SUBACCOUNT-EMAIL---", BinanceFuturesType.UsdtMarginedFutures);
var subaccount_313 = await api.SubAccount.GetSpotTransferHistoryAsync();
var subaccount_314 = await api.SubAccount.GetSpotSummaryAsync();
var subaccount_315 = await api.SubAccount.GetUniversalTransferHistoryAsync();
var subaccount_316 = await api.SubAccount.FuturesAssetTransferAsync("---FROM-EMAIL---", "---TO-EMAIL---", BinanceFuturesType.UsdtMarginedFutures, "---ASSET---", 100.0m);
var subaccount_317 = await api.SubAccount.GetTransferHistoryAsync();
var subaccount_318 = await api.SubAccount.TransferSubAccountToMasterAsync("---ASSET---", 100.0m);
var subaccount_319 = await api.SubAccount.TransferSubAccountToSubAccountAsync("---SUBACCOUNT-EMAIL---", "---ASSET---", 100.0m);
var subaccount_320 = await api.SubAccount.UniversalTransferAsync(BinanceSubAccountTransferAccountType.Spot, BinanceSubAccountTransferAccountType.UsdtFuture, "---ASSET---", 100.0m);

// Convert -> Market Data Methods (PRIVATE)
var convert_101 = await api.Convert.GetPairsAsync();
var convert_102 = await api.Convert.GetAssetsAsync();

// Crypto Loan -> Flexible Rate -> Market Data Methods (PRIVATE)
var loan_103 = await api.CryptoLoan.Flexible.GetLoanableAssetsAsync("---ASSET---");
var loan_101 = await api.CryptoLoan.Flexible.GetCollateralAssetsAsync("---ASSET---");
var loan_102 = await api.CryptoLoan.Flexible.GetInterestRateHistoryAsync("---ASSET---");

// Crypto Loan -> Flexible Rate -> Trade Methods (PRIVATE)
var loan_201 = await api.CryptoLoan.Flexible.BorrowAsync("---LOAN-ASSET---", "---COLLATERAL-ASSET---", 100.0m);
var loan_202 = await api.CryptoLoan.Flexible.RepayAsync("---LOAN-ASSET---", "---COLLATERAL-ASSET---", 100.0m);
var loan_203 = await api.CryptoLoan.Flexible.AdjustAsync("---LOAN-ASSET---", "---COLLATERAL-ASSET---", 100.0m, BinanceCryptoLoanAdjustmentDirection.Additional);

// Crypto Loan -> Flexible Rate -> User Information Methods (PRIVATE)
var loan_301 = await api.CryptoLoan.Flexible.GetAdjustmentHistoryAsync();
var loan_302 = await api.CryptoLoan.Flexible.GetCollateralRepayRateAsync("---LOAN-ASSET---", "---COLLATERAL-ASSET---");
var loan_303 = await api.CryptoLoan.Flexible.GetBorrowHistoryAsync();
var loan_304 = await api.CryptoLoan.Flexible.GetOpenBorrowOrdersAsync();
var loan_305 = await api.CryptoLoan.Flexible.GetLiquidationHistoryAsync();
var loan_306 = await api.CryptoLoan.Flexible.GetRepayHistoryAsync();

// Crypto Loan -> Stable Rate -> Market Data Methods (PRIVATE)
var loan_401 = await api.CryptoLoan.Stable.GetIncomeHistoryAsync("---ASSET---");
var loan_402 = await api.CryptoLoan.Stable.GetLoanableAssetsAsync();
var loan_403 = await api.CryptoLoan.Stable.GetCollateralAssetsAsync();

// Crypto Loan -> Stable Rate -> Trade Methods (PRIVATE)
var loan_501 = await api.CryptoLoan.Stable.BorrowAsync("---LOAN-ASSET---", "---COLLATERAL-ASSET---", 10, 100.0m);
var loan_502 = await api.CryptoLoan.Stable.RepayAsync(1_000_000, 100.0m);
var loan_503 = await api.CryptoLoan.Stable.AdjustAsync(1_000_000, 100.0m, BinanceCryptoLoanAdjustmentDirection.Additional);

// Crypto Loan -> Stable Rate -> User Information Methods (PRIVATE)
var loan_601 = await api.CryptoLoan.Stable.GetAdjustmentHistoryAsync();
var loan_602 = await api.CryptoLoan.Stable.GetCollateralRepayRateAsync("---LOAN-ASSET---", "---COLLATERAL-ASSET---", 100.0m);
var loan_603 = await api.CryptoLoan.Stable.GetBorrowHistoryAsync();
var loan_604 = await api.CryptoLoan.Stable.GetOpenBorrowOrdersAsync();
var loan_605 = await api.CryptoLoan.Stable.GetRepayHistoryAsync();
var loan_606 = await api.CryptoLoan.Stable.CustomizeMarginCallAsync(100.0m);

// C2C Methods (PRIVATE)
var c2c_101 = await api.C2C.GetHistoryAsync();

// Fiat Methods (PRIVATE)
var fiat_101 = await api.Fiat.GetDepositHistoryAsync();
var fiat_102 = await api.Fiat.GetWithdrawalHistoryAsync();
var fiat_103 = await api.Fiat.GetPaymentHistoryAsync(BinanceFiatPaymentType.Buy);

// Convert -> Trade Methods (PRIVATE)
var convert_201 = await api.Convert.QuoteRequestAsync("---FROM-ASSET---", "---TO-ASSET---");
var convert_202 = await api.Convert.AcceptQuoteAsync("---QUOTE-ID---");
var convert_203 = await api.Convert.GetHistoryAsync(DateTime.UtcNow.AddDays(-7), DateTime.UtcNow);
var convert_204 = await api.Convert.GetStatusAsync("---ORDER-ID---");
var convert_205 = await api.Convert.PlaceLimitOrderAsync("---BASE-ASSET---", "---QUOTE-ASSET---", 100.0m, BinanceOrderSide.Buy, BinanceConvertExpiredTime.OneDay, 10.0m);
var convert_206 = await api.Convert.CancelLimitOrderAsync("---ORDER-ID---");
var convert_207 = await api.Convert.GetLimitOrdersAsync();

// Institutional Loan Methods (PRIVATE)
var insloan_101 = await api.InstitutionalLoan.GetLoanGroupsAsync();
var insloan_102 = await api.InstitutionalLoan.GetLoanGroupAsync(1_000_000);

// Binance Link -> Exchange Link -> Account Methods (PRIVATE)
var exclink_101 = await api.Link.ExchangeLink.CreateSubAccountAsync();
var exclink_102 = await api.Link.ExchangeLink.GetSubAccountsAsync();
var exclink_103 = await api.Link.ExchangeLink.EnableFuturesAsync("---SUBACCOUNT-ID---");
var exclink_104 = await api.Link.ExchangeLink.EnableMarginAsync("---SUBACCOUNT-ID---");
var exclink_105 = await api.Link.ExchangeLink.EnableLeverageTokenAsync("---SUBACCOUNT-ID---");
var exclink_106 = await api.Link.ExchangeLink.CreateApiKeyAsync("---SUBACCOUNT-ID---", true, true, true);
var exclink_107 = await api.Link.ExchangeLink.SetApiKeyPermissionAsync("---SUBACCOUNT-ID---", "---API-KEY---", true, true, true);
var exclink_108 = await api.Link.ExchangeLink.SetApiKeyIpRestrictionAsync("---SUBACCOUNT-ID---", "---API-KEY---", BinanceLinkApiKeyIpRestriction.IpUnrestricted);
var exclink_109 = await api.Link.ExchangeLink.DeleteApiKeyIpRestrictionAsync("---SUBACCOUNT-ID---", "---API-KEY---", "---IP-ADDRESS---");
var exclink_110 = await api.Link.ExchangeLink.DeleteApiKeyAsync("---SUBACCOUNT-ID---", "---API-KEY---");
var exclink_111 = await api.Link.ExchangeLink.GetApiKeyIpRestrictionAsync("---SUBACCOUNT-ID---", "---API-KEY---");
var exclink_112 = await api.Link.ExchangeLink.GetApiKeyAsync("---SUBACCOUNT-ID---");
var exclink_113 = await api.Link.ExchangeLink.GetBrokerAccountAsync();
var exclink_114 = await api.Link.ExchangeLink.SetBnbBurnForSpotAndMarginAsync("---SUBACCOUNT-ID---", true);
var exclink_115 = await api.Link.ExchangeLink.SetBnbBurnForMarginInterestAsync("---SUBACCOUNT-ID---", true);
var exclink_116 = await api.Link.ExchangeLink.GetBnbBurnStatusAsync("---SUBACCOUNT-ID---");

// Binance Link -> Exchange Link -> Asset Methods (PRIVATE)
var exclink_201 = await api.Link.ExchangeLink.TransferAsync("---ASSET---", 100.0m, "---FROM-ID---", "---TO-ID---");
var exclink_202 = await api.Link.ExchangeLink.GetTransfersAsync();
var exclink_203 = await api.Link.ExchangeLink.FuturesTransferAsync("---ASSET---", 100.0m, BinanceFuturesType.CoinMarginedFutures, "---FROM-ID---", "---TO-ID---");
var exclink_204 = await api.Link.ExchangeLink.GetFuturesTransfersAsync("---SUBACCOUNT-ID---", BinanceFuturesType.CoinMarginedFutures);
var exclink_205 = await api.Link.ExchangeLink.GetDepositsAsync();
var exclink_206 = await api.Link.ExchangeLink.GetSpotAssetInfoAsync();
var exclink_207 = await api.Link.ExchangeLink.GetMarginAssetInfoAsync();
var exclink_208 = await api.Link.ExchangeLink.GetFuturesAssetInfoAsync(BinanceFuturesType.CoinMarginedFutures, "---SUBACCOUNT-ID---");
var exclink_209 = await api.Link.ExchangeLink.UniversalTransferAsync("---ASSET---", 100.0m, "---FROM-ID---", BinanceLinkAccountType.FuturesCoin, "---TO-ID---", BinanceLinkAccountType.FuturesCoin);
var exclink_210 = await api.Link.ExchangeLink.GetUniversalTransfersAsync();

// Binance Link -> Exchange Link -> Fee Methods (PRIVATE)
var exclink_301 = await api.Link.ExchangeLink.SetCommissionAsync("---SUBACCOUNT-ID---", 100, 150);
var exclink_302 = await api.Link.ExchangeLink.SetFuturesCommissionAdjustmentAsync("---SUBACCOUNT-ID---", "---SYMBOL---", 100, 150);
var exclink_303 = await api.Link.ExchangeLink.GetFuturesCommissionAdjustmentAsync("---SUBACCOUNT-ID---");
var exclink_304 = await api.Link.ExchangeLink.SetCoinFuturesCommissionAdjustmentAsync("---SUBACCOUNT-ID---", "---PAIR---", 100, 150);
var exclink_305 = await api.Link.ExchangeLink.GetCoinFuturesCommissionAdjustmentAsync("---SUBACCOUNT-ID---");
var exclink_306 = await api.Link.ExchangeLink.GetBrokerCommissionRebatesAsync("---SUBACCOUNT-ID---");
var exclink_307 = await api.Link.ExchangeLink.GetBrokerFuturesCommissionRebatesAsync(BinanceFuturesType.CoinMarginedFutures, DateTime.UtcNow.AddDays(-7), DateTime.UtcNow);

// Binance Link -> Link and Trade -> Spot Methods (PRIVATE)
var linktrade_101 = await api.Link.LinkAndTrade.Spot.GetIfNewUserAsync("---AGENT-CODE---");
var linktrade_102 = await api.Link.LinkAndTrade.Spot.SetCustomerIdByPartnerAsync("---EMAIL---", "---CUSTOMER-ID---");
var linktrade_103 = await api.Link.LinkAndTrade.Spot.GetCustomerIdByPartnerAsync("---EMAIL---", "---CUSTOMER-ID---");
var linktrade_104 = await api.Link.LinkAndTrade.Spot.SetCustomerIdByClientAsync("---CUSTOMER-ID---", "---AGENT-CODE---");
var linktrade_105 = await api.Link.LinkAndTrade.Spot.GetCustomerIdByClientAsync("---AGENT-CODE---");
var linktrade_106 = await api.Link.LinkAndTrade.Spot.GetRebateHistoryByPartnerAsync(DateTime.UtcNow.AddDays(-7), DateTime.UtcNow);
var linktrade_107 = await api.Link.LinkAndTrade.Spot.GetRebateHistoryByClientAsync();

// Binance Link -> Link and Trade -> Futures Methods (PRIVATE)
var linktrade_201 = await api.Link.LinkAndTrade.Futures.GetIfNewUserAsync("---BROKER-ID---");
var linktrade_202 = await api.Link.LinkAndTrade.Futures.SetCustomerIdByPartnerAsync("---EMAIL---", "---CUSTOMER-ID---");
var linktrade_203 = await api.Link.LinkAndTrade.Futures.GetCustomerIdByPartnerAsync("---EMAIL---", "---CUSTOMER-ID---");
var linktrade_204 = await api.Link.LinkAndTrade.Futures.SetCustomerIdByClientAsync("---CUSTOMER-ID---", "---AGENT-CODE---");
var linktrade_205 = await api.Link.LinkAndTrade.Futures.GetCustomerIdByClientAsync("---AGENT-CODE---");
var linktrade_206 = await api.Link.LinkAndTrade.Futures.GetIncomeHistoryAsync();
var linktrade_207 = await api.Link.LinkAndTrade.Futures.GetTraderNumberAsync();
var linktrade_208 = await api.Link.LinkAndTrade.Futures.GetRebateDataOverviewAsync();
var linktrade_209 = await api.Link.LinkAndTrade.Futures.GetUserTradeVolumeAsync();
var linktrade_210 = await api.Link.LinkAndTrade.Futures.GetUserRebateVolumeAsync();
var linktrade_211 = await api.Link.LinkAndTrade.Futures.GetTraderDetailsAsync();

// Binance Link -> Link and Trade -> Portfolio Margin Methods (PRIVATE)
var linktrade_301 = await api.Link.LinkAndTrade.PortfolioMargin.GetIfNewUserAsync("---BROKER-ID---");
var linktrade_302 = await api.Link.LinkAndTrade.PortfolioMargin.SetCustomerIdByClientAsync("---CUSTOMER-ID---", "---AGENT-CODE---");
var linktrade_303 = await api.Link.LinkAndTrade.PortfolioMargin.GetCustomerIdByClientAsync("---AGENT-CODE---");

// Binance Link -> Link and Trade -> Fast API Methods (PRIVATE)
var linktrade_401 = await api.Link.LinkAndTrade.FastApi.GetUserStatusAsync("---ACCESS-TOKEN---");
var linktrade_402 = await api.Link.LinkAndTrade.FastApi.CreateApiKeyForUserAsync("---ACCESS-TOKEN---", "---API-NAME---", true, true, true, true, "---PUBLIC-KEY---");

// Mining Methods (PRIVATE)
var mining_101 = await api.Mining.GetAlgorithmsAsync(); // PUBLIC
var mining_102 = await api.Mining.GetCoinsAsync(); // PUBLIC
var mining_103 = await api.Mining.GetHashrateResalesAsync();
var mining_104 = await api.Mining.GetWorkersAsync("---ALGORITHM---", "---USERNAME---");
var mining_105 = await api.Mining.GetWorkerDetailsAsync("---ALGORITHM---", "---USERNAME---", "---WORKER---");
var mining_106 = await api.Mining.GetOtherRevenuesAsync("---ALGORITHM---", "---USERNAME---");
var mining_107 = await api.Mining.GetMiningRevenuesAsync("---ALGORITHM---", "---USERNAME---");
var mining_108 = await api.Mining.CancelHashrateResaleRequestAsync(1_000_000, "---USERNAME---");
var mining_109 = await api.Mining.GetHashrateResaleDetailsAsync(1_000_000, "---USERNAME---");
var mining_110 = await api.Mining.GetEarningsAsync("---ALGORITHM---");
var mining_111 = await api.Mining.GetStatisticsAsync("---ALGORITHM---", "---USERNAME---");
var mining_112 = await api.Mining.PlaceHashrateResaleRequestAsync("---ALGORITHM---", "---USERNAME---", DateTime.UtcNow, DateTime.UtcNow.AddDays(7), "---TO-USER---", 10.0m);
var mining_113 = await api.Mining.GetAccountsAsync("---ALGORITHM---", "---USERNAME---");

// NFT Methods (PRIVATE)
var nft_101 = await api.NFT.GetDepositsAsync();
var nft_102 = await api.NFT.GetWithdrawalsAsync();
var nft_103 = await api.NFT.GetTransactionsAsync(BinanceNftOrderType.PurchaseOrder);
var nft_104 = await api.NFT.GetAssetsAsync();

// ETH Staking -> Account Methods (PRIVATE)
var ethstaking_101 = await api.Staking.ETH.GetAccountAsync();
var ethstaking_102 = await api.Staking.ETH.GetQuotaAsync();

// ETH Staking -> Staking Methods (PRIVATE)
var ethstaking_201 = await api.Staking.ETH.StakeAsync(100.0m);
var ethstaking_202 = await api.Staking.ETH.RedeemAsync(100.0m);
var ethstaking_203 = await api.Staking.ETH.WrapAsync(100.0m);

// ETH Staking -> History Methods (PRIVATE)
var ethstaking_301 = await api.Staking.ETH.GetStakingHistoryAsync();
var ethstaking_302 = await api.Staking.ETH.GetRedemptionHistoryAsync();
var ethstaking_303 = await api.Staking.ETH.GetRewardsHistoryAsync();
var ethstaking_304 = await api.Staking.ETH.GetWbEthRewardsHistoryAsync();
var ethstaking_305 = await api.Staking.ETH.GetWbEthRateHistoryAsync();
var ethstaking_306 = await api.Staking.ETH.GetWbEthWrapHistoryAsync();
var ethstaking_307 = await api.Staking.ETH.GetWbEthUnwrapHistoryAsync();

// SOL Staking -> Account Methods (PRIVATE)
var solstaking_101 = await api.Staking.SOL.GetAccountAsync();
var solstaking_102 = await api.Staking.SOL.GetQuotaAsync();

// SOL Staking -> Staking Methods (PRIVATE)
var solstaking_201 = await api.Staking.SOL.StakeAsync(100.0m);
var solstaking_202 = await api.Staking.SOL.RedeemAsync(100.0m);
var solstaking_203 = await api.Staking.SOL.ClaimAsync();

// SOL Staking -> History Methods (PRIVATE)
var solstaking_301 = await api.Staking.SOL.GetStakingHistoryAsync();
var solstaking_302 = await api.Staking.SOL.GetRedemptionHistoryAsync();
var solstaking_303 = await api.Staking.SOL.GetBnSolRewardsHistoryAsync();
var solstaking_304 = await api.Staking.SOL.GetBnSolRateHistoryAsync();
var solstaking_305 = await api.Staking.SOL.GetBoostRewardsHistoryAsync(BinanceSolStakingRewardType.Claim);
var solstaking_306 = await api.Staking.SOL.GetUnclaimedRewardsAsync();

// Dual Investment -> Market Data Methods (PUBLIC)
var dual_101 = await api.DualInvestment.GetProductsAsync( BinanceOptionsSide.Put, "BNB", "USDT");

// Dual Investment -> Market Data Methods (PRIVATE)
var dual_201 = await api.DualInvestment.SubscribeAsync(1_000_000, 1_000_000, 100.0m, BinanceDualInvestmentAutoCompoundPlan.Standard);
var dual_202 = await api.DualInvestment.GetPositionsAsync();
var dual_203 = await api.DualInvestment.GetAccountAsync();
var dual_204 = await api.DualInvestment.SetAutoCompoundPlanAsync(1_000_000, BinanceDualInvestmentAutoCompoundPlan.Advanced);

// Pay History Methods (PRIVATE)
var pay_101 = await api.Pay.History.GetHistoryAsync();

// Rebate Methods (PRIVATE)
var rebate_101 = await api.Rebate.GetSpotRebateHistoryAsync();

// Simple Earn -> Flexible -> Account Methods (PRIVATE)
var flexible_101 = await api.SimpleEarn.Flexible.GetAccountAsync();
var flexible_102 = await api.SimpleEarn.Flexible.GetProductsAsync();
var flexible_103 = await api.SimpleEarn.Flexible.GetPositionsAsync();
var flexible_104 = await api.SimpleEarn.Flexible.GetQuotaAsync("---PRODUCT-ID---");

// Simple Earn -> Flexible -> Earn Methods (PRIVATE)
var flexible_201 = await api.SimpleEarn.Flexible.SubscribeAsync("---PRODUCT-ID---", 100.0m);
var flexible_202 = await api.SimpleEarn.Flexible.RedeemAsync("---PRODUCT-ID---");
var flexible_203 = await api.SimpleEarn.Flexible.SetAutoSubscribeAsync("---PRODUCT-ID---", true);
var flexible_204 = await api.SimpleEarn.Flexible.GetSubscriptionPreviewAsync("---PRODUCT-ID---", 100.0m);

// Simple Earn -> Flexible -> Earn History (PRIVATE)
var flexible_301 = await api.SimpleEarn.Flexible.GetSubscriptionsAsync("---PRODUCT-ID---");
var flexible_302 = await api.SimpleEarn.Flexible.GetRedemptionsAsync("---PRODUCT-ID---");
var flexible_303 = await api.SimpleEarn.Flexible.GetRewardsAsync(BinanceSimpleEarnRewardType.All);
var flexible_304 = await api.SimpleEarn.Flexible.GetCollateralsAsync("---PRODUCT-ID---");
var flexible_305 = await api.SimpleEarn.Flexible.GetRatesAsync("---PRODUCT-ID---");

// Simple Earn -> Locked -> Account Methods (PRIVATE)
var locked_101 = await api.SimpleEarn.Locked.GetAccountAsync();
var locked_102 = await api.SimpleEarn.Locked.GetProductsAsync();
var locked_103 = await api.SimpleEarn.Locked.GetPositionsAsync();
var locked_104 = await api.SimpleEarn.Locked.GetQuotaAsync("---PRODUCT-ID---");

// Simple Earn -> Locked -> Earn Methods (PRIVATE)
var locked_201 = await api.SimpleEarn.Locked.SubscribeAsync("---PROJECT-ID---", 100.0m);
var locked_202 = await api.SimpleEarn.Locked.RedeemAsync("---POSITION-ID---");
var locked_203 = await api.SimpleEarn.Locked.SetAutoSubscribeAsync("---POSITION-ID---", true);
var locked_204 = await api.SimpleEarn.Locked.GetSubscriptionPreviewAsync("---PROJECT-ID---", 100.0m);
var locked_205 = await api.SimpleEarn.Locked.SetRedeemOptionAsync("---POSITION-ID---", BinanceSimpleEarnRedeemOption.Spot);

// Simple Earn -> Locked -> Earn History (PRIVATE)
var locked_301 = await api.SimpleEarn.Locked.GetSubscriptionsAsync();
var locked_302 = await api.SimpleEarn.Locked.GetRedemptionsAsync();
var locked_303 = await api.SimpleEarn.Locked.GetRewardsAsync();

// Auto Invest -> Market Data (PRIVATE)
var autoinvest_101 = await api.AutoInvest.GetAssetsAsync();
var autoinvest_102 = await api.AutoInvest.GetSourceAssetsAsync("---USAGE-TYPE---");
var autoinvest_103 = await api.AutoInvest.GetTargetAssetsAsync();
var autoinvest_104 = await api.AutoInvest.GetTargetAssetRoisAsync("---ASSET---", AutoInvestRoiType.OneYear);
var autoinvest_105 = await api.AutoInvest.GetIndexInfoAsync("---INDEX-ID---");
var autoinvest_106 = await api.AutoInvest.GetPlansAsync(BinanceAutoInvestPlanType.All);

// Auto Invest -> Trade (PRIVATE)
var autoinvest_201 = await api.AutoInvest.OneTimeTransactionAsync("---USAGE-TYPE---", "---REQUEST-ID---", 100.0m, "---ASSET---", true, 1_000_000L, []);
var autoinvest_202 = await api.AutoInvest.SetPlanStatusAsync(1_000_000L, BinanceAutoInvestPlanStatus.Ongoing);
var autoinvest_203 = await api.AutoInvest.SetPlanAsync("---PLAN-ID---", 100.0m, AutoInvestSubscriptionCycle.Daily, "---ASSET---", []);
var autoinvest_204 = await api.AutoInvest.RedeemAsync("---INDEX-ID---", "---REQUEST-ID---", 10);
var autoinvest_205 = await api.AutoInvest.GetSubscriptionHistoryAsync();
var autoinvest_206 = await api.AutoInvest.GetOneTimeTransactionAsync(1_000_000L, "---REQUEST-ID---");
var autoinvest_207 = await api.AutoInvest.CreatePlanAsync("---SOURCE-TYPE---", BinanceAutoInvestPlanType.All, 100.0m, AutoInvestSubscriptionCycle.Daily, 6, "---ASSET---", []);
var autoinvest_208 = await api.AutoInvest.GetRedemptionHistoryAsync(1_000_000L);
var autoinvest_209 = await api.AutoInvest.GetHoldingsAsync();
var autoinvest_210 = await api.AutoInvest.GetPositionAsync(1_000_000L);
var autoinvest_211 = await api.AutoInvest.GetRebalanceHistoryAsync();
```

## WebSocket Api Query Methods

The Binance.Api socket client provides several query methods.

```csharp
// WebSocket API Client
var ws = new BinanceSocketApiClient();
ws.SetApiCredentials("XXXXXXXX-API-KEY-XXXXXXXX", "XXXXXXXX-API-SECRET-XXXXXXXX");

// Spot Web Socket API > General Methods (PUBLIC)
var spot_101 = await ws.Spot.PingAsync();
var spot_102 = await ws.Spot.GetTimeAsync();
var spot_103 = await ws.Spot.GetExchangeInfoAsync();

// Spot Web Socket API > Market Data Query Methods (PUBLIC)
var spot_201 = await ws.Spot.GetOrderBookAsync("BTCUSDT");
var spot_202 = await ws.Spot.GetRecentTradesAsync("BTCUSDT");
var spot_203 = await ws.Spot.GetHistoricalTradesAsync("BTCUSDT");
var spot_204 = await ws.Spot.GetAggregatedTradesAsync("BTCUSDT");
var spot_205 = await ws.Spot.GetKlinesAsync("BTCUSDT", BinanceKlineInterval.OneDay);
var spot_206 = await ws.Spot.GetUIKlinesAsync("BTCUSDT", BinanceKlineInterval.OneDay);
var spot_207 = await ws.Spot.GetAveragePriceAsync("BTCUSDT");
var spot_211 = await ws.Spot.GetTickerAsync("BTCUSDT");
var spot_212 = await ws.Spot.GetTickersAsync(["BTCUSDT", "ETHUSDT"]);
var spot_214 = await ws.Spot.GetTickersAsync();
var spot_215 = await ws.Spot.GetMiniTickerAsync("BTCUSDT");
var spot_216 = await ws.Spot.GetMiniTickersAsync(["BTCUSDT", "ETHUSDT"]);
var spot_217 = await ws.Spot.GetMiniTickersAsync();
var spot_221 = await ws.Spot.GetTradingDayTickerAsync("BTCUSDT");
var spot_222 = await ws.Spot.GetTradingDayTickersAsync(["BTCUSDT", "ETHUSDT"]);
var spot_223 = await ws.Spot.GetTradingDayTickersAsync();
var spot_224 = await ws.Spot.GetTradingDayMiniTickerAsync("BTCUSDT");
var spot_225 = await ws.Spot.GetTradingDayMiniTickersAsync(["BTCUSDT", "ETHUSDT"]);
var spot_226 = await ws.Spot.GetTradingDayMiniTickersAsync();
var spot_231 = await ws.Spot.GetRollingWindowTickerAsync("BTCUSDT");
var spot_232 = await ws.Spot.GetRollingWindowTickersAsync(["BTCUSDT", "ETHUSDT"], TimeSpan.FromHours(4));
var spot_241 = await ws.Spot.GetBookTickerAsync("BTCUSDT");
var spot_242 = await ws.Spot.GetBookTickersAsync(["BTCUSDT", "ETHUSDT"]);
var spot_243 = await ws.Spot.GetBookTickersAsync();

// Spot Web Socket API > Trading Query Methods (PRIVATE)
var spot_301 = await ws.Spot.PlaceOrderAsync("BTCUSDT", BinanceOrderSide.Buy, BinanceSpotOrderType.Market, 0.01m);
var spot_302 = await ws.Spot.PlaceTestOrderAsync("BTCUSDT", BinanceOrderSide.Buy, BinanceSpotOrderType.Market, 0.01m);
var spot_303 = await ws.Spot.GetOrderAsync("BTCUSDT", orderId: 100000001);
var spot_304 = await ws.Spot.GetOrderAsync("BTCUSDT", origClientOrderId: "---CLIENT-ORDER-ID---");
var spot_305 = await ws.Spot.CancelOrderAsync("BTCUSDT", orderId: 100000001);
var spot_306 = await ws.Spot.CancelOrderAsync("BTCUSDT", origClientOrderId: "---CLIENT-ORDER-ID---");
var spot_307 = await ws.Spot.ReplaceOrderAsync("BTCUSDT", BinanceOrderSide.Buy, BinanceSpotOrderType.Market, BinanceSpotOrderCancelReplaceMode.StopOnFailure, cancelOrderId: 100000001, quantity: 0.1m);
var spot_308 = await ws.Spot.GetOpenOrdersAsync("BTCUSDT");
var spot_309 = await ws.Spot.CancelOrdersAsync("BTCUSDT");

// Spot Web Socket API > Account Query Methods (PRIVATE)
var spot_401 = await ws.Spot.GetAccountAsync();
var spot_402 = await ws.Spot.GetRateLimitsAsync();
var spot_403 = await ws.Spot.GetOrdersAsync("BTCUSDT");
var spot_404 = await ws.Spot.GetOcoOrdersAsync();
var spot_405 = await ws.Spot.GetUserTradesAsync("BTCUSDT");
var spot_406 = await ws.Spot.GetPreventedTradesAsync("BTCUSDT", orderId: 100000001);

// USDⓈ-M Futures Web Socket API -> General Methods (PUBLIC)
var futures_101 = await ws.UsdFutures.PingAsync();
var futures_102 = await ws.UsdFutures.GetTimeAsync();
var futures_103 = await ws.UsdFutures.GetOrderBookAsync("---SYMBOL---");

// USDⓈ-M Futures Web Socket API -> Market Data Methods (PUBLIC)
var futures_201 = await ws.UsdFutures.GetPriceAsync("---SYMBOL---");
var futures_202 = await ws.UsdFutures.GetPricesAsync();
var futures_203 = await ws.UsdFutures.GetBookPriceAsync("---SYMBOL---");
var futures_204 = await ws.UsdFutures.GetBookPricesAsync();

// USDⓈ-M Futures Web Socket API -> Account Methods (PRIVATE)
var futures_301 = await ws.UsdFutures.GetBalancesAsync();
var futures_302 = await ws.UsdFutures.GetAccountAsync();

// USDⓈ-M Futures -> Trading Methods (PRIVATE)
var futures_401 = await ws.UsdFutures.PlaceOrderAsync("---SYMBOL---", BinanceOrderSide.Buy, BinanceFuturesOrderType.Market, 100.0m);
var futures_402 = await ws.UsdFutures.ModifyOrderAsync("---SYMBOL---", BinanceOrderSide.Buy, 110.0m, orderId: 1_000_000L);
var futures_403 = await ws.UsdFutures.CancelOrderAsync("---SYMBOL---", orderId: 1_000_000L);
var futures_404 = await ws.UsdFutures.GetOrderAsync("---SYMBOL---", orderId: 1_000_000L);
var futures_405 = await ws.UsdFutures.GetPositionsAsync();

// Coin-M Futures -> Market Data Methods (PUBLIC)
var futures_601 = await ws.CoinFutures.PingAsync();
var futures_602 = await ws.CoinFutures.GetTimeAsync();

// Coin-M Futures -> Trading Methods (PRIVATE)
var futures_701 = await ws.CoinFutures.PlaceOrderAsync("---SYMBOL---", BinanceOrderSide.Buy, BinanceFuturesOrderType.Market, 100.0m);
var futures_702 = await ws.CoinFutures.ModifyOrderAsync("---SYMBOL---", BinanceOrderSide.Buy, 100.0m);
var futures_703 = await ws.CoinFutures.CancelOrderAsync("---SYMBOL---", orderId: 1_000_000L);
var futures_704 = await ws.CoinFutures.GetOrderAsync("---SYMBOL---", orderId: 1_000_000L);
var futures_705 = await ws.CoinFutures.GetPositionsAsync();

// Coin-M Futures -> Account Methods (PRIVATE)
var futures_901 = await ws.CoinFutures.GetBalancesAsync();
var futures_903 = await ws.CoinFutures.GetAccountInfoAsync();
```

## WebSocket Api Stream Methods

The Binance.Api socket client provides several stream methods to which can be subscribed.

```csharp
// WebSocket API Client
var ws = new BinanceSocketApiClient();
ws.SetApiCredentials("XXXXXXXX-API-KEY-XXXXXXXX", "XXXXXXXX-API-SECRET-XXXXXXXX");

// Subscription Samples
var sub01 = await ws.Spot.SubscribeToAggregatedTradesAsync("BTCUSDT", (data) =>
{
    Console.WriteLine($"{data.Data.Symbol} {data.Data.Price} {data.Data.Quantity} {data.Data.TradeTime}");
});

var sub02 = await ws.Spot.SubscribeToAggregatedTradesAsync(["ETHUSDT", "XRPUSDT"], (data) =>
{
    Console.WriteLine($"{data.Data.Symbol} {data.Data.Price} {data.Data.Quantity} {data.Data.TradeTime}");
});

// Unsubscription Methods
await ws.Spot.UnsubscribeAsync(sub01.Data);
await ws.Spot.UnsubscribeAsync(sub02.Data.Id);
await ws.Spot.UnsubscribeAllAsync();

// Spot Web Socket Stream > Market Data Subscriptions (PUBLIC)
await ws.Spot.SubscribeToAggregatedTradesAsync("BTCUSDT", (data) => { });
await ws.Spot.SubscribeToAggregatedTradesAsync(["ETHUSDT", "XRPUSDT"], (data) => { });
await ws.Spot.SubscribeToTradesAsync("BTCUSDT", (data) => { });
await ws.Spot.SubscribeToTradesAsync(["ETHUSDT", "XRPUSDT"], (data) => { });
await ws.Spot.SubscribeToKlinesAsync("BTCUSDT", BinanceKlineInterval.OneDay, (data) => { });
await ws.Spot.SubscribeToKlinesAsync(["ETHUSDT", "XRPUSDT"], BinanceKlineInterval.OneDay, (data) => { });
await ws.Spot.SubscribeToKlinesAsync("BTCUSDT", [BinanceKlineInterval.OneDay, BinanceKlineInterval.FourHours,], (data) => { });
await ws.Spot.SubscribeToKlinesAsync(["ETHUSDT", "XRPUSDT"], [BinanceKlineInterval.OneDay, BinanceKlineInterval.FourHours,], (data) => { });
await ws.Spot.SubscribeToMiniTickersAsync("BTCUSDT", (data) => { });
await ws.Spot.SubscribeToMiniTickersAsync(["ETHUSDT", "XRPUSDT"], (data) => { });
await ws.Spot.SubscribeToMiniTickersAsync((data) => { });
await ws.Spot.SubscribeToTickersAsync("BTCUSDT", (data) => { });
await ws.Spot.SubscribeToTickersAsync(["ETHUSDT", "XRPUSDT"], (data) => { });
await ws.Spot.SubscribeToTickersAsync((data) => { });
await ws.Spot.SubscribeToRollingWindowTickersAsync("BTCUSDT", TimeSpan.FromMinutes(60), (data) => { });
await ws.Spot.SubscribeToRollingWindowTickersAsync(TimeSpan.FromMinutes(60), (data) => { });
await ws.Spot.SubscribeToBookTickersAsync("BTCUSDT", (data) => { });
await ws.Spot.SubscribeToBookTickersAsync(["ETHUSDT", "XRPUSDT"], (data) => { });
await ws.Spot.SubscribeToPartialOrderBooksAsync("BTCUSDT", 20, null, (data) => { });
await ws.Spot.SubscribeToPartialOrderBooksAsync("BTCUSDT", 20, 100, (data) => { });
await ws.Spot.SubscribeToPartialOrderBooksAsync("BTCUSDT", 20, 1000, (data) => { });
await ws.Spot.SubscribeToOrderBooksAsync("BTCUSDT", null, (data) => { });
await ws.Spot.SubscribeToOrderBooksAsync("BTCUSDT", 100, (data) => { });
await ws.Spot.SubscribeToOrderBooksAsync("BTCUSDT", 1000, (data) => { });

// Spot Web Socket Stream > User Data Subscriptions (PRIVATE)
await ws.Spot.SubscribeToUserDataStreamAsync("-----LISTEN-KEY-----",
    onOrderUpdateMessage: (data) => { },
    onOcoOrderUpdateMessage: (data) => { },
    onAccountPositionMessage: (data) => { },
    onAccountBalanceUpdate: (data) => { },
    onBalanceLockUpdate: (data) => { },
    onUserDataStreamTerminated: (data) => { },
    onListenKeyExpired: (data) => { });

// USDⓈ-M Futures Web Socket Stream -> Market Data Methods (PUBLIC)
var futures_501 = await ws.UsdFutures.SubscribeToAggregatedTradesAsync("---SYMBOL---", (data) => { });
var futures_502 = await ws.UsdFutures.SubscribeToAggregatedTradesAsync(["---SYMBOL---"], (data) => { });
var futures_503 = await ws.UsdFutures.SubscribeToMarkPricesAsync("---SYMBOL---", 1000, (data) => { });
var futures_504 = await ws.UsdFutures.SubscribeToMarkPricesAsync(["---SYMBOL---"], 3000, (data) => { });
var futures_505 = await ws.UsdFutures.SubscribeToMarkPricesAsync(null, (data) => { });
var futures_506 = await ws.UsdFutures.SubscribeToKlinesAsync("---SYMBOL---", BinanceKlineInterval.FourHours, (data) => { });
var futures_507 = await ws.UsdFutures.SubscribeToKlinesAsync("---SYMBOL---", [BinanceKlineInterval.FourHours, BinanceKlineInterval.OneDay], (data) => { });
var futures_508 = await ws.UsdFutures.SubscribeToKlinesAsync(["---SYMBOL---"], BinanceKlineInterval.FourHours, (data) => { });
var futures_509 = await ws.UsdFutures.SubscribeToKlinesAsync(["---SYMBOL---"], [BinanceKlineInterval.FourHours, BinanceKlineInterval.OneDay], (data) => { });
var futures_510 = await ws.UsdFutures.SubscribeToContinuousContractKlinesAsync("---PAIR---", BinanceFuturesContractType.Perpetual, BinanceKlineInterval.OneDay, (data) => { });
var futures_511 = await ws.UsdFutures.SubscribeToContinuousContractKlinesAsync(["---PAIR---"], BinanceFuturesContractType.Perpetual, BinanceKlineInterval.OneDay, (data) => { });
var futures_512 = await ws.UsdFutures.SubscribeToMiniTickersAsync("---SYMBOL---", (data) => { });
var futures_513 = await ws.UsdFutures.SubscribeToMiniTickersAsync(["---SYMBOL---"], (data) => { });
var futures_517 = await ws.UsdFutures.SubscribeToMiniTickersAsync((data) => { });
var futures_514 = await ws.UsdFutures.SubscribeToTickersAsync((data) => { });
var futures_515 = await ws.UsdFutures.SubscribeToTickersAsync("---SYMBOL---", (data) => { });
var futures_516 = await ws.UsdFutures.SubscribeToTickersAsync(["---SYMBOL---"], (data) => { });
var futures_518 = await ws.UsdFutures.SubscribeToBookTickersAsync("---SYMBOL---", (data) => { });
var futures_519 = await ws.UsdFutures.SubscribeToBookTickersAsync(["---SYMBOL---"], (data) => { });
var futures_520 = await ws.UsdFutures.SubscribeToBookTickersAsync((data) => { });
var futures_521 = await ws.UsdFutures.SubscribeToLiquidationsAsync("---SYMBOL---", (data) => { });
var futures_522 = await ws.UsdFutures.SubscribeToLiquidationsAsync(["---SYMBOL---"], (data) => { });
var futures_523 = await ws.UsdFutures.SubscribeToLiquidationsAsync((data) => { });
var futures_524 = await ws.UsdFutures.SubscribeToPartialOrderBooksAsync("---SYMBOL---", 20, 100, (data) => { });
var futures_525 = await ws.UsdFutures.SubscribeToPartialOrderBooksAsync(["---SYMBOL---"], 20, 250, (data) => { });
var futures_526 = await ws.UsdFutures.SubscribeToOrderBooksAsync("---SYMBOL---", 500, (data) => { });
var futures_527 = await ws.UsdFutures.SubscribeToOrderBooksAsync(["---SYMBOL---"], null, (data) => { });
var futures_528 = await ws.UsdFutures.SubscribeToCompositeIndexesAsync("---SYMBOL---", (data) => { });
var futures_529 = await ws.UsdFutures.SubscribeToSymbolsAsync((data) => { });
var futures_530 = await ws.UsdFutures.SubscribeToAssetIndexesAsync("---SYMBOL---", (data) => { });
var futures_531 = await ws.UsdFutures.SubscribeToAssetIndexesAsync((data) => { });
var futures_532 = await ws.UsdFutures.SubscribeToTradesAsync("---SYMBOL---", (data) => { });
var futures_533 = await ws.UsdFutures.SubscribeToTradesAsync(["---SYMBOL---"], (data) => { });

// USDⓈ-M Futures Web Socket Stream -> User Data Methods (PRIVATE)
var futures_534 = await ws.UsdFutures.SubscribeToUserDataStreamAsync("-----LISTEN-KEY-----",
    onAccountUpdated: (data) => { },
    onLeverageUpdated: (data) => { },
    onMarginUpdated: (data) => { },
    onOrderUpdated: (data) => { },
    onTradeUpdated: (data) => { },
    onStrategyUpdated: (data) => { },
    onGridUpdated: (data) => { },
    onListenKeyExpired: (data) => { },
    onConditionalOrderTriggerRejectUpdate: (data) => { }
    );
```
