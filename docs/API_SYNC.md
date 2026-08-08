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
- The current ApiSharp rate-limiter configuration API is obsolete and emits build warnings on every main-library target. Rate-limit behavior must be revalidated during the shared transport audit rather than treated as a cosmetic warning.

## Review log

| Slice | Status | Scope | Evidence |
| --- | --- | --- | --- |
| 1 | Complete | Baseline inventory, test harness, Margin cancel-order defect | Official Margin REST Trade reference, solution build, request-level regression test |
| 2 | Complete | REST and WebSocket API authentication compatibility | Official Spot signed-endpoint rules, mixed query/body and percent-encoding tests, RSA PEM and Ed25519 cryptographic verification tests |
