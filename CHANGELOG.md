## Change Log & Release Notes

* Unreleased
  * Added mandatory Spot FIX QuickFIX/n TLS projection with exact direct production/Testnet role hosts for both TCP and SNI, certificate and revocation validation locked on, and real in-process matching-host, hostname-mismatch, and untrusted-chain handshake coverage
  * Added a bounded Spot FIX Ed25519 Logon boundary with the official SOH/ASCII/Base64 signature vector, Ed25519-only PKCS#8 import, role-correct required fields, disposed-key rejection, and exact tag 96/553 wire redaction
  * Added the first bounded FIX session-core slice with explicit production/Testnet selection, exact immutable role endpoints, strict SenderCompID and heartbeat validation, sequential message handling, Order Entry/Drop Copy response defaults, and Market Data response-mode isolation
  * Completed Backward Review 33 across the FIX/SBE foundation and generation spikes: kept Binance-derived generated artifacts out of Git pending explicit redistribution authority, proved QuickFIX/n 1.14.1 can fail closed on sequence gaps without sending or persisting unsupported ResendRequest messages, and narrowed the next executable work to the non-generated common session core
  * Proved deterministic generation of both Binance Spot text FIX 4.4 dictionaries with QuickFIX/n DDTool 1.14.1: separate internal role namespaces, a shared 148-field internal catalog, unchanged published Core assemblies, warning-free net8/net9/net10 compilation, and zero exported generated types without committing Binance-derived source
  * Proved the pinned FIX SBE `1:1` C# generation pipeline with portable JDK 17 and SbeTool 1.39.0: two identical 84-file outputs, exact upstream warning/vendor-metadata gates, deterministic internalization, all five consumer target frameworks, and zero exported wire types without committing generated Binance artifacts
  * Added a license-safe Spot FIX schema provenance lock and maintainer-only verified downloader for the two text dictionaries, FIX SBE `1:1`, aliases, and lifecycle files; corrected checkout-converted hashes to exact commit-pinned raw bytes without redistributing Binance artifacts
  * Established the truthful non-packable FIX/SBE foundation: removed the empty public placeholder and false completeness claims, upgraded to QuickFIX/n Core 1.14.1 without the generic FIX44 package, and added netstandard SBE plus dedicated protocol test projects
  * Defined the evidence-backed Spot FIX/SBE implementation execution contract: truthful package boundaries, pinned schema and generator provenance, fail-closed TLS/session/binary safety gates, deterministic conformance policy, and 23 small implementation slices with five mandatory reviews
  * Completed the final Options documentation audit across all 68 active XML references; corrected three Market Maker kill-switch anchors and proved exact 55/55 canonical-target parity with zero active retired-root residue
  * Completed Backward Review 32 across the final four Options implementation slices; corrected Market kline and Public ticker volume names to state their published contract units, and reconfirmed Block Trade, stream routing and payloads, and exact 44-operation REST parity
  * Removed the four unsupported wrapper-only Options REST operations and their dedicated public response types, leaving exact method/path parity with all 44 operations in the current official catalog and connector
  * Aligned all five Options Market WebSocket stream contracts with the current all-index, kline, option-mark-price, new-symbol, and open-interest topics, strict input domains, complete event/int64/risk/book payload models, exact array handling, and canonical documentation
  * Aligned all five Options Public WebSocket stream contracts with the current Public route, canonical depth, book-ticker, 24-hour-ticker, and trade topics, strict levels and update speeds, complete int64/timestamp payload models, fail-closed topic validation, and canonical documentation
  * Aligned all seven Options Market Maker Block Trade contracts with the current single-leg request, exact query/body placement, empty cancellation acknowledgement, complete order timestamps, int64 receive-window ceiling, fail-closed validation, unambiguous query methods, and canonical documentation
  * Completed Backward Review 31 across the four Options read-contract slices; removed invented Trade-read receive-window and lower-limit restrictions, corrected User Exercise default-limit documentation, and reconfirmed the Market Data and Account contracts
  * Aligned the Options Account Funding Flow with its current typed USDT currency, exact query key, int64 limit and receive window, published validation boundary, complete response, and canonical documentation
  * Aligned all seven Options REST Trade read contracts with required and optional identifiers, symbol-dependent open-order weights, int64 limits and receive windows, endpoint-specific order results, and complete current trade, position, exercise, and commission responses
  * Aligned all Options REST Market Data response contracts with current endpoint-specific assets, contracts, filters, tuple layouts, identifiers, int64 fields, order-book update IDs, and ticker prices
  * Aligned all twelve Options REST Market Data request contracts with current weights, optional filters, int64 limits, exact open-interest date formatting, Options-valid symbol and kline validation, and canonical endpoint documentation
  * Completed Backward Review 30 across the four risk-first Options slices; reconfirmed the production contracts and corrected README/console batch examples that were guaranteed to fail pre-transport validation
  * Aligned all six Options Market Maker kill-switch and protection contracts with current weights, optional filters, singular request fields, int64 timing and receive-window values, complete responses, and fail-closed input validation
  * Aligned the Options private user-data stream with the current direct listen-key route and all six published account, balance/position, order/trade, Greek, risk-level, and listen-key-expiration event contracts
  * Aligned all six Options order mutation contracts with the current REST schemas: required quantities, ten-order batches, optional ACK/RESULT and self-trade-prevention modes, int64 receive windows, exact query serialization and weights, complete order fields, and endpoint-specific cancel-all acknowledgements
  * Aligned the Options REST user-data-stream lifecycle with the current parameterless API-key-only weight-one POST/PUT/DELETE contracts, exposed the start response's int64 expiration, handled empty keepalive/close responses, and updated executable call sites and endpoint documentation
  * Completed Backward Review 29 across the final COIN-M and Options inventory work; moved `modifyId` out of placement/cancellation acknowledgement surfaces into exact REST/WebSocket modification result types, and split and risk-reordered the remaining Options execution contract into smaller slices
  * Inventoried the complete current Options REST, public/market WebSocket stream, and user-data surfaces; confirmed exact coverage of all 44 current REST method/routes but exposed stale request weights, parameters, response models, listen-key lifecycle, WebSocket routing/topics/events, four unsupported wrapper-only routes, and ten bounded remediation slices
  * Migrated the final 35 owned COIN-M REST, WebSocket market-stream, and user-data-stream XML documentation links to their exact current canonical pages and anchors, eliminating the retired COIN-M documentation root from the repository
  * Aligned the five remaining COIN-M WebSocket API account, balance, cancel, query-order, and position contracts with current int64 request fields and ceilings, exact position filters and response models, fail-closed validation, and canonical Account documentation links
  * Migrated COIN-M WebSocket API normal order placement to the effective LIMIT/MARKET-only contract, removed conditional-only request fields, aligned numeric and int64 serialization, added fail-closed parameter validation, and moved the owned Trade documentation link to its current canonical anchor
  * Separated all six COIN-M REST and three WebSocket immediate order mutation responses from query-order models, removing impossible average, cumulative quote/base, creation-time, and good-till-date fields while preserving the complete read-order contract
  * Completed Backward Review 28 across COIN-M REST listen-key, market-data, income, signed-read, and normal/Algo order changes; prevented blank symbol filters from selecting lower dynamic request weights, rejected blank open-order pair filters, and corrected shared Futures Algo model documentation
  * Migrated COIN-M REST normal order placement to LIMIT/MARKET-only contracts, removed historical conditional fields from single and shared batch requests, added the three current DAPI conditional Algo placement/cancellation/open-order operations with shared complete models and fail-closed validation, and updated owned Trade documentation and examples
  * Aligned remaining signed COIN-M REST receive-window ceilings, current Account Information and Position Information response shapes, the Current All Open Orders pair filter and weights, and the three owned Account/Trade documentation links
  * Fully aligned COIN-M Income History with its exact seven-value income domain, int64 pagination and receive window, documented range boundary, string transaction identifier, dedicated response type, and current Account documentation link
  * Fully aligned five COIN-M REST kline and three futures-data contracts, including exact weight boundaries, required identifiers, interval/date/period validation, operation-specific contract types, safe `ALL` support, breaking correction of transport-specific kline response fields, complete response reconciliation, and current Market Data documentation links
  * Aligned the COIN-M REST user-data-stream lifecycle with current parameterless API-key-only weight-one requests, returned the refreshed listen key from keepalive, handled the documented empty close response, and migrated the owned documentation links and executable call sites
  * Closed a Spot and USDⓈ-M reconnect-authentication race that could re-logon after logout or API-key revocation, and completed backward Review 27 across session transitions, USDⓈ-M WebSocket market data, and COIN-M inventory/history-download work
  * Aligned all six COIN-M history-download ID and link contracts with current request weights, int64 receive-window validation, required identifiers, response fields, tolerant expiry parsing, and exact Account documentation links
  * Inventoried the complete current COIN-M REST, WebSocket API, market-stream, and user-data surfaces, recorded the post-integration remediation queue, and corrected three cross-product WebSocket Trade documentation links
  * Aligned the complete USDⓈ-M WebSocket API Market Data contract, including the default and discrete order-book limit weights, required-symbol validation, WebSocket-specific int64 book-ticker update IDs, and documented RPI exclusions
  * Serialized Spot and USDⓈ-M WebSocket API session logon, status, logout, and reconnect-authentication transitions to prevent concurrent logon from registering duplicate lifecycle subscriptions, with cancellation-safe ordering and corrected restoration guidance
  * Aligned the USDⓈ-M REST user-data-stream lifecycle with current parameterless API-key-only weight-one requests, returned the refreshed listen key from keepalive, handled the documented empty close response safely, and updated executable call sites
  * Aligned all four USDⓈ-M Futures Convert REST contracts, including public exchange-info authentication, signed request fields and weights, receive-window ceilings, string valid-time and order-status fields, response precision, and removal of the obsolete Futures Convert enums
  * Migrated the final twelve owned USDⓈ-M REST Convert, REST User Data Stream, and WebSocket API Market Data XML documentation links to their current canonical endpoint anchors
  * Added the routed USDⓈ-M `tradingSession` market stream with its exact topic, typed combined-stream callback, one-second session payload, and complete current U.S., commodity, Korean, and Hong Kong market event coverage
  * Added Ed25519-only USDⓈ-M WebSocket API session logon, status, logout, reconnect re-authentication, and API-key-revocation state, while preventing individually signed queries from being misclassified as connection-authenticated
  * Corrected USDⓈ-M account information and income-history contracts found by Backward Review 25, including current request weights and receive-window guards, typed income filters, int64 pagination and transaction identifiers, string trade identifiers, and the v2 `feeBurn` response field
  * Added the current coexisting USDⓈ-M WebSocket API v1 account, balance, and position queries alongside v2, aligning int64 request fields, complete response models, source-conflict guidance, and canonical documentation links
  * Migrated USDⓈ-M WebSocket order placement to the current normal/Algo split, added native conditional Algo placement and cancellation, aligned validation, numeric and int64 serialization, weights, response envelopes, and owned Trade documentation links
  * Migrated USDⓈ-M REST normal single and batch order placement to the post-2025 Algo contract, retaining only LIMIT/MARKET orders, removing conditional-only order parameters, rejecting invalid batch sizes before transport, preserving the separate test-order contract, and migrating all owned Trade documentation links
  * Aligned read-only USDⓈ-M REST kline intervals, continuous-contract types, and income types with the current contract, and migrated the owned Account and Market Data documentation links
  * Isolated USDⓈ-M, COIN-M, and Options trade-rule settings across REST and Futures WebSocket order validation, including the USDⓈ-M invalid client cast and Options batch gate
  * Fixed `CancelMarginOrderAsync` sending a GET request instead of the documented DELETE request
  * Fixed signed REST requests failing to generate a signature with RSA PEM credentials and omitting query parameters when a body is present
  * Fixed WebSocket API RSA and Ed25519 signatures, including UTF-8 parameter payloads
  * Added the optional Spot `executionReport.eR` expiry reason to user-data stream order updates with current typed enum mapping
  * Fixed USDⓈ-M position ADL quantile requests sending GET parameters in the body and deserializing symbol-filtered array responses as an object
  * Fixed six USDⓈ-M futures-data methods using an invalid `/fapi/futures/data` path, corrected their request weights, and added the missing CoinMarketCap circulating-supply field
  * Added current USDⓈ-M RPI order-book and symbol-level ADL risk Market Data queries with exact weights, parameters, response variants, and 64-bit fields
  * Added the current USDⓈ-M TradFi trading schedule, aligned the v2 price ticker contract, and excluded the deprecated v1 price route
  * Added the current coexisting USDⓈ-M v2 account-balance query and aligned both v2 and v3 balance contracts with the documented receive-window ceiling
  * Added native USDⓈ-M conditional Algo order and open-order queries with exact identifier, weight, receive-window, and response-shape contracts
  * Added the native USDⓈ-M all-conditional-Algo-orders query with current pagination, time-range, retention, and shared list-item response contracts
  * Added native USDⓈ-M single conditional Algo order cancellation with current identifiers, signed query placement, receive-window validation, weight, and response contract
  * Added native USDⓈ-M conditional Algo order placement with the current five-type surface, signed form body, explicit combination guards, zero IP weight, RPI time-in-force, and complete response contract
  * Added native USDⓈ-M bulk conditional Algo cancellation with the current symbol-scoped signed query, receive-window validation, weight, and complete response contract
  * Added explicit USDⓈ-M TradFi Perps agreement signing with the current signed form contract, receive-window ceiling, weight, response precision, and non-executable sample warning
  * Added explicit TradFi Options agreement signing with the current signed form contract, receive-window ceiling, weight, response precision, and non-executable sample warning
  * Moved the current Options Margin Account query from the Market Maker subclient to `GetMarginAccountAsync` on the Account client and aligned its complete response contract
  * Quarantined four undocumented Options operations from executable README and console examples without treating catalog absence as retirement evidence
  * Marked those four retained undocumented Options operations as unsupported current contracts in public API documentation and removed their stale official-documentation links
  * Removed the retired COIN-M Classic Portfolio Margin account-information operation; Binance directs clients to the active USDⓈ-M operation
  * Fixed the active USDⓈ-M Classic Portfolio Margin account-information query omitting USER_DATA signing and the documented receive-window ceiling
  * Added current COIN-M pair-default leverage brackets and corrected the symbol-specific route parameter, dynamic weight, receive-window ceiling, and response fields
  * Aligned USDⓈ-M and COIN-M historical market trades with the current API-key-only contract, IP weight 200, required-symbol and limit validation, canonical documentation, and complete USDⓈ-M RPI response data
  * Aligned USDⓈ-M funding-rate history with optional symbol and time filters, the current limit contract, canonical shared-rate-limit guidance, and the new funding-rate type response field
  * Aligned USDⓈ-M and COIN-M account trade lists with current signed query combinations, seven-day windows, receive-window ceilings, post-migration weight, identifier types, and complete product response fields
  * Aligned USDⓈ-M and COIN-M `ACCOUNT_UPDATE` user data events with the funding-fee symbol and COIN-M account alias fields
  * Added the complete USDⓈ-M `ALGO_UPDATE` user data event with native conditional-order callback routing and the activation placeholder
  * Aligned USDⓈ-M and COIN-M single-order modification with mandatory quantity and price, correct COIN-M side and identifier serialization, current IP weights, receive-window validation, and int64 `modifyId` pass-through
  * Aligned USDⓈ-M and COIN-M batch order modification with one-to-five-item validation, mandatory post-migration quantity and price, numeric JSON fields, int64 `modifyId`, per-item results, current IP weights, and receive-window limits
  * Aligned USDⓈ-M and COIN-M order-modification history with required order identity, inclusive time filters, current limit and retention guidance, exact weights, receive-window validation, int64 counts, and nested conditional `modifyId`
  * Aligned USDⓈ-M and COIN-M WebSocket API order modification with mandatory price, signed numeric parameters, int64 identities and `modifyId`, product-specific receive-window limits, and current IP-weight metadata
  * Aligned USDⓈ-M and COIN-M `ORDER_TRADE_UPDATE` events with the conditional string `modifyId`, product-specific account, margin, strategy, and expiry fields, and reliable wire enum and timestamp conversion
  * Clarified that WebSocket API request weights and returned counters are not proactively rate-limited, and corrected the documented default client-order-id adjustment behavior
  * Aligned USDⓈ-M and COIN-M All Orders with current symbol/pair scope, identifier combinations, seven-day windows, limits, post-migration weights, receive-window ceilings, retention guidance, and complete response fields
  * Aligned USDⓈ-M and COIN-M User's Force Orders with the missing limit, product-specific receive-window and retention contracts, dynamic weights, current filters, and complete response fields
  * Kept the existing positional Force Orders receive-window argument from being silently reinterpreted as the newly added limit
  * Aligned USDⓈ-M per-symbol and all-market mark-price streams with current topics, update speeds, moving-average and post-integration symbol-type fields, and canonical documentation
  * Aligned the USDⓈ-M aggregate-trade stream with a product-specific model containing RPI-excluded normal quantity and post-integration symbol type, current topic validation, and no stale ignore field
  * Aligned the COIN-M aggregate-trade stream with a product-specific model containing the post-integration symbol type, current topic validation, and no stale ignore or USDⓈ-M-only normal-quantity fields
  * Aligned COIN-M per-symbol, pair, and cross-host all-market mark-price streams with current topics, update speeds, post-integration symbol type, moving average, canonical documentation, product-neutral all-market callbacks, and a single estimated-settlement-price property
  * Aligned COIN-M individual and merged all-market book-ticker streams with current real-time topics, pair and post-integration symbol-type fields, canonical documentation, and deterministic topic/payload coverage
  * Aligned the merged USDⓈ-M and COIN-M contract-info stream with its current real-time topic, optional bracket payload, post-integration symbol type, canonical documentation, current contract/status enums, and int64 bracket fields
  * Corrected documented USDⓈ-M WebSocket market-stream subscriptions to use the current `/market/stream` and `/public/stream` channels instead of the previously hardcoded shared `/stream` path
  * Corrected the USDⓈ-M listen-key user-data subscription to use the current `/private/stream` channel instead of the decommissioned unrouted `/stream` path and reject blank listen keys before connecting
  * Aligned standard USDⓈ-M and COIN-M kline streams with current topics, intervals, outer event data, int64 trade counts, complete payloads, and product-specific base/quote/contract volume semantics; removed the undocumented USDⓈ-M premium-index stream switch
  * Aligned USDⓈ-M and COIN-M continuous-contract kline streams with complete outer and nested payloads, pair/contract identity, product-specific volume units, exact interval and contract-type support, and the current USDⓈ-M TradFi perpetual contract type
  * Aligned the COIN-M mark-price kline stream with outer event/pair identity, nested symbol and complete price data, exact topics and intervals, int64 counters, and explicit ignore-only fields without misleading volume semantics
  * Aligned the COIN-M index-price kline stream with outer event/pair identity, the complete nested payload, exact topics and intervals, int64 fields, placeholder-symbol guidance, and documented generic volume names without invented asset or contract units
  * Aligned USDⓈ-M and COIN-M individual and merged all-market liquidation streams with their complete event envelopes, product-dependent pair location, merged symbol type, exact topics, canonical documentation, and deterministic payload coverage
  * Aligned standard USDⓈ-M and COIN-M partial/diff depth streams with current levels, explicit update speeds, default suffix behavior, pair and symbol-type fields, canonical documentation, and deterministic topic/payload coverage
  * Added the current USDⓈ-M RPI diff-depth stream with its fixed 500-millisecond Public topic, complete pair/symbol-type payload, RPI aggregation semantics, canonical documentation, and deterministic coverage
  * Aligned USDⓈ-M and COIN-M individual and merged all-market mini-ticker streams with pair/symbol-type fields, exact topics and cadences, a shared product-discriminated model, and safe base/quote/contract volume access
  * Aligned USDⓈ-M and COIN-M individual and merged all-market 24-hour ticker streams with the complete current payload, product-safe volume semantics, exact topics/cadences, canonical documentation, and removal of stale previous-close and bid/ask fields
  * Removed the unsupported USDⓈ-M and COIN-M raw `@trade` subscriptions and their stale shared model; current Futures market trade streaming remains available through the documented `@aggTrade` contracts
  * Added the USDⓈ-M WebSocket API user-data-stream start, keepalive, and stop lifecycle with API-key-only authentication, current weights, listen-key responses, and controlled missing-credential errors
  * Added the COIN-M WebSocket API user-data-stream start, keepalive, and stop lifecycle with API-key-only authentication, current weights, listen-key responses, and controlled missing-credential errors
  * Fixed USDⓈ-M position-margin history using the undocumented `/fapi/v3` path and added the current 30-day query-range constraint
  * Removed the retired Cross Margin Pro liability leverage-bracket operation and response types
  * Fixed `RateLimiterEnabled=false` being ignored by the underlying transport
  * Removed stale single-dimensional default REST rate limits that could not correctly model Binance's dynamic IP, UID, raw-request, and order-count dimensions; explicit custom limiters remain supported
  * Added server-directed REST and WebSocket API backoff for 418/429 responses, exposing Binance's retry time and guarding subsequent requests without automatically retrying failed operations
  * Aligned both Convert Market Data operations with their current filter, receive-window, request-weight, and 64-bit precision contracts
  * Aligned the three read-only Convert Trade queries with current ranges, weights, parameters, terminology, and complete open-limit-order response data
  * Corrected Convert order-status identifier exclusivity and removed stale limit-order guidance for an undocumented exchange-info field
  * Aligned the Convert quote request and acceptance workflow with current wallet types, request validation, receive-window limits, response contracts, and executable examples
  * Aligned Convert limit-order placement and cancellation with current validation, 64-bit identifiers, request weights, receive-window limits, and response schemas
  * Aligned Futures Algo VP and TWAP order placement with current form-body contracts, UID weights, fixed-length client Algo IDs, enum and duration constraints, receive-window limits, and response fields
  * Aligned Spot Algo TWAP order placement with its current form-body contract, fixed-length client Algo IDs, validation rules, and removal of the undocumented receive-window parameter
  * Aligned Spot and Futures Algo cancellation with signed DELETE query parameters, receive-window validation, canonical identifiers, and 64-bit response codes
  * Aligned all three read-only Futures Algo queries with current pagination, receive-window, terminology, documentation, and response contracts
  * Aligned all three read-only Spot Algo queries with current pagination, receive-window, terminology, documentation, and shared response contracts
  * Aligned Options account block-trade history and user commission queries with current routes, weights, receive-window validation, and response contracts
  * Replaced retired Spot listen-key REST and stream operations with signed WebSocket API user data subscriptions, subscription-ID routing, reconnect-safe signing, and current event models
  * Replaced the removed Margin listen-key documentation contract with API-key-issued listen tokens, WebSocket API subscriptions, replacement-token extension, and current Margin event models
  * Added the separate current Cross Margin risk-data listen-key lifecycle and `margin-stream.binance.com` margin-level/liability event stream
  * Aligned Margin listen-token subscription identifiers with the current int64 WebSocket contract and corrected `executionReport.I` to explicit ignored-field semantics with complete conditional string-wire coverage
  * Aligned Margin order placement and cancellation contracts, including query-based DELETE requests, previously omitted order fields, side-effect-dependent weights, and complete OTO/OTOCO support
  * Enforced the documented GTC requirement for every Margin OTO/OTOCO iceberg leg
  * Added the current Margin manual-liquidation and liquidation-loan query, repayment, and repayment-history contracts
  * Aligned Margin borrow/repay contracts with the current combined operation, both history types, current request weights and ranges, and current response schemas; removed obsolete archived and limit parameters
  * Added the current Cross and Isolated Margin capital-flow query with seven-day range validation, all documented flow types, and institutional-loan notes
  * Added current Margin limit-price pairs, listing schedule, risk-based liquidation ratios, and restricted-assets market-data queries
  * Added the current Margin prevented-matches query with its documented identifier combinations, pagination cursor, and response schema
  * Aligned the complete Margin Market Data surface, including signed USER_DATA queries, current parameters, canonical links, 64-bit fields, raw undefined-unit timestamps, and corrected public names
  * Aligned the complete Margin Account surface, including leverage and isolated-account request constraints, symbol filtering, canonical links, 64-bit fee and limit fields, decimal risk levels, and current account response fields
  * Aligned both Margin Transfer queries, including the missing asset filter, optional direction, 30-day range, 64-bit pagination and response fields, raw undefined-unit timestamps, and canonical links
  * Aligned all read-only Margin Trade queries, including current public names, pagination names and types, account-scope fields, boolean encoding, 24-hour ranges, receive-window validation, 64-bit counters, exact client-order identifiers, and current response schemas
  * Added all six current Margin Special Key operations with signed parameter placement, key and IP targeting, permission modes, destructive-scope guards, non-value-formatted key models, and current account weights
  * Fixed Margin cancellation calls altering caller-supplied existing client-order identifiers when broker ID appending is enabled; only newly created cancellation identifiers are now prefixed
  * Enforced the 60000-millisecond receive-window ceiling across Margin order mutations and the documented one-to-ten-asset limit for small-liability exchange
  * Made the executable console sample network-safe by default; live balance-changing examples now require both an explicit command-line flag and exact typed confirmation
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
