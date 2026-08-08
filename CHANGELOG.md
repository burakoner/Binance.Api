## Change Log & Release Notes

* Unreleased
  * Fixed `CancelMarginOrderAsync` sending a GET request instead of the documented DELETE request
  * Fixed signed REST requests failing to generate a signature with RSA PEM credentials and omitting query parameters when a body is present
  * Fixed WebSocket API RSA and Ed25519 signatures, including UTF-8 parameter payloads
  * Fixed USDⓈ-M position ADL quantile requests sending GET parameters in the body and deserializing symbol-filtered array responses as an object
  * Fixed six USDⓈ-M futures-data methods using an invalid `/fapi/futures/data` path, corrected their request weights, and added the missing CoinMarketCap circulating-supply field
  * Fixed USDⓈ-M position-margin history using the undocumented `/fapi/v3` path and added the current 30-day query-range constraint
  * Removed the retired Cross Margin Pro liability leverage-bracket operation and response types
  * Fixed `RateLimiterEnabled=false` being ignored by the underlying transport
  * Replaced retired Spot listen-key REST and stream operations with signed WebSocket API user data subscriptions, subscription-ID routing, reconnect-safe signing, and current event models
  * Replaced the removed Margin listen-key documentation contract with API-key-issued listen tokens, WebSocket API subscriptions, replacement-token extension, and current Margin event models
  * Aligned Spot General REST and WebSocket API contracts, including execution rules, current symbol statuses, exchange-info precision and capability fields, SOR groups, self-trade-prevention modes, and symbol filters
  * Aligned Spot Market Data REST and WebSocket API query contracts, including block trades, reference prices, status filters, current weights and limits, kline timezones, rolling-window models, and required trading-day symbols
  * Added the Spot `CANCEL_ONLY` response status and separated response statuses from the narrower request-filter enum
  * Aligned Spot market streams with reference-price, block-trade, average-price, UTC+8 kline, microsecond timestamp, stream-limit, testnet-host, and server-shutdown contracts
  * Removed retired buyer/seller order IDs from Spot trade stream events and mapped the remaining ignored field correctly
  * Removed the retired Spot all-market ticker stream (`!ticker@arr`) overload and examples
  * Added a request-level test project for endpoint contract regression coverage

* Version 5.10.19 - 19 Oct 2025
  * Updated to ApiSharp 4.1.0

* Version 5.10.13 - 13 Oct 2025
  * Added new SetApiCredentials method override

* Version 5.9.29 - 29 Sep 2025
  * Fixed minor bugs
  * Added missing endpoints as below
    * IBinanceFuturesRestClientCoinMarketData
      - GetIndexPriceConstituentsAsync

    * IBinanceFuturesRestClientCoinPortfolioMargin
      - GetPortfolioMarginAccountInfoAsync

    * IBinanceFuturesRestClientCoinTrade
      - ModifyOrderAsync
      - ModifyOrdersAsync
      - GetOrderModifyHistoryAsync

    * IBinanceFuturesRestClientUsdAccount
      - GetAccountInfoAsync

    * IBinanceFuturesRestClientUsdMarketData
      - GetDeliveryPricesAsync
      - GetIndexPriceConstituentsAsync
      - GetInsuranceBalancesAsync

    * IBinanceFuturesRestClientUsdPortfolioMargin
      - GetPortfolioMarginAccountInfoAsync

    * IBinanceFuturesRestClientUsdTrade
      - PlaceTestOrderAsync

    * IBinanceFuturesSocketClientCoinStreamMarketData
      - SubscribeToAllMarkPriceUpdatesOfAllSymbolsOfPairAsync

    * IBinanceWalletRestClientAsset
      - GetDelegationHistoryAsync
      - GetListScheduleAsync

* Version 5.7.21 - 21 Jul 2025

* Version 5.6.19 - 17 Jun 2025
  * Implemented "Vip Loan" Section

* Version 5.6.18 - 17 Jun 2025
  * Updated to ApiSharp 3.8.2
  * Implemented "European Options -> WebSocket Market Streams" Section
  * Implemented "European Options -> WebSocket User Data Stream" Section

* Version 5.6.17 - 17 Jun 2025
  * Implemented "European Options -> User Data Stream" Section
  * Implemented "European Options -> Market Maker" Section

* Version 5.6.15 - 15 Jun 2025
  * Refactored base response models
  * Implemented "C2C" Section
  * Implemented "Fiat" Section
  * Implemented "Dual Investment" Section
  * Implemented "Futures Data" Section

* Version 5.6.13 - 13 Jun 2025
  * Renamed "Broker" Section to "Link"
  * Implemented "Institutional Loan" Section
  * Implemented "Rebate" Section
  * Implemented "Binance Pay History" Section

* Version 5.6.12 - 12 Jun 2025
* Version 5.6.11 - 11 Jun 2025
* Version 5.6.10 - 10 Jun 2025
* Version 5.6.08 - 08 Jun 2025
* Version 5.5.23 - 22 May 2025
* Version 5.5.22 - 20 May 2025
* Version 5.5.21 - 20 May 2025
* Version 5.5.20 - 20 May 2025
* Version 5.5.17 - 15 May 2025
* Version 5.5.16 - 15 May 2025
* Version 5.5.15 - 14 May 2025
* Version 5.5.14 - 14 May 2025
* Version 3.5.512 - 12 May 2025
* Version 3.5.505 - 05 May 2025
* Version 3.5.501 - 01 May 2025
* Version 3.5.500 - 01 May 2025
* Version 2.0.0 - 07 May 2024
* Version 1.3.2 - 25 Apr 2023
* Version 1.3.1 - 18 Apr 2023
* Version 1.3.0 - 30 Mar 2023
* Version 1.2.7 - 27 Mar 2023
* Version 1.2.6 - 24 Mar 2023
* Version 1.1.5 - 07 Mar 2023
* Version 1.1.2 - 23 Feb 2023
* Version 1.1.1 - 17 Feb 2023
* Version 1.1.0 - 14 Feb 2023
* Version 1.0.6 - 30 Jan 2023
* Version 1.0.5 - 29 Jan 2023
* Version 1.0.2 - 29 Jan 2023
* Version 1.0.1 - 27 Jan 2023
* Version 1.0.0 - 25 Jan 2023
