namespace Binance.Api.Margin;

internal partial class BinanceMarginRestClient
{
    public Task<RestCallResult<BinanceMarginSpecialKeyCreateResult>> CreateMarginSpecialKeyAsync(
        string apiName,
        string? symbol = null,
        IEnumerable<string>? ipAddresses = null,
        string? publicKey = null,
        BinanceMarginSpecialKeyPermissionMode? permissionMode = null,
        int? receiveWindow = null,
        CancellationToken ct = default)
    {
        ValidateRequiredSpecialKeyValue(apiName, nameof(apiName));
        ValidateOptionalMarginSymbol(symbol, nameof(symbol));
        if (publicKey != null && string.IsNullOrWhiteSpace(publicKey))
            throw new ArgumentException("publicKey cannot be empty when provided", nameof(publicKey));

        var parameters = new ParameterCollection
        {
            { "apiName", apiName }
        };
        parameters.AddOptional("symbol", symbol);
        parameters.AddOptional("ip", SerializeSpecialKeyIpAddresses(ipAddresses, false));
        parameters.AddOptional("publicKey", publicKey);
        parameters.AddOptionalEnum("permissionMode", permissionMode);
        parameters.AddOptional("recvWindow", ValidateMarginReceiveWindow(receiveWindow));

        return RequestAsync<BinanceMarginSpecialKeyCreateResult>(
            GetUrl(sapi, v1, "margin/apiKey"),
            HttpMethod.Post,
            ct,
            true,
            bodyParameters: parameters,
            requestWeight: 1);
    }

    public async Task<RestCallResult<bool>> DeleteMarginSpecialKeyAsync(
        string? apiKey = null,
        string? apiName = null,
        string? symbol = null,
        int? receiveWindow = null,
        CancellationToken ct = default)
    {
        if (apiKey == null && apiName == null)
            throw new ArgumentException("apiKey or apiName is required; an unscoped Special Key deletion is not documented");
        ValidateOptionalSpecialKeyValue(apiKey, nameof(apiKey));
        ValidateOptionalSpecialKeyValue(apiName, nameof(apiName));
        ValidateOptionalMarginSymbol(symbol, nameof(symbol));

        var parameters = new ParameterCollection();
        parameters.AddOptional("apiKey", apiKey);
        parameters.AddOptional("apiName", apiName);
        parameters.AddOptional("symbol", symbol);
        parameters.AddOptional("recvWindow", ValidateMarginReceiveWindow(receiveWindow));

        var result = await RequestAsync<object>(
            GetUrl(sapi, v1, "margin/apiKey"),
            HttpMethod.Delete,
            ct,
            true,
            queryParameters: parameters,
            requestWeight: 1).ConfigureAwait(false);
        return result.As(result.Success);
    }

    public async Task<RestCallResult<bool>> UpdateMarginSpecialKeyIpAsync(
        string apiKey,
        IEnumerable<string> ipAddresses,
        string? symbol = null,
        int? receiveWindow = null,
        CancellationToken ct = default)
    {
        ValidateRequiredSpecialKeyValue(apiKey, nameof(apiKey));
        ValidateOptionalMarginSymbol(symbol, nameof(symbol));

        var parameters = new ParameterCollection
        {
            { "apiKey", apiKey },
            { "ip", SerializeSpecialKeyIpAddresses(ipAddresses, true)! }
        };
        parameters.AddOptional("symbol", symbol);
        parameters.AddOptional("recvWindow", ValidateMarginReceiveWindow(receiveWindow));

        var result = await RequestAsync<object>(
            GetUrl(sapi, v1, "margin/apiKey/ip"),
            HttpMethod.Put,
            ct,
            true,
            bodyParameters: parameters,
            requestWeight: 1).ConfigureAwait(false);
        return result.As(result.Success);
    }

    public async Task<RestCallResult<bool>> ExitMarginSpecialKeyModeAsync(
        int? receiveWindow = null,
        CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", ValidateMarginReceiveWindow(receiveWindow));

        var result = await RequestAsync<object>(
            GetUrl(sapi, v1, "margin/exit-special-key-mode"),
            HttpMethod.Post,
            ct,
            true,
            bodyParameters: parameters,
            requestWeight: 10).ConfigureAwait(false);
        return result.As(result.Success);
    }

    public Task<RestCallResult<BinanceMarginSpecialKey>> GetMarginSpecialKeyAsync(
        string apiKey,
        string? symbol = null,
        int? receiveWindow = null,
        CancellationToken ct = default)
    {
        ValidateRequiredSpecialKeyValue(apiKey, nameof(apiKey));
        ValidateOptionalMarginSymbol(symbol, nameof(symbol));

        var parameters = new ParameterCollection
        {
            { "apiKey", apiKey }
        };
        parameters.AddOptional("symbol", symbol);
        parameters.AddOptional("recvWindow", ValidateMarginReceiveWindow(receiveWindow));

        return RequestAsync<BinanceMarginSpecialKey>(
            GetUrl(sapi, v1, "margin/apiKey"),
            HttpMethod.Get,
            ct,
            true,
            queryParameters: parameters,
            requestWeight: 1);
    }

    public Task<RestCallResult<List<BinanceMarginSpecialKey>>> GetMarginSpecialKeysAsync(
        string? symbol = null,
        int? receiveWindow = null,
        CancellationToken ct = default)
    {
        ValidateOptionalMarginSymbol(symbol, nameof(symbol));

        var parameters = new ParameterCollection();
        parameters.AddOptional("symbol", symbol);
        parameters.AddOptional("recvWindow", ValidateMarginReceiveWindow(receiveWindow));

        return RequestAsync<List<BinanceMarginSpecialKey>>(
            GetUrl(sapi, v1, "margin/api-key-list"),
            HttpMethod.Get,
            ct,
            true,
            queryParameters: parameters,
            requestWeight: 1);
    }

    private static string? SerializeSpecialKeyIpAddresses(IEnumerable<string>? ipAddresses, bool required)
    {
        if (ipAddresses == null)
        {
            if (required)
                throw new ArgumentNullException(nameof(ipAddresses));
            return null;
        }

        var values = ipAddresses.ToArray();
        if (values.Length is < 1 or > 30)
            throw new ArgumentOutOfRangeException(nameof(ipAddresses), "ipAddresses must contain between 1 and 30 entries");
        if (values.Any(string.IsNullOrWhiteSpace))
            throw new ArgumentException("ipAddresses cannot contain an empty entry", nameof(ipAddresses));
        if (values.Any(ipAddress => ipAddress.Contains(',')))
            throw new ArgumentException("an individual IP address cannot contain a comma", nameof(ipAddresses));
        return string.Join(",", values);
    }

    private static void ValidateRequiredSpecialKeyValue(string value, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"{parameterName} is required", parameterName);
    }

    private static void ValidateOptionalSpecialKeyValue(string? value, string parameterName)
    {
        if (value != null)
            ValidateRequiredSpecialKeyValue(value, parameterName);
    }
}
