// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VegaIot.External.Bomin
{
    public class FtpDrillFilePathOptions
    {
        public string FtpHost { get; set; } = "127.0.0.1";
        public string FtpUsername { get; set; } = "dril";
        public string FtpPassword { get; set; } = "JlcDril123";
        public int FtpPort { get; set; } = 21;
        public string FtpPath { get; set; } = "Dril/PCB1-ZK/";
        public string LocalDirectory { get; set; } = "D:\\data";
        public string GetDrillRecipes { get; set; } = string.Empty;
    }
}
