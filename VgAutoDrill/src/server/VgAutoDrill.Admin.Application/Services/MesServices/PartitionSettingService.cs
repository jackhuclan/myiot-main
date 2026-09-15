using AutoMapper;
using SqlSugar;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.PartitionSetting;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig;

namespace VgAutoDrill.Admin.Application.Services.MesServices
{
    public class PartitionSettingService : BaseServiceWithoutTree<PartitionSetting, PartitionSettingDto, AddOrUpdatePartitionSettingReq>, IPartitionSettingService
    {
        private static List<PartitionSetting>? PartitionSettings { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainService"></param>
        /// <param name="mapper"></param>
        public PartitionSettingService(IPartitionSettingDomainService domainService, IMapper mapper)
            : base(domainService, mapper)
        {
        }
        /// <summary>
        /// 根据编码获取配置项数值
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<ResponseDto<object>> GetDataByCode(long partitionId, string configCode, bool isReadCache = true)
        {
            if (partitionId <= 0)
            {
                return Fail<object>("未识别分区code！");
            }

            if (string.IsNullOrEmpty(configCode))
            {
                return Fail<object>("未识别有效的入参！");
            }

            PartitionSetting data = new PartitionSetting();
            if (!isReadCache)
            {
                if (!await _domainService.IsExistAsync(p => !string.IsNullOrEmpty(p.ConfigCode) && p.ConfigCode.ToLower().Equals(configCode.ToLower())
                                  && partitionId == p.partition_id))
                {
                    return Fail<object>("未查询到数据！");
                }
                data = await _domainService.FindSingleAsync(p => !string.IsNullOrEmpty(p.ConfigCode) && p.ConfigCode.ToLower().Equals(configCode.ToLower())
                                  && partitionId == p.partition_id);
            }
            else
            {
                if (PartitionSettings == null || PartitionSettings.Count == 0)
                {
                    await InitializeSysConfigs();
                }

                if (!PartitionSettings.Exists(p => !string.IsNullOrEmpty(p.ConfigCode) && p.ConfigCode.ToLower().Equals(configCode.ToLower())
                                   && partitionId == p.partition_id))
                {
                    if (!await _domainService.IsExistAsync(p => !string.IsNullOrEmpty(p.ConfigCode) && p.ConfigCode.ToLower().Equals(configCode.ToLower())
                                  && partitionId == p.partition_id))
                    {
                        return Fail<object>("未查询到数据！");
                    }
                    else
                    {
                        await InitializeSysConfigs();
                    }
                }

                data = PartitionSettings.SingleOrDefault(p => !string.IsNullOrEmpty(p.ConfigCode) && p.ConfigCode.ToLower().Equals(configCode.ToLower())
                                   && partitionId == p.partition_id);
            }

            if (data == null)
            {
                return Fail<object>("未查询到数据！");
            }

            if (string.IsNullOrEmpty(data.ConfigType) || string.IsNullOrEmpty(data.ConfigValue))
            {
                return Fail<object>("未识别有效的ConfigType和ConfigValue！");
            }

            ResponseDto<object> result = new ResponseDto<object>();
            result.Code = ResponseCode.Success;

            switch (data.ConfigType.ToLower())
            {
                case "string":
                    var stringResult = data.ConfigValue.ToString();
                    result.Data = stringResult;
                    break;

                case "int":
                    int num = 0;
                    int.TryParse(data.ConfigValue, out num);
                    result.Data = num;
                    break;

                case "bool":
                    if (data.ConfigValue.ToLower().Equals("true") || data.ConfigValue == "1")
                    {
                        result.Data = true;
                    }
                    else
                    {
                        result.Data = false;
                    }
                    break;

                case "enum":
                    var enumResult = new SysConfigEnum();
                    enumResult.ConfigEnumValue = data.ConfigValue;
                    enumResult.ConfigEnumDescript = data.ConfigDescript;
                    result.Data = enumResult;
                    break;
            }

            return result;
        }
        public async Task InitializeSysConfigs(long partitionId = 0)
        {
            PartitionSettings = await _domainService.QueryAsync(p => (partitionId <= 0 || partitionId == p.partition_id) && p.IsDeleted == 0, p => p.Id, OrderByType.Asc);
        }
    }
}
