using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

namespace VgDrillFileHub.Controllers;


[ApiController]
[Route("api/v1/drill")]
public class FileController : ControllerBase
{
    private readonly ILogger<FileController> _logger;
    private readonly IFileProvider _fileProvider;

    public FileController(ILogger<FileController> logger, IWebHostEnvironment webHostEnvironment)
    {
        _logger = logger;
        _fileProvider = webHostEnvironment.ContentRootFileProvider;
    }

    [HttpGet("file", Name = "FileList")]
    public OkObjectResult FileList(string code)
    {
        IDirectoryContents directories = _fileProvider.GetDirectoryContents("");//1. 获取到DrillFilesRoot所有的目录和文件，根据料号和文件的映射规则返回对应的文件
        //2.根据code，找到合适的文件
        var q = from f in directories.Where(d => d.Name.StartsWith(code, StringComparison.OrdinalIgnoreCase))
                select new
                {
                    f.PhysicalPath,
                    f.Name,
                };

        return Ok(q.ToList());
    }
}
