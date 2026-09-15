namespace VgAutoDrill.Infrastructure;

/// <summary>
/// 对象包装器
/// </summary>
/// <typeparam name="T"></typeparam>
public class ObjectWrapper<T>
{
    private readonly T _obj;

    private ObjectWrapper(T obj)
    {
        _obj = obj;
    }

    public static ObjectWrapper<T> Wrap(T obj)
    {
        return new ObjectWrapper<T>(obj);
    }

    public T GetObject()
    {
        return _obj;
    }
}
