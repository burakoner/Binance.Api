namespace Binance.Api.Wallet;

internal partial class BinanceWalletRestApiClient
{
    public Task<RestCallResult<IEnumerable<BinanceUserAsset>>> GetUserAssetsAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<IEnumerable<BinanceUserAsset>>(GetUrl(sapi, v1, "capital/config/getall"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10);
    }

    public Task<RestCallResult<BinanceWithdrawalPlaced>> WithdrawAsync(string asset, string address, decimal quantity, string? withdrawOrderId = null, string? network = null, string? addressTag = null, string? name = null, bool? transactionFeeFlag = null, BinanceWalletType? walletType = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        asset.ValidateNotNull(nameof(asset));
        address.ValidateNotNull(nameof(address));

        var parameters = new ParameterCollection
        {
            { "coin", asset },
            { "address", address },
            { "amount", quantity.ToString(CultureInfo.InvariantCulture) }
        };
        parameters.AddOptional("name", name);
        parameters.AddOptional("withdrawOrderId", withdrawOrderId);
        parameters.AddOptional("network", network);
        parameters.AddOptional("transactionFeeFlag", transactionFeeFlag);
        parameters.AddOptional("addressTag", addressTag);
        parameters.AddOptionalEnum("walletType", walletType);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceWithdrawalPlaced>(GetUrl(sapi, v1, "capital/withdraw/apply"), HttpMethod.Post, ct, true, queryParameters: parameters, requestWeight: 900);
    }

    public Task<RestCallResult<IEnumerable<BinanceWithdrawal>>> GetWithdrawalHistoryAsync(string? asset = null, string? withdrawOrderId = null, BinanceWithdrawalStatus? status = null, DateTime? startTime = null, DateTime? endTime = null, int? receiveWindow = null, int? limit = null, int? offset = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("coin", asset);
        parameters.AddOptional("withdrawOrderId", withdrawOrderId);
        parameters.AddOptionalEnum("status", status);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));
        parameters.AddOptional("limit", limit);
        parameters.AddOptional("offset", offset);

        return RequestAsync<IEnumerable<BinanceWithdrawal>>(GetUrl(sapi, v1, "capital/withdraw/history"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 18000);
    }

    public Task<RestCallResult<IEnumerable<BinanceWithdrawalAddress>>> GetWithdrawalAddressesAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<IEnumerable<BinanceWithdrawalAddress>>(GetUrl(sapi, v1, "capital/withdraw/address/list"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10);
    }


    public Task<RestCallResult<IEnumerable<BinanceDeposit>>> GetDepositHistoryAsync(string? asset = null, BinanceDepositStatus? status = null, DateTime? startTime = null, DateTime? endTime = null, int? offset = null, int? limit = null, int? receiveWindow = null, bool includeSource = false, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("coin", asset);
        parameters.AddOptional("offset", offset?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptional("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalEnum("status", status);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));
        parameters.AddOptional("includeSource", includeSource.ToString());

        return RequestAsync<IEnumerable<BinanceDeposit>>(GetUrl(sapi, v1, "capital/deposit/hisrec"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceDepositAddress>> GetDepositAddressAsync(string asset, string? network = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        asset.ValidateNotNull(nameof(asset));

        var parameters = new ParameterCollection
        {
            { "coin", asset }
        };
        parameters.AddOptional("network", network);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceDepositAddress>(GetUrl(sapi, v1, "capital/deposit/address"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10);
    }
}