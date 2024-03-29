﻿namespace Binance.Api.Models.RestApi.SubAccount;

/// <summary>
/// Sub account historic transfer
/// </summary>
public class BinanceSubAccountTransferSubAccount
{
    /// <summary>
    /// Counter party of the transfer
    /// </summary>
    public string CounterParty { get; set; }
    /// <summary>
    /// Email of the account
    /// </summary>
    public string Email { get; set; }
    /// <summary>
    /// From account type
    /// </summary>
    public string FromAccountType { get; set; }
    /// <summary>
    /// To account type
    /// </summary>
    public string ToAccountType { get; set; }
    /// <summary>
    /// Status
    /// </summary>
    public string Status { get; set; }
    /// <summary>
    /// Transfer type
    /// </summary>
    [JsonConverter(typeof(SubAccountTransferSubAccountTypeConverter))]
    public SubAccountTransferSubAccountType Type { get; set; }
    /// <summary>
    /// Asset
    /// </summary>
    public string Asset { get; set; }

    /// <summary>
    /// Transaction id
    /// </summary>
    [JsonProperty("tranId")]
    public string TransactionId { get; set; }
    /// <summary>
    /// Quantity
    /// </summary>
    [JsonProperty("qty")]
    public decimal Quantity { get; set; }
    /// <summary>
    /// Timestamp of the transfer
    /// </summary>
    [JsonProperty("time"), JsonConverter(typeof(DateTimeConverter))]
    public DateTime Timestamp { get; set; }
}
