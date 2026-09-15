using System.IO.Ports;
using Microsoft.Extensions.Logging;
using Modbus.Device;

namespace VgDeviceGateway.Devices.Drill.Other
{
    public class FrontIp20
    {
        private static readonly object _lockObj = new object();
        private static ILogger<FrontIp20> logger;
        private static SerialPort serialPort = new SerialPort();
        private static IModbusMaster _modbus;
        private const int _retryTime = 3; // 重试次数
        private const int _overTime = 500; // 超时时间ms

        public static bool IsConnected
        {
            get
            {
                return serialPort.IsOpen;
            }
        }

        public static void Init(string portName, ILogger<FrontIp20> loggerInt)
        {
            serialPort.PortName = portName;
            serialPort.DataBits = 8;
            serialPort.BaudRate = 9600;//设置波特率
            serialPort.StopBits = StopBits.One;
            serialPort.Parity = Parity.Even;
            logger = loggerInt;
        }

        /// <summary>
        /// 打开串口
        /// </summary>
        /// <returns></returns>
        public static bool OpenSP()
        {
            try
            {
                if (serialPort != null && serialPort.IsOpen)
                {
                    serialPort.Close();
                }
                _modbus = ModbusSerialMaster.CreateRtu(serialPort);

                //打开串口
                if (!serialPort.IsOpen) serialPort.Open();
                return serialPort.IsOpen;
            }
            catch (Exception ex)
            {
                logger.LogError("连接失败" + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 关闭串口
        /// </summary>
        /// <returns></returns>
        public static bool CloseSP()
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
                logger.LogError("关闭失败" + ex.Message);
                return false;
            }
        }

        public static bool ReadOutputCoils(byte slaveID, ushort address, ushort length, out bool[]? data)
        {
            return Read(slaveID, address, length, out data, (slave, add, leng) =>
            {
                return _modbus.ReadCoils(slave, add, leng);
            });
        }

        public static bool ReadInputCoils(byte slaveID, ushort address, ushort length, out bool[]? data)
        {
            return Read(slaveID, address, length, out data, (slave, add, leng) =>
            {
                return _modbus.ReadInputs(slave, add, leng);
            });
        }

        public static bool ReadHoldRegister(byte slaveID, ushort address, ushort length, out ushort[]? data)
        {
            return Read<ushort>(slaveID, address, length, out data, (slave, add, leng) =>
            {
                return _modbus.ReadHoldingRegisters(slave, add, leng);
            });
        }

        public static bool ReadInputRegister(byte slaveID, ushort address, ushort length, out ushort[]? data)
        {
            return Read<ushort>(slaveID, address, length, out data, (slave, add, leng) =>
            {
                return _modbus.ReadInputRegisters(slave, add, leng);
            });
        }

        private static bool Read<T>(byte slaveID, ushort address, ushort length, out T[]? data, Func<byte, ushort, ushort, T[]> func)
        {
            data = default;
            if (!serialPort.IsOpen)
                return false;

            try
            {
                lock (_lockObj)
                {
                    for (int count = 0; count < _retryTime; count++)
                    {
                        try
                        {
                            data = func.Invoke(slaveID, address, length);

                            if (data != null && data.Length > 0)
                                return true;
                        }
                        catch (Exception ee)
                        {
                            logger.LogError(ee.Message);
                        }
                    }
                    logger.LogError($"读取线圈失败 address:{address}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"读取线圈失败 {ex.Message}");
                return false;
            }
        }

        public static bool WriteRegisters(byte slaveID, ushort address, ushort[] data)
        {
            if (data == null)
            {
                return false;
            }
            return Write<ushort>(slaveID, address, data, (slave, add, da) =>
            {
                _modbus.WriteMultipleRegisters(slave, add, da);
            });
        }

        public static bool WriteCoils(byte slaveID, ushort address, bool[] data)
        {
            if (data == null)
            {
                return false;
            }
            return Write<bool>(slaveID, address, data, (slave, add, da) =>
            {
                _modbus.WriteMultipleCoils(slave, add, da);
            });
        }

        private static bool Write<T>(byte slaveID, ushort address, T[] data, Action<byte, ushort, T[]> action)
        {
            if (!serialPort.IsOpen)
                return false;

            try
            {
                lock (_lockObj)
                {
                    for (int count = 0; count < _retryTime; count++)
                    {
                        try
                        {
                            action.Invoke(slaveID, address, data);
                            return true;
                        }
                        catch (Exception ee)
                        {
                            logger.LogError(ee.Message);
                        }
                    }
                    logger.LogError("写动作错误");
                    return false;
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"写动作失败 {ex.Message}");
                return false;
            }
        }
    }
}
