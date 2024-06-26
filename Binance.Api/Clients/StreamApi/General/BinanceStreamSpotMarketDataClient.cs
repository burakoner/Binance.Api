﻿using Binance.Api.Models.RestApi.Blvt;

namespace Binance.Api.Clients.StreamApi.General;

public class BinanceStreamBlvtClient
{
    // BLVT
    private const string bltvInfoEndpoint = "@tokenNav";
    private const string bltvKlineEndpoint = "@nav_kline";

    // Internal References
    internal BinanceStreamGeneralClient MainClient { get; }
    internal ILogger Logger { get => MainClient.Logger; }
    internal string BaseAddress { get => Options.BaseAddress; }
    internal BinanceWebSocketApiClientOptions Options { get => MainClient.RootClient.ClientOptions; }
    internal CallResult<T> Deserialize<T>(string data, JsonSerializer serializer = null, int? requestId = null) => MainClient.Deserializer<T>(data, serializer, requestId);
    internal CallResult<T> Deserialize<T>(JToken obj, JsonSerializer serializer = null, int? requestId = null) => MainClient.Deserializer<T>(obj, serializer, requestId);
    internal Task<CallResult<WebSocketUpdateSubscription>> SubscribeAsync<T>(string url, IEnumerable<string> topics, Action<WebSocketDataEvent<T>> onData, CancellationToken ct)
    => MainClient.SubscribeAsync<T>(url, topics, onData, ct);

    internal BinanceStreamBlvtClient(BinanceStreamGeneralClient main)
    {
        MainClient = main;
    }

    #region Blvt info update
    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToBlvtInfoUpdatesAsync(string token,
        Action<WebSocketDataEvent<BinanceBlvtInfoUpdate>> onMessage, CancellationToken ct = default)
        => SubscribeToBlvtInfoUpdatesAsync(new List<string> { token }, onMessage, ct);

    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToBlvtInfoUpdatesAsync(IEnumerable<string> tokens, Action<WebSocketDataEvent<BinanceBlvtInfoUpdate>> onMessage, CancellationToken ct = default)
    {
        if (Options.BlvtStreamAddress == null)
            throw new Exception("No url found for Blvt stream, check the `BlvtStreamAddress` client option");

        tokens = tokens.Select(a => a.ToUpper(CultureInfo.InvariantCulture) + bltvInfoEndpoint).ToArray();
        var handler = new Action<WebSocketDataEvent<BinanceCombinedStream<BinanceBlvtInfoUpdate>>>(data => onMessage(data.As(data.Data.Data, data.Data.Data.TokenName)));
        return await SubscribeAsync(Options.BlvtStreamAddress, tokens, handler, ct).ConfigureAwait(false);
    }
    #endregion

    #region Blvt kline update
    public Task<CallResult<WebSocketUpdateSubscription>> SubscribeToBlvtKlineUpdatesAsync(string token,
        KlineInterval interval, Action<WebSocketDataEvent<BinanceStreamKlineData>> onMessage, CancellationToken ct = default) =>
        SubscribeToBlvtKlineUpdatesAsync(new List<string> { token }, interval, onMessage, ct);

    public async Task<CallResult<WebSocketUpdateSubscription>> SubscribeToBlvtKlineUpdatesAsync(IEnumerable<string> tokens, KlineInterval interval, Action<WebSocketDataEvent<BinanceStreamKlineData>> onMessage, CancellationToken ct = default)
    {
        if (Options.BlvtStreamAddress == null)
            throw new Exception("No url found for Blvt stream, check the `BlvtStreamAddress` client option");

        tokens = tokens.Select(a => a.ToUpper(CultureInfo.InvariantCulture) + bltvKlineEndpoint + "_" + JsonConvert.SerializeObject(interval, new KlineIntervalConverter(false))).ToArray();
        var handler = new Action<WebSocketDataEvent<BinanceCombinedStream<BinanceStreamKlineData>>>(data => onMessage(data.As(data.Data.Data, data.Data.Data.Symbol)));
        return await SubscribeAsync(Options.BlvtStreamAddress, tokens, handler, ct).ConfigureAwait(false);
    }
    #endregion

}