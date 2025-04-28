/*
namespace Binance.Api.Shared;

internal class BinanceSystemQuery<T> : Query<T> where T: BinanceSocketQueryResponse
{
    public override HashSet<string> ListenerIdentifiers { get; set; }

    public BinanceSystemQuery(BinanceSocketRequest request, bool authenticated, int weight = 1) : base(request, authenticated, weight)
    {
        ListenerIdentifiers = new HashSet<string> { request.Id.ToString() };
    }
}
*/