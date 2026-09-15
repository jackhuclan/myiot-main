using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Common.Extensions;
using VgAutoDrill.Admin.Common.Util;
using VgAutoDrill.Admin.Model.Enum;
using VgAutoDrill.Admin.Model.ViewModels.Mes.External.JingWangRequest;
using VgAutoDrill.Admin.Model.ViewModels.Mes.External.JingwangResponse;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig;

namespace VgAutoDrill.Admin.Application.Services.MesServices.External
{
    public class ExternalGetStockInfo : BaseService, IExternalGetStockInfo
    {
        private readonly ILogger<ExternalGetStockInfo> _logger;
        private readonly ISysConfigManager _sysConfigManager;
        private readonly IAPIHelper _apiHelper;
        public static List<string> NoTargetAreas = new List<string>();

        public ExternalGetStockInfo(ILoggerFactory loggerFactory, IAPIHelper apiHelper, ISysConfigManager sysConfigManager)
        {
            _logger = loggerFactory.CreateLogger<ExternalGetStockInfo>();
            _sysConfigManager = sysConfigManager;
            _apiHelper = apiHelper;
        }

        private async Task<List<StockInfoQueryDataDto>> GetDrillStockInfoByConfig()
        {
            var urlAdress = await _sysConfigManager.GetStringValue(MESConfigConstants.URL_KINWONG_STOCK_INFO_QUERY, SysConfigCategoryEnum.Kinwong, false);
            if (string.IsNullOrEmpty(urlAdress))
            {
                return new List<StockInfoQueryDataDto>();
            }

            var targetPosArea = await _sysConfigManager.GetStringValue(MESConfigConstants.STOCK_INFO_QUERY_TARGET_POSAREA, SysConfigCategoryEnum.Kinwong, false);
            if (string.IsNullOrEmpty(targetPosArea))
            {
                return new List<StockInfoQueryDataDto>();
            }

            var drillAreaCode = await _sysConfigManager.GetStringValue(MESConfigConstants.STOCK_INFO_QUERY_DRILLAREA_CODE, SysConfigCategoryEnum.Kinwong, false);
            if (string.IsNullOrEmpty(drillAreaCode))
            {
                return new List<StockInfoQueryDataDto>();
            }

            string targetPosAreaReq = drillAreaCode + targetPosArea;

            var returnData = await GetStockInfos(urlAdress, targetPosAreaReq);

            return returnData;
        }

        public async Task<List<StockInfoQueryDataDto>> GetStockInfos(string urlAdress, string targetPosArea)
        {
            List<StockInfoQueryDataDto> resultDtos = new List<StockInfoQueryDataDto>();
            if (string.IsNullOrEmpty(urlAdress) || string.IsNullOrEmpty(targetPosArea))
            {
                return resultDtos;
            }

            if (targetPosArea.Contains("${02}"))
            {
                targetPosArea = targetPosArea.Replace("-", "_");
            }
            else if (targetPosArea.Contains("${04}"))
            {
                targetPosArea = targetPosArea.Replace("_", "-");
            }

            StockInfoQueryReq jWangRequest = new StockInfoQueryReq
            {
                ReqCode = Guid.NewGuid().ToString(),
                TargetPosArea = targetPosArea,
            };
            string reqStr = JsonSerializer.Serialize(jWangRequest);

            try
            {
                var result = _apiHelper.RequestData(urlAdress, "post", reqStr);
                if (string.IsNullOrEmpty(result))
                {
                    _logger.LogError($"StockInfoQuery api {urlAdress},reqStr {reqStr}, return empty ");
                    return resultDtos;
                }
                _logger.LogInformation($"StockInfoQuery api {urlAdress}, request:{reqStr}, result:{result}");

                var jobject = JObject.Parse(result);
                if (jobject.ContainsKey("code") && jobject["code"].ToString() != "0")
                {
                    string msg = jobject["message"].ToString();
                    if (msg.Contains("传入区域不存在"))
                    {
                        NoTargetAreas.Add(targetPosArea);
                    }
                    return resultDtos;
                }

                var stockInfoQueryDto = JsonSerializer.Deserialize<StockInfoQueryDto>(result);
                if (stockInfoQueryDto == null)
                {
                    _logger.LogError($"StockInfoQuery api {urlAdress}, analytical null {result}");
                    return resultDtos;
                }

                if (stockInfoQueryDto.code != "0")
                {
                    _logger.LogError($"StockInfoQuery api {urlAdress}, return Code {stockInfoQueryDto.code}");
                    return resultDtos;
                }

                if (stockInfoQueryDto.data == null || stockInfoQueryDto.data.Count == 0)
                {
                    _logger.LogError($"StockInfoQuery api {urlAdress}, data null {result}");
                    return resultDtos;
                }

                resultDtos.AddRange(stockInfoQueryDto.data);
                await Task.Delay(500);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"StockInfoQuery api {urlAdress} Error");
            }

            return resultDtos;
        }

        public async Task<int> GetStockQuantity(List<StockInfoQueryDataDto> stockInfoQueryDatas, string sourceCode)
        {
            if (string.IsNullOrEmpty(sourceCode) || stockInfoQueryDatas == null || stockInfoQueryDatas.Count == 0)
            {
                return 0;
            }

            var containsLotDatas = stockInfoQueryDatas.Where(p => !string.IsNullOrEmpty(p.lot) &&
            p.lot.ToLower().StartsWith(sourceCode.ToLower())).ToList();

            if (containsLotDatas == null || containsLotDatas.Count == 0)
            {
                return 0;
            }

            var sumQty = containsLotDatas.Where(p => p.qty != null && p.qty > 0).Select(p => p.qty).Sum();

            return sumQty.ToInt();
        }

        public async Task<int> GetStockQuantityByLot(string itemCode)
        {
            var stockInfosByConfig = await GetDrillStockInfoByConfig();
            if (stockInfosByConfig == null || stockInfosByConfig.Count == 0)
            {
                return 0;
            }

            var containsLotDatas = stockInfosByConfig.Where(p => !string.IsNullOrEmpty(p.lot) &&
            p.lot.ToLower().StartsWith(itemCode.ToLower())).ToList();

            if (containsLotDatas == null || containsLotDatas.Count == 0)
            {
                return 0;
            }

            var sumQty = containsLotDatas.Where(p => p.qty != null && p.qty > 0).Select(p => p.qty).Sum();

            return sumQty.ToInt();
        }
    }
}
