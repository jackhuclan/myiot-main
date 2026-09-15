using Mapster;
using SqlSugar;
using VgAutoDrill.Admin.Common.Util;
using VgAutoDrill.Admin.Domain.Interfaces.User;
using VgAutoDrill.Admin.Model.Entites;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Req.User;
using VgAutoDrill.Admin.Model.ViewModels.Res.User;
using VgAutoDrill.Admin.Repository.Interfaces;
using VgAutoDrill.Admin.Repository.Interfaces.Position;
using VgAutoDrill.Admin.Repository.Interfaces.Role;
using VgAutoDrill.Admin.Repository.Interfaces.User;
using VgAutoDrill.Admin.Repository.Interfaces.UserRole;

namespace VgAutoDrill.Admin.Domain.Services.User
{
    public class UserDomainService : BaseDomainService<SysUser>, IUserDomainService
    {
        private readonly IUserRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRoleRepository _roleRepository;
        private readonly IPositionRepository _positionRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IServiceProvider _serviceProvider;

        public UserDomainService(IUnitOfWork unitOfWork,
            IServiceProvider serviceProvider,
            IRoleRepository roleRepository,
            IPositionRepository positionRepository,
            IUserRoleRepository userRoleRepository,
            IUserRepository repository)
        {
            _serviceProvider = serviceProvider;
            this._repository = repository;
            _roleRepository = roleRepository;
            _positionRepository = positionRepository;
            _userRoleRepository = userRoleRepository;
            base._baseRepository = repository;
            _unitOfWork = unitOfWork;
        }
        /// <summary>
        /// 添加修改用户信息
        /// </summary>
        /// <param name="req">用户信息</param>
        /// <param name="creatorId">操作人ID</param>
        /// <returns></returns>
        public async Task<string> AddOrUpdateUserInfo(AddUserReq req, int creatorId)
        {
            if (req.PostIds <= 0)
            {
                return "请选择岗位!";
            }

            if (req.RoleIds <= 0)
            {
                return "请选择角色!";
            }
            var model = req.Adapt<SysUser>();
            try
            {
                _unitOfWork.BeginTran();
                var flag = await _roleRepository.IsExistAsync(p => p.Id == req.RoleIds);
                if (!flag)
                {
                    throw new Exception("角色信息错误!");
                }
                flag = await _positionRepository.IsExistAsync(p => p.Id == req.PostIds);
                if (!flag)
                {
                    throw new Exception("岗位信息错误!");
                }
                model.PositionId = req.PostIds;
                model.Status = model.Status == 1 ? model.Status : 0;
                if (req.Id <= 0)
                {
                    var salt = StringHelper.GetUUID();
                    model.Password = SecurityHelper.EncryptUserPassword(model.Password, salt);
                    model.Salt = salt;
                    var sysUserModel = await _repository.FindSingleAsync(p => p.UserName.Equals(req.UserName.Trim()));
                    if (sysUserModel != null)
                    {
                        throw new Exception($"登录名{sysUserModel.UserName}已存在!");
                    }

                    model.CreateTime = DateTime.Now;
                    model.CreatorId = creatorId;
                    var userId = await _repository.AddReturnId(model);
                    if (userId <= 0)
                    {
                        throw new Exception("添加用户失败!");
                    }
                    flag = await _userRoleRepository.Add(new SysUserRole
                    {
                        UserId = userId,
                        RoleId = req.RoleIds,
                        CreateTime = DateTime.Now,
                        CreatorId = creatorId,
                    });
                    if (!flag)
                    {
                        throw new Exception("添加用户关联角色信息错误!");
                    }
                }
                else
                {
                    var sysUserModel = await _repository.FindSingleAsync(p => p.UserName.Equals(req.UserName.Trim()) && p.Id != req.Id);
                    if (sysUserModel != null)
                    {
                        throw new Exception($"登录名{sysUserModel.UserName}已存在!");
                    }
                    sysUserModel = await _repository.QueryByID(req.Id);
                    if (sysUserModel == null)
                    {
                        throw new Exception($"未找到相应用户信息!");
                    }
                    sysUserModel.ModifyTime = DateTime.Now;
                    sysUserModel.ModifierId = creatorId;
                    sysUserModel.PositionId = req.PostIds;
                    sysUserModel.Status = model.Status;
                    sysUserModel.RealName = model.RealName;
                    sysUserModel.Email = model.Email;
                    sysUserModel.Mobile = model.Mobile;
                    sysUserModel.Sex = model.Sex;
                    sysUserModel.Remark = model.Remark;
                    sysUserModel.DepartmentId = req.DepartmentId;
                    flag = await _repository.Update(sysUserModel);
                    if (!flag)
                    {
                        throw new Exception("更新用户信息错误!");
                    }
                    var userRoleModel = await _userRoleRepository.FindSingleAsync(p => p.UserId == req.Id);
                    if (userRoleModel == null)
                    {
                        throw new Exception("获取用户关联角色信息错误!");
                    }
                    userRoleModel.RoleId = req.RoleIds;
                    flag = await _userRoleRepository.Update(userRoleModel);
                    if (!flag)
                    {
                        throw new Exception("修改用户关联角色信息错误!");
                    }
                }
                _unitOfWork.CommitTran();
            }
            catch (Exception ex)
            {
                _unitOfWork.RollbackTran();
                return $"添加用户信息失败：{ex.Message}";
            }
            return "";
        }
        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns></returns>
        public async Task<string> DeleteUserById(long id)
        {
            try
            {
                _unitOfWork.BeginTran();
                var flag = await _repository.DeleteById(id);
                if (!flag)
                {
                    throw new Exception("删除用户信息失败!");
                }
                var model = await _userRoleRepository.FindSingleAsync(p => p.UserId == id);
                if (model != null)
                {
                    flag = await _userRoleRepository.DeleteAsync(p => p.UserId == id);
                    if (!flag)
                    {
                        throw new Exception("删除用户关联角色信息失败!");
                    }
                }
                _unitOfWork.CommitTran();
            }
            catch (Exception ex)
            {
                _unitOfWork.RollbackTran();
                return $"删除用户失败({ex.Message})";
            }
            return "";
        }



        /// <summary>
        /// 获取用户分页列表数据
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<PageDto<UserInfoDto>> PageList(GetUserPageListReq req)
        {
            var pageDto = new PageDto<UserInfoDto>(req.PageNum, req.PageSize);
            var result = await _repository.GetUserList(req);
            pageDto.Total = result.TotalCount;
            pageDto.List = result.ToList().Adapt<List<UserInfoDto>>();
            return pageDto;

            /*
            var sql = @"SELECT A.*,B.department_name,B.id as DeptId FROM sys_user AS A
                        LEFT JOIN sys_department AS B
                        ON A.Department_Id=B.Id";

            var whereSb = new StringBuilder();
            whereSb.Append(" Is_Deleted=0");
            var pars = new Dictionary<string, object>();
            if (!string.IsNullOrEmpty(req.UserName))
            {
                whereSb.Append(" AND User_Name like @UserName ");
                pars.Add("UserName", $"%{req.UserName}%");
            }
            if (!string.IsNullOrEmpty(req.Mobile))
            {
                whereSb.Append(" AND Mobile like @Mobile ");
                pars.Add("Mobile", $"%{req.Mobile}%");
            }
            if (req.Status > -1)
            {
                whereSb.Append(" AND Status=@Status ");
                pars.Add("Status", req.Status);
            }
            if (req.DepartmentId > -1)
            {
                whereSb.Append(" AND DeptId=@DeptId ");
                pars.Add("DeptId", req.DepartmentId);
            }
            if (!string.IsNullOrEmpty(req.Params.BeginTime) && !string.IsNullOrEmpty(req.Params.EndTime))
            {
                whereSb.Append(" AND Create_Time >= @StartTime and Create_Time <= @EndTime ");
                pars.Add("StartTime", $"{Convert.ToDateTime(req.Params.BeginTime).ToString("yyyy-MM-dd")} 00:00:00");
                pars.Add("EndTime", $"{Convert.ToDateTime(req.Params.EndTime).ToString("yyyy-MM-dd")} 23:59:59");
            }
            var orderBy = "Is_Admin DESC,Modify_Time DESC";

            

            var result = await _repository.QueryPageAsync(sql, whereSb.ToString(), pars, orderBy, req.PageNum, req.PageSize);
            pageDto.Total = result.TotalCount;
            pageDto.List = result.ToList().Adapt<List<UserInfoDto>>();
            return pageDto;
            */
        }
        /// <summary>
        /// 批量添加用户信息
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public async Task<bool> BatchAdd(List<SysUser> list)
        {
            var isSave = await _baseRepository.BulkInsert(list);
            return isSave;
        }


        public async Task<List<SysUser>> GetListByIds(List<int> userIds)
        {
            if (userIds?.Count <= 0)
            {
                return [];
            }
            return await _repository.QueryAsync(s => userIds!.Contains(s.Id), s => s.CreateTime, OrderByType.Asc); ;
        }
    }
}
