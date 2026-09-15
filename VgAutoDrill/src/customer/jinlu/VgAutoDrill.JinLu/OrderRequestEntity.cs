
namespace VgAutoDrill.JinLu
{
    public class OrderRequestEntity
    {
        /// <summary>
        /// 车辆ID
        /// </summary>
        public int vehicle_id { get; set; }

        /// <summary>
        /// 优先
        /// </summary>
        public int priority { get; set; }

        /// <summary>
        /// 任务
        /// </summary>
        public List<RequestMission> mission { get; set; }

    }
    public class RequestMission
    {
        public string type { get; set; }
        public int destination { get; set; }
        public int map_id { get; set; }
        public int action_id { get; set; }
        public int action_param1 { get; set; }
        public int action_param2 { get; set; }
    }
}
