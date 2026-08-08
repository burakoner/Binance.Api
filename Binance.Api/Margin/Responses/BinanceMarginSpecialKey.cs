namespace Binance.Api.Margin;

/// <summary>
/// Margin Special Key metadata. This class deliberately does not use record-generated value
/// formatting so logging the object does not print its API key.
/// </summary>
public sealed class BinanceMarginSpecialKey
{
    /// <summary>
    /// API name.
    /// </summary>
    public string ApiName { get; set; } = string.Empty;

    /// <summary>
    /// API key.
    /// </summary>
    public string ApiKey { get; set; } = string.Empty;

    /// <summary>
    /// Comma-separated IP restrictions as returned by Binance.
    /// </summary>
    [JsonProperty("ip")]
    public string IpAddresses { get; set; } = string.Empty;

    /// <summary>
    /// Key type as returned by Binance.
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Permission mode as returned by Binance.
    /// </summary>
    public string PermissionMode { get; set; } = string.Empty;
}

/// <summary>
/// Result of creating a Margin Special Key. This class deliberately does not use record-generated
/// value formatting so logging the object does not print newly issued key material.
/// </summary>
public sealed class BinanceMarginSpecialKeyCreateResult
{
    /// <summary>
    /// Newly issued API key.
    /// </summary>
    public string ApiKey { get; set; } = string.Empty;

    /// <summary>
    /// Newly issued secret. Binance documents this as null for RSA keys.
    /// </summary>
    public string? SecretKey { get; set; }

    /// <summary>
    /// Key type as returned by Binance.
    /// </summary>
    public string Type { get; set; } = string.Empty;
}
