namespace VgAutoDrill.Central.Core.Schedule.Summary;

internal static class ItemSummaryList
{
    public static IEnumerable<TSource> Concat<TSource, TKey>(this IEnumerable<TSource> first,
        IEnumerable<TSource> second,
        Func<TSource, TKey> keySelector,
        Func<IGrouping<TKey, TSource>, TSource> selector)
    {
        if (first == null)
        {
            ArgumentNullException.ThrowIfNull(first);
        }
        if (second == null)
        {
            ArgumentNullException.ThrowIfNull(second);
        }

        return first.Concat(second).GroupBy(keySelector).Select(selector);
    }
}
