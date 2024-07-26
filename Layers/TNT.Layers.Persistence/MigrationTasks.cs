using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TNT.Layers.Persistence
{
    public static class MigrationTasks<TDbContext>
    {
        // [IMPORTANT] It is acceptable to comment obsolete migration logics (e.g, Changes in properties)
        public static readonly List<Func<TDbContext, IServiceProvider, Task>> Tasks = new List<Func<TDbContext, IServiceProvider, Task>>();
    }
}
