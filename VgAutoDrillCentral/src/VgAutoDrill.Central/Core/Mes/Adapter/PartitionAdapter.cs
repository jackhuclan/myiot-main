using System.Text.Json;
using AutoMapper;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.ViewModels.Mes.AgvRest;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Partition;
using VgAutoDrill.Central.Core.Mes.Interface;

namespace VgAutoDrill.Central.Core.Mes.Adapter;

public class PartitionAdapter : IPartitionAdapter
{
    private readonly IAgvRestService _agvRestService;
    private readonly ILogger<PartitionAdapter> _logger;
    private readonly IMapper _mapper;
    private readonly IPartitionService _partitionService;

    public PartitionAdapter(IPartitionService partitionService,
        IAgvRestService agvRestService,
        ILogger<PartitionAdapter> logger,
        IMapper mapper,
        IRackService rackService)
    {
        _partitionService = partitionService;
        _agvRestService = agvRestService;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<List<PartitionDto>> GetPartitions()
    {
        var response = await _partitionService.GetList(new GetPartitionListReq { PageSize = int.MaxValue });
        return response.Data.List;
    }

    public async Task HandleBookRest(string restCode, string prebookAgv)
    {
        await _agvRestService.BookAgvRest(restCode, prebookAgv);
    }

    public async Task HandleUnbookRest(string restCode)
    {
        await _agvRestService.UnBookAgvRest(restCode);
    }

    public async Task RefreshCurrentAgv(string carDeviceId, string carCurrentPos)
    {
        var newRestFound = await _agvRestService.GetList(new GetRestCodeListReq { Point = carCurrentPos });
        if (newRestFound != null && newRestFound.Data != null && newRestFound.Data.List.Any())
        {
            var rest = newRestFound.Data.List.FirstOrDefault();
            var updateByPoint = _mapper.Map<AddOrUpdateRestCodeReq>(rest);
            updateByPoint.CurrentAgv = carDeviceId;
            var response = await _agvRestService.UpdateData(updateByPoint);
            _logger.LogInformation($"update rest 当前点位,{JsonSerializer.Serialize(updateByPoint)},response,{response.Message}");
        }

        var originalRestFound = await _agvRestService.GetList(new GetRestCodeListReq { CurrentAgv = carDeviceId });
        if (originalRestFound != null && originalRestFound.Data != null && originalRestFound.Data.List.Any())
        {
            foreach (var original in originalRestFound.Data.List.Where(x => !string.IsNullOrEmpty(x.Point) && x.Point.ToLower() != carCurrentPos.ToLower()))
            {
                var update = _mapper.Map<AddOrUpdateRestCodeReq>(original);
                update.CurrentAgv = string.Empty;
                update.PreBookTime = null;
                update.PreBookAgv = string.Empty;
                var response = await _agvRestService.UpdateData(update);
            }
        }
    }

    public async Task<bool> TryUnBookPartition(string partitionCode, string deviceId)
    {
        var resMsg = await _partitionService.UnBookPart(partitionCode);
        if (string.IsNullOrEmpty(resMsg))
        {
            //_logger.LogInformation($"UndoBookPart success(partitionCode:{partitionCode},deviceId:{deviceId}).");
            return true;
        }

        //_logger.LogError($"UndoBookPart failed(partitionCode:{partitionCode},deviceId:{deviceId}),response message:{resMsg}.");
        return false;
    }

    public async Task UnbookPart(string partitionCode)
    {
        await _partitionService.UnBookPart(partitionCode);
    }
}
