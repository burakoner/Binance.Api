namespace Binance.Api.PortfolioMargin;

internal partial class BinancePortfolioMarginRestClientUsdFutures
{
    public Task<RestCallResult<BinancePortfolioMarginPositionRiskUM>> GetPositionRiskAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<RestCallResult<BinancePortfolioMarginInitialLeverageUM>> SetInitialLeverageAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<RestCallResult<bool>> SetPositionModeAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<RestCallResult<BinancePortfolioMarginPositionModeUM>> GetPositionModeAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<RestCallResult<List<BinancePortfolioMarginLeverageBracketUM>>> GetLeverageBracketsAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<RestCallResult<BinancePortfolioMarginTradingQuantitativeRulesIndicatorsUM>> GetTradingQuantitativeRulesIndicatorsAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<RestCallResult<BinancePortfolioMarginCommissionRateUM>> GetUserCommissionRateAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<RestCallResult<List<BinancePortfolioMarginIncomeUM>>> GetIncomeHistoryAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<RestCallResult<BinancePortfolioMarginAccountUM>> GetAccountAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<RestCallResult<BinancePortfolioMarginAccountConfigurationUM>> GetAccountConfigurationAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        // https://developers.binance.com/docs/derivatives/portfolio-margin/account/Get-UM-Futures-Account-Config
        throw new NotImplementedException();
    }

    public Task<RestCallResult<List<BinancePortfolioMarginSymbolConfigurationUM>>> GetSymbolConfigurationAsync(int? receiveWindow = null, CancellationToken ct = default)
    {
        // https://developers.binance.com/docs/derivatives/portfolio-margin/account/Get-UM-Futures-Symbol-Config
        throw new NotImplementedException();
    }

    public Task<RestCallResult<BinancePortfolioMarginAccountUMV2>> GetAccountV2Async(int? receiveWindow = null, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<RestCallResult<BinancePortfolioMarginDownloadId>> GetDownloadIdForTradeHistoryAsync(DateTime startTime, DateTime endTime, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddMilliseconds("startTime", startTime);
        parameters.AddMilliseconds("endTime", endTime);
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinancePortfolioMarginDownloadId>(GetUrl(papi, v1, "um/trade/asyn"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1500);
    }

    public Task<RestCallResult<BinancePortfolioMarginDownloadLink>> GetDownloadLinkForTradeHistoryAsync(string downloadId, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "downloadId", downloadId }
        };
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinancePortfolioMarginDownloadLink>(GetUrl(papi, v1, "um/trade/asyn/id"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10);
    }

    public Task<RestCallResult<BinancePortfolioMarginDownloadId>> GetDownloadIdForOrderHistoryAsync(DateTime startTime, DateTime endTime, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddMilliseconds("startTime", startTime);
        parameters.AddMilliseconds("endTime", endTime);
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinancePortfolioMarginDownloadId>(GetUrl(papi, v1, "um/order/asyn"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1500);
    }

    public Task<RestCallResult<BinancePortfolioMarginDownloadLink>> GetDownloadLinkForOrderHistoryAsync(string downloadId, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "downloadId", downloadId }
        };
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinancePortfolioMarginDownloadLink>(GetUrl(papi, v1, "um/order/asyn/id"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10);
    }

    public Task<RestCallResult<BinancePortfolioMarginDownloadId>> GetDownloadIdForIncomeHistoryAsync(DateTime startTime, DateTime endTime, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection();
        parameters.AddMilliseconds("startTime", startTime);
        parameters.AddMilliseconds("endTime", endTime);
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinancePortfolioMarginDownloadId>(GetUrl(papi, v1, "um/income/asyn"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1500);
    }

    public Task<RestCallResult<BinancePortfolioMarginDownloadLink>> GetDownloadLinkForIncomeHistoryAsync(string downloadId, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
        {
            { "downloadId", downloadId }
        };
        parameters.AddOptional("recvWindow", __.ReceiveWindow(receiveWindow));

        return RequestAsync<BinancePortfolioMarginDownloadLink>(GetUrl(papi, v1, "um/income/asyn/id"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 10);
    }
}







