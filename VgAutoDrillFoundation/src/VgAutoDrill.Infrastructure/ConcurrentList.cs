using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Linq.Expressions;

namespace VgAutoDrill.Infrastructure;

public class ConcurrentList<TKey, TValue>
    where TKey : IEquatable<TKey>
    where TValue : class
{
    private readonly ConcurrentDictionary<TKey, TValue> _memoryCache = new();
    private readonly Func<TValue, TKey> _expression;
    private readonly object _lock = new object();

    public ConcurrentList(Expression<Func<TValue, TKey>> expression)
    {
        _expression = expression.Compile();
    }

    public bool Add(TValue value)
    {
        lock (_lock)
        {
            var key = _expression.Invoke(value);
            return _memoryCache.TryAdd(key, value);
        }
    }

    public void AddRange(IEnumerable<TValue> objects)
    {
        lock (_lock)
        {
            foreach (var obj in objects)
            {
                Add(obj);
            }
        }
    }

    public IReadOnlyList<TValue> GetAll()
    {
        lock (_lock)
        {
            return _memoryCache.Values.ToImmutableList();
        }
    }

    public void RemoveAll()
    {
        lock (_lock)
        {
            _memoryCache.Clear();
        }
    }

    public bool TryRemove(Predicate<TValue> predicate, out TValue? @object)
    {
        lock (_lock)
        {
            foreach (var obj in _memoryCache.Values)
            {
                if (predicate(obj))
                {
                    var key = _expression.Invoke(obj);
                    return _memoryCache.TryRemove(key, out @object);
                }
            }

            @object = default;
            return false;
        }
    }

    public bool ContainsKey(TKey key)
    {
        lock (_lock)
        {
            return _memoryCache.ContainsKey(key);
        }
    }

    public bool TryGetValue(TKey key, out TValue? @object)
    {
        lock (_lock)
        {
            return _memoryCache.TryGetValue(key, out @object);
        }
    }

    public TValue? FirstOrDefault(Func<TValue, bool> predicate)
    {
        lock (_lock)
        {
            return _memoryCache.Values.FirstOrDefault(predicate);
        }
    }

    public TValue? First(Func<TValue, bool> predicate)
    {
        lock (_lock)
        {
            return _memoryCache.Values.First(predicate);
        }
    }

    public IReadOnlyList<TValue> GetAll(Func<TValue, bool> predicate)
    {
        lock (_lock)
        {
            return _memoryCache.Values.Where(predicate).ToImmutableList();
        }
    }

    public TValue AddOrUpdate(TKey key, Func<TKey, TValue> addValueFactory, Func<TKey, TValue, TValue> updateValueFactory)
    {
        lock (_lock)
        {
            return _memoryCache.AddOrUpdate(key, addValueFactory, updateValueFactory);
        }
    }
}
