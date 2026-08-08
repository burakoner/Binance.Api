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
  * Removed stale single-dimensional default REST rate limits that could not correctly model Binance's dynamic IP, UID, raw-request, and order-count dimensions; explicit custom limiters remain supported
  * Replaced retired Spot listen-key REST and stream operations with signed WebSocket API user data subscriptions, subscription-ID routing, reconnect-safe signing, and current event models
  * Replaced the removed Margin listen-key documentation contract with API-key-issued listen tokens, WebSocket API subscriptions, replacement-token extension, and current Margin event models
  * Added the separate current Cross Margin risk-data listen-key lifecycle and `margin-stream.binance.com` margin-level/liability event stream
  * Aligned Margin order placement and cancellation contracts, including query-based DELETE requests, previously omitted order fields, side-effect-dependent weights, and complete OTO/OTOCO support
  * Enforced the documented GTC requirement for every Margin OTO/OTOCO iceberg leg
  * Added the current Margin manual-liquidation and liquidation-loan query, repayment, and repayment-history contracts
  * Aligned Margin borrow/repay contracts with the current combined operation, both history types, current request weights and ranges, and current response schemas; removed obsolete archived and limit parameters
  * Added the current Cross and Isolated Margin capital-flow query with seven-day range validation, all documented flow types, and institutional-loan notes
  * Added current Margin limit-price pairs, listing schedule, risk-based liquidation ratios, and restricted-assets market-data queries
  * Added the current Margin prevented-matches query with its documented identifier combinations, pagination cursor, and response schema
  * Aligned the complete Margin Market Data surface, including signed USER_DATA queries, current parameters, canonical links, 64-bit fields, raw undefined-unit timestamps, and corrected public names
  * Aligned the complete Margin Account surface, including leverage and isolated-account request constraints, symbol filtering, canonical links, 64-bit fee and limit fields, decimal risk levels, and current account response fields
  * Fixed Isolated Margin account disable requests sending signed DELETE parameters in a form body instead of the documented query string
  * Enforced the documented 90-day availability window for explicit Margin capital-flow time filters
  * Aligned Spot General REST and WebSocket API contracts, including execution rules, current symbol statuses, exchange-info precision and capability fields, SOR groups, self-trade-prevention modes, and symbol filters
  * Aligned Spot Market Data REST and WebSocket API query contracts, including block trades, reference prices, status filters, current weights and limits, kline timezones, rolling-window models, and required trading-day symbols
  * Added the Spot `CANCEL_ONLY` response status and separated response statuses from the narrower request-filter enum
  * Aligned Spot market streams with reference-price, block-trade, average-price, UTC+8 kline, microsecond timestamp, stream-limit, testnet-host, and server-shutdown contracts
  * Removed retired buyer/seller order IDs from Spot trade stream events and mapped the remaining ignored field correctly
  * Removed the retired Spot all-market ticker stream (`!ticker@arr`) overload and examples
  * Aligned Spot Account REST and WebSocket API queries with the current routes, request weights, fractional receive windows, parameter constraints, order-list, allocation, commission, amendment, account-filter, and conditional order response contracts
  * Aligned core Spot Trade REST and WebSocket API operations with current pegged-order, fractional receive-window, cancel-replace, amend-keep-priority, SOR, commission, and response contracts
  * Added current Spot REST and WebSocket API order-list cancellation and OCO, OPO, OPOCO, OTO, and OTOCO placement contracts without adding the deprecated OCO operations
  * Fixed Spot cancel-order and cancel-all DELETE requests sending endpoint parameters in form bodies instead of signed query strings
  * Fixed broker client-order ID cleanup removing valid leading characters when an ID did not start with the exact Binance broker prefix
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
