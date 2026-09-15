// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace SharpNodeSettings.Node.Server
{
    public class NodeModbusServer : ServerNode
    {

        /// <summary>
        /// 实例化一个Modbus服务器的节点对象
        /// </summary>
        public NodeModbusServer()
        {
            Name = "Modbus 服务器";
            Description = "这是一个Modbus服务器";
            ServerType = ServerNode.ModbusServer;
        }



    }
}
