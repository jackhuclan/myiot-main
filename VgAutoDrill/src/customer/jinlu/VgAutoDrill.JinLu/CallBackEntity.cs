namespace VgAutoDrill.JinLu
{
    public class CallBackEntity
    {
        public CallBackEntity()
        {
            vehicle_id = 0;
            vehicle_name = "";
            action = new List<int>();
            map_id = 0;
            station_no = 0;
            station_name = string.Empty;
            order_id = 0;
        }
        /// <summary>
        ///  车辆ID
        /// </summary>
        public int vehicle_id { get; set; }

        /// <summary>
        /// 车辆名称
        /// </summary>
        public string vehicle_name { get; set; }
        /// <summary>
        /// 动作列表
        /// </summary>
        public List<int> action { get; set; }
        /// <summary>
        /// 地图ID
        /// </summary>
        public int map_id { get; set; }
        /// <summary>
        /// 站点ID
        /// </summary>
        public int station_no { get; set; }
        /// <summary>
        /// 站点名称
        /// </summary>
        public string station_name { get; set; }
        /// <summary>
        /// 订单ID
        /// </summary>
        public long order_id { get; set; }
    }
}
