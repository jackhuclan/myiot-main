using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Admin.Model.ViewModels.Mes.FeedBack;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProTask;

namespace VgAutoDrill.Admin.Domain.Interfaces.MesServices
{
    public interface IFeedBackDomainService : IBaseDomainService<FeedBack>
    {
        Task<PageDto<FeedBackDto>> GetList(GetFeedBackListReq req);

        Task<PageDto<TaskDto>> GetTaskList(GetTaskListReq req);
    }
}
