using VgAutoDrill.Admin.Model.ViewModels.Mes.External.JingwangResponse;

namespace VgAutoDrill.Admin.Application.Interfaces.MesServices
{
    public interface IExternalGetStockInfo
    {
        Task<List<StockInfoQueryDataDto>> GetStockInfos(string urlAdress, string targetPosArea);
        Task<int> GetStockQuantity(List<StockInfoQueryDataDto> stockInfoQueryDatas, string sourceCode);
        Task<int> GetStockQuantityByLot(string itemCode);
    }
}
