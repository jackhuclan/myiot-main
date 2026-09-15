// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VgEAPClient.Common;
public class AOIDbOptions
{
    public bool IsEnable { get; set; } = false;
    public string ConnectionString { get; set; } = string.Empty;
    public int HisDay { get; set; } = 0;
}
