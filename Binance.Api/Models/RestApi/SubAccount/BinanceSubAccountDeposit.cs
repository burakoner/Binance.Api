﻿using Binance.Api.Wallet;

namespace Binance.Api.Models.RestApi.SubAccount;

/// <summary>
/// Information about a deposit
/// </summary>
public record BinanceSubAccountDeposit
{
    /// <summary>
    /// Time the deposit was added to Binance
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime InsertTime { get; set; }
    /// <summary>
    /// The quantity deposited
    /// </summary>
    [JsonProperty("amount")]
    public decimal Quantity { get; set; }
    /// <summary>
    /// The asset deposited
    /// </summary>
    [JsonProperty("coin")]
    public string Asset { get; set; } = "";
    /// <summary>
    /// Network
    /// </summary>
    public string Network { get; set; } = "";
    /// <summary>
    /// The address of the deposit
    /// </summary>
    public string Address { get; set; } = "";
    /// <summary>
    /// The address tag
    /// </summary>
    public string AddressTag { get; set; } = "";
    /// <summary>
    /// The transaction id
    /// </summary>
    [JsonProperty("txId")]
    public string TransactionId { get; set; } = "";
    /// <summary>
    /// Confirmation status
    /// </summary>
    public string ConfirmTimes { get; set; } = "";
    /// <summary>
    /// Transfer type
    /// </summary>
    public int TransferType { get; set; }
    /// <summary>
    /// The status of the deposit
    /// </summary>
    public BinanceWalletDepositStatus Status { get; set; }
}
