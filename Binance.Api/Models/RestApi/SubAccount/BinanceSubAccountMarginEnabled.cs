﻿namespace Binance.ApiClient.Models.RestApi.SubAccount
{
    /// <summary>
    /// Sub account margin trading enabled
    /// </summary>
    public class BinanceSubAccountMarginEnabled
    {
        /// <summary>
        /// Email of the account
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Whether Margin trading is enabled
        /// </summary>
        public bool IsMarginEnabled { get; set; }
    }
}