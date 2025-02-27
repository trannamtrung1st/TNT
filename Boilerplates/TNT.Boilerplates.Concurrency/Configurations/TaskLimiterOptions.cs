namespace TNT.Boilerplates.Concurrency.Configurations
{
    // Reference: https://engineering.zalando.com/posts/2019/04/how-to-set-an-ideal-thread-pool-size.html
    public class TaskLimiterOptions : RateLimiterOptions
    {
        public new string Name { get; set; }
        public new int InitialLimit { get; set; }
        public double AvailableCores { get; set; }
        public double TargetCpuUtil { get; set; }
        public double WaitTime { get; set; }
        public double ServiceTime { get; set; }
    }
}
