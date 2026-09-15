using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace VgAutoDrill.Fundation.Utils;

public static class ObjectExtensions
{
    public static bool ToBool(this object? tVal)
    {
        if (tVal == null)
        {
            return false;
        }

        if (bool.TryParse(tVal.ToString(), out bool res))
        {
            return res;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 数据转换为DateTime类型
    /// </summary>
    /// <param name="val"></param>
    /// <returns></returns>
    public static DateTime ToDate(this object val)
    {
        DateTime result = DateTime.MinValue;
        if (val != null && DateTime.TryParse(val.ToString(), out result))
            result = DateTime.Parse(val.ToString());
        return result;
    }

    public static int ToInt(this object? tVal, int defaultInt = 0)
    {
        if (tVal == null)
        {
            return defaultInt;
        }

        if (int.TryParse(tVal.ToString(), out int res))
        {
            return res;
        }
        else
        {
            return defaultInt;
        }
    }

    public static float ToFloat(this object? tVal, float defaultFloat = 0.0f)
    {
        if (tVal == null)
        {
            return defaultFloat;
        }

        if (float.TryParse(tVal.ToString(), out float res))
        {
            return res;
        }
        else
        {
            return defaultFloat;
        }
    }

    public static double ToDouble(this object? tVal, double defaultFloat = 0.0)
    {
        if (tVal == null)
        {
            return defaultFloat;
        }

        if (double.TryParse(tVal.ToString(), out double res))
        {
            return res;
        }
        else
        {
            return defaultFloat;
        }
    }

    public static byte ToByte(this object? tVal, byte defaultbyte = 0)
    {
        if (tVal == null)
        {
            return defaultbyte;
        }

        if (byte.TryParse(tVal.ToString(), out byte res))
        {
            return res;
        }
        else
        {
            return defaultbyte;
        }
    }

    public static ushort ToUshort(this object? tVal, ushort defaultshort = 0)
    {
        if (tVal == null)
        {
            return defaultshort;
        }

        if (ushort.TryParse(tVal.ToString(), out ushort res))
        {
            return res;
        }
        else
        {
            return defaultshort;
        }
    }

    public static string ToStr(this object? tVal, string defaultstring = "")
    {
        if (tVal == null)
        {
            return defaultstring;
        }
        return tVal.ToString();
    }

    public static long ToLong(this object? tVal, long defaultlong = 0)
    {
        if (tVal == null)
        {
            return defaultlong;
        }
        if (long.TryParse(tVal.ToString(), out long res))
        {
            return res;
        }
        else
        {
            return defaultlong;
        }
    }

    public static ushort[] FloatToReal(this float value)
    {
        /*float value = 123.45f;*/ // 设置要写入的浮点数值
        //byte[] valueBytes = BitConverter.GetBytes(value); // 将浮点数转换为字节数组
        //short highBytes = BitConverter.ToInt16(valueBytes, 0); // 取字节数组的第3、4位，并解析为高16位数据
        //short lowBytes = BitConverter.ToInt16(valueBytes, 2); // 取字节数组的第1、2位，并解析为低16位数据

        //ushort highOrderValue = BitConverter.ToUInt16(BitConverter.GetBytes(value), 0);

        //ushort lowOrderValue = BitConverter.ToUInt16(BitConverter.GetBytes(value), 2);
        //return new ushort[] { highOrderValue, lowOrderValue };

        byte[] valueBytes = BitConverter.GetBytes(value);
        ushort[] dest = Bytes2Ushorts(valueBytes);
        return dest;
    }

    public static float UshortsToFloat(this ushort[] val, float defaultfloat = 0)
    {
        if (val.Length != 2)
        {
            return defaultfloat;
        }
        List<byte> result = new List<byte>();

        result.AddRange(BitConverter.GetBytes(val[0]));

        result.AddRange(BitConverter.GetBytes(val[1]));

        float floatValue = BitConverter.ToSingle(result.ToArray(), 0);
        return floatValue;
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

    public static string UshortToString(this ushort[] inUshort)
    {
        byte[] outByte = new byte[inUshort.Length * 2];

        for (int i = 0; i < inUshort.Length; i++)
        {
            byte[] bufByte = BitConverter.GetBytes(inUshort[i]);
            outByte[i * 2] = bufByte[1];

            outByte[i * 2 + 1] = bufByte[0];
        }

        string str = ASCIIEncoding.ASCII.GetString(outByte).Trim();
        str = str.Trim("\0".ToCharArray()).Trim("0\0".ToCharArray());
        return str;
    }

    public static ushort[] StringToUshort(this String inString)
    {
        if (inString.Length % 2 == 1) { inString += " "; }

        char[] bufChar = inString.ToCharArray();

        byte[] outByte = new byte[bufChar.Length];

        byte[] bufByte = new byte[2];

        ushort[] outShort = new ushort[bufChar.Length / 2];

        for (int i = 0, j = 0; i < bufChar.Length; i += 2, j++)

        {
            bufByte[0] = BitConverter.GetBytes(bufChar[i])[0];

            bufByte[1] = BitConverter.GetBytes(bufChar[i + 1])[0];

            outShort[j] = BitConverter.ToUInt16(bufByte, 0);
        }

        return outShort;
    }

    /// <summary>
    /// json 序列化
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <param name="jsonSerializer"></param>
    /// <returns></returns>
    public static string ToJson<T>(this T obj, JsonSerializerOptions? jsonSerializer = null)
    {
        return JsonSerializer.Serialize(obj, jsonSerializer ?? new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        });
    }
}
