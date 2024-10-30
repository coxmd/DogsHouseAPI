namespace DogsHouse.API.Middleware
{
    /// <summary>
    /// Middleware that implements rate limiting using the Token Bucket algorithm.
    /// This algorithm provides smooth rate limiting by using tokens that are replenished over time.
    /// </summary>
    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        // Thread-safe dictionary to store token buckets for each client
        private static readonly Dictionary<string, TokenBucket> _buckets = new();
        private static readonly object _lock = new();

        public RateLimitingMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Get client identifier (IP address in this case)
            var clientIp = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";

            // Get rate limit from configuration (requests per second)
            var requestsPerSecond = _configuration.GetValue<int>("RateLimiting:RequestsPerSecond");
            // Thread-safe bucket initialization
            lock (_lock)
            {
                if (!_buckets.ContainsKey(clientIp))
                {
                    _buckets[clientIp] = new TokenBucket(
                        capacity: requestsPerSecond,    // Maximum number of tokens
                        refillRate: requestsPerSecond   // Tokens added per second
                    );
                }
            }

            // Try to consume a token
            if (_buckets[clientIp].TryConsume())
            {
                // If successful, process the request
                await _next(context);
            }
            else
            {
                // If no tokens available, return 429 Too Many Requests
                context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                await context.Response.WriteAsync("Too many requests. Please try again later.");
            }
        }
    }

    /// <summary>
    /// Implements the Token Bucket algorithm for rate limiting.
    /// The bucket has a fixed capacity and tokens are replenished at a fixed rate.
    /// Each request consumes one token.
    /// </summary>
    public class TokenBucket
    {
        private readonly int _capacity;        // Maximum number of tokens the bucket can hold
        private readonly int _refillRate;      // Number of tokens added per second
        private double _tokens;                // Current number of tokens (can be fractional)
        private DateTime _lastRefill;          // Last time tokens were added
        private readonly object _syncLock = new();  // For thread safety

        /// <summary>
        /// Initializes a new token bucket.
        /// </summary>
        /// <param name="capacity">Maximum number of tokens the bucket can hold</param>
        /// <param name="refillRate">Number of tokens to add per second</param>
        public TokenBucket(int capacity, int refillRate)
        {
            _capacity = capacity;
            _refillRate = refillRate;
            _tokens = capacity;          // Start with a full bucket
            _lastRefill = DateTime.UtcNow;
        }

        /// <summary>
        /// Attempts to consume a token from the bucket.
        /// </summary>
        /// <returns>True if a token was consumed, false if no tokens were available</returns>
        public bool TryConsume()
        {
            lock (_syncLock)
            {
                RefillTokens();

                if (_tokens >= 1)
                {
                    _tokens--;
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// Refills tokens based on elapsed time since last refill.
        /// </summary>
        private void RefillTokens()
        {
            var now = DateTime.UtcNow;
            var secondsElapsed = (now - _lastRefill).TotalSeconds;

            // Calculate tokens to add based on elapsed time
            var tokensToAdd = secondsElapsed * _refillRate;

            // Update token count, ensuring it doesn't exceed capacity
            _tokens = Math.Min(_capacity, _tokens + tokensToAdd);

            // Update last refill time
            _lastRefill = now;
        }
    }
}
