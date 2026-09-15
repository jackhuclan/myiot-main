
namespace VgAutoDrill.Infrastructure;

/// <summary>
/// 锁保管员
/// </summary>
public interface ILockKeeper
{
    /// <summary>
    /// 尝试获取一把锁
    /// </summary>
    /// <param name="lockName">锁的名称</param>
    /// <returns></returns>
    bool TryGetLock(string lockName);
    /// <summary>
    /// 尝试释放一把锁
    /// </summary>
    /// <param name="lockName">锁的名称</param>
    /// <returns></returns>
    bool TryReleaseLock(string lockName);
    /// <summary>
    /// 尝试等待一把锁
    /// </summary>
    /// <param name="lockName">锁的名称</param>
    /// <param name="timeSpan">等待锁的时间</param>
    /// <returns></returns>
    bool TryWaitLock(string lockName, TimeSpan? timeSpan = null);
    /// <summary>
    /// 尝试移除一把锁
    /// </summary>
    /// <param name="lockName">锁的名称</param>
    /// <returns></returns>
    bool TryRemoveLock(string lockName);
}
