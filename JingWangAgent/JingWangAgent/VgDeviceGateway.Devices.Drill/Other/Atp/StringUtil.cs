using System.Text;

namespace VgDeviceGateway.Devices.Drill.Other.Atp
{
    public class StringUtil
    {
        /// <summary>
        /// 根据拆分的字符数组对字符串进行拆分处理，返回处理后的字符串数组和没有包含的字符
        /// </summary>
        public static (List<string> segments, List<char> missingChars) SplitString(string inputString, params char[] delimiters)
        {
            List<string> segments = new List<string>();
            List<char> missingChars = new List<char>(delimiters);

            if (!string.IsNullOrEmpty(inputString))
            {
                StringBuilder segmentBuilder = new StringBuilder();
                bool isInQuotes = false;

                foreach (char c in inputString)
                {
                    if (c == '"')
                    {
                        isInQuotes = !isInQuotes;
                    }
                    if (!isInQuotes && delimiters.Contains(c))
                    {
                        missingChars.Remove(c);

                        segments.Add(segmentBuilder.ToString());
                        segmentBuilder.Clear();
                    }
                    else
                    {
                        segmentBuilder.Append(c);
                    }
                }
                segments.Add(segmentBuilder.ToString());
            }

            return (segments, missingChars);
        }

        /// <summary>
        /// 根据拆分的字符数组对字符串进行拆分处理，返回处理后的字符串数组和出现顺序的字符列表
        /// </summary>
        public static (List<string> segments, List<char> containsChars) SplitString2(string inputString, params char[] delimiters)
        {
            List<string> segments = new List<string>();
            List<char> containsChars = new List<char>();

            if (!string.IsNullOrEmpty(inputString))
            {
                StringBuilder segmentBuilder = new StringBuilder();
                foreach (char c in inputString)
                {
                    if (delimiters.Contains(c))
                    {
                        containsChars.Add(c);
                        segments.Add(segmentBuilder.ToString());
                        segmentBuilder.Clear();
                    }
                    else
                    {
                        segmentBuilder.Append(c);
                    }
                }
                segments.Add(segmentBuilder.ToString());
            }
            return (segments, containsChars);
        }

        public static int GetIntCode(string value)
        {
            return string.IsNullOrEmpty(value) ? 0 : int.Parse(value);
        }

        public static float GetFloatCode(string value)
        {
            if (value == "*")
            {
                return 0;
            }
            float floatValue = string.IsNullOrEmpty(value) ? 0.00f : float.Parse(value);
            return (float)Math.Round(floatValue, 3);
        }
    }
}
