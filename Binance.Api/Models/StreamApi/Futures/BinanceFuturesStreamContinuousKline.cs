using Binance.ApiClient.Converters;
using Binance.ApiClient.Enums;
using Binance.ApiClient.Interfaces;
using Newtonsoft.Json;
using Binance.ApiClient.Models.StreamApi;
using Binance.ApiClient.Models.StreamApi.MarketData;

namespace Binance.ApiClient.Models.zzz.Futures.Socket
{
    /// <summary>
    /// Wrapper for continuous kline information for a symbol
    /// </summary>
    public class BinanceStreamContinuousKlineData : BinanceStreamEvent, IBinanceStreamKlineData
    {
        /// <summary>
        /// The symbol the data is for
        /// </summary>
        [JsonProperty("ps")]
        public string Symbol { get; set; }

        /// <summary>
        /// The contract type
        /// </summary>
        [JsonProperty("ct")]
        public ContractType ContractType { get; set; } = ContractType.Unknown;

        /// <summary>
        /// The data
        /// </summary>
        [JsonProperty("k")]
        [JsonConverter(typeof(InterfaceConverter<BinanceStreamKline>))]
        public IBinanceStreamKline Data { get; set; } = default!;
    }
}
