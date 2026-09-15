using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Req.AppSecret;
using VgAutoDrill.Admin.Model.ViewModels.Res.AppSecret;

namespace VgAutoDrill.Admin.Application.Interfaces
{
    public interface IAppSecretService
    {
        /// <summary>
        /// 添加应用
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> AddAppSecret(AddAppSecretReq req);
        /// <summary>
        /// 获取应用
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseDto<AppSecretDto>> GetAppSecret(long id);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<PageDto<AppSecretDto>>> GetPageList(GetAppSecretPageListReq req);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> Delete(long id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> Update(SysAppSecretReq req);
    }
}
