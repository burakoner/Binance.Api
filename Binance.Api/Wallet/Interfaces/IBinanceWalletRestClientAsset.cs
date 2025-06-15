namespace Binance.Api.Wallet;

/// <summary>
/// Interface for Binance Wallet Rest API Client Methods
/// </summary>
public interface IBinanceWalletRestClientAsset
{
    /// <summary>
    /// Gets the withdraw/deposit details for an asset
    /// <para><a href="https://developers.binance.com/docs/wallet/asset" /></para>
    /// </summary>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Asset detail</returns>
    Task<RestCallResult<Dictionary<string, BinanceWalletAssetDetails>>> GetAssetDetailsAsync(int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Receive balances of the different user wallets
    /// <para><a href="https://developers.binance.com/docs/wallet/asset/query-user-wallet-balance" /></para>
    /// </summary>
    /// <param name="quoteAsset">Quote asset, for example `USDT`, `ETH`, `USDC`, `BNB`, etc. default `BTC`</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceWalletBalance>>> GetWalletBalancesAsync(string quoteAsset = "BTC", int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Retrieve balance info
    /// <para><a href="https://developers.binance.com/docs/wallet/asset/user-assets" /></para>
    /// </summary>
    /// <param name="asset">Return for this asset, for example `ETH`</param>
    /// <param name="needBtcValuation">Whether the response should include the BtcValuation. If false (default) BtcValuation will be 0.</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceWalletUserBalance>>> GetBalancesAsync(string? asset = null, bool? needBtcValuation = null, int? receiveWindow = null, CancellationToken ct = default);

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
    Task<RestCallResult<BinanceWalletTransaction>> TransferAsync(BinanceWalletUniversalTransferType type, string asset, decimal quantity, string? fromSymbol = null, string? toSymbol = null, int? receiveWindow = null, CancellationToken ct = default);

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
    Task<RestCallResult<BinanceRowsResult<BinanceWalletTransfer>>> GetTransfersAsync(BinanceWalletUniversalTransferType type, DateTime? startTime = null, DateTime? endTime = null, int? page = null, int? pageSize = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Sets the status of the BNB burn switch for spot trading and margin interest
    /// <para><a href="https://developers.binance.com/docs/wallet/asset/Toggle-BNB-Burn-On-Spot-Trade-And-Margin-Interest" /></para>
    /// </summary>
    /// <param name="spotTrading">If BNB burning should be enabled for spot trading</param>
    /// <param name="marginInterest">If BNB burning should be enabled for margin interest</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceWalletBnbBurnStatus>> SetBnbBurnStatusAsync(bool? spotTrading = null, bool? marginInterest = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get assets that can be converted to BNB
    /// <para><a href="https://developers.binance.com/docs/wallet/asset/assets-can-convert-bnb" /></para>
    /// </summary>
    /// <param name="accountType">Spot or Margin account</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceEligibleDusts>> GetAssetsForDustTransferAsync(BinanceAccountType? accountType = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Converts dust (small amounts of) assets to BNB 
    /// <para><a href="https://developers.binance.com/docs/wallet/asset/dust-transfer" /></para>
    /// </summary>
    /// <param name="assets">The assets to convert to BNB, for example `ETH`</param>
    /// <param name="accountType">Spot or Margin account</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Dust transfer result</returns>
    Task<RestCallResult<BinanceWalletDustTransferResult>> DustTransferAsync(IEnumerable<string> assets, BinanceAccountType? accountType = null, int? receiveWindow = null, CancellationToken ct = default);

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
    Task<RestCallResult<BinanceDustLogList>> GetDustLogAsync(DateTime? startTime = null, DateTime? endTime = null, BinanceAccountType? accountType = null, int? receiveWindow = null, CancellationToken ct = default);

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
    Task<RestCallResult<BinanceRowsResult<BinanceDividendRecord>>> GetAssetDividendRecordsAsync(string? asset = null, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Gets the trade fee for a symbol
    /// <para><a href="https://developers.binance.com/docs/wallet/asset/trade-fee" /></para>
    /// </summary>
    /// <param name="symbol">Symbol to get withdrawal fee for, for example `ETHUSDT`</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Trade fees</returns>
    Task<RestCallResult<List<BinanceWalletTradeFee>>> GetTradeFeeAsync(string? symbol = null, int? receiveWindow = null, CancellationToken ct = default);

    /// <summary>
    /// Get funding wallet assets
    /// <para><a href="https://developers.binance.com/docs/wallet/asset/funding-wallet" /></para>
    /// </summary>
    /// <param name="asset">Filter by asset, for example `ETH`</param>
    /// <param name="needBtcValuation">Return BTC valuation</param>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>List of assets</returns>
    Task<RestCallResult<List<BinanceWalletFundingAsset>>> GetFundingWalletAsync(string? asset = null, bool? needBtcValuation = null, int? receiveWindow = null, CancellationToken ct = default);

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
    Task<RestCallResult<BinanceRowsResult<BinanceWalletCloudMiningHistory>>> GetCloudMiningHistoryAsync(DateTime startTime, DateTime endTime, long? transferId = null, string? clientTransferId = null, string? asset = null, int? page = null, int? pageSize = null, int? receiveWindow = null, CancellationToken ct = default);

    // TODO: Query User Delegation History(For Master Account)(USER_DATA)

    /// <summary>
    /// Get spot symbols delist schedule
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#get-symbols-delist-schedule-for-spot-market_data" /></para>
    /// </summary>
    /// <param name="receiveWindow">The receive window for which this request is active. When the request takes longer than this to complete the server will reject the request</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<List<BinanceWalletDelistSchedule>>> GetDelistScheduleAsync(int? receiveWindow = null, CancellationToken ct = default);

    // TODO: Get Open Symbol List (MARKET_DATA)
}