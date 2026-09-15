namespace VgAutoDrill.Admin.Model.ViewModels.Req.Department
{
    public class GetDepartmentListReq : Page
    {
        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartmentName { set; get; }
        /// <summary>
        /// 状态
        /// </summary>

        public int Status { set; get; } = -1;
    }
}
