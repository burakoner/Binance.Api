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
    internal BinanceRestApiClient RootClient { get; }
    internal BinanceRestApiMarginClient MarginClient { get; }
    internal BinanceRestApiClientOptions Options { get => RootClient.Options; }
    internal Uri GetUrl(string endpoint, string api, string version = null) => MarginClient.GetUrl(endpoint, api, version);
    internal async Task<RestCallResult<T>> SendRequestInternal<T>(
    Uri uri, HttpMethod method, CancellationToken cancellationToken, Dictionary<string, object> parameters = null, bool signed = false,
    HttpMethodParameterPosition? postPosition = null, ArraySerialization? arraySerialization = null, int weight = 1, bool ignoreRateLimit = false) where T : class
        => await MarginClient.SendRequestInternal<T>(uri, method, cancellationToken, parameters, signed, postPosition, arraySerialization, weight, ignoreRateLimit);

    internal BinanceRestApiMarginPortfolioClient(BinanceRestApiClient root, BinanceRestApiMarginClient margin)
    {
        RootClient = root;
        MarginClient = margin;
    }

    #region Get Portfolio Margin Account Info
    public async Task<RestCallResult<BinancePortfolioMarginInfo>> GetPortfolioMarginAccountInfoAsync(long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        return await SendRequestInternal<BinancePortfolioMarginInfo>(GetUrl(portfolioMarginAccountEndpoint, marginApi, marginVersion), HttpMethod.Get, ct, parameters, true, weight: 1).ConfigureAwait(false);
    }
    #endregion

    #region Portfolio Margin Collateral Rate
    public async Task<RestCallResult<IEnumerable<BinancePortfolioMarginCollateralRate>>> GetPortfolioMarginCollateralRateAsync(long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        return await SendRequestInternal<IEnumerable<BinancePortfolioMarginCollateralRate>>(GetUrl(portfolioMarginCollateralRateEndpoint, marginApi, marginVersion), HttpMethod.Get, ct, parameters, true, weight: 50).ConfigureAwait(false);
    }
    #endregion

    #region Query Portfolio Margin Bankruptcy Loan Amount
    public async Task<RestCallResult<BinancePortfolioMarginLoan>> GetPortfolioMarginBankruptcyLoanAsync(long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        return await SendRequestInternal<BinancePortfolioMarginLoan>(GetUrl(portfolioMarginLoanEndpoint, marginApi, marginVersion), HttpMethod.Get, ct, parameters, true, weight: 500).ConfigureAwait(false);
    }
    #endregion

    #region Portfolio Margin Bankruptcy Loan Repay
    public async Task<RestCallResult<BinanceTransaction>> PortfolioMarginBankruptcyLoanRepayAsync(long? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new Dictionary<string, object>();
        parameters.AddOptionalParameter("recvWindow", receiveWindow?.ToString(CultureInfo.InvariantCulture) ?? Options.ReceiveWindow.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        return await SendRequestInternal<BinanceTransaction>(GetUrl(portfolioMarginRepayEndpoint, marginApi, marginVersion), HttpMethod.Post, ct, parameters, true, weight: 3000).ConfigureAwait(false);
    }
    #endregion
}