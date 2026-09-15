using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using NModbus;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Configuration;
using VgAutoDrill.Infrastructure;
using VgDeviceGateway.Devices.Shelf;
using static VgDeviceGateway.Devices.Common.Agv.Hik.HikCarStatus;

namespace VegaIot.External.Hik;

public class HikTransfer : SiloShelf
{
    private readonly ILogger<HikTransfer> _logger;
    private readonly CentralTransferJobProcessor _jobProcessor;
    private volatile bool _isBusy = false;
    private readonly HikHandler _hikHandler;

    public HikTransfer(DeviceDescriptor DeviceDescriptor,
        IServiceProvider serviceProvider,
        IDeviceEngine deviceEngine,
        IObjectFactory objectFactory)
        : base(DeviceDescriptor, serviceProvider, deviceEngine)
    {
        _logger = LoggerFactory.CreateLogger<HikTransfer>();
        _jobProcessor = ObjectFactory.GetOrCreate<CentralTransferJobProcessor>(this);
        _hikHandler = ObjectFactory.GetOrCreate<HikHandler>(this);

        PeriodicTimers["20s"]!.OnTick += async () =>
        {
            if (!_isBusy)
            {
                try
                {
                    _isBusy = true;
                    await _jobProcessor.ProcessCentralJob();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }
                finally
                {
                    _isBusy = false;
                }
            }
        };
    }

    protected override void CreateShelfHandler()
    {
        PropertyContainer.AddHandler<HikPropertyHandler, HikTransfer>(this);
        AlarmContainer.AddHandler<HikAlarmHandler, HikTransfer>(this);
        EventContainer.AddHandler<HikEventHandler, HikTransfer>(this);
        StateContainer.AddHandler<HikStateHandler, HikTransfer>(this);
        AGVToShelfSiloShelfLoadPolicy = ObjectFactory.CreateObject<AGVToSiloShelf_LoadMaterial_SiloShelf_InteractionPolicy>(this);
        AGVToShelfSiloShelfUnloadPolicy = ObjectFactory.CreateObject<AGVToSiloShelf_UnloadMaterial_SiloShelf_InteractionPolicy>(this);
    }

    /// <summary>
    /// 查询任务状态
    /// </summary>
    /// <param name="task"></param>
    /// <returns></returns>
    public async Task<string> QueryTaskStatus(string taskCode)
    {
        try
        {
            var res = await _hikHandler.QueryTaskStatus(taskCode, "Hik", "TaskStatus");//TaskStatus
            return res;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return string.Empty;
        }
    }

    public string GetTaskStatus(string taskStatus)
    {
        //1-已创建，2-正在执行，5 - 取消完成，9 - 已结束, 10 - 被打断
        string msg = string.Empty;
        switch (taskStatus)
        {
            case "1":
                msg = "已创建";
                break;

            case "2":
                msg = "正在执行";
                break;

            case "5":
                msg = "取消完成";
                break;

            case "9":
                msg = "已结束";
                break;

            case "10":
                //msg = "已结束";
                msg = "被打断";
                break;

            default:
                msg = "未查询到任务状态";
                break;
        }
        return msg;
    }

    public async Task<string> bindLineSideStock(BindLineSideStock bindLineSideStock)
    {
        return await _hikHandler.UnbindLineSideStock(bindLineSideStock);
    }

    public void setNewPanelLog(PanelList panels, string shelfName)
    {
        if (!string.IsNullOrEmpty(panels[0].SiloCode))
        {
            _logger.LogWarning($"=======中转位{shelfName}的料仓{panels[0].SiloCode}的物料信息被维护位{JsonSerializer.Serialize(panels)}=============");
        }
        else
        {
            _logger.LogWarning($"=======中转位{shelfName}的物料信息被维护位: 料仓不存在 =============");
        }
    }

    //测试传感器
    public bool SelectSensor(int position)
    {
        bool res = false;
        try
        {
            //int index = (int)Math.Floor(( (position-1) / 2).ToFloat());//两个插齿位的传感器共用一个IP
            //string IP = sensorIp[index];
            //TcpClient client = new TcpClient(IP, 502);//port 固定
            //var factory = new ModbusFactory();
            //NModbus.IModbusMaster master = factory.CreateMaster(client);
            //byte slaveId = 1;
            //ushort startAddress = 0;
            //bool[] result = master.ReadCoils(slaveId, startAddress, 2);
            //if (position % 2 == 0)
            //{
            //    res = result[0];
            //    logger.LogDebug($"{position}号插齿检测是否有料仓：{result[1]}");
            //}
            //else
            //{
            //    res = result[1];
            //    logger.LogDebug($"{position}号插齿检测是否有料仓：{result[0]}");
            //}

            //目前只有一个IP可用
            TcpClient client = new TcpClient("10.50.32.140", 502);//port 固定
            var factory = new ModbusFactory();
            IModbusMaster master = factory.CreateMaster(client);
            byte slaveId = 1;
            ushort startAddress = 0;
            bool[] result = master.ReadCoils(slaveId, startAddress, 2);
            res = false;
            if (position % 2 == 0)
            {
                res = result[0];
            }
            else
            {
                res = result[1];
            }
            _logger.LogDebug($"{position}号插齿检测是否有料仓：{res}");
            return res;
        }
        catch (Exception ex)
        {
            _logger.LogDebug($"{position}号插齿检测是否有料仓：{ex.Message}");
            _logger.LogError($"{position}号插齿检测是否有料仓：{ex.Message}");
            res = false;
            return res;
        }
    }

    public string[] Camera(string IP, int port, string postion)
    {
        string[] resLst = [];
        NetworkStream? stream = null;
        try
        {
            IPAddress ip = IPAddress.Parse(IP);
            var client = new TcpClient(ip.ToString(), port); // 替换hostname和端口号
            stream = client.GetStream();

            if (stream.CanWrite)
            {
                string message = "+"; // 触发拍照
                byte[] data = Encoding.UTF8.GetBytes(message);
                stream.Write(data, 0, data.Length);
            }

            // 读取响应
            if (stream.CanRead)
            {
                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                string recMsg = response;//返回结果：0没有，1有， ZJ13_0;ZJ14_0;ZJ15_0;ZJ16_0;ZJ17_0;ZJ18_0;
                if (string.IsNullOrEmpty(recMsg))
                {
                    _logger.LogDebug($"中转位{postion}触发拍照结果为空");
                    _logger.LogError($"中转位{postion}触发拍照结果为空");
                }
                else
                {
                    if (recMsg.Contains(';'))
                    {
                        resLst = recMsg.Substring(0, recMsg.Length - 1).Split(';');
                    }
                    else
                    {
                        resLst = [recMsg];
                    }
                }
            }
            if (stream != null)
            {
                stream.Close();
            }
        }
        catch (Exception ex)
        {
            _logger.LogDebug($"中转位{postion}触发拍照结果异常：{ex.Message}");
            _logger.LogError($"中转位{postion}触发拍照结果异常：{ex.Message}");
            if (stream != null)
            {
                stream.Close();
            }
            resLst = ["0"];
        }

        return resLst;
    }
}
