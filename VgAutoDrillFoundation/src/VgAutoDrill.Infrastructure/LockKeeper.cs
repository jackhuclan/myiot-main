using System.Collections.Concurrent;

namespace VgAutoDrill.Infrastructure;

/// <summary>
/// 锁保管员
/// </summary>
public class LockKeeper : ILockKeeper
{
    private ConcurrentDictionary<string, AutoResetEvent> _locks = new();
    private ConcurrentDictionary<string, bool> _lockedStatus = new();

    public bool TryGetLock(string lockName)
    {
        if (_locks.ContainsKey(lockName))
        {
            return true;
        }

        var _autoResetLock = new AutoResetEvent(true);
        return _locks.TryAdd(lockName, _autoResetLock);
    }

    public bool TryWaitLock(string lockName, TimeSpan? timeSpan = null)
    {
        if (!_locks.ContainsKey(lockName))
        {
            return false;
        }

        var @lock = _locks[lockName];
        var locked = timeSpan == null ? @lock.WaitOne() : @lock.WaitOne(timeSpan.Value);
        _lockedStatus.TryAdd(lockName, locked);
        return locked;
    }

    public bool TryReleaseLock(string lockName)
    {
        if (!_locks.ContainsKey(lockName)) return true;
        if (!_lockedStatus.ContainsKey(lockName)) return true;

        var @lock = _locks[lockName];
        _lockedStatus.TryRemove(lockName, out _);
        return @lock.Set();
    }

    public bool TryRemoveLock(string lockName)
    {
        if (!_locks.ContainsKey(lockName)) return true;

        var @lock = _locks[lockName];
        @lock.Dispose();
        _lockedStatus.TryRemove(lockName, out _);
        return _locks.TryRemove(lockName, out _);
    }
}
