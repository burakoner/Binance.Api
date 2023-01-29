using Binance.Api;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Binance.ApiConsole
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            /** /
            var ws = new BinanceStreamClient();
            await ws.SubscribeToAllMiniTickerUpdatesAsync((data) =>
            {
                foreach (var d in data.Data)
                {
                    Console.WriteLine($"{d.Symbol} O:{d.OpenPrice} H:{d.HighPrice} L:{d.LowPrice} C:{d.LastPrice} V:{d.Volume}");
                }
                Console.WriteLine($"Got {data.Data.Count()} rows");
            }, default);
            /**/

            Console.WriteLine("Ready!..");
            Console.ReadLine();
            //await ws.UnsubscribeAllAsync();

            var api = new BinanceRestApiClient(new BinanceRestApiClientOptions
            {
                OutputOriginalData= true,
            });
            await api.Spot.Server.GetServerTimeAsync();
            var i = 0;
            var sw01 = Stopwatch.StartNew();
            while (true)
            {
                i++;
                var date = await api.Spot.Server.GetServerTimeAsync();
                Console.WriteLine($"{i} Response:{date.Data}  -  Duration:{date.ResponseTime.Value.TotalMilliseconds}ms  -  Avg:{sw01.ElapsedMilliseconds / i}ms");
            }

            Console.WriteLine("Done!..");
            Console.ReadLine();
        }
    }
}