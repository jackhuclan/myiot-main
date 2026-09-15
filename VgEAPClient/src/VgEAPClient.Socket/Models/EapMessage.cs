// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Xml.Serialization;

namespace VgEAPClient.Socket.Models;

[XmlRoot("message")]
public class EapMessage
{
    [XmlElement("header")]
    public EapHeader Header { get; set; } = new();

    [XmlElement("body")]
    public EapBody Body { get; set; } = new();

    [XmlElement("return")]
    public EapReturn Return { get; set; } = new();

    [XmlRoot("header")]
    public class EapHeader
    {
        [XmlElement("messagename")]
        public string MessageName { get; set; } = string.Empty;

        [XmlElement("transactionid")]
        public string TransactionId { get; set; } = string.Empty;
    }

    [XmlRoot("body")]
    public class EapBody
    {
    }

    [XmlRoot("return")]
    public class EapReturn
    {
        [XmlElement("returncode")]
        public string ReturnCode { get; set; } = string.Empty;

        [XmlElement("returnmessage")]
        public string ReturnMessage { get; set; } = string.Empty;
    }

    public override string ToString()
    {
        return EapMessageXmlHelper.Serialize(this);
    }
}
