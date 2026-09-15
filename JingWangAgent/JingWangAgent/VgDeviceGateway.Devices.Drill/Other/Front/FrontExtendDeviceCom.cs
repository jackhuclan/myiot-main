using System.IO.Ports;
using Microsoft.Extensions.Logging;
using Modbus.Device;

namespace VgDeviceGateway.Devices.Drill.Other.Front
{
    public class FrontExtendDeviceCom
    {
        #region fields

        private readonly object _lockObj = new object();
        private readonly ILogger<FrontExtendDeviceCom> logger;
        private ModbusSerialMaster _modbus;
        private const int _retryTime = 3; // 重试次数
        private const int _overTime = 500; // 超时时间ms
        private SerialPort serialPort = new SerialPort();

        #endregion fields

        public IModbusMaster ModBus
        {
            get
            {
                return _modbus ?? null;
            }
        }

        public bool IsConnected
        {
            get
            {
                return serialPort.IsOpen;
            }
        }

        #region Functions

        public FrontExtendDeviceCom(ILogger<FrontExtendDeviceCom> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// 打开串口
        /// </summary>
        /// <returns></returns>
        public bool OpenSP(string portName)
        {
            try
            {
                serialPort.PortName = portName;
                serialPort.BaudRate = 9600;
                serialPort.Parity = Parity.Even;
                serialPort.DataBits = 8;
                serialPort.StopBits = StopBits.One;

                if (serialPort.IsOpen)
                {
                    serialPort.Close();
                }
                Thread.Sleep(10);
                serialPort.Open();
                _modbus = ModbusSerialMaster.CreateRtu(serialPort);

                return serialPort.IsOpen;
            }
            catch (Exception ex)
            {
                logger.LogError("前上料外置设备连接失败" + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 关闭串口
        /// </summary>
        /// <returns></returns>
        public bool CloseSP()
        {
            if (!serialPort.IsOpen)
                return true;

            try
            {
                serialPort.Close();
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError("前上料外置设备关闭失败" + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 读线圈 读数据
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        //
        public (bool, bool[]) ReadInputs(byte slaveID, string address, int num, bool inputFlag = true)
        {
            if (!serialPort.IsOpen)
                return (false, default);

            try
            {
                lock (_lockObj)
                {
                    for (int count = 0; count < _retryTime; count++)
                    {
                        try
                        {
                            bool[] data = inputFlag ? this._modbus.ReadInputs(slaveID, ushort.Parse(address), (ushort)num) : this._modbus.ReadCoils(slaveID, ushort.Parse(address), (ushort)num);

                            if (data != null)
                                return (true, data);
                        }
                        catch (Exception ee)
                        {
                            logger.LogError(ee.Message);
                        }
                    }
                    logger.LogError("外置设备 read Info Failed!" + string.Format($"address:{address}"));
                    return (false, default);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("外置设备 read Info Failed!" + ex.Message);
                return (false, default);
            }
        }

        /// <summary>
        /// 写多个线圈指令
        /// </summary>
        /// <param name="address"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool WriteCoilInfo(byte slaveID, string address, bool flag)
        {
            if (!serialPort.IsOpen)
                return false;

            try
            {
                lock (_lockObj)
                {
                    for (int i = 0; i < _retryTime; i++)
                    {
                        try
                        {
                            this._modbus.WriteSingleCoil(slaveID, ushort.Parse(address), flag);
                            return true;
                        }
                        catch (Exception ee)
                        {
                            logger.LogError(ee.Message);
                        }
                    }
                    logger.LogError("外置设备 write info failed. " + string.Format($"address:{address}, message:{flag}"));
                    return false;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("外置设备 write info failed" + ex.Message);
                return false;
            }
        }

        #endregion Functions
    }
}
