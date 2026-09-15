using VgAutoDrill.Admin.Model.ViewModels.Req.Equipment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.EquipmentType;
using VgAutoDrill.Admin.Model.ViewModels.Mes.AgvTasks;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DvAlarmRecords;

namespace VgAutoDrill.Admin.Application.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    /// <typeparam name="TGetReq"></typeparam>
    /// <typeparam name="TAddorUpdateReq"></typeparam>
    /// <typeparam name="TTreeDto"></typeparam>
    public interface IBaseMesServiceWithTree<TDto, TGetReq, TAddorUpdateReq,TTreeDto> : IBaseMesService<TDto, TGetReq, TAddorUpdateReq>
    {
        /// <summary>
        /// 获取树形结构数据
        /// </summary>
        /// <returns></returns>
        Task<ResponseDto<List<TTreeDto>>> GetTreeList();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    /// <typeparam name="TGetReq"></typeparam>
    /// <typeparam name="TAddorUpdateReq"></typeparam>
    public interface IBaseMesService<TDto,TGetReq,TAddorUpdateReq>
    {
        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<PageDto<TDto>>> GetList(TGetReq req);


        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseDto<TDto>> QueryByID(int id);

        /// <summary>
        /// 添加信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> Add(TAddorUpdateReq req);

        /// <summary>
        /// 修改信息
        /// </summary>
        /// <returns></returns>
        Task<ResponseDto<string>> Update(TAddorUpdateReq req);

        /// <summary>
        /// 删除信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseDto<string>> Delete(int id);
    }
}
