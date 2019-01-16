using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class DictionaryExtension
{
    public static TValue TryGetV<Tkey, TValue>(this Dictionary<Tkey, TValue> dict,Tkey key)
    {
        TValue value;
        dict.TryGetValue(key, out value);
        return value;
    }
}
