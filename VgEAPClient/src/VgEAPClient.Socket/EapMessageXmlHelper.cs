// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using VgEAPClient.Socket.Models;

namespace VgEAPClient.Socket;

public static class EapMessageXmlHelper
{
    public static string ExtractMessageNameFromXml(string xml, string messagename = "messagename")
    {
        var regex = new Regex($"<{messagename}>(.*)</{messagename}>", RegexOptions.Compiled);
        var match = regex.Match(xml);
        return match.Groups[1].Value;
    }

    public static string Serialize<T>(T serializingObject)
        where T : EapMessage
    {
        var messageName = serializingObject.Header.MessageName;
        var attrOverrides = CreateEapBodyXmlAttributeOverrides(messageName);

        var serializer = new XmlSerializer(typeof(T), attrOverrides);
        var sb = new StringBuilder();
        using (var sw = new StringWriter(sb))
        {
            serializer.Serialize(sw, serializingObject);
        }

        return sb.ToString();
    }

    public static T? Deserialize<T>(string text, T? defaultV = default, string nodename = "messagename")
        where T : EapMessage
    {
        var messageName = ExtractMessageNameFromXml(text, nodename);
        var attrOverrides = CreateEapBodyXmlAttributeOverrides(messageName);

        var serializer = new XmlSerializer(typeof(T), attrOverrides);
        var textReader = new StringReader(text);
        var obj = serializer.Deserialize(textReader);
        if (obj != null)
        {
            return (T)obj;
        }

        return defaultV ?? default;
    }

    private static XmlAttributeOverrides CreateEapBodyXmlAttributeOverrides(string messageName)
    {
        var attrOverrides = new XmlAttributeOverrides();

        if (EapBodyModelMapping.ModelTypes.TryGetValue(messageName, out var modelType))
        {
            XmlAttributes attrs = new XmlAttributes();
            XmlElementAttribute attr = new XmlElementAttribute();
            attr.ElementName = "body";
            attr.Type = modelType;
            attrs.XmlElements.Add(attr);

            attrOverrides.Add(typeof(EapMessage), "Body", attrs);
        }

        return attrOverrides;
    }
}
