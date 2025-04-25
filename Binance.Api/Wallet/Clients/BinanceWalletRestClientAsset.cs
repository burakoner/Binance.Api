namespace Binance.Api.Wallet;

internal partial class BinanceWalletRestClient
{
    public Task<RestCallResult<Dictionary<string, BinanceAssetDetails>>> GetAssetDetailsAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<Dictionary<string, BinanceAssetDetails>>(GetUrl(sapi, v1, "asset/assetDetail"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<IEnumerable<BinanceWalletBalance>>> GetWalletBalancesAsync(string quoteAsset = "BTC", int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddParameter("quoteAsset", quoteAsset);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<IEnumerable<BinanceWalletBalance>>(GetUrl(sapi, v1, "asset/wallet/balance"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 60);
    }

    public Task<RestCallResult<IEnumerable<BinanceUserBalance>>> GetBalancesAsync(string? asset = null, bool? needBtcValuation = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));
        parameters.AddOptional("asset", asset);
        parameters.AddOptional("needBtcValuation", needBtcValuation);

        return RequestAsync<IEnumerable<BinanceUserBalance>>(GetUrl(sapi, v1, "asset/getUserAsset"), HttpMethod.Post, ct, true, queryParameters: parameters, requestWeight: 5);
    }

    public Task<RestCallResult<BinanceTransaction>> TransferAsync(BinanceUniversalTransferType type, string asset, decimal quantity, string? fromSymbol = null, string? toSymbol = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "asset", asset },
            { "amount", quantity.ToString(CultureInfo.InvariantCulture) }
        };
        parameters.AddEnum("type", type);
        parameters.AddOptional("fromSymbol", fromSymbol);
        parameters.AddOptional("toSymbol", toSymbol);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceTransaction>(GetUrl(sapi, v1, "asset/transfer"), HttpMethod.Post, ct, true, queryParameters: parameters, requestWeight: 900);
    }

    public Task<RestCallResult<BinanceQueryRecords<BinanceTransfer>>> GetTransfersAsync(BinanceUniversalTransferType type, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddEnum("type", type);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("current", page?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptional("size", pageSize?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceQueryRecords<BinanceTransfer>>(GetUrl(sapi, v1, "asset/transfer"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceWalletBnbBurnStatus>> SetBnbBurnStatusAsync(bool? spotTrading = null, bool? marginInterest = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        if (spotTrading == null && marginInterest == null)
            throw new ArgumentException("SpotTrading or MarginInterest should be provided");

        var parameters = new ParameterCollection();
        parameters.AddOptional("spotBNBBurn", spotTrading);
        parameters.AddOptional("interestBNBBurn", marginInterest);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceWalletBnbBurnStatus>(GetUrl(sapi, v1, "bnbBurn"), HttpMethod.Post, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceEligibleDusts>> GetAssetsForDustTransferAsync(BinanceAccountType? accountType = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptionalEnum("accountType", accountType);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceEligibleDusts>(GetUrl(sapi, v1, "asset/dust-btc"), HttpMethod.Post, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceDustTransferResult>> DustTransferAsync(IEnumerable<string> assets, BinanceAccountType? accountType = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var assetsArray = assets.ToArray();

        assetsArray.ValidateNotNull(nameof(assets));
        foreach (var asset in assetsArray) asset.ValidateNotNull(nameof(asset));

        var parameters = new ParameterCollection()
        {
            { "asset", assetsArray }
        };
        parameters.AddOptionalEnum("accountType", accountType);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceDustTransferResult>(GetUrl(sapi, v1, "asset/dust"), HttpMethod.Post, ct, true, queryParameters: parameters, requestWeight: 10);
    }

    public Task<RestCallResult<BinanceDustLogList>> GetDustLogAsync(DateTime? startTime = null, DateTime? endTime = null, BinanceAccountType? accountType = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptionalEnum("accountType", accountType);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);

        return RequestAsync<BinanceDustLogList>(GetUrl(sapi, v1, "asset/dribblet"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceQueryRecords<BinanceDividendRecord>>> GetAssetDividendRecordsAsync(string? asset = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("asset", asset);
        parameters.AddOptional("limit", limit);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceQueryRecords<BinanceDividendRecord>>(GetUrl(sapi, v1, "asset/assetDividend"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10);
    }

    public Task<RestCallResult<IEnumerable<BinanceTradeFee>>> GetTradeFeeAsync(string? symbol = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("symbol", symbol);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<IEnumerable<BinanceTradeFee>>(GetUrl(sapi, v1, "asset/tradeFee"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<IEnumerable<BinanceFundingAsset>>> GetFundingWalletAsync(string? asset = null, bool? needBtcValuation = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("asset", asset);
        parameters.AddOptional("needBtcValuation", needBtcValuation?.ToString());
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<IEnumerable<BinanceFundingAsset>>(GetUrl(sapi, v1, "asset/get-funding-asset"), HttpMethod.Post, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public Task<RestCallResult<BinanceQueryRecords<BinanceCloudMiningHistory>>> GetCloudMiningHistoryAsync(DateTime startTime, DateTime endTime, long? transferId = null, string? clientTransferId = null, string? asset = null, int? page = null, int? pageSize = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("tranId", transferId);
        parameters.AddOptional("clientTranId", clientTransferId);
        parameters.AddOptional("asset", asset);
        parameters.AddOptional("current", page);
        parameters.AddOptional("size", pageSize);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceQueryRecords<BinanceCloudMiningHistory>>(GetUrl(sapi, v1, "asset/ledger-transfer/cloud-mining/queryByPage"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 600);
    }

    public Task<RestCallResult<IEnumerable<BinanceDelistSchedule>>> GetDelistScheduleAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<IEnumerable<BinanceDelistSchedule>>(GetUrl(sapi, v1, "spot/delist-schedule"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 100);
    }
}