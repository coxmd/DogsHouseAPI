namespace DogsHouse.API.Middleware
{
    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        private static readonly Dictionary<string, TokenBucket> _buckets = new();

        public RateLimitingMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var clientIp = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var requestsPerSecond = _configuration.GetValue<int>("RateLimiting:RequestsPerSecond");

            if (!_buckets.ContainsKey(clientIp))
            {
                _buckets[clientIp] = new TokenBucket(requestsPerSecond, requestsPerSecond);
            }

            if (_buckets[clientIp].TryConsume())
            {
                await _next(context);
            }
            else
            {
                context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                await context.Response.WriteAsync("Too many requests");
            }
        }
    }

    public class TokenBucket
    {
        private readonly int _capacity;
        private readonly int _refillRate;
        private double _tokens;
        private DateTime _lastRefill;

        public TokenBucket(int capacity, int refillRate)
        {
            _capacity = capacity;
            _refillRate = refillRate;
            _tokens = capacity;
            _lastRefill = DateTime.UtcNow;
        }

        public bool TryConsume()
        {
            RefillTokens();
            if (_tokens >= 1)
            {
                _tokens--;
                return true;
            }
            return false;
        }

        private void RefillTokens()
        {
            var now = DateTime.UtcNow;
            var secondsElapsed = (now - _lastRefill).TotalSeconds;
            var tokensToAdd = secondsElapsed * _refillRate;
            _tokens = Math.Min(_capacity, _tokens + tokensToAdd);
            _lastRefill = now;
        }
    }
}
