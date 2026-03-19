namespace PinoHeladeria.API.MIddleWares
{
    public class GeneralRateLimiterPolicies
    {
        public const string RateLimitPolicies = "RateLimitPolicies";
        public string? FixedPolicy { get; set; }
        public string? ConcurrentPolicy { get; set; }
        public string? SlidingWindowPolicy { get; set; }
        public string? TokenBucketPolicy { get; set; }
    }
}
