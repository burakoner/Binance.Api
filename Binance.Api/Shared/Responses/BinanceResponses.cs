namespace Binance.Api.Shared;

#region Binance Result
/// <summary>
/// Binance Result with Rate Limits
/// </summary>
/// <typeparam name="T">Type of the data</typeparam>
public class BinanceResultWithRateLimits<T>
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
    public BinanceResultError? Error { get; set; }

    /// <summary>
    /// Data result
    /// </summary>
    [JsonProperty("result")]
    public T Result { get; set; } = default!;

    /// <summary>
    /// Rate limit info
    /// </summary>
    [JsonProperty("rateLimits")]
    public List<BinanceCurrentRateLimit> Ratelimits { get; set; } = [];
}

/// <summary>
/// Binance error response
/// </summary>
public class BinanceResultError
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
    public BinanceResultErrorData? Data { get; set; }
}

/// <summary>
/// Error data
/// </summary>
public class BinanceResultErrorData 
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
#endregion

#region Binance Response
/// <summary>
/// Binance Response
/// </summary>
internal record BinanceResponse
{
    /// <summary>
    /// Result code
    /// </summary>
    [JsonProperty("code")]
    public int Code { get; set; }

    /// <summary>
    /// Message
    /// </summary>
    [JsonProperty("msg")]
    public string? Message { get; set; }
}

/// <summary>
/// Query result
/// </summary>
/// <typeparam name="T"></typeparam>
internal record BinanceResponse<T> : BinanceResponse
{
    /// <summary>
    /// The data
    /// </summary>
    [JsonProperty("data")]
    public T Data { get; set; } = default!;
}
#endregion

/// <summary>
/// Binance error response
/// </summary>
internal class BinanceDataResponse<T>
{
    /// <summary>
    /// Error code
    /// </summary>
    [JsonProperty("code")]
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Error message
    /// </summary>
    [JsonProperty("msg")]
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Error data
    /// </summary>
    [JsonProperty("data")]
    public T? Data { get; set; }

    /// <summary>
    /// Success
    /// </summary>
    [JsonProperty("success")]
    public bool Success { get; set; }
}

#region Binance List Responses
/// <summary>
/// List Result
/// </summary>
/// <typeparam name="T"></typeparam>
internal record BinanceListResponse<T>
{
    /// <summary>
    /// The Data
    /// </summary>
    [JsonProperty("list")]
    public List<T> List { get; set; } = [];
}

/// <summary>
/// List Response with Total Count
/// </summary>
/// <typeparam name="T"></typeparam>
public record BinanceListTotalResponse<T>
{
    /// <summary>
    /// The Data
    /// </summary>
    [JsonProperty("list")]
    public List<T> List { get; set; } = [];

    /// <summary>
    /// The total count of the records
    /// </summary>
    [JsonProperty("total")]
    public int Total { get; set; }
}

/// <summary>
/// List Response with Toal Count and Time Range
/// </summary>
/// <typeparam name="T"></typeparam>
public record BinanceListRangeResponse<T>
{
    /// <summary>
    /// The Data
    /// </summary>
    [JsonProperty("list")]
    public List<T> List { get; set; } = [];

    /// <summary>
    /// Data start time
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    [JsonProperty("startTime")]
    public DateTime StartTime { get; set; }

    /// <summary>
    /// Emd to,e
    /// </summary>
    [JsonConverter(typeof(DateTimeConverter))]
    [JsonProperty("endTime")]
    public DateTime EndTime { get; set; }

    /// <summary>
    /// Limit
    /// </summary>
    [JsonProperty("limit")]
    public int Limit { get; set; }

    /// <summary>
    /// More data available
    /// </summary>
    [JsonProperty("moreData")]
    public bool MoreData { get; set; }
}
#endregion

/// <summary>
/// Rows Results
/// </summary>
/// <typeparam name="T"></typeparam>
public record BinanceRowsResult<T>
{
    /// <summary>
    /// The list records
    /// </summary>
    public List<T> Rows { get; set; } = [];

    /// <summary>
    /// The total count of the records
    /// </summary>
    public int Total { get; set; }
}

/// <summary>
/// Binance Response Data and Total Number
/// </summary>
/// <typeparam name="T"></typeparam>
public record BinanceDataTotalResponse<T>
{
    /// <summary>
    /// Code
    /// </summary>
    [JsonProperty("code")]
    public string Code { get; set; } = "";

    /// <summary>
    /// Message
    /// </summary>
    [JsonProperty("message")]
    public string Message { get; set; } = "";

    /// <summary>
    /// Success
    /// </summary>
    [JsonProperty("success")]
    public bool Success { get; set; }

    /// <summary>
    /// Data
    /// </summary>
    [JsonProperty("data")]
    public T Data { get; set; } = default!;

    /// <summary>
    /// Total Count
    /// </summary>
    [JsonProperty("total")]
    public int Total { get; set; }
}
