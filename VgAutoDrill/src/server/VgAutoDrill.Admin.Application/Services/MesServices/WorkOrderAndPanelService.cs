using AutoMapper;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Common.Extensions;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.WorkOrderAndPanel;
using VgAutoDrill.Admin.Repository.Interfaces;

namespace VgAutoDrill.Admin.Application.Services.MesServices
{
    public class WorkOrderAndPanelService : BaseServiceWithoutTree<WorkOrderAndPanel, WorkOrderAndPanelDto, AddOrUpdateWorkOrderAndPanelReq>, IWorkOrderAndPanelService
    {
        public readonly IWorkOrderAndPanelDomainService _innerOrderAndPanelDomainService;
        public readonly IUnitOfWork _unitOfWork;
        public WorkOrderAndPanelService(IWorkOrderAndPanelDomainService domainService, IUnitOfWork unitOfWork, IMapper mapper)
            : base(domainService, mapper)
        {
            _innerOrderAndPanelDomainService = domainService;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<WorkOrderAndPanelDto>> GetWorkOrderAndPanelInfors(GetWorkOrderAndPanelInforsReq req)
        {
            var db = _unitOfWork.GetDbClient();
            var where = db.Queryable<WorkOrderAndPanel>();

            where = where.Where(t => t.IsDeleted == 0);

            if (!string.IsNullOrEmpty(req.WorkOrderCode))
            {
                where = where.Where(t => t.WorkOrderCode == req.WorkOrderCode);
            }
            if (!string.IsNullOrEmpty(req.PanelCode))
            {
                where = where.Where(t => t.PanelCode == req.PanelCode);
            }
            if (!string.IsNullOrEmpty(req.BatchCode))
            {
                where = where.Where(t => t.BatchCode == req.BatchCode);
            }
            if (!string.IsNullOrEmpty(req.ItemCode))
            {
                where = where.Where(t => t.ItemCode == req.ItemCode);
            }

            if (!string.IsNullOrEmpty(req.TaskCode))
            {
                where = where.Where(t => t.TaskCode == req.TaskCode);
            }

            if (!string.IsNullOrEmpty(req.ExternalWorkerOrder))
            {
                where = where.Where(t => t.ExternalWorkerOrder == req.ExternalWorkerOrder);
            }

            var result = await where.Select(t => new WorkOrderAndPanelDto
            {
                PanelCode = t.PanelCode,
                WorkOrderCode = t.WorkOrderCode,
                TaskCode = t.TaskCode,
                ItemCode = t.ItemCode,
                Pcs = t.Pcs,
                BatchCode = t.BatchCode,
                ProductStatus = t.ProductStatus,
                BoardLocation = t.BoardLocation,
                PanelWidth = t.PanelWidth,
                PinOffset = t.PinOffset,
                ExternalWorkerOrder = t.ExternalWorkerOrder,

            }).ToListAsync();

            return result.ToList();
        }

        public async Task<ResponseDto<PageDto<WorkOrderAndPanelDto>>> GetWorkOrderAndPanelList(GetWorkOrderAndPanelReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;
            var pageDto = new PageDto<WorkOrderAndPanelDto>(req.PageNum, req.PageSize);

            var where = PredicateBuilder.True<WorkOrderAndPanel>();
            where = where.And(p => p.IsDeleted == 0);
            if (!string.IsNullOrEmpty(req.WorkOrderCode))
            {
                where = where.And(t => t.WorkOrderCode == req.WorkOrderCode);
            }
            if (!string.IsNullOrEmpty(req.PanelCode))
            {
                where = where.And(t => t.PanelCode == req.PanelCode);
            }
            if (!string.IsNullOrEmpty(req.BatchCode))
            {
                where = where.And(t => t.BatchCode == req.BatchCode);
            }
            if (!string.IsNullOrEmpty(req.ItemCode))
            {
                where = where.And(t => t.ItemCode == req.ItemCode);
            }

            if (!string.IsNullOrEmpty(req.TaskCode))
            {
                where = where.And(t => t.TaskCode == req.TaskCode);
            }

            if (!string.IsNullOrEmpty(req.ExternalWorkerOrder))
            {
                where = where.And(t => t.ExternalWorkerOrder == req.ExternalWorkerOrder);
            }


            var result = await _domainService.QueryPageAsync(where, q => q.PanelCode, SqlSugar.OrderByType.Desc, req.PageNum, req.PageSize);
            pageDto.Total = result.TotalCount;
            pageDto.List = _mapper.Map<List<WorkOrderAndPanel>, List<WorkOrderAndPanelDto>>(result.ToList());
            return Success(pageDto);
        }

        public async Task<ResponseDto<string>> AddWorkOrderAndPanelList(List<AddOrUpdateWorkOrderAndPanelReq> req)
        {
            if (req == null || req.Count() == 0)
            {
                return Fail("数据格式错误");
            }

            for (int i = 0; i < req.Count(); i++)
            {
                var selectRes = _domainService.QueryAsync(q => q.PanelCode == req[i].PanelCode, q => q.Id, SqlSugar.OrderByType.Asc);
                if (selectRes.Result.Count() > 0)
                {
                    return Fail($"{req[i].PanelCode}在系统中已经存在，不能添加");
                }

                if (string.IsNullOrEmpty(req[i].PanelCode))
                {
                    return Fail($"数据中有空的板材号，不能添加");
                }
            }
            var pancelCode = req.Select(t => t.PanelCode).ToList().Distinct().Count();
            if (pancelCode < req.Count())
            {
                return Fail($"数据中有重复的板材号，不能添加");
            }
            List<WorkOrderAndPanel> lst = new List<WorkOrderAndPanel>();
            for (int i = 0; i < req.Count(); i++)
            {
                WorkOrderAndPanel moPanel = new WorkOrderAndPanel();
                moPanel.PanelCode = req[i].PanelCode;
                moPanel.TaskCode = req[i].TaskCode;
                moPanel.IsDeleted = 0;
                moPanel.Status = 1;
                moPanel.WorkOrderCode = req[i].WorkOrderCode;
                moPanel.ItemCode = req[i].ItemCode;
                moPanel.BatchCode = req[i].BatchCode;
                moPanel.Pcs = req[i].Pcs;
                moPanel.ProductStatus = req[i].ProductStatus;
                moPanel.BoardLocation = req[i].BoardLocation;
                moPanel.CreateTime = DateTime.Now;
                moPanel.CreatorId = UserId;
                moPanel.PanelWidth = req[i].PanelWidth;
                moPanel.PinOffset = req[i].PinOffset;
                moPanel.ExternalWorkerOrder = req[i].ExternalWorkerOrder;
                lst.Add(moPanel);
            }
            await _domainService.BulkInsert(lst);
            return Success();
        }

        /// <summary>
        /// 删除数据（taskCode or panel）
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> DeleteList(List<string> req)
        {
            if (req == null || req.Count() == 0)
            {
                return Fail("数据格式错误");
            }

            object[] tempArr = new object[req.Count()];
            for (int i = 0; i < req.Count(); i++)
            {

                var selectRes = _domainService.QueryAsync(q => q.PanelCode == req[i], q => q.Id, SqlSugar.OrderByType.Asc);
                if (selectRes.Result.Count() > 0) //list 中是pancel
                {
                    var mopanel = _mapper.Map<WorkOrderAndPanel>(selectRes.Result[0]);
                    tempArr[i] = selectRes.Result[0].Id;
                }
                else
                {
                    //list 中是task
                    selectRes = _domainService.QueryAsync(q => q.TaskCode == req[i], q => q.Id, SqlSugar.OrderByType.Asc);
                    if (selectRes.Result.Count() > 0)
                    {
                        var mopanel = _mapper.Map<WorkOrderAndPanel>(selectRes.Result[0]);
                        tempArr[i] = selectRes.Result[0].Id;
                    }
                }
            }

            var result = await _domainService.DeleteByIds(tempArr);
            if (result.ToBool())
            {
                return Success();
            }
            else
            {
                return Fail("删除失败");
            }
        }

        public async Task<ResponseDto<string>> UpdateWorkOrderAndPanelList(List<AddOrUpdateWorkOrderAndPanelReq> req)
        {
            if (req == null || req.Count() == 0)
            {
                return Fail("数据格式错误");
            }

            List<WorkOrderAndPanel> lst = new List<WorkOrderAndPanel>();
            for (int i = 0; i < req.Count(); i++)
            {
                if (string.IsNullOrEmpty(req[i].PanelCode))
                {
                    return Fail($"数据中有空的板材号，不能添加");
                }

                var model = _mapper.Map<WorkOrderAndPanel>(req[i]);
                var result = _domainService.QueryAsync(t => t.PanelCode == req[i].PanelCode, t => t.PanelCode, SqlSugar.OrderByType.Asc);
                if (result.Result == null || result.Result.Count() == 0)
                {
                    return Fail($"系统中不存在板材：{req[i].PanelCode},不能更新");
                }
                model.Id = result.Result.First().Id;
                model.CreateTime = result.Result.First().CreateTime;
                model.CreatorId = result.Result.First().CreatorId;
                model.ModifierId = UserId;
                model.ModifyTime = DateTime.Now;
                lst.Add(model);
            }

            var res = await _domainService.BulkUpdate(lst);
            if (!res)
            {
                return Fail<string>("更新任务表失败");
            }
            return Success();
        }

        public async Task<ResponseDto<string>> DeleteByExternalCode(string externalSourceCode)
        {
            if (string.IsNullOrEmpty(externalSourceCode))
            {
                return Fail("数据格式错误");
            }

            var result = await _innerOrderAndPanelDomainService.DeleteAsync(d => d.ExternalWorkerOrder.ToLower() == externalSourceCode.ToLower());
            if (result.ToBool())
            {
                return Success();
            }
            else
            {
                return Fail("删除失败");
            }
        }
    }
}
