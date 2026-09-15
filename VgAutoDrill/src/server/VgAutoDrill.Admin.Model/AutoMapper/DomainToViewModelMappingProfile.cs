using AutoMapper;
using VgAutoDrill.Admin.Model.Entites;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.AgvRest;
using VgAutoDrill.Admin.Model.ViewModels.Mes.AgvRestAndPart;
using VgAutoDrill.Admin.Model.ViewModels.Mes.CheckRecords;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Client;
using VgAutoDrill.Admin.Model.ViewModels.Mes.CutterConfigDetail;
using VgAutoDrill.Admin.Model.ViewModels.Mes.CutterConfigMaster;
using VgAutoDrill.Admin.Model.ViewModels.Mes.CutterGroup;
using VgAutoDrill.Admin.Model.ViewModels.Mes.CutterGroupDetail;
using VgAutoDrill.Admin.Model.ViewModels.Mes.CutterPlan;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceAndRoute;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceAndSubject;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceCutter;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceEvent;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceGateway;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceMain;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceMaintainDetail;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DevicePanel;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DevicePanelHistory;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceParameter;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceRecords;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceRecordsSummary;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceServiceInvocation;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceTemporaryMaintenanceRecords;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DrillPanelDetail;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DrillRateFactor;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DrillWorkOrder;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DvAlarmRecord;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DvAlarmType;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DvInformType;
using VgAutoDrill.Admin.Model.ViewModels.Mes.EncodeBuildRules;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Equipment;
using VgAutoDrill.Admin.Model.ViewModels.Mes.EquipmentType;
using VgAutoDrill.Admin.Model.ViewModels.Mes.External;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ExternalTask;
using VgAutoDrill.Admin.Model.ViewModels.Mes.FeedBack;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ItemAtpFile;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ItemAtpFileDetail;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ItemDrillFile;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ItemDrillFileDetail;
using VgAutoDrill.Admin.Model.ViewModels.Mes.LocationDetail;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ManualCallAgv;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ManualCallAgvTask;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ManualCallAgvTask.Req;
using VgAutoDrill.Admin.Model.ViewModels.Mes.MaterialStock;
using VgAutoDrill.Admin.Model.ViewModels.Mes.MaterialStockDetail;
using VgAutoDrill.Admin.Model.ViewModels.Mes.MaterialStockOverview;
using VgAutoDrill.Admin.Model.ViewModels.Mes.MaterialStockOverviewHistory;
using VgAutoDrill.Admin.Model.ViewModels.Mes.MaterialStockStorage;
using VgAutoDrill.Admin.Model.ViewModels.Mes.MaterialStockStorageHistory;
using VgAutoDrill.Admin.Model.ViewModels.Mes.MdItem;
using VgAutoDrill.Admin.Model.ViewModels.Mes.MdItemType;
using VgAutoDrill.Admin.Model.ViewModels.Mes.MesProcess;
using VgAutoDrill.Admin.Model.ViewModels.Mes.MesProcessRecipe;
using VgAutoDrill.Admin.Model.ViewModels.Mes.NotificationRecord;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Partition;
using VgAutoDrill.Admin.Model.ViewModels.Mes.PartitionSetting;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProBoardTrace;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProduceTask;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProduceTaskHistory;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProductBom;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProductCategory;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProTask;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProTransOrder;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProWorkOrder;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Rack;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Route;
using VgAutoDrill.Admin.Model.ViewModels.Mes.RouteAndProcess;
using VgAutoDrill.Admin.Model.ViewModels.Mes.RouteAndProductCategory;
using VgAutoDrill.Admin.Model.ViewModels.Mes.RouteProcessAndWorkStation;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Schedule;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Schedulement;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SchedulementDetail;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Silo;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SiloPanelTrace;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Subject;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig;
using VgAutoDrill.Admin.Model.ViewModels.Mes.TransferJob;
using VgAutoDrill.Admin.Model.ViewModels.Mes.TransferJobLog;
using VgAutoDrill.Admin.Model.ViewModels.Mes.UnitMeasure;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Vendor;
using VgAutoDrill.Admin.Model.ViewModels.Mes.WorkOrderAlterLog;
using VgAutoDrill.Admin.Model.ViewModels.Mes.WorkOrderAndPanel;
using VgAutoDrill.Admin.Model.ViewModels.Mes.WorkOrderAndWorkStation;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Workshop;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Workstation;
using VgAutoDrill.Admin.Model.ViewModels.Req.AppSecret;
using VgAutoDrill.Admin.Model.ViewModels.Req.Department;
using VgAutoDrill.Admin.Model.ViewModels.Req.Menu;
using VgAutoDrill.Admin.Model.ViewModels.Req.Position;
using VgAutoDrill.Admin.Model.ViewModels.Req.Role;
using VgAutoDrill.Admin.Model.ViewModels.Req.User;
using VgAutoDrill.Admin.Model.ViewModels.Res.Department;
using VgAutoDrill.Admin.Model.ViewModels.Res.Menu;
using VgAutoDrill.Admin.Model.ViewModels.Res.Position;
using VgAutoDrill.Admin.Model.ViewModels.Res.Role;
using VgAutoDrill.Admin.Model.ViewModels.Res.User;

namespace VgAutoDrill.Admin.Model.AutoMapper;

/// <summary>
/// 
/// </summary>
public class DomainToViewModelMappingProfile : Profile
{
    /// <summary>
    /// 
    /// </summary>
    public DomainToViewModelMappingProfile()
    {

        CreateMap<AddAppSecretReq, SysAppSecret>();
        CreateMap<AddOrUpdatePositionReq, SysPosition>();
        CreateMap<SysPosition, PositionDto>();
        CreateMap<AddOrUpdateRoleReq, SysRole>();
        CreateMap<SysRole, RoleDto>();
        CreateMap<MenuReq, SysMenu>();
        CreateMap<SysMenu, MenuDto>();
        //部门
        CreateMap<SysDepartment, DepartmentInfoDto>();
        CreateMap<SysDepartment, DepartmentDto>();
        CreateMap<AddOrUpdateDepatmentReq, SysDepartment>();
        //用户
        CreateMap<SysUser, UserDetailDto>();
        CreateMap<AddUserReq, SysUser>();

        //设备
        CreateMap<AddOrUpdateDeviceReq, Device>();
        CreateMap<Device, DeviceDto>()
            .ForMember(d => d.RawLocationCodes,
            opt => opt.MapFrom(m => string.IsNullOrWhiteSpace(m.RawLocationCode) ? new List<string>() : m.RawLocationCode.Split(",", StringSplitOptions.None).ToList()
            ))
            .ForMember(d => d.ClinkerLocationCodes,
            opt => opt.MapFrom(m => string.IsNullOrWhiteSpace(m.ClinkerLocationCode) ? new List<string>() : m.ClinkerLocationCode.Split(",", StringSplitOptions.None).ToList()))
            .ReverseMap();
        CreateMap<Device, DeviceInfoDto>();
        CreateMap<Device, DrillPanelFullData>();

        //设备类型
        CreateMap<AddOrUpdateDeviceTypeReq, DeviceType>();
        CreateMap<DeviceType, DeviceTypeDto>();
        CreateMap<DeviceType, DeviceTypeInfoDto>();

        //AGV调度记录
        CreateMap<AddOrUpdateScheduleReq, Schedule>();
        CreateMap<Schedule, ScheduleDto>();
        CreateMap<Schedule, SimpleScheduleDto>();
        CreateMap<Schedule, ScheduleHistory>();
        CreateMap<Schedule, ScheduleLogsDto>()
            .ForMember(x => x.RequestJson, opt => opt.Ignore())
            .ForMember(x => x.ScheduleLogs, opt => opt.Ignore());

        //AGV调度历史记录
        CreateMap<AddOrUpdateScheduleReq, ScheduleHistory>();
        CreateMap<ScheduleHistory, ScheduleDto>();
        CreateMap<ScheduleHistory, Schedule>();
        CreateMap<ScheduleHistory, SimpleScheduleDto>();
        CreateMap<ScheduleHistory, ScheduleLogsDto>()
            .ForMember(x => x.RequestJson, opt => opt.Ignore())
            .ForMember(x => x.ScheduleLogs, opt => opt.Ignore());

        //告警记录
        CreateMap<AddOrUpdateAlarmReq, Alarm>();
        CreateMap<Alarm, AlarmDto>();
        //告警类型
        CreateMap<AddOrUpdateAlarmSettingReq, AlarmSetting>();
        CreateMap<AlarmSetting, AlarmSettingDto>();
        //通知记录
        CreateMap<AddOrUpdateNotifyReq, Notify>();
        CreateMap<Notify, NotifyDto>();
        //通知类型
        CreateMap<AddOrUpdateNotifySettingReq, NotifySetting>();
        CreateMap<NotifySetting, NotifySettingDto>();

        //物料类型
        CreateMap<AddOrUpdateItemTypeReq, ItemType>();
        CreateMap<ItemType, ItemTypeDto>();

        //物料
        CreateMap<AddOrUpdateItemReq, Item>();
        CreateMap<Item, ItemDto>();

        //工序
        CreateMap<AddOrUpdateMesProcessReq, Process>();
        CreateMap<Process, MesProcessDto>();

        //工艺参数
        CreateMap<AddOrUpdateMesProcessRecipeReq, MesProcessRecipe>();
        CreateMap<MesProcessRecipe, MesProcessRecipeDto>();

        //板料管理
        CreateMap<AddOrUpdatePanelReq, TracePanel>();
        CreateMap<TracePanel, PanelDto>();
        CreateMap<BatchInsertPanelReq, TracePanel>();

        //生产工单
        CreateMap<AddOrUpdateWorkOrderReq, WorkOrder>();
        CreateMap<WorkOrder, WorkOrderDto>();

        //生产任务单
        CreateMap<AddOrUpdateTaskReq, Entites.Mes.WorkTask>();
        CreateMap<Entites.Mes.WorkTask, TaskDto>();

        //生产流转单
        CreateMap<AddOrUpdateTransOrderReq, TransOrder>();
        CreateMap<TransOrder, TransOrderDto>();

        //设备事件
        CreateMap<AddOrUpdateEventDefineReq, EventDefine>();
        CreateMap<EventDefine, EventDefineDto>();
        //刀具
        CreateMap<AddOrUpdateCutterReq, Cutter>();
        CreateMap<Cutter, CutterDto>();
        //设备参数
        CreateMap<AddOrUpdateDeviceParameterReq, DeviceParameter>();
        CreateMap<DeviceParameter, DeviceParameterDto>();

        CreateMap<AddOrUpdateCutterConfigMasterReq, CutterConfigMaster>();
        CreateMap<CutterConfigMaster, CutterConfigMasterDto>();

        CreateMap<AddOrUpdateCutterConfigDetailReq, CutterConfigDetail>();
        CreateMap<CutterConfigDetail, CutterConfigDetailDto>();

        CreateMap<AddOrUpdateCutterPlanReq, CutterPlan>();
        CreateMap<CutterPlan, CutterPlanDto>();

        CreateMap<AddOrUpdateWorkshopReq, Workshop>();
        CreateMap<Workshop, WorkshopDto>();

        CreateMap<AddOrUpdateWorkstationReq, WorkStation>();
        CreateMap<WorkStation, WorkstationDto>();

        CreateMap<AddOrUpdateDeviceGatewayReq, DeviceGateway>();
        CreateMap<DeviceGateway, DeviceGatewayDto>();

        CreateMap<AddOrUpdateVendorReq, Vendor>();
        CreateMap<Vendor, VendorDto>();

        CreateMap<AddOrUpdateClientReq, Client>();
        CreateMap<Client, ClientDto>();

        CreateMap<AddOrUpdateUnitMeasureReq, UnitMeasure>();
        CreateMap<UnitMeasure, UnitMeasureDto>();

        CreateMap<AddOrUpdateProductCategoryReq, ProductCategory>();
        CreateMap<ProductCategory, ProductCategoryDto>();

        CreateMap<AddOrUpdatePartitionReq, Partition>();
        CreateMap<Partition, PartitionDto>();
        CreateMap<PartitionSetting, PartitionSettingDto>();
        CreateMap<PartitionSettingDto, PartitionSetting>();

        CreateMap<AddOrUpdateMaterialStockReq, MaterialStock>();
        CreateMap<MaterialStock, MaterialStockDto>();

        CreateMap<AddOrUpdateFeedBackReq, FeedBack>();
        CreateMap<FeedBack, FeedBackDto>();

        CreateMap<AddOrUpdateMaterialStockDetailReq, MaterialStockDetail>();
        CreateMap<MaterialStockDetail, MaterialStockDetailDto>();

        CreateMap<AddOrUpdateMaterialStockOverviewReq, MaterialStockOverview>();
        CreateMap<MaterialStockOverview, MaterialStockOverviewDto>();

        CreateMap<AddOrUpdateMaterialStockOverviewHistoryReq, MaterialStockOverviewHistory>();
        CreateMap<MaterialStockOverviewHistory, MaterialStockOverviewHistoryDto>();

        CreateMap<AddOrUpdateMaterialStockStorageReq, MaterialStockStorage>();
        CreateMap<MaterialStockStorage, MaterialStockStorageDto>();

        CreateMap<AddOrUpdateMaterialStockStorageHistoryReq, MaterialStockStorageHistory>();
        CreateMap<MaterialStockStorageHistory, MaterialStockStorageHistoryDto>();

        CreateMap<AddOrUpdateProduceTaskReq, ProduceTask>();
        CreateMap<ProduceTask, ProduceTaskDto>();

        CreateMap<AddOrUpdateProduceTaskHistoryReq, ProduceTaskHistory>();
        CreateMap<ProduceTaskHistory, ProduceTaskHistoryDto>();

        CreateMap<AddOrUpdateRouteReq, Route>();
        CreateMap<Route, RouteDto>();

        CreateMap<AddOrUpdateProductBomReq, ProductBom>();
        CreateMap<ProductBom, ProductBomDto>();

        CreateMap<AddOrUpdateRouteAndProcessReq, RouteAndProcess>();
        CreateMap<RouteAndProcess, RouteAndProcessDto>();

        CreateMap<AddRouteAndProductCategoryReq, RouteAndProductCategory>();
        CreateMap<RouteAndProductCategory, RouteAndProductCategoryDto>();

        CreateMap<UpdateRouteProcessAndWorkStationReq, RouteProcessAndWorkStation>();
        CreateMap<RouteProcessAndWorkStation, RouteProcessAndWorkStationDto>();

        CreateMap<AddOrUpdateSubjectReq, Subject>();
        CreateMap<Subject, SubjectDto>();

        CreateMap<AddOrUpdateItemDrillFileReq, ItemDrillFile>();
        CreateMap<ItemDrillFile, ItemDrillFileDto>();

        CreateMap<AddOrUpdateItemAtpFileReq, ItemAtpFile>();
        CreateMap<ItemAtpFile, ItemAtpFileDto>();

        CreateMap<AddOrUpdateItemAtpFileDetailReq, ItemAtpFileDetail>();
        CreateMap<ItemAtpFileDetail, ItemAtpFileDetailDto>();

        CreateMap<AddOrUpdateItemDrillFileDetailReq, ItemDrillFileDetail>();
        CreateMap<ItemDrillFileDetail, ItemDrillFileDetailDto>();

        CreateMap<AddOrUpdateCheckRecordsReq, CheckRecords>();
        CreateMap<CheckRecords, CheckRecordsDto>();

        CreateMap<AddOrUpdateDevicePanelReq, DevicePanel>();
        CreateMap<DevicePanel, DevicePanelDto>();

        CreateMap<AddOrUpdateDevicePanelHistoryReq, DevicePanelHistory>();
        CreateMap<DevicePanelHistory, DevicePanelHistoryDto>();

        CreateMap<AddOrUpdateDeviceMaintainDetailReq, DeviceMaintainDetail>();
        CreateMap<DeviceMaintainDetail, DeviceMaintainDetailDto>();

        CreateMap<AddOrUpdateDeviceMaintainReq, DeviceMaintain>();
        CreateMap<DeviceMaintain, DeviceMaintainDto>();

        CreateMap<AddOrUpdateDeviceAndSubjectReq, DeviceAndSubject>();
        CreateMap<DeviceAndSubject, DeviceAndSubjectDto>();

        CreateMap<AddOrUpdateEncodeBuildRulesReq, EncodeBuildRules>();
        CreateMap<EncodeBuildRules, EncodeBuildRulesDto>();

        CreateMap<AddOrUpdateDrillWorkOrderReq, DrillWorkOrder>();
        CreateMap<DrillWorkOrder, DrillWorkOrderDto>();

        CreateMap<AddOrUpdateWorkOrderAndWorkStationReq, WorkOrderAndWorkStation>();
        CreateMap<WorkOrderAndWorkStation, WorkOrderAndWorkStationDto>();

        CreateMap<AddOrUpdateScheduleLogReq, ScheduleLog>();
        CreateMap<ScheduleLog, ScheduleLogDto>();

        CreateMap<AddOrUpdateDeviceAndRouteReq, DeviceAndRoute>();
        CreateMap<DeviceAndRoute, DeviceAndRouteDto>();

        CreateMap<AddOrUpdateSysConfigReq, SysConfig>();
        CreateMap<SysConfig, SysConfigDto>();
        CreateMap<SysConfig, SysConfigTreeDto>();

        //External 工单
        CreateMap<AddOrUpdateExternalWorkOrderReq, ExternalWorkOrder>();
        CreateMap<ExternalWorkOrder, ExternalWorkOrderDto>();
        CreateMap<ExternalWorkOrder, WorkOrder>();
        CreateMap<WorkOrderDto, ExternalWorkTaskDto>();
        CreateMap<Entites.Mes.WorkTask, TaskViewDto>();
        CreateMap<WorkOrderDto, AddOrUpdateDrillWorkOrderReq>();
        CreateMap<ExternalAndInnerWorkOrderDto, ExternalWorkTaskDto>();
        CreateMap<ExternalAddOrUpdatePanelReq, AddOrUpdateWorkOrderAndPanelReq>();
        CreateMap<ExternalAddOrUpdatePanelReq, AddOrUpdatePanelReq>();

        //钻机任务
        CreateMap<Entites.Mes.WorkTask, ExternalTaskDto>();
        CreateMap<TaskMoveByDeviceAndDateReq, Entites.Mes.WorkTask>();

        //钻机服务调用
        CreateMap<DeviceServiceInvocation, DeviceServiceInvocationDto>();
        CreateMap<AddOrUpdateDeviceServiceInvocationReq, DeviceServiceInvocation>();

        CreateMap<WorkOrderAndPanel, WorkOrderAndPanelDto>();
        CreateMap<AddOrUpdateWorkOrderAndPanelReq, WorkOrderAndPanel>();

        //料架 Rack
        CreateMap<Rack, RackDto>();
        CreateMap<AddOrUpdateRackReq, Rack>();
        CreateMap<ExternalAddOrUpdateRackReq, AddOrUpdateRackReq>();
        CreateMap<RackExcelDto, Rack>();
        CreateMap<Rack, RackFullData>();

        //料仓 Silo
        CreateMap<Silo, SiloDto>();
        CreateMap<AddOrUpdateSiloReq, Silo>();
        CreateMap<SiloDetail, SiloDetailDto>();
        CreateMap<AddOrUpdateSiloDetailReq, SiloDetail>();
        CreateMap<ExternalAddOrUpdateSiloReq, AddOrUpdateSiloReq>();
        CreateMap<ExternalAddOrUpdateSiloDetailReq, SiloDetail>();
        CreateMap<UnBindSiloDetail, SiloDetail>();
        CreateMap<SiloExcelDto, Silo>();

        //钻机料仓
        CreateMap<AddOrUpdateDrillPanelDetailReq, DrillPanelDetail>();
        CreateMap<DrillPanelDetail, DrillPanelDetailDto>();
        CreateMap<BatchAddOrUpdateDrillPanelDetailReq, DrillPanelDetail>();

        //料仓任务
        CreateMap<AddOrUpdateTransferJobReq, TransferJob>();
        CreateMap<TransferJob, TransferJobDto>();
        CreateMap<TransportationHistoryTask, TransferJobDto>();
        CreateMap<TransferJob, TransportationHistoryTask>();
        CreateMap<TransferJobDto, TransportationTaskToExcelDto>();

        //AGV点位
        CreateMap<AddOrUpdateRestCodeReq, AgvRest>();
        CreateMap<AgvRest, RestCodeDto>();

        //AGV点位和分区关联关系
        CreateMap<AddOrUpdateRestAndPartReq, AgvRestAndPart>();
        CreateMap<RestAndPartDto, AgvRestAndPart>();

        //工单变更记录表
        CreateMap<AddOrUpdateWorkOrderAlterLogReq, WorkOrderAlterLog>();
        CreateMap<WorkOrderAlterLog, WorkOrderAlterLogDto>();

        //料仓任务详细表
        CreateMap<AddOrUpdateTransferJobLogReq, TransferJobLog>();
        CreateMap<TransferJobLog, TransferJobLogDto>();
        CreateMap<TransferJob, TransferJobLogsDto>();
        CreateMap<TransportationHistoryTask, TransferJobLogsDto>();

        //设备记录表
        CreateMap<AddOrUpdateDeviceRecordsReq, DeviceRecords>();
        CreateMap<DeviceRecords, DeviceRecordsDto>();

        //设备记录汇总表
        CreateMap<AddOrUpdateDeviceRecordsSummaryReq, DeviceRecordsSummary>();
        CreateMap<DeviceRecordsSummary, DeviceRecordsSummaryDto>();

        //稼动率因素表
        CreateMap<AddOrUpdateDrillRateFactorReq, DrillRateFactor>();
        CreateMap<DrillRateFactor, DrillRateFactorDto>();

        //设备临时保养记录
        CreateMap<AddOrUpdateDeviceTemporaryMaintenanceRecordsReq, DeviceTemporaryMaintenanceRecordsDto>();
        CreateMap<AddOrUpdateDeviceTemporaryMaintenanceRecordsReq, DeviceTemporaryMaintenanceRecords>();
        CreateMap<DeviceTemporaryMaintenanceRecords, DeviceTemporaryMaintenanceRecordsDto>();

        //自动配刀组
        CreateMap<CutterGroup, ExternaCutterlTaskDto>();
        CreateMap<CutterGroup, CutterGroupDto>();
        CreateMap<CutterGroupDetail, CutterGroupDetailDto>();

        //手动呼叫agv
        CreateMap<ManualCallAgvLog, ManualCallAgvLogDto>();
        CreateMap<ManualCallAgvTask, ManualCallAgvTaskDto>();
        CreateMap<AddOrUpdateManualCallAgvTaskReq, ManualCallAgvTask>();

        //料仓板料追溯 SiloPanelTrace
        CreateMap<SiloPanelTrace, SiloPanelTraceDto>();
        CreateMap<AddOrUpdateSiloPanelTraceReq, SiloPanelTrace>();
        CreateMap<SiloPanelTrace, SiloPanelTraceToExcelDto>();

        //料仓板料追溯详细 SiloPanelTraceDetail
        CreateMap<SiloPanelTraceDetail, SiloPanelTraceDetailDto>();
        CreateMap<AddOrUpdateSiloPanelTraceDetailReq, SiloPanelTraceDetail>();

        //库位明细 LocationDetail
        CreateMap<LocationDetail, LocationDetailDto>();
        CreateMap<AddOrUpdateLocationDetailReq, LocationDetail>();
    }

}
