using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using VgAutoDrill.Central.Core.Calculator;
using VgAutoDrill.Fundation.Iot.Transportation;
using VgAutoDrill.Fundation.Utils;
using VgAutoDrill.Infrastructure;
using VgAutoDrill.OpenAPI;

namespace VegaIot.External.StdAgv;

public class StdCalcutorUseAPI : ICalcutorUseAPI
{
    private readonly ILogger<StdCalcutorUseAPI> _logger;
    private readonly IHttpRequestInvoker _httpRequestInvoker;
    private readonly StdAgvConfig _stdAgvConfig;

    public StdCalcutorUseAPI(IServiceProvider serviceProvider, IObjectFactory objectFactory)
    {
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        _logger = loggerFactory.CreateLogger<StdCalcutorUseAPI>();
        _stdAgvConfig = serviceProvider.GetRequiredService<StdAgvConfig>();
        _httpRequestInvoker = serviceProvider.GetRequiredService<IHttpRequestInvoker>();
    }

    public async Task<bool> IsHaveEmptyLocation(TransportationKind transportationKind)
    {
        string areaCode = await GetArea(transportationKind);

        try
        {
            var _getAreaEmptyPosUrl = await _stdAgvConfig.GetAreaEmptyPos();

            var request = new QueryEmptyPos()
            {
                reqCode = $"Vega_2.5_{DateTime.Now.Ticks.ToString()}",
                targetPosArea = areaCode
            };

            _logger.LogInformation($"IsHaveEmptyLocation:area :{areaCode},transportationKind:{transportationKind} ,request:{request.ToJson()}");
            var response = await _httpRequestInvoker.PostAsJsonAsync<QueryEmptyPos?, QueryEmptyPosRes>(_getAreaEmptyPosUrl, request);
            _logger.LogInformation($"IsHaveEmptyLocation:{areaCode},response:{response.ToJson()}");

            return response != null && response.code == 0 && response.data.Any();
        }
        catch (Exception ex)
        {
            _logger.LogError($"IsHaveEmptyLocation:area :{areaCode},Error:{ex.Message}", ex);
        }

        return false;
    }

    public async Task<string> GetArea(TransportationKind transportationKind)
    {
        string area = string.Empty;
        switch (transportationKind)
        {
            case TransportationKind.EmptySilo:
                area = await _stdAgvConfig.GetEmptyStore();
                break;

            case TransportationKind.Raw:
                area = await _stdAgvConfig.GetLineStore();
                break;

            case TransportationKind.Clinker:
                area = await _stdAgvConfig.GetClinkerStore();
                break;

            default:
                area = "NotAvalibaleArea";
                break;
        }

        return area;
    }

    public async Task<bool> IsHaveRawStock(TransportationKind transportationKind, string lot, string pnlStatus)
    {

        string areaCode = await GetArea(transportationKind);

        try
        {
            var _getCheckLotUrl = await _stdAgvConfig.GetCheckLot();

            var request = new QueryCheckLotReq()
            {
                reqCode = $"Vega_Clot_{DateTime.Now.Ticks.ToString()}",
                targetPosArea = areaCode,
                lot = lot,
                pnlStatus = pnlStatus
            };

            _logger.LogInformation($"IsHaveRawStock:area :{areaCode},transportationKind:{transportationKind} ,request:{request.ToJson()}");
            var response = await _httpRequestInvoker.PostAsJsonAsync<QueryCheckLotReq?, QueryCheckLotRes>(_getCheckLotUrl, request);
            _logger.LogInformation($"IsHaveRawStock: area: {areaCode},response:{response.ToJson()}");

            return response != null && response.code == 0 && response.data;
        }
        catch (Exception ex)
        {
            _logger.LogError($"IsHaveRawStock:area :{areaCode},Error:{ex.Message}", ex);
        }

        return false;
    }
    public async Task<bool> IsHaveEmptySilo(TransportationKind transportationKind)
    {
        string areaCode = await GetArea(transportationKind);

        try
        {
            var _getCheckSiloUrl = await _stdAgvConfig.GetCheckEmptySilo();

            var request = new QueryEmptyPos()
            {
                reqCode = $"Vega_Csilo_{DateTime.Now.Ticks.ToString()}",
                targetPosArea = areaCode
            };

            _logger.LogInformation($"IsHaveEmptySilo:area :{areaCode},transportationKind:{transportationKind} ,request:{request.ToJson()}");
            var response = await _httpRequestInvoker.PostAsJsonAsync<QueryEmptyPos?, QueryCheckLotRes>(_getCheckSiloUrl, request);
            _logger.LogInformation($"IsHaveEmptySilo: area: {areaCode},response:{response.ToJson()}");

            return response != null && response.code == 0 && response.data;
        }
        catch (Exception ex)
        {
            _logger.LogError($"IsHaveEmptySilo:area :{areaCode},Error:{ex.Message}", ex);
        }

        return false;
    }
}
