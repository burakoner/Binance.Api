﻿namespace Binance.Net.Objects.Models.Futures
{
    /// <summary>
    /// Price statistics of the last 24 hours
    /// </summary>
    public record BinanceFuturesCoin24HPrice : Binance24HPriceBase
    {
        /// <summary>
        /// The pair the price is for
        /// </summary>
        [JsonProperty("pair")]
        public string Pair { get; set; } = string.Empty;

        /// <inheritdoc />
        [JsonProperty("baseVolume")]
        public override decimal Volume { get; set; }
        /// <inheritdoc />
        [JsonProperty("volume")]
        public override decimal QuoteVolume { get; set; }
    }
}
