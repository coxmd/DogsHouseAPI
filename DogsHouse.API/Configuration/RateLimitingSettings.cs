namespace DogsHouse.API.Configuration
{
    public class RateLimitingSettings
    {
        public int PermitLimit { get; set; }        // Number of requests allowed per window
        public int Window { get; set; }             // Time window in seconds
        public int QueueLimit { get; set; }         // Queue limit for requests that exceed the limit
    }
}
