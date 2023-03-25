namespace BtcTurk.Api;

public class BtcTurkClientOptions : RestApiClientOptions
{
    // Additional Api Addresses
    public string GraphApiAddress { get; set; }

    // Auto Timestamp
    public bool AutoTimestamp { get; set; }
    public TimeSpan TimestampRecalculationInterval { get; set; }

    public BtcTurkClientOptions() : base()
    {
        // Base Addresses
        this.BaseAddress = BtcTurkAddresses.Default.ApiAddress;
        this.GraphApiAddress = BtcTurkAddresses.Default.GraphApiAddress;

        // Rate Limiters
        RateLimiters = new List<IRateLimiter>
        {
            new RateLimiter()
            .AddTotalRateLimit(600, TimeSpan.FromSeconds(600))
            .AddPartialEndpointLimit("/api/v1/users/balances", 120, TimeSpan.FromSeconds(600), HttpMethod.Get, true, false)

            .AddPartialEndpointLimit("/api/v1/order", 300, TimeSpan.FromSeconds(600), HttpMethod.Post, true, false)
            .AddPartialEndpointLimit("/api/v1/order", 300, TimeSpan.FromSeconds(600), HttpMethod.Delete, true, false)
            .AddPartialEndpointLimit("/api/v1/openOrders", 330, TimeSpan.FromSeconds(600), HttpMethod.Get, true, false)
            .AddPartialEndpointLimit("/api/v1/allOrders", 330, TimeSpan.FromSeconds(600), HttpMethod.Get, true, false)

            .AddPartialEndpointLimit("/api/v1/trades", 90, TimeSpan.FromSeconds(600), HttpMethod.Get, true, false)

            .AddPartialEndpointLimit("/api/v1/users/transactions/trade", 90, TimeSpan.FromSeconds(600), HttpMethod.Get, true, false)
            .AddPartialEndpointLimit("/api/v1/users/transactions/trade/*", 90, TimeSpan.FromSeconds(600), HttpMethod.Get, true, false)
            .AddPartialEndpointLimit("/api/v1/users/transactions/crypto", 90, TimeSpan.FromSeconds(600), HttpMethod.Get, true, false)
            .AddPartialEndpointLimit("/api/v1/users/transactions/fiat", 90, TimeSpan.FromSeconds(600), HttpMethod.Get, true, false)

            .AddPartialEndpointLimit("/api/v2/ticker", 600, TimeSpan.FromSeconds(600), HttpMethod.Get, true, false)
            .AddPartialEndpointLimit("/api/v2/orderBook", 60, TimeSpan.FromSeconds(600), HttpMethod.Get, true, false)
        };

        // Auto Timestamp
        AutoTimestamp = true;
        TimestampRecalculationInterval = TimeSpan.FromHours(1);
    }
}