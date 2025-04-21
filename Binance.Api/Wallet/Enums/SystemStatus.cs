namespace Binance.Api.Wallet.Enums
{
    /// <summary>
    /// Status of the Binance system
    /// </summary>
    public enum SystemStatus:byte
    {
        /// <summary>
        /// Operational
        /// </summary>
        [Map("0")]
        Normal=0,

        /// <summary>
        /// In maintenance
        /// </summary>
        [Map("1")]
        Maintenance=1
    }
}
