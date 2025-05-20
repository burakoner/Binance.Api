namespace Binance.Api.Mining;

/// <summary>
/// Interface for the Binance Mining Rest API client.
/// </summary>
public interface IBinanceMiningRestClient
{
    /// <summary>
    /// Gets mining algorithms info
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#acquiring-algorithm-market_data" /></para>
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Algorithms info</returns>
    Task<RestCallResult<List<BinanceMiningAlgorithm>>> GetAlgorithmsAsync(CancellationToken ct = default);

    /// <summary>
    /// Gets mining coins info
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#acquiring-coinname-market_data" /></para>
    /// </summary>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Coins info</returns>
    Task<RestCallResult<List<BinanceMiningCoin>>> GetCoinsAsync(CancellationToken ct = default);

    /// <summary>
    /// Gets hash rate resale list
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#hashrate-resale-list-user_data" /></para>
    /// </summary>
    /// <param name="page">Page</param>
    /// <param name="pageSize">Results per page</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Resale list</returns>
    Task<RestCallResult<BinanceMiningHashrateResaleList>> GetHashrateResalesAsync(int? page = null, int? pageSize = null, CancellationToken ct = default);

    /// <summary>
    /// Gets miner list
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#request-for-miner-list-user_data" /></para>
    /// </summary>
    /// <param name="algorithm">Algorithm</param>
    /// <param name="userName">Mining account</param>
    /// <param name="page">Result page</param>
    /// <param name="sortAscending">Sort in ascending order</param>
    /// <param name="sortColumn">Column to sort by</param>
    /// <param name="workerStatus">Filter by status</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Miner list</returns>
    Task<RestCallResult<BinanceMiningWorkers>> GetWorkersAsync(string algorithm, string userName, int? page = null, bool? sortAscending = null, string? sortColumn = null, BinanceMiningMinerStatus? workerStatus = null, CancellationToken ct = default);

    /// <summary>
    /// Gets miner details
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#request-for-detail-miner-list-user_data" /></para>
    /// </summary>
    /// <param name="algorithm">Algorithm</param>
    /// <param name="userName">Mining account</param>
    /// <param name="workerName">Miners name</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Miner details</returns>
    Task<RestCallResult<List<BinanceMiningWorkerDetails>>> GetWorkerDetailsAsync(string algorithm, string userName, string workerName, CancellationToken ct = default);

    /// <summary>
    /// Get other revenue list
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#extra-bonus-list-user_data" /></para>
    /// </summary>
    /// <param name="algorithm">Algorithm</param>
    /// <param name="userName">Mining account</param>
    /// <param name="page">Result page</param>
    /// <param name="pageSize">Results per page</param>
    /// <param name="coin">Coin</param>
    /// <param name="startDate">Start date</param>
    /// <param name="endDate">End date</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Revenue list</returns>
    Task<RestCallResult<BinanceMiningOtherRevenues>> GetOtherRevenuesAsync(string algorithm, string userName, string? coin = null, DateTime? startDate = null, DateTime? endDate = null, int? page = null, int? pageSize = null, CancellationToken ct = default);

    /// <summary>
    /// Gets revenue list
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#earnings-list-user_data" /></para>
    /// </summary>
    /// <param name="algorithm">Algorithm</param>
    /// <param name="userName">Mining account</param>
    /// <param name="page">Result page</param>
    /// <param name="pageSize">Results per page</param>
    /// <param name="coin">Coin</param>
    /// <param name="startDate">Start date</param>
    /// <param name="endDate">End date</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Revenue list</returns>
    Task<RestCallResult<BinanceMiningRevenues>> GetMiningRevenuesAsync(string algorithm, string userName, string? coin = null, DateTime? startDate = null, DateTime? endDate = null, int? page = null, int? pageSize = null, CancellationToken ct = default);

    /// <summary>
    /// Cancel Hashrate Resale Configuration
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#cancel-hashrate-resale-configuration-user_data" /></para>
    /// </summary>
    /// <param name="configId">Mining id</param>
    /// <param name="userName">Mining account</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Success</returns>
    Task<RestCallResult<bool>> CancelHashrateResaleRequestAsync(int configId, string userName, CancellationToken ct = default);

    /// <summary>
    /// Gets hash rate resale details
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#hashrate-resale-detail-user_data" /></para>
    /// </summary>
    /// <param name="configId">The mining id</param>
    /// <param name="userName">Mining account</param>
    /// <param name="page">Page</param>
    /// <param name="pageSize">Results per page</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Resale details</returns>
    Task<RestCallResult<BinanceMiningHashrateResaleDetails>> GetHashrateResaleDetailsAsync(int configId, string userName, int? page = null, int? pageSize = null, CancellationToken ct = default);

    /// <summary>
    /// Get mining account earnings
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#mining-account-earning-user_data" /></para>
    /// </summary>
    /// <param name="algo">Algorithm</param>
    /// <param name="startDate">Filter by start time</param>
    /// <param name="endDate">Filter by end time</param>
    /// <param name="page">Page</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns></returns>
    Task<RestCallResult<BinanceMiningEarnings>> GetEarningsAsync(string algo, DateTime? startDate = null, DateTime? endDate = null, int? page = null, int? pageSize = null, CancellationToken ct = default);

    /// <summary>
    /// Get mining statistics
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#statistic-list-user_data" /></para>
    /// </summary>
    /// <param name="algorithm">Algorithm</param>
    /// <param name="userName">User name</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Mining statistics</returns>
    Task<RestCallResult<BinanceMiningStatistic>> GetStatisticsAsync(string algorithm, string userName, CancellationToken ct = default);

    /// <summary>
    /// Hashrate resale request
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#hashrate-resale-request-user_data" /></para>
    /// </summary>
    /// <param name="userName">Mining account</param>
    /// <param name="algorithm">Transfer algorithm</param>
    /// <param name="startDate">Resale start time</param>
    /// <param name="endDate">Resale end time</param>
    /// <param name="toUser">To mining account</param>
    /// <param name="hashRate">Resale hashrate h/s must be transferred (BTC is greater than 500000000000 ETH is greater than 500000)</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Mining account</returns>
    Task<RestCallResult<int>> PlaceHashrateResaleRequestAsync(string algorithm, string userName, DateTime startDate, DateTime endDate, string toUser, decimal hashRate, CancellationToken ct = default);

    /// <summary>
    /// Gets mining account list
    /// <para><a href="https://binance-docs.github.io/apidocs/spot/en/#account-list-user_data" /></para>
    /// </summary>
    /// <param name="algorithm">Algorithm</param>
    /// <param name="userName">Mining account user name</param>
    /// <param name="ct">Cancellation token</param>
    /// <returns>Revenue list</returns>
    Task<RestCallResult<List<BinanceMiningAccount>>> GetAccountsAsync(string algorithm, string userName, CancellationToken ct = default);
}