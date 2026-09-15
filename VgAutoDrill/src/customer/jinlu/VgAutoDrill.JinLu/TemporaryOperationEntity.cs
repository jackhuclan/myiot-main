using VgAutoDrill.Fundation.Vendor.Vega;

namespace VgAutoDrill.JinLu
{
    public class TemporaryOperationEntity
    {
        public TemporaryOperationEntity() {
            AgvPosition=string.Empty;
            OperationList = new List<SwapPanel>();
            PlcPreActionList=new List<PlcPreAction>();
        }   
        /// <summary>
        /// agv位置
        /// </summary>
        public string AgvPosition { set; get; }

        /// <summary>
        /// 动作列表
        /// </summary>
        public List<SwapPanel> OperationList { set; get; }

        public List<PlcPreAction> PlcPreActionList { set; get; }
}
}
