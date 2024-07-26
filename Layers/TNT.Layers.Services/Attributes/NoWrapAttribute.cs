using System;

namespace TNT.Layers.Services.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Method)]
    public class NoWrapAttribute : Attribute
    {
    }
}
