using System;

namespace TNT.Layers.Service.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Method)]
    public class NoWrapAttribute : Attribute
    {
    }
}
