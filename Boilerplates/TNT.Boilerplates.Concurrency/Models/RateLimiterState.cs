namespace TNT.Boilerplates.Concurrency.Models
{
    public readonly struct RateLimiterState
    {
        public RateLimiterState(int limit, int acquired, int available) : this()
        {
            Limit = limit;
            Acquired = acquired;
            Available = available;
        }

        public int Limit { get; }
        public int Acquired { get; }
        public int Available { get; }
    }
}
