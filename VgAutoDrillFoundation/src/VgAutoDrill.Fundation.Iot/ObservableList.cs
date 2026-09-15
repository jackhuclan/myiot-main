namespace VgAutoDrill.Fundation.Iot;

public class ObservableList<T, TResult> : List<T>
{
    public event Func<string, Task<TResult>>? CollectionChanged;

    private AutoResetEvent _autoResetEvent = new AutoResetEvent(true);

    /// <summary>
    /// 以线程安全方式来执行改变集合行为的task，并且在task执行完后自动触发集合的RaiseCollectionChangedEvent方法，
    /// 所以在task里面不需要再调用RaiseCollectionChangedEvent
    /// </summary>
    /// <param name="locationCode">库位编号</param>
    /// <param name="changingListTask">改变集合的task</param>
    /// <returns></returns>
    public async Task<TResult> ChangeListSafely(string locationCode, Task changingListTask)
    {
        try
        {
            _autoResetEvent.WaitOne();
            await changingListTask;
            return await RaiseCollectionChangedEvent(locationCode);
        }
        finally
        {
            _autoResetEvent.Set();
        }
    }

    /// <summary>
    /// 触发集合变化事件
    /// </summary>
    /// <param name="locationCode">库位编号</param>
    public Task<TResult> RaiseCollectionChangedEvent(string locationCode)
    {
        ArgumentNullException.ThrowIfNull(CollectionChanged, nameof(CollectionChanged));
        return CollectionChanged!.Invoke(locationCode);
    }
}
