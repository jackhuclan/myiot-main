using System.IO.Ports;

namespace VgDeviceGateway.Devices.Drill.Other
{
    public class PutUpExtend
    {
        private static SerialPort port = new SerialPort();
        private static AutoResetEvent _resetEvent = new AutoResetEvent(false);

        #region cmd byte

        private static readonly byte[] OpenCmd = new byte[] { 0x01, 0x01, 0x01, 0x01 };
        private static readonly byte[] CloseCmd = new byte[] { 0x01, 0x01, 0x01, 0x00 };

        #endregion cmd byte

        private static int _readFlag = 0;
        private static int _readResult = 0;
        private static AutoResetEvent _readEvent = new AutoResetEvent(false);

        public static bool Init(string portName)
        {
            try
            {
                port.PortName = portName;
                port.BaudRate = 9600;
                port.StopBits = StopBits.One;
                port.Parity = Parity.None;
                port.DataReceived += Port_DataReceived;
                if (port.IsOpen)
                {
                    port.Close();
                    Task.Delay(500).GetAwaiter().GetResult();
                }
                port.Open();
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                _resetEvent.Reset();
            }
            return false;
        }

        public static bool ConnectStatus()
        {
            return port.IsOpen;
        }

        public static bool GetSiteStatue(int siteNo)
        {
            var cmd = new PutUpCmdModel()
            {
                SiteNo = siteNo,
                Opr = 0
            };
            _readFlag = 1;
            if (cmd.TryGetBytes(out byte[] cmdBytes))
            {
                _resetEvent.WaitOne(1000);
                _readResult = 0;
                _readEvent.Reset();
                port.Write(cmdBytes, 0, cmdBytes.Length);
                if (_readEvent.WaitOne(1000))
                {
                    _resetEvent.Set();
                    return _readResult == 2;
                }
                _resetEvent.Set();
            }
            return false;
        }

        public static async Task ControlSite(int siteNo, int actionCmd)
        {
            var cmd = new PutUpCmdModel()
            {
                SiteNo = siteNo,
                ActionCmd = actionCmd,
                Opr = 1
            };

            if (cmd.TryGetBytes(out byte[] cmdBytes))
            {
                _resetEvent.WaitOne(1000);
                _readResult = 0;
                _readEvent.Reset();
                port.Write(cmdBytes, 0, cmdBytes.Length);
                await Task.Delay(100);
                _resetEvent.Set();
            }
        }

        private static void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var response = new byte[port.ReadBufferSize];
            port.Read(response, 0, port.ReadBufferSize);

            if (_readFlag == 1)
            {
                int flag = 0;
                for (var i = 0; i < 4; i++)
                {
                    if (flag != 1 && response[i] != OpenCmd[i])
                    {
                        flag += 1;
                    }
                    if (flag != 2 && response[i] != CloseCmd[i])
                    {
                        flag += 2;
                    }

                    if (flag == 3)
                    {
                        break;
                    }
                }

                _readResult = flag;
                _readEvent.Set();
            }
        }
    }
}
