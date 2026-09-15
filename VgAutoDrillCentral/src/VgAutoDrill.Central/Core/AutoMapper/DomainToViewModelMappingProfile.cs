using AutoMapper;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProBoardTrace;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProTask;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Rack;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Schedulement;
using VgAutoDrill.Admin.Model.ViewModels.Mes.TransferJob;
using VgAutoDrill.Central.Core.Domain;
using VgAutoDrill.Central.Core.Mes;
using VgAutoDrill.Central.Core.Mes.Model;
using VgAutoDrill.Fundation.Iot.Transportation;
using Panel = VgAutoDrill.Fundation.Iot.Models.Panel;

namespace VgAutoDrill.Central.Core.AutoMapper;

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
        CreateMap<QueryScheduleRequest, GetScheduleListReq>();
        CreateMap<TaskDto, WorkOrderTask>();
        CreateMap<Admin.Model.Entites.Mes.WorkTask, WorkOrderTask>();

        CreateMap<ScheduleDto, ScheduleTask>()
            .ForMember(dest => dest.CallerDeviceId, opt => opt.MapFrom(src => src.SourceDeviceId))
            .ForMember(dest => dest.LocationCode, opt => opt.MapFrom(src => src.SubDeviceCode))
            .ForMember(dest => dest.AllocatedAgv, opt => opt.MapFrom(src => src.RequireDeviceId));
        CreateMap<ScheduleDto, ScheduleTaskWithRequest>()
            .ForMember(dest => dest.CallerDeviceId, opt => opt.MapFrom(src => src.SourceDeviceId))
            .ForMember(dest => dest.LocationCode, opt => opt.MapFrom(src => src.SubDeviceCode))
            .ForMember(dest => dest.AllocatedAgv, opt => opt.MapFrom(src => src.RequireDeviceId));
        CreateMap<ScheduleDto, DrillScheduleTask>()
            .ForMember(dest => dest.CallerDeviceId, opt => opt.MapFrom(src => src.SourceDeviceId))
            .ForMember(dest => dest.LocationCode, opt => opt.MapFrom(src => src.SubDeviceCode))
            .ForMember(dest => dest.AllocatedAgv, opt => opt.MapFrom(src => src.RequireDeviceId));
        CreateMap<ScheduleDto, PinScheduleTask>()
            .ForMember(dest => dest.CallerDeviceId, opt => opt.MapFrom(src => src.SourceDeviceId))
            .ForMember(dest => dest.LocationCode, opt => opt.MapFrom(src => src.SubDeviceCode))
            .ForMember(dest => dest.AllocatedAgv, opt => opt.MapFrom(src => src.RequireDeviceId));
        CreateMap<ScheduleDto, UnpinScheduleTask>()
            .ForMember(dest => dest.CallerDeviceId, opt => opt.MapFrom(src => src.SourceDeviceId))
            .ForMember(dest => dest.LocationCode, opt => opt.MapFrom(src => src.SubDeviceCode))
            .ForMember(dest => dest.AllocatedAgv, opt => opt.MapFrom(src => src.RequireDeviceId));
        CreateMap<ScheduleDto, ForkScheduleTask>()
            .ForMember(dest => dest.CallerDeviceId, opt => opt.MapFrom(src => src.SourceDeviceId))
            .ForMember(dest => dest.LocationCode, opt => opt.MapFrom(src => src.SubDeviceCode))
            .ForMember(dest => dest.AllocatedAgv, opt => opt.MapFrom(src => src.RequireDeviceId));
        CreateMap<ScheduleDto, ShelfScheduleTask>()
            .ForMember(dest => dest.CallerDeviceId, opt => opt.MapFrom(src => src.SourceDeviceId))
            .ForMember(dest => dest.LocationCode, opt => opt.MapFrom(src => src.SubDeviceCode))
            .ForMember(dest => dest.AllocatedAgv, opt => opt.MapFrom(src => src.RequireDeviceId));

        CreateMap<ScheduleTaskWithRequest, DrillScheduleTask>();
        CreateMap<ScheduleTaskWithRequest, PinScheduleTask>();
        CreateMap<ScheduleTaskWithRequest, UnpinScheduleTask>();
        CreateMap<ScheduleTaskWithRequest, ForkScheduleTask>();
        CreateMap<ScheduleTaskWithRequest, ShelfScheduleTask>();
        CreateMap<DrillScheduleTask, DrillScheduleTask>();
        CreateMap<PinScheduleTask, PinScheduleTask>();
        CreateMap<UnpinScheduleTask, UnpinScheduleTask>();
        CreateMap<ForkScheduleTask, ForkScheduleTask>();
        CreateMap<ShelfScheduleTask, ShelfScheduleTask>();
        CreateMap<ScheduleTask, ScheduleTaskWithRequest>();

        CreateMap<GetNextPanelRequest, BatchInsertPanelReq>();
        CreateMap<PanelDto, Panel>();

        CreateMap<TransferJobDto, TransferJob>()
            .ForMember(dest => dest.MasterScheduleId, opt => opt.MapFrom(src => src.RelatedScheduleId))
            .ForMember(dest => dest.PartitionCode, opt => opt.MapFrom(src => src.WarehouseCode))
            .ForMember(dest => dest.MasterRouteCode, opt => opt.MapFrom(src => src.RelatedRoute))
            .ForMember(dest => dest.MasterLocationCode, opt => opt.MapFrom(src => src.RelateDeviceCode))
            .ForMember(dest => dest.TransferDesc, opt => opt.MapFrom(src => src.RelatedDrillTrace));

        CreateMap<QueryTransportationRequest, GetTransferJobListReq>();

        CreateMap<TransferJob, AddOrUpdateTransferJobReq>()
            .ForMember(dest => dest.RelatedScheduleId, opt => opt.MapFrom(src => src.MasterScheduleId))
            .ForMember(dest => dest.WarehouseCode, opt => opt.MapFrom(src => src.PartitionCode))
            .ForMember(dest => dest.RelatedRoute, opt => opt.MapFrom(src => src.MasterRouteCode))
            .ForMember(dest => dest.RelateDeviceCode, opt => opt.MapFrom(src => src.MasterLocationCode))
            .ForMember(dest => dest.RelatedDrillTrace, opt => opt.MapFrom(src => src.TransferDesc));

        CreateMap<AddOrUpdateTransferJobReq, VgAutoDrill.Admin.Model.Entites.Mes.TransferJob>();

        CreateMap<RackDto, Location>()
            .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
            .ForMember(dest => dest.PartitionCode, opt => opt.MapFrom(src => src.WareHouseCode))
            .ForMember(dest => dest.SiloCode, opt => opt.MapFrom(src => src.SiloCode))
            .ForMember(dest => dest.DeviceId, opt => opt.MapFrom(src => src.RelateDeviceCode))
            .ForMember(dest => dest.LocationDeviceKind, opt => opt.MapFrom(src => src.DeviceKind));
        //.ForMember(dest => dest.TransAGVInnerPoint, opt => opt.MapFrom(src => src.TransAGVInnerPoint))
        //.ForMember(dest => dest.TransAGVOutputPoint, opt => opt.MapFrom(src => src.TransAGVOutputPoint))
        //.ForMember(dest => dest.TransAGVRestPoint, opt => opt.MapFrom(src => src.TransAGVRestPoint))
        //.ForMember(dest => dest.FeedAGVInnerPoint, opt => opt.MapFrom(src => src.FeedAGVInnerPoint))
        //.ForMember(dest => dest.FeedAGVOutputPoint, opt => opt.MapFrom(src => src.FeedAGVOutputPoint))
        //.ForMember(dest => dest.FeedAGVRestPoint, opt => opt.MapFrom(src => src.FeedAGVRestPoint))

        CreateMap<Admin.Model.ViewModels.Mes.Partition.PartitionDto, Mes.Model.Partition>()
            .ForMember(dest => dest.PartCode, opt => opt.MapFrom(src => src.Code))
            .ForMember(dest => dest.PartName, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.SiloKind, opt => opt.MapFrom(src => src.TransportationKind));

        CreateMap<Admin.Model.ViewModels.Mes.AgvRest.RestCodeDto, Mes.Model.RestPoint>()
            .ForMember(dest => dest.RestCode, opt => opt.MapFrom(src => src.Code))
            .ForMember(dest => dest.RestName, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Point, opt => opt.MapFrom(src => src.Point))
            .ForMember(dest => dest.PreBookAGV, opt => opt.MapFrom(src => src.PreBookAgv))
            .ForMember(dest => dest.CurrentAgv, opt => opt.MapFrom(src => src.CurrentAgv));

        CreateMap<VgAutoDrill.Admin.Model.ViewModels.Mes.AgvRest.RestCodeDto, VgAutoDrill.Admin.Model.ViewModels.Mes.AgvRest.AddOrUpdateRestCodeReq>();

        CreateMap<Admin.Model.ViewModels.Mes.DvAlarmType.AlarmSettingDto, Mes.Model.AlarmSetting>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
            .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.AlarmLevel))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.AlarmDesc));

        CreateMap<Admin.Model.Entites.Mes.AlarmSetting, Mes.Model.AlarmSetting>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Code, opt => opt.MapFrom(src => src.Code))
            .ForMember(dest => dest.Level, opt => opt.MapFrom(src => src.AlarmLevel))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.AlarmDesc));

        CreateMap<Admin.Model.ViewModels.Mes.DvAlarmRecord.AlarmDto, Mes.Model.AlarmLog>();
    }
}
