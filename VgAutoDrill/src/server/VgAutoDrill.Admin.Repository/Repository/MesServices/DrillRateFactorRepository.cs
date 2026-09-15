using SqlSugar;
using VgAutoDrill.Admin.Common.Util;
using VgAutoDrill.Admin.Model.CentralModels;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DrillRateFactor;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Repository.Repository.MesServices
{
    public class DrillRateFactorRepository : BaseRepository<DrillRateFactor>, IDrillRateFactorRepository
    {
        public DrillRateFactorRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public async Task<List<DrillRateFactorSummaryDto>> GetListByDate(List<string> deviceCodes, DateTime startDate, DateTime endDate)
        {
            var drillRateFactorData = new List<DrillRateFactorSummaryDto>();

            var listMiddle = await DBClient.Queryable<DrillRateFactor>()
                            .Where(p => !string.IsNullOrEmpty(p.DeviceId) && deviceCodes.Contains(p.DeviceId.ToLower())
                            && p.StartTime >= startDate && p.EndTime < endDate && p.StartTime <= p.EndTime)
                            .Select(p => p).ToListAsync();

            if (listMiddle != null && listMiddle.Count > 0)
            {
                foreach (var item in listMiddle)
                {
                    var deviceData = new DrillRateFactorSummaryDto();

                    deviceData.DeviceId = item.DeviceId;
                    deviceData.Reason = item.Reason;
                    deviceData.ReasonTime = Convert.ToInt32((item.EndTime - item.StartTime).Value.TotalMinutes);
                    deviceData.ReasonTimeTotalSeconds = Convert.ToInt32((item.EndTime - item.StartTime).Value.TotalSeconds);
                    drillRateFactorData.Add(deviceData);
                }
            }

            var listStart = await DBClient.Queryable<DrillRateFactor>()
                            .Where(p => !string.IsNullOrEmpty(p.DeviceId) && deviceCodes.Contains(p.DeviceId.ToLower())
                            && p.StartTime < startDate && p.EndTime > startDate && p.EndTime < endDate)
                            .Select(p => p).ToListAsync();

            if (listStart != null && listStart.Count > 0)
            {
                foreach (var item in listStart)
                {
                    var deviceData = new DrillRateFactorSummaryDto();

                    deviceData.DeviceId = item.DeviceId;
                    deviceData.Reason = item.Reason;
                    deviceData.ReasonTime = Convert.ToInt32((item.EndTime - startDate).Value.TotalMinutes);
                    deviceData.ReasonTimeTotalSeconds = Convert.ToInt32((item.EndTime - item.StartTime).Value.TotalSeconds);
                    drillRateFactorData.Add(deviceData);
                }
            }

            var listEnd = await DBClient.Queryable<DrillRateFactor>()
                            .Where(p => !string.IsNullOrEmpty(p.DeviceId) && deviceCodes.Contains(p.DeviceId.ToLower())
                            && p.StartTime >= startDate && p.StartTime < endDate
                            && (p.EndTime > endDate || p.EndTime == null || p.EndTime < p.StartTime))
                            .Select(p => p).ToListAsync();

            if (listEnd != null && listEnd.Count > 0)
            {
                foreach (var item in listEnd)
                {
                    var deviceData = new DrillRateFactorSummaryDto();

                    deviceData.DeviceId = item.DeviceId;
                    deviceData.Reason = item.Reason;
                    if (item.EndTime < item.StartTime && item.ModifyTime >= item.StartTime)
                    {
                        deviceData.ReasonTime = Convert.ToInt32((item.ModifyTime - item.StartTime).Value.TotalMinutes);
                        deviceData.ReasonTimeTotalSeconds = Convert.ToInt32((item.ModifyTime - item.StartTime).Value.TotalSeconds);
                    }
                    else
                    {
                        DateTime date = endDate > DateTime.Now ? DateTime.Now : endDate;
                        deviceData.ReasonTime = date < item.StartTime ? 0 : Convert.ToInt32((date - item.StartTime).Value.TotalMinutes);
                        deviceData.ReasonTimeTotalSeconds = date < item.StartTime ? 0 : Convert.ToInt32((date - item.StartTime).Value.TotalSeconds);
                    }

                    drillRateFactorData.Add(deviceData);
                }
            }

            var listSE = await DBClient.Queryable<DrillRateFactor>()
                            .Where(p => !string.IsNullOrEmpty(p.DeviceId) && deviceCodes.Contains(p.DeviceId.ToLower())
                            && p.StartTime < startDate && (p.EndTime > endDate || p.EndTime == null))
                            .Select(p => p).ToListAsync();

            if (listSE != null && listSE.Count > 0)
            {
                foreach (var item in listSE)
                {
                    var deviceData = new DrillRateFactorSummaryDto();

                    deviceData.DeviceId = item.DeviceId;
                    deviceData.Reason = item.Reason;

                    DateTime date = endDate > DateTime.Now ? DateTime.Now : endDate;
                    deviceData.ReasonTime = Convert.ToInt32((date - startDate).TotalMinutes);
                    deviceData.ReasonTimeTotalSeconds = Convert.ToInt32((date - startDate).TotalSeconds);
                    drillRateFactorData.Add(deviceData);
                }
            }

            var resultData = new List<DrillRateFactorSummaryDto>();
            foreach (var deviceId in deviceCodes)
            {
                var datas = drillRateFactorData.Where(p => p.DeviceId.ToLower() == deviceId.ToLower()).GroupBy(p => p.Reason).ToList();
                if (datas == null || datas.Count == 0)
                {
                    continue;
                }

                foreach (var item in datas)
                {
                    DrillRateFactorSummaryDto model = new DrillRateFactorSummaryDto();
                    model.DeviceId = item.ToList()[0].DeviceId;
                    model.Reason = item.Key;
                    model.ReasonTime = item.ToList().Sum(p => p.ReasonTime);
                    model.ReasonTimeTotalSeconds = item.ToList().Sum(p => p.ReasonTimeTotalSeconds);
                    model.ReasonCount = item.ToList().Count;

                    resultData.Add(model);
                }
            }

            return resultData;
        }

        public async Task<List<DrillRateFactorDto>> GetRateReasonDetails(string deviceCode, DateTime startDate, DateTime endDate)
        {
            var drillRateFactorData = new List<DrillRateFactorDto>();

            var listMiddle = await DBClient.Queryable<DrillRateFactor>()
                            .Where(p => !string.IsNullOrEmpty(p.DeviceId) && deviceCode.ToLower() == p.DeviceId.ToLower()
                            && p.StartTime >= startDate && p.EndTime < endDate)
                            .Select(p => p).ToListAsync();

            if (listMiddle != null && listMiddle.Count > 0)
            {
                foreach (var item in listMiddle)
                {
                    var deviceData = new DrillRateFactorDto();

                    deviceData.DeviceId = item.DeviceId;
                    deviceData.Reason = item.Reason;
                    deviceData.ReasonDesc = GetDescriptionByEnum<DrillRateFactorReason>.GetEnumDescription((DrillRateFactorReason)item.Reason);
                    deviceData.EndTime = item.EndTime;
                    deviceData.StartTime = item.StartTime;
                    deviceData.IntervalTime = Convert.ToInt32((item.EndTime - item.StartTime).Value.TotalMinutes);
                    deviceData.Id = item.Id;
                    deviceData.CreateTime = item.CreateTime;
                    deviceData.LocationCode = item.LocationCode;
                    deviceData.Memo = item.Memo;

                    drillRateFactorData.Add(deviceData);
                }
            }

            var listStart = await DBClient.Queryable<DrillRateFactor>()
                            .Where(p => !string.IsNullOrEmpty(p.DeviceId) && deviceCode.ToLower() == p.DeviceId.ToLower()
                            && p.StartTime < startDate && p.EndTime > startDate && p.EndTime < endDate)
                            .Select(p => p).ToListAsync();

            if (listStart != null && listStart.Count > 0)
            {
                foreach (var item in listStart)
                {
                    var deviceData = new DrillRateFactorDto();

                    deviceData.DeviceId = item.DeviceId;
                    deviceData.Reason = item.Reason;
                    deviceData.ReasonDesc = GetDescriptionByEnum<DrillRateFactorReason>.GetEnumDescription((DrillRateFactorReason)item.Reason);
                    deviceData.EndTime = item.EndTime;
                    deviceData.StartTime = startDate;
                    deviceData.IntervalTime = Convert.ToInt32((item.EndTime - startDate).Value.TotalMinutes);
                    deviceData.Id = item.Id;
                    deviceData.CreateTime = item.CreateTime;
                    deviceData.LocationCode = item.LocationCode;
                    deviceData.Memo = item.Memo;

                    drillRateFactorData.Add(deviceData);
                }
            }

            var listEnd = await DBClient.Queryable<DrillRateFactor>()
                            .Where(p => !string.IsNullOrEmpty(p.DeviceId) && deviceCode.ToLower() == p.DeviceId.ToLower()
                            && p.StartTime >= startDate && p.StartTime < endDate && (p.EndTime > endDate || p.EndTime == null))
                            .Select(p => p).ToListAsync();

            if (listEnd != null && listEnd.Count > 0)
            {
                foreach (var item in listEnd)
                {
                    var deviceData = new DrillRateFactorDto();

                    deviceData.DeviceId = item.DeviceId;
                    deviceData.Reason = item.Reason;
                    deviceData.ReasonDesc = GetDescriptionByEnum<DrillRateFactorReason>.GetEnumDescription((DrillRateFactorReason)item.Reason);
                    deviceData.EndTime = endDate > DateTime.Now ? DateTime.Now : endDate;
                    deviceData.StartTime = item.StartTime;
                    deviceData.IntervalTime = Convert.ToInt32((deviceData.EndTime - item.StartTime).Value.TotalMinutes);
                    deviceData.Id = item.Id;
                    deviceData.CreateTime = item.CreateTime;
                    deviceData.LocationCode = item.LocationCode;
                    deviceData.Memo = item.Memo;

                    drillRateFactorData.Add(deviceData);
                }
            }

            var listSE = await DBClient.Queryable<DrillRateFactor>()
                            .Where(p => !string.IsNullOrEmpty(p.DeviceId) && deviceCode.ToLower() == p.DeviceId.ToLower()
                            && p.StartTime < startDate && (p.EndTime > endDate || p.EndTime == null))
                            .Select(p => p).ToListAsync();

            if (listSE != null && listSE.Count > 0)
            {
                foreach (var item in listSE)
                {
                    var deviceData = new DrillRateFactorDto();

                    deviceData.DeviceId = item.DeviceId;
                    deviceData.Reason = item.Reason;
                    deviceData.ReasonDesc = GetDescriptionByEnum<DrillRateFactorReason>.GetEnumDescription((DrillRateFactorReason)item.Reason);
                    deviceData.EndTime = endDate > DateTime.Now ? DateTime.Now : endDate;
                    deviceData.StartTime = startDate;
                    deviceData.IntervalTime = Convert.ToInt32((deviceData.EndTime - startDate).Value.TotalMinutes);
                    deviceData.Id = item.Id;
                    deviceData.CreateTime = item.CreateTime;
                    deviceData.LocationCode = item.LocationCode;
                    deviceData.Memo = item.Memo;

                    drillRateFactorData.Add(deviceData);
                }
            }

            return drillRateFactorData;
        }

        public async Task<List<DrillRateFactor>> GetLastRateReasonDatas(List<string> deviceCodes, DateTime startDate, DateTime endDate)
        {
            var dataList = new List<DrillRateFactor>();

            var listMiddle = await DBClient.Queryable<DrillRateFactor>()
                            .Where(p => !string.IsNullOrEmpty(p.DeviceId) && deviceCodes.Contains(p.DeviceId.ToLower())
                            && p.StartTime >= startDate && p.EndTime < endDate)
                            .GroupBy(p => new { p.DeviceId })
                            .Select(p => new
                            {
                                deviceCode = p.DeviceId,
                                singleData = SqlFunc.Subqueryable<DrillRateFactor>()
                                .Where(s => s.DeviceId == p.DeviceId && s.StartTime >= startDate && s.EndTime < endDate)
                                .OrderByDesc(s => s.StartTime)
                                .First()
                            }).ToListAsync();

            if (listMiddle != null && listMiddle.Count > 0)
            {
                foreach (var item in listMiddle)
                {
                    dataList.Add(item.singleData);
                }
            }

            var listStart = await DBClient.Queryable<DrillRateFactor>()
                            .Where(p => !string.IsNullOrEmpty(p.DeviceId) && deviceCodes.Contains(p.DeviceId.ToLower())
                            && p.StartTime < startDate && p.EndTime > startDate && p.EndTime < endDate)
                            .GroupBy(p => new { p.DeviceId })
                            .Select(p => new
                            {
                                deviceCode = p.DeviceId,
                                singleData = SqlFunc.Subqueryable<DrillRateFactor>()
                                .Where(s => s.DeviceId == s.DeviceId && s.StartTime < startDate && s.EndTime > startDate && s.EndTime < endDate)
                                .OrderByDesc(s => s.StartTime)
                                .First()
                            }).ToListAsync();

            if (listStart != null && listStart.Count > 0)
            {
                foreach (var item in listStart)
                {
                    item.singleData.StartTime = startDate;
                    dataList.Add(item.singleData);
                }
            }

            var listEnd = await DBClient.Queryable<DrillRateFactor>()
                            .Where(p => !string.IsNullOrEmpty(p.DeviceId) && deviceCodes.Contains(p.DeviceId.ToLower())
                            && p.StartTime >= startDate && p.StartTime < endDate && (p.EndTime > endDate || p.EndTime == null))
                            .GroupBy(p => new { p.DeviceId })
                            .Select(p => new
                            {
                                deviceCode = p.DeviceId,
                                singleData = SqlFunc.Subqueryable<DrillRateFactor>()
                                .Where(s => s.DeviceId == p.DeviceId && s.StartTime >= startDate && s.StartTime < endDate && (s.EndTime > endDate || s.EndTime == null))
                                .OrderByDesc(s => s.StartTime)
                                .First()
                            }).ToListAsync();

            if (listEnd != null && listEnd.Count > 0)
            {
                foreach (var item in listEnd)
                {
                    item.singleData.EndTime = endDate > DateTime.Now ? DateTime.Now : endDate;
                    dataList.Add(item.singleData);
                }
            }

            var listSE = await DBClient.Queryable<DrillRateFactor>()
                            .Where(p => !string.IsNullOrEmpty(p.DeviceId) && deviceCodes.Contains(p.DeviceId.ToLower())
                            && p.StartTime < startDate && (p.EndTime > endDate || p.EndTime == null))
                            .GroupBy(p => new { p.DeviceId })
                            .Select(p => new
                            {
                                deviceCode = p.DeviceId,
                                singleData = SqlFunc.Subqueryable<DrillRateFactor>()
                                .Where(s => s.DeviceId == p.DeviceId && s.StartTime < startDate && (s.EndTime > endDate || s.EndTime == null))
                                .OrderByDesc(s => s.StartTime)
                                .First()
                            }).ToListAsync();

            if (listSE != null && listSE.Count > 0)
            {
                foreach (var item in listSE)
                {
                    item.singleData.StartTime = startDate;
                    item.singleData.EndTime = endDate > DateTime.Now ? DateTime.Now : endDate;
                    dataList.Add(item.singleData);
                }
            }

            if (dataList == null || dataList.Count == 0)
            {
                return new List<DrillRateFactor> { };
            }

            var datas = from p in dataList
                        group p by p.DeviceId into g
                        select g.OrderByDescending(s => s.StartTime).FirstOrDefault();

            var resultDatas = new List<DrillRateFactor>();
            foreach (var item in datas)
            {
                resultDatas.Add(item);
            }
            return resultDatas;
        }
    }
}
