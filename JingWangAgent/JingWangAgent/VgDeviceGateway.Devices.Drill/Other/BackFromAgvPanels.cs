// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.Concurrent;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgDeviceGateway.Devices.Drill.Other;
public class BackFromAgvPanels
{
    public ConcurrentBag<Panel> CourrentBufferRawPanel { get; private set; } = new ConcurrentBag<Panel>();
    public ConcurrentBag<Panel> BufferRawPanel { get; private set; } = new ConcurrentBag<Panel>();
    public ConcurrentBag<Panel> DrillPanel { get; private set; } = new ConcurrentBag<Panel>();
    public ConcurrentBag<Panel> BufferClinkerPanel { get; private set; } = new ConcurrentBag<Panel>();

    public void AddNewPanel(Panel panel)
    {
        if (!CourrentBufferRawPanel.Any(s => s.TaskCode == panel.TaskCode))
        {
            BufferRawPanel = CourrentBufferRawPanel;
            CourrentBufferRawPanel = new ConcurrentBag<Panel>();
        }
        CourrentBufferRawPanel.Add(panel);
    }



}
