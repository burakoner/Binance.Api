# Spot FIX and SBE implementation execution contract

Status: approved living execution contract; Slice 146d2 complete, Review 33B next, generated-output redistribution blocked

Evidence date: 2026-08-12

Owning inventory: Slice 118

Completed implementation slices: Slices 142-145 and Slices 146a-146d2; completed review: Review 33

Next action: Review 33B across Slices 146a-146d2. Next external gate: generated-output Rights Gate 33A before Slice 147 or any generated codec enters Git/package output.

## Brutal current state

`Binance.FIX.Api` is still not a usable Binance FIX wrapper. Slice 142 removed its empty public placeholder and false completeness claims, made it non-packable, and upgraded its then-sole runtime dependency to QuickFIX/n Core 1.14.1. A non-packable `Binance.SBE.Api` foundation and dedicated protocol test projects now exist; Slice 143 pins the exact FIX-owned upstream inputs without redistributing them; Slice 144 proves that FIX SBE `1:1` can be generated deterministically, internalized, and compiled across every current consumer target; Slice 145 proves the two text FIX dictionaries can share a Binance-owned internal field catalog and separate role namespaces while compiling against the unchanged published QuickFIX/n Core package. Review 33 found no foundation/generator regression and proved a contained QuickFIX/n no-`ResendRequest` fail-close path, but did not invent redistribution rights over Binance-derived generated output. Slices 146a-146d2 now provide an internal common text-FIX session core: immutable endpoints/options, Ed25519 Logon signing, mandatory TLS/SNI validation, exact local role budgets, native heartbeat/TestRequest/Logout behavior, cancellation-safe bounded stop, explicit ambiguous-delivery classification, and fail-closed sequence-gap handling. The core is intentionally internal and uses no generated role dictionary or typed application-message dispatcher. There is still no public role client, committed generated message, decoder, live conformance evidence, or usable FIX/SBE wrapper. A successful solution build still proves neither protocol complete.

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

The official schema repository was pinned at commit `b483413fcdf4da783cd3fcaad6fab7200a93297f` for this inventory. Hashes below are over the exact `raw.githubusercontent.com` response bytes at that commit, not checkout files that Git may rewrite from LF to CRLF.

| Artifact | Current identity | Raw bytes | SHA-256 at the pinned commit |
| --- | --- | ---: | --- |
| `fix/schemas/spot-fix-oe.xml` | FIX 4.4 Order Entry dictionary, 19 messages | 24,513 | `55891d2ae2c7b5a5e9dbec0003ddcfd3034ba5846e0b7fa6561defa8f44797e9` |
| `fix/schemas/spot-fix-md.xml` | FIX 4.4 Market Data dictionary, 14 messages | 12,374 | `b18432105ae64f24acc49e1ff1e357811ccfb5a84c7475cfba7347ad9e30a2ec` |
| `sbe/schemas/spot-fixsbe-1_1.xml` | FIX SBE `1:1`, 29 templates | 48,186 | `6b44afe558d439ddc94323fd6ff026a949184ddd79a2cf033f610194c2537e70` |
| `sbe/schemas/stream_1_0.xml` | independent market stream `1:0`, 4 templates | 6,192 | `6ea328467e144311b1f1efff38e9fe613829997f041dd02a3b7077885d10a1f7` |
| `sbe/schemas/spot_3_5.xml` | general Spot SBE `3:5`, 92 templates | 140,961 | `542776a038883dafe962341a041323ff5d8d3e36f1b860ab60156a88a4080bcf` |
| `sbe/schemas/sbe_schema_lifecycle_prod.json` | current general schema `3:5` | 1,306 | `c1a67dad4092414746272709a2da8365d7dd0f0bfcd1b4ac2d65ab5f4aeeeefc` |
| `sbe/schemas/sbe_fix_schema_lifecycle_prod.json` | current FIX SBE schema `1:1` | 355 | `8c1b60d87c86510af1dad77d0cdac6d462be4d39a6ec1dba4175f4d8af076c1a` |

`eng/binance-spot-fix-schema-lock.json` is the executable lock for the seven FIX-owned inputs: both text dictionaries, FIX SBE `1:1`, production/testnet aliases, and production/testnet lifecycle files. The maintainer-only downloader verifies every size, hash, identity, count, alias, and lifecycle fact in memory before writing any file, refuses output inside the repository worktree, and refuses existing targets unless `-Force` is explicit. Normal restore, build, and test never call the network.

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

Slice 142 made both protocol projects non-packable, removed the FIX package's false completeness claim, upgraded QuickFIX/n Core to 1.14.1, and removed the generic `QuickFIXn.FIX44` package. Packaging remains disabled until the final release gate. The current Binance dictionaries are custom FIX 4.4 dictionaries and must own their message factory.

Slice 146b reuses the solution's existing MIT-licensed NSec.Cryptography dependency for Ed25519 rather than implementing cryptography locally. Version `25.4.0` is the latest stable NSec release supporting the package's required `net8.0` and `net9.0` targets through one coherent asset set. Official NuGet metadata shows `26.4.0` targets `net9.0` and fails the required `net8.0` restore, so it is not a compatible upgrade. The current official NuGet vulnerability feed reports no known vulnerable direct or transitive package in `Binance.FIX.Api`. This decision must be revisited when the package target matrix changes, not bypassed with per-target signing behavior.

## Generated-schema contract

Generated wire code is required; hand-maintaining 29, 4, and 92 SBE templates is rejected as an avoidable correctness risk.

- Real Logic/Aeron SbeTool C# generation is pinned to release `1.39.0`, commit `e773b57cac6b2008ce30dd219a33de49766c6013`.
- SbeTool remains a maintainer-only dependency. Slice 144 pins the current patched Eclipse Temurin JDK `17.0.20+8`, the Apache-2.0 `sbe-all` 1.39.0 executable, the exact matching seven C# runtime sources, and the official runtime's `System.Memory` 4.5.3 support assembly by URI, byte length, and SHA-256. The portable JDK lives outside the repository and does not replace the workstation's Java 8 installation.
- The exact Binance FIX SBE `1:1` schema generates 84 C# files and 1,498,986 bytes with manifest SHA-256 `91e6310676c8bbcb0b5978ee28577069abbf4ccac6e16f685ee6cf52c88710c5`; two independent runs are byte-for-byte identical. The generated plus matching runtime sources compile warning-free for `netstandard2.0`, `netstandard2.1`, `net8.0`, `net9.0`, and `net10.0` with an offline NuGet source set.
- SbeTool emits one reviewed warning because `MarketDataIncrementalTrade.MDEntries` deliberately uses `groupSize32Encoding` with `uint32 numInGroup`, rather than the tool's recommended `uint8`/`uint16`. That exact warning is allowlisted; a missing, changed, or additional warning fails generation. The schema also contains 56 Binance `mbx:exponent` extension attributes referencing `Exponent`, `PriceExponent`, or `QtyExponent`, so it is intentionally not valid against the unextended standard SBE XSD. The original XML is generated unchanged; decimal metadata ownership remains explicit for Slice 151 mapping.
- The official repository publishes no `LICENSE`, `COPYING`, or `NOTICE` file. The linked [Binance Terms](https://www.binance.com/en/terms) and [Testnet Terms](https://www.binance.com/en/about-legal/terms-testnets) do not expressly grant redistribution of these raw artifacts. This is a fail-closed project decision, not legal advice: raw Binance XML/JSON files are not committed, packed, or copied into Git history. Exact commit, URI, byte length, hash, and reviewed identity are committed instead, with an explicit maintainer-only verified downloader.
- SbeTool emits all 84 top-level codec types as `public`. The selected package boundary performs exactly one deterministic top-level `public` to `internal` accessibility change per generated file, changes no member/wire code, and proves zero exported types after compilation. Any generator output that does not match that exact 84-replacement manifest fails closed.
- Generated C# must not be committed or released merely because generation succeeds. Review 33 refreshed official repository HEAD `b483413fcdf4da783cd3fcaad6fab7200a93297f` and confirmed that it still has no `LICENSE`, `COPYING`, or `NOTICE`. Binance's separately MIT-licensed SBE sample repositories demonstrate redistribution only for the files in those repositories; they do not expressly license C# output derived from the separate FIX dictionaries or FIX SBE schema. Review 33 therefore denied repository/package entry under the current evidence. Slice 144 continues to generate and compile only in an external temporary workspace, and normal restore/build/test remains offline.
- Rights Gate 33A requires an explicit license or permission covering redistribution of the relevant generated works, or a documented legal review accepted by the repository owner. Risk acceptance alone does not create third-party rights. After that gate is satisfied and generated output is committed, CI regenerates into a temporary directory and fails on a diff. It does not rewrite the worktree.
- Generated namespaces include schema identity. Generated types are implementation detail, not stable high-level business API; Slice 144 must prove the selected internalization boundary before consumer mapping begins.
- Every frame is length-checked before decode. Schema ID, schema version, template ID, block length, repeating-group counts, variable data lengths, null values, enum values, and microsecond timestamps receive deterministic boundary tests.
- Decoder tests include independent byte fixtures and field-offset assertions. Encoder-to-decoder round trips alone are insufficient evidence because the same generator can reproduce the same defect on both sides.

QuickFIX/n DDTool 1.14.1 cannot consume the two official Binance FIX dictionaries together unchanged because both identify as `FIX44`, and its normal generation path overwrites `QuickFIXn/Fields/Fields.cs` plus `FieldTags.cs`. Slice 145 accepts DDTool only behind a bounded adapter: the exact dictionaries receive distinct generation-only `customname` values; the resulting 148-field union is moved into `Binance.FIX.Api.Internal.Fields`; Order Entry and Market Data receive separate internal namespaces and factories; and the two DDTool writes into a disposable extracted source tree are never treated as a Core fork.

The exact current dictionaries produce 39 C# files: two shared field files, 21 Order Entry sources, and 16 Market Data sources. Two independent Windows runs produced identical raw and adapted manifests. The adapter performs exactly 148 field-class, one tag-table, and 37 top-level message/factory accessibility changes; compilation against the untouched `QuickFIXn.Core` 1.14.1 assemblies succeeds warning-free for `net8.0`, `net9.0`, and `net10.0`. Reflection verifies all 19 Order Entry and 14 Market Data messages and both factories, 205 defined internal implementation types, and zero exported types. The maintainer verifier also fails on field name/tag/type conflicts, case-sensitive message inventory drift, unexpected DDTool source writes, manifest drift, or package/source hash drift.

The pinned QuickFIX/n source and NuGet package use the QuickFIX Software License 1.0, but that does not resolve rights over Binance-derived generated output. As with SBE, no generated text FIX source, dictionary, tool archive, NuGet binary, or generated assembly is committed. Review 33 kept the rights gate closed and moved its explicit unblock criteria to Rights Gate 33A. Review 33 separately proved the contained QuickFIX/n session adapter described below; that engine decision no longer blocks Slice 146.

## FIX session safety contract

QuickFIX/n may own the text FIX state machine and TLS transport only after all of these behaviors are locked by tests:

- TLS is mandatory. Slice 146c projects the immutable role endpoint into the same exact hostname for direct `SocketConnectHost` and `SSLServerName`, fixes port 9000, enables QuickFIX/n SSL, disables system-proxy tunneling, and locks certificate and online revocation validation to `true`. No public host, proxy, certificate-authority, protocol, validation, or revocation override exists. QuickFIX/n 1.14.1 passes `SSLServerName` directly to `SslStream.AuthenticateAsClient`, so it owns both SNI and hostname validation. Tests invoke that pinned internal factory over loopback: a matching certificate under the configured test CA succeeds and exposes the exact official SNI, while a hostname mismatch and a certificate from a different CA fail authentication. The ephemeral test CA has no CRL/OCSP service, so only the in-process custom-CA handshake disables revocation internally to isolate name/chain behavior; production projection remains revocation-on and is asserted before that test-only override.
- Only Ed25519 credentials are accepted. Slice 146b imports unencrypted PKCS#8 Ed25519 PEM directly into the cryptographic provider, keeps no PEM string, clears temporary private/signature byte buffers, and rejects use after disposal. Before any validation/signing attempt, the authenticator clears every Logon field it owns so a failed retry cannot retain stale credentials. Logon signs the exact SOH-joined ASCII `35`, `49`, `56`, `34`, and `52` values only after QuickFIX/n has populated and validated the header. It then applies `95`, `96`, `98=0`, `108`, `141=Y`, `553`, `25035`, plus role-correct `25036` and `9406`. The generic signing operation and API key are not public. Exact SOH-delimited tags `96` and `553` are redacted by the structured wire redactor; raw wire logging remains off because no production logger or session exists yet.
- Order Entry, Drop Copy, and Market Data use distinct client types, endpoints, permissions, connection counters, rate limits, and message catalogs. A generic public `Send(Message)` escape hatch is out of scope.
- Heartbeat is constrained to the official 5-60 second range and defaults to 30 seconds. Slice 146d2 binds it to QuickFIX/n's session state machine and proves Heartbeat, TestRequest, and timeout-disconnect paths without wall-clock sleeps.
- `MessageHandling=SEQUENTIAL` is the shared safe default. Order Entry and Drop Copy default to `ResponseMode=EVERYTHING`; Market Data's current dictionary does not define that field. Unordered handling and acknowledgement-only responses require explicit opt-in and documented consequences on roles that support them.
- Logon and session reset use the documented sequence contract. Slice 146d2 disables message persistence, resets sequence state on logon/logout/disconnect, and attempts graceful Logout for at most the smaller of one heartbeat or ten seconds before force-stop. Only an inbound `35=5` completes the graceful signal; QuickFIX/n's generic `OnLogout` callback cannot, because it also represents network loss.
- Binance documents `ResendRequest` as unsupported. QuickFIX/n 1.14.1 automatically generates one when the inbound sequence is too high. Review 33 proved on the unchanged 1.14.1 NuGet package that `IApplication.ToAdmin` observes the generated `35=2` before persistence or responder send. Slice 146d2 moves that exact disconnect-plus-`DoNotSend` behavior into the production application adapter and also fail-closes an inbound `ResendRequest`. The regression still proves one disconnect, zero wire writes, zero persisted messages, unchanged sender sequence, and unchanged expected target sequence; any engine upgrade must rerun it before acceptance.
- No order mutation is automatically replayed after timeout, disconnect, reconnect, or ambiguous write completion. Slice 146d2 rejects mutation handoff while logged out, disables QuickFIX message persistence, and returns only `NotSent` or explicit `UnknownDelivery`; even a completed transport write remains `UnknownDelivery` until execution-report, Drop Copy, or REST reconciliation.
- Client order IDs and list IDs are never reused automatically. Reconnect creates a fresh session identity according to the current official rules.
- Slice 146d1 exposes the exact current per-role message, connection-attempt, concurrent-account, and Market Data stream limits as immutable metadata. Slice 146d2 binds the outbound counter immediately before every QuickFIX `ToAdmin`/`ToApp` send and binds the connection counter immediately before every actual `DoConnect`. Outbound counters remain per connection; same-process sessions can share one validated account-role connection budget so constructing another session cannot multiply capacity. Other processes remain invisible, and Binance server rejections, LimitResponse data, logout, and disconnect remain authoritative; local observations are not exchange truth.
- RecvWindow uses the transport’s documented unit and precision: milliseconds with up to three decimal places for text FIX and microseconds for FIX SBE. The 60-second ceiling is fail-closed.
- Slice 146d2 makes stop/disposal one-way and idempotent, consumes cancellation as a bounded force-stop request, prevents restart, stops the engine before destroying Ed25519 key material, and cannot race a reconnect into re-authenticating a user-requested closed session.

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
| 143 | License-safe schema lock and provenance | No raw redistribution; exact upstream commit/URI/size/hash/id/version lock; strict alias/lifecycle conflict tests; verified maintainer download only; no network in normal build | 3-6 h |
| 144 | Pinned SbeTool C# feasibility pipeline | JDK 17 generation-only toolchain, ignored temporary output, deterministic generation proof, all-consuming-TFM compile matrix, internalization decision; no generated output committed before Review 33 rights decision | 4-8 h |
| 145 | Binance FIX dictionary generation spike | Both custom FIX 4.4 dictionaries generate, compile, and validate without modifying QuickFIX/n NuGet source | 5-10 h |
| Review 33 | Foundation review and re-estimate | Review Slices 142-145; decide schema/generated-output rights and whether QuickFIX/n remains viable before session implementation | 2-4 h |
| 146a | Immutable text FIX session contract | Explicit production/Testnet selection, exact role endpoint, immutable SenderCompID, heartbeat validation, sequential default, and role-correct response-mode isolation | 2-3 h |
| 146b | Ed25519 logon and credential boundary | Exact SOH payload/signature vector, Ed25519-only key acceptance, required logon fields, signature/API-key redaction | 2-4 h |
| 146c | Mandatory TLS transport projection | QuickFIX/n SSL enablement, exact endpoint hostname for TCP and SNI, hostname/certificate/revocation validation with no public disable switch, in-process TLS rejection tests | 2-4 h |
| 146d1 | Immutable role limits and local budget primitive | Exact message/connection-attempt/concurrent/stream values, monotonic rolling-window boundary, non-consuming observation, and thread-safe acquisition; no session integration | 1-2 h |
| 146d2 | Session lifecycle and budget integration | Heartbeat/test/logout, outbound/connection budget binding, cancellation/disposal, ambiguous delivery boundary, and productionized sequence-gap/no-resend fail-close behavior | 1-3 h |
| Review 33B | Common session-core review and gate | Backward review of Slices 146a-146d2 across code, tests, documentation, dependency/TLS/key lifecycle, and forward scope before generated-code work | 1-2 h |
| Rights Gate 33A | Generated-output redistribution authority | Written license/permission or documented legal review before any Binance-derived generated source enters Git/package output; blocks Slice 147 and all generated-code slices, but not Slice 146 | external wait |
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

The initial implementation envelope was 23 development slices plus five mandatory reviews: **28 small turns and 146-292 active hours**. Review 33 split the 8-16 hour Slice 146 into four smaller turns without increasing its active-hour envelope; Slice 146d was later split into two smaller turns inside the same 2-5 hour envelope. The required four-to-five-turn backward-review cadence adds Review 33B after the five resulting common-session turns and before any generated-code gate. Slices 142-145, Slices 146a-146d2, and Review 33 are complete. The current implementation/review envelope is **23 small turns and 123-246 active hours**. At 40 focused hours per week this is roughly 3.1-6.2 weeks; at 20 focused hours per week it is roughly 6.2-12.3 weeks. These are effort conversions, not a promised calendar date. Review 33B is next; Rights Gate 33A then blocks the remaining generated-code chain, and its external wait has no honest calendar estimate. Credential/user authorization, Binance/Testnet availability, and any future upstream defect are also excluded.

| Remaining milestone | Minimum remaining turns | Remaining active budget |
| --- | ---: | ---: |
| Review the complete common text FIX session core before Rights Gate 33A | 1 | 1-2 h |
| Complete safe text FIX after Review 34 | 6 plus external rights gate | 31-62 h plus external wait |
| Complete text, hybrid, and full FIX SBE after Review 35 | 11 plus external rights gate | 56-112 h plus external wait |
| Independent SBE market streams after Review 36 | 15 plus external rights gate | 74-148 h plus external wait |
| General REST/WebSocket/user-data SBE after Review 37 | 21 plus external rights gate | 114-228 h plus external wait |
| Release-gated current FIX/SBE program | 23 plus external rights gate | 123-246 h plus external wait |

Review 33 inspected `b4aa5ef..3ea34da` and accepted the four-slice foundation/generator work without correction. QuickFIX/n 1.14.1 remains viable because the exact no-`ResendRequest` fail-close path is locked by a deterministic regression. The generated-output rights decision failed closed: no relevant redistribution license or permission was found, so Binance-derived generated output remains external and Rights Gate 33A blocks Slice 147 onward. Slices 146a-146d2 now complete the internal common text-FIX session core without contacting Binance: exact endpoints/options, Ed25519 Logon, direct TLS/SNI validation, native liveness/logout, operational local budgets, bounded disposal, non-replay, and explicit ambiguous delivery. Review 33B must now inspect all five common-session turns before work stops at Rights Gate 33A unless qualifying rights evidence exists.

## Definition of done

The FIX/SBE program is complete only when:

1. all current official role, message, template, transport, lifecycle, authentication, limit, and model contracts in this document are implemented or explicitly documented by Binance as non-representable;
2. generated registries exactly match the pinned current XML message/template sets, with no unowned template;
3. the existing JSON API remains backward compatible except for separately approved correctness breaks, and JSON/SBE outputs have deterministic semantic parity;
4. sequence gaps, ambiguous delivery, reconnects, timeouts, malformed frames, unknown versions, and credential/logging paths fail safely;
5. the full legacy suite plus all new deterministic protocol tests pass on every target framework and a forced solution build has zero errors;
6. opt-in Spot Testnet conformance evidence exists for each transport, with no production call or credential;
7. pack contents and public API are reviewed, documentation is executable and safe, and package metadata makes no claim broader than the tested implementation.
