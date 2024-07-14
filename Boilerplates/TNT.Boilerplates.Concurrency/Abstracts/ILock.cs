using System;

namespace TNT.Boilerplates.Concurrency.Abstracts
{
    public interface ILock : IDisposable
    {
        string Key { get; }
    }
}