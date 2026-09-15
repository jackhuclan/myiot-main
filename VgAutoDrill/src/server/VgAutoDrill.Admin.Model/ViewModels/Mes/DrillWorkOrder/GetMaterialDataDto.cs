using VgAutoDrill.Admin.Model.ViewModels.Mes.MaterialStockOverview;
using VgAutoDrill.Admin.Model.ViewModels.Mes.MaterialStockStorage;
using VgAutoDrill.Admin.Model.ViewModels.Mes.ProduceTask;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.DrillWorkOrder
{
    public class GetMaterialDataDto
    {
        public MaterialStockOverviewDto? MaterialStockOverview { get; set; }

        public PageDto<MaterialStockStorageDto>? Storages { get; set; }

        public PageDto<ProduceTaskDto>? ProduceTasks { get; set; }
    }
}
