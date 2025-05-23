﻿namespace Binance.Api.Shared;

/// <summary>
/// Binance response
/// </summary>
public class BinanceResponse
{
    /// <summary>
    /// Identifier
    /// </summary>
    [JsonProperty("id")]
    public int Id { get; set; }

    /// <summary>
    /// Result status
    /// </summary>
    [JsonProperty("status")]
    public int Status { get; set; }

    /// <summary>
    /// Error info
    /// </summary>
    [JsonProperty("error")]
    public BinanceResponseError? Error { get; set; }

    /// <summary>
    /// Rate limit info
    /// </summary>
    [JsonProperty("rateLimits")]
    public List<BinanceCurrentRateLimit> Ratelimits { get; set; } = [];
}

/// <summary>
/// Binance response
/// </summary>
/// <typeparam name="T">Type of the data</typeparam>
public class BinanceResponse<T> : BinanceResponse
{
    /// <summary>
    /// Data result
    /// </summary>
    [JsonProperty("result")]
    public T Result { get; set; } = default!;
}

/// <summary>
/// Binance error response
/// </summary>
public class BinanceResponseError
{
    /// <summary>
    /// Error code
    /// </summary>
    [JsonProperty("code")]
    public int Code { get; set; }

    /// <summary>
    /// Error message
    /// </summary>
    [JsonProperty("msg")]
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Error data
    /// </summary>
    [JsonProperty("data")]
    public BinanceResponseErrorData? Data { get; set; }
}

/// <summary>
/// Error data
/// </summary>
public class BinanceResponseErrorData 
{
    /// <summary>
    /// Server time
    /// </summary>
    [JsonProperty("serverTime")]
    public DateTime? ServerTime { get; set; }

    /// <summary>
    /// Retry after time
    /// </summary>
    [JsonProperty("retryAfter")]
    public DateTime? RetryAfter { get; set; }
}
