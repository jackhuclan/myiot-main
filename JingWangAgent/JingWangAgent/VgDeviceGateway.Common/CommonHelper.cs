using System.Text;
using VgAutoDrill.Fundation.Utils;

namespace VgDeviceGateway.Devices.Common;

public static class CommonHelper
{
    public static string UshortToStrings(this ushort[] inUshort)
    {
        byte[] array = new byte[inUshort.Length * 2];
        for (int i = 0; i < inUshort.Length; i++)
        {
            byte[] bytes = BitConverter.GetBytes(inUshort[i]);
            array[i * 2] = bytes[1];
            array[i * 2 + 1] = bytes[0];
        }

        return Encoding.ASCII.GetString(array).Trim().Trim("\0".ToCharArray());
    }

    public static float UshortToFloat(this ushort[] inUshort)
    {
        string[] array = new string[inUshort.Length];
        for (int i = 0; i < inUshort.Length; i++)
        {
            array[i] = Convert.ToString(inUshort[i], 2);
        }

        string contactStr = "";
        for (int i = inUshort.Length - 1; i >= 0; i--)
        {
            int complementation = (array[i].Length) % 8;
            contactStr += complementation == 0 ? array[i] : string.Join("", Enumerable.Repeat("0", 8 - complementation)) + array[i];
        }

        string uFloat = Convert.ToInt32(contactStr, 2).ToStr();

        return uFloat.Length > 2 ? string.Concat(uFloat.Substring(0, uFloat.Length - 2), ".", uFloat.Substring(uFloat.Length - 2)).ToFloat() : uFloat.ToFloat();
    }

    public static ushort[] StringFloatToUshort(this float input)
    {
        int intValue = (input.ToFloat() * 100).ToInt();
        string bit32Value = Convert.ToString(intValue, 2).PadLeft(32, '0');
        ushort low = Convert.ToUInt16(bit32Value.Substring(0, 16), 2);
        ushort high = Convert.ToUInt16(bit32Value.Substring(16), 2);
        return new ushort[] { high, low };
    }

    public static string ConvertUshortArrayToStr(this ushort[] input)
    {

        // 用于存储结果的StringBuilder
        StringBuilder resultBuilder = new StringBuilder();

        foreach (ushort num in input)
        {
            // 提取低字节（最低有效8位）
            byte lowByte = (byte)(num & 0xFF);
            // 提取高字节（最高有效8位）
            byte highByte = (byte)((num >> 8) & 0xFF);

            // 将低位字节转换为ASCII字符（先添加）
            char lowChar = Convert.ToChar(lowByte);
            // 将高位字节转换为ASCII字符（后添加）
            char highChar = Convert.ToChar(highByte);

            // 添加交换后的字符：低位在前，高位在后
            resultBuilder.Append(lowChar);
            resultBuilder.Append(highChar);
        }
        string finalString = resultBuilder.ToString().Trim('\0');
        return finalString;
    }
}
