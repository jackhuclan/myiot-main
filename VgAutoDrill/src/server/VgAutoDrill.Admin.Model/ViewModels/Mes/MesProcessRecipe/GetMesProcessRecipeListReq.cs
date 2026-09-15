namespace VgAutoDrill.Admin.Model.ViewModels.Mes.MesProcessRecipe
{
    public class GetMesProcessRecipeListReq : Page
    {
        public string Name { set; get; }

        public string Code { set; get; }


        /// <summary>
        /// 状态
        /// </summary>

        public int Status { set; get; } = -1;
    }
}
