using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ItemDrillFile;
using VgAutoDrill.Infrastructure.Authentication;

namespace VgAutoDrill.Admin.WebApi.Controllers.MesServices
{
    /// <summary>
    /// 钻带参数
    /// </summary>
    public class ItemDrillFileController : BaseController
    {
        private readonly IItemDrillFileService _mainService;
        private readonly IWebHostEnvironment _environment;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainService"></param>
        /// <param name="environment"></param>
        public ItemDrillFileController(IItemDrillFileService mainService, IWebHostEnvironment environment)
        {
            _mainService = mainService;
            _environment = environment;
        }

        /// <summary>
        /// 获取钻带参数列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetList")]
        [ProducesResponseType(typeof(ResponseDto<List<ItemDrillFileDto>>), 200)]
        [PermissionAuthorize("material:drillFile:list")]
        public async Task<ActionResult> GetList([FromBody] GetItemDrillFileListReq req)
        {
            var result = await _mainService.GetEquipmentList(req);
            return Ok(result);
        }
        /// <summary>
        ///获取钻带参数
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseDto<ItemDrillFileDto>), 200)]
        [PermissionAuthorize("material:drillFile:view")]
        public async Task<ActionResult> Get(long id)
        {
            var result = await _mainService.QueryByID(id);
            return Ok(result);
        }
        /// <summary>
        /// 添加钻带参数
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("material:drillFile:add")]
        public async Task<ActionResult> Post([FromBody] AddOrUpdateItemDrillFileReq req)
        {
            var result = await _mainService.Add(req);
            return Ok(result);
        }
        /// <summary>
        /// 修改钻带参数
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(ResponseDto<ItemDrillFileDto>), 200)]
        [PermissionAuthorize("material:drillFile:edit")]
        public async Task<ActionResult> Put([FromBody] AddOrUpdateItemDrillFileReq req)
        {
            var result = await _mainService.Update(req);
            return Ok(result);
        }
        /// <summary>
        /// 删除钻带参数
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("material:drillFile:remove")]
        public async Task<ActionResult> Delete(long id)
        {
            var result = await _mainService.Delete(id);
            return Ok(result);
        }
        /// <summary>
        /// 删除钻带参数集合
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("material:drillFile:remove")]
        public async Task<ActionResult> DeleteList([FromBody] object[] idList)
        {
            var result = await _mainService.DeleteList(idList);
            return Ok(result);
        }

        /// <summary>
        /// 上传钻带参数文件到服务器
        /// </summary>
        /// <param name="objFile"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpLoad")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("material:drillFile:upload")]
        public ActionResult UpLoad([FromForm(Name = "file")] IFormFile objFile)
        {
            string resultPath;
            try
            {
                if (objFile == null)
                {
                    return BadRequest();
                }
                string filePath = Path.Combine(_environment.ContentRootPath, "data", "drl");
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                resultPath = Path.Combine(filePath, objFile.FileName);

                var stream = objFile.OpenReadStream();
                byte[] bytes = new byte[stream.Length];
                stream.Read(bytes, 0, bytes.Length);
                stream.Seek(0, SeekOrigin.Begin);
                using FileStream fs = new FileStream(Path.Combine(filePath, objFile.FileName), FileMode.Create);
                using BinaryWriter bw = new BinaryWriter(fs);
                bw.Write(bytes);
                bw.Close();
                fs.Close();
            }
            catch
            {
                return BadRequest();
            }
            return Ok(resultPath);
        }

        /// <summary>
        /// 下载钻带参数文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("DownLoad")]
        [ProducesResponseType(typeof(FileResult), 200)]
        [PermissionAuthorize("material:drillFile:download")]
        public ActionResult DownLoad(string fileName)
        {
            string filePath = Path.Combine(_environment.ContentRootPath, "data", "drl", fileName);
            if (!System.IO.File.Exists(filePath))
            {
                return BadRequest();
            }

            FileStream fileStream = new FileStream(filePath, FileMode.Open);
            return File(fileStream, "application/octet-stream", fileDownloadName: fileName);
        }
    }
}
