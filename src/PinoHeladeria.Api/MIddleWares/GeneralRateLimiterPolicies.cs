namespace PinoHeladeria.API.MIddleWares
{
    public class GeneralRateLimiterPolicies
    {
        public const string RateLimiterPolicy = "RateLimiterPolicies";
        public string? FixedPolicy { get; set; }
        public string? ConcurrentPolicy { get; set; }
        public string? SlidingPolicy { get; set; }
        public string? TokenPolicy { get; set; }
    }
}
