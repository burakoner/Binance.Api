namespace Binance.Api.Broker;

/// <summary>
/// Interface for the Binance Link and Trade (Fast API) Rest API client.
/// </summary>
public interface IBinanceBrokerRestClientLinkAndTradeFastApi
{

    /// <summary>
    /// Get User Status
    /// <para><a href="https://developers.binance.com/docs/binance_link/link-and-trade/fast-api/get-user-status" /></para>
    /// </summary>
    /// <param name="accessToken">Access Token</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceBrokerFastUserStatus>> GetUserStatusAsync(string accessToken, CancellationToken ct = default);

    /// <summary>
    /// Create API key for User
    /// <para><a href="https://developers.binance.com/docs/binance_link/link-and-trade/fast-api/create-api-key" /></para>
    /// </summary>
    /// <param name="accessToken">Access Token</param>
    /// <param name="apiName">custom api name, can be repeated</param>
    /// <param name="enableTrade">true or false</param>
    /// <param name="enableFutureTrade">true or false</param>
    /// <param name="enableMargin">true or false</param>
    /// <param name="enableEuropeanOptions">true or false</param>
    /// <param name="publicKey">Your custom public key. The server side will use it to encrypt the apikey, and client decrypts with a private key. Use RSA algorithm, the keysize is customized with client, usually 1024 or 2048 (Reference).</param>
    /// <param name="apiKeyPublicKey">The public key for asymmetrical key type(Ed25519 and RSA). Please see https://www.binance.com/en/support/faq/detail/6b9a63f1e3384cf48a2eedb82767a69a</param>
    /// <param name="status">1 or null = Access IP unrestricted, 2 = Trusted IPs only, 3 = Trusted Third Party IPs only. If you want to enable any Trade permission(including Spot, Margin or Futures), you must restrict the IP access.</param>
    /// <param name="ipAddress">Trusted IPs. Can be added in batches, separated by commas. Max 30 for an API key</param>
    /// <param name="thirdPartyName">Third party name must match the name in the server side.</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceBrokerFastApiKey>> CreateApiKeyForUserAsync(
        string accessToken,
        string apiName,
        bool enableTrade,
        bool enableFutureTrade,
        bool enableMargin,
        bool enableEuropeanOptions,
        string publicKey,
        string? apiKeyPublicKey = null,
        BinanceBrokerApiKeyIpRestriction? status = null,
        string? ipAddress = null,
        string? thirdPartyName = null,
        CancellationToken ct = default);
}