/********************************************************************************************
 *                         Suzhou Vega Technology Co., Ltd                                  *
 ********************************************************************************************
 * Copyright    : Copyright(c) Suzhou Vega Technology Co., Ltd                              *
 *                All rights reserved.                                                      *
 *                                                                                          *
 * DateTime       Author          Comment                                                   *
 * 2024.05.01    Li Haiyan        New                                                       *
 * 2024.06.01    Zhu Shipeng      Re-implementation of CNC84                                *
 ********************************************************************************************/

using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Utils;

namespace VgDeviceGateway.Devices.Drill.StateHandler
{
    public class FrontPanel95NoBufferStateHandler : DeviceShare<DefaultDrill>, IDrillStateHandler
    {
        private readonly ILogger<FrontPanel95NoBufferStateHandler> logger;

        public FrontPanel95NoBufferStateHandler(ILogger<FrontPanel95NoBufferStateHandler> logger, IServiceProvider serviceProvider, DefaultDrill device) : base(serviceProvider, device)
        {
            this.logger = logger;
        }

        public bool SetExceptionStateCondition(WatchableProperties properties)
        {
         
            var isNoErrorOnCnc84 = properties.Property("Drill_NoError").NewValue.ToBool();
            var isHaltOnCnc84 = properties.Property("Drill_Halt").NewValue.ToBool();

            return isHaltOnCnc84 || !isNoErrorOnCnc84;
   
        }

        public bool SetWorkingStateCondition(WatchableProperties properties)
        {
 
            var isDrillHoleEndOnCnc84 = properties.Property("Drill_DrillHoleEnd").NewValue.ToBool();
            return !isDrillHoleEndOnCnc84;
        }

        public bool SetReadyStateCondition(WatchableProperties properties)
        {
            var isNoErrorOnCnc84 = properties.Property("Drill_NoError").NewValue.ToBool();
            var isHaltOnCnc84 = properties.Property("Drill_Halt").NewValue.ToBool();
            var isDrillHoleEndOnCnc84 = properties.Property("Drill_DrillHoleEnd").NewValue.ToBool();

            return isNoErrorOnCnc84 && !isHaltOnCnc84 && isDrillHoleEndOnCnc84;
        }

        public List<string> SetReadyProperties()
        {
            return new List<string>()
            {  
                "Drill_NoError",
                "Drill_Halt",                
            };
        }

        public List<string> SetWorkProperties()
        {
            return new List<string>()
            {               
                "Drill_DrillHoleEnd",
            };
        }

        public List<string> SetExceptionProperties()
        {
            return new List<string>()
            {
                "Drill_NoError",
                "Drill_Halt"
            };
        }
    }
}
