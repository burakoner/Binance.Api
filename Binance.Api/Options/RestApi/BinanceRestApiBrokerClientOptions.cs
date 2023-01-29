namespace Binance.Api.Options.RestApi;

public class BinanceRestApiBrokerClientOptions
{
    // Base Address
    public string BaseAddress { get; set; }

    public BinanceRestApiBrokerClientOptions()
    {
        // Base Address
        BaseAddress = "https://api.binance.com";
    }
}
