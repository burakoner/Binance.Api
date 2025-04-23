namespace Binance.Net.Objects.Models.Futures
{
    /// <summary>
    /// Trading rules status
    /// </summary>
    public record BinanceFuturesTradingStatus
    {
        /// <summary>
        /// The trading rule indicators
        /// </summary>
        [JsonProperty("indicators")]
        public Dictionary<string, IEnumerable<BinanceFuturesTradingStatusIndicator>> Indicators { get; set; } = new Dictionary<string, IEnumerable<BinanceFuturesTradingStatusIndicator>>();
        /// <summary>
        /// Last update time
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonProperty("updateTime")]
        public DateTime UpdateTime { get; set; }
    }

    /// <summary>
    /// Indicator details
    /// </summary>
    public record BinanceFuturesTradingStatusIndicator
    {
        /// <summary>
        /// Locked
        /// </summary>
        [JsonProperty("isLocked")]
        public bool IsLocked { get; set; }
        /// <summary>
        /// Planned time when indicator is unlocked
        /// </summary>
        [JsonConverter(typeof(DateTimeConverter))]
        [JsonProperty("plannedRecoveryTime")]
        public DateTime? PlannedRecoveryTime { get; set; }
        /// <summary>
        /// The indicator name
        /// </summary>
        [JsonProperty("indicator")]
        public string Indicator { get; set; } = string.Empty;
        /// <summary>
        /// Current value of the indicator
        /// </summary>
        [JsonProperty("value")]
        public decimal Value { get; set; }
        /// <summary>
        /// The trigger value of the indicator
        /// </summary>
        [JsonProperty("triggerValue")]
        public decimal TriggerValue { get; set; }
    }
}
