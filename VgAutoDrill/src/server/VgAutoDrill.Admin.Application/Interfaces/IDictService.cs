using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Res.Dict;

namespace VgAutoDrill.Admin.Application.Interfaces
{
    public interface IDictService
    {
        /// <summary>
        /// 根据类型获取字典列表
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<ResponseDto<List<DictDataDto>>> GetDictData(string type);
    }
}
