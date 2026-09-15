using VgAutoDrill.Admin.Model.ViewModels.Mes.Route;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.RouteAndProcess
{
    public class RouteInfoAndProcessInfo
    {
        //工艺路线集合
        public virtual List<RouteDto> RouteInfos { get; set; } = new List<RouteDto>();

        //第一个工艺路线下的工序集合
        public virtual List<RouteAndProcessDto> ProcessInfos { get; set; } = new List<RouteAndProcessDto>();
    }
}
