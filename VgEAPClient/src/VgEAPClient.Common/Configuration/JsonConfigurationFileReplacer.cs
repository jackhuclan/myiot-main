// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Unicode;
using VgAutoDrill.Fundation.Utils;

namespace VgEAPClient.Common.Configuration;

internal class JsonConfigurationFileReplacer : IConfigurationFileReplacer
{
    private readonly Dictionary<string, JsonNode?> _jsonData = new();
    public string Replace(string json, Dictionary<string, string?> data)
    {
        var jsonDocumentOptions = new JsonDocumentOptions
        {
            CommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true,
        };

        var jsonNodeOptions = new JsonNodeOptions { PropertyNameCaseInsensitive = true };
        try
        {
            var jsonNode = JsonNode.Parse(json, jsonNodeOptions, jsonDocumentOptions);

            if (jsonNode == null || jsonNode is not JsonObject)
                throw new FormatException(string.Format("Top-level JSON element must be an object. Instead"));
            _jsonData.Clear();
            VisitJsonNode(jsonNode);

            foreach (var item in data)
            {
                var fullPath = item.Key.StartsWith("$") ? item.Key : GetFullPath(item.Key.Split(":"));
                if (!_jsonData.ContainsKey(fullPath)) continue;
                var targetNode = _jsonData[fullPath];
                if (targetNode == null || targetNode.Parent == null) continue;
                ReplaceJsonNode(targetNode.Parent, fullPath, item.Value);
            }

            var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.Create(UnicodeRanges.All) };
            return jsonNode.ToJsonString(options);
        }
        catch (Exception)
        {
            throw;
        }
    }

    private void VisitJsonNode(JsonNode jsonNode)
    {
        if (jsonNode == null || jsonNode is JsonValue) return;


        if (jsonNode is JsonObject jsonObj)
        {
            var enumerator = jsonObj.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var item = enumerator.Current;
                _jsonData.Add(item.Value.GetPath(), item.Value);
                VisitJsonNode(item.Value);
            }
        }

        if (jsonNode is JsonArray jsonArr)
        {
            var enumerator = jsonArr.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var item = enumerator.Current;
                _jsonData.Add(item.GetPath(), item);
                VisitJsonNode(item);
            }
        }
    }

    private static string GetFullPath(string[] paths) => string.Join('.', "$", string.Join('.', paths.Take(paths.Length).ToArray()));

    private void ReplaceJsonNode(JsonNode jsonNode, string fullPath, string? values)
    {
        if (jsonNode is JsonObject jsonObj)
        {
            var enumerator = jsonObj.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var item = enumerator.Current;
                if (item.Value.GetPath() == fullPath)
                {
                    jsonNode[item.Key] = values;
                    break;
                }
            }
        }

        if (jsonNode is JsonArray jsonArr)
        {
            var enumerator = jsonArr.GetEnumerator();
            var i = 0;
            while (enumerator.MoveNext())
            {
                var item = enumerator.Current;
                if (item.GetPath() == fullPath)
                {
                    jsonArr[i] = values;
                    break;
                }
                i++;
            }
        }
    }

    private void ReplaceJsonNode(JsonNode jsonNode, string fullPath, bool values)
    {
        if (jsonNode is JsonObject jsonObj)
        {
            var enumerator = jsonObj.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var item = enumerator.Current;
                if (item.Value.GetPath() == fullPath)
                {
                    jsonNode[item.Key] = values;
                    break;
                }
            }
        }

        if (jsonNode is JsonArray jsonArr)
        {
            var enumerator = jsonArr.GetEnumerator();
            var i = 0;
            while (enumerator.MoveNext())
            {
                var item = enumerator.Current;
                if (item.GetPath() == fullPath)
                {
                    jsonArr[i] = values;
                    break;
                }
                i++;
            }
        }
    }

    private void ReplaceJsonNode(JsonNode jsonNode, string fullPath, decimal values)
    {
        if (jsonNode is JsonObject jsonObj)
        {
            var enumerator = jsonObj.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var item = enumerator.Current;
                if (item.Value.GetPath() == fullPath)
                {
                    jsonNode[item.Key] = values;
                    break;
                }
            }
        }

        if (jsonNode is JsonArray jsonArr)
        {
            var enumerator = jsonArr.GetEnumerator();
            var i = 0;
            while (enumerator.MoveNext())
            {
                var item = enumerator.Current;
                if (item.GetPath() == fullPath)
                {
                    jsonArr[i] = values;
                    break;
                }
                i++;
            }
        }
    }
    private void ReplaceJsonNode(JsonNode jsonNode, string[] values)
    {

        if (jsonNode is JsonArray jsonArr)
        {
            jsonArr.Clear();
            foreach (var value in values)
            {
                jsonArr.Add(value);
            }
        }
    }

    public string Replace(string json, Dictionary<string, string[]> data)
    {
        var jsonDocumentOptions = new JsonDocumentOptions
        {
            CommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true,

        };

        var jsonNodeOptions = new JsonNodeOptions { PropertyNameCaseInsensitive = true };

        var jsonNode = JsonNode.Parse(json, jsonNodeOptions, jsonDocumentOptions);

        if (jsonNode == null || jsonNode is not JsonObject)
        {
            throw new FormatException(string.Format("Top-level JSON element must be an object. Instead"));
        }
        _jsonData.Clear();
        VisitJsonNode(jsonNode);
        foreach (var item in data)
        {
            var fullPath = item.Key.StartsWith("$") ? item.Key : GetFullPath(item.Key.Split(":"));
            if (!_jsonData.ContainsKey(fullPath)) continue;
            var targetNode = _jsonData[fullPath];
            if (targetNode == null || targetNode.Parent == null) continue;
            ReplaceJsonNode(targetNode, item.Value);
        }
        var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.Create(UnicodeRanges.All) };
        return jsonNode.ToJsonString(options);
    }

    public string Replace(string json, Dictionary<string, bool> data)
    {
        var jsonDocumentOptions = new JsonDocumentOptions
        {
            CommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true,
        };

        var jsonNodeOptions = new JsonNodeOptions { PropertyNameCaseInsensitive = true };
        try
        {
            var jsonNode = JsonNode.Parse(json, jsonNodeOptions, jsonDocumentOptions);

            if (jsonNode == null || jsonNode is not JsonObject)
                throw new FormatException(string.Format("Top-level JSON element must be an object. Instead"));
            _jsonData.Clear();
            VisitJsonNode(jsonNode);

            foreach (var item in data)
            {
                var fullPath = item.Key.StartsWith("$") ? item.Key : GetFullPath(item.Key.Split(":"));
                if (!_jsonData.ContainsKey(fullPath)) continue;
                var targetNode = _jsonData[fullPath];
                if (targetNode == null || targetNode.Parent == null) continue;
                ReplaceJsonNode(targetNode.Parent, fullPath, item.Value);
            }

            var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.Create(UnicodeRanges.All) };
            return jsonNode.ToJsonString(options);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public string Replace(string json, Dictionary<string, object> data)
    {
        var jsonDocumentOptions = new JsonDocumentOptions
        {
            CommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true,
        };

        var jsonNodeOptions = new JsonNodeOptions { PropertyNameCaseInsensitive = true };
        try
        {
            var jsonNode = JsonNode.Parse(json, jsonNodeOptions, jsonDocumentOptions);

            if (jsonNode == null || jsonNode is not JsonObject)
                throw new FormatException(string.Format("Top-level JSON element must be an object. Instead"));
            _jsonData.Clear();
            VisitJsonNode(jsonNode);

            foreach (var item in data)
            {
                var fullPath = item.Key.StartsWith("$") ? item.Key : GetFullPath(item.Key.Split(":"));
                if (!_jsonData.ContainsKey(fullPath)) continue;
                var targetNode = _jsonData[fullPath];
                if (targetNode == null || targetNode.Parent == null) continue;
                if (item.Value is decimal
                    || item.Value is int
                    || item.Value is short
                    || item.Value is long
                    || item.Value is float
                    || item.Value is double
                    || item.Value is uint)
                {
                    ReplaceJsonNode(targetNode.Parent, fullPath, item.Value.ToDecimal());
                }
                else if (item.Value is bool)
                {
                    ReplaceJsonNode(targetNode.Parent, fullPath, item.Value.ToBool());
                }
                else
                {
                    ReplaceJsonNode(targetNode.Parent, fullPath, item.Value.ToString());
                }


            }

            var options = new JsonSerializerOptions { WriteIndented = true, Encoder = JavaScriptEncoder.Create(UnicodeRanges.All) };
            return jsonNode.ToJsonString(options);
        }
        catch (Exception)
        {
            throw;
        }
    }
}
