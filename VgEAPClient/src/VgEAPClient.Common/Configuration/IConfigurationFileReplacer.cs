// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient.Common.Configuration;

public interface IConfigurationFileReplacer
{
    string Replace(string json, Dictionary<string, string?> data);

    string Replace(string json, Dictionary<string, bool> data);

    string Replace(string json, Dictionary<string, object> data);

    string Replace(string json, Dictionary<string, string[]> data);
}
