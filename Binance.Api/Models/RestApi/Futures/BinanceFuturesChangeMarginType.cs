﻿namespace Binance.Net.Objects.Models.Futures
{
    /// <summary>
    /// Result from a change margin type request
    /// </summary>
    public record BinanceFuturesChangeMarginTypeResult
    {
        /// <summary>
        /// Response code
        /// </summary>
        [JsonProperty("code")]
        public int Code { get; set; }
        /// <summary>
        /// Response message
        /// </summary>
        [JsonProperty("msg")]
        public string? Message { get; set; }
    }
}
