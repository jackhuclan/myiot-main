using Microsoft.AspNetCore.Mvc;
using VgAutoDrill.Central.Core.Manager;

namespace VgAutoDrill.Central.WebApi.Controllers.stat;

[ApiController]
[Route("central/stat/partition")]
public class PartitionController : ControllerBase
{
    private readonly IPartitionManager _partitionManager;

    public PartitionController(IPartitionManager partitionManager)
    {
        _partitionManager = partitionManager;
    }

    [HttpGet]
    public object? Index(string? id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return new
            {
                _partitionManager.Partitions,
                _partitionManager.RestPoints,
                _partitionManager.BookedRestPoints,
                _partitionManager.PartitionRelations
            };
        }

        return _partitionManager.Partitions.FirstOrDefault(x => x.PartCode == id);
    }
}
