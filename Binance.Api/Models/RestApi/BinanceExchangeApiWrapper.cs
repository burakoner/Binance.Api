namespace Binance.Api.Models.RestApi;

internal class BinanceExchangeApiWrapper<T>
{
    public int Code { get; set; }
    public string Message { get; set; }
    public string MessageDetail { get; set; }

    public T Data { get; set; } = default!;

    public bool Success { get; set; }
}
