using Microsoft.Extensions.Logging;

namespace VgDeviceGateway.Devices.Drill.Other.Front
{
    public class FrontExtendDevice
    {
        private const int READSTARTPOSITION = 200;
        private const int SPINDLESTATENUM = 4;
        private string comStr;

        public FrontExtendDevice(ILoggerFactory loggerFactory, ILogger<FrontExtendDevice> logger)
        {
            this.loggerFactory = loggerFactory;
            this.logger = logger;
            frontExtendDeviceCom = new FrontExtendDeviceCom(loggerFactory.CreateLogger<FrontExtendDeviceCom>());
        }

        public FrontExtendDeviceCom frontExtendDeviceCom;
        private readonly ILoggerFactory loggerFactory;
        private readonly ILogger<FrontExtendDevice> logger;

        public bool IsConnected
        {
            get
            {
                return frontExtendDeviceCom.IsConnected;
            }
        }

        public bool OpenDevice(string portName)
        {
            comStr = portName;
            return frontExtendDeviceCom.OpenSP(portName);
        }

        public bool CloseDevice()
        {
            if (!string.IsNullOrEmpty(comStr))
            {
                return frontExtendDeviceCom.OpenSP(comStr);
            }
            return false;
        }

        public AllMachineSplindleStates ReadCoilsAll()
        {
            (bool, bool[]) states = frontExtendDeviceCom.ReadInputs(1, READSTARTPOSITION.ToString(), 25);
            if (states.Item1)
            {
                AllMachineSplindleStates allMachineSplindleStates = new AllMachineSplindleStates();
                for (int i = 0; i < 24; i += SPINDLESTATENUM)
                {
                    var subFlags = states.Item2.Skip(i).Take(4).ToArray();
                    int splineIndex = 6 - i / SPINDLESTATENUM - 1;
                    allMachineSplindleStates.frontSplindleStates[splineIndex] = new FrontSplindleState()
                    {
                        BigAirState = subFlags[0],
                        SmallAirState = subFlags[1],
                        UpDownState = subFlags[2],
                        PanelState = subFlags[3]
                    };
                }
                allMachineSplindleStates.DoorState = states.Item2[24];
                return allMachineSplindleStates;
            }

            return default;
        }

        public FrontSplindleState ReadSingleSplineInf(int splineNum)
        {
            int startPosition = (6 - splineNum) * SPINDLESTATENUM;
            (bool, bool[]) splineStates = frontExtendDeviceCom.ReadInputs(1, "200", 24);
            if (splineStates.Item1)
            {
                var subSpindle = splineStates.Item2.Skip((int)startPosition).Take(4).ToArray();

                return new FrontSplindleState()
                {
                    BigAirState = subSpindle[0],
                    SmallAirState = subSpindle[1],
                    UpDownState = subSpindle[2],
                    PanelState = subSpindle[3]
                };
            }
            return default;
        }

        public bool ControlMushroomInOut(bool inFlag)
        {
            return inFlag ? ControlMushroomIn() : ControlMushroomOut();
        }

        private bool ControlMushroomOut()
        {
            if (MushroomIsOnOutPosition(out bool[] controlFlags))
            {
                return true;
            }
            else
            {
                frontExtendDeviceCom.WriteCoilInfo(1, "134", true);
                Thread.Sleep(20);
                frontExtendDeviceCom.WriteCoilInfo(1, "132", false);
                Thread.Sleep(20);
                frontExtendDeviceCom.WriteCoilInfo(1, "134", false);
                Thread.Sleep(20);

                return MushroomIsOnOutPosition(out bool[] _);
            }
        }

        private bool ControlMushroomIn()
        {
            if (MushroomIsOnInPosition(out bool[] controlFlags))
            {
                return true;
            }
            else
            {
                frontExtendDeviceCom.WriteCoilInfo(1, "134", true);
                Thread.Sleep(20);
                frontExtendDeviceCom.WriteCoilInfo(1, "132", true);
                Thread.Sleep(20);
                frontExtendDeviceCom.WriteCoilInfo(1, "134", false);
                Thread.Sleep(20);

                return MushroomIsOnInPosition(out bool[] _);
            }
        }

        public bool MushroomState(out bool inFlag, out bool outFlag)
        {
            inFlag = false;
            outFlag = false;
            var data = frontExtendDeviceCom.ReadInputs(1, "132", 3, false);
            if (!data.Item1)
            {
                return false;
            }
            inFlag = data.Item2[0] && !data.Item2[2];
            outFlag = !data.Item2[0] && !data.Item2[2];
            return true;
        }

        private bool MushroomIsOnInPosition(out bool[] controlFlags)
        {
            controlFlags = new bool[3];
            var data = frontExtendDeviceCom.ReadInputs(1, "132", 3, false);
            if (!data.Item1)
            {
                return false;
            }
            controlFlags = data.Item2;
            return data.Item2[0] && !data.Item2[2];
        }

        private bool MushroomIsOnOutPosition(out bool[] controlFlags)
        {
            controlFlags = new bool[3];
            var data = frontExtendDeviceCom.ReadInputs(1, "132", 3, false);
            if (!data.Item1)
            {
                return false;
            }
            controlFlags = data.Item2;
            return !data.Item2[0] && !data.Item2[2];
        }

        public bool ControlUpDown(bool upFlag)
        {
            var result = upFlag ? UpDownIsAllOnUpPosition() : UpDownIsAllOnDownPosition();
            if (result)
            {
                return true;
            }
            frontExtendDeviceCom.WriteCoilInfo(1, "133", upFlag);
            Thread.Sleep(1000);
            result = upFlag ? UpDownIsAllOnUpPosition() : UpDownIsAllOnDownPosition();

            return result;
        }

        public bool UpDownIsAllOnUpPosition()
        {
            (bool, bool[]) states = frontExtendDeviceCom.ReadInputs(1, READSTARTPOSITION.ToString(), 24);
            if (states.Item1)
            {
                return states.Item2.Where((s, i) => i % 4 == 2).All(s => s);
            }
            return false;
        }

        public bool UpDownIsAllOnDownPosition()
        {
            (bool, bool[]) states = frontExtendDeviceCom.ReadInputs(1, READSTARTPOSITION.ToString(), 24);
            if (states.Item1)
            {
                return states.Item2.Where((s, i) => i % 4 == 2).All(s => !s);
            }
            return false;
        }

        public bool DoorIsOpen()
        {
            (bool, bool[]) doorState = frontExtendDeviceCom.ReadInputs(1, "200", 25);
            if (doorState.Item1)
            {
                return doorState.Item2[doorState.Item2.Length - 1];
            }
            return false;
        }

        public string GetPanelStateString()
        {
            (bool, bool[]) states = frontExtendDeviceCom.ReadInputs(1, READSTARTPOSITION.ToString(), 24);
            if (states.Item1)
            {
                return string.Join("", states.Item2.Where((s, i) => i % 4 == 3).Select(s => s ? "1" : "0").ToArray().Reverse());
            }
            return string.Empty;
        }
    }
}
