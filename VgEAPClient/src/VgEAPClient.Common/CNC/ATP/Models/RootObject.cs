// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient.Common.CNC.ATP.Models;

public class Arrange
{
    public int BoxIndex { get; set; }
    public int Site { get; set; }
    public double PgmDiameter { get; set; }
    public int MoCount { get; set; }
    public int Life { get; set; }
}

public class LifeDefine
{
    public double PgmDiameter { get; set; }
    public int MoCount { get; set; }
    public int Life { get; set; }
}

public class Box
{
    public int BoxIndex { get; set; }
    public string SpindleBoxCode_1 { get; set; }
    public string SpindleBoxCode_2 { get; set; }
    public string SpindleBoxCode_3 { get; set; }
    public string SpindleBoxCode_4 { get; set; }
    public string SpindleBoxCode_5 { get; set; }
    public string SpindleBoxCode_6 { get; set; }
}

public class Data
{
    public string WipId { get; set; }
    public List<LifeDefine> lifeDefines { get; set; }
    public List<Arrange> Arranges { get; set; }
    public List<Box> Boxs { get; set; }
}

public class RootObject
{
    public string Code { get; set; }
    public string Info { get; set; }
    public Data Data { get; set; }
}
