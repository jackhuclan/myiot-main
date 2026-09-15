// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using VgEAPClient.Socket.Models;

namespace VgEAPClient.Socket;

/// <summary>
/// todo: need to verify
/// </summary>
internal static class EapMessageJsonHelper
{
    public static string ExtractMessageNameFromJson(string xml)
    {
        Regex regex = new Regex("\"MessageName\":\"(.*)\",", RegexOptions.Compiled);
        var match = regex.Match(xml);
        return match.Groups[1].Value;
    }

    public static string ExtractBodyFromJson(string xml)
    {
        Regex regex = new Regex("\"Body\":(.*),", RegexOptions.Compiled);
        var match = regex.Match(xml);
        return match.Groups[1].Value;
    }

    public static string Serialize<T>(T serializingObject)
        where T : EapMessage
    {
        var serializeOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            Converters =
            {
                new EapMessageJsonConverter()
            }
        };

        return JsonSerializer.Serialize(serializingObject, serializeOptions);
    }

    public static T? Deserialize<T>(string text, T? defaultV = default)
        where T : EapMessage
    {
        var serializeOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            Converters =
            {
                new EapMessageJsonConverter()
            }
        };

        var obj = JsonSerializer.Deserialize<T>(text, serializeOptions);
        return obj ?? defaultV ?? default;
    }
}

internal class EapMessageJsonConverter : JsonConverter<EapMessage>
{
    public override EapMessage? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string message = reader.GetString() ?? string.Empty;
        if (string.IsNullOrEmpty(message)) return null;

        string messageName = EapMessageJsonHelper.ExtractMessageNameFromJson(message);
        string bodyContent = EapMessageJsonHelper.ExtractBodyFromJson(message);
        var obj = JsonSerializer.Deserialize<EapMessage?>(message);
        if (EapBodyModelMapping.ModelTypes.TryGetValue(messageName, out var modelType))
        {
            var body = JsonSerializer.Deserialize(bodyContent, modelType) as EapMessage.EapBody;
            if (obj != null && body != null)
            {
                obj.Body = body;
            }
        }

        return obj;
    }

    public override void Write(Utf8JsonWriter writer, EapMessage value, JsonSerializerOptions options)
    {
        writer.WriteStartObject(value.Header.GetType().Name);
        JsonSerializer.Serialize(writer, value.Header, options);
        writer.WriteEndObject();

        writer.WriteStartObject(value.Body.GetType().Name);
        foreach (var propertyInfo in value.Body.GetType().GetProperties())
        {
            var property = propertyInfo.GetValue(value.Body, null);
            JsonSerializer.Serialize(writer, property, options);
        }
        writer.WriteEndObject();

        writer.WriteStartObject(value.Return.GetType().Name);
        JsonSerializer.Serialize(writer, value.Return, options);
        writer.WriteEndObject();
    }
}
