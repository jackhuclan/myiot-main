using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Utils;

namespace VgDeviceGateway.Devices.Drill.StateHandler
{
    public class RearPanelDrillStateHandler : DeviceShare<DefaultDrill>, IDrillStateHandler
    {
        private readonly ILogger<RearPanelDrillStateHandler> logger;

        public RearPanelDrillStateHandler(ILogger<RearPanelDrillStateHandler> logger, IServiceProvider serviceProvider, DefaultDrill device) : base(serviceProvider, device)
        {
            this.logger = logger;
        }

        public bool SetExceptionStateCondition(WatchableProperties properties)
        {
            var isAutomaticOnBuffer = properties.Property("Buffer_Automatic").NewValue.ToBool();
            var isNoErrorOnBuffer = properties.Property("Buffer_NoError").NewValue.ToBool();
            var isNoErrorOnCnc84 = properties.Property("Drill_NoError").NewValue.ToBool();
            var isHaltOnCnc84 = properties.Property("Drill_Halt").NewValue.ToBool();

            return isHaltOnCnc84
            || !isNoErrorOnBuffer
            || !isNoErrorOnCnc84
            || !isAutomaticOnBuffer;
        }

        public bool SetWorkingStateCondition(WatchableProperties properties)
        {
            var isAutomaticOnBuffer = properties.Property("Buffer_Automatic").NewValue.ToBool();
            var isNoErrorOnBuffer = properties.Property("Buffer_NoError").NewValue.ToBool();
            var isNoErrorOnCnc84 = properties.Property("Drill_NoError").NewValue.ToBool();
            var isHaltOnCnc84 = properties.Property("Drill_Halt").NewValue.ToBool();
            var isExistBoardOnCnc84 = properties.Property("Drill_ExistBoard").NewValue.ToBool();
            var isOnAgvPositionOnBuffer = properties.Property("Buffer_Position").NewValue.ToBool();
            var isPressBoardEndOnCnc84 = properties.Property("Drill_PressBoardEnd").NewValue.ToBool();
            var isFrontMushroomOpenOnCnc84 = properties.Property("Drill_FrontMushroom").NewValue.ToBool();
            var isMiddleMushroomOpenOnCnc84 = properties.Property("Drill_MiddleMushroom").NewValue.ToBool();
            var isDrillHoleEndOnCnc84 = properties.Property("Drill_DrillHoleEnd").NewValue.ToBool();

            if (InteractingDevice.Status == DeviceStatus.Ready)
            {
                var subflag = isOnAgvPositionOnBuffer
                && isPressBoardEndOnCnc84
                && isFrontMushroomOpenOnCnc84
                && !isDrillHoleEndOnCnc84;

                if (InteractingDevice.middleMushroomExist)
                {
                    return subflag && isMiddleMushroomOpenOnCnc84;
                }
                return subflag;
            }
            else if (InteractingDevice.Status == DeviceStatus.Exception)
            {
                return !isHaltOnCnc84
                && isNoErrorOnBuffer
                && isNoErrorOnCnc84
                && isAutomaticOnBuffer
                && !isDrillHoleEndOnCnc84
                && isExistBoardOnCnc84;
            }

            return false;
        }

        public bool SetReadyStateCondition(WatchableProperties properties)
        {
            var isPowerOnBuffer = properties.Property("Buffer_EnergizeStatus").NewValue.ToBool();
            var isAutomaticOnBuffer = properties.Property("Buffer_Automatic").NewValue.ToBool();
            var isNoErrorOnBuffer = properties.Property("Buffer_NoError").NewValue.ToBool();
            var isNoErrorOnCnc84 = properties.Property("Drill_NoError").NewValue.ToBool();
            var isHaltOnCnc84 = properties.Property("Drill_Halt").NewValue.ToBool();
            var isParkingPositionOnCnc84 = properties.Property("Drill_ParkingPosition").NewValue.ToBool();
            var isExistBoardOnCnc84 = properties.Property("Drill_ExistBoard").NewValue.ToBool();
            var isFrontMushroomOpenOnCnc84 = properties.Property("Drill_FrontMushroom").NewValue.ToBool();
            var isMiddleMushroomOpenOnCnc84 = properties.Property("Drill_MiddleMushroom").NewValue.ToBool();
            var isDrillHoleEndOnCnc84 = properties.Property("Drill_DrillHoleEnd").NewValue.ToBool();

            bool subflag = isPowerOnBuffer
            && isAutomaticOnBuffer
            && isNoErrorOnBuffer
            && isNoErrorOnCnc84
            && !isHaltOnCnc84
            && isParkingPositionOnCnc84
            && !isExistBoardOnCnc84
            && !isFrontMushroomOpenOnCnc84
            && isDrillHoleEndOnCnc84;
            if (InteractingDevice.middleMushroomExist)
            {
                return subflag && !isMiddleMushroomOpenOnCnc84;
            }
            return subflag;
        }

        public List<string> SetReadyProperties()
        {
            return new List<string>()
            {
                "Buffer_EnergizeStatus",
                "Buffer_Automatic",
                "Buffer_NoError",
                "Drill_NoError",
                "Drill_Halt",
                "Drill_ParkingPosition",
                "Drill_ExistBoard",
                "Drill_FrontMushroom",
                "Drill_MiddleMushroom",
                "Drill_DrillHoleEnd"
            };
        }

        public List<string> SetWorkProperties()
        {
            return new List<string>()
            {
                "Buffer_Automatic",
                "Buffer_NoError",
                "Drill_NoError",
                "Drill_Halt",
                "Drill_ExistBoard",
                "Buffer_Position",
                "Drill_FrontMushroom",
                "Drill_MiddleMushroom",
                "Drill_DrillHoleEnd",
                "Drill_PressBoardEnd"
            };
        }

        public List<string> SetExceptionProperties()
        {
            return new List<string>()
            {
                "Buffer_Automatic",
                "Buffer_NoError",
                "Drill_NoError",
                "Drill_Halt"
            };
        }
    }
}
