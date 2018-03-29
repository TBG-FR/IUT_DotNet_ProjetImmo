using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjetImmo.Core.Tools
{
    public static class DictionaryExtensions
    {
        public static Dictionary<K, V> ResetValues<K, V>(this Dictionary<K, V> dic)
        {
            dic.Keys.ToList().ForEach(x => dic[x] = default(V));
            return dic;
        }

        public static Dictionary<K, V> ResetValuesWithNewDictionary<K, V>(this Dictionary<K, V> dic)
        {
            return dic.ToDictionary(x => x.Key, x => default(V), dic.Comparer);
        }

    }
}
