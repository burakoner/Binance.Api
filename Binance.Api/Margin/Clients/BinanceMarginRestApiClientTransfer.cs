using Binance.Api.Margin.Responses;

namespace Binance.Api.Margin;

internal partial class BinanceMarginRestApiClient
{
    public Task<RestCallResult<BinanceQueryRecords<BinanceTransferHistory>>> GetMarginTransferHistoryAsync(TransferDirection direction, int? page = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, string? isolatedSymbol = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        limit?.ValidateIntBetween(nameof(limit), 1, 100);

        var parameters = new ParameterCollection();
        parameters.AddEnum("direction", direction);
        parameters.AddOptional("isolatedSymbol", isolatedSymbol);
        parameters.AddOptional("size", limit);
        parameters.AddOptional("current", page);
        parameters.AddOptionalMilliseconds("startTime", startTime);
        parameters.AddOptionalMilliseconds("endTime", endTime);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        return RequestAsync<BinanceQueryRecords<BinanceTransferHistory>>(GetUrl(sapi, v1, "margin/transfer"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 1);
    }

    public async Task<RestCallResult<decimal>> GetMarginMaxTransferAmountAsync(string asset, string? isolatedSymbol = null, int? receiveWindow = null, CancellationToken ct = default)
    {
        asset.ValidateNotNull(nameof(asset));
        var parameters = new ParameterCollection
        {
            { "asset", asset }
        };
        parameters.AddOptional("isolatedSymbol", isolatedSymbol);
        parameters.AddOptional("recvWindow", _.ReceiveWindow(receiveWindow));

        var result = await RequestAsync<BinanceMarginAmount>(GetUrl(sapi, v1, "margin/maxTransferable"), HttpMethod.Get, ct, true, queryParameters: parameters, requestWeight: 50);
        if (!result) return result.As<decimal>(default);

        return result.As(result.Data.Quantity);
    }

}