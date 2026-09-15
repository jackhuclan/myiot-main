using VgAutoDrill.Central.Core.Mes.Model;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Schedule;

namespace VgAutoDrill.Central.Core.Schedule.Handler;

internal class DrillScheduleTaskComparer : IComparer<DrillScheduleTask>
{
    public int Compare(DrillScheduleTask? x, DrillScheduleTask? y)
    {
        if (x == null && y == null) return 0;
        if (x == null && y != null) return 1;
        if (y == null && x != null) return -1;

        //部分完成
        if (x.InteractionSequence != InteractionSequence.UnloadOnly
            && y.InteractionSequence != InteractionSequence.UnloadOnly
            && x.ScheduledTaskStatus == ScheduledTaskStatus.PartCompleted
            && y.ScheduledTaskStatus != ScheduledTaskStatus.PartCompleted)
            return -1;

        //紧急调度
        if (x.ScheduledTaskStatus != ScheduledTaskStatus.Allocated
            && y.ScheduledTaskStatus != ScheduledTaskStatus.Allocated
            && x.IsUrgent > 0
            && y.IsUrgent <= 0)
            return -1;

        //钻机缺料
        if (x.ScheduledTaskStatus != ScheduledTaskStatus.Allocated
            && y.ScheduledTaskStatus != ScheduledTaskStatus.Allocated
            && x.InteractionSequence == InteractionSequence.LoadOnly
            && x.InteractionSequence == InteractionSequence.LoadOnly
            && x.DrillBoardPositionStatus == 0
            && x.BufferRawMaterialLayerBoardStatus == 0
            && y.DrillBoardPositionStatus != 0
            && y.BufferRawMaterialLayerBoardStatus != 0)
            return -1;

        //只上料
        if (x.ScheduledTaskStatus != ScheduledTaskStatus.Allocated
            && y.ScheduledTaskStatus != ScheduledTaskStatus.Allocated
            && x.InteractionSequence == InteractionSequence.LoadOnly
            && x.InteractionSequence != InteractionSequence.LoadOnly)
            return -1;

        //既上又下
        if (x.ScheduledTaskStatus != ScheduledTaskStatus.Allocated
            && y.ScheduledTaskStatus != ScheduledTaskStatus.Allocated
            && x.InteractionSequence == InteractionSequence.LoadThenUnload && !string.IsNullOrEmpty(x.ItemCode)
            && x.InteractionSequence != InteractionSequence.LoadThenUnload && !string.IsNullOrEmpty(y.ItemCode))
            return -1;

        //钻孔进度百分比
        if (x.ScheduledTaskStatus == y.ScheduledTaskStatus
            && x.Percentage > y.Percentage)
            return -1;

        return x.Id.CompareTo(y.Id);
    }
}
