// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.Json;
using System.Text.Json.Nodes;

namespace VgEAPClient.Common.Configuration;

internal class JsonConfigurationFileReader
{
    private static Dictionary<string, string?> _jsonData = new();

    public static Dictionary<string, string?> Parse(Stream stream)
    {
        var jsonDocumentOptions = new JsonDocumentOptions
        {
            CommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true,
        };

        var jsonNodeOptions = new JsonNodeOptions { PropertyNameCaseInsensitive = true };
        try
        {
            using (var reader = new StreamReader(stream))
            {
                var jsonNode = JsonNode.Parse(reader.ReadToEnd(), jsonNodeOptions, jsonDocumentOptions);

                if (jsonNode == null || jsonNode is not JsonObject)
                    throw new FormatException(string.Format("Top-level JSON element must be an object. Instead"));

                VisitJsonNode(jsonNode);

                return _jsonData;
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    private static void VisitJsonNode(JsonNode jsonNode)
    {
        if (jsonNode == null) return;
        if (jsonNode is JsonValue)
        {
            _jsonData.Add(jsonNode.GetPath(), jsonNode.ToString());
            return;
        }

        if (jsonNode is JsonObject jsonObj)
        {
            var enumerator = jsonObj.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var item = enumerator.Current;
                VisitJsonNode(item.Value);
            }
        }

        if (jsonNode is JsonArray jsonArr)
        {
            var enumerator = jsonArr.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var item = enumerator.Current;
                VisitJsonNode(item);
            }
        }
    }

    public static Dictionary<string, string?> ParseToDictionary(Stream stream)
    {
        using (var reader = new StreamReader(stream))
        {
            _jsonData = JsonSerializer.Deserialize<Dictionary<string, string?>>(reader.ReadToEnd())!;
        }

        return _jsonData;
    }
}
