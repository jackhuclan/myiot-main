using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Common.Util;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.FeedBack;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProTask;
using VgAutoDrill.Infrastructure.Authentication;

namespace VgAutoDrill.Admin.WebApi.Controllers.MesServices
{
    /// <summary>
    /// 生产报工记录
    /// </summary>
    public class FeedBackController : BaseController
    {
        private readonly IFeedBackService _mainService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainService"></param>
        public FeedBackController(IFeedBackService mainService)
        {
            _mainService = mainService;
        }

        /// <summary>
        /// 获取生产报工记录列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetList")]
        [ProducesResponseType(typeof(ResponseDto<List<FeedBackDto>>), 200)]
        [PermissionAuthorize("produce:feedback:list")]
        public async Task<ActionResult> GetList([FromBody] GetFeedBackListReq req)
        {
            var result = await _mainService.GetEquipmentList(req);
            return Ok(result);
        }
        /// <summary>
        ///获取生产报工记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseDto<FeedBackDto>), 200)]
        [PermissionAuthorize("produce:feedback:view")]
        public async Task<ActionResult> Get(long id)
        {
            var result = await _mainService.QueryByID(id);
            return Ok(result);
        }
        /// <summary>
        /// 添加生产报工记录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("produce:feedback:add")]
        public async Task<ActionResult> Post([FromBody] AddOrUpdateFeedBackReq req)
        {
            var result = await _mainService.Add(req);
            return Ok(result);
        }
        /// <summary>
        /// 修改生产报工记录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(ResponseDto<FeedBackDto>), 200)]
        [PermissionAuthorize("produce:feedback:edit")]
        public async Task<ActionResult> Put([FromBody] AddOrUpdateFeedBackReq req)
        {
            var result = await _mainService.Update(req);
            return Ok(result);
        }
        /// <summary>
        /// 删除生产报工记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("produce:feedback:remove")]
        public async Task<ActionResult> Delete(long id)
        {
            var result = await _mainService.DeleteData(id);
            return Ok(result);
        }
        /// <summary>
        /// 删除生产报工记录集合
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("produce:feedback:remove")]
        public async Task<ActionResult> DeleteList([FromBody] List<long> idList)
        {
            var result = await _mainService.DeleteDataList(idList);
            return Ok(result);
        }
        /// <summary>
        /// 提交生产报工记录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("Commit")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("produce:feedback:commit")]
        public async Task<ActionResult> Commit([FromBody] CommitFeedBackReq req)
        {
            var result = await _mainService.Commit(req);
            return Ok(result);
        }

        /// <summary>
        /// 撤销提交生产报工记录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("RevokeCommit")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("produce:feedback:revokeCommit")]
        public async Task<ActionResult> RevokeCommit([FromBody] CommitFeedBackReq req)
        {
            var result = await _mainService.RevokeCommit(req);
            return Ok(result);
        }

        /// <summary>
        /// 获取生产任务列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetTaskList")]
        [ProducesResponseType(typeof(ResponseDto<List<TaskDto>>), 200)]
        [PermissionAuthorize("produce:feedback:list")]
        public async Task<ActionResult> GetTaskList([FromBody] GetTaskListReq req)
        {
            var result = await _mainService.GetTaskList(req);
            return Ok(result);
        }

        /// <summary>
        /// 导出生产报工Excel
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("DownLoadList")]
        [ProducesResponseType(typeof(FileResult), 200)]
        [PermissionAuthorize("produce:feedback:download")]
        public async Task<ActionResult> DownLoadListAsync()
        {
            List<FeedBackToExcelDto> list = new List<FeedBackToExcelDto>();

            var result = await _mainService.GetList(new GetFeedBackListReq
            {
                PageNum = 1,
                PageSize = int.MaxValue
            });

            if (result != null && result.Data.List != null)
            {
                foreach (var item in result.Data.List)
                {
                    FeedBackToExcelDto model = new FeedBackToExcelDto();
                    model.FeedBackChannel = item.FeedBackChannel;
                    model.FeedBackStatus = item.FeedBackStatus;
                    model.FeedBackTime = item.FeedBackTime;
                    model.FeedBackType = item.FeedBackType;
                    model.ItemCode = item.ItemCode;
                    model.ItemId = item.ItemId;
                    model.ItemName = item.ItemName;
                    model.ItemTypeId = item.ItemTypeId;
                    model.KeyFlag = item.KeyFlag;
                    model.NickName = item.NickName;
                    model.ProcessCode = item.ProcessCode;
                    model.ProcessId = item.ProcessId;
                    model.ProcessName = item.ProcessName;
                    model.Quantity = item.Quantity;
                    model.QuantityFeedBack = item.QuantityFeedBack;
                    model.QuantityQualified = item.QuantityQualified;
                    model.QuantityUnQuanlified = item.QuantityUnQuanlified;
                    model.Remark = item.Remark;
                    model.Specification = item.Specification;
                    model.TaskCode = item.TaskCode;
                    model.TaskId = item.TaskId;
                    model.UserName = item.UserName;
                    model.UnitOfMeasure = item.UnitOfMeasure;
                    model.WorkOrderId = item.WorkOrderId;
                    model.WorkOrderName = item.WorkOrderName;
                    model.WorkOrderCode = item.WorkOrderCode;
                    model.WorkStationCode = item.WorkStationCode;
                    model.WorkStationId = item.WorkStationId;
                    model.WorkStationName = item.WorkStationName;

                    list.Add(model);
                }
            }

            //List转为Excel文件
            var fileStream = ExcelHelper.ParseListToExcel(list);
            return File(fileStream.ToArray(), "application/vnd.ms-excel", "生产报工.xlsx");
        }
    }
}
