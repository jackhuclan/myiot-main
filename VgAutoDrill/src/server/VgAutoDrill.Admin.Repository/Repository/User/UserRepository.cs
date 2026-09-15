using SqlSugar;
using VgAutoDrill.Admin.Model;
using VgAutoDrill.Admin.Model.Entites;
using VgAutoDrill.Admin.Model.ViewModels.Req.User;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.User;

namespace VgAutoDrill.Admin.Repository.Repository.User
{
    public class UserRepository : BaseRepository<SysUser>, IUserRepository
    {
        public UserRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public async Task<IPageList<dynamic>> GetUserList(GetUserPageListReq req)
        {
            var query = DBClient.Queryable<SysUser, SysDepartment>
                ((user, dept) => new object[]
                    {
                        JoinType.Left, user.DepartmentId == dept.Id
                    });

            query = query.Where((user, dept) => user.IsDeleted == 0);

            if (!string.IsNullOrEmpty(req.UserName))
            {
                query = query.Where((user, dept) => user.UserName.Contains(req.UserName));
            }
            if (!string.IsNullOrEmpty(req.Mobile))
            {
                query = query.Where((user, dept) => user.Mobile.Contains(req.Mobile));
            }
            if (req.Status > -1)
            {
                query = query.Where((user, dept) => user.Status == req.Status);
            }
            if (req.DepartmentId > 0)
            {
                query = query.Where((user, dept) => dept.Id == req.DepartmentId);
            }
            if (!string.IsNullOrEmpty(req.Params.BeginTime) && !string.IsNullOrEmpty(req.Params.EndTime))
            {
                var start = DateTime.Parse($"{Convert.ToDateTime(req.Params.BeginTime).ToString("yyyy-MM-dd")} 00:00:00");
                var end = DateTime.Parse($"{Convert.ToDateTime(req.Params.EndTime).ToString("yyyy-MM-dd")} 23:59:59");
                query = query.Where((user, dept) => user.CreateTime >= start && user.CreateTime <= end);
            }

            query = query.OrderByDescending((user, dept) => user.IsAdmin)
                         .OrderByDescending((user, dept) => user.ModifyTime);

            RefAsync<int> totalCount = 0;
            var data = await query.Select((user, dept) => new
            {
                user.Id,
                user.Portrait,
                user.DepartmentId,
                dept.DepartmentName,
                user.UserName,
                user.RealName,
                user.Email,
                user.Mobile,
                user.Status,
                user.CreateTime
            }).ToPageListAsync(req.PageNum, req.PageSize, totalCount);

            int pageCount = req.PageSize > 0 ? (int)Math.Ceiling((double)totalCount / req.PageSize) : 0;
            if (pageCount < req.PageNum && (data == null || data.Count == 0))
            {
                req.PageNum = pageCount;
                data = await query.Select((user, dept) => new
                {
                    user.Id,
                    user.Portrait,
                    user.DepartmentId,
                    dept.DepartmentName,
                    user.UserName,
                    user.RealName,
                    user.Email,
                    user.Mobile,
                    user.Status,
                    user.CreateTime
                }).ToPageListAsync(req.PageNum, req.PageSize, totalCount);
            }

            var list = new PageList<dynamic>(data, req.PageNum, req.PageSize, totalCount);
            return list;
        }
    }
}
