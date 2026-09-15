namespace VgAutoDrill.Fundation.Iot;

public enum ActionTiggerMode
{
    /// <summary>
    /// 所有action以同步方式运行
    /// </summary>
    Synchronized = 0,

    /// <summary>
    /// 所有action以异步task方式运行
    /// </summary>
    Asynchronized = 1,
}
