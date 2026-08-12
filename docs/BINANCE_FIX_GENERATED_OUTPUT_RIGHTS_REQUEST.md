# Binance FIX generated-output rights request

Status: copy-ready request prepared; no permission has been granted

Evidence baseline: `binance/binance-spot-api-docs` commit `b483413fcdf4da783cd3fcaad6fab7200a93297f`

This document is a coordination packet, not legal advice and not evidence of permission. Until a qualifying written response is accepted under the execution contract's Rights Gate 33A, the repository must not commit or distribute Binance-derived generated source or binaries.

## Copy-ready request

Subject: Redistribution terms for C# code generated from Binance Spot FIX schemas

Hello Binance API team,

We maintain the public open-source .NET wrapper at <https://github.com/burakoner/Binance.Api>. We want to implement the current Binance Spot FIX and FIX SBE protocols using schema files published in the official `binance/binance-spot-api-docs` repository.

The relevant source files at commit `b483413fcdf4da783cd3fcaad6fab7200a93297f` are:

- `fix/schemas/spot-fix-oe.xml`
- `fix/schemas/spot-fix-md.xml`
- `sbe/schemas/spot-fixsbe-1_1.xml`
- `sbe/schemas/spot_fix_prod_latest.xml`
- `sbe/schemas/spot_fix_testnet_latest.xml`
- `sbe/schemas/sbe_fix_schema_lifecycle_prod.json`
- `sbe/schemas/sbe_fix_schema_lifecycle_testnet.json`

We have not found a `LICENSE`, `LICENCE`, `COPYING`, or `NOTICE` file in that repository. Before distributing anything derived from these files, please clarify in writing whether Binance authorizes the following acts:

1. Generate C# source from the two text FIX dictionaries with QuickFIX/n DDTool and from the FIX SBE schema with Real Logic SbeTool.
2. Modify the generated C# only as needed for namespace and accessibility integration.
3. Commit that generated C# to our public Git repository.
4. Compile and distribute the generated implementation inside public NuGet assemblies.
5. Redistribute the raw XML/JSON inputs inside source archives or NuGet packages, if required at runtime or for deterministic regeneration.
6. Repeat the same process for future published versions of these Binance Spot FIX schema files.

If any of these acts are permitted, please identify the required license terms, copyright notices, attribution, source links, trademark restrictions, and any other redistribution conditions. If source redistribution and binary redistribution have different terms, please state both. Adding an explicit license or notice to the official schema repository would be the clearest resolution.

Thank you.

## Rights Gate 33A acceptance checklist

A response opens the gate only after the repository owner accepts documented evidence that:

- it comes from an attributable Binance-controlled repository, support case, or authorized Binance representative;
- it identifies the relevant schema files or an unambiguous class that includes them;
- it expressly covers generated/derivative C# source and compiled binary distribution, not merely use of the Binance API;
- it permits public Git and NuGet distribution, including the required modifications, or explicitly states the narrower permitted scope;
- it defines the applicable license, notices, attribution, and restrictions;
- the original response, URL or case identifier, respondent identity, and date can be archived with the gate decision.

Generic permission to use the API, the licenses of QuickFIX/n or SbeTool, and licenses attached to separate Binance sample repositories do not by themselves satisfy this checklist.

## Independent project-license decision

The repository root `LICENSE` contains Apache License 2.0, while `Binance.Api.csproj`, the current generated `Binance.Api` NuGet metadata, and `Binance.FIX.Api.csproj` declare `MIT`. Both declarations have historical provenance, so this packet does not guess which one the owner intended. The repository owner must reconcile that mismatch before any future package release and before accepting Binance terms that depend on the wrapper's outbound license.
