using SqlSugar;
using VgAutoDrill.Admin.Model;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.DeviceAndSubject;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.MesServices;

namespace VgAutoDrill.Admin.Repository.Repository.MesServices
{
    public class DeviceAndSubjectRepository : BaseRepository<DeviceAndSubject>, IDeviceAndSubjectRepository
    {
        public DeviceAndSubjectRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public async Task<PageList<DeviceAndSubjectDto>> GetList(GetDeviceAndSubjectListReq req)
        {
            var query = DBClient.Queryable<DeviceAndSubject, Subject>
                ((r, p) => new object[]
                    {
                        JoinType.Left, r.SubjectId == p.Id
                    });

            query = query.Where((r, p) => r.IsDeleted == 0 && p.IsDeleted == 0);

            if (req.SubjectId > 0)
            {
                query = query.Where((r, p) => r.SubjectId == req.SubjectId);
            }

            if (req.DeviceId > 0)
            {
                query = query.Where((r, p) => r.DeviceId == req.DeviceId);
            }

            query = query.OrderByDescending((r, p) => r.CreateTime);

            RefAsync<int> totalCount = 0;
            var data = await query.Select((r, p) => new DeviceAndSubjectDto
            {
                Id = r.Id,
                DeviceId = r.DeviceId,
                SubjectId = r.SubjectId,
                SubjectCode = p.Code,
                SubjectName = p.Name,
                Standard = p.Standard,
                Status = r.Status,
            }).ToPageListAsync(req.PageNum, req.PageSize, totalCount);

            int pageCount = req.PageSize > 0 ? (int)Math.Ceiling((double)totalCount / req.PageSize) : 0;
            if (pageCount < req.PageNum && (data == null || data.Count == 0))
            {
                req.PageNum = pageCount;
                data = await query.Select((r, p) => new DeviceAndSubjectDto
                {
                    Id = r.Id,
                    DeviceId = r.DeviceId,
                    SubjectId = r.SubjectId,
                    SubjectCode = p.Code,
                    SubjectName = p.Name,
                    Standard = p.Standard,
                    Status = r.Status,
                }).ToPageListAsync(req.PageNum, req.PageSize, totalCount);
            }

            var list = new PageList<DeviceAndSubjectDto>(data, req.PageNum, req.PageSize, totalCount);
            return list;
        }
    }
}
