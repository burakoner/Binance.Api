﻿using Binance.Api.Shared.Responses;

namespace Binance.Api.Models.RestApi.FuturesAlgoOrders;

/// <summary>
/// Algo order result
/// </summary>
public record BinanceAlgoOrderResult : BinanceResult
{
    /// <summary>
    /// Order id
    /// </summary>
    public string ClientAlgoId { get; set; } = "";
    /// <summary>
    /// Successful
    /// </summary>
    public bool Success { get; set; }
}
