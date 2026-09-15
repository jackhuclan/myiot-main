using SqlSugar;
using VgAutoDrill.Admin.Model;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceRecordsSummary;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;
using VgAutoDrill.Fundation.Iot;

namespace VgAutoDrill.Admin.Repository.Repository.MesServices
{
    public class DeviceRecordsSummaryRepository : BaseRepository<DeviceRecordsSummary>, IDeviceRecordsSummaryRepository
    {
        public DeviceRecordsSummaryRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public async Task<PageList<DeviceRecordsSummaryDto>> GetList(GetDeviceRecordsSummaryListReq req)
        {
            var query = DBClient.Queryable<DeviceRecordsSummary, Device, WorkStation, RouteProcessAndWorkStation, RouteAndProcess, Route>
                            ((dr, d, w, rpW, rp, r) => new object[]
                            {
                                JoinType.Left, dr.DeviceCode == d.Code,
                                JoinType.Left, d.WorkStationId == w.Id,
                                JoinType.Left, w.Id == rpW.WorkStationId,
                                JoinType.Left, rpW.RouteAndProcessId == rp.Id,
                                JoinType.Left, rp.RouteId ==r.Id,
                            });

            query = query.Where((dr, d, w, rpW, rp, r) => dr.IsDeleted == 0);

            if (!string.IsNullOrEmpty(req.DeviceCode))
            {
                query = query.Where((dr, d, w, rpW, rp, r) => !string.IsNullOrEmpty(dr.DeviceCode) && dr.DeviceCode.ToLower().Contains(req.DeviceCode.ToLower()));
            }

            if (req.QueryStartTime != null)
            {
                query = query.Where((dr, d, w, rpW, rp, r) => dr.CreateTime >= req.QueryStartTime.Value);
            }
            if (req.QueryEndTime != null)
            {
                query = query.Where((dr, d, w, rpW, rp, r) => dr.CreateTime <= req.QueryEndTime.Value);
            }

            if (req.RouteCodeList != null && req.RouteCodeList.Count > 0)
            {
                query = query.Where((dr, d, w, rpW, rp, r) => !string.IsNullOrEmpty(r.Code) && req.RouteCodeList.Contains(r.Code));
            }

            if (req.Sailings > -1)
            {
                query = query.Where((dr, d, w, rpW, rp, r) => dr.Sailings == req.Sailings);
            }

            if (req.DeviceCodeList != null && req.DeviceCodeList.Count > 0)
            {
                query = query.Where((dr, d, w, rpW, rp, r) => !string.IsNullOrEmpty(dr.DeviceCode) && req.DeviceCodeList.Contains(dr.DeviceCode.ToLower()));
            }

            if (req.QueryOrderBy == null)
            {
                if (req.OrderByWaitTime == null)
                {
                    query = query.OrderBy((dr, d, w, rpW, rp, r) => dr.DeviceCode);
                    query = query.OrderBy((dr, d, w, rpW, rp, r) => dr.CreateTime);
                    query = query.OrderBy((dr, d, w, rpW, rp, r) => dr.Sailings);
                }
                else
                {
                    switch (req.OrderByWaitTime)
                    {
                        case 0:
                            query = query.OrderBy((dr, d, w, rpW, rp, r) => dr.WaitTime);
                            break;

                        case 1:
                            query = query.OrderByDescending((dr, d, w, rpW, rp, r) => dr.WaitTime);
                            break;
                    }
                }
            }
            else
            {
                switch (req.QueryOrderBy)
                {
                    case QueryOrderByEnum.OrderByCodeDesc:
                        if (req.OrderByWaitTime == null)
                        {
                            query = query.OrderByDescending((dr, d, w, rpW, rp, r) => dr.DeviceCode);
                            query = query.OrderBy((dr, d, w, rpW, rp, r) => dr.CreateTime);
                            query = query.OrderBy((dr, d, w, rpW, rp, r) => dr.Sailings);
                        }
                        else
                        {
                            query = query.OrderByDescending((dr, d, w, rpW, rp, r) => dr.DeviceCode);
                            switch (req.OrderByWaitTime)
                            {
                                case 0:
                                    query = query.OrderBy((dr, d, w, rpW, rp, r) => dr.WaitTime);
                                    break;

                                case 1:
                                    query = query.OrderByDescending((dr, d, w, rpW, rp, r) => dr.WaitTime);
                                    break;
                            }
                        }
                        break;

                    case QueryOrderByEnum.OrderByCodeASC:
                        if (req.OrderByWaitTime == null)
                        {
                            query = query.OrderBy((dr, d, w, rpW, rp, r) => dr.DeviceCode);
                            query = query.OrderBy((dr, d, w, rpW, rp, r) => dr.CreateTime);
                            query = query.OrderBy((dr, d, w, rpW, rp, r) => dr.Sailings);
                        }
                        else
                        {
                            query = query.OrderBy((dr, d, w, rpW, rp, r) => dr.DeviceCode);
                            switch (req.OrderByWaitTime)
                            {
                                case 0:
                                    query = query.OrderBy((dr, d, w, rpW, rp, r) => dr.WaitTime);
                                    break;

                                case 1:
                                    query = query.OrderByDescending((dr, d, w, rpW, rp, r) => dr.WaitTime);
                                    break;
                            }
                        }
                        break;

                    case QueryOrderByEnum.OrderByCreateTimeDesc:
                        if (req.OrderByWaitTime == null)
                        {
                            query = query.OrderByDescending((dr, d, w, rpW, rp, r) => dr.CreateTime.Date);
                            query = query.OrderByDescending((dr, d, w, rpW, rp, r) => dr.DeviceCode);
                            query = query.OrderBy((dr, d, w, rpW, rp, r) => dr.Sailings);
                        }
                        else
                        {
                            query = query.OrderByDescending((dr, d, w, rpW, rp, r) => dr.CreateTime);
                            switch (req.OrderByWaitTime)
                            {
                                case 0:
                                    query = query.OrderBy((dr, d, w, rpW, rp, r) => dr.WaitTime);
                                    break;

                                case 1:
                                    query = query.OrderByDescending((dr, d, w, rpW, rp, r) => dr.WaitTime);
                                    break;
                            }
                        }
                        break;

                    case QueryOrderByEnum.OrderByCreateTimeASC:
                        if (req.OrderByWaitTime == null)
                        {
                            query = query.OrderBy((dr, d, w, rpW, rp, r) => dr.CreateTime.Date);
                            query = query.OrderBy((dr, d, w, rpW, rp, r) => dr.DeviceCode);
                            query = query.OrderBy((dr, d, w, rpW, rp, r) => dr.Sailings);
                        }
                        else
                        {
                            query = query.OrderBy((dr, d, w, rpW, rp, r) => dr.CreateTime);
                            switch (req.OrderByWaitTime)
                            {
                                case 0:
                                    query = query.OrderBy((dr, d, w, rpW, rp, r) => dr.WaitTime);
                                    break;

                                case 1:
                                    query = query.OrderByDescending((dr, d, w, rpW, rp, r) => dr.WaitTime);
                                    break;
                            }
                        }
                        break;

                    default:
                        if (req.OrderByWaitTime == null)
                        {
                            query = query.OrderBy((dr, d, w, rpW, rp, r) => dr.CreateTime.Date);
                            query = query.OrderBy((dr, d, w, rpW, rp, r) => dr.DeviceCode);
                            query = query.OrderBy((dr, d, w, rpW, rp, r) => dr.Sailings);
                        }
                        else
                        {
                            query = query.OrderBy((dr, d, w, rpW, rp, r) => dr.CreateTime);
                            switch (req.OrderByWaitTime)
                            {
                                case 0:
                                    query = query.OrderBy((dr, d, w, rpW, rp, r) => dr.WaitTime);
                                    break;

                                case 1:
                                    query = query.OrderByDescending((dr, d, w, rpW, rp, r) => dr.WaitTime);
                                    break;
                            }
                        }
                        break;
                }
            }

            RefAsync<int> totalCount = 0;
            var data = await query.Select((dr, d, w, rpW, rp, r) => new DeviceRecordsSummaryDto
            {
                DeviceCode = dr.DeviceCode,
                DateString = dr.DateString,
                Duty = dr.Duty,
                CollectClearTime = dr.CollectClearTime,
                CreateTime = dr.CreateTime,
                CreatorId = dr.CreatorId,
                RouteCode = r.Code,
                EndToStartTime = dr.EndToStartTime,
                ErrorTime = dr.ErrorTime,
                Id = dr.Id,
                ModifierId = dr.ModifierId,
                ModifyTime = dr.ModifyTime,
                OpenTime = dr.OpenTime,
                WaitTime = dr.WaitTime,
                WorkTime = dr.WorkTime,
                Status = dr.Status,
                Sailings = dr.Sailings,
                WithoutTaskTime = dr.WithoutTaskTime,
                WithoutDrillFileTime = dr.WithoutDrillFileTime,
                WithoutPanelTime = dr.WithoutPanelTime,
                AlarmTime = dr.AlarmTime,
                ToolLifeExporedTime = dr.ToolLifeExporedTime,
                ToolLifeExporedCount = dr.ToolLifeExporedCount,
                RunTime = dr.RunTime,
                BufferClinkerExistTime = dr.BufferClinkerExistTime,
                BufferNoBoardTime = dr.BufferNoBoardTime,
                DeviceDisableTime = dr.DeviceDisableTime,
                DrillRawExistToRunTime = dr.DrillRawExistToRunTime,
                BufferRawCompleteTime = dr.BufferRawCompleteTime,
                BufferAutomaticTime = dr.BufferAutomaticTime,
                DrillToolEvaluationTime = dr.DrillToolEvaluationTime,
                DrillTestPinTime = dr.DrillTestPinTime,
                DrillBoardDirectionTime = dr.DrillBoardDirectionTime,
                BufferManualTime = dr.BufferManualTime,
                DrillNoVacuumCount = dr.DrillNoVacuumCount,
                DrillNoVacuumStandardTime = dr.DrillNoVacuumStandardTime,
                ToolLifeExporedChangeStandardTime = dr.ToolLifeExporedChangeStandardTime,
                NoSwitchMaterialToolChangeCount = dr.NoSwitchMaterialToolChangeCount,
                NoSwitchMaterialToolChangeStandardTime = dr.NoSwitchMaterialToolChangeStandardTime,
                SwitchMaterialToolChangeCount = dr.SwitchMaterialToolChangeCount,
                SwitchMaterialToolChangeStandardTime = dr.SwitchMaterialToolChangeStandardTime,
                PINReviseCount = dr.PINReviseCount,
                PINReviseStandardTime = dr.PINReviseStandardTime,
                DetectSwingTorqueCount = dr.DetectSwingTorqueCount,
                DetectSwingTorqueStandardTime = dr.DetectSwingTorqueStandardTime,
                PressureFootChangeCount = dr.PressureFootChangeCount,
                PressureFootChangeStandardTime = dr.PressureFootChangeStandardTime,
                MinMultilayerBoardsStandardValue = dr.MinMultilayerBoardsStandardValue,
                TwoBoardsWaitFirstResultCount = dr.TwoBoardsWaitFirstResultCount,
                TwoBoardsWaitFirstResultStandardTime = dr.TwoBoardsWaitFirstResultStandardTime,
                MultilayerBoardWaitFirstResultCount = dr.MultilayerBoardWaitFirstResultCount,
                MultilayerBoardWaitFirstResultStandardTime = dr.MultilayerBoardWaitFirstResultStandardTime,
                TheoryDuty = dr.TheoryDuty,
                DutyRate = dr.DutyRate,
                NecessaryTime = dr.NecessaryTime,
            }).ToPageListAsync(req.PageNum, req.PageSize, totalCount);

            int pageCount = req.PageSize > 0 ? (int)Math.Ceiling((double)totalCount / req.PageSize) : 0;
            if (pageCount < req.PageNum && (data == null || data.Count == 0))
            {
                req.PageNum = pageCount;
                data = await query.Select((dr, d, w, rpW, rp, r) => new DeviceRecordsSummaryDto
                {
                    DeviceCode = dr.DeviceCode,
                    DateString = dr.DateString,
                    Duty = dr.Duty,
                    CollectClearTime = dr.CollectClearTime,
                    CreateTime = dr.CreateTime,
                    CreatorId = dr.CreatorId,
                    RouteCode = r.Code,
                    EndToStartTime = dr.EndToStartTime,
                    ErrorTime = dr.ErrorTime,
                    Id = dr.Id,
                    ModifierId = dr.ModifierId,
                    ModifyTime = dr.ModifyTime,
                    OpenTime = dr.OpenTime,
                    WaitTime = dr.WaitTime,
                    WorkTime = dr.WorkTime,
                    Status = dr.Status,
                    Sailings = dr.Sailings,
                    WithoutTaskTime = dr.WithoutTaskTime,
                    WithoutPanelTime = dr.WithoutPanelTime,
                    WithoutDrillFileTime = dr.WithoutDrillFileTime,
                    AlarmTime = dr.AlarmTime,
                    ToolLifeExporedTime = dr.ToolLifeExporedTime,
                    ToolLifeExporedCount = dr.ToolLifeExporedCount,
                    RunTime = dr.RunTime,
                    BufferClinkerExistTime = dr.BufferClinkerExistTime,
                    BufferNoBoardTime = dr.BufferNoBoardTime,
                    DeviceDisableTime = dr.DeviceDisableTime,
                    BufferAutomaticTime = dr.BufferAutomaticTime,
                    BufferRawCompleteTime = dr.BufferRawCompleteTime,
                    DrillRawExistToRunTime = dr.DrillRawExistToRunTime,
                    DrillBoardDirectionTime = dr.DrillBoardDirectionTime,
                    DrillTestPinTime = dr.DrillTestPinTime,
                    DrillToolEvaluationTime = dr.DrillToolEvaluationTime,
                    BufferManualTime = dr.BufferManualTime,
                    DrillNoVacuumCount = dr.DrillNoVacuumCount,
                    DrillNoVacuumStandardTime = dr.DrillNoVacuumStandardTime,
                    ToolLifeExporedChangeStandardTime = dr.ToolLifeExporedChangeStandardTime,
                    NoSwitchMaterialToolChangeCount = dr.NoSwitchMaterialToolChangeCount,
                    NoSwitchMaterialToolChangeStandardTime = dr.NoSwitchMaterialToolChangeStandardTime,
                    SwitchMaterialToolChangeCount = dr.SwitchMaterialToolChangeCount,
                    SwitchMaterialToolChangeStandardTime = dr.SwitchMaterialToolChangeStandardTime,
                    PINReviseCount = dr.PINReviseCount,
                    PINReviseStandardTime = dr.PINReviseStandardTime,
                    DetectSwingTorqueCount = dr.DetectSwingTorqueCount,
                    DetectSwingTorqueStandardTime = dr.DetectSwingTorqueStandardTime,
                    PressureFootChangeCount = dr.PressureFootChangeCount,
                    PressureFootChangeStandardTime = dr.PressureFootChangeStandardTime,
                    MinMultilayerBoardsStandardValue = dr.MinMultilayerBoardsStandardValue,
                    TwoBoardsWaitFirstResultCount = dr.TwoBoardsWaitFirstResultCount,
                    TwoBoardsWaitFirstResultStandardTime = dr.TwoBoardsWaitFirstResultStandardTime,
                    MultilayerBoardWaitFirstResultCount = dr.MultilayerBoardWaitFirstResultCount,
                    MultilayerBoardWaitFirstResultStandardTime = dr.MultilayerBoardWaitFirstResultStandardTime,
                    TheoryDuty = dr.TheoryDuty,
                    DutyRate = dr.DutyRate,
                }).ToPageListAsync(req.PageNum, req.PageSize, totalCount);
            }
            var list = new PageList<DeviceRecordsSummaryDto>(data, req.PageNum, req.PageSize, totalCount);
            return list;
        }

        public async Task<DateTime?> GetMaxDate()
        {
            var query = DBClient.Queryable<DeviceRecordsSummary>();

            query = query.Where(p => !string.IsNullOrEmpty(p.DeviceCode));
            query = query.OrderByDescending(p => p.CreateTime);

            var data = await query.FirstAsync();
            if (data == null)
            {
                return null;
            }

            return data.CreateTime;
        }

    }
}
