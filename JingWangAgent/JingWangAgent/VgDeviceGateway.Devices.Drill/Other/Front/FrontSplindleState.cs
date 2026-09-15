namespace VgDeviceGateway.Devices.Drill.Other.Front
{
    public class FrontSplindleState
    {
        public bool BigAirState { get; set; }
        public bool SmallAirState { get; set; }
        public bool UpDownState { get; set; }
        public bool PanelState { get; set; }
    }

    public class AllMachineSplindleStates
    {
        public FrontSplindleState[] frontSplindleStates = new FrontSplindleState[6];
        public bool DoorState { get; set; }
    }
}
