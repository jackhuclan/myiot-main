// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IoTClient.Clients.PLC;

namespace VgDeviceGateway.Devices.Common
{
    public static class MitsubishiClientExtensions
    {


        /// <summary>
        /// 三菱MC写入字符串
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static bool WriteStringExtensions(this MitsubishiClient mitsubishiClient, string address, string value, int maxLength = 100)
        {
            // 转换字符串为字节数组（根据PLC编码调整）
            byte[] stringBytes = Encoding.ASCII.GetBytes(value);

            // 检查长度
            if (stringBytes.Length > maxLength)
            {
                Console.WriteLine($"错误: 字符串长度({stringBytes.Length})超过最大限制({maxLength})");
                return false;
            }

            // 创建带结束符的完整字节数组
            byte[] dataToWrite = new byte[maxLength];
            Array.Copy(stringBytes, dataToWrite, stringBytes.Length);

            // 如果字符串不足最大长度，添加空字符结束符
            if (stringBytes.Length < maxLength)
                dataToWrite[stringBytes.Length] = 0;

            // 写入数据到PLC
            var writeResult = mitsubishiClient.Write(address, dataToWrite);
            return writeResult.IsSucceed;
        }

        /// <summary>
        ///  三菱MC读取字符串 
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static string ReadStringExtensions(this MitsubishiClient mitsubishiClient, string address, ushort length = 100)
        {
            // 从PLC读取字节数据
            var readResult = mitsubishiClient.Read(address, length);

            if (!readResult.IsSucceed || readResult.Value == null)
                return "";

            byte[] bytes = readResult.Value;

            // 查找字符串结束符
            int nullIndex = Array.IndexOf(bytes, (byte)0);
            byte[] validBytes;

            if (nullIndex >= 0)
                validBytes = new byte[nullIndex];
            else
                validBytes = bytes;

            // 复制有效字节
            if (nullIndex >= 0)
                Array.Copy(bytes, validBytes, nullIndex);

            // 转换为字符串
            try
            {
                string result = Encoding.ASCII.GetString(validBytes);
                return result.Trim();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"读取字符串出错：{ex.Message}");
                return "";
            }
        }
    }
}
