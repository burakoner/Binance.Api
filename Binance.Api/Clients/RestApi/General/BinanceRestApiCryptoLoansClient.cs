using Binance.Api.Models.RestApi.CryptoLoans;

namespace Binance.Api.Clients.RestApi.General;

public class BinanceRestApiCryptoLoansClient
{
    // Crypto Loans
    private const string incomingEndpoint = "loan/income";
    private const string borrowEndpoint = "loan/borrow";
    private const string borrowHistoryEndpoint = "loan/borrow/history";
    private const string openBorrowOrdersEndpoint = "loan/ongoing/orders";
    private const string repayEndpoint = "loan/repay";
    private const string repayHistoryEndpoint = "loan/repay/history";
    private const string adjustLtvEndpoint = "loan/adjust/ltv";
    private const string adjustLtvHistoryEndpoint = "loan/ltv/adjustment/history";
    // TODO: Get Loanable Assets Data
    // TODO: Get Collateral Assets Data
    // TODO: Check Collateral Repay Rate
    // TODO: Crypto Loan Customize Margin Call

    // Internal References
    internal BinanceRestApiGeneralClient MainClient { get; }
    internal BinanceRestApiClientOptions ClientOptions { get => MainClient.RootClient.RestOptions; }
    internal Uri GetUrl(string endpoint, string api, string version = null) => MainClient.GetUrl(endpoint, api, version);
    internal async Task<RestCallResult<T>> SendRequestInternal<T>(
        Uri uri, HttpMethod method, CancellationToken cancellationToken, bool signed = false,
        Dictionary<string, object> queryParameters = null, Dictionary<string, object> bodyParameters = null, Dictionary<string, string> headerParameters = null,
        ArraySerialization? serialization = null, JsonSerializer deserializer = null, bool ignoreRatelimit = false, int requestWeight = 1) where T : class
        => await MainClient.SendRequestInternal<T>(uri, method, cancellationToken, signed, queryParameters, bodyParameters, headerParameters, serialization, deserializer, ignoreRatelimit, requestWeight);

    internal BinanceRestApiCryptoLoansClient(BinanceRestApiGeneralClient main)
    {
        MainClient = main;
    }

    #region Get Crypto Loans Income History
    public async Task<RestCallResult<IEnumerable<BinanceCryptoLoanIncome>>> GetIncomeHistoryAsync(string asset, LoanIncomeType? type = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
            {
                { "asset", asset }
            };
        parameters.AddOptionalParameter("type", type.HasValue ? JsonConvert.SerializeObject(type.Value, new LoanIncomeTypeConverter(false)) : null);
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<IEnumerable<BinanceCryptoLoanIncome>>(GetUrl(incomingEndpoint, "sapi", "1"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 6000).ConfigureAwait(false);
    }
    #endregion

    #region Borrow - Crypto Loan Borrow
    public async Task<RestCallResult<BinanceCryptoLoanBorrow>> BorrowAsync(string loanAsset, string collateralAsset, int loanTerm, decimal? loanQuantity = null, decimal? collateralQuantity = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
            {
                { "loanCoin", loanAsset },
                { "collateralCoin", collateralAsset },
                { "loanTerm", loanTerm },
            };
        parameters.AddOptionalParameter("loanAmount", loanQuantity?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("collateralAmount", collateralQuantity?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceCryptoLoanBorrow>(GetUrl(borrowEndpoint, "sapi", "1"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 6000).ConfigureAwait(false);
    }
    #endregion

    #region Borrow - Get Loan Borrow History
    public async Task<RestCallResult<BinanceQueryRecords<BinanceCryptoLoanBorrowRecord>>> GetBorrowHistoryAsync(long? orderId = null, string loanAsset = null, string collateralAsset = null, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? limit = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("orderId", orderId);
        parameters.AddOptionalParameter("loanAsset", loanAsset);
        parameters.AddOptionalParameter("collateralAsset", collateralAsset);
        parameters.AddOptionalParameter("current", page?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceQueryRecords<BinanceCryptoLoanBorrowRecord>>(GetUrl(borrowHistoryEndpoint, "sapi", "1"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 400).ConfigureAwait(false);
    }
    #endregion

    #region Borrow - Get Loan Ongoing Orders
    public async Task<RestCallResult<BinanceQueryRecords<BinanceCryptoLoanOpenBorrowOrder>>> GetOpenBorrowOrdersAsync(long? orderId = null, string loanAsset = null, string collateralAsset = null, int? page = null, int? limit = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("orderId", orderId);
        parameters.AddOptionalParameter("loanAsset", loanAsset);
        parameters.AddOptionalParameter("collateralAsset", collateralAsset);
        parameters.AddOptionalParameter("current", page?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceQueryRecords<BinanceCryptoLoanOpenBorrowOrder>>(GetUrl(openBorrowOrdersEndpoint, "sapi", "1"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 400).ConfigureAwait(false);
    }
    #endregion

    #region Repay - Crypto Loan Repay
    public async Task<RestCallResult<BinanceCryptoLoanRepay>> RepayAsync(long orderId, decimal quantity, bool? repayWithBorrowedAsset = null, bool? collateralReturn = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
            {
                { "orderId", orderId },
                { "amount", quantity.ToString(CultureInfo.InvariantCulture) }
            };
        parameters.AddOptionalParameter("type", repayWithBorrowedAsset == null ? null : repayWithBorrowedAsset.Value ? "1" : "2");
        parameters.AddOptionalParameter("collateralReturn", collateralReturn);
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceCryptoLoanRepay>(GetUrl(repayEndpoint, "sapi", "1"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 6000).ConfigureAwait(false);
    }
    #endregion

    #region Repay - Get Loan Repayment History
    public async Task<RestCallResult<BinanceQueryRecords<BinanceCryptoLoanRepayRecord>>> GetRepayHistoryAsync(long? orderId = null, string loanAsset = null, string collateralAsset = null, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? limit = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("orderId", orderId);
        parameters.AddOptionalParameter("loanAsset", loanAsset);
        parameters.AddOptionalParameter("collateralAsset", collateralAsset);
        parameters.AddOptionalParameter("current", page?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceQueryRecords<BinanceCryptoLoanRepayRecord>>(GetUrl(repayHistoryEndpoint, "sapi", "1"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 400).ConfigureAwait(false);
    }
    #endregion

    #region Adjust LTV - Crypto Loan Adjust LTV
    public async Task<RestCallResult<BinanceCryptoLoanLtvAdjust>> AdjustLTVAsync(long orderId, decimal quantity, bool addOrRmove, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>
            {
                { "orderId", orderId },
                { "amount", quantity.ToString(CultureInfo.InvariantCulture) },
                { "direction", addOrRmove ? "ADDITIONAL" : "REDUCED" }
            };
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceCryptoLoanLtvAdjust>(GetUrl(adjustLtvEndpoint, "sapi", "1"), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 6000).ConfigureAwait(false);
    }
    #endregion

    #region Adjust LTV - Get Loan LTV Adjustment History
    public async Task<RestCallResult<BinanceQueryRecords<BinanceCryptoLoanLtvAdjustRecord>>> GetLtvAdjustHistoryAsync(long? orderId = null, string loanAsset = null, string collateralAsset = null, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? limit = null, long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("orderId", orderId);
        parameters.AddOptionalParameter("loanAsset", loanAsset);
        parameters.AddOptionalParameter("collateralAsset", collateralAsset);
        parameters.AddOptionalParameter("current", page?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("limit", limit?.ToString(CultureInfo.InvariantCulture));
        parameters.AddOptionalParameter("startTime", startTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("endTime", endTime.ConvertToMilliseconds());
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow?.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));

        return await SendRequestInternal<BinanceQueryRecords<BinanceCryptoLoanLtvAdjustRecord>>(GetUrl(adjustLtvHistoryEndpoint, "sapi", "1"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 400).ConfigureAwait(false);
    }
    #endregion
}