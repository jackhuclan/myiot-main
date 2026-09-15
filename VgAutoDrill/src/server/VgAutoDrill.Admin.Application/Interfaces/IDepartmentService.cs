using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Req.Department;
using VgAutoDrill.Admin.Model.ViewModels.Res.Department;

namespace VgAutoDrill.Admin.Application.Interfaces
{
    public interface IDepartmentService
    {
        /// <summary>
        /// 获取部门数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<PageDto<DepartmentDto>>> GetDepartmentList(GetDepartmentListReq req);
        /// <summary>
        /// 获取部门树形结构数据
        /// </summary>
        /// <returns></returns>
        Task<ResponseDto<List<DepartmentTreeDto>>> GetDepartmentTreeList();
        /// <summary>
        /// 获取部门信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseDto<DepartmentInfoDto>> QueryByID(long id);
        /// <summary>
        /// 添加部门信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> Add(AddOrUpdateDepatmentReq req);
        /// <summary>
        /// 修改部门信息
        /// </summary>
        /// <returns></returns>
        Task<ResponseDto<string>> Update(AddOrUpdateDepatmentReq req);
        /// <summary>
        /// 删除部门信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> Delete(long id);
    }
}
