using System.Collections.Generic;

namespace System.Collections
{
    public static class DictionaryExtensions
    {
        public static void AssignTo<TKey, TValue>(this IDictionary<TKey, TValue> source, IDictionary<TKey, TValue> target)
        {
            foreach (var kvp in source)
                target[kvp.Key] = kvp.Value;
        }
    }
}
