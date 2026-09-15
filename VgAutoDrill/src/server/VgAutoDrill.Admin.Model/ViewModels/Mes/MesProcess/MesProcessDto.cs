namespace VgAutoDrill.Admin.Model.ViewModels.Mes.MesProcess
{
    public class MesProcessDto : BaseDto
    {
        public string Name { set; get; }

        public string Code { set; get; }

        /// <summary>
        /// 工艺要求
        /// </summary>
        public string Attention { get; set; }
    }
}
