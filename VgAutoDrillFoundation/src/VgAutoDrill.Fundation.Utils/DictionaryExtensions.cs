namespace VgAutoDrill.Fundation.Utils;

public static class DictionaryExtensions
{
    public static Dictionary<TKey, TValue> AddMore<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key, TValue value)
        where TKey : notnull
    {
        dic.Add(key, value);
        return dic;
    }
}
