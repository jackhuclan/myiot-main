namespace VgAutoDrill.Fundation.Utils;

public static class ConvertUtil
{
    //public static string UshortToString(this ushort[] ushorts)
    //{
    //    int byteLength = ushorts.Length * 2;
    //    byte[] buf = new byte[byteLength];
    //    Buffer.BlockCopy(ushorts, 0, buf, 0, byteLength);
    //    string str = System.Text.Encoding.UTF8.GetString(buf);
    //    return str;
    //}

    public static byte[] UshortToByte(this ushort[] ushorts)
    {

        int byteLength = ushorts.Length * 2;
        byte[] buf = new byte[byteLength];
        Buffer.BlockCopy(ushorts, 0, buf, 0, byteLength);
        return buf;
    }

    //public static ushort[] StringToUshort(this string str)
    //{
    //    byte[] buf = System.Text.Encoding.UTF8.GetBytes(str);
    //    ushort[] arr = new ushort[buf.Length / 2];
    //    Buffer.BlockCopy(buf, 0, arr, 0, buf.Length);
    //    return arr;
    //}


    public static ushort[] SetReal(this float value)
    {
        byte[] bytes = BitConverter.GetBytes(value);

        ushort[] dest = Bytes2Ushorts(bytes);

        return dest;
    }

    public static float GetReal(ushort[] src, int start)
    {
        ushort[] temp = new ushort[2];
        for (int i = 0; i < 2; i++)
        {
            temp[i] = src[i + start];
        }
        byte[] bytesTemp = Ushorts2Bytes(temp);
        float res = BitConverter.ToSingle(bytesTemp, 0);
        return res;
    }
    private static ushort[] Bytes2Ushorts(byte[] src, bool reverse = false)
    {
        int len = src.Length;

        byte[] srcPlus = new byte[len + 1];
        src.CopyTo(srcPlus, 0);
        int count = len >> 1;

        if (len % 2 != 0)
        {
            count += 1;
        }

        ushort[] dest = new ushort[count];
        if (reverse)
        {
            for (int i = 0; i < count; i++)
            {
                dest[i] = (ushort)(srcPlus[i * 2] << 8 | srcPlus[2 * i + 1] & 0xff);
            }
        }
        else
        {
            for (int i = 0; i < count; i++)
            {
                dest[i] = (ushort)(srcPlus[i * 2] & 0xff | srcPlus[2 * i + 1] << 8);
            }
        }

        return dest;
    }

    private static byte[] Ushorts2Bytes(ushort[] src, bool reverse = false)
    {

        int count = src.Length;
        byte[] dest = new byte[count << 1];
        if (reverse)
        {
            for (int i = 0; i < count; i++)
            {
                dest[i * 2] = (byte)(src[i] >> 8);
                dest[i * 2 + 1] = (byte)(src[i] >> 0);
            }
        }
        else
        {
            for (int i = 0; i < count; i++)
            {
                dest[i * 2] = (byte)(src[i] >> 0);
                dest[i * 2 + 1] = (byte)(src[i] >> 8);
            }
        }
        return dest;
    }

    public static bool ObjToBool(this object? obj)
    {
        return (obj is null) ? false : (bool)obj;
    }

    public static float ObjToFloat(this object obj)
    {
        return (obj is null) ? 0 : (float)obj;
    }

    public static int ObjToInt(this object? obj)
    {
        return (obj is null) ? 0 : (int)obj;
    }

}
