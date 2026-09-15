// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Concurrent;

namespace VgDeviceGateway.Devices.Drill.Other;
public sealed class FixedQueue<T>
{
    private readonly ConcurrentQueue<T> _queue = new();
    private readonly int _limit;
    private readonly object _syncRoot = new();   // 保护“读 Count + Dequeue”复合操作

    public FixedQueue(int limit)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(limit, 1);
        _limit = limit;
    }

    public void Enqueue(T item)
    {
        lock (_syncRoot)
        {
            // 先加进去
            _queue.Enqueue(item);

            // 如果超限就弹出最旧的
            while (_queue.Count > _limit && _queue.TryDequeue(out _)) { }
        }
    }

    public int Count => _queue.Count;            // 并发读 Count 是原子操作，无需锁
    public IEnumerable<T> Items => _queue;       // 快照遍历，线程安全
    public ConcurrentQueue<T> Queue => _queue;       // 快照遍历，线程安全
}
