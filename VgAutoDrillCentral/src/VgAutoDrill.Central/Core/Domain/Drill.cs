using Microsoft.Extensions.DependencyInjection;
using VgAutoDrill.Central.Core.Manager;
using VgAutoDrill.Central.Core.Mes;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Utils;

namespace VgAutoDrill.Central.Core.Domain;

public class Drill : DeviceProxy
{
    private readonly ILocationManager _locationManager;

    [ActivatorUtilitiesConstructor]
    public Drill(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _locationManager = serviceProvider.GetRequiredService<ILocationManager>();
    }

    /// <summary>
    /// 机器尺寸,如2530 2849，
    /// 默认空
    /// </summary>
    public string MachineSize
    {
        get
        {
            if (!this.Descriptor.Extra.ContainsKey("MachineSize"))
            {
                return string.Empty;
            }

            return this.Descriptor.Extra["MachineSize"].ToStr();
        }
    }

    /// <summary>
    /// 自动机器还是手动机器，
    /// 默认为自动机器，使用agv自动上下料；
    /// 手动机器, 人工上下料
    /// </summary>
    public bool IsAuto
    {
        get
        {
            if (!this.Descriptor.Extra.ContainsKey("IsAuto"))
            {
                return true;
            }

            return this.Descriptor.Extra["IsAuto"].ToBool();
        }
    }

    /// <summary>
    /// 是否是DUO型（AB轴）钻机
    /// 默认不是
    /// </summary>
    public bool IsDuo
    {
        get
        {
            if (!this.Descriptor.Extra.ContainsKey("IsDuo"))
            {
                return false;
            }

            return this.Descriptor.Extra["IsDuo"].ToBool();
        }
    }

    public override bool IsAvailbleForAgv
    {
        get
        {
            if (!this.Properties.ContainsKey("IsAvailbleForAgv"))
                return true;

            return this.Properties["IsAvailbleForAgv"].ToBool();
        }
    }

    public bool BufferAutomatic
    {
        get
        {
            if (!this.Properties.ContainsKey("Buffer_Automatic"))
            {
                return false;
            }

            return this.Properties["Buffer_Automatic"].ToBool();
        }
    }

    /// <summary>
    /// 所拥有的库位
    /// </summary>
    public Location? Location => _locationManager.Locations.FirstOrDefault(x => x.HostDevice?.DeviceId == this.DeviceId);

    public override bool SiloPlaceable => false;

    public InteractionSequence SuggestPanelInteractionSequence
    {
        get
        {
            if (Properties.ContainsKey("SuggestPanelInteractionSequence"))
            {
                return (InteractionSequence)Properties["SuggestPanelInteractionSequence"].ToInt();
            }
            return InteractionSequence.None;
        }
    }

    public int Percentage
    {
        get
        {
            return Properties != null && Properties.ContainsKey("Drill_Percentage") ? Properties["Drill_Percentage"].ToInt() : 0;
        }
    }

    public int CurrentOpentime
    {
        get
        {
            return Properties != null && Properties.ContainsKey("Drill_CurrentOpentime") ? Properties["Drill_CurrentOpentime"].ToInt() : 0;
        }
    }

    public int CurrentWorktime
    {
        get
        {
            return Properties != null && Properties.ContainsKey("Drill_CurrentWorktime") ? Properties["Drill_CurrentWorktime"].ToInt() : 0;
        }
    }

    public int CurrentWaittime
    {
        get
        {
            return Properties != null && Properties.ContainsKey("Drill_CurrentWaittime") ? Properties["Drill_CurrentWaittime"].ToInt() : 0;
        }
    }

    public int CurrentErrortime
    {
        get
        {
            return Properties != null && Properties.ContainsKey("Drill_CurrentErrortime") ? Properties["Drill_CurrentErrortime"].ToInt() : 0;
        }
    }

    /// <summary>
    /// 班次开始时间
    /// </summary>
    public DateTime? ShiftsStartTime
    {
        get
        {
            return (Properties != null && Properties.ContainsKey("Drill_ShiftsStartTime") && !string.IsNullOrEmpty(Properties["Drill_ShiftsStartTime"].ToStr()))
                ? Properties["Drill_ShiftsStartTime"].ToDate() : null;
        }
    }

    /// <summary>
    /// 班次持续时间(分钟)
    /// </summary>
    public int ShiftsTime
    {
        get
        {
            return Properties != null && Properties.ContainsKey("Drill_ShiftsTime") ? Properties["Drill_ShiftsTime"].ToInt() : 0;
        }
    }

    /// <summary>
    /// 钻机台面板料数量
    /// </summary>
    public int DrillBoardPositionStatus
    {
        get
        {
            return Properties != null && Properties.ContainsKey("Drill_BoardPositionStatus") ? Properties["Drill_BoardPositionStatus"].ToInt() : -1;
        }
    }

    /// <summary>
    /// Buffer生料层板料数量
    /// </summary>
    public int BufferRawMaterialLayerBoardStatus
    {
        get
        {
            return Properties != null && Properties.ContainsKey("Buffer_RawMaterialLayerBoardStatus") ? Properties["Buffer_RawMaterialLayerBoardStatus"].ToInt() : -1;
        }
    }

    /// <summary>
    /// Buffer熟料层板料数量
    /// </summary>
    public int BufferClinkerLayerBoardStatus
    {
        get
        {
            return Properties != null && Properties.ContainsKey("Buffer_ClinkerLayerBoardStatus") ? Properties["Buffer_ClinkerLayerBoardStatus"].ToInt() : -1;
        }
    }

    public string TaskCode { get; set; } = string.Empty;

    public string TaskBarCode { get; set; } = string.Empty;

    public string TaskItemCode { get; set; } = string.Empty;

    public int TaskWadCount { get; set; } = 0;

    public int NowPanelCount { get; set; } = 0;

    /// <summary>
    /// 待生产工单数
    /// </summary>
    public List<WorkOrderTask> PendingWorkOrders { get; set; } = new();

    public bool ExistPendingWorkOrders
    {
        get
        {
            return PendingWorkOrders != null && PendingWorkOrders.Count != 0;
        }
    }

    public string AlarmDesc
    {
        get
        {
            string str = string.Empty;
            if (!ExistPendingWorkOrders)
            {
                str += $"无待生产任务；{Environment.NewLine} ";
            }

            return str;
        }
    }

    public DateTime? WithoutPendingWorkOrdersTime { get; set; }

    /// <summary>
    /// 刀具寿命到开始时间
    /// </summary>
    public DateTime? DrillToolLifeExpiredStartTime
    {
        get
        {
            return (Properties != null && Properties.ContainsKey("Drill_ToolLifeExpiredStartTime") && !string.IsNullOrEmpty(Properties["Drill_ToolLifeExpiredStartTime"].ToStr()))
                ? Properties["Drill_ToolLifeExpiredStartTime"].ToDate() : null;
        }
    }

    /// <summary>
    /// 刀具寿命到结束时间
    /// </summary>
    public DateTime? DrillToolLifeExpiredEndTime
    {
        get
        {
            return (Properties != null && Properties.ContainsKey("Drill_ToolLifeExpiredEndTime") && !string.IsNullOrEmpty(Properties["Drill_ToolLifeExpiredEndTime"].ToStr()))
                ? Properties["Drill_ToolLifeExpiredEndTime"].ToDate() : null;
        }
    }

    /// <summary>
    /// 钻机从有板变到无板时间
    /// </summary>
    public DateTime? DrillChangeNoBoardTime
    {
        get
        {
            return (Properties != null && Properties.ContainsKey("Drill_ChangeNoBoardTime") && !string.IsNullOrEmpty(Properties["Drill_ChangeNoBoardTime"].ToStr()))
                ? Properties["Drill_ChangeNoBoardTime"].ToDate() : null;
        }
    }

    /// <summary>
    /// 钻机从无板变到有板时间
    /// </summary>
    public DateTime? DrillChangeExistBoardTime
    {
        get
        {
            return (Properties != null && Properties.ContainsKey("Drill_ChangeExistBoardTime") && !string.IsNullOrEmpty(Properties["Drill_ChangeExistBoardTime"].ToStr()))
                ? Properties["Drill_ChangeExistBoardTime"].ToDate() : null;
        }
    }

    /// <summary>
    /// 程序开始时间
    /// </summary>
    public DateTime? DrillRunStartTime
    {
        get
        {
            return (Properties != null && Properties.ContainsKey("Drill_RunStartTime") && !string.IsNullOrEmpty(Properties["Drill_RunStartTime"].ToStr()))
                ? Properties["Drill_RunStartTime"].ToDate() : null;
        }
    }

    /// <summary>
    /// 程序结束时间
    /// </summary>
    public DateTime? DrillRunEndTime
    {
        get
        {
            return (Properties != null && Properties.ContainsKey("Drill_RunEndTime") && !string.IsNullOrEmpty(Properties["Drill_RunEndTime"].ToStr()))
                ? Properties["Drill_RunEndTime"].ToDate() : null;
        }
    }

    /// <summary>
    /// buffer生料层从有板变到无板时间
    /// </summary>
    public DateTime? BufferRawChangeNoBoardTime
    {
        get
        {
            return (Properties != null && Properties.ContainsKey("Buffer_RawChangeNoBoardTime") && !string.IsNullOrEmpty(Properties["Buffer_RawChangeNoBoardTime"].ToStr()))
                ? Properties["Buffer_RawChangeNoBoardTime"].ToDate() : null;
        }
    }

    /// <summary>
    /// buffer生料层从无板变到有板时间
    /// </summary>
    public DateTime? BufferRawChangeExistBoardTime
    {
        get
        {
            return (Properties != null && Properties.ContainsKey("Buffer_RawChangeExistBoardTime") && !string.IsNullOrEmpty(Properties["Buffer_RawChangeExistBoardTime"].ToStr()))
                ? Properties["Buffer_RawChangeExistBoardTime"].ToDate() : null;
        }
    }

    /// <summary>
    /// buffer熟料层从无板变到有板时间
    /// </summary>
    public DateTime? BufferClinkerChangeExistTime
    {
        get
        {
            return (Properties != null && Properties.ContainsKey("Buffer_ClinkerChangeExistTime") && !string.IsNullOrEmpty(Properties["Buffer_ClinkerChangeExistTime"].ToStr()))
                ? Properties["Buffer_ClinkerChangeExistTime"].ToDate() : null;
        }
    }

    /// <summary>
    /// buffer熟料层从有板变到无板时间
    /// </summary>
    public DateTime? BufferClinkerChangeNoBoardTime
    {
        get
        {
            return (Properties != null && Properties.ContainsKey("Buffer_ClinkerChangeNoBoardTime") && !string.IsNullOrEmpty(Properties["Buffer_ClinkerChangeNoBoardTime"].ToStr()))
                ? Properties["Buffer_ClinkerChangeNoBoardTime"].ToDate() : null;
        }
    }

    /// <summary>
    /// buffer整个个上下料结束时间
    /// </summary>
    public DateTime? BufferAllUnloadAndLoadEndTime
    {
        get
        {
            return (Properties != null && Properties.ContainsKey("Buffer_AllUnloadAndLoadEndTime") && !string.IsNullOrEmpty(Properties["Buffer_AllUnloadAndLoadEndTime"].ToStr()))
                ? Properties["Buffer_AllUnloadAndLoadEndTime"].ToDate() : null;
        }
    }

    /// <summary>
    /// 清洗夹头开始时间
    /// </summary>
    public DateTime? DrillCollectCleanBegin
    {
        get
        {
            return (Properties != null && Properties.ContainsKey("Drill_CollectCleanBegin") && !string.IsNullOrEmpty(Properties["Drill_CollectCleanBegin"].ToStr()))
                ? Properties["Drill_CollectCleanBegin"].ToDate() : null;
        }
    }

    /// <summary>
    /// 清洗夹头结束时间
    /// </summary>
    public DateTime? DrillCollectCleanEnd
    {
        get
        {
            return (Properties != null && Properties.ContainsKey("Drill_CollectCleanEnd") && !string.IsNullOrEmpty(Properties["Drill_CollectCleanEnd"].ToStr()))
                ? Properties["Drill_CollectCleanEnd"].ToDate() : null;
        }
    }

    /// <summary>
    /// 异常开始时间
    /// </summary>
    public DateTime? DrillErrorStartTime
    {
        get
        {
            return (Properties != null && Properties.ContainsKey("Drill_ErrorStartTime") && !string.IsNullOrEmpty(Properties["Drill_ErrorStartTime"].ToStr()))
                ? Properties["Drill_ErrorStartTime"].ToDate() : null;
        }
    }

    /// <summary>
    /// 异常结束时间
    /// </summary>
    public DateTime? DrillErrorEndTime
    {
        get
        {
            return (Properties != null && Properties.ContainsKey("Drill_ErrorEndTime") && !string.IsNullOrEmpty(Properties["Drill_ErrorEndTime"].ToStr()))
                ? Properties["Drill_ErrorEndTime"].ToDate() : null;
        }
    }

    /// <summary>
    /// buffer自动开始时间
    /// </summary>
    public DateTime? DrillAutomaticStartTime
    {
        get
        {
            return (Properties != null && Properties.ContainsKey("Drill_AutomaticStartTime") && !string.IsNullOrEmpty(Properties["Drill_AutomaticStartTime"].ToStr()))
                ? Properties["Drill_AutomaticStartTime"].ToDate() : null;
        }
    }

    /// <summary>
    /// buffer自动结束时间
    /// </summary>
    public DateTime? DrillAutomaticEndTime
    {
        get
        {
            return (Properties != null && Properties.ContainsKey("Drill_AutomaticEndTime") && !string.IsNullOrEmpty(Properties["Drill_AutomaticEndTime"].ToStr()))
                ? Properties["Drill_AutomaticEndTime"].ToDate() : null;
        }
    }

    /// <summary>
    /// 屏幕字段
    /// </summary>
    public string? DrillScreenText
    {
        get
        {
            return (Properties != null && Properties.ContainsKey("Drill_ScreenText") && Properties["Drill_ScreenText"] != null)
                ? Properties["Drill_ScreenText"].ToStr() : string.Empty;
        }
    }

    /// <summary>
    /// 钻机事件id
    /// </summary>
    public string? DrillEventId
    {
        get
        {
            return (Properties != null && Properties.ContainsKey("Drill_EventId")) ? Properties["Drill_EventId"].ToStr() : string.Empty;
        }
    }

    /// <summary>
    /// 板方向检测开始
    /// </summary>
    public DateTime? DrillBoardDirectionStartTime
    {
        get
        {
            return (Properties != null && Properties.ContainsKey("Drill_BoardDirectionStartTime") && !string.IsNullOrEmpty(Properties["Drill_BoardDirectionStartTime"].ToStr()))
                ? Properties["Drill_BoardDirectionStartTime"].ToDate() : null;
        }
    }

    /// <summary>
    /// 板方向检测结束
    /// </summary>
    public DateTime? DrillBoardDirectionEndTime
    {
        get
        {
            return (Properties != null && Properties.ContainsKey("Drill_BoardDirectionEndTime") && !string.IsNullOrEmpty(Properties["Drill_BoardDirectionEndTime"].ToStr()))
                ? Properties["Drill_BoardDirectionEndTime"].ToDate() : null;
        }
    }

    /// <summary>
    /// pin检测开始
    /// </summary>
    public DateTime? DrillTestPinStartTime
    {
        get
        {
            return (Properties != null && Properties.ContainsKey("Drill_TestPinStartTime") && !string.IsNullOrEmpty(Properties["Drill_TestPinStartTime"].ToStr()))
                ? Properties["Drill_TestPinStartTime"].ToDate() : null;
        }
    }

    /// <summary>
    /// pin检测结束
    /// </summary>
    public DateTime? DrillTestPinEndTime
    {
        get
        {
            return (Properties != null && Properties.ContainsKey("Drill_TestPinEndTime") && !string.IsNullOrEmpty(Properties["Drill_TestPinEndTime"].ToStr()))
                ? Properties["Drill_TestPinEndTime"].ToDate() : null;
        }
    }

    /// <summary>
    /// 刀检开始
    /// </summary>
    public DateTime? DrillToolEvaluationStartTime
    {
        get
        {
            return (Properties != null && Properties.ContainsKey("Drill_ToolEvaluationStartTime") && !string.IsNullOrEmpty(Properties["Drill_ToolEvaluationStartTime"].ToStr()))
                ? Properties["Drill_ToolEvaluationStartTime"].ToDate() : null;
        }
    }

    /// <summary>
    /// 刀检结束
    /// </summary>
    public DateTime? DrillToolEvaluationEndTime
    {
        get
        {
            return (Properties != null && Properties.ContainsKey("Drill_ToolEvaluationEndTime") && !string.IsNullOrEmpty(Properties["Drill_ToolEvaluationEndTime"].ToStr()))
                ? Properties["Drill_ToolEvaluationEndTime"].ToDate() : null;
        }
    }

    /// <summary>
    /// BUFFER 手动开始时间
    /// </summary>
    public DateTime? BufferManualStartTime
    {
        get
        {
            return (Properties != null && Properties.ContainsKey("Buffer_ManualStartTime") && !string.IsNullOrEmpty(Properties["Buffer_ManualStartTime"].ToStr()))
                ? Properties["Buffer_ManualStartTime"].ToDate() : null;
        }
    }

    /// <summary>
    /// BUFFER 手动结束时间
    /// </summary>
    public DateTime? BufferManualEndTime
    {
        get
        {
            return (Properties != null && Properties.ContainsKey("Buffer_ManualEndTime") && !string.IsNullOrEmpty(Properties["Buffer_ManualEndTime"].ToStr()))
                ? Properties["Buffer_ManualEndTime"].ToDate() : null;
        }
    }

    /// <summary>
    /// 钻机吸尘报警，开始时间
    /// </summary>
    public DateTime? DrillNoVacuumStartTime
    {
        get
        {
            return (Properties != null && Properties.ContainsKey("Drill_NoVacuumStartTime") && !string.IsNullOrEmpty(Properties["Drill_NoVacuumStartTime"].ToStr()))
                ? Properties["Drill_NoVacuumStartTime"].ToDate() : null;
        }
    }

    /// <summary>
    /// 钻机吸尘报警，结束时间
    /// </summary>
    public DateTime? DrillNoVacuumEndTime
    {
        get
        {
            return (Properties != null && Properties.ContainsKey("Drill_NoVacuumEndTime") && !string.IsNullOrEmpty(Properties["Drill_NoVacuumEndTime"].ToStr()))
                ? Properties["Drill_NoVacuumEndTime"].ToDate() : null;
        }
    }

    /// <summary>
    /// 班次开始时间
    /// </summary>
    public DateTime? DrillShiftsStartTime
    {
        get
        {
            return (Properties != null && Properties.ContainsKey("Drill_ShiftsStartTime") && !string.IsNullOrEmpty(Properties["Drill_ShiftsStartTime"].ToStr()))
                ? Properties["Drill_ShiftsStartTime"].ToDate() : null;
        }
    }

    protected override void UpdateLocationCode(PanelList panels)
    {
        if (panels == null || panels.Count == 0) return;
        if (string.IsNullOrEmpty(panels.LocationCode))
            panels.SetLocationCode(GetLocationCode());
    }

    /// <summary>
    /// 设置板料到指定位置（轴），参数<paramref name="forceRaiseChangedEvent"/>是否强制发生板料变化事件
    /// </summary>
    /// <param name="panels">板料</param>
    /// <param name="forceRaiseChangedEvent">强制调用板料变化事件</param>
    protected override void SetPanelsToLocations(PanelList panels, bool forceRaiseChangedEvent = false)
    {
        SetPanelsToLocation(panels, 1, forceRaiseChangedEvent);
    }
}
