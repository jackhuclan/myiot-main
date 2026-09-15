using AutoMapper;
using VgAutoDrill.Admin.Application.Interfaces;
using VgAutoDrill.Admin.Application.Interfaces.BigScreenServices;
using VgAutoDrill.Admin.Application.Interfaces.MesServices;
using VgAutoDrill.Admin.Common.Extensions;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Device;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DevicePanel;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DvAlarmRecord;
using VgAutoDrill.Admin.Model.ViewModels.Mes.Equipment;
using VgAutoDrill.Admin.Model.ViewModels.Mes.EquipmentType;
using VgAutoDrill.Admin.Model.ViewModels.Mes.FeedBack;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProBoardTrace;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProTask;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProWorkOrder;
using VgAutoDrill.Admin.Model.ViewModels.Req.Equipment;
using VgAutoDrill.Fundation.Iot;

namespace VgAutoDrill.Admin.Application.Services.BigScreenServices
{
    /// <summary>
    /// 大屏接口
    /// </summary>
    public class BigScreenServices : IBigScreenServices
    {
        private readonly IMapper _mapper;
        private readonly IDeviceService _deviceService;
        private readonly IDeviceTypeService _typeService;
        private readonly IAlarmService _alarmService;
        private readonly IPanelService _panelService;
        private readonly IDevicePanelService _devicePanelService;
        private readonly IFeedBackService _feedBackService;
        private readonly IWorkOrderService _WorkOrderService;
        private readonly ITaskService _taskService;
        private readonly IDrillPanelDetailService _drillPanelDetailService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="equipmentService"></param>
        /// <param name="deviceTypeService"></param>
        /// <param name="boardTraceService"></param>
        /// <param name="devicePanelService"></param>
        /// <param name="feedBackService"></param>
        /// <param name="proWorkOrderService"></param>
        /// <param name="taskService"></param>
        /// <param name="alarmService"></param>
        /// <param name="mapper"></param>
        public BigScreenServices(IDeviceService equipmentService,
            IDeviceTypeService deviceTypeService,
            IPanelService boardTraceService,
            IDevicePanelService devicePanelService,
            IFeedBackService feedBackService,
            IWorkOrderService proWorkOrderService,
            ITaskService taskService,
            IAlarmService alarmService,
            IMapper mapper,
            IDrillPanelDetailService drillPanelDetailService)
        {
            _mapper = mapper;
            _deviceService = equipmentService;
            _typeService = deviceTypeService;
            _alarmService = alarmService;
            _panelService = boardTraceService;
            _devicePanelService = devicePanelService;
            _feedBackService = feedBackService;
            _WorkOrderService = proWorkOrderService;
            _taskService = taskService;
            _drillPanelDetailService = drillPanelDetailService;
        }

        /// <summary>
        /// 获取设备统计数据
        ///（设备类型、总数、在线比例）
        /// </summary>
        /// <returns></returns>
        public async Task<List<DeviceStatsDto>> GetDeviceStats()
        {
            List<DeviceStatsDto> result = new List<DeviceStatsDto>();

            var typeData = await _typeService.GetEquipmentTypeList(new GetEquipmentTypeListReq
            {
                PageNum = 1,
                PageSize = int.MaxValue
            });

            var deviceData = await _deviceService.GetEquipmentList(new GetDeviceListReq
            {
                PageNum = 1,
                PageSize = int.MaxValue
            });

            if (typeData != null && typeData.Data.List != null
                && deviceData != null && deviceData.Data.List != null)
            {
                for (int i = 0; i < typeData.Data.List.Count; i++)
                {
                    DeviceTypeDto? item = typeData.Data.List[i];
                    if (string.IsNullOrEmpty(item.Name) || item.ParentId == 0)
                    {
                        continue;
                    }

                    DeviceStatsDto data = new DeviceStatsDto();

                    data.DeviceTypeName = item.Name;

                    var deviceOnType = deviceData.Data.List.FindAll(p => p.DeviceTypeId == item.Id).ToList();
                    decimal allCount = deviceOnType.Count;
                    data.AllDeviceCount = allCount;

                    if (allCount > 0)
                    {
                        decimal onLineCount = deviceOnType.FindAll(p => p.DeviceStatus == DeviceStatus.Online || p.DeviceStatus == DeviceStatus.Ready
                        || p.DeviceStatus == DeviceStatus.Working).Count;

                        decimal onLinePr = onLineCount / allCount * 100;

                        data.OnlineDevicePr = decimal.Round(onLinePr, 1);
                    }
                    else
                    {
                        data.OnlineDevicePr = 0;
                    }
                    result.Add(data);
                }
            }
            return result;
        }

        /// <summary>
        /// 获取设备告警统计数据
        ///（设备名称、告警日期、告警名称、告警级别）
        /// </summary>
        /// <returns></returns>
        public async Task<List<List<string>>> GetDeviceAlarmStats(int deviceId)
        {
            List<List<string>> result = new List<List<string>>();

            var alarmData = await _alarmService.GetList(new GetAlarmListReq
            {
                PageNum = 1,
                PageSize = int.MaxValue,
                IsHandled = false
            });

            var deviceList = await _deviceService.GetEquipmentList(new GetDeviceListReq
            {
                PageNum = 1,
                PageSize = int.MaxValue
            });

            if (deviceList == null || deviceList.Data.List == null)
            {
                return result;
            }
            var deviceData = deviceList.Data.List;
            if (deviceId > 0)
            {
                deviceData = deviceList.Data.List.Where(p => p.Id == deviceId).ToList();
            }

            if (alarmData != null && alarmData.Data.List != null
                && deviceData != null)
            {
                for (int i = 0; i < alarmData.Data.List.Count; i++)
                {
                    AlarmDto? item = alarmData.Data.List[i];
                    if (item == null || string.IsNullOrEmpty(item.AlarmName) || item.DeviceId == 0)
                    {
                        continue;
                    }

                    var deviceOnAlarm = deviceData.SingleOrDefault(p => p.Id == item.DeviceId);
                    if (deviceOnAlarm == null || string.IsNullOrEmpty(deviceOnAlarm.Name))
                    {
                        continue;
                    }

                    List<string> data = new List<string>();

                    data.Add(deviceOnAlarm.Name);

                    string alarmTime = "";
                    if (item.AlarmTime != null)
                    {
                        alarmTime = string.Format("{0:yyyy-MM-dd HH.mm.ss}", item.AlarmTime);
                    }
                    data.Add(alarmTime);

                    data.Add(item.AlarmName);

                    string alarmLevel = "";
                    switch (item.AlarmLevel)
                    {
                        case 1:
                            alarmLevel = "普通";
                            break;
                        case 2:
                            alarmLevel = "严重";
                            break;
                        case 3:
                            alarmLevel = "紧急";
                            break;
                        default:
                            alarmLevel = "未知";
                            break;
                    }
                    data.Add(alarmLevel);

                    result.Add(data);
                }
            }

            return result;
        }

        /// <summary>
        /// 获取板料追踪统计数据
        ///（板料料号，物料代码，对应设备）
        /// </summary>
        /// <returns></returns>
        public async Task<List<List<string>>> GetPanelStats(int deviceId)
        {
            List<List<string>> result = new List<List<string>>();

            var panelData = await _panelService.GetList(new GetPanelListReq
            {
                PageNum = 1,
                PageSize = int.MaxValue
            });

            var devicePanelData = await _devicePanelService.GetList(new GetDevicePanelListReq
            {
                PageNum = 1,
                PageSize = int.MaxValue
            });

            var deviceList = await _deviceService.GetEquipmentList(new GetDeviceListReq
            {
                PageNum = 1,
                PageSize = int.MaxValue
            });

            if (deviceList == null || deviceList.Data.List == null)
            {
                return result;
            }
            var deviceData = deviceList.Data.List;
            if (deviceId > 0)
            {
                deviceData = deviceList.Data.List.Where(p => p.Id == deviceId).ToList();
            }

            if (panelData != null && panelData.Data.List != null
                && devicePanelData != null && devicePanelData.Data.List != null
                && deviceData != null)
            {
                foreach (var item in panelData.Data.List)
                {
                    if (string.IsNullOrEmpty(item.PanelCode) || string.IsNullOrEmpty(item.ItemCode))
                    {
                        continue;
                    }

                    //devicePanelData按照时间倒序，取最新的一个
                    var devicePanelInfo = devicePanelData.Data.List.FirstOrDefault(p => p.PanelCode == item.PanelCode);
                    if (devicePanelInfo == null)
                    {
                        continue;
                    }

                    var deviceInfo = deviceData.SingleOrDefault(p => p.Code.ToLower() == devicePanelInfo.DeviceCode.ToLower());
                    if (deviceInfo == null || string.IsNullOrEmpty(deviceInfo.Name))
                    {
                        continue;
                    }

                    List<string> data = new List<string>
                    {
                        item.PanelCode,
                        item.ItemCode,
                        deviceInfo.Name
                    };

                    result.Add(data);
                }
            }

            return result;
        }

        /// <summary>
        /// 获取工单进度统计数据
        ///（工单号，产品名，工单进度）
        /// </summary>
        /// <returns></returns>
        public async Task<List<List<string>>> GetWorkOrderStats()
        {
            List<List<string>> result = new List<List<string>>();

            var workOrderData = await _WorkOrderService.GetList(new GetWorkOrderListReq
            {
                PageNum = 1,
                PageSize = int.MaxValue
            });

            if (workOrderData != null && workOrderData.Data.List != null)
            {
                foreach (var item in workOrderData.Data.List)
                {
                    if (string.IsNullOrEmpty(item.Code) || string.IsNullOrEmpty(item.ItemName)
                        || item.Quantity == 0)
                    {
                        continue;
                    }

                    decimal quantityPr = 0;
                    if (item.Quantity > 0 && item.QuantityProduced > 0)
                    {
                        quantityPr = (decimal)item.QuantityProduced / (decimal)item.Quantity * 100;
                    }

                    List<string> data = new List<string> {
                        item.Code,
                        item.ItemName,
                        quantityPr.ToString("#0.0") +"%"
                    };

                    result.Add(data);
                }
            }

            return result;
        }

        /// <summary>
        /// 获取近一周产量统计
        ///（工序、近一周每天产量、合格率）
        /// </summary>
        /// <returns></returns>
        public async Task<FeedBackStatsByTimeDto> GetFeedBackStats()
        {
            FeedBackStatsByTimeDto statsData = new FeedBackStatsByTimeDto();

            var feedBackData = await _feedBackService.GetList(new GetFeedBackListReq
            {
                PageNum = 1,
                PageSize = int.MaxValue
            });

            if (feedBackData != null && feedBackData.Data.List != null)
            {
                statsData.Times = new List<string>();
                statsData.QuantitysStats = new List<QuantitysStatsDto>();

                QuantitysStatsDto pinStatsData = new QuantitysStatsDto();
                pinStatsData.ProcessName = "叠板";
                pinStatsData.Quantitys = new List<string>();

                QuantitysStatsDto drillStatsData = new QuantitysStatsDto();
                drillStatsData.ProcessName = "钻孔";
                drillStatsData.Quantitys = new List<string>();

                QuantitysStatsDto unpinStatsData = new QuantitysStatsDto();
                unpinStatsData.ProcessName = "拆板";
                unpinStatsData.Quantitys = new List<string>();

                QuantitysStatsDto rateStatsData = new QuantitysStatsDto();
                rateStatsData.ProcessName = "合格率";
                rateStatsData.Quantitys = new List<string>();
                //当前只统计三种工序：叠板、钻孔、拆板
                for (int i = 6; i >= 0; i--)
                {
                    DateTime sTime = DateTime.Now.AddDays(-i).Date;

                    statsData.Times.Add(string.Format("{0:MM/dd}", sTime));

                    string pinCount = GetStatsData("叠板", sTime, feedBackData.Data.List);
                    if (!string.IsNullOrEmpty(pinCount))
                    {
                        pinStatsData.Quantitys.Add(pinCount);
                    }

                    string drillCount = GetStatsData("钻孔", sTime, feedBackData.Data.List);
                    if (!string.IsNullOrEmpty(drillCount))
                    {
                        drillStatsData.Quantitys.Add(drillCount);
                    }

                    string unpinCount = GetStatsData("拆板", sTime, feedBackData.Data.List);
                    if (!string.IsNullOrEmpty(unpinCount))
                    {
                        unpinStatsData.Quantitys.Add(unpinCount);
                    }

                    string rateStats = GetStatsData("合格率", sTime, feedBackData.Data.List);
                    if (!string.IsNullOrEmpty(rateStats))
                    {
                        rateStatsData.Quantitys.Add(rateStats);
                    }

                    //todo 根据工序表统计每个工序的近一周产量
                    //工序表需要添加orderNum用来排序
                    //仅查询提交状态的记录

                }
                statsData.QuantitysStats.Add(pinStatsData);
                statsData.QuantitysStats.Add(drillStatsData);
                statsData.QuantitysStats.Add(unpinStatsData);
                statsData.QuantitysStats.Add(rateStatsData);
            }

            return statsData;
        }

        /// <summary>
        /// 获取统计数据
        /// </summary>
        /// <param name="processName"></param>
        /// <param name="sTime"></param>
        /// <param name="feedBackList"></param>
        /// <returns></returns>
        private static string GetStatsData(string processName, DateTime sTime, List<FeedBackDto> feedBackList)
        {
            IEnumerable<FeedBackDto> feedBackData;
            if (processName.Equals("合格率"))
            {
                feedBackData = feedBackList.Where(p => !string.IsNullOrEmpty(p.ProcessCode) && p.ProcessCode.ToLower().Contains("drill")
                && p.FeedBackTime != null && p.FeedBackTime.ToDate().Date.Equals(sTime));
            }
            else
            {
                feedBackData = feedBackList.Where(p => !string.IsNullOrEmpty(p.ProcessName) && p.ProcessName.Contains(processName)
                && p.FeedBackTime != null && p.FeedBackTime.ToDate().Date.Equals(sTime));
            }

            if (feedBackData == null)
            {
                return "0";
            }

            decimal feedBackCount = 0;
            decimal qualifiedCount = 0;
            foreach (var item in feedBackData)
            {
                if (item.QuantityFeedBack != null)
                {
                    feedBackCount += (decimal)item.QuantityFeedBack;
                }
                if (item.QuantityQualified != null)
                {
                    qualifiedCount += (decimal)item.QuantityQualified;
                }
            }

            if (processName.Equals("合格率"))
            {
                if (feedBackCount == 0 || qualifiedCount == 0)
                {
                    return "0";
                }
                else
                {
                    decimal rate = qualifiedCount / feedBackCount * 100;
                    return rate.ToString("#0.0");
                }
            }
            else
            {
                return feedBackCount.ToString();
            }
        }

        /// <summary>
        /// 获取生产量统计
        ///（生产目标、实际产量、日进度）
        /// </summary>
        /// <returns></returns>
        public async Task<TaskStatsByTime> GetTaskStats()
        {
            TaskStatsByTime result = new TaskStatsByTime();

            var taskData = await _taskService.GetEquipmentList(new GetTaskListReq
            {
                PageNum = 1,
                PageSize = int.MaxValue
            });

            if (taskData != null && taskData.Data.List != null)
            {
                DateTime statsTime = DateTime.Now.Date;

                //统计结束时间为当天的，钻孔的生产量
                var nowData = taskData.Data.List.Where(p => p.EndTime != null && p.EndTime.ToDate().Date.Equals(statsTime)
                && !string.IsNullOrEmpty(p.ProcessCode) && p.ProcessCode.ToLower().Contains("drill")).ToList();
                if (nowData == null || nowData.Count == 0)
                {
                    result.ProgressDaily = 0;
                    return result;
                }

                decimal targetsCount = 0;
                decimal actualCount = 0;
                foreach (var item in nowData)
                {
                    if (item.Quantity != null)
                    {
                        targetsCount += (decimal)item.Quantity;
                    }

                    if (item.QuantityProduced != null)
                    {
                        actualCount += (decimal)item.QuantityProduced;
                    }
                }

                result.ProductionActual = actualCount;
                result.ProductionTargets = targetsCount;

                if (actualCount == 0 || targetsCount == 0)
                {
                    result.ProgressDaily = 0;
                }
                else
                {
                    decimal daily = actualCount / targetsCount * 100;
                    result.ProgressDaily = decimal.Round(daily, 1);
                }
            }

            return result;
        }

        /// <summary>
        /// 获取工单统计
        ///（生产增率、工单增率）
        /// </summary>
        /// <returns></returns>
        public async Task<WorkOrderRateStatsDto> GetWorkOrderRateStats()
        {
            WorkOrderRateStatsDto result = new WorkOrderRateStatsDto();

            var feedBackData = await _feedBackService.GetList(new GetFeedBackListReq
            {
                PageNum = 1,
                PageSize = int.MaxValue
            });

            var workOrderData = await _WorkOrderService.GetList(new GetWorkOrderListReq
            {
                PageNum = 1,
                PageSize = int.MaxValue
            });

            DateTime time = DateTime.Now.Date;

            if (feedBackData != null && feedBackData.Data.List != null)
            {
                var todayData = feedBackData.Data.List.Where(p => p.FeedBackTime != null && p.FeedBackTime.ToDate().Date.Equals(time)
                && !string.IsNullOrEmpty(p.ProcessCode) && p.ProcessCode.ToLower().Contains("drill")).ToList();

                var yesterdayData = feedBackData.Data.List.Where(p => p.FeedBackTime != null && p.FeedBackTime.ToDate().Date.Equals(time.AddDays(-1))
                && !string.IsNullOrEmpty(p.ProcessCode) && p.ProcessCode.ToLower().Contains("drill")).ToList();

                result.FeedBackAddRate = SetRateStast(todayData, yesterdayData, "FeedBackDto");
            }

            if (workOrderData != null && workOrderData.Data.List != null)
            {
                var todayData = workOrderData.Data.List.Where(p => p.CreateTime.ToDate().Date.Equals(time)).ToList();

                var yesterdayData = workOrderData.Data.List.Where(p => p.CreateTime.ToDate().Date.Equals(time.AddDays(-1))).ToList();

                result.WorkOrderAddRate = SetRateStast(todayData, yesterdayData, "WorkOrderDto");
            }

            return result;
        }

        private string SetRateStast<T>(List<T> todayData, List<T> yesterdayData, string type)
        {
            if (todayData == null)
            {
                return "0";
            }
            else
            {
                if (yesterdayData == null)
                {
                    return "100";
                }
                else
                {
                    decimal today = 0;
                    decimal yesterday = 0;

                    switch (type)
                    {
                        case "FeedBackDto":
                            foreach (var item in todayData)
                            {
                                var dto = _mapper.Map<FeedBackDto>(item);
                                if (dto == null)
                                {
                                    continue;
                                }

                                if (dto.QuantityFeedBack != null)
                                {
                                    today += (decimal)dto.QuantityFeedBack;
                                }
                            }

                            foreach (var item in yesterdayData)
                            {
                                var dto = _mapper.Map<FeedBackDto>(item);
                                if (dto == null)
                                {
                                    continue;
                                }

                                if (dto.QuantityFeedBack != null)
                                {
                                    yesterday += (decimal)dto.QuantityFeedBack;
                                }
                            }

                            break;
                        case "WorkOrderDto":
                            foreach (var item in todayData)
                            {
                                var dto = _mapper.Map<WorkOrderDto>(item);
                                if (dto == null)
                                {
                                    continue;
                                }

                                if (dto.QuantityChanged != null)
                                {
                                    today += (decimal)dto.QuantityChanged;
                                }
                            }

                            foreach (var item in yesterdayData)
                            {
                                var dto = _mapper.Map<WorkOrderDto>(item);
                                if (dto == null)
                                {
                                    continue;
                                }

                                if (dto.QuantityChanged != null)
                                {
                                    yesterday += (decimal)dto.QuantityChanged;
                                }
                            }

                            break;
                        default:
                            return "0";
                    }

                    if (today == 0)
                    {
                        return "0";
                    }
                    else
                    {
                        if (yesterday == 0)
                        {
                            return "100";
                        }
                        else
                        {
                            string rate;
                            if (today >= yesterday)
                            {
                                rate = ((today - yesterday) / yesterday * 100).ToString("#0.0");
                            }
                            else
                            {
                                rate = "-" + ((yesterday - today) / today * 100).ToString("#0.0");
                            }
                            return rate;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 获取设备运行情况统计
        ///（运行中、故障中、停用、待机）
        /// </summary>
        /// <returns></returns>
        public async Task<DeviceStatusStatsDto> GetDeviceStatusStats(int deviceId)
        {
            //暂定按照当天统计
            DeviceStatusStatsDto result = new DeviceStatusStatsDto();

            if (deviceId == 0)
            {
                return result;
            }

            //var deviceData = await _deviceService.QueryByDeviceId(deviceId.ToString());

            //if (deviceData == null)
            //{
            //    return result;
            //}

            //运行状态暂定写死，后期根据数据库具体计算
            result.WorkingDevicePr = "85.4";
            result.StopDevicePr = "3.2";
            result.StandbyDevicePr = "8.7";
            result.WarnningDevicePr = "2.7";

            return result;
        }

        /// <summary>
        /// 获取设备开机率统计
        /// </summary>
        /// <returns></returns>
        public async Task<List<List<string>>> GetDeviceUptimeStats(int deviceId)
        {
            //设备开机率 = 实际运行时间/总运行时间 *100%

            List<List<string>> result = new List<List<string>>();

            if (deviceId == 0)
            {
                return result;
            }

            //设备开机率暂定写死，后期根据数据库具体计算
            //var deviceData = await _deviceService.QueryByDeviceId(deviceId.ToString());

            //if (deviceData == null)
            //{
            //    return result;
            //}

            List<string> times = new List<string>();
            List<string> quantitys = new List<string>();

            for (int i = 6; i >= 0; i--)
            {
                DateTime sTime = DateTime.Now.AddDays(-i).Date;
                times.Add(string.Format("{0:MM/dd}", sTime));
            }

            quantitys.Add("92.5");
            quantitys.Add("97.8");
            quantitys.Add("91.7");
            quantitys.Add("96");
            quantitys.Add("95.4");
            quantitys.Add("90.2");
            quantitys.Add("88.9");

            result.Add(times);
            result.Add(quantitys);

            return result;
        }

        /// <summary>
        /// 获取设备稼动率统计
        /// </summary>
        /// <returns></returns>
        public async Task<List<List<string>>> GetDeviceMovementStats(int deviceId)
        {
            //设备稼动率 = （作业时间 - 流失时间）/作业时间 * 100%

            List<List<string>> result = new List<List<string>>();

            if (deviceId == 0)
            {
                return result;
            }

            //设备稼动率暂定写死，后期根据数据库具体计算
            //var deviceData = await _deviceService.QueryByDeviceId(deviceId.ToString());

            //if (deviceData == null)
            //{
            //    return result;
            //}

            List<string> times = new List<string>();
            List<string> quantitys = new List<string>();

            for (int i = 6; i >= 0; i--)
            {
                DateTime sTime = DateTime.Now.AddDays(-i).Date;
                times.Add(string.Format("{0:MM/dd}", sTime));
            }

            quantitys.Add("91.5");
            quantitys.Add("93.8");
            quantitys.Add("82.7");
            quantitys.Add("88.5");
            quantitys.Add("90.4");
            quantitys.Add("83.2");
            quantitys.Add("89.9");

            result.Add(times);
            result.Add(quantitys);

            return result;
        }

        /// <summary>
        /// 获取设备加工时间统计
        /// </summary>
        /// <returns></returns>
        public async Task<List<List<string>>> GetDeviceProcessingStats(int deviceId)
        {
            List<List<string>> result = new List<List<string>>();

            if (deviceId == 0)
            {
                return result;
            }

            //设备加工时间暂定写死，后期根据数据库具体计算
            //var deviceData = await _deviceService.QueryByDeviceId(deviceId.ToString());

            //if (deviceData == null)
            //{
            //    return result;
            //}

            List<string> times = new List<string>();
            List<string> quantitys = new List<string>();

            for (int i = 6; i >= 0; i--)
            {
                DateTime sTime = DateTime.Now.AddDays(-i).Date;
                times.Add(string.Format("{0:MM/dd}", sTime));
            }

            quantitys.Add("511");
            quantitys.Add("550");
            quantitys.Add("450");
            quantitys.Add("486");
            quantitys.Add("505");
            quantitys.Add("465");
            quantitys.Add("493");

            result.Add(times);
            result.Add(quantitys);

            return result;
        }

        public async Task<List<DeviceDataToScreen>> GetDeviceDatas()
        {
            var result = await _drillPanelDetailService.GetDeviceDatasToScreen(null);
            return result;
        }

        public async Task<List<DeviceDataToScreen>> GetSpecificDeviceDatas(GetDeviceListReq request)
        {
            var result = await _drillPanelDetailService.GetDeviceDatasToScreen(request);
            return result;
        }
    }
}
