// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient.Common.Util;
public class VegaLanguage
{

    public string Default { get; set; } = string.Empty;
    public string en { get; set; } = string.Empty;

    public VegaLanguage()
    {

    }
    public VegaLanguage(string _Default, string _en)
    {
        Default = _Default;
        en = _en;
    }
}
