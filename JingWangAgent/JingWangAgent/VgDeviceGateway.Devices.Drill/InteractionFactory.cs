// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

/********************************************************************************************
 *                         Suzhou Vega Technology Co., Ltd                                  *
 ********************************************************************************************
 * Copyright    : Copyright(c) Suzhou Vega Technology Co., Ltd                              *
 *                All rights reserved.                                                      *
 *                                                                                          *
 * DateTime       Author          Comment                                                   *
 * 2024.05.01    Li Haiyan        New                                                       *
 * 2024.05.01    Zhu Shipeng      Add Rear95                                                *
 ********************************************************************************************/

using VgAutoDrill.Infrastructure;
using VgDeviceGateway.Devices.Drill.DrillDevice;
using VgDeviceGateway.Devices.Drill.EventHandler;
using VgDeviceGateway.Devices.Drill.InteractionLoad;
using VgDeviceGateway.Devices.Drill.InteractionUnload;
using VgDeviceGateway.Devices.Drill.PropertyHandler;
using VgDeviceGateway.Devices.Drill.StateHandler;

namespace VgDeviceGateway.Devices.Drill
{
    public static class InteractionFactory
    {
        public static Dictionary<string, IDrillUnloadInteraction> CreateDrillInteractiveUnloadAllObject(IObjectFactory objectFactory, DefaultDrill defaultDrill)
        {
            Dictionary<string, IDrillUnloadInteraction> InteractionDrills = new Dictionary<string, IDrillUnloadInteraction>();
            InteractionDrills.TryAdd("Rear95UnloadPanel", objectFactory.CreateObject<Rear95DrillUnloadBoard>(defaultDrill));
            InteractionDrills.TryAdd("RearUnloadPanel", objectFactory.CreateObject<RearDrillUnloadBoard>(defaultDrill));
            InteractionDrills.TryAdd("FrontUnloadPanel", objectFactory.CreateObject<FrontDrillUnloadBoard>(defaultDrill));
            InteractionDrills.TryAdd("RearUnloadCutter", objectFactory.CreateObject<RearDrillUnloadTool>(defaultDrill));
            InteractionDrills.TryAdd("FrontUnloadCutter", objectFactory.CreateObject<FrontDrillUnloadTool>(defaultDrill));
            InteractionDrills.TryAdd("HalfRearUnloadPanel", objectFactory.CreateObject<HalfRearDrillUnloadBoard>(defaultDrill));
            return InteractionDrills;
        }

        public static Dictionary<string, IDrillLoadInteraction> CreateDrillInteractiveLoadAllObject(IObjectFactory objectFactory, DefaultDrill defaultDrill)
        {
            Dictionary<string, IDrillLoadInteraction> InteractionDrills = new Dictionary<string, IDrillLoadInteraction>();
            InteractionDrills.TryAdd("Rear95LoadPanel", objectFactory.CreateObject<Rear95DrillLoadBoard>(defaultDrill));
            InteractionDrills.TryAdd("RearLoadPanel", objectFactory.CreateObject<RearDrillLoadBoard>(defaultDrill));
            InteractionDrills.TryAdd("FrontLoadPanel", objectFactory.CreateObject<FrontDrillLoadBoard>(defaultDrill));
            InteractionDrills.TryAdd("RearLoadCutter", objectFactory.CreateObject<RearDrillLoadTool>(defaultDrill));
            InteractionDrills.TryAdd("FrontLoadCutter", objectFactory.CreateObject<FrontDrillLoadTool>(defaultDrill));
            InteractionDrills.TryAdd("HalfRearLoadPanel", objectFactory.CreateObject<HalfRearDrillLoadBoard>(defaultDrill));

            return InteractionDrills;
        }

        public static IDrillStateHandler CreatetDrillStateHandler(IObjectFactory objectFactory, DefaultDrill defaultDrill, int type)
        {
            // 0 CNC95只后上料  1 只前上料 2 只后上料  3 只前换刀  4 只后换刀（预）  5 前上料前换刀 6 前上料后换刀（预）  7 后上料前换刀 8后上料后换刀（预）
            //8  halfAutoRearPanel 全自动中的半自动（无agv）    9  FrontPanel95NoBuffer(95)(前面上板子没有buffer) 10 iot
            switch (type)
            {
                case 0:
                    return objectFactory.CreateObject<Rear95DrillStateHandler>(defaultDrill);

                case 1:
                    return objectFactory.CreateObject<FrontPanelDrillStateHandler>(defaultDrill);

                case 2:
                case 8:
                    return objectFactory.CreateObject<RearPanelDrillStateHandler>(defaultDrill);

                case 3:
                    return objectFactory.CreateObject<FrontToolDrillStateHandler>(defaultDrill);
                case 9:
                    return objectFactory.CreateObject<FrontPanel95NoBufferStateHandler>(defaultDrill);
                case 10:
                    return objectFactory.CreateObject<RearPanelIotDrillStateHandler>(defaultDrill);
                default:
                    break;
            }
            return null;
        }

        public static IDrillPropertyHandler CreatetDrillPropertyHandler(IObjectFactory objectFactory, DefaultDrill defaultDrill, int type)
        {
            // 0 CNC95只后上料  1 只前上料 2 只后上料  3 只前换刀  4 只后换刀（预）  5 前上料前换刀 6 前上料后换刀（预）  7 后上料前换刀 8后上料后换刀（预）
            //8  halfAutoRearPanel 全自动中的半自动（无agv）  9  FrontPanel95NoBuffer(95)(前面上板子没有buffer)   10 iot
            switch (type)
            {
                case 0:
                    return objectFactory.CreateObject<Rear95DrillPropertyHandler>(defaultDrill);

                case 1:
                    return objectFactory.CreateObject<FrontPanelDrillPropertyHandler>(defaultDrill);

                case 2:
                case 8:
                case 10:
                    return objectFactory.CreateObject<RearPanelDrillPropertyHandler>(defaultDrill);

                case 3:
                    return objectFactory.CreateObject<FrontToolDrillPropertyHandler>(defaultDrill);
                case 9:
                    return objectFactory.CreateObject<FrontPanel95NoBufferPropertyHandler>(defaultDrill);

                default:
                    break;
            }
            return null;
        }

        public static IDrillEventHandler CreatetDrillEventHandler(IObjectFactory objectFactory, DefaultDrill defaultDrill, int type)
        {
            // 0 CNC95只后上料  1 只前上料 2 只后上料  3 只前换刀  4 只后换刀（预）  5 前上料前换刀 6 前上料后换刀（预）  7 后上料前换刀 8后上料后换刀（预）
            // 8  halfAutoRearPanel 全自动中的半自动（无agv）  9  FrontPanel95NoBuffer(95)(前面上板子没有buffer) 10 iot
            switch (type)
            {
                case 0:
                    return objectFactory.CreateObject<Rear95DrillEventHandler>(defaultDrill);

                case 1:
                    return objectFactory.CreateObject<FrontPanelDrillEventHandler>(defaultDrill);

                case 2:
                case 8:
                    return objectFactory.CreateObject<RearPanelDrillEventHandler>(defaultDrill);

                case 3:
                    return objectFactory.CreateObject<FrontToolDrillEventHandler>(defaultDrill);
                case 9:
                    return objectFactory.CreateObject<FrontPanel95NoBufferEventHandler>(defaultDrill);
                case 10:
                    return objectFactory.CreateObject<RearPanelIotDrillEventHandler>(defaultDrill);

                default:
                    break;
            }
            return null;
        }

        public static IDrillDevice CreatetDrillDevice(IObjectFactory objectFactory, DefaultDrill defaultDrill, int type)
        {
            // 0 CNC95只后上料  1 只前上料 2 只后上料  3 只前换刀  4 只后换刀（预）  5 前上料前换刀 6 前上料后换刀（预）  7 后上料前换刀 8后上料后换刀（预）
            // 8  halfAutoRearPanel 全自动中的半自动（无agv）  9  FrontPanel95NoBuffer(95)(前面上板子没有buffer) 10 iot 交互
            switch (type)
            {
                case 0:
                    return objectFactory.CreateObject<Rear95DrillDevice>(defaultDrill);

                case 1:
                    return objectFactory.CreateObject<FrontPanelDrillDevice>(defaultDrill);

                case 2:
                case 8:
                case 10:
                    return objectFactory.CreateObject<RearPanelDrillDevice>(defaultDrill);

                case 3:
                    return objectFactory.CreateObject<FrontToolDrillDevice>(defaultDrill);
                case 9:
                    return objectFactory.CreateObject<FrontPanel95NoBuffer>(defaultDrill);

                default:
                    break;
            }
            return null;
        }
    }
}
