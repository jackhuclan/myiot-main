using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DrillRateFactor;

namespace VgAutoDrill.Admin.Application.Interfaces.MesServices
{
    public interface IDrillRateFactorService
    {
        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<PageDto<DrillRateFactorDto>>> GetList(GetDrillRateFactorListReq req);

        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseDto<DrillRateFactorDto>> QueryByID(long id);

        /// <summary>
        /// 添加信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> Add(AddOrUpdateDrillRateFactorReq req);

        /// <summary>
        /// 修改信息
        /// </summary>
        /// <returns></returns>
        Task<ResponseDto<string>> Update(AddOrUpdateDrillRateFactorReq req);

        /// <summary>
        /// 删除信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> Delete(long id);
        /// <summary>
        /// 删除信息集合
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> DeleteList(object[] idList);

        Task<ResponseDto<string>> AddOrUpdate(AddOrUpdateDrillRateFactorReq req);
        Task<ResponseDto<string>> BulkAddOrUpdate(List<AddOrUpdateDrillRateFactorReq> reqs);
        Task<DrillRateFactor> GetRateFactorWithoutEndTime(AddOrUpdateDrillRateFactorReq req);
    }
}
