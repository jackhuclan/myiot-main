using AutoMapper;
using SqlSugar;
using System.Text;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Common.Extensions;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.Enum;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.MesProcess;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig;
using VgAutoDrill.Admin.Repository.Interfaces;

namespace VgAutoDrill.Admin.Application.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class ProcessService : BaseServiceWithoutTree<Process, MesProcessDto, AddOrUpdateMesProcessReq>, IMesProcessService
    {
        private readonly ISysConfigManager _sysConfigManager;
        private readonly IUnitOfWork _unitOfWork;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainService"></param>
        /// <param name="mapper"></param>
        public ProcessService(IProcessDomainService domainService, ISysConfigManager sysConfigManager, IUnitOfWork unitOfWork, IMapper mapper)
            : base(domainService, mapper)
        {
            _sysConfigManager = sysConfigManager;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<MesProcessDto>>> GetList(GetMesProcessListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;
            var pageDto = new PageDto<MesProcessDto>(req.PageNum, req.PageSize);

            var where = PredicateBuilder.True<Process>();
            where = where.And(p => p.IsDeleted == 0);

            if (!string.IsNullOrEmpty(req.Code))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.Code) && p.Code.Contains(req.Code));
            }

            if (!string.IsNullOrEmpty(req.Name))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.Name) && p.Name.Contains(req.Name));
            }

            if (req.Status > -1)
            {
                where = where.And(p => p.Status == req.Status);
            }

            var result = await _domainService.QueryPageAsync(where, q => q.Code, SqlSugar.OrderByType.Asc, req.PageNum, req.PageSize);

            pageDto.Total = result.TotalCount;
            pageDto.List = _mapper.Map<List<Process>, List<MesProcessDto>>(result.ToList());
            return Success<PageDto<MesProcessDto>>(pageDto);
        }

        public async Task<ResponseDto<List<DropSelectDto>>> GetDropSelectDatas(GetMesProcessListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = int.MaxValue;
            var pageDto = new PageDto<MesProcessDto>(req.PageNum, req.PageSize);

            var where = PredicateBuilder.True<Process>();
            where = where.And(p => p.IsDeleted == 0);

            if (!string.IsNullOrEmpty(req.Code))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.Code) && p.Code.Contains(req.Code));
            }

            if (!string.IsNullOrEmpty(req.Name))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.Name) && p.Name.Contains(req.Name));
            }

            if (req.Status > -1)
            {
                where = where.And(p => p.Status == req.Status);
            }

            var datas = await _domainService.QueryPageAsync(where, q => q.Code, SqlSugar.OrderByType.Asc, req.PageNum, req.PageSize);
            if (datas == null || datas.Count == 0)
            {
                return Success(new List<DropSelectDto>());
            }

            var result = new List<DropSelectDto>();
            foreach (var data in datas)
            {
                result.Add(new DropSelectDto
                {
                    Id = data.Id,
                    Code = data.Code,
                    Name = data.Name,
                    Label = $"{data.Code}({data.Name})",
                });
            }

            return Success(result);
        }

        /// <summary>
        /// 批量添加
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> BulkInsert(List<ProcessToExcelDto> list)
        {
            if (list == null || list.Count == 0)
            {
                return Fail("未识别有效的数据！");
            }

            bool importStatus = await _sysConfigManager.GetBoolValue(MESConfigConstants.IMPORT_STATUS);

            int failCount = 0;
            StringBuilder sb = new StringBuilder();
            List<Process> processs = new List<Process>();
            foreach (var item in list)
            {
                if (string.IsNullOrEmpty(item.Code) || string.IsNullOrEmpty(item.Name))
                {
                    sb.Append("编码：" + item.Code + " 或名称：" + item.Name + " 为空；");
                    sb.Append("\r\n");
                    failCount++;
                    continue;
                }

                var isExsitCode = await _domainService.IsExistAsync(p => p.Code == item.Code);
                if (isExsitCode)
                {
                    sb.Append("编码" + item.Code + " 数据库已存在；");
                    sb.Append("\r\n");
                    failCount++;
                    continue;
                }

                if (processs.Exists(p => p.Code == item.Code))
                {
                    sb.Append("编码" + item.Code + " 导入列表中已存在；");
                    sb.Append("\r\n");
                    failCount++;
                    continue;
                }

                Process model = new Process();
                model.Code = item.Code;
                model.Name = item.Name;
                model.Attention = item.Attention;
                if (importStatus)
                {
                    model.Status = 1;
                }
                else
                {
                    model.Status = 0;
                }
                model.CreatorId = UserId;
                model.CreateTime = DateTime.Now;
                processs.Add(model);
            }
            var result = await _domainService.BulkInsert(processs);
            if (!result)
            {
                return Fail("导入失败！");
            }

            string str = string.Format("预计导入：{0} 条；成功导入：{1} 条；失败：{2} 条；\r\n", list.Count, list.Count - failCount, failCount);
            str = str + sb.ToString();

            return Success(str);
        }

        /// <summary>
        /// 添加信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> AddData(AddOrUpdateMesProcessReq req)
        {
            if (req == null)
            {
                return Fail("信息格式错误!");
            }

            var isExsitCode = await _domainService.IsExistAsync(p => p.Code == req.Code);
            if (isExsitCode)
            {
                return Fail("已存在相同的编码!");
            }

            var model = _mapper.Map<Process>(req);
            model.CreateTime = DateTime.Now;
            model.CreatorId = UserId;
            model.Status = (int)DataStatusEnum.Enable;
            await _domainService.Add(model);
            return Success();
        }

        /// <summary>
        /// 修改信息
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> UpdateData(AddOrUpdateMesProcessReq req)
        {
            if (req == null)
            {
                return Fail("信息格式错误!");
            }

            var entity = await _domainService.QueryByID(req.Id);
            if (entity == null)
            {
                return Fail("信息不存在!");
            }

            var isExsitCode = await _domainService.IsExistAsync(p => p.Code == req.Code && p.Id != req.Id);
            if (isExsitCode)
            {
                return Fail("已存在相同的编码!");
            }

            var model = _mapper.Map<Process>(req);
            model.CreateTime = entity.CreateTime;
            model.CreatorId = entity.CreatorId;
            model.ModifierId = UserId;
            model.ModifyTime = DateTime.Now;
            await _domainService.Update(model);
            return Success();
        }


        public async Task<ResponseDto<string>> DeleteProcessList(object[] idList)
        {
            if (idList == null || idList.Length == 0)
            {
                return Fail("信息错误");
            }

            for (int i = 0; i < idList.Length; i++)
            {
                List<RouteAndProcess> list = await GetRouteInf(idList[i].ToInt());
                if (list.Count() > 0)
                {
                    return Fail($"不能删除:流程在工艺路线中使用");
                }
            }
            var res = DeleteList(idList);
            if (res.Result.Code.ToString() != "Success")
            {
                return Fail($"删除失败:{res.Result.Message}");
            }
            return Success();
        }

        public async Task<ResponseDto<string>> DeleteProcess(long id)
        {
            if (id > 0)
            {
                var result = _domainService.QueryByID(id);
                if (result == null)
                {
                    return Fail("信息错误");
                }

                var db = _unitOfWork.GetDbClient();
                List<RouteAndProcess> list = await GetRouteInf(id);
                if (list.Count() > 0)
                {
                    var routeId = list.Select(t => t.RouteId).FirstOrDefault();
                    var route = db.Queryable<Route>().Where(r => r.IsDeleted == 0 && r.Id == routeId);
                    var RouteName = route.ToList().Select(t => t.Name).FirstOrDefault();
                    return Fail($"不能删除:工艺路线{RouteName}在使用");
                }
                else
                {
                    var delete = Delete(id);
                    if (delete.Result.Code.ToString() != "Success")
                    {
                        return Fail($"删除失败:{delete.Result.Message}");
                    }
                }
            }
            return Success();
        }

        public async Task<List<RouteAndProcess>> GetRouteInf(long id)
        {
            var db = _unitOfWork.GetDbClient();
            var query = db.Queryable<RouteAndProcess, Process>
            ((rp, p) => new object[]
            {
                        JoinType.Inner,rp.ProcessId == p.Id
             });
            query = query.Where((rp, p) => rp.IsDeleted == 0 && p.IsDeleted == 0 && p.Id == id);
            query = query.OrderBy((rp, p) => p.Code);
            return query.ToList();
        }
    }
}
