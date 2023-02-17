using Binance.Api.Models.RestApi.Account;
using Binance.Api.Models.RestApi.PortfolioMargin;

namespace Binance.Api.Clients.RestApi.Margin;

public class BinanceRestApiMarginPortfolioClient
{
    // Api
    protected const string marginApi = "sapi";
    protected const string marginVersion = "1";

    // Portfolio Margin
    private const string portfolioMarginAccountEndpoint = "portfolio/account";
    private const string portfolioMarginCollateralRateEndpoint = "portfolio/collateralRate";
    private const string portfolioMarginLoanEndpoint = "portfolio/pmLoan";
    private const string portfolioMarginRepayEndpoint = "portfolio/repay";

    // Internal References
    internal BinanceRestApiMarginClient MainClient { get; }
    internal BinanceRestApiClientOptions ClientOptions { get => MainClient.RootClient.ClientOptions; }
    internal Uri GetUrl(string endpoint, string api, string version = null) => MainClient.GetUrl(endpoint, api, version);
    internal async Task<RestCallResult<T>> SendRequestInternal<T>(
        Uri uri, HttpMethod method, CancellationToken cancellationToken, bool signed = false,
        Dictionary<string, object> queryParameters = null, Dictionary<string, object> bodyParameters = null, Dictionary<string, string> headerParameters = null,
        ArraySerialization? serialization = null, JsonSerializer deserializer = null, bool ignoreRatelimit = false, int requestWeight = 1) where T : class
        => await MainClient.SendRequestInternal<T>(uri, method, cancellationToken, signed, queryParameters, bodyParameters, headerParameters, serialization, deserializer, ignoreRatelimit, requestWeight);

    internal BinanceRestApiMarginPortfolioClient(BinanceRestApiMarginClient main)
    {
        MainClient = main;
    }

    #region Get Portfolio Margin Account Info
    public async Task<RestCallResult<BinancePortfolioMarginInfo>> GetPortfolioMarginAccountInfoAsync(long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        return await SendRequestInternal<BinancePortfolioMarginInfo>(GetUrl(portfolioMarginAccountEndpoint, marginApi, marginVersion), HttpMethod.Get, ct, true, queryParameters: parameters).ConfigureAwait(false);
    }
    #endregion

    #region Portfolio Margin Collateral Rate
    public async Task<RestCallResult<IEnumerable<BinancePortfolioMarginCollateralRate>>> GetPortfolioMarginCollateralRateAsync(long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        return await SendRequestInternal<IEnumerable<BinancePortfolioMarginCollateralRate>>(GetUrl(portfolioMarginCollateralRateEndpoint, marginApi, marginVersion), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 50).ConfigureAwait(false);
    }
    #endregion

    #region Query Portfolio Margin Bankruptcy Loan Amount
    public async Task<RestCallResult<BinancePortfolioMarginLoan>> GetPortfolioMarginBankruptcyLoanAsync(long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        return await SendRequestInternal<BinancePortfolioMarginLoan>(GetUrl(portfolioMarginLoanEndpoint, marginApi, marginVersion), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 500).ConfigureAwait(false);
    }
    #endregion

    #region Portfolio Margin Bankruptcy Loan Repay
    public async Task<RestCallResult<BinanceTransaction>> PortfolioMarginBankruptcyLoanRepayAsync(long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? ClientOptions.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        return await SendRequestInternal<BinanceTransaction>(GetUrl(portfolioMarginRepayEndpoint, marginApi, marginVersion), HttpMethod.Post, ct, true, bodyParameters: parameters, requestWeight: 3000).ConfigureAwait(false);
    }
    #endregion
}