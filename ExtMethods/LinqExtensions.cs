using System;
using System.Collections.Generic;
using System.Text;

namespace MobileDeliveryGeneral.ExtMethods
{
    public static class LinqExtensions
    {
        public static void ForEach<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, Action<TKey, TValue> invoke)
        {
            foreach (var kvp in dictionary)
                invoke(kvp.Key, kvp.Value);
        }

        const int _nSize=30;
        public static List<List<T>> SplitList<T>(this List<T> locations, int nSize = _nSize)
        {
            if (nSize == 0)
                nSize = _nSize;

            var list = new List<List<T>>();

            for (int i = 0; i < locations.Count; i += nSize)
            {
                list.Add(locations.GetRange(i, Math.Min(nSize, locations.Count - i)));
            }

            return list;
        }
    }
}
