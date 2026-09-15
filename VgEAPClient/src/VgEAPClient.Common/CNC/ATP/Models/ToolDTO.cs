// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient.Common.CNC.ATP.Models;

[Serializable]
public class ToolDTO
{
    public int toolId { get; set; }//T
    public float toolDiameter { get; set; }//D
    public int toolType { get; set; }//E
    public float spindleSpeed { get; set; }//S
    public float infeedZAxis { get; set; }//F
    public float retractZAxis { get; set; }//R
    public int waitTime { get; set; }//A
    public float workPlaneAdjustment { get; set; }//Z
    public int toolLife { get; set; }//N
    public int drillToolLife { get; set; }//B
    public float routerToolLife { get; set; }//C
    public float toolLifeMonitoring { get; set; }//
    public float compensationDiameter { get; set; }//
    public float routingFeedRate { get; set; }//V
    public float routerWear { get; set; }//W
    public float circularRoutingFeedRate { get; set; }//
    public float J { get; set; }
    public float I { get; set; }
    public List<bool> toolFunctions { get; set; } = Enumerable.Range(1, 16).Select(f => false).ToList();
    public float unknownL { get; set; }
    public List<float> drillMethod { get; set; } = Enumerable.Range(1, 6).Select(f => new float()).ToList();
    public List<int> magazines { get; set; } = new List<int>();

    public ToolDTO Clone()
    {
        ToolDTO cloneToolDTO = new ToolDTO();
        cloneToolDTO.toolId = this.toolId;
        cloneToolDTO.toolDiameter = this.toolDiameter;
        cloneToolDTO.toolType = this.toolType;
        cloneToolDTO.spindleSpeed = this.spindleSpeed;
        cloneToolDTO.infeedZAxis = this.infeedZAxis;
        cloneToolDTO.retractZAxis = this.retractZAxis;
        cloneToolDTO.waitTime = this.waitTime;
        cloneToolDTO.workPlaneAdjustment = this.workPlaneAdjustment;
        cloneToolDTO.toolLife = this.toolLife;
        cloneToolDTO.drillToolLife = this.drillToolLife;
        cloneToolDTO.routerToolLife = this.routerToolLife;
        cloneToolDTO.toolLifeMonitoring = this.toolLifeMonitoring;
        cloneToolDTO.compensationDiameter = this.compensationDiameter;
        cloneToolDTO.routingFeedRate = this.routingFeedRate;
        cloneToolDTO.routerWear = this.routerWear;
        cloneToolDTO.circularRoutingFeedRate = this.circularRoutingFeedRate;
        cloneToolDTO.J = this.J;
        cloneToolDTO.I = this.I;
        cloneToolDTO.toolFunctions = new List<bool>(this.toolFunctions);
        cloneToolDTO.unknownL = this.unknownL;
        cloneToolDTO.drillMethod = new List<float>(this.drillMethod);
        cloneToolDTO.magazines = new List<int>(this.magazines);
        return cloneToolDTO;
    }
}
