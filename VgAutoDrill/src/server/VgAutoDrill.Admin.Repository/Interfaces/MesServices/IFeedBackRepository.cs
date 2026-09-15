using VgAutoDrill.Admin.Model;
using VgAutoDrill.Admin.Model.Entites.Mes;
using VgAutoDrill.Admin.Model.ViewModels.Mes.FeedBack;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProTask;

namespace VgAutoDrill.Admin.Repository.Interfaces.MesServices
{
    public interface IFeedBackRepository : IBaseRepository<FeedBack>
    {
        Task<IPageList<FeedBack>> GetList(GetFeedBackListReq req);

        Task<IPageList<Model.Entites.Mes.WorkTask>> GetTaskList(GetTaskListReq req);
    }
}
