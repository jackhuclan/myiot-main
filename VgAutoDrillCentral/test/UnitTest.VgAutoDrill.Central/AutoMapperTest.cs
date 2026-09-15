using AutoMapper;
using VgAutoDrill.Central.Core.AutoMapper;
using VgAutoDrill.Central.Core.Mes.Model;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;
using Panel = VgAutoDrill.Fundation.Iot.Models.Panel;

namespace UnitTest.VgAutoDrill.Central;

public class AutoMapperTest
{
    [Fact]
    public void Example2()
    {
        var configuration = new MapperConfiguration(cfg =>
        {
            cfg.AddIgnoreMapAttribute();
            cfg.CreateMap<ScheduleTaskWithRequest, DrillScheduleTask>();
        });

        var mapper = new Mapper(configuration);

        var scheduleOfDb = new ScheduleTaskWithRequest
        {
            Id = DateTime.Now.Ticks,
            RequestDeviceKind = DeviceKind.CNC84Drill,
            CallerDeviceId = "drill",
            RouteCode = "B001",
            Code = Guid.NewGuid().ToString(),
            TaskId = Guid.NewGuid().ToString(),
            ScheduledTaskStatus = ScheduledTaskStatus.Completed,
            CreateTime = DateTime.Now,
            ItemCode = "TO20240926000048",
            LocationCode = "drill",
            InteractionSequence = InteractionSequence.LoadThenUnload,
            EventRequest = new DeviceEventReportRequest
            {
                RequestDeviceKind = DeviceKind.CNC84Drill,
                DeviceId = "drill",
                EventId = "drill",
                RequestInteractionBehavior = InteractionBehavior.Make(DeviceKind.CNC84Drill, InteractionBehavior.REAR_LOAD_PANEL_ONLY),
                RequestMaterialKind = MaterialKind.Panel,
                RequestInteractionDirection = InteractionPosition.Rear,
                PayloadPanels = Panel.HasSilo.HasPanelForSpindleFirst("abc", 3 * 2, 2, ProductStatus.Finished_DRILL),
            },
            Appointed = false,
        };

        var scheduleOfMemory = new DrillScheduleTask
        {
            Id = DateTime.Now.Ticks,
            RequestDeviceKind = DeviceKind.CNC84Drill,
            CallerDeviceId = "drill",
            RouteCode = "B001",
            Code = Guid.NewGuid().ToString(),
            TaskId = Guid.NewGuid().ToString(),
            ScheduledTaskStatus = ScheduledTaskStatus.Running,
            CreateTime = DateTime.Now,
            ItemCode = "TO20240926000048",
            LocationCode = "drill",
            InteractionSequence = InteractionSequence.LoadThenUnload,
            EventRequest = new DeviceEventReportRequest
            {
                RequestDeviceKind = DeviceKind.CNC84Drill,
                DeviceId = "drill",
                EventId = "drill",
                RequestInteractionBehavior = InteractionBehavior.Make(DeviceKind.CNC84Drill, InteractionBehavior.REAR_LOAD_PANEL_ONLY),
                RequestMaterialKind = MaterialKind.Panel,
                RequestInteractionDirection = InteractionPosition.Rear,
                PayloadPanels = Panel.HasSilo.HasPanelForSpindleFirst("abc", 3 * 2, 2, ProductStatus.Finished_DRILL),
            },
            Appointed = true,
        };

        mapper.Map(scheduleOfDb, scheduleOfMemory);

        Assert.True(scheduleOfMemory.Appointed);
        Assert.Equal(ScheduledTaskStatus.Completed, scheduleOfMemory.ScheduledTaskStatus);
    }
}
