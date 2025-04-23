namespace Binance.Net.Objects.Models.Futures
{
    /// <summary>
    /// If the new user info
    /// </summary>
    public record BinanceIfNewUser
    {
        /// <summary>
        /// Broker Id
        /// </summary>
        [JsonProperty("brokerId")]
        public string BrokerId { get; set; } = string.Empty;
        /// <summary>
        /// If the apiAgentCode is working
        /// </summary>
        [JsonProperty("rebateWorking")]
        public bool RebateWorking { get; set; }
        /// <summary>
        /// If new user
        /// </summary>
        [JsonProperty("ifNewUser")]
        public bool IfNewUser { get; set; }
    }
}
