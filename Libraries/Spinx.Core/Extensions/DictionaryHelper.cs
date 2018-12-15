using System.Collections.Generic;

namespace Spinx.Core.Extensions
{
    public static class DictionaryHelper
    {
        public static void AddIfNotNull<T,TU>(this Dictionary<T,TU> dic, T key, TU value) where TU : class
        {
            if (value != null) { dic.Add(key, value); }
        }
    }
}