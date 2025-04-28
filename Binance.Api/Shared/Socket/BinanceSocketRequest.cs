namespace Binance.Api.Shared;

internal class BinanceSocketMessage
{
    [JsonProperty("method")]
    public string Method { get; set; } = string.Empty;

    [JsonProperty("id")]
    public int Id { get; set; }
}

internal class BinanceSocketRequest : BinanceSocketMessage
{
    [JsonProperty("params")]
    public string[] Params { get; set; } = [];
}

internal class BinanceSocketQuery : BinanceSocketMessage
{
    [JsonProperty("params")]
    public Dictionary<string, object> Params { get; set; } = [];
}
