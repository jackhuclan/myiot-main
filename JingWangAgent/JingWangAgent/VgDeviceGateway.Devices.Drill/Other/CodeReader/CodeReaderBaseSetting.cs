// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Text;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Fundation.Utils;

namespace VgDeviceGateway.Devices.Drill.Other.CodeReader
{
    public class CodeReaderBaseSetting
    {
        private static TcpIpClient[] tcpIpClients = new TcpIpClient[6];
        public static VegaBordInfo vegaBordInfo { get; set; }
        public static CodeFileInfo codeFileInfo { get; set; }
        public Dictionary<string, string> receiveData = new Dictionary<string, string>();
        public static string[] receiveDataInf;
        public Dictionary<int, string> codeReader = new Dictionary<int, string>();
        public static string receiveFLag;
        public readonly int scanCount;
        public string[] receiveQr;
        private readonly ILogger<CodeReaderBaseSetting> logger;
        private readonly DefaultDrill device;
        public readonly byte slaveID;
        public static long triggerNum = 0;
        public static long successNum = 0;
        public static DateTime CodeReaderStartTime = DateTime.Now;
        public static Action<int, string> CodeReaderReceiveAction = null;
        public static Action<int> CodeReaderTriggerBeforeAction = null;
        public static bool scanFlag = false;
        public bool existPlc = false;

        public CodeReaderBaseSetting(ILogger<CodeReaderBaseSetting> logger, IServiceProvider serviceProvider, DefaultDrill device)
        {
            scanCount = device.spindleNum;
            receiveQr = new string[scanCount];
            receiveDataInf = new string[scanCount];
            this.device = device;
            existPlc = device.DeviceDescriptor.Extra.ContainsKey("ExistPlc") ? device.DeviceDescriptor.Extra["ExistPlc"].ToBool() : true;
            slaveID = (byte)device.DeviceDescriptor.Extra["SlaveID"].ToInt();

            this.logger = logger;
        }

        public bool StartScanConnect()
        {
            for (int i = 0; i < scanCount; i++)
            {
                //Spline1CodeReaderPort
                string scanIp = device.DeviceDescriptor.Extra[$"Spline{i + 1}CodeReaderIP"].ToStr();
                int scanPort = device.DeviceDescriptor.Extra[$"Spline{i + 1}CodeReaderPort"].ToInt();
                TcpIpClient tmp = new TcpIpClient();
                // tmp.Disconnect();
                tmp.Connect(scanIp, scanPort);
                if (tmp != null)
                {
                    tmp.DataReceived += MyDataReceived;
                    tcpIpClients[i] = tmp;
                }
                else
                {
                    tcpIpClients = new TcpIpClient[6];
                    int tmp_log = 51 + i;
                    if (existPlc) WriteWarningToPlc((ushort)tmp_log);
                    logger.LogError($"读码器{i + 1}未配置");
                    scanFlag = false;
                    return false;
                }
            }
            receiveFLag = new String('0', scanCount);
            scanFlag = true;
            return true;
        }

        //钻机扫码枪 回调函数
        public static string oldMessage = "";
        public void MyDataReceived(object TcpIpClient, Message message)
        {
            logger.LogInformation($"读码器  回调的原始数据 ：{message.MessageString}");
            //if (oldMessage.Equals(message.MessageString,StringComparison.OrdinalIgnoreCase))
            //{
            //    return;
            //}
            oldMessage = message.MessageString;
            string[] receive = message.MessageString.Split(':', StringSplitOptions.RemoveEmptyEntries);
            int index = Convert.ToInt32(receive[0]);
            if (receive.Count() == 2 && !string.IsNullOrWhiteSpace(receive[1]))
            {
                successNum++;
                string receive_data = receive[1].Replace("\0", "");
                receiveDataInf[index] = receive_data;
                receiveData.Add(DateTime.Now.ToFileTimeUtc().ToString() + index.ToString(), receive_data);
                Thread.Sleep(100);
                if (existPlc) WriteCodeInfoToPlc(index, receive_data);
                CodeReaderReceiveAction?.Invoke(index, receive_data);
                logger.LogInformation("发送数据个数" + receiveData.Count());
            }
        }

        public void SendScanMesssage(string board_position_binary_data)
        {
            //清空上一次板子信息
            receiveData.Clear();
            //清空上一次的钻带程序文件
            codeFileInfo = new CodeFileInfo()
            {
                Code = "",
                VegaFileInfo = new VegaFileInfo()
                {
                    DiaFilePath = "",
                    DrilFilePath = "",
                }
            };
            //解析板子信息
            for (int i = 0; i < board_position_binary_data.Length; i++)
            {
                if (board_position_binary_data[i] == '1')
                {
                    codeReader.Add(i, "1");
                }
            }
            vegaBordInfo = new VegaBordInfo()
            {
                num = codeReader.Count(),
                BordMark = board_position_binary_data
            };
            //将receiveFLag 轮流触发读码器
            for (int i = 0; i < board_position_binary_data.Length; i++)
            {
                if (board_position_binary_data[i] == '1')
                {
                    //LogManagers.log.Info("开始触发读码器");
                    tcpIpClients[i].Write("1");
                    Thread.Sleep(100);
                }
            }
        }

        private void WriteWarningToPlc(ushort code)
        {
            try
            {
                device.modbusIpMaster?.WriteSingleRegister(slaveID, device.DeviceDescriptor.Extra["WarningInfoWriteOnPlc"].ToUshort(), code);
            }
            catch (Exception ee)
            {
                logger.LogError($"写错误异常:{ee.Message}");
            }
        }

        private void WriteCodeInfoToPlc(int index, string code)
        {
            try
            {
                // ushort[] result = code.Select(c => (ushort)c).ToArray();
                ushort[] result = ConvertHelper.GetUShortArrayFromByteArray(Encoding.UTF8.GetBytes(code), DataFormat.BADC);
                device.modbusIpMaster.WriteSingleRegister(slaveID, device.DeviceDescriptor.Extra[$"Spline{index + 1}CodeReaderCodeNum"].ToUshort(), (ushort)result.Length);
                device.modbusIpMaster.WriteMultipleRegisters(slaveID, device.DeviceDescriptor.Extra[$"Spline{index + 1}CodeReaderCodeStartPosition"].ToUshort(), result);
            }
            catch (Exception ee)
            {
                logger.LogError($"写错误异常:{ee.Message}");
            }
        }

        public void SendScanMesssageToSignal(int index, string command, bool isFirst = false)
        {
            try
            {
                var CodeReaderTriggerIsM = device.DeviceDescriptor.Extra["CodeReaderTriggerIsM"].ToBool();
                logger.LogWarning($"{index}触发扫码:true,CodeReaderTriggerIsM ={CodeReaderTriggerIsM}");
                if (isFirst) triggerNum++;
                receiveDataInf[index] = string.Empty;
                if (command == "1")
                {
                    if (isFirst) CodeReaderTriggerBeforeAction?.Invoke(index);
                    tcpIpClients[index].Write("1");
                    Thread.Sleep(100);
                }
            }
            catch (Exception ee)
            {

                logger.LogError($"触发错误 {ee.Message}");
            }

        }
    }

    public class CodeFileInfo
    {
        public string Code { get; set; }
        private string preDrilFile;

        public string PreDrilFile
        {
            get { return preDrilFile; }
            set { preDrilFile = value; }
        }

        public VegaFileInfo VegaFileInfo { get; set; }
    }

    public class VegaFileInfo
    {
        public string DrilFilePath { get; set; }
        public string DiaFilePath { get; set; }
    }

    public class VegaBordInfo
    {
        public int num { get; set; }
        public string BordMark { get; set; }
    }
}
