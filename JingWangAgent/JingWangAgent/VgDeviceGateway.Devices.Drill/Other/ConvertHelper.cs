// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text;

namespace VgDeviceGateway.Devices.Drill.Other
{
    public enum DataFormat
    {
        /// <summary>
        /// 按照顺序排序
        /// </summary>
        ABCD = 0,

        /// <summary>
        /// 按照单字反转
        /// </summary>
        BADC = 1,

        /// <summary>
        /// 按照双字反转
        /// </summary>
        CDAB = 2,

        /// <summary>
        /// 按照倒序排序
        /// </summary>
        DCBA = 3,
    }

    internal class ConvertHelper
    {
        public static byte[] Get2ByteArray(byte[] source, int start, DataFormat type = DataFormat.ABCD)
        {
            byte[] Res = new byte[2];

            byte[] ResTemp = GetByteArray(source, start, 2);

            if (ResTemp == null) return null;

            switch (type)
            {
                case DataFormat.ABCD:
                case DataFormat.CDAB:
                    Res[0] = ResTemp[1];
                    Res[1] = ResTemp[0];
                    break;

                case DataFormat.BADC:
                case DataFormat.DCBA:
                    Res = ResTemp;
                    break;
            }
            return Res;
        }

        public static byte[] GetByteArray(byte[] source, int start, int length)
        {
            byte[] Res = new byte[length];
            if (source != null && start >= 0 && length > 0 && source.Length >= (start + length))
            {
                Array.Copy(source, start, Res, 0, length);
                return Res;
            }
            else
            {
                return null;
            }
        }

        public static ushort GetUShortFromByteArray(byte[] source, int start = 0, DataFormat type = DataFormat.ABCD)
        {
            return BitConverter.ToUInt16(Get2ByteArray(source, start, type), 0);
        }

        public static ushort[] GetUShortArrayFromByteArray(byte[] source, DataFormat type = DataFormat.ABCD)
        {
            ushort[] result = new ushort[source.Length / 2];

            for (int i = 0; i < result.Length; i++)
            {
                result[i] = GetUShortFromByteArray(source, i * 2, type);
            }
            return result;
        }

        public static string UshortToString(ushort[] data)
        {
            List<byte> byteData = new List<byte>();
            for (int i = 0; i < data.Length; i++)
            {
                byteData.AddRange(BitConverter.GetBytes(data[i]));
            }

            return Encoding.UTF8.GetString(byteData.ToArray());
        }
    }
}
