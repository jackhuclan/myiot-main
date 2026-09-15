using AutoMapper;
using SqlSugar;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Common.Extensions;
using VgAutoDrill.Admin.Domain.Interfaces.MesServices;
using VgAutoDrill.Admin.Model.CentralModels;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.Enum;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceRecords;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceRecordsSummary;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceTemporaryMaintenanceRecords;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DrillRateFactor;
using VgAutoDrill.Admin.Model.ViewModels.Mes.SysConfig;

namespace VgAutoDrill.Admin.Application.Services.MesServices
{
    public class DeviceRecordsService : BaseServiceWithoutTree<DeviceRecords, DeviceRecordsDto, AddOrUpdateDeviceRecordsReq>, IDeviceRecordsService
    {
        private readonly IDeviceRecordsDomainService _deviceRecordsDomainService;
        private readonly IDeviceRecordsSummaryDomainService _deviceRecordsSummaryDomainService;
        private readonly IDeviceTemporaryMaintenanceRecordsDomainService _deviceTemporaryMaintenanceRecordsDomainService;
        private readonly IDeviceRecordsSummaryExtendDomainService _deviceRecordsSummaryExtendDomainService;
        private readonly ISysConfigManager _sysConfigManager;
        private readonly IDrillRateFactorDomainService _drillRateFactorDomainService;
        private readonly ISysConfigService _sysConfigService;

        /// <summary>
        ///
        /// </summary>
        /// <param name="domainService"></param>
        /// <param name="hisDomainService"></param>
        /// <param name="deviceDomainService"></param>
        /// <param name="mapper"></param>
        public DeviceRecordsService(IDeviceRecordsDomainService domainService,
            IDeviceRecordsSummaryDomainService deviceRecordsSummaryDomainService,
            ISysConfigManager sysConfigManager,
            IDeviceTemporaryMaintenanceRecordsDomainService deviceTemporaryMaintenanceRecordsDomainService,
            IDeviceRecordsSummaryExtendDomainService deviceRecordsSummaryExtendDomainService,
            ISysConfigService sysConfigService,
            IDrillRateFactorDomainService drillRateFactorDomainService,
            IMapper mapper)
            : base(domainService, mapper)
        {
            _deviceRecordsDomainService = domainService;
            _sysConfigService = sysConfigService;
            _deviceTemporaryMaintenanceRecordsDomainService = deviceTemporaryMaintenanceRecordsDomainService;
            _deviceRecordsSummaryExtendDomainService = deviceRecordsSummaryExtendDomainService;
            _deviceRecordsSummaryDomainService = deviceRecordsSummaryDomainService;
            _sysConfigManager = sysConfigManager;
            _drillRateFactorDomainService = drillRateFactorDomainService;
        }

        /// <summary>
        /// 获取数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<DeviceRecordsDto>>> GetList(GetDeviceRecordsListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;

            var result = await _deviceRecordsDomainService.GetList(req);
            return Success(result);
        }

        public async Task<ResponseDto<string>> InsertData(AddOrUpdateDeviceRecordsReq req)
        {
            if (req == null)
            {
                return Fail("无法识别有效的入参!");
            }

            var model = _mapper.Map<DeviceRecords>(req);
            model.CreateTime = string.IsNullOrEmpty(req.DateString) ? DateTime.Now : DateTime.Parse(req.DateString);
            model.CreatorId = UserId;
            model.Status = (int)DataStatusEnum.Enable;

            var result = await _deviceRecordsDomainService.Add(model);
            if (!result)
            {
                return Fail("添加失败!");
            }

            return Success();
        }

        public async Task<ResponseDto<bool>> BulkInsert(List<AddOrUpdateDeviceRecordsReq> addList)
        {
            if (addList == null || addList.Count == 0)
            {
                return Success(false);
            }

            List<DeviceRecords> addDatas = new List<DeviceRecords>();
            foreach (var item in addList)
            {
                addDatas.Add(new DeviceRecords
                {
                    DeviceCode = item.DeviceCode,
                    WaitTime = item.WaitTime,
                    WorkTime = item.WorkTime,
                    CollectClearTime = item.CollectClearTime,
                    EndToStartTime = item.EndToStartTime,
                    DateString = item.DateString,
                    Duty = item.Duty,
                    ErrorTime = item.ErrorTime,
                    OpenTime = item.OpenTime,
                    CreateTime = DateTime.Now,
                    CreatorId = UserId,
                });
            }

            var result = await _domainService.BulkInsert(addDatas);
            if (!result)
            {
                return Success(false);
            }

            return Success(true);
        }

        public async Task<List<DeviceRecordsToExcelDto>> GetToExcelList(GetDeviceRecordsListReq req)
        {
            List<DeviceRecordsToExcelDto> returnDtos = new List<DeviceRecordsToExcelDto>();

            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 1000;
            var datas = await GetList(req);
            if (datas == null || datas.Data == null || datas.Data.List == null || datas.Data.List.Count == 0)
            {
                return returnDtos;
            }

            foreach (var item in datas.Data.List)
            {
                returnDtos.Add(new DeviceRecordsToExcelDto
                {
                    DeviceCode = item.DeviceCode,
                    CollectClearTime = item.CollectClearTime,
                    DateString = item.DateString,
                    Duty = item.Duty,
                    ErrorTime = item.ErrorTime,
                    OpenTime = item.OpenTime,
                    EndToStartTime = item.EndToStartTime,
                    WaitTime = item.WaitTime,
                    WorkTime = item.WorkTime,
                });
            }

            return returnDtos;
        }

        /// <summary>
        /// 获取汇总数据列表
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<DeviceRecordsSummaryDto>>> GetSummaryList(GetDeviceRecordsSummaryListReq req)
        {
            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 1) req.PageSize = 10;

            await RefreshSummaryDatas(); // 定时刷新

            var result = await _deviceRecordsSummaryDomainService.GetList(req);
            return Success(result);
        }

        public async Task RefreshSummaryDatas()
        {
            var summaryDate = await _deviceRecordsSummaryDomainService.GetMaxDate();
            if (summaryDate == null)
            {
                summaryDate = await _deviceRecordsDomainService.GetMinDate();
            }
            else
            {
                summaryDate = summaryDate.ToDate().AddDays(-1);
            }

            if (summaryDate == null)
            {
                return;
            }

            int days = await DateDiff((DateTime)summaryDate, DateTime.Now);
            if (days < 1)
            {
                return;
            }

            string morning = string.Empty;
            string middle = string.Empty;
            string night = string.Empty;

            var sailings = await _sysConfigManager.GetStringValue(MESConfigConstants.SAILINGS);
            if (!string.IsNullOrEmpty(sailings) && sailings.Contains("|"))
            {
                var arrSailings = sailings.Split('|');
                if (arrSailings.Length > 1)
                {
                    for (int i = 0; i < arrSailings.Length; i++)
                    {
                        if (arrSailings[i].StartsWith("0-"))
                        {
                            morning = arrSailings[i].Replace("0-", "");
                        }
                        else if (arrSailings[i].StartsWith("1-"))
                        {
                            middle = arrSailings[i].Replace("1-", "");
                        }
                        else if (arrSailings[i].StartsWith("2-"))
                        {
                            night = arrSailings[i].Replace("2-", "");
                        }
                    }
                }

                List<DeviceRecordsSummary> summaryRecords = new List<DeviceRecordsSummary>();

                for (int i = days; i >= 0; i--)
                {
                    DateTime startDate = DateTime.Now.Date.AddDays(-i);
                    DateTime endDate = DateTime.Now.Date.AddDays(-i + 1);
                    int morningTimes = 0;
                    int middleTimes = 0;
                    int nightTimes = 0;

                    if (string.IsNullOrEmpty(morning))
                    {
                        var records = await _deviceRecordsDomainService.GetListByDate(startDate, endDate);
                        if (records == null || records.Count == 0)
                        { continue; }

                        summaryRecords.AddRange(await GetDataBySailings(records, 0, startDate, endDate));
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(middle))
                        {
                            if (string.IsNullOrEmpty(night))
                            {
                                var records = await _deviceRecordsDomainService.GetListByDate(startDate, endDate);
                                if (records?.Count() > 0)
                                {
                                    summaryRecords.AddRange(await GetDataBySailings(records, 0, startDate, endDate));
                                }
                            }
                            else
                            {
                                morningTimes = await GetMinute(morning);
                                nightTimes = await GetMinute(night);

                                DateTime morningSDate = DateTime.Now.Date.AddDays(-i).AddMinutes(morningTimes);
                                DateTime morningEDate = DateTime.Now.Date.AddDays(-i).AddMinutes(nightTimes);
                                DateTime nightEDate = DateTime.Now.Date.AddDays(-i + 1).AddMinutes(morningTimes);

                                var mRecords = await _deviceRecordsDomainService.GetListByDate(morningSDate, morningEDate);
                                if (mRecords?.Count() > 0)
                                {
                                    summaryRecords.AddRange(await GetDataBySailings(mRecords, 0, morningSDate, morningEDate));
                                }
                                var nRecords = await _deviceRecordsDomainService.GetListByDate(morningEDate, nightEDate);
                                if (nRecords?.Count() > 0)
                                {
                                    summaryRecords.AddRange(await GetDataBySailings(nRecords, 2, morningEDate, nightEDate));
                                }
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(night))
                            {
                                morningTimes = await GetMinute(morning);
                                middleTimes = await GetMinute(middle);

                                DateTime morningSDate = DateTime.Now.Date.AddDays(-i).AddMinutes(morningTimes);
                                DateTime morningEDate = DateTime.Now.Date.AddDays(-i).AddMinutes(middleTimes);

                                var mRecords = await _deviceRecordsDomainService.GetListByDate(morningSDate, morningEDate);
                                if (mRecords?.Count() > 0)
                                {
                                    summaryRecords.AddRange(await GetDataBySailings(mRecords, 0, morningSDate, morningEDate));
                                }
                                var middleRecords = await _deviceRecordsDomainService.GetListByDate(morningEDate, endDate);
                                if (middleRecords?.Count() > 0)
                                {
                                    summaryRecords.AddRange(await GetDataBySailings(middleRecords, 1, morningEDate, endDate));
                                }

                            }
                            else
                            {
                                morningTimes = await GetMinute(morning);
                                middleTimes = await GetMinute(middle);
                                nightTimes = await GetMinute(night);

                                DateTime morningSDate = DateTime.Now.Date.AddDays(-i).AddMinutes(morningTimes);
                                DateTime morningEDate = DateTime.Now.Date.AddDays(-i).AddMinutes(middleTimes);
                                DateTime middleEDate = DateTime.Now.Date.AddDays(-i).AddMinutes(nightTimes);
                                DateTime nightEDate = DateTime.Now.Date.AddDays(-i + 1).AddMinutes(morningTimes);

                                var mRecords = await _deviceRecordsDomainService.GetListByDate(morningSDate, morningEDate);
                                if (mRecords?.Count() > 0)
                                {
                                    summaryRecords.AddRange(await GetDataBySailings(mRecords, 0, morningSDate, morningEDate));
                                }
                                var middleRecords = await _deviceRecordsDomainService.GetListByDate(morningEDate, middleEDate);
                                if (middleRecords?.Count() > 0)
                                {
                                    summaryRecords.AddRange(await GetDataBySailings(middleRecords, 1, morningEDate, middleEDate));
                                }
                                var nRecords = await _deviceRecordsDomainService.GetListByDate(middleEDate, nightEDate);
                                if (nRecords?.Count() > 0)
                                {
                                    summaryRecords.AddRange(await GetDataBySailings(nRecords, 2, middleEDate, nightEDate));
                                }
                            }
                        }
                    }
                }

                if (summaryRecords != null && summaryRecords.Count > 0)
                {
                    await _deviceRecordsSummaryDomainService.BulkInsert(summaryRecords);
                }
            }
        }

        private async Task<List<DeviceRecordsSummary>> GetDataBySailings(List<DeviceRecords> deviceRecords, int sailings, DateTime startDate, DateTime endDate)
        {
            List<DeviceRecordsSummary> returnDatas = new List<DeviceRecordsSummary>();

            var summaryDatas = await _deviceRecordsSummaryDomainService.QueryAsync(p => p.CreateTime >= startDate && p.CreateTime < endDate
            , p => p.DeviceCode, OrderByType.Asc);

            List<string> deviceIds = deviceRecords.Where(p => !string.IsNullOrEmpty(p.DeviceCode)).Select(p => p.DeviceCode.ToLower()).ToList();
            var drillRateDatas = await _drillRateFactorDomainService.GetListByDate(deviceIds, startDate, endDate);

            List<DeviceRecordsSummary> updateList = new List<DeviceRecordsSummary>();
            foreach (var record in deviceRecords)
            {
                //TODO，新增吸尘报警
                //TODO-------------------
                #region 读取参数设定的标准值

                //吸尘报警---单次耗时(秒)
                int drillNoVacuumConfigTime = await _sysConfigManager.GetIntValue(nameof(DrillRateFactorReason.DrillNoVacuum));
                int drillNoVacuumCount = await GetReasonCount(drillRateDatas, record.DeviceCode, DrillRateFactorReason.DrillNoVacuum);
                int drillNoVacuumStandardTime = drillNoVacuumConfigTime * drillNoVacuumCount;
                #endregion

                //TODO-------------------
                //int drillNoVacuumRealTime = await GetReasonTimeTotalSeconds(drillRateDatas, record.DeviceCode, DrillRateFactorReason.DrillNoVacuum);

                int withoutTaskTime = await GetReasonTime(drillRateDatas, record.DeviceCode, DrillRateFactorReason.WithoutPendingWorkOrders);
                int withoutPanelTime = await GetReasonTime(drillRateDatas, record.DeviceCode, DrillRateFactorReason.WithoutPendingPanel);
                int withoutDrillFileTime = await GetReasonTime(drillRateDatas, record.DeviceCode, DrillRateFactorReason.WithoutDrillFile);
                int collectClearTime = await GetReasonTime(drillRateDatas, record.DeviceCode, DrillRateFactorReason.DrillCollectClear);
                int toolLifeExporedTime = await GetReasonTime(drillRateDatas, record.DeviceCode, DrillRateFactorReason.DrillToolLifeExpored);
                int toolLifeExporedCount = await GetReasonCount(drillRateDatas, record.DeviceCode, DrillRateFactorReason.DrillToolLifeExpored);
                int alarmTime = await GetReasonTime(drillRateDatas, record.DeviceCode, DrillRateFactorReason.DrillAlarm);
                int runTime = await GetReasonTime(drillRateDatas, record.DeviceCode, DrillRateFactorReason.DrillRun);
                int bufferNoBoardTime = await GetReasonTime(drillRateDatas, record.DeviceCode, DrillRateFactorReason.BufferRawChangeNoBoard);
                int bufferClinkerExistTime = await GetReasonTime(drillRateDatas, record.DeviceCode, DrillRateFactorReason.BufferClinkerChangeExist);
                int deviceDisableTime = await GetReasonTime(drillRateDatas, record.DeviceCode, DrillRateFactorReason.DeviceDisable);
                int drillRawExistToRunTime = await GetReasonTime(drillRateDatas, record.DeviceCode, DrillRateFactorReason.DrillRawExistToRun);
                int bufferAutomaticTime = await GetReasonTime(drillRateDatas, record.DeviceCode, DrillRateFactorReason.BufferAutomatic);
                int drillBoardDirectionTime = await GetReasonTime(drillRateDatas, record.DeviceCode, DrillRateFactorReason.DrillBoardDirection);
                int drillToolEvaluationTime = await GetReasonTime(drillRateDatas, record.DeviceCode, DrillRateFactorReason.DrillToolEvaluation);
                int drillTestPinTime = await GetReasonTime(drillRateDatas, record.DeviceCode, DrillRateFactorReason.DrillTestPin);
                int bufferManualTime = await GetReasonTime(drillRateDatas, record.DeviceCode, DrillRateFactorReason.BufferManual);
                int endToStartTime = await GetReasonTime(drillRateDatas, record.DeviceCode, DrillRateFactorReason.DrillEndToStart);
                int waitTime = withoutDrillFileTime + withoutPanelTime + withoutTaskTime
                    + collectClearTime + toolLifeExporedTime + alarmTime + drillToolEvaluationTime;


                if (summaryDatas != null && summaryDatas.Count > 0)
                {
                    var sData = summaryDatas.FirstOrDefault(p => p.DeviceCode == record.DeviceCode && p.CreateTime == startDate && p.Sailings == sailings);
                    if (sData != null)
                    {
                        sData.WithoutTaskTime = withoutTaskTime;
                        sData.WithoutDrillFileTime = withoutDrillFileTime;
                        sData.WithoutPanelTime = withoutPanelTime;
                        sData.CollectClearTime = collectClearTime;
                        sData.AlarmTime = alarmTime;
                        sData.ToolLifeExporedTime = toolLifeExporedTime;
                        sData.ToolLifeExporedCount = toolLifeExporedCount;
                        sData.RunTime = runTime;
                        sData.BufferClinkerExistTime = bufferClinkerExistTime;
                        sData.BufferNoBoardTime = bufferNoBoardTime;
                        sData.DeviceDisableTime = deviceDisableTime;
                        sData.BufferAutomaticTime = bufferAutomaticTime;
                        sData.DrillRawExistToRunTime = drillRawExistToRunTime;
                        sData.DrillToolEvaluationTime = drillToolEvaluationTime;
                        sData.DrillTestPinTime = drillTestPinTime;
                        sData.DrillBoardDirectionTime = drillBoardDirectionTime;
                        sData.BufferManualTime = bufferManualTime;
                        sData.EndToStartTime = endToStartTime;
                        // sData.WaitTime = waitTime.ToString();

                        //吸尘报警
                        sData.DrillNoVacuumCount = drillNoVacuumCount;
                        sData.DrillNoVacuumStandardTime = drillNoVacuumStandardTime;
                        //sData.DrillNoVacuumRealTime = drillNoVacuumRealTime;

                        int oldOpenTime = (int)sData.OpenTime;
                        int newOpenTime = 0;
                        int.TryParse(record.OpenTime, out newOpenTime);

                        if (oldOpenTime < newOpenTime)
                        {
                            sData.OpenTime = newOpenTime;
                            sData.ErrorTime = record.ErrorTime.ToInt();
                            sData.DateString = record.DateString;
                            sData.Duty = record.Duty.ToInt();
                            sData.WorkTime = record.WorkTime.ToInt();
                            sData.WaitTime = record.WaitTime.ToInt();
                            sData.ErrorTime = record.ErrorTime.ToInt();
                            sData.ModifyTime = DateTime.Now;
                        }
                        await GetDeviceRecordsSummaryExtend(drillRateDatas, record.DeviceCode, sData, startDate, endDate);
                        updateList.Add(sData);
                    }
                    else
                    {
                        sData = new DeviceRecordsSummary
                        {
                            Sailings = sailings,
                            DateString = record.DateString,
                            EndToStartTime = endToStartTime,
                            WaitTime = record.WaitTime.ToInt(),
                            WorkTime = record.WorkTime.ToInt(),
                            OpenTime = record.OpenTime.ToInt(),
                            DeviceCode = record.DeviceCode,
                            Duty = record.Duty.ToInt(),
                            ErrorTime = record.ErrorTime.ToInt(),
                            CreateTime = startDate,
                            CreatorId = record.CreatorId,
                            Status = record.Status,
                            IsDeleted = record.IsDeleted,
                            WithoutTaskTime = withoutTaskTime,
                            WithoutPanelTime = withoutPanelTime,
                            WithoutDrillFileTime = withoutDrillFileTime,
                            ToolLifeExporedTime = toolLifeExporedTime,
                            ToolLifeExporedCount = toolLifeExporedCount,
                            AlarmTime = alarmTime,
                            CollectClearTime = collectClearTime,
                            RunTime = runTime,
                            BufferNoBoardTime = bufferNoBoardTime,
                            BufferClinkerExistTime = bufferClinkerExistTime,
                            DeviceDisableTime = deviceDisableTime,
                            BufferAutomaticTime = bufferAutomaticTime,
                            DrillRawExistToRunTime = drillRawExistToRunTime,
                            DrillToolEvaluationTime = drillToolEvaluationTime,
                            DrillTestPinTime = drillTestPinTime,
                            DrillBoardDirectionTime = drillBoardDirectionTime,
                            BufferManualTime = bufferManualTime,
                            //吸尘报警
                            DrillNoVacuumStandardTime = drillNoVacuumStandardTime,
                            //DrillNoVacuumRealTime = drillNoVacuumRealTime,
                            DrillNoVacuumCount = drillNoVacuumCount

                        };
                        await GetDeviceRecordsSummaryExtend(drillRateDatas, record.DeviceCode, sData, startDate, endDate);
                        returnDatas.Add(sData);
                    }
                }
                else
                {
                    var sData = new DeviceRecordsSummary
                    {
                        Sailings = sailings,
                        DateString = record.DateString,
                        EndToStartTime = endToStartTime,
                        WaitTime = record.WaitTime.ToInt(),
                        WorkTime = record.WorkTime.ToInt(),
                        OpenTime = record.OpenTime.ToInt(),
                        DeviceCode = record.DeviceCode,
                        Duty = record.Duty.ToInt(),
                        ErrorTime = record.ErrorTime.ToInt(),
                        CreateTime = startDate,
                        CreatorId = record.CreatorId,
                        Status = record.Status,
                        IsDeleted = record.IsDeleted,
                        WithoutTaskTime = withoutTaskTime,
                        WithoutPanelTime = withoutPanelTime,
                        WithoutDrillFileTime = withoutDrillFileTime,
                        ToolLifeExporedTime = toolLifeExporedTime,
                        ToolLifeExporedCount = toolLifeExporedCount,
                        AlarmTime = alarmTime,
                        CollectClearTime = collectClearTime,
                        RunTime = runTime,
                        BufferNoBoardTime = bufferNoBoardTime,
                        BufferClinkerExistTime = bufferClinkerExistTime,
                        DeviceDisableTime = deviceDisableTime,
                        BufferAutomaticTime = bufferAutomaticTime,
                        DrillRawExistToRunTime = drillRawExistToRunTime,
                        DrillToolEvaluationTime = drillToolEvaluationTime,
                        DrillTestPinTime = drillTestPinTime,
                        DrillBoardDirectionTime = drillBoardDirectionTime,
                        BufferManualTime = bufferManualTime,
                        //吸尘报警
                        DrillNoVacuumStandardTime = drillNoVacuumStandardTime,
                        //DrillNoVacuumRealTime = drillNoVacuumRealTime,
                        DrillNoVacuumCount = drillNoVacuumCount
                    };
                    await GetDeviceRecordsSummaryExtend(drillRateDatas, record.DeviceCode, sData, startDate, endDate);
                    returnDatas.Add(sData);
                }
            }

            await _deviceRecordsSummaryDomainService.BulkUpdate(updateList);

            return returnDatas;
        }

        private async Task<DeviceRecordsSummary> GetDeviceRecordsSummaryExtend(List<DrillRateFactorSummaryDto> drillRateDatas, string deviceCode, DeviceRecordsSummary deviceRecordSummary, DateTime startDate, DateTime endDate)
        {
            var data = _deviceRecordsSummaryExtendDomainService.GetListByDeviceCode(deviceCode);
            if (data == null)
            {
                return deviceRecordSummary;
            }
            var toolLifeExporedChangeConfigTime = await _sysConfigManager.GetIntValue(nameof(DrillRateFactorReason.ToolLifeExporedChange));
            var toolLifeExporedChangeStandard = toolLifeExporedChangeConfigTime * deviceRecordSummary.ToolLifeExporedCount;

            var noSwitchMaterialToolChangeConfigTime = await _sysConfigManager.GetIntValue(nameof(DrillRateFactorReason.NoSwitchMaterialToolChange));
            var noSwitchMaterialToolChangeCount = await GetReasonCount(drillRateDatas, deviceCode, DrillRateFactorReason.NoSwitchMaterialToolChange);
            var noSwitchMaterialToolChangeStandard = noSwitchMaterialToolChangeConfigTime * noSwitchMaterialToolChangeCount;


            var switchMaterialToolChangeConfigTime = await _sysConfigManager.GetIntValue(nameof(DrillRateFactorReason.SwitchMaterialToolChange));
            var switchMaterialToolChangeCount = await GetReasonCount(drillRateDatas, deviceCode, DrillRateFactorReason.SwitchMaterialToolChange);
            var switchMaterialToolChangeStandard = switchMaterialToolChangeConfigTime * switchMaterialToolChangeCount;

            var pinReviseConfigTime = await _sysConfigManager.GetIntValue(nameof(DrillRateFactorReason.PINRevise));
            var pinReviseStandard = pinReviseConfigTime;



            var detectSwingTorqueConfigTime = await _sysConfigManager.GetIntValue(nameof(DrillRateFactorReason.DetectSwingTorque));
            var detectSwingTorqueCount = await GetReasonCount(drillRateDatas, deviceCode, DrillRateFactorReason.DetectSwingTorque);
            var detectSwingTorqueStandard = detectSwingTorqueConfigTime * detectSwingTorqueCount;

            var pressureFootChangeConfigTime = await _sysConfigManager.GetIntValue(nameof(DrillRateFactorReason.PressureFootChange));
            var pressureFootChangeCount = 0;
            var pressureFootChangeStandard = 0;
            //var pressureFootChangeRealTime =0;  
            var pressureFootChangeList = await _deviceTemporaryMaintenanceRecordsDomainService.GetList(new GetDeviceTemporaryMaintenanceRecordsListReq()
            {
                DeviceCode = deviceCode,
                StartTime = startDate,
                EndTime = endDate,
                PageSize = int.MaxValue,
                PageNum = 1
            });
            if (pressureFootChangeList?.List?.Count() > 0)
            {
                pressureFootChangeCount = pressureFootChangeList.List.//之后可能会新增临时保养项目类型，要做条件判断
                    Where(s => s.CountType == DeviceTemporaryMaintenanceCountTypeEnum.Frequency  /*&& s.MaintenanceType*/).Count();
                pressureFootChangeStandard = pressureFootChangeConfigTime * pressureFootChangeCount;

            }


            var minMultilayerBoardsValueConfigValue = await _sysConfigManager.GetIntValue(nameof(DrillRateFactorReason.MinMultilayerBoardsValue));
            var minMultilayerBoardsValueStandard = minMultilayerBoardsValueConfigValue;


            var twoBoardsWaitFirstConfigTime = await _sysConfigManager.GetIntValue(nameof(DrillRateFactorReason.TwoBoardsWaitFirstResult));
            var twoBoardsWaitFirstCount = await GetReasonCount(drillRateDatas, deviceCode, DrillRateFactorReason.TwoBoardsWaitFirstResult);
            var twoBoardsWaitFirstStandard = twoBoardsWaitFirstConfigTime * twoBoardsWaitFirstCount;


            var multilayerBoardsWaitFirstResultConfigTime = await _sysConfigManager.GetIntValue(nameof(DrillRateFactorReason.MultilayerBoardsWaitFirstResult));
            var multilayerBoardsWaitFirstResultCount = await GetReasonCount(drillRateDatas, deviceCode, DrillRateFactorReason.MultilayerBoardsWaitFirstResult);
            var multilayerBoardsWaitFirstResultStandard = multilayerBoardsWaitFirstResultConfigTime * multilayerBoardsWaitFirstResultCount;

            deviceRecordSummary.ToolLifeExporedChangeStandardTime = toolLifeExporedChangeStandard;


            deviceRecordSummary.NoSwitchMaterialToolChangeCount = noSwitchMaterialToolChangeCount;
            deviceRecordSummary.NoSwitchMaterialToolChangeStandardTime = noSwitchMaterialToolChangeStandard;

            deviceRecordSummary.SwitchMaterialToolChangeCount = switchMaterialToolChangeCount;
            deviceRecordSummary.SwitchMaterialToolChangeStandardTime = switchMaterialToolChangeStandard;


            deviceRecordSummary.SwitchMaterialToolChangeCount = switchMaterialToolChangeCount;
            deviceRecordSummary.SwitchMaterialToolChangeStandardTime = switchMaterialToolChangeStandard;



            deviceRecordSummary.PINReviseStandardTime = pinReviseStandard;


            deviceRecordSummary.DetectSwingTorqueCount = detectSwingTorqueCount;
            deviceRecordSummary.DetectSwingTorqueStandardTime = detectSwingTorqueStandard;


            deviceRecordSummary.PressureFootChangeCount = pressureFootChangeCount;
            deviceRecordSummary.PressureFootChangeStandardTime = pressureFootChangeStandard;


            deviceRecordSummary.MinMultilayerBoardsStandardValue = minMultilayerBoardsValueStandard;


            deviceRecordSummary.TwoBoardsWaitFirstResultCount = twoBoardsWaitFirstCount;
            deviceRecordSummary.TwoBoardsWaitFirstResultStandardTime = twoBoardsWaitFirstStandard;



            deviceRecordSummary.MultilayerBoardWaitFirstResultCount = multilayerBoardsWaitFirstResultCount;
            deviceRecordSummary.MultilayerBoardWaitFirstResultStandardTime = multilayerBoardsWaitFirstResultStandard;


            #region 理论稼动率   理论稼动率 = (720 - 必要时间）/ 720
            /*必要时间 :
            理论必要时长（秒） =
            刀具寿命标准（秒） * 次数 +
            换料标准耗时（不换料号）（秒） * 次数+
            换料标准耗时（换料号）（秒） * 次数+
            夹PIN按班次耗时（不用乘） + 
            检测摆幅扭力耗时（不用乘）+            
            压力脚更换标准耗时（秒） * 次数 +
            吸尘报警标准耗时（秒） * 次数 +
            首件等待两层板标准耗时（秒） * 次数 +
            首件等待多层板标准耗时（秒） * 次数 
            */

            // 必要时长， 不计入 清洗夹头的时长

            var necessaryTime = deviceRecordSummary.ToolLifeExporedChangeStandardTime
               + deviceRecordSummary.NoSwitchMaterialToolChangeStandardTime
               + deviceRecordSummary.SwitchMaterialToolChangeStandardTime
               + deviceRecordSummary.PINReviseStandardTime
               + deviceRecordSummary.DetectSwingTorqueStandardTime
               + deviceRecordSummary.PressureFootChangeStandardTime
               + deviceRecordSummary.DrillNoVacuumStandardTime
               + deviceRecordSummary.TwoBoardsWaitFirstResultStandardTime
               + deviceRecordSummary.MultilayerBoardWaitFirstResultStandardTime;
            deviceRecordSummary.NecessaryTime = necessaryTime;
            var theoryDuty = 720 * 60 - necessaryTime;
            deviceRecordSummary.TheoryDuty = (int)(Math.Round((decimal)(theoryDuty > 0 ? theoryDuty : 0) / (720 * 60), 2, MidpointRounding.AwayFromZero) * 100);
            #endregion

            #region 标准稼动率   标准稼动率 = 实际稼动时长 / 720 

            #endregion

            #region 理论稼动率达成率   理论稼动率达成率 = 标准稼动率 / 理论稼动率 = 实际稼动 / ( 720 - 必要时间)
            var dutyRate = deviceRecordSummary.TheoryDuty > 0 ?
                (int)(Math.Round((decimal)deviceRecordSummary.Duty
                / (decimal)deviceRecordSummary.TheoryDuty, 2, MidpointRounding.AwayFromZero) * 100) : 0;
            deviceRecordSummary.DutyRate = dutyRate;
            #endregion
            return deviceRecordSummary;
        }



        private async Task<int> GetReasonTime(List<DrillRateFactorSummaryDto> drillRateDatas, string deviceId, DrillRateFactorReason reason)
        {
            var withoutTask = drillRateDatas.FirstOrDefault(p => p.DeviceId.ToLower() == deviceId.ToLower()
                            && p.Reason == reason);
            int withoutTaskTime = withoutTask == null ? 0 : withoutTask.ReasonTime;
            return withoutTaskTime;
        }

        private async Task<int> GetReasonTimeTotalSeconds(List<DrillRateFactorSummaryDto> drillRateDatas, string deviceId, DrillRateFactorReason reason)
        {
            var withoutTask = drillRateDatas.FirstOrDefault(p => p.DeviceId.ToLower() == deviceId.ToLower()
                            && p.Reason == reason);
            int withoutTaskTime = withoutTask == null ? 0 : withoutTask.ReasonTimeTotalSeconds;
            return withoutTaskTime;
        }


        private async Task<int> GetReasonCount(List<DrillRateFactorSummaryDto> drillRateDatas, string deviceId, DrillRateFactorReason reason)
        {
            var data = drillRateDatas.FirstOrDefault(p => p.DeviceId.ToLower() == deviceId.ToLower()
                            && p.Reason == reason);
            int reasonCount = data == null ? 0 : data.ReasonCount;
            return reasonCount;
        }

        private async Task<int> DateDiff(DateTime dateStart, DateTime dateEnd)
        {
            DateTime start = Convert.ToDateTime(dateStart.ToShortDateString());
            DateTime end = Convert.ToDateTime(dateEnd.ToShortDateString());
            TimeSpan sp = end.Subtract(start);
            return sp.Days;
        }

        private async Task<int> GetMinute(string sailingsTime)
        {
            int minute = 0;
            if (sailingsTime.Contains(":"))
            {
                var arrDatas = sailingsTime.Split(':');
                if (arrDatas.Length > 1)
                {
                    int h = 0;
                    int.TryParse(arrDatas[0], out h);
                    int m = 0;
                    int.TryParse(arrDatas[1], out m);

                    minute = h * 60 + m;
                }
            }
            else if (sailingsTime.Contains("："))
            {
                var arrDatas = sailingsTime.Split('：');
                if (arrDatas.Length > 1)
                {
                    int h = 0;
                    int.TryParse(arrDatas[0], out h);
                    int m = 0;
                    int.TryParse(arrDatas[1], out m);

                    minute = h * 60 + m;
                }
            }

            return minute;
        }

        public async Task<List<DeviceRecordsSummaryToExcelDto>> GetSummaryToExcelList(GetDeviceRecordsSummaryListReq req)
        {
            List<DeviceRecordsSummaryToExcelDto> returnDtos = new List<DeviceRecordsSummaryToExcelDto>();

            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 100) req.PageSize = 1000;
            var datas = await GetSummaryList(req);
            if (datas == null || datas.Data == null || datas.Data.List == null || datas.Data.List.Count == 0)
            {
                return returnDtos;
            }

            foreach (var item in datas.Data.List)
            {
                string sailings = string.Empty;
                switch (item.Sailings)
                {
                    case 0:
                        sailings = item.CreateTime.ToString("yyyy-MM-dd") + " 白班";
                        break;

                    case 1:
                        sailings = item.CreateTime.ToString("yyyy-MM-dd") + " 中班";
                        break;

                    case 2:
                        sailings = item.CreateTime.ToString("yyyy-MM-dd") + " 晚班";
                        break;

                    default:
                        sailings = item.CreateTime.ToString("yyyy-MM-dd") + " 白班";
                        break;
                }

                try
                {
                    item.DateString = DateTime.Parse(item.DateString).ToString("yyyy-MM-dd hh:mm:ss");
                }
                catch { }

                returnDtos.Add(new DeviceRecordsSummaryToExcelDto
                {
                    DeviceCode = item.DeviceCode,
                    RouteCode = item.RouteCode,
                    CollectClearTime = item.CollectClearTime,
                    DateString = item.DateString,
                    Duty = item.Duty,
                    //TheoryDuty = item.TheoryDuty,
                    //DutyRate = item.DutyRate,
                    ErrorTime = item.ErrorTime,
                    OpenTime = item.OpenTime,
                    WaitTime = item.WaitTime,
                    WorkTime = item.WorkTime,
                    Sailings = sailings,
                    WithoutTaskTime = item.WithoutTaskTime,
                    WithoutDrillFileTime = item.WithoutDrillFileTime,
                    ToolLifeExporedTime = item.ToolLifeExporedTime,
                    DeviceDisableTime = item.DeviceDisableTime,
                    DrillToolEvaluationTime = item.DrillToolEvaluationTime,
                    BufferManualTime = item.BufferManualTime,
                    MesRecordDate = item.ModifyTime.Value.ToString("yyyy-MM-dd hh:mm:ss")
                });
            }

            return returnDtos;
        }

        public async Task<List<DeviceRecordsSummaryToExcelDtoInner>> GetInnerSummaryToExcelList(GetDeviceRecordsSummaryListReq req)
        {
            List<DeviceRecordsSummaryToExcelDtoInner> returnDtos = new List<DeviceRecordsSummaryToExcelDtoInner>();

            if (req.PageNum < 1) req.PageNum = 1;
            if (req.PageSize < 100) req.PageSize = 1000;
            var datas = await GetSummaryList(req);
            if (datas == null || datas.Data == null || datas.Data.List == null || datas.Data.List.Count == 0)
            {
                return returnDtos;
            }

            foreach (var item in datas.Data.List)
            {
                string sailings = string.Empty;
                switch (item.Sailings)
                {
                    case 0:
                        sailings = item.CreateTime.ToString("yyyy-MM-dd") + " 白班";
                        break;

                    case 1:
                        sailings = item.CreateTime.ToString("yyyy-MM-dd") + " 中班";
                        break;

                    case 2:
                        sailings = item.CreateTime.ToString("yyyy-MM-dd") + " 晚班";
                        break;

                    default:
                        sailings = item.CreateTime.ToString("yyyy-MM-dd") + " 白班";
                        break;
                }

                try
                {
                    item.DateString = DateTime.Parse(item.DateString).ToString("yyyy-MM-dd hh:mm:ss");
                }
                catch { }

                returnDtos.Add(new DeviceRecordsSummaryToExcelDtoInner
                {
                    DeviceCode = item.DeviceCode,
                    RouteCode = item.RouteCode,
                    CollectClearTime = item.CollectClearTime,
                    DateString = item.DateString,
                    Duty = item.Duty,
                    //TheoryDuty = item.TheoryDuty,
                    //DutyRate = item.DutyRate,
                    ErrorTime = item.ErrorTime,
                    OpenTime = item.OpenTime,
                    WaitTime = item.WaitTime,
                    WorkTime = item.WorkTime,
                    Sailings = sailings,
                    WithoutTaskTime = item.WithoutTaskTime,
                    WithoutDrillFileTime = item.WithoutDrillFileTime,
                    ToolLifeExporedTime = item.ToolLifeExporedTime,
                    ToolLifeExporedCount = item.ToolLifeExporedCount,
                    DeviceDisableTime = item.DeviceDisableTime,
                    DrillToolEvaluationTime = item.DrillToolEvaluationTime,
                    BufferManualTime = item.BufferManualTime,
                    //BufferAutomaticTime = item.BufferAutomaticTime,
                    BufferNoBoardTime = item.BufferNoBoardTime,
                    BufferClinkerExistTime = item.BufferClinkerExistTime,
                    DrillRawExistToRunTime = item.DrillRawExistToRunTime,
                    DrillBoardDirectionTime = item.DrillBoardDirectionTime,
                    AlarmTime = item.AlarmTime,
                    DrillTestPinTime = item.DrillTestPinTime,
                    EndToStartTime = item.EndToStartTime,
                    RunTime = item.RunTime,
                    WithoutPanelTime = item.WithoutPanelTime,
                    MesRecordDate = item.ModifyTime.Value.ToString("yyyy-MM-dd hh:mm:ss")
                });
            }

            return returnDtos;
        }

        public async Task<ResponseDto<List<DrillRateFactorDto>>> GetRateReasonDetails(long recordSummaryId)
        {
            var recordSummary = await _deviceRecordsSummaryDomainService.QueryByID(recordSummaryId);
            if (recordSummary == null)
            {
                return Fail<List<DrillRateFactorDto>>($"未找到数据 {recordSummaryId} !");
            }

            var returnList = await GetRateReasonDatas(recordSummary);
            if (returnList != null && returnList.Count > 0)
            {
                returnList = returnList.OrderBy(p => p.StartTime).ToList();

                foreach (var item in returnList)
                {
                    switch (recordSummary.Sailings)
                    {
                        case 0:
                            item.DateSailings = recordSummary.CreateTime.ToString("yyyy-MM-dd") + " -白班";
                            break;

                        case 1:
                            item.DateSailings = recordSummary.CreateTime.ToString("yyyy-MM-dd") + " -中班";
                            break;

                        case 2:
                            item.DateSailings = recordSummary.CreateTime.ToString("yyyy-MM-dd") + " -晚班";
                            break;

                        default:
                            item.DateSailings = recordSummary.CreateTime.ToString("yyyy-MM-dd") + " -白班";
                            break;
                    }
                }
            }
            else
            {
                returnList = new List<DrillRateFactorDto>();
            }

            return Success(returnList);
        }

        private async Task<List<DrillRateFactorDto>> GetRateReasonDatas(DeviceRecordsSummary recordSummary)
        {
            DateTime startDate = recordSummary.CreateTime.Date;
            DateTime endDate = recordSummary.CreateTime.Date;

            string morning = string.Empty;
            string middle = string.Empty;
            string night = string.Empty;

            var sailings = await _sysConfigManager.GetStringValue(MESConfigConstants.SAILINGS);
            if (!string.IsNullOrEmpty(sailings) && sailings.Contains("|"))
            {
                var arrSailings = sailings.Split('|');
                if (arrSailings.Length > 1)
                {
                    for (int i = 0; i < arrSailings.Length; i++)
                    {
                        if (arrSailings[i].StartsWith("0-"))
                        {
                            morning = arrSailings[i].Replace("0-", "");
                        }
                        else if (arrSailings[i].StartsWith("1-"))
                        {
                            middle = arrSailings[i].Replace("1-", "");
                        }
                        else if (arrSailings[i].StartsWith("2-"))
                        {
                            night = arrSailings[i].Replace("2-", "");
                        }
                    }
                }
            }

            switch (recordSummary.Sailings)
            {
                case 0:
                    if (!string.IsNullOrEmpty(morning))
                    {
                        startDate = startDate.AddMinutes(await GetMinute(morning));
                        if (!string.IsNullOrEmpty(middle))
                        {
                            endDate = endDate.AddMinutes(await GetMinute(middle));
                            break;
                        }
                        else if (!string.IsNullOrEmpty(night))
                        {
                            endDate = endDate.AddMinutes(await GetMinute(night));
                            break;
                        }
                    }
                    endDate = endDate.AddDays(1);
                    break;

                case 1:
                    if (!string.IsNullOrEmpty(middle))
                    {
                        startDate = startDate.AddMinutes(await GetMinute(middle));
                        if (!string.IsNullOrEmpty(night))
                        {
                            endDate = endDate.AddMinutes(await GetMinute(night));
                            break;
                        }
                        else if (!string.IsNullOrEmpty(morning))
                        {
                            endDate = endDate.AddDays(1).AddMinutes(await GetMinute(morning));
                            break;
                        }
                    }
                    endDate = endDate.AddDays(1);
                    break;

                case 2:
                    if (!string.IsNullOrEmpty(night))
                    {
                        startDate = startDate.AddMinutes(await GetMinute(night));
                        if (!string.IsNullOrEmpty(morning))
                        {
                            endDate = endDate.AddDays(1).AddMinutes(await GetMinute(morning));
                            break;
                        }
                        else if (!string.IsNullOrEmpty(middle))
                        {
                            endDate = endDate.AddDays(1).AddMinutes(await GetMinute(middle));
                            break;
                        }
                    }
                    endDate = endDate.AddDays(1);
                    break;
            }

            if (endDate > DateTime.Now)
            {
                endDate = DateTime.Now;
            }

            var returnList = await _drillRateFactorDomainService.GetRateReasonDetails(recordSummary.DeviceCode, startDate, endDate);
            return returnList;
        }

        public async Task RegularDeleteData()
        {
            var rateSummaryTime = await _sysConfigManager.GetIntValue(MESConfigConstants.RATE_RECORD_SUMMARY_RETAIN_TIME);
            var rateTime = await _sysConfigManager.GetIntValue(MESConfigConstants.RATE_FACTOR_RETAIN_TIME);
            if (rateSummaryTime <= 0)
            {
                rateSummaryTime = 6;
            }
            if (rateTime <= 0)
            {
                rateTime = 1;
            }

            DateTime summaryTime = DateTime.Now.AddMonths(-rateSummaryTime);
            DateTime recordsTime = DateTime.Now.AddMonths(-rateTime);

            await _deviceRecordsSummaryDomainService.DeleteAsync(p => p.CreateTime < summaryTime);
            await _deviceRecordsDomainService.DeleteAsync(p => p.ModifyTime < recordsTime);
            await _drillRateFactorDomainService.DeleteAsync(p => p.ModifyTime < recordsTime);
        }

        /// <summary>
        /// 获取设备保养配置
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<ResponseDto<PageDto<SysConfigDto>>> GetDeviceMaintenanceConfigs(GetSysConfigListReq req)
        {
            return await _sysConfigService.GetList(req);
        }

        /// <summary>
        /// 保存设备保养配置
        /// </summary>
        /// <param name="reqs"></param>
        /// <returns></returns>
        public async Task<ResponseDto<string>> SaveDeviceMaintenanceConfigs(List<AddOrUpdateSysConfigReq> reqs)
        {
            return await _sysConfigService.SaveBasicSysData(reqs);
        }
    }
}