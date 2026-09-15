using VgAutoDrill.Admin.Model.ViewModels;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Schedule;

namespace VgAutoDrill.Central.Core.Mes.Interface;

public interface IItemAdapter
{
    Task<Item> GetItemAsync(string itemCode, string incodeNumber);

    Task<object> GetItemCode(QueryItemCodeRequest itemListReq);

    Task<Item> GetItemDataAsync(string itemCode);

    Task<ResponseDto<Item>> SetItemDrillFilePath(string itemCode, string drillFilePath);

    //
    // 摘要:
    //     获取产品代码列表
    //
    // 参数:
    //   req:
    Task<List<QueryItemCodeResponse>> GetProductCodes(QueryItemCodeRequest req);

    Task<bool> ProduceItemData(Item item);

    /// <summary>
    /// 从数据库加载板料基本信息
    /// </summary>
    /// <param name="panels"></param>
    /// <returns></returns>
    Task<string> LoadPanelInfo(PanelList panels);
}
