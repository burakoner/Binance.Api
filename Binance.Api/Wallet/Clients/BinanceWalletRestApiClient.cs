namespace Binance.Api.Wallet;

/// <summary>
/// Binance Wallet Rest API Client
/// </summary>
/// <param name="root">Parent</param>
public class BinanceWalletRestApiClient(BinanceRestApiClient root)
{
    // Api
    private const string v1 = "1";
    private const string v3 = "3";
    private const string api = "api";
    private const string sapi = "sapi";

    // Parent
    internal BinanceRestApiClient _ { get; } = root;

    // Internal
    internal ILogger Logger => Logger;
    internal BinanceRestApiClientOptions Options => Options;
    internal DateTime? LastExchangeInfoUpdate { get; private set; }
    internal BinanceExchangeInfo? ExchangeInfo { get; private set; }

    #region Internal Methods
    internal Task<RestCallResult<T>> RequestAsync<T>(
        Uri uri, HttpMethod method, CancellationToken cancellationToken, bool signed = false,
        Dictionary<string, object>? queryParameters = null,
        Dictionary<string, object>? bodyParameters = null,
        Dictionary<string, string>? headerParameters = null,
        ArraySerialization? serialization = null,
        JsonSerializer? deserializer = null,
        bool ignoreRatelimit = false,
        int requestWeight = 1) where T : class
        => _.RequestAsync<T>(uri, method, cancellationToken, signed, queryParameters, bodyParameters, headerParameters, serialization, deserializer, ignoreRatelimit, requestWeight);

    internal Uri GetUrl(string api, string version, string endpoint)
    {
        var url = Options.BaseAddress.AppendPath(api);
        if (!string.IsNullOrEmpty(version)) url = url.AppendPath($"v{version}");
        if (!string.IsNullOrEmpty(endpoint)) url = url.AppendPath($"{endpoint}");

        return new Uri(url);
    }
    #endregion

    #region Capital Methods
    /// <summary>
    /// Gets information of assets for a user
    /// <para><a href="https://developers.binance.com/docs/wallet/capital" /></para>
    /// </summary>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Assets info</returns>
    public Task<RestCallResult<IEnumerable<BinanceUserAsset>>> GetUserAssetsAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<IEnumerable<BinanceUserAsset>>(GetUrl(sapi, v1, "capital/config/getall"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10);
    }

    /// <summary>
    /// Withdraw assets from Binance to an address
    /// <para><a href="https://developers.binance.com/docs/wallet/capital/withdraw" /></para>
    /// </summary>
    /// <param name="asset">The asset to withdraw, for example `ETH`</param>
    /// <param name="address">The address to send the funds to</param>
    /// <param name="addressTag">Secondary address identifier for assets like XRP,XMR etc.</param>
    /// <param name="withdrawOrderId">Custom client order id</param>
    /// <param name="transactionFeeFlag">When making internal transfer, true for returning the fee to the destination account; false for returning the fee back to the departure account. Default false.</param>
    /// <param name="quantity">The quantity to withdraw</param>
    /// <param name="network">The network to use</param>
    /// <param name="walletType">The wallet type for withdraw</param>
    /// <param name="name">Description of the address</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Withdrawal confirmation</returns>
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

    /// <summary>
    /// Gets the withdrawal history
    /// <para><a href="https://developers.binance.com/docs/wallet/capital/withdraw-history" /></para>
    /// </summary>
    /// <param name="asset">Filter by asset, for example `ETH`</param>
    /// <param name="withdrawOrderId">Filter by withdraw order id</param>
    /// <param name="status">Filter by status</param>
    /// <param name="startTime">Filter start time from</param>
    /// <param name="endTime">Filter end time till</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <param name="limit">Add limit. Default: 1000, Max: 1000</param>
    /// <param name="offset">Add offset</param>
    /// <returns>List of withdrawals</returns>
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

    /// <summary>
    /// Get list of withdrawal addresses
    /// <para><a href="https://developers.binance.com/docs/wallet/capital/fetch-withdraw-address" /></para>
    /// </summary>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    public Task<RestCallResult<IEnumerable<BinanceWithdrawalAddress>>> GetWithdrawalAddressesAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<IEnumerable<BinanceWithdrawalAddress>>(GetUrl(sapi, v1, "capital/withdraw/address/list"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10);
    }

    // TODO: Fetch withdraw quota (USER_DATA)

    /// <summary>
    /// Gets the deposit history
    /// <para><a href="https://developers.binance.com/docs/wallet/capital/deposite-history" /></para>
    /// </summary>
    /// <param name="asset">Filter by asset, for example `ETH`</param>
    /// <param name="status">Filter by status</param>
    /// <param name="limit">Amount of results</param>
    /// <param name="offset">Offset the results</param>
    /// <param name="startTime">Filter start time from</param>
    /// <param name="endTime">Filter end time till</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="includeSource">Include source address to response</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of deposits</returns>
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

    /// <summary>
    /// Gets the deposit address for an asset
    /// <para><a href="https://developers.binance.com/docs/wallet/capital/deposite-address" /></para>
    /// </summary>
    /// <param name="asset">Asset to get address for, for example `ETH`</param>
    /// <param name="network">Network</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Deposit address</returns>
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

    // TODO: Fetch deposit address list with network(USER_DATA)
    // TODO: One click arrival deposit apply (for expired address deposit) (USER_DATA)

    #endregion

    #region Asset Methods
    /// <summary>
    /// Gets the withdraw/deposit details for an asset
    /// <para><a href="https://developers.binance.com/docs/wallet/asset" /></para>
    /// </summary>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Asset detail</returns>
    public Task<RestCallResult<Dictionary<string, BinanceAssetDetails>>> GetAssetDetailsAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<Dictionary<string, BinanceAssetDetails>>(GetUrl(sapi, v1, "asset/assetDetail"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    /// <summary>
    /// Receive balances of the different user wallets
    /// <para><a href="https://developers.binance.com/docs/wallet/asset/query-user-wallet-balance" /></para>
    /// </summary>
    /// <param name="quoteAsset">Quote asset, for example `USDT`, `ETH`, `USDC`, `BNB`, etc. default `BTC`</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    public Task<RestCallResult<IEnumerable<BinanceWalletBalance>>> GetWalletBalancesAsync(string quoteAsset = "BTC", int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddParameter("quoteAsset", quoteAsset);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<IEnumerable<BinanceWalletBalance>>(GetUrl(sapi, v1, "asset/wallet/balance"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 60);
    }

    /// <summary>
    /// Retrieve balance info
    /// <para><a href="https://developers.binance.com/docs/wallet/asset/user-assets" /></para>
    /// </summary>
    /// <param name="asset">Return for this asset, for example `ETH`</param>
    /// <param name="needBtcValuation">Whether the response should include the BtcValuation. If false (default) BtcValuation will be 0.</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    public Task<RestCallResult<IEnumerable<BinanceUserBalance>>> GetBalancesAsync(string? asset = null, bool? needBtcValuation = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));
        parameters.AddOptional("asset", asset);
        parameters.AddOptional("needBtcValuation", needBtcValuation);

        return RequestAsync<IEnumerable<BinanceUserBalance>>(GetUrl(sapi, v1, "asset/getUserAsset"), HttpMethod.Post, ct, true, queryParameters: parameters, requestWeight: 5);
    }

    /// <summary>
    /// Transfers between accounts
    /// <para><a href="https://developers.binance.com/docs/wallet/asset/user-universal-transfer" /></para>
    /// </summary>
    /// <param name="type">The type of transfer</param>
    /// <param name="asset">The asset to transfer, for example `ETH`</param>
    /// <param name="quantity">The quantity to transfer</param>
    /// <param name="fromSymbol">From symbol when transferring from/to isolated margin</param>
    /// <param name="toSymbol">To symbol when transferring from/to isolated margin</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
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

    /// <summary>
    /// Get transfer history
    /// <para><a href="https://developers.binance.com/docs/wallet/asset/query-user-universal-transfer" /></para>
    /// </summary>
    /// <param name="type">The type of transfer</param>
    /// <param name="startTime">Filter by startTime</param>
    /// <param name="endTime">Filter by endTime</param>
    /// <param name="page">The page</param>
    /// <param name="pageSize">Results per page</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
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

    /// <summary>
    /// Sets the status of the BNB burn switch for spot trading and margin interest
    /// <para><a href="https://developers.binance.com/docs/wallet/asset/Toggle-BNB-Burn-On-Spot-Trade-And-Margin-Interest" /></para>
    /// </summary>
    /// <param name="spotTrading">If BNB burning should be enabled for spot trading</param>
    /// <param name="marginInterest">If BNB burning should be enabled for margin interest</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    public Task<RestCallResult<BinanceBnbBurnStatus>> SetBnbBurnStatusAsync(bool? spotTrading = null, bool? marginInterest = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        if (spotTrading == null && marginInterest == null)
            throw new ArgumentException("SpotTrading or MarginInterest should be provided");

        var parameters = new ParameterCollection();
        parameters.AddOptional("spotBNBBurn", spotTrading);
        parameters.AddOptional("interestBNBBurn", marginInterest);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceBnbBurnStatus>(GetUrl(sapi, v1, "bnbBurn"), HttpMethod.Post, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    /// <summary>
    /// Get assets that can be converted to BNB
    /// <para><a href="https://developers.binance.com/docs/wallet/asset/assets-can-convert-bnb" /></para>
    /// </summary>
    /// <param name="accountType">Spot or Margin account</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    public Task<RestCallResult<BinanceEligibleDusts>> GetAssetsForDustTransferAsync(BinanceAccountType? accountType = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptionalEnum("accountType", accountType);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceEligibleDusts>(GetUrl(sapi, v1, "asset/dust-btc"), HttpMethod.Post, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    /// <summary>
    /// Converts dust (small amounts of) assets to BNB 
    /// <para><a href="https://developers.binance.com/docs/wallet/asset/dust-transfer" /></para>
    /// </summary>
    /// <param name="assets">The assets to convert to BNB, for example `ETH`</param>
    /// <param name="accountType">Spot or Margin account</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Dust transfer result</returns>
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

    /// <summary>
    /// Gets the history of dust conversions
    /// <para><a href="https://developers.binance.com/docs/wallet/asset/dust-log" /></para>
    /// </summary>
    /// <param name="startTime">The start time</param>
    /// <param name="endTime">The end time</param>
    /// <param name="accountType">Spot or Margin account</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The history of dust conversions</returns>
    public Task<RestCallResult<BinanceDustLogList>> GetDustLogAsync(DateTime? startTime = null, DateTime? endTime = null, BinanceAccountType? accountType = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptionalEnum("accountType", accountType);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);

        return RequestAsync<BinanceDustLogList>(GetUrl(sapi, v1, "asset/dribblet"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    /// <summary>
    /// Get asset dividend records
    /// <para><a href="https://developers.binance.com/docs/wallet/asset/assets-divided-record" /></para>
    /// </summary>
    /// <param name="asset">Filter by asset, for example `ETH`</param>
    /// <param name="startTime">Filter by start time from</param>
    /// <param name="endTime">Filter by end time till</param>
    /// <param name="limit">Page size</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Dividend records</returns>
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

    /// <summary>
    /// Gets the trade fee for a symbol
    /// <para><a href="https://developers.binance.com/docs/wallet/asset/trade-fee" /></para>
    /// </summary>
    /// <param name="symbol">Symbol to get withdrawal fee for, for example `ETHUSDT`</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Trade fees</returns>
    public Task<RestCallResult<IEnumerable<BinanceTradeFee>>> GetTradeFeeAsync(string? symbol = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("symbol", symbol);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<IEnumerable<BinanceTradeFee>>(GetUrl(sapi, v1, "asset/tradeFee"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    /// <summary>
    /// Get funding wallet assets
    /// <para><a href="https://developers.binance.com/docs/wallet/asset/funding-wallet" /></para>
    /// </summary>
    /// <param name="asset">Filter by asset, for example `ETH`</param>
    /// <param name="needBtcValuation">Return BTC valuation</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of assets</returns>
    public Task<RestCallResult<IEnumerable<BinanceFundingAsset>>> GetFundingWalletAsync(string? asset = null, bool? needBtcValuation = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("asset", asset);
        parameters.AddOptional("needBtcValuation", needBtcValuation?.ToString());
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<IEnumerable<BinanceFundingAsset>>(GetUrl(sapi, v1, "asset/get-funding-asset"), HttpMethod.Post, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    /// <summary>
    /// Get the query of Cloud-Mining payment and refund history
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#get-cloud-mining-payment-and-refund-history-user_data" /></para>
    /// </summary>
    /// <param name="transferId">Filter by transferId</param>
    /// <param name="clientTransferId">Filter by clientTransferId</param>
    /// <param name="asset">Filter by asset, for example `ETH`</param>
    /// <param name="startTime">Filter by start time</param>
    /// <param name="endTime">Filter by end time</param>
    /// <param name="page">Page</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
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

    // TODO: Query User Delegation History(For Master Account)(USER_DATA)

    /// <summary>
    /// Get spot symbols delist schedule
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#get-symbols-delist-schedule-for-spot-market_data" /></para>
    /// </summary>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    public Task<RestCallResult<IEnumerable<BinanceDelistSchedule>>> GetDelistScheduleAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<IEnumerable<BinanceDelistSchedule>>(GetUrl(sapi, v1, "spot/delist-schedule"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 100);
    }

    // TODO: Get Open Symbol List (MARKET_DATA)

    #endregion

    #region Account Methods
    /// <summary>
    /// Get account VIP level and margin/futures enabled status
    /// <para><a href="https://developers.binance.com/docs/wallet/account" /></para>
    /// </summary>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    public Task<RestCallResult<BinanceVipLevelAndStatus>> GetAccountVipLevelAndStatusAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceVipLevelAndStatus>(GetUrl(sapi, v1, "account/info"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    /// <summary>
    /// Get a daily account snapshot (balances)
    /// <para><a href="https://developers.binance.com/docs/wallet/account/daily-account-snapshoot" /></para>
    /// </summary>
    /// <param name="startTime">The start time</param>
    /// <param name="endTime">The end time</param>
    /// <param name="limit">The amount of days to retrieve</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    public Task<RestCallResult<IEnumerable<BinanceSpotAccountSnapshot>>> GetDailySpotAccountSnapshotAsync(
        DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null,
        CancellationToken ct = default)
        => GetDailyAccountSnapshot<IEnumerable<BinanceSpotAccountSnapshot>>(BinanceAccountType.Spot, startTime, endTime, limit, receiveWindow, ct);

    /// <summary>
    /// Get a daily account snapshot (balances)
    /// <para><a href="https://developers.binance.com/docs/wallet/account/daily-account-snapshoot" /></para>
    /// </summary>
    /// <param name="startTime">The start time</param>
    /// <param name="endTime">The end time</param>
    /// <param name="limit">The amount of days to retrieve</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    public Task<RestCallResult<IEnumerable<BinanceMarginAccountSnapshot>>> GetDailyMarginAccountSnapshotAsync(
        DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null,
        CancellationToken ct = default)
        => GetDailyAccountSnapshot<IEnumerable<BinanceMarginAccountSnapshot>>(BinanceAccountType.Margin, startTime, endTime, limit, receiveWindow, ct);

    /// <summary>
    /// Get a daily account snapshot (balances)
    /// <para><a href="https://developers.binance.com/docs/wallet/account/daily-account-snapshoot" /></para>
    /// </summary>
    /// <param name="startTime">The start time</param>
    /// <param name="endTime">The end time</param>
    /// <param name="limit">The amount of days to retrieve</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    public Task<RestCallResult<IEnumerable<BinanceFuturesAccountSnapshot>>> GetDailyFutureAccountSnapshotAsync(
        DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null,
        CancellationToken ct = default)
        => GetDailyAccountSnapshot<IEnumerable<BinanceFuturesAccountSnapshot>>(BinanceAccountType.Futures, startTime, endTime, limit, receiveWindow, ct);

    private async Task<RestCallResult<T>> GetDailyAccountSnapshot<T>(BinanceAccountType accountType, 
        DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null,
        CancellationToken ct = default) where T : class
    {
        limit?.ValidateIntBetween(nameof(limit), 7, 30);

        var parameters = new ParameterCollection();
        parameters.AddEnum("type", accountType);
        parameters.AddOptional("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        var result = await RequestAsync<BinanceSnapshotWrapper<T>>(GetUrl(sapi, v1, "accountSnapshot"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 2400).ConfigureAwait(false);
        if (!result.Success) return result.As<T>(default!);
        if (result.Data.Code != 200) return result.AsError<T>(new ServerError(result.Data.Code, result.Data.Message!));

        return result.As(result.Data.SnapshotData);
    }

    /// <summary>
    /// This request will disable fastwithdraw switch under your account.
    /// You need to enable "trade" option for the api key which requests this endpoint.
    /// <para><a href="https://developers.binance.com/docs/wallet/account/disable-fast-withdraw-switch" /></para>
    /// </summary>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    public async Task<RestCallResult<bool>> DisableFastWithdrawSwitchAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        var result = await RequestAsync<object>(GetUrl(sapi, v1, "account/disableFastWithdrawSwitch"), HttpMethod.Post, ct, true, queryParameters: parameters, requestWeight: 1).ConfigureAwait(false);
        return result.As(result.Success);
    }

    /// <summary>
    /// This request will enable fastwithdraw switch under your account.
    /// You need to enable "trade" option for the api key which requests this endpoint.
    ///
    /// When Fast Withdraw Switch is on, transferring funds to a Binance account will be done instantly.
    /// There is no on-chain transaction, no transaction ID and no withdrawal fee.
    /// <para><a href="https://developers.binance.com/docs/wallet/account/enable-fast-withdraw-switch" /></para>
    /// </summary>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    public async Task<RestCallResult<bool>> EnableFastWithdrawSwitchAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        var result = await RequestAsync<object>(GetUrl(sapi, v1, "account/enableFastWithdrawSwitch"), HttpMethod.Post, ct, true, queryParameters: parameters, requestWeight: 1).ConfigureAwait(false);
        return result.As(result.Success);
    }

    /// <summary>
    /// Gets the status of the account associated with the api key/secret
    /// <para><a href="https://developers.binance.com/docs/wallet/account/account-status" /></para>
    /// </summary>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Account status</returns>
    public Task<RestCallResult<BinanceAccountStatus>> GetAccountStatusAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceAccountStatus>(GetUrl(sapi, v1, "account/status"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    /// <summary>
    /// Gets the trading status for the current account
    /// <para><a href="https://developers.binance.com/docs/wallet/account/account-api-trading-status" /></para>
    /// </summary>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The trading status of the account</returns>
    public async Task<RestCallResult<BinanceTradingStatus>> GetTradingStatusAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        var result = await RequestAsync<BinanceResult<BinanceTradingStatus>>(GetUrl(sapi, v1, "account/apiTradingStatus"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1).ConfigureAwait(false);
        if (!result) return result.As<BinanceTradingStatus>(default!);
        return !string.IsNullOrEmpty(result.Data.Message) ? result.AsError<BinanceTradingStatus>(new ServerError(result.Data.Message!)) : result.As(result.Data.Data);
    }

    /// <summary>
    /// Get permission info for the current API key
    /// <para><a href="https://developers.binance.com/docs/wallet/account/api-key-permission" /></para>
    /// </summary>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Permission info</returns>
    public Task<RestCallResult<BinanceAPIKeyPermissions>> GetAPIKeyPermissionsAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceAPIKeyPermissions>(GetUrl(sapi, v1, "account/apiRestrictions"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }
    #endregion

    #region Travel Rule (Local Entity) Methods
    // TODO: Withdraw (for local entities that require travel rule) (USER_DATA)
    // TODO: Withdraw History (for local entities that require travel rule) (supporting network) (USER_DATA)
    // TODO: Withdraw History V2 (for local entities that require travel rule) (supporting network) (USER_DATA)
    // TODO: Withdraw Questionnaire Contents (for existing local entities)
    // TODO: Submit Deposit Questionnaire (For local entities that require travel rule) (supporting network) (USER_DATA)
    // TODO: Deposit History (for local entities that required travel rule) (supporting network) (USER_DATA)
    // TODO: Deposit Questionnaire Contents (for existing local entities)
    // TODO: Onboarded VASP list (for local entities that require travel rule) (supporting network) (USER_DATA)
    // TODO: Broker Withdraw (for brokers of local entities that require travel rule) (USER_DATA)
    // TODO: Submit Deposit Questionnaire (For local entities that require travel rule) (supporting network) (USER_DATA)
    #endregion

    #region Other Methods
    /// <summary>
    /// Gets the status of the Binance platform
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#system-status-system" /></para>
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>The system status</returns>
    public Task<RestCallResult<BinanceSystemStatus>> GetSystemStatusAsync(CancellationToken ct = default)
    {
        return RequestAsync<BinanceSystemStatus>(GetUrl(sapi, v1, "system/status"), HttpMethod.Get, ct, false, requestWeight: 1);
    }

    // TODO: Get symbols delist schedule for spot (MARKET_DATA)

    #endregion
}