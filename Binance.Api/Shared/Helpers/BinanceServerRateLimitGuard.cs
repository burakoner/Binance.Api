namespace Binance.Api.Shared;

/// <summary>
/// Applies Binance server-directed backoff without retrying the request that received the limit response.
/// </summary>
internal sealed class BinanceServerRateLimitGuard
{
    private long retryAfterUtcTicks;

    internal DateTime? RetryAfter
    {
        get
        {
            var ticks = Interlocked.Read(ref retryAfterUtcTicks);
            return ticks == 0 ? null : new DateTime(ticks, DateTimeKind.Utc);
        }
    }

    internal bool IsActive => RetryAfter is DateTime retryAfter && retryAfter > DateTime.UtcNow;

    internal async Task<CallResult<bool>> WaitAsync(CancellationToken ct)
    {
        try
        {
            while (true)
            {
                var retryAfter = RetryAfter;
                if (!retryAfter.HasValue)
                    return new CallResult<bool>(true);

                var delay = retryAfter.Value - DateTime.UtcNow;
                if (delay <= TimeSpan.Zero)
                    return new CallResult<bool>(true);

                await Task.Delay(delay, ct).ConfigureAwait(false);
            }
        }
        catch (OperationCanceledException) when (ct.IsCancellationRequested)
        {
            return new CallResult<bool>(new CancellationRequestedError());
        }
    }

    internal void Extend(DateTime? retryAfter)
    {
        if (!retryAfter.HasValue)
            return;

        var utcRetryAfter = retryAfter.Value.Kind == DateTimeKind.Utc
            ? retryAfter.Value
            : retryAfter.Value.ToUniversalTime();
        if (utcRetryAfter <= DateTime.UtcNow)
            return;

        var newTicks = utcRetryAfter.Ticks;
        while (true)
        {
            var currentTicks = Interlocked.Read(ref retryAfterUtcTicks);
            if (currentTicks >= newTicks)
                return;
            if (Interlocked.CompareExchange(ref retryAfterUtcTicks, newTicks, currentTicks) == currentTicks)
                return;
        }
    }

    internal static DateTime? ParseRestRetryAfter(IEnumerable<KeyValuePair<string, IEnumerable<string>>> headers)
    {
        var value = headers
            .FirstOrDefault(header => string.Equals(header.Key, "Retry-After", StringComparison.OrdinalIgnoreCase))
            .Value?
            .FirstOrDefault();
        if (string.IsNullOrWhiteSpace(value))
            return null;

        if (long.TryParse(value, NumberStyles.None, CultureInfo.InvariantCulture, out var seconds) && seconds >= 0)
        {
            try
            {
                return DateTime.UtcNow.AddSeconds(seconds);
            }
            catch (ArgumentOutOfRangeException)
            {
                return null;
            }
        }

        if (DateTimeOffset.TryParse(
            value,
            CultureInfo.InvariantCulture,
            DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal,
            out var retryAfter))
        {
            return retryAfter.UtcDateTime;
        }

        return null;
    }

    internal static DateTime? ParseWebSocketRetryAfter(JToken response)
    {
        var token = response["error"]?["data"]?["retryAfter"];
        if (token == null
            || !long.TryParse(token.ToString(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var value))
            return null;

        try
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(value).UtcDateTime;
        }
        catch (ArgumentOutOfRangeException)
        {
            return null;
        }
    }

    internal static BinanceRateLimitError? ParseWebSocketRateLimitError(JToken response)
    {
        var status = response["status"]?.Value<int>();
        if (status is not (418 or 429))
            return null;

        var error = response["error"];
        var retryAfter = ParseWebSocketRetryAfter(response);
        return new BinanceRateLimitError(
            error?["code"]?.Value<int>() ?? status,
            error?["msg"]?.Value<string>() ?? "Binance rate limit exceeded",
            error?["data"])
        {
            RetryAfter = retryAfter
        };
    }
}
