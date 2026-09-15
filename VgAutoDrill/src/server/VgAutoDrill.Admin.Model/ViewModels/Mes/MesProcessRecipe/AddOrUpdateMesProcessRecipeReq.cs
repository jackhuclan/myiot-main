namespace VgAutoDrill.Admin.Model.ViewModels.Mes.MesProcessRecipe
{
    public class AddOrUpdateMesProcessRecipeReq : BaseAddOrUpdateDto
    {
        public string Name { set; get; }

        public string Code { set; get; }

        /// <summary>
        /// 产品编号
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 工序编号
        /// </summary>
        public string ProcessCode { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName { get; set; }

        /// <summary>
        /// 参数
        /// json 字符串
        /// </summary>
        public virtual string? Parameters { get; set; }
    }
}