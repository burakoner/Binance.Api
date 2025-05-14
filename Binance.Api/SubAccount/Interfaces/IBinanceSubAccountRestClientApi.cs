namespace Binance.Api.SubAccount;

/// <summary>
/// Interface for the Binance Sub-Account API Rest API client.
/// </summary>
public interface IBinanceSubAccountRestClientApi
{
    /// <summary>
    /// Get the ip restriction for a sub-account
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#get-ip-restriction-for-a-sub-account-api-key-for-master-account" /></para>
    /// </summary>
    /// <param name="email">The sub account email</param>
    /// <param name="apiKey">The sub account api key</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceSubAccountIpRestriction>> GetApiKeyIpRestrictionAsync(string email, string apiKey, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Remove the ip restriction for a sub-account
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#delete-ip-list-for-a-sub-account-api-key-for-master-account" /></para>
    /// </summary>
    /// <param name="email">The sub account email</param>
    /// <param name="apiKey">The sub account api key</param>
    /// <param name="ipAddresses">Addresses to remove from whitelist</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceSubAccountIpRestriction>> RemoveApiKeyIpRestrictionAsync(string email, string apiKey, IEnumerable<string>? ipAddresses, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Update the ip restriction for a sub-account
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#update-ip-restriction-for-sub-account-api-key-for-master-account" /></para>
    /// </summary>
    /// <param name="email">The sub account email</param>
    /// <param name="apiKey">The sub account api key</param>
    /// <param name="ipRestrict">Enable or disable ip restrictions</param>
    /// <param name="ipAddresses">Addresses to whitelist</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceSubAccountIpRestriction>> SetApiKeyIpRestrictionAsync(string email, string apiKey, bool ipRestrict, IEnumerable<string>? ipAddresses, int? receiveWindow = null, CancellationToken ct = default);
}