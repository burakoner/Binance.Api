# Binance API Synchronization

## Objective

Synchronize the wrapper with the current official Binance documentation without preserving obsolete endpoint contracts. Changelogs are discovery and prioritization inputs; each touched endpoint must be checked against its current canonical endpoint reference, including method, path, authentication, request weights, parameters, validation, response models, enums, and stream payloads.

## Official baseline

- Baseline date: 2026-08-08
- Documentation index: https://developers.binance.com/en/docs/llms.txt
- Complete machine-readable documentation: https://developers.binance.com/en/docs/llms-full.txt
- Spot: https://developers.binance.com/en/docs/products/spot/CHANGELOG
- Margin: https://developers.binance.com/en/docs/products/margin-trading/Introduction
- Convert: https://developers.binance.com/en/docs/products/convert/Introduction
- Algo Trading: https://developers.binance.com/en/docs/products/algo/Introduction
- USDⓈ-M Futures: https://developers.binance.com/en/docs/products/derivatives-trading-usds-futures/general-info
- COIN-M Futures: https://developers.binance.com/en/docs/products/derivatives-trading-coin-futures/general-info
- Options: https://developers.binance.com/en/docs/products/derivatives-trading-options/general-info

The official documentation is a moving target. The baseline date must be advanced only after re-running the inventory and reviewing changes published since the previous baseline.

## Execution contract

1. Audit shared transport, authentication, signing, time handling, rate limiting, serialization, and error handling before relying on endpoint-level tests.
2. Process product families in risk-adjusted slices: verified critical defects first, then Spot, Margin, Convert, Algo Trading, USDⓈ-M Futures, COIN-M Futures, and Options.
3. For each REST operation, compare HTTP method, path, security type, weight, parameter location, required/optional state, bounds, enums, response schema, and deprecation/removal state.
4. For each WebSocket API operation and stream, compare URL, subscription method, request payload, authentication, weight, event routing, reconnect behavior, and event schema.
5. Add regression tests for every corrected defect and every new contract where the request or response can affect balances, positions, or orders.
6. Do not retain an obsolete endpoint merely for historical compatibility. Remove retired operations after confirming the current official reference and update call sites and documentation in the same slice.
7. After every four or five implementation slices, perform a backward review of code, tests, documentation, and this execution contract; revise ordering and scope before continuing.

## Initial findings

- The solution initially had no test project. A request-level test project is required before broad endpoint changes.
- The main library builds on all declared target frameworks after package restore.
- The Spot documentation contains production changes newer than the latest functional library commit, including signed-payload encoding, retired listen-key operations, new execution-rule and reference-price operations, block trades, expiry reasons, and symbol status values.
- Preliminary route inventory shows material REST coverage gaps in Spot and Margin, plus smaller gaps in Options and COIN-M Futures. Counts remain preliminary until each canonical endpoint page is reviewed.
- `CancelMarginOrderAsync` used `GET /sapi/v1/margin/order` instead of the documented `DELETE /sapi/v1/margin/order`. This is the first verified critical defect and is covered by a request-level regression test.
- REST signatures now use the exact percent-encoded query string followed by the form body, including mixed query/body requests. RSA PEM signing is covered by a cryptographic verification test.
- WebSocket API signing now distinguishes HMAC, RSA, and Ed25519 credentials and signs UTF-8 payload bytes. RSA and Ed25519 behavior is covered by cryptographic verification tests with non-ASCII parameters.
- ApiSharp 4.5.1 strips Ed25519 PEM markers before asking NSec to parse a PEM key. Binance.Api now imports the PKIX private key directly for .NET 8 and later and accepts both full PEM and its base64 body.
- USDⓈ-M `GET /fapi/v1/adlQuantile` sent signed parameters in a GET body and expected an object when `symbol` was supplied, although the current endpoint always accepts query parameters and returns an array. Both mismatches are corrected and request/response behavior is covered by a regression test.
- Six USDⓈ-M futures-data methods generated `/fapi/futures/data/...` instead of `/futures/data/...`. Three also treated the separate 1,000 requests/5 minutes quota as endpoint request weight 1,000 even though the documented IP weight is 0. Paths and weights are corrected, basis constraints are enforced, and the missing `CMCCirculatingSupply` response field is modeled.
- USDⓈ-M position-margin-change history used undocumented `GET /fapi/v3/positionMargin/history`. It now uses the current v1 route, validates the required symbol and documented maximum 30-day query range, and has request/response regression coverage.
- ApiSharp 4.5.1 does not read its own `RateLimiterEnabled` option in the active request path. Binance.Api now explicitly bypasses configured limiters when the option is false, with regression coverage.
- The remaining default limiter configuration is not a reliable model of current Binance limits: it covers only `/api/` and `/sapi/`, omits derivatives product buckets, and receives one integer that cannot distinguish IP request weight, UID weight, order counters, and separate fixed-window quotas. Numeric rules will be corrected only from verified product contracts; the obsolete ApiSharp configuration warning is not treated as cosmetic.
- The retired Spot listen-key REST operations and legacy stream subscription have been replaced by the current `userDataStream.subscribe.signature` WebSocket API flow. The replacement signs HMAC, RSA, or Ed25519 requests, supports fractional `recvWindow`, tracks Binance's `subscriptionId`, routes the current event envelope, refreshes timestamp/signature/subscription state on reconnect, and implements both single and connection-wide unsubscribe behavior.
- Spot user data event models now expose the server subscription ID and current execution-report conditional fields, including strategy, self-trade-prevention, allocation, SOR, and pegged-order data. The obsolete Spot listen-key identity and retired reject/execution enum values were removed. No authenticated live Binance account was used; coverage is deterministic request, response, reconnect-state, envelope, and model testing rather than a production connection test.

## REST route inventory baseline

This is a route-level candidate inventory from the official `llms-full.txt` API Reference and literal `GetUrl(...)` calls in the wrapper. An exact route match does not prove parameter or schema compatibility. An official-only or wrapper-only route is a review candidate, not an automatic implementation/removal instruction; the canonical endpoint page and product changelog remain authoritative for each change.

| Product | Official routes | Wrapper routes | Exact matches | Official-only candidates | Wrapper-only candidates |
| --- | ---: | ---: | ---: | ---: | ---: |
| Algo Trading | 11 | 11 | 11 | 0 | 0 |
| Convert | 9 | 9 | 9 | 0 | 0 |
| Margin | 65 | 49 | 44 | 21 | 5 |
| Spot | 48 | 30 | 27 | 21 | 3 |
| USDⓈ-M Futures | 95 | 83 | 83 | 12 | 0 |
| COIN-M Futures | 64 | 64 | 63 | 1 | 1 |
| Options | 44 | 45 | 41 | 3 | 4 |

The official compact inventory currently lists `GET /dapi/v1/leverageBracket`, while the product changelog documents `GET /dapi/v2/leverageBracket` and the wrapper uses v2. This conflict is a concrete example of why route candidates must be verified against the endpoint page before code changes.

## Backward review 1 (after slices 1-4)

- Re-read every change from the `8eefadc` baseline through slice 4. No defect requiring rollback was found in the Margin method correction, shared signing changes, USDⓈ-M ADL quantile correction, or futures-data correction.
- The request-level suite passes all 17 tests. A full solution build succeeds for every declared target; 13 warnings remain outside the reviewed contracts. The obsolete ApiSharp rate-limiter configuration and retired user-data-stream calls are functional audit items, not ignorable build noise.
- No REST `GET` request in the current source still places parameters in `bodyParameters`. This removes one systemic source of signature/request disagreement, but does not prove that every non-GET endpoint uses the documented parameter location.
- `GetMarginChangeHistoryAsync` targeted undocumented `GET /fapi/v3/positionMargin/history`; the current endpoint reference and changelog both specify `GET /fapi/v1/positionMargin/history`. Slice 5 corrected the full request contract and closed the only USDⓈ-M wrapper-only route candidate.
- `GetLiabilityCoinLeverageBracketInCrossMarginProModeAsync` exposed `GET /sapi/v1/margin/leverageBracket`, which the official Margin changelog says was retired on 2026-04-13 with Cross Margin Pro Mode. Slice 6 removed the operation, its two orphaned public response types, and the two repository-owned example calls.
- Spot listen-key REST operations were retired, but removal is intentionally ordered behind a complete WebSocket API user-data replacement. The replacement must implement authenticated session or signed subscription, subscription-ID lifecycle, the current event envelope, reconnect behavior, and tests in one coherent slice; removing only the old operations would strand users without account events.

Slice 8 completed that replacement and removed all three Spot wrapper-only REST routes. The current Spot REST route state is therefore 27 wrapper routes, 27 exact route matches, 21 official-only candidates, and no wrapper-only candidates. This route count still says nothing about parameter or response compatibility for the 27 matches.

### Revised next order

1. Replace the incomplete single-weight default rate-limit model as verified endpoint audits identify IP, UID, order-count, and fixed-window dimensions; do not infer one dimension from another.
2. Replace the retired Margin listen-key operations with the current listen-token WebSocket API flow before removing the old REST contracts; Spot migration is complete.
3. Continue product-family audits using the route candidates only as discovery input: Spot, Margin, Convert, Algo Trading, USDⓈ-M, COIN-M, then Options.

## Review log

| Slice | Status | Scope | Evidence |
| --- | --- | --- | --- |
| 1 | Complete | Baseline inventory, test harness, Margin cancel-order defect | Official Margin REST Trade reference, solution build, request-level regression test |
| 2 | Complete | REST and WebSocket API authentication compatibility | Official Spot signed-endpoint rules, mixed query/body and percent-encoding tests, RSA PEM and Ed25519 cryptographic verification tests |
| 3 | Complete | USDⓈ-M position ADL quantile endpoint | Official USDⓈ-M Trade reference, request-level query placement and symbol-filtered array response test |
| 4 | Complete | USDⓈ-M futures-data paths, weights, constraints, and response schema | Official USDⓈ-M Market Data reference, six route tests, constraint tests, and circulating-supply deserialization test |
| Review 1 | Complete | Backward code/test/documentation review and execution-order revision | `8eefadc..32f0ba8` diff review, 17 request/authentication tests, full solution build, signed-GET parameter scan, canonical route and retirement verification |
| 5 | Complete | USDⓈ-M position-margin-change history | Official USDⓈ-M Trade reference and generated official connector, v1 request contract and response test, 30-day range constraint test |
| 6 | Complete | Retired Cross Margin Pro leverage-bracket operation | Official Margin changelog effective 2026-04-13, repository-wide call-site scan, README/console example cleanup, solution build |
| 7 | Complete | Rate-limiter enable/disable transport contract | ApiSharp 4.5.1 source audit and disabled-limiter regression test |
| 8 | Complete | Spot WebSocket API signed user data stream and retired listen-key removal | Official Spot WebSocket API and User Data Stream references, generated official connector, 28 unit/request/model tests, full multi-target solution build |
