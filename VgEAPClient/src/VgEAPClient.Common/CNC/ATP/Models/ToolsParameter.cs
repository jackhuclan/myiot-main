// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

//namespace VgEAPClient.Common.CNC.ATP.Models;

//public class ToolsParameter
//{
//    [Key]
//    [MaxLength(4)]
//    [Comment("Tool")]
//    public string T { get; set; } = "T1";

//    [Precision(14, 3)]
//    [Comment("Diameter [mm]")]
//    public decimal D { get; set; } = 0.800M;

//    [Precision(14, 3)]
//    [Comment("Tool type")]
//    public string E { get; set; } = "drill";

//    [Precision(14, 1)]
//    [Comment("Spindle speed [krpm]")]
//    public decimal S { get; set; } = 65.0M;

//    [Precision(14, 3)]
//    [Comment("Feed rate [m/min]")]
//    public decimal F { get; set; } = 0.180M;

//    [Precision(14, 3)]
//    [Comment("Retract rate [m/min]")]
//    public decimal R { get; set; } = 1.500M;

//    [Comment("Dwell time [ms]")]
//    public uint A { get; set; } = 20U;

//    [Precision(14, 3)]
//    [Comment("Z offset  [mm]")]
//    public decimal Z { get; set; } = -0.120M;

//    [Comment("Tool life")]
//    public uint N { get; set; } = 2000;

//    [Comment("Current drilling tool life")]
//    public uint B { get; set; } = 841U;

//    [Precision(14, 3)]
//    [Comment("Current Routing tool life")]
//    public decimal C { get; set; } = 0.000M;

//    [Precision(14, 3)]
//    [Comment("Routing comp. diameter [mm]")]
//    public decimal U { get; set; } = 0.000M;

//    [Precision(14, 3)]
//    [Comment("Routing speed  [m/min]")]
//    public decimal V { get; set; } = 0.000M;

//    [Precision(14, 3)]
//    [Comment("Wear rate  [um/m]")]
//    public decimal W { get; set; } = 0.000M;

//    [Precision(14, 3)]
//    [Comment("Negative deameter tolerance [mm]")]
//    public decimal Qn { get; set; } = 0.000M;

//    [Precision(14, 3)]
//    [Comment("Positive diameter tolerance [mm]")]
//    public decimal Qp { get; set; } = 0.000M;

//    [Precision(14, 3)]
//    [Comment("Maximun of angular velocity [m/min]")]
//    public decimal VAngle { get; set; } = 0.000M;

//    [Precision(14, 3)]
//    [Comment("Routing comp. diameter of coutour [mm]")]
//    public decimal U2 { get; set; } = 0.000M;

//    [Precision(14, 3)]
//    [Comment("Routing speed of contour cleaning [m/min]")]
//    public decimal V2 { get; set; } = 0.000M;

//    //将 string 属性映射到 nvarchar(200) [Column(TypeName = "varchar(200)")]
//    //在 SQL Server 上创建 nvarchar(500) 类型的列 [MaxLength(500)]
//    //
//}
