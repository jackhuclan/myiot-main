using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.JinLu
{
    public class PlcPreAction
    {
        /// <summary>
        /// 对应Drill轴ID
        /// </summary>
        public int SpindleId { set; get; }

        /// <summary>
        /// 对应轴动作
        /// </summary>
        public InteractionBehavior SpindleAction { set; get; }

    }
}
