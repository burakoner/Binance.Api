namespace Binance.Api.Options.RestApi;

public class BinanceRestApiEuropeanClientOptions
{
    // Base Address
    public string BaseAddress { get; set; }

    public BinanceRestApiEuropeanClientOptions()
    {
        // Base Address
        BaseAddress = "https://eapi.binance.com";
    }
}