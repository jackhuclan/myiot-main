using VgAutoDrill.Fundation.Iot.Models;

namespace VgDeviceGateway.Devices.Common;

public class CurrentContext
{
    private volatile int _rackIndex = 0;
    private volatile bool _isUpdatingPanels = false;
    private volatile string _panelCode = "";
    private volatile string _barcode= "";
    public string ItemCode { get; set; } = "";
    public string Message { get; set; } = "";
    public int StartShelfNum { get; set; } = 0;
    public int AddSiloCount { get; set; } = 1;
    public string SiloCode { get; set; } = "";

    public string SiloMatch { get; set; } = "";
    public int ShelfIndex { get; set; } = 0;
    public string LocationCode { get; set; } = "";
    public int StartLayer { get; set; } = 0;
    public int EndLayer { get; set; } = 1;
    public int PanelType { get; set; } = 1;//0空  1生料 2熟料
    public int RackIndex { get => _rackIndex; set => _rackIndex = value; }
    public bool IsExistSilo { get; set; }// 存在
    public int ItemOrder { get; set; } = 2;//2-按创建时间倒序、3-按物料代码正序、4-按物料代码倒序 QueryOrderByEnum.OrderByCreateTimeDesc
    public bool isReady { get; set; } = false;
    public string DeviceId { get; set; } = "";
    public string DeviceName { get; set; } = "";

    /// <summary>
    /// 装载到料仓架的Panel数量
    /// </summary>
    public int RawPanelCount { get; set; } = 18;

    public float PanelWidth { get; set; } = 622f;
    public float PanelLength { get; set; } = 820f;
    public string Barcode { get => _barcode; set => _barcode = value; }
    public  string PanelCode { get => _panelCode; set => _panelCode = value; }
    public float PinOffset { get; set; } = 3.2f;
    public string PlcAddr { get; set; }
    public int PlcHandleType { get; set; }
    public string PlcLength { get; set; }
    public string PlcValue { get; set; }
    public int PanelTypeWhenExistNoSilo { get; set; }
    public string AGVCode { get; set; } = string.Empty;
    public string MaterialMatchCode { get; set; }
    public int operationType { get; set; }//操作类型
    public string Region { get; set; } = string.Empty; //线边仓区域
    public bool TransferLocationIsExistSilo { get; set; }   // 中转位是否存在料仓
    public string HikTaskCode { get; set; } = string.Empty;// 小车任务号
    public string? InternalLotNo { get; set; } //内部lot
    public string? ExternalLotNo { get; set; }//内部lot

    /// <summary>
    /// 物料工艺分组
    /// </summary>
    public string? SpecGroup { get; set; }//物料工艺分组

    public int SumPcs { get; set; } = 0;//料框总的物料片数

    public long? TransferId { get; set; }//l料仓任务ID

    //海康任务请求参数
    public DeviceServiceInvokeRequest HikTaskRequest { get; set; } = new DeviceServiceInvokeRequest();

    /// <summary>
    /// 每层片数
    /// </summary>
    public int Pcs { get; set; } = 1;
    /// <summary>
    /// 料仓是转入（in）还是转出（out）
    /// </summary>
    public string Direction { get; set; } = "in";
    /// <summary>
    /// 料仓任务的code
    /// </summary>
    public string? TransferJobCode { get; set; }

    /// <summary>
    /// 接收到海康物料，正在更新板料
    /// </summary>
    public bool IsUpdatingPanels { get => _isUpdatingPanels; set => _isUpdatingPanels = value; }

    public void Reset()
    {
        TransferId = 0;
        HikTaskCode = string.Empty;
        InternalLotNo = string.Empty;
        ExternalLotNo = string.Empty;
        operationType = 0;
        Direction = string.Empty;
        MaterialMatchCode = string.Empty;
        Region = string.Empty;
        AGVCode = string.Empty;
        ItemCode = string.Empty;
        SiloCode = string.Empty;
        StartLayer = 0;
        SpecGroup = string.Empty;
        SumPcs = 0;
        Pcs = 0;
        TransferJobCode = "";
        IsUpdatingPanels = false;
        HikTaskRequest.Params.Clear();
    }
}

public class PostionAndDevice
{
    public string pos { get; set; }
    public string Device { get; set; }
    public string DeviceId { get; set; }
    public string ProductId { get; set; }
    public string ClientId { get; set; }
}
