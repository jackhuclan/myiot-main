// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient.Common.CNC.Write9XNode;

public class Write9XNodeData
{
    public Cnc9XWriteNodeData Cnc9XWriteNodeData = new Cnc9XWriteNodeData();

    public async Task Write9XNodeValueAsync(string strNode, string strValueType, string strValue)
    {
        await Cnc9XWriteNodeData.LockWriteNodeValue.WaitAsync();
        try
        {
            Cnc9XWriteNodeData.Node = strNode;
            Cnc9XWriteNodeData.ValueType = strValueType;
            Cnc9XWriteNodeData.Value = strValue;
            Cnc9XWriteNodeData.IsWriteNodeValue = true;
        }
        finally
        {
            Cnc9XWriteNodeData.LockWriteNodeValue.Release();
        }
    }
}

public class Cnc9XWriteNodeData
{
    public readonly SemaphoreSlim LockWriteNodeValue = new SemaphoreSlim(1, 1);
    public bool IsWriteNodeValue { get; set; } = false;
    public string Node { get; set; } = string.Empty;
    public string ValueType { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}
