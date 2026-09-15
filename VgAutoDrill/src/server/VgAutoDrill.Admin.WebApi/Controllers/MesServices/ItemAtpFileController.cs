using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ItemAtpFile;
using VgAutoDrill.Infrastructure.Authentication;

namespace VgAutoDrill.Admin.WebApi.Controllers.MesServices
{
    /// <summary>
    /// ATP文件
    /// </summary>
    public class ItemAtpFileController : BaseController
    {
        private readonly IItemAtpFileService _mainService;
        private readonly IWebHostEnvironment _environment;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainService"></param>
        /// <param name="environment"></param>
        public ItemAtpFileController(IItemAtpFileService mainService, IWebHostEnvironment environment)
        {
            _mainService = mainService;
            _environment = environment;
        }

        /// <summary>
        /// 获取ATP文件列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetList")]
        [ProducesResponseType(typeof(ResponseDto<List<ItemAtpFileDto>>), 200)]
        [PermissionAuthorize("material:atpFile:list")]
        public async Task<ActionResult> GetList([FromBody] GetItemAtpFileListReq req)
        {
            var result = await _mainService.GetEquipmentList(req);
            return Ok(result);
        }
        /// <summary>
        ///获取ATP文件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ResponseDto<ItemAtpFileDto>), 200)]
        [PermissionAuthorize("material:atpFile:view")]
        public async Task<ActionResult> Get(long id)
        {
            var result = await _mainService.QueryByID(id);
            return Ok(result);
        }
        /// <summary>
        /// 添加ATP文件
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("material:atpFile:add")]
        public async Task<ActionResult> Post([FromBody] AddOrUpdateItemAtpFileReq req)
        {
            var result = await _mainService.AddCheck(req);
            return Ok(result);
        }
        /// <summary>
        /// 修改ATP文件
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(ResponseDto<ItemAtpFileDto>), 200)]
        [PermissionAuthorize("material:atpFile:edit")]
        public async Task<ActionResult> Put([FromBody] AddOrUpdateItemAtpFileReq req)
        {
            var result = await _mainService.UpdateCheck(req);
            return Ok(result);
        }
        /// <summary>
        /// 删除ATP文件
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("material:atpFile:remove")]
        public async Task<ActionResult> Delete(long id)
        {
            var result = await _mainService.DeleteData(id);
            return Ok(result);
        }
        /// <summary>
        /// 删除ATP文件集合
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("material:atpFile:remove")]
        public async Task<ActionResult> DeleteList([FromBody] List<long> idList)
        {
            var result = await _mainService.DeleteDataList(idList);
            return Ok(result);
        }

        /// <summary>
        /// 上传ATP文件到服务器
        /// </summary>
        /// <param name="objFile"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpLoad")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("material:atpFile:upload")]
        public ActionResult UpLoad([FromForm(Name = "file")] IFormFile objFile)
        {
            string resultPath;
            try
            {
                if (objFile == null)
                {
                    return BadRequest();
                }
                string filePath = Path.Combine(_environment.ContentRootPath, "data", "atp");
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
        /// 下载ATP文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("DownLoad")]
        [ProducesResponseType(typeof(FileResult), 200)]
        [PermissionAuthorize("material:atpFile:download")]
        public ActionResult DownLoad(string fileName)
        {
            string filePath = Path.Combine(_environment.ContentRootPath, "data", "atp", fileName);
            if (!System.IO.File.Exists(filePath))
            {
                return BadRequest();
            }

            FileStream fileStream = new FileStream(filePath, FileMode.Open);
            return File(fileStream, "application/octet-stream", fileDownloadName: fileName);
        }

        /// <summary>
        /// 生成ATP文件
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GenerateATP")]
        [ProducesResponseType(typeof(ResponseDto<string>), 200)]
        [PermissionAuthorize("material:atpFile:generateATP")]
        public async Task<ActionResult> GenerateATP(long Id)
        {
            string filePath = Path.Combine(_environment.ContentRootPath, "data", "atp");
            var result = await _mainService.GenerateATP(filePath, Id);
            return Ok(result);
        }
    }
}