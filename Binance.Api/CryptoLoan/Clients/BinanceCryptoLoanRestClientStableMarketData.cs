namespace Binance.Api.CryptoLoan;

internal partial class BinanceCryptoLoanRestClientStable
{
    public Task<RestCallResult<BinanceCryptoLoanIncome[]>> GetIncomeHistoryAsync(string asset, BinanceCryptoLoanIncomeType? type = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        var parameters = new ParameterCollection
            {
                { "asset", asset }
            };
        parameters.AddOptionalEnum("type", type);
        parameters.AddOptional("limit", limit);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("recvWindow", _._.ReceiveWindow(receiveWindow));

        return _.RequestAsync<BinanceCryptoLoanIncome[]>(_.GetUrl(sapi, v1, "loan/income"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 6000);
    }
}