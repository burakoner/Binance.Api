# Spot FIX and SBE implementation execution contract

Status: approved execution contract, not protocol implementation

Evidence date: 2026-08-11

Owning inventory: Slice 118

Completed implementation slices: Slice 142

Next implementation slice: Slice 143

## Brutal current state

`Binance.FIX.Api` is not a usable Binance FIX wrapper today. It contains an empty `Class1`, references QuickFIX/n 1.14.0, has no session, transport, authentication, message, decoder, public client, or test implementation, and nevertheless describes itself as an up-to-date and complete package. The solution contains no SBE project or binary protocol implementation at all. A successful solution build proves neither protocol support.

This program is much larger than adding a few missing JSON endpoints. The current official surface contains:

- three TLS FIX session roles: Order Entry, Drop Copy, and Market Data;
- 25 distinct text FIX message types across the 19-message Order Entry and 14-message Market Data dictionaries;
- text FIX on port 9000, FIX requests with SBE responses on port 9001, and full FIX SBE on port 9002;
- a 29-template FIX SBE schema, currently schema `1:1`;
- a separate four-template SBE market-data stream schema, `1:0`;
- a 92-template general Spot SBE schema, currently schema `3:5`, for REST, WebSocket API, and user-data output.

No protocol implementation will be called complete merely because generated classes exist or a socket connects.

## Authoritative evidence

The contract was derived from the current official [Spot FIX API](https://developers.binance.com/en/docs/products/spot/fix-api), [SBE Market Data Streams](https://developers.binance.com/en/docs/products/spot/sbe-market-data-streams), [SBE FAQ](https://developers.binance.com/en/docs/products/spot/faqs/sbe_faq), [Spot User Data Stream](https://developers.binance.com/en/docs/products/spot/user-data-stream), and the official [binance-spot-api-docs repository](https://github.com/binance/binance-spot-api-docs). Changelog entries remain supporting evidence only; current operation pages and schemas own the executable contract.

The official schema repository was pinned at commit `b483413fcdf4da783cd3fcaad6fab7200a93297f` for this inventory.

| Artifact | Current identity | SHA-256 at the pinned commit |
| --- | --- | --- |
| `fix/schemas/spot-fix-oe.xml` | FIX 4.4 Order Entry dictionary, 19 messages | `b147d360bbb7bbbf9046dcfc66e24c31412bcc8e6b94fafb42ac866360e21a7a` |
| `fix/schemas/spot-fix-md.xml` | FIX 4.4 Market Data dictionary, 14 messages | `6e7e9afaf3309a7388c7788b57e3d08fc4762120f1f851d827825bb1b855bdcf` |
| `sbe/schemas/spot-fixsbe-1_1.xml` | FIX SBE `1:1`, 29 templates | `85c3f816b8dfa61bed1838cce4699152fca8af83e3b2063e27aabece9b257933` |
| `sbe/schemas/stream_1_0.xml` | independent market stream `1:0`, 4 templates | `526b5151837ae5c13d2385ba64a46ebfafab3453f6ec197304ef418fe9642d09` |
| `sbe/schemas/spot_3_5.xml` | general Spot SBE `3:5`, 92 templates | `df6895db4c194c1bbf0377f1216ce0d09f770bac31b32c66d34425561e37fcc7` |
| `sbe/schemas/sbe_schema_lifecycle_prod.json` | current general schema `3:5` | `3dc91bc475b04419769f74cff47afaf95c058d716b08ea05ed9a759b42e3b9f0` |
| `sbe/schemas/sbe_fix_schema_lifecycle_prod.json` | current FIX SBE schema `1:1` | `539a968a9d8b65641ab2dd8e9d0f35ca475c694051b9515fb3d565a90748d957` |

Two upstream defects prevent blind `latest` consumption:

1. `spot_prod_latest.xml` points to `spot_3_4.xml`, while the production lifecycle file and the actual current XML identify `3:5`, released 2026-07-07.
2. `sbe_fix_schema_lifecycle_prod.json` contains a trailing comma and is not valid strict JSON.

Therefore aliases and lifecycle files are discovery inputs, not trusted build inputs. A schema promotion requires an explicit filename, exact upstream commit, file hash, XML `id/version`, lifecycle state, generated-output diff, and human review. Any disagreement stops promotion; it must never silently choose one source.

## Package and dependency boundary

The smallest coherent boundary is three layers:

| Owner | Responsibility | Explicit non-responsibility |
| --- | --- | --- |
| `Binance.FIX.Api` | Public Order Entry, Drop Copy, and Market Data session clients; text FIX; hybrid and full FIX SBE session behavior | General REST/WebSocket API SBE negotiation and standalone stream codecs |
| new `Binance.SBE.Api` | Versioned SBE codecs, SOFH/SBE framing, schema registry, binary safety, and standalone SBE market streams | Order semantics, QuickFIX/n sessions, or JSON REST ownership |
| existing `Binance.Api` | Opt-in REST, WebSocket API, and user-data SBE negotiation mapped into its existing public domain models | Generated codec ownership and FIX session behavior |

Dependency direction is one-way: `Binance.Api -> Binance.SBE.Api` and `Binance.FIX.Api -> Binance.SBE.Api`. `Binance.SBE.Api` must not reference either consumer. No shared “protocol framework” project will be introduced unless a concrete circular dependency proves it necessary.

The current empty FIX package must become non-packable and its false completeness claim must be removed in Slice 142. Packaging remains disabled until the final release gate. QuickFIX/n will be upgraded from 1.14.0 to the current 1.14.1 for implementation work. The `QuickFIXn.FIX44` package is not automatically retained: the current Binance dictionaries are custom FIX 4.4 dictionaries and must own their message factory.

## Generated-schema contract

Generated wire code is required; hand-maintaining 29, 4, and 92 SBE templates is rejected as an avoidable correctness risk.

- Real Logic/Aeron SbeTool C# generation is pinned to release `1.39.0`, commit `e773b57cac6b2008ce30dd219a33de49766c6013`.
- SbeTool is a maintainer-only generation dependency. It currently requires JDK 17, while this workstation exposes only Java 8. Generation cannot be claimed operational until Slice 144 supplies and tests the pinned JDK 17 toolchain.
- The official C# runtime project targets .NET Standard 2.0 and 2.1, but Slice 144 must still compile the actual Binance-generated output across every consuming target framework. Review 33 owns any package-boundary revision if that concrete matrix fails; target support will not be assumed from the runtime project alone.
- Official XML files are vendored unchanged with provenance. Generated C# is committed so normal restore/build/test is offline and requires neither Java nor network access.
- CI regenerates into a temporary directory and fails on a diff. It does not rewrite the worktree.
- Generated namespaces include schema identity. Generated types are implementation detail, not stable high-level business API; Slice 144 must prove the selected internalization boundary before consumer mapping begins.
- Every frame is length-checked before decode. Schema ID, schema version, template ID, block length, repeating-group counts, variable data lengths, null values, enum values, and microsecond timestamps receive deterministic boundary tests.
- Decoder tests include independent byte fixtures and field-offset assertions. Encoder-to-decoder round trips alone are insufficient evidence because the same generator can reproduce the same defect on both sides.

QuickFIX/n DDTool 1.14.1 was also tested against the two official Binance FIX dictionaries. Each dictionary parses individually, but combined generation fails because both identify as `FIX44`. DDTool also writes Binance-specific fields into QuickFIX/n core generated source, which is incompatible with consuming the published NuGet package unchanged. Slice 145 therefore owns a bounded generator-adapter decision: deterministic `customname` preprocessing plus Binance-owned internal field/message namespaces, or rejection of DDTool if the generated project cannot compile without modifying QuickFIX/n. Public order methods cannot start before that gate passes.

## FIX session safety contract

QuickFIX/n may own the text FIX state machine and TLS transport only after all of these behaviors are locked by tests:

- TLS is mandatory. The official hostname is always supplied for SNI and certificate hostname validation. Certificate validation and revocation checking cannot be disabled through the public API.
- Only Ed25519 credentials are accepted. Logon signs the exact SOH-joined `35`, `49`, `56`, `34`, and `52` values after QuickFIX/n has populated the header. API key and raw signature fields are redacted; raw wire logging is off by default.
- Order Entry, Drop Copy, and Market Data use distinct client types, endpoints, permissions, connection counters, rate limits, and message catalogs. A generic public `Send(Message)` escape hatch is out of scope.
- Heartbeat is constrained to the official 5-60 second range and defaults to 30 seconds. TestRequest/Heartbeat timeout behavior is deterministic.
- `MessageHandling=SEQUENTIAL` and `ResponseMode=EVERYTHING` are safe defaults. Unordered handling and acknowledgement-only responses require explicit opt-in and documented consequences.
- Logon and session reset use the documented sequence contract. Graceful Logout is attempted, but application shutdown cannot pretend it succeeded.
- Binance documents `ResendRequest` as unsupported. QuickFIX/n 1.14.1 automatically generates one when the inbound sequence is too high. Before any order API ships, a deterministic gap test must prove that the wrapper closes fail-closed without transmitting `ResendRequest`. If that cannot be achieved through a contained adapter, the team must stop and decide on a narrow QuickFIX/n fork or a different engine; it must not weaken the requirement.
- No order mutation is automatically replayed after timeout, disconnect, reconnect, or ambiguous write completion. Pending mutations become an explicit `UnknownDelivery` state and require execution-report, Drop Copy, or REST reconciliation.
- Client order IDs and list IDs are never reused automatically. Reconnect creates a fresh session identity according to the current official rules.
- The wrapper enforces the documented per-role message and connection-attempt budgets locally, while treating server rejections and disconnects as authoritative. Limit state is observable.
- RecvWindow uses the transport’s documented unit and precision: milliseconds with up to three decimal places for text FIX and microseconds for FIX SBE. The 60-second ceiling is fail-closed.
- Disposal is cancellation-aware, idempotent, and cannot race a reconnect into re-authenticating a user-requested closed session.

## SBE transport safety contract

FIX SBE and the independent WebSocket market stream are not the same transport.

- FIX SBE reads the six-byte Simple Open Framing Header, validates the little-endian encoding type `0xEB50`, then validates the 20-byte message header before payload dispatch.
- Port 9001 sends text FIX and receives FIX SBE. Port 9002 sends and receives FIX SBE. They share session roles and exchange limits with text FIX but not the text decoder.
- The independent SBE stream uses its own WebSocket endpoint, JSON text control messages, and binary market events. API-key authentication is the documented Ed25519-key header only; it must not invent a signature or timestamp.
- The independent stream enforces five control messages per second, 1,024 streams per connection, the documented connection-attempt budget, server ping/pong timing, the 24-hour connection lifetime, and JSON `serverShutdown` handling.
- Binary data never passes through the JSON serializer. Text control frames never pass through an SBE decoder.
- General Spot SBE negotiation checks response content type on every REST and WebSocket API response. JSON fallback and `NonRepresentable` results are explicit outcomes, never silent partial models.
- Same-schema higher versions are decoded only under the published additive-version rule and generated acting-version semantics. Unknown schema IDs, retired requested schemas, unknown template IDs, truncated frames, impossible group counts, and numeric overflow fail with typed protocol errors.
- Decimal mantissa/exponent, microsecond timestamps, optional/null semantics, and every `mbx:` mapping used to project into existing JSON-domain models have exact parity tests.

## Test and external-call policy

Default tests are deterministic and make no network call. They cover XML-to-generated registries, golden wire bytes, malformed/fuzzed frames, TLS configuration, signature bytes, logon/logout, heartbeat and timeout clocks, sequence gaps, reconnect cancellation, rate counters, message mapping, schema compatibility, and JSON/SBE semantic parity.

Integration tests use an in-process TLS FIX acceptor/WebSocket server with a test certificate and scripted frames. Tests must prove hostname mismatch rejection and signature/log redaction without printing real credentials.

Live conformance is Spot Testnet only, opt-in, and never part of normal CI. It requires disposable Ed25519 credentials and explicit environment gates. Market-data and session checks are read-only. Any testnet order placement/cancellation requires a second mutation-specific gate and explicit user authorization for that run. Production endpoints, production credentials, and production orders are prohibited in automated tests.

## Ordered small-slice program

The queue is a living execution contract. Review gates may split, reorder, or stop later work when evidence changes; they may not silently erase a safety requirement.

| Slice | Scope | Acceptance gate | Active budget |
| --- | --- | --- | ---: |
| 142 | Truthful package foundation | Disable FIX packing, remove empty surface/false description, add SBE/FIX test shells, upgrade QuickFIX/n, preserve green legacy build | 2-4 h |
| 143 | Vendored schemas and provenance | Exact upstream commit/hash/id/version manifest; strict alias/lifecycle conflict tests; no network in normal build | 3-6 h |
| 144 | Pinned SbeTool C# pipeline | JDK 17 generation-only toolchain, deterministic committed output, all-consuming-TFM compile matrix, internalization decision, regeneration diff gate | 4-8 h |
| 145 | Binance FIX dictionary generation spike | Both custom FIX 4.4 dictionaries generate, compile, and validate without modifying QuickFIX/n NuGet source | 5-10 h |
| Review 33 | Foundation review and re-estimate | Review Slices 142-145; decide whether QuickFIX/n remains viable before session implementation | 2-4 h |
| 146 | Common text FIX session core | TLS/SNI, Ed25519 logon, redaction, heartbeat, logout, limits, cancellation, sequence-gap/no-resend proof | 8-16 h |
| 147 | Order Entry single-order lifecycle | New, cancel, cancel-replace, amend-keep-priority, exact acknowledgements/rejects, unknown-delivery handling | 8-16 h |
| 148 | Order Entry lists and mass cancel | Order lists, list status, mass cancel, exact grouping and correlation behavior | 8-16 h |
| 149 | Drop Copy | Read-only execution/list status, delay semantics, reconnect/reconciliation surface | 4-8 h |
| 150 | Text FIX Market Data | Limit and instrument queries, subscriptions, snapshots/incrementals, unsubscribe and recovery | 8-16 h |
| Review 34 | Complete text FIX review | Full 25-message registry, session-role separation, mutation non-replay, docs/examples and testnet readiness | 2-4 h |
| 151 | FIX SBE `1:1` codec and framing | 29-template generated registry, SOFH/header/bounds/golden-vector tests | 5-10 h |
| 152 | Hybrid FIX/SBE port 9001 | Text requests plus binary responses for all applicable roles with public-model parity | 6-12 h |
| 153 | Full FIX SBE port 9002 | Binary logon/session and request encoding plus response decoding; exact time units and null semantics | 8-16 h |
| 154 | Cross-mode FIX parity | Equivalent text/hybrid/full outcomes, failure states, rate limits, and no mutation replay | 4-8 h |
| Review 35 | FIX SBE review | Schema/version/framing/session audit and post-generation scope recalibration | 2-4 h |
| 155 | Independent SBE stream transport | Endpoint/header, text control protocol, binary dispatch, ping/pong, shutdown and control limits | 6-12 h |
| 156 | Four SBE market events | Trade, best bid/ask, diff depth, and depth snapshot exact models and callbacks | 5-10 h |
| 157 | Stream recovery and binary hardening | 24-hour reconnect/subscription restore, depth sequence rules, malformed-frame and fuzz boundaries | 5-10 h |
| Review 36 | Independent stream review | Transport/event/reconnect/security audit and documentation check | 2-4 h |
| 158 | General Spot SBE `3:5` registry | 92-template dispatch, compatibility metadata, non-representable and unknown-message outcomes | 6-12 h |
| 159 | REST SBE negotiation | Opt-in headers, content-type dispatch, JSON fallback, errors, and existing REST model mapping | 8-16 h |
| 160 | WebSocket API SBE negotiation | Connection response mode, binary dispatch, request correlation, and existing socket model mapping | 8-16 h |
| 161 | User-data SBE | Subscription mode and complete current user-event mapping into existing Spot callbacks/models | 8-16 h |
| 162 | Complete 92-template parity matrix | Every current template mapped or explicitly non-representable; JSON/SBE value, null, enum, decimal and timestamp parity | 8-16 h |
| Review 37 | General SBE review | Complete schema/template/consumer audit, compatibility review and documentation correction | 2-4 h |
| 163 | Lifecycle and CI updater | Deprecation/retirement reporting, explicit promotion workflow, deterministic generation and public-API diff gates | 5-10 h |
| 164 | Testnet conformance and release gate | Opt-in live evidence, operational docs/examples, all legacy/new tests, pack contents, truthful package metadata | 4-8 h |

## Forecast and mandatory recalibration

The initial implementation envelope is 23 development slices plus five mandatory reviews: **28 small turns and 146-292 active hours**. At 40 focused hours per week this is roughly 3.7-7.3 weeks; at 20 hours per week it is roughly 7.3-14.6 weeks. Those are effort conversions, not a promised calendar date, and exclude waiting for credentials, user authorization, Binance/Testnet availability, or an upstream engine defect.

| Milestone | Cumulative minimum turns | Cumulative active budget |
| --- | ---: | ---: |
| Foundation/generation decision after Review 33 | 5 | 16-32 h |
| Complete safe text FIX after Review 34 | 11 | 54-108 h |
| Complete text, hybrid, and full FIX SBE after Review 35 | 16 | 79-158 h |
| Independent SBE market streams after Review 36 | 20 | 97-194 h |
| General REST/WebSocket/user-data SBE after Review 37 | 26 | 137-274 h |
| Release-gated current FIX/SBE program | 28 | 146-292 h |

Review 33 is the first mandatory re-estimation point because it replaces two current unknowns with executable evidence: whether the FIX dictionaries can be generated cleanly and whether QuickFIX/n can satisfy Binance’s no-ResendRequest rule without an unsafe fork. If either fails, downstream slice numbers and budgets must change before development continues.

## Definition of done

The FIX/SBE program is complete only when:

1. all current official role, message, template, transport, lifecycle, authentication, limit, and model contracts in this document are implemented or explicitly documented by Binance as non-representable;
2. generated registries exactly match the pinned current XML message/template sets, with no unowned template;
3. the existing JSON API remains backward compatible except for separately approved correctness breaks, and JSON/SBE outputs have deterministic semantic parity;
4. sequence gaps, ambiguous delivery, reconnects, timeouts, malformed frames, unknown versions, and credential/logging paths fail safely;
5. the full legacy suite plus all new deterministic protocol tests pass on every target framework and a forced solution build has zero errors;
6. opt-in Spot Testnet conformance evidence exists for each transport, with no production call or credential;
7. pack contents and public API are reviewed, documentation is executable and safe, and package metadata makes no claim broader than the tested implementation.
