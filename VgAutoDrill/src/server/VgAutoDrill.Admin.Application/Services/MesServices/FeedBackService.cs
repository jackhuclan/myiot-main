using AutoMapper;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Common.Extensions;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.FeedBack;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProTask;

namespace VgAutoDrill.Admin.Application.Services.MesServices
{
    /// <summary>
    /// 生产报工记录
    /// </summary>
    public class FeedBackService : BaseServiceWithoutTree<FeedBack, FeedBackDto, AddOrUpdateFeedBackReq>, IFeedBackService
    {
        private readonly IFeedBackDomainService _feedBackService;
        private readonly ITaskDomainService _taskDomainService;
        private readonly IWorkOrderDomainService _workOrderDomainService;
        private readonly IDrillTaskDomainService _drillTaskDomainService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainService"></param>
        /// <param name="taskDomainService"></param>
        /// <param name="workOrderDomainService"></param>
        /// <param name="drillTaskDomainService"></param>
        /// <param name="mapper"></param>
        public FeedBackService(IFeedBackDomainService domainService,
            ITaskDomainService taskDomainService,
            IWorkOrderDomainService workOrderDomainService,
            IDrillTaskDomainService drillTaskDomainService,
            IMapper mapper)
            : base(domainService, mapper)
        {
            _feedBackService = domainService;
            _taskDomainService = taskDomainService;
            _workOrderDomainService = workOrderDomainService;
            _drillTaskDomainService = drillTaskDomainService;
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<FeedBackDto>>> GetList(GetFeedBackListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;
            var pageDto = new PageDto<FeedBackDto>(req.PageNum, req.PageSize);

            var where = PredicateBuilder.True<FeedBack>();
            where = where.And(p => p.IsDeleted == 0);

            if (!string.IsNullOrEmpty(req.ItemCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.ItemCode) && p.ItemCode.Contains(req.ItemCode));
            }

            if (!string.IsNullOrEmpty(req.ItemName))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.ItemName) && p.ItemName.Contains(req.ItemName));
            }

            if (!string.IsNullOrEmpty(req.ProcessCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.ProcessCode) && p.ProcessCode.Contains(req.ProcessCode));
            }

            if (!string.IsNullOrEmpty(req.ProcessName))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.ProcessName) && p.ProcessName.Contains(req.ProcessName));
            }

            if (!string.IsNullOrEmpty(req.WorkOrderCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.WorkOrderCode) && p.WorkOrderCode.Contains(req.WorkOrderCode));
            }

            if (!string.IsNullOrEmpty(req.WorkOrderName))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.WorkOrderName) && p.WorkOrderName.Contains(req.WorkOrderName));
            }

            if (!string.IsNullOrEmpty(req.WorkStationCode))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.WorkStationCode) && p.WorkStationCode.Contains(req.WorkStationCode));
            }

            if (!string.IsNullOrEmpty(req.WorkStationName))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.WorkStationName) && p.WorkStationName.Contains(req.WorkStationName));
            }

            if (!string.IsNullOrEmpty(req.FeedBackType))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.FeedBackType) && p.FeedBackType.Equals(req.FeedBackType));
            }

            if (!string.IsNullOrEmpty(req.FeedBackStatus))
            {
                where = where.And(p => !string.IsNullOrEmpty(p.FeedBackStatus) && p.FeedBackStatus.ToUpper().Equals(req.FeedBackStatus.ToUpper()));
            }

            var result = await _domainService.QueryPageAsync(where, q => q.CreateTime, SqlSugar.OrderByType.Desc, req.PageNum, req.PageSize);

            pageDto.Total = result.TotalCount;
            pageDto.List = _mapper.Map<List<FeedBack>, List<FeedBackDto>>(result.ToList());
            return Success<PageDto<FeedBackDto>>(pageDto);
        }

        /// <summary>
        /// 根据ItemTypeId获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<FeedBackDto>>> GetEquipmentList(GetFeedBackListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;

            var result = await _feedBackService.GetList(req);
            return Success(result);
        }

        public async override Task<ResponseDto<string>> Add(AddOrUpdateFeedBackReq req)
        {

            var taskModel = await _taskDomainService.QueryByID(req.TaskId);
            if (taskModel != null)
            {
                taskModel.ModifierId = UserId;
                taskModel.ModifyTime = DateTime.Now;
                taskModel.QuantityProduced = (taskModel.QuantityProduced.HasValue ? taskModel.QuantityProduced : 0) + req.QuantityFeedBack;
                taskModel.QuantityQuanlify = (taskModel.QuantityQuanlify.HasValue ? taskModel.QuantityQuanlify : 0) + req.QuantityQualified;
                taskModel.QuantityUnquanlify = (taskModel.QuantityUnquanlify.HasValue ? taskModel.QuantityUnquanlify : 0) + req.QuantityUnQuanlified;
                await _taskDomainService.Update(taskModel);
            }

            await base.Add(req);

            return Success();
        }

        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> Commit(CommitFeedBackReq req)
        {
            if (req.Ids != null && req.Ids.Count > 0)
            {
                Dictionary<int, decimal?> dicWorkOrder = new Dictionary<int, decimal?>(); //用来记录需要刷新的workOrder已生产数量
                Dictionary<int, Model.Entites.Mes.WorkTask> dicTask = new Dictionary<int, Model.Entites.Mes.WorkTask>(); //用来记录需要刷新的Task表中的数量
                List<FeedBack> updateFeedBackList = new List<FeedBack>();

                foreach (var id in req.Ids)
                {
                    var entity = await _domainService.QueryByID(id);
                    if (entity == null)
                    {
                        continue;
                    }

                    if (!string.IsNullOrEmpty(entity.FeedBackStatus) && !entity.FeedBackStatus.ToUpper().Equals(CommitStatus.DRAFT.ToString()))
                    {
                        return Fail("完成状态初始值不是DRAFT!");
                    }

                    if (string.IsNullOrEmpty(req.FeedBackStatus) || !req.FeedBackStatus.ToUpper().Equals(CommitStatus.COMMITED.ToString()))
                    {
                        return Fail("FeedBackStatus未填写或填写错误!");
                    }

                    entity.FeedBackStatus = req.FeedBackStatus.ToUpper();
                    entity.ModifierId = UserId;
                    entity.ModifyTime = DateTime.Now;

                    updateFeedBackList.Add(entity);

                    //更新Task表
                    //if (entity.TaskId != null)
                    //{
                    //    bool isExist = await _taskDomainService.IsExistAsync(p => p.Id == entity.TaskId);
                    //    if (isExist)
                    //    {
                    //        if (dicTask.ContainsKey((int)entity.TaskId))
                    //        {
                    //            dicTask[(int)entity.TaskId].QuantityProduced += entity.QuantityFeedBack;
                    //            dicTask[(int)entity.TaskId].QuantityQuanlify += entity.QuantityQualified;
                    //            dicTask[(int)entity.TaskId].QuantityUnquanlify += entity.QuantityUnQuanlified;
                    //        }
                    //        else
                    //        {
                    //            Model.Entites.Mes.Task value = new Model.Entites.Mes.Task();
                    //            value.QuantityProduced = entity.QuantityFeedBack;
                    //            value.QuantityQuanlify = entity.QuantityQualified;
                    //            value.QuantityUnquanlify = entity.QuantityUnQuanlified;
                    //            dicTask.Add((int)entity.TaskId, value);
                    //        }
                    //    }
                    //}

                    //更新workOrder表
                    if (entity.WorkOrderId != null && !string.IsNullOrEmpty(entity.KeyFlag) && entity.KeyFlag.Equals("1"))
                    {
                        bool isExist = await _workOrderDomainService.IsExistAsync(p => p.Id == entity.WorkOrderId);
                        if (isExist)
                        {
                            //todo，回写工单时，乘以层数
                            if (dicWorkOrder.ContainsKey((int)entity.WorkOrderId))
                            {
                                dicWorkOrder[(int)entity.WorkOrderId] += entity.QuantityFeedBack * entity.PanelCount;
                            }
                            else
                            {
                                dicWorkOrder.Add((int)entity.WorkOrderId, entity.QuantityFeedBack * entity.PanelCount);
                            }
                        }
                    }
                }

                await _domainService.BulkUpdate(updateFeedBackList);

                //更新task表
                //if (dicTask.Count > 0)
                //{
                //    List<Model.Entites.Mes.Task> updateTaskList = new List<Model.Entites.Mes.Task>();
                //    foreach (var item in dicTask)
                //    {
                //        var taskModel = await _taskDomainService.QueryByID(item.Key);
                //        if (taskModel != null)
                //        {
                //            taskModel.ModifierId = UserId;
                //            taskModel.ModifyTime = DateTime.Now;
                //            taskModel.QuantityProduced += item.Value.QuantityProduced;
                //            taskModel.QuantityQuanlify += item.Value.QuantityQuanlify;
                //            taskModel.QuantityUnquanlify += item.Value.QuantityUnquanlify;

                //            updateTaskList.Add(taskModel);
                //        }
                //    }

                //    await _taskDomainService.BulkUpdate(updateTaskList);
                //}

                //更新workOrder表
                if (dicWorkOrder.Count > 0)
                {
                    List<WorkOrder> updateWorkOrderList = new List<WorkOrder>();
                    foreach (var item in dicWorkOrder)
                    {
                        var workModel = await _workOrderDomainService.QueryByID(item.Key);
                        if (workModel != null)
                        {
                            workModel.ModifyTime = DateTime.Now;
                            workModel.ModifierId = UserId;
                            workModel.QuantityProduced += item.Value;

                            updateWorkOrderList.Add(workModel);
                        }
                    }

                    await _workOrderDomainService.BulkUpdate(updateWorkOrderList);
                }
            }

            return Success();
        }

        /// <summary>
        /// 撤销提交
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> RevokeCommit(CommitFeedBackReq req)
        {
            if (req.Ids != null && req.Ids.Count > 0)
            {
                Dictionary<int, decimal?> dicWorkOrder = new Dictionary<int, decimal?>(); //用来记录需要刷新的workOrder已生产数量
                Dictionary<int, Model.Entites.Mes.WorkTask> dicTask = new Dictionary<int, Model.Entites.Mes.WorkTask>(); //用来记录需要刷新的Task表中的数量
                List<FeedBack> updateFeedBackList = new List<FeedBack>();

                foreach (var id in req.Ids)
                {
                    var entity = await _domainService.QueryByID(id);
                    if (entity == null)
                    {
                        continue;
                    }

                    if (!string.IsNullOrEmpty(entity.FeedBackStatus) && !entity.FeedBackStatus.ToUpper().Equals(CommitStatus.COMMITED.ToString()))
                    {
                        return Fail("存在TaskStatus初始值不是COMMITED !");
                    }

                    if (string.IsNullOrEmpty(req.FeedBackStatus) || !req.FeedBackStatus.ToUpper().Equals(CommitStatus.DRAFT.ToString()))
                    {
                        return Fail("FeedBackStatus未填写或填写错误!");
                    }

                    entity.FeedBackStatus = req.FeedBackStatus.ToUpper();
                    entity.ModifierId = UserId;
                    entity.ModifyTime = DateTime.Now;

                    updateFeedBackList.Add(entity);

                    //更新Task表
                    //if (entity.TaskId != null)
                    //{
                    //    bool isExist = await _taskDomainService.IsExistAsync(p => p.Id == entity.TaskId);
                    //    if (isExist)
                    //    {
                    //        if (dicTask.ContainsKey((int)entity.TaskId))
                    //        {
                    //            dicTask[(int)entity.TaskId].QuantityProduced += entity.QuantityFeedBack;
                    //            dicTask[(int)entity.TaskId].QuantityQuanlify += entity.QuantityQualified;
                    //            dicTask[(int)entity.TaskId].QuantityUnquanlify += entity.QuantityUnQuanlified;
                    //        }
                    //        else
                    //        {
                    //            Model.Entites.Mes.Task value = new Model.Entites.Mes.Task();
                    //            value.QuantityProduced = entity.QuantityFeedBack;
                    //            value.QuantityQuanlify = entity.QuantityQualified;
                    //            value.QuantityUnquanlify = entity.QuantityUnQuanlified;
                    //            dicTask.Add((int)entity.TaskId, value);
                    //        }
                    //    }
                    //}

                    //更新workOrder表
                    if (entity.WorkOrderId != null && !string.IsNullOrEmpty(entity.KeyFlag) && entity.KeyFlag.Equals("1"))
                    {
                        bool isExist = await _workOrderDomainService.IsExistAsync(p => p.Id == entity.WorkOrderId);
                        if (isExist)
                        {
                            //todo，回写工单时，乘以层数
                            if (dicWorkOrder.ContainsKey((int)entity.WorkOrderId))
                            {
                                dicWorkOrder[(int)entity.WorkOrderId] += entity.QuantityFeedBack * entity.PanelCount;
                            }
                            else
                            {
                                dicWorkOrder.Add((int)entity.WorkOrderId, entity.QuantityFeedBack * entity.PanelCount);
                            }
                        }
                    }
                }

                await _domainService.BulkUpdate(updateFeedBackList);

                //更新task表
                //if (dicTask.Count > 0)
                //{
                //    List<Model.Entites.Mes.Task> updateTaskList = new List<Model.Entites.Mes.Task>();
                //    foreach (var item in dicTask)
                //    {
                //        var taskModel = await _taskDomainService.QueryByID(item.Key);
                //        if (taskModel != null)
                //        {
                //            taskModel.ModifierId = UserId;
                //            taskModel.ModifyTime = DateTime.Now;
                //            if (taskModel.QuantityProduced > item.Value.QuantityProduced)
                //            {
                //                taskModel.QuantityProduced -= item.Value.QuantityProduced;
                //            }
                //            else
                //            {
                //                taskModel.QuantityProduced = 0;
                //            }

                //            if (taskModel.QuantityQuanlify > item.Value.QuantityQuanlify)
                //            {
                //                taskModel.QuantityQuanlify -= item.Value.QuantityQuanlify;
                //            }
                //            else
                //            {
                //                taskModel.QuantityQuanlify = 0;
                //            }

                //            if (taskModel.QuantityUnquanlify > item.Value.QuantityUnquanlify)
                //            {
                //                taskModel.QuantityUnquanlify -= item.Value.QuantityUnquanlify;
                //            }
                //            else
                //            {
                //                taskModel.QuantityUnquanlify = 0;
                //            }

                //            updateTaskList.Add(taskModel);
                //        }
                //    }

                //    await _taskDomainService.BulkUpdate(updateTaskList);
                //}

                //更新workOrder表
                //工单表中的数量，是单片数量
                if (dicWorkOrder.Count > 0)
                {
                    List<WorkOrder> updateWorkOrderList = new List<WorkOrder>();
                    foreach (var item in dicWorkOrder)
                    {
                        var workModel = await _workOrderDomainService.QueryByID(item.Key);
                        if (workModel != null)
                        {
                            workModel.ModifyTime = DateTime.Now;
                            workModel.ModifierId = UserId;
                            if (workModel.QuantityProduced > item.Value)
                            {
                                workModel.QuantityProduced -= item.Value;
                            }
                            else
                            {
                                workModel.QuantityProduced = 0;
                            }

                            updateWorkOrderList.Add(workModel);
                        }
                    }

                    await _workOrderDomainService.BulkUpdate(updateWorkOrderList);
                }
            }

            return Success();
        }

        /// <summary>
        /// 删除信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> DeleteData(long id)
        {
            var entity = await _domainService.QueryByID(id);
            if (entity == null)
            {
                return Fail("信息不存在!");
            }

            if (!string.IsNullOrEmpty(entity.FeedBackStatus)
                && entity.FeedBackStatus.ToUpper().Equals(CommitStatus.COMMITED.ToString()))
            {
                return Fail("已提交审批后，不允许删除!");
            }

            var result = await _domainService.DeleteById(id);
            if (result)
            {
                return Success("");
            }
            return Fail("删除失败");
        }

        /// <summary>
        /// 删除信息集合
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> DeleteDataList(List<long> idList)
        {
            if (idList != null)
            {
                object[] deleteList = new object[idList.Count];
                for (int i = 0; i < idList.Count; i++)
                {
                    var entity = await _domainService.QueryByID(idList[i]);
                    if (entity == null)
                    {
                        continue;
                    }

                    if (!string.IsNullOrEmpty(entity.FeedBackStatus)
                        && entity.FeedBackStatus.ToUpper().Equals(CommitStatus.COMMITED.ToString()))
                    {
                        return Fail(idList[i] + " 已提交审批后，不允许删除!");
                    }
                    deleteList[i] = idList[i];
                }

                var result = await _domainService.DeleteByIds(deleteList);
                if (result)
                {
                    return Success("");
                }
            }
            return Fail("删除失败");

        }

        /// <summary>
        /// 获取生产任务信息
        /// 过滤掉草稿状态
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<TaskDto>>> GetTaskList(GetTaskListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;

            var result = await _feedBackService.GetTaskList(req);
            return Success(result);
        }
    }
}
