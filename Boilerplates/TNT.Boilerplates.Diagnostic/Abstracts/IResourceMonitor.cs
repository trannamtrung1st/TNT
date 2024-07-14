using System;

namespace TNT.Boilerplates.Diagnostic.Abstracts
{
    public interface IResourceMonitor
    {
        double TotalCores { get; }
        event EventHandler<(double Cpu, double Memory)> Collected;

        void Start(double interval = 5000);
        void Stop();
        double GetCpuUsage();
        double GetMemoryUsage();
    }
}