using Binance.Api.Futures;

namespace Binance.Net.Objects.Models.Futures
{
    /// <summary>
    /// Buy/sell volume ratio
    /// </summary>
    public record BinanceFuturesCoinBuySellVolumeRatio
    {
        /// <summary>
        /// The pair
        /// </summary>
        [JsonProperty("pair")]
        public string Pair { get; set; } = string.Empty;
        /// <summary>
        /// Contract type
        /// </summary>
        [JsonProperty("contractType")]
        public BinanceContractType ContractType { get; set; }
        /// <summary>
        /// The taker buy volume
        /// </summary>
        [JsonProperty("takerBuyVol")]
        public decimal TakerBuyVolume { get; set; }
        /// <summary>
        /// The taker sell volume
        /// </summary>
        [JsonProperty("takerSellVol")]
        public decimal TakerSellVolume { get; set; }
        /// <summary>
        /// The taker buy value
        /// </summary>
        [JsonProperty("takerBuyVolValue")]
        public decimal TakerBuyVolumeValue { get; set; }
        /// <summary>
        /// The taker sell value
        /// </summary>
        [JsonProperty("takerSellVolValue")]
        public decimal TakerSellVolumeValue { get; set; }
        /// <summary>
        /// Data timestamp
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }
    }
}
