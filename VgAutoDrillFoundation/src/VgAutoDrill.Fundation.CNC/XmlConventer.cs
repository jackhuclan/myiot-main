using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using VgAutoDrill.Fundation.CNC.Packet;

namespace VgAutoDrill.Fundation.CNC;

public static class XmlConventer
{
    private static XmlSerializer xmlSerializer = new XmlSerializer(typeof(SMDncPacket));

    public static SMDncPacket Deserialize(string xmlStr)
    {
        xmlStr = ChangeMessage(xmlStr);
        xmlStr = Value(xmlStr);
        using (var sr = new StringReader(xmlStr))
        {
            var data = xmlSerializer.Deserialize(sr);
            if (data is SMDncPacket packet)
            {
                return packet;
            }
        }
        return null;
    }



    public static string Serialize(SMDncPacket packet, int requestId = -1)
    {
        var sb = new StringBuilder();
        using (var sw = new StringWriter(sb))
        {
            xmlSerializer.Serialize(sw, packet);
        }

        StringBuilder stringBuilder = new StringBuilder();
        using (var sr = new StringReader(sb.ToString()))
        {
            int line = 0;
            while (true)
            {
                var str = sr.ReadLine();
                line++;
                if (line == 1)
                {
                    continue;
                }
                if (string.IsNullOrEmpty(str))
                {
                    break;
                }
                str = str.Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", string.Empty);
                str = str.Replace(CommFunc.NULLVALUE, string.Empty);
                if (str.Contains(CommFunc.REQUESTID))
                {
                    str = str.Replace(CommFunc.REQUESTID, requestId.ToString());
                }
                str = Regex.Replace(str, "\\s+<\\w+ xsi:nil=\"true\" \\/>", string.Empty);
                if (string.IsNullOrWhiteSpace(str))
                {
                    continue;
                }
                stringBuilder.Append(str.Trim());
            }
        }
        return stringBuilder.ToString();
    }

    #region 过滤消息内的
    private static string ChangeMessage(string message)
    {
        var index = message.IndexOf("</");
        var index2 = message.IndexOf('>', index);
        var s = message.Substring(index + 2, index2 - index - 2);
        var index3 = message.IndexOf(s);
        var index4 = message.IndexOf('>', index3);
        var body = message.Substring(index4 + 1, index - index4 - 1);
        body = body.Replace("<", "(").Replace(">", ")");
        StringBuilder sb = new StringBuilder(message.Length);
        sb.Append(message.Substring(0, index4 + 1));
        sb.Append(body);
        sb.Append(message.Substring(index, message.Length - index));
        return sb.ToString();
    }
    #endregion

    #region 给Value值添加"
    private static string Value(string str)
    {
        var ms = Regex.Matches(str, "VALUE=([\\w]*)>");

        StringBuilder newStr = new StringBuilder(str.Length + ms.Count * 2);
        int startIndex = 0;
        foreach (Match mm in ms)
        {
            var tempStr = str.Substring(startIndex, mm.Index - startIndex + 6);
            newStr.Append(tempStr);
            newStr.Append("\"");
            tempStr = mm.Value.Substring(6, mm.Length - 6 - 1);
            newStr.Append(tempStr);
            newStr.Append("\"");
            newStr.Append(mm.Value.Last());
            startIndex = mm.Index + mm.Length;
        }

        newStr.Append(str.Substring(startIndex, str.Length - startIndex));
        return newStr.ToString();
    }
    #endregion
}
