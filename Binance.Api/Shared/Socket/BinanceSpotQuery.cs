/*
namespace Binance.Api.Shared;

internal class BinanceSpotQuery<T>(BinanceSocketQuery request, bool authenticated, int weight = 1) : Query<T> where T : BinanceResponse
{
    public override HashSet<string> ListenerIdentifiers { get; set; } = [request.Id.ToString()];

    public override CallResult<T> HandleMessage(WebSocketConnection connection, WebSocketDataEvent<T> message)
    {
        if (message.Data.Status != 200)
        {
            if (message.Data.Status == 418 || message.Data.Status == 429)
            {
                // Rate limit error 
                return new CallResult<T>(new BinanceRateLimitError(message.Data.Error!.Code, message.Data.Error!.Message, null)
                {
                    RetryAfter = message.Data.Error.Data!.RetryAfter
                }, message.Raw);
            }

            return new CallResult<T>(new ServerError(message.Data.Error!.Code, message.Data.Error!.Message), message.Raw);
        }

        return new CallResult<T>(message.Data, message.Raw);
    }
}
*/