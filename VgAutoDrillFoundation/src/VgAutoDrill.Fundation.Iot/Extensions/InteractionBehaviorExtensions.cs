using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Fundation.Iot.Extensions;

public static class InteractionBehaviorExtensions
{
    public static InteractionBehavior ChangeSequence(this InteractionBehavior original, InteractionSequence newSequence)
    {
        return InteractionBehavior.Make(original.InteractionMode, original.DeviceKind, newSequence, original.InteractionPosition, original.MaterialKind);
    }

    /// <summary>
    /// 匹配呼叫类型，和当前的AGV类型是否一致
    /// </summary>
    /// <param name="deviceKind">AGV的类型</param>
    /// <param name="requesetMaterialKind">呼叫的类型</param>
    /// <param name="position">前、后</param>
    /// <returns></returns>
    public static bool MatchAgvAndMaterialKind(DeviceKind deviceKind, MaterialKind requesetMaterialKind, InteractionPosition position)
    {
        //前上料钻机 呼叫 上料
        if (position == InteractionPosition.Front && requesetMaterialKind == MaterialKind.Panel && deviceKind == DeviceKind.FrontPanelAgv)
            return true;
        //后上料钻机 呼叫 上料
        else if (position == InteractionPosition.Rear && requesetMaterialKind == MaterialKind.Panel && deviceKind == DeviceKind.BackPanelAgv)
            return true;
        //钻机 呼叫 前换刀
        //TODO, 如果能提供后换刀AGV时，需要补加position == InteractionPosition.Front or position == InteractionPosition.Rear
        else if (requesetMaterialKind == MaterialKind.Cutter && deviceKind == DeviceKind.FrontToolAgv)
            return true;
        //料架 呼叫 运输AGV，拉走熟料--暂无
        //料架 呼叫 后上料来取生料
        //料架 呼叫 后上料来放熟料
        //前上料 呼叫 转运AGV, 放生料 或者 取熟料 --暂无
        else if (requesetMaterialKind == MaterialKind.PanelSilo && (deviceKind == DeviceKind.ShelfSiloAgv || deviceKind == DeviceKind.RollerSiloAgv || deviceKind == DeviceKind.BackPanelAgv))
            return true;
        else
            return false;
    }
}
