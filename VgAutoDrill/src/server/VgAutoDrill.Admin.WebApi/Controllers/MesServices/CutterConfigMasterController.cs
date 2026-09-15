using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.CutterConfigMaster;
using VgAutoDrill.Infrastructure.Authentication;

namespace VgAutoDrill.Admin.WebApi.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class CutterConfigMasterController : BaseController
    {
        private readonly ICutterConfigMasterService _mainService;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger _logger;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainService"></param>
        /// <param name="environment"></param>
        public CutterConfigMasterController(ICutterConfigMasterService mainService, IWebHostEnvironment environment, ILoggerFactory loggerFactory)
        {
            _mainService = mainService;
            _environment = environment;
            _logger = loggerFactory.CreateLogger<CutterConfigMasterController>();
        }

        /// <summary>
        /// 获取钻孔刀具参数列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetList")]
        [ProducesResponseType(typeof(ResponseDto<List<CutterConfigMasterDto>>), 200)]
        [PermissionAuthorize("material:cutterConfigMaster:list")]
        public async Task<ActionResult> GetList([FromBody] GetCutterConfigMasterListReq req)
        {
            var result = await _mainService.GetList(req);
            return Ok(result);
        }
        /// <summary>
        ///获取钻孔刀具参数
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseDto<CutterConfigMasterDto>), 200)]
        [PermissionAuthorize("material:cutterConfigMaster:view")]
        public async Task<ActionResult> Get(long id)
        {
            var result = await _mainService.QueryByID(id);
            return Ok(result);
        }
        /// <summary>
        /// 添加钻孔刀具参数
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("material:cutterConfigMaster:add")]
        public async Task<ActionResult> Post([FromBody] AddOrUpdateCutterConfigMasterReq req)
        {
            var result = await _mainService.Add(req);
            return Ok(result);
        }
        /// <summary>
        /// 修改钻孔刀具参数
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(ResponseDto<CutterConfigMasterDto>), 200)]
        [PermissionAuthorize("material:cutterConfigMaster:edit")]
        public async Task<ActionResult> Put([FromBody] AddOrUpdateCutterConfigMasterReq req)
        {
            var result = await _mainService.Update(req);
            return Ok(result);
        }
        /// <summary>
        /// 删除钻孔刀具参数
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("material:cutterConfigMaster:remove")]
        public async Task<ActionResult> Delete(long id)
        {
            var result = await _mainService.Delete(id);
            return Ok(result);
        }
        /// <summary>
        /// 删除钻孔刀具参数集合
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("material:cutterConfigMaster:remove")]
        public async Task<ActionResult> DeleteList([FromBody] object[] idList)
        {
            var result = await _mainService.DeleteList(idList);
            return Ok(result);
        }

        /// <summary>
        /// 上传刀具参数文件到服务器
        /// </summary>
        /// <param name="objFile"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpLoad")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("material:cutterConfigMaster:upload")]
        public ActionResult UpLoad([FromForm(Name = "file")] IFormFile objFile)
        {
            string resultPath;
            try
            {
                if (objFile == null)
                {
                    return BadRequest();
                }
                string filePath = Path.Combine(_environment.ContentRootPath, "data", "dia");
                _logger.LogInformation(filePath);
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
        /// 下载刀具参数文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("DownLoad")]
        [ProducesResponseType(typeof(FileResult), 200)]
        [PermissionAuthorize("material:cutterConfigMaster:download")]
        public ActionResult DownLoad(string fileName)
        {
            string filePath = Path.Combine(_environment.ContentRootPath, "data", "dia", fileName);
            if (!System.IO.File.Exists(filePath))
            {
                return BadRequest();
            }

            FileStream fileStream = new FileStream(filePath, FileMode.Open);
            return File(fileStream, "application/octet-stream", fileDownloadName: fileName);
        }
    }
}
