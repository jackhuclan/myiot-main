// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VegaIot.External.AgvEntity.STD;

namespace VegaIot.External.Std.Shelf;
public class BindSiloStockEntity : STDBaseEntity
{
    public string mapDataCode { get; set; }
    /// <summary>
    /// 托盘编号
    /// </summary>
    public String? podCode { get; set; }
    /// <summary>
    /// 1:绑定 0:解绑
    /// </summary>
    public String indBind { get; set; }
}
