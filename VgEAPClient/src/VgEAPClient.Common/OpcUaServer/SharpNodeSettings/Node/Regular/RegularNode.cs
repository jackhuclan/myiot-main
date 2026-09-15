// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using SharpNodeSettings.Node.NodeBase;

namespace SharpNodeSettings.Node.Regular
{
    /// <summary>
    /// 字节解析规则节点，该节点下挂载解析节点的子项
    /// </summary>
    public class RegularNode : NodeClass
    {
        /// <summary>
        /// 实例化一个默认的解析对象
        /// </summary>
        public RegularNode()
        {
            this.NodeType = NodeClassInfo.RegularNode;
            this.NodeHead = "RegularNode";
        }

    }
}
