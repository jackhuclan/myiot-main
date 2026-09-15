using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using VgAutoDrill.Fundation.Iot.Extensions;
using VgAutoDrill.Fundation.Iot.Models;
using VgAutoDrill.Fundation.Iot.Transportation;

namespace VgAutoDrill.Fundation.Iot;

public class PanelList : ObservableList<Panel, DevicePanelChangedResponse>
{
    private PanelListSnapshot _panelListSnapshot = null;
    public static PanelList Empty = new();
    public IReadOnlyList<ProductStatus> ProductStatuses => this.Select(x => x.ProductStatus).Distinct().ToList();

    /// <summary>
    /// 获取空位
    /// </summary>
    /// <returns></returns>
    public IReadOnlyList<Panel> EmptySiloBoxPanels => this.Where(x => ProductStatusConstants.EmptySiloBox.Contains(x.ProductStatus)).ToList();

    /// <summary>
    /// 获取钻好的(Finished_DRILL)板子, 不包含首件
    /// </summary>
    /// <returns></returns>
    public IReadOnlyList<Panel> DrilledPanels => this.Where(x => ProductStatusConstants.Finished_DRILL.Contains(x.ProductStatus)
                                                    && !string.IsNullOrEmpty(x.ItemCode)
                                                    && !x.IsFirst).ToList();

    /// <summary>
    /// 获取所有的板子 不包含首件
    /// </summary>
    /// <returns></returns>
    public IReadOnlyList<Panel> AllPanelsNotHaveFirst => this.Where(x =>
                                                      !string.IsNullOrEmpty(x.ItemCode)
                                                      && !x.IsFirst).ToList();

    /// <summary>
    /// 首件板子
    /// </summary>
    public IReadOnlyList<Panel> FirstPanels => this.Where(x => ProductStatusConstants.Finished_DRILL.Contains(x.ProductStatus)
                                                    && !string.IsNullOrEmpty(x.ItemCode)
                                                    && x.IsFirst).ToList();

    /// <summary>
    /// 所有料号
    /// </summary>
    public IReadOnlyList<string> ItemCodes => this.Where(x => !string.IsNullOrWhiteSpace(x.ItemCode)).Select(y => y.ItemCode).Distinct().ToList();

    /// <summary>
    /// 获取生料料号列表
    /// </summary>
    /// <returns></returns>
    public IReadOnlyList<string> UndrilledItemCodes => UndrilledPanels.Select(y => y.ItemCode).Distinct().ToList();
    /// <summary>
    /// 获取正在钻孔的料号列表
    /// </summary>
    /// <returns></returns>
    public IReadOnlyList<string> DrillingItemCodes => DrillingPanels.Select(y => y.ItemCode).Distinct().ToList();
    /// <summary>
    /// 获取熟料料号列表
    /// </summary>
    public IReadOnlyList<string> DrilledItemCodes => DrilledPanels.Select(y => y.ItemCode).Distinct().ToList();

    /// <summary>
    /// 获取首件料号列表
    /// </summary>
    public IReadOnlyList<string> FirstItemCodes => FirstPanels.Select(y => y.ItemCode).Distinct().ToList();

    /// <summary>
    /// 获取某个熟料料号的钻好的板子
    /// </summary>
    /// <param name="drilledItemCode">熟料料号</param>
    /// <returns></returns>
    public IReadOnlyList<Panel> GetDrilledPanels(string drilledItemCode)
    {
        return this.DrilledPanels.Where(x => x.ItemCode.ToLower() == drilledItemCode.ToLower()).ToList();
    }

    /// <summary>
    /// 获取某个熟料料号的钻好的板子
    /// </summary>
    /// <param name="drilledItemCodes">熟料料号</param>
    /// <returns></returns>
    public IReadOnlyList<Panel> GetDrilledPanels(IReadOnlyList<string> drilledItemCodes)
    {
        return this.DrilledPanels.Where(x => drilledItemCodes.Any() && drilledItemCodes.Any(i => i.ToLower() == x.ItemCode)).ToList();
    }

    /// <summary>
    /// 获取设备上未钻的板子
    /// </summary>
    /// <returns></returns>
    public IReadOnlyList<Panel> UndrilledPanels => this.Where(x => ProductStatusConstants.Finished_PIN.Contains(x.ProductStatus) && !string.IsNullOrEmpty(x.ItemCode)).ToList();
    public IReadOnlyList<Panel> DrillingPanels => this.Where(x => x.ProductStatus == ProductStatus.Drilling && !string.IsNullOrEmpty(x.ItemCode)).ToList();
    public string LocationCode
    {
        get
        {
            var panel = this.FirstOrDefault();
            return panel == null ? string.Empty : panel.LocationCode;
        }
    }

    /// <summary>
    /// 料仓号
    /// </summary>
    public string SiloCode
    {
        get
        {
            var panel = this.FirstOrDefault();
            return panel == null ? string.Empty : panel.SiloCode;
        }
    }

    public int Position
    {
        get
        {
            var panel = this.FirstOrDefault();
            return panel == null ? 1 : panel.Position;
        }
    }

    /// <summary>
    /// 获取设备上生料的板子的数量
    /// </summary>
    /// <returns></returns>
    public int CountUndrilledPanels() => UndrilledPanels.Count;

    public int CountFirstPanels() => FirstPanels.Count;

    public TransportationKind CalculateTransportationKind()
    {
        if (IsEmptySiloBox)
        {
            return TransportationKind.EmptySilo;
        }
        else if (ContainsUndrilled())
        {
            return TransportationKind.Raw;
        }
        else if (ContainsDrilled() && !ContainsUndrilled() && !ContainsFirst())
        {
            return TransportationKind.Clinker;
        }
        else if (ContainsFirst() && !ContainsUndrilled() && !ContainsDrilled())
        {
            return TransportationKind.First;
        }

        return TransportationKind.None;
    }

    /// <summary>
    /// 获取板料快照
    /// </summary>
    public PanelListSnapshot PanelSnapshot
    {
        get
        {
            var summaryInfos = this.Where(p => p != null)
                                .GroupBy(p => new { ProductStatus = ProductStatusConstants.SimplifyProductStatus(p.ProductStatus), p.ItemCode, p.SiloCode })
                                .Select(p => new PanelListSnapshotEntry
                                {
                                    SiloCode = p.Key.SiloCode,
                                    ItemCode = p.Key.ItemCode,
                                    ItemStatus = p.Key.ProductStatus,
                                    ItemCount = p.Count(),
                                }).ToList();
            var snapshot = new PanelListSnapshot(summaryInfos);

            if (_panelListSnapshot == null
                || !_panelListSnapshot.Equals(snapshot))
            {
                _panelListSnapshot = snapshot;
            }

            return _panelListSnapshot;
        }
    }

    public string SummaryPanelInfo()
    {
        if (this.IsEmptyPayload) return string.Empty;

        var summaryInfos = this.Where(p => p != null)
                            .GroupBy(p => new { ProductStatus = ProductStatusConstants.SimplifyProductStatus(p.ProductStatus), p.ItemCode, p.SiloCode })
                            .Select(p => new
                            {
                                p.Key.ItemCode,
                                p.Key.ProductStatus,
                                Count = p.Count(),
                                p.Key.SiloCode
                            })
                            .OrderBy(p => p.SiloCode).ThenByDescending(p => p.ProductStatus).ThenBy(p => p.ItemCode)
                            .ToList();

        StringBuilder sb = new StringBuilder();
        if (summaryInfos.Any())
        {
            sb.Append($"料仓：{summaryInfos.FirstOrDefault()?.SiloCode}；{Environment.NewLine}");
        }
        foreach (var item in summaryInfos)
        {
            string itemCodeStr = string.IsNullOrEmpty(item.ItemCode) ? "空" : item.ItemCode.ToString();
            sb.Append($"产品编号：{itemCodeStr}，板料状态：{ProductStatusConstants.SimplifyProductStatusName(item.ProductStatus)}，数量：{item.Count}；{Environment.NewLine}");
        }

        return sb.ToString();
    }
    
    public string SummaryDrillPanelInfo()
    {
        var panels = this.Where(p => p != null)
                        .GroupBy(p => new { p.ProductStatus, p.ItemCode, p.SiloCode, p.Layer })
                        .Select(p => new
                        {
                            p.Key.ItemCode,
                            p.Key.ProductStatus,
                            SiloCount = p.Count(),
                            p.Key.SiloCode,
                            p.Key.Layer,
                        }).ToList();

        StringBuilder sb = new StringBuilder();
        foreach (var item in panels)
        {
            string itemCodeStr = string.IsNullOrEmpty(item.ItemCode) ? "空" : item.ItemCode.ToString();
            string layerStr = "";
            switch (item.Layer)
            {
                case 0:
                    layerStr = "生料：";
                    break;

                case 1:
                    layerStr = "钻机：";
                    break;

                case 2:
                    layerStr = "熟料：";
                    break;
            }

            sb.Append($"{layerStr}产品编号：{itemCodeStr}，状态：{ProductStatusConstants.SimplifyProductStatusName(item.ProductStatus)}，数量：{item.SiloCount}；{Environment.NewLine}");
        }

        return sb.ToString();
    }

    /// <summary>
    /// 库位编号是否有效，一个库位只有一个库位号
    /// </summary>
    public bool LocationCodeIsValid => this.Any() && this.Select(x => x.LocationCode)
        .Where(x => !string.IsNullOrWhiteSpace(x))
        .Distinct()
        .Count() == 1;

    /// <summary>
    /// 料仓号是否有效，一个料仓只有一个料仓号
    /// </summary>
    public bool SiloCodeIsValid => this.Any() && this.Select(x => x.SiloCode)
        .Where(x => !string.IsNullOrWhiteSpace(x))
        .Distinct()
        .Count() == 1;

    /// <summary>
    /// 是否存在某个料号的板子
    /// </summary>
    /// <param name="itemCode">料号</param>
    /// <returns></returns>
    public bool HasPanelWithItemCode(string itemCode)
    {
        return this.Any(x => !string.IsNullOrEmpty(x.ItemCode) && x.ItemCode.ToLower() == itemCode.ToLower());
    }

    /// <summary>
    /// 是否所有存在的熟料的板子都是某个料号
    /// </summary>
    /// <param name="itemCode">料号</param>
    /// <returns></returns>
    public bool IsAllExistingDrilledPanelWithItemCode(string itemCode)
    {
        return this.All(x => x.ItemCode.ToLower() == itemCode.ToLower() && ProductStatusConstants.Finished_DRILL.Contains(x.ProductStatus));
    }

    /// <summary>
    /// 获取设备上某个生料的未钻的板子
    /// </summary>
    /// <param name="itemCode">生料</param>
    /// <returns></returns>
    public List<Panel> GetUndrilledPanels(string itemCode)
    {
        return this.UndrilledPanels.Where(x => x.ItemCode.ToLower() == itemCode.ToLower()).ToList();
    }

    /// <summary>
    /// 获取设备上某个生料的未钻的板子的数量
    /// </summary>
    /// <param name="itemCode">生料</param>
    /// <returns></returns>
    public int CountUndrilledPanels(string itemCode)
    {
        return GetUndrilledPanels(itemCode).Count();
    }

    /// <summary>
    /// 统计空位
    /// </summary>
    /// <returns></returns>
    public int CountEmptySiloBoxPanels => EmptySiloBoxPanels.Count();

    /// <summary>
    /// 统计钻好的板子
    /// </summary>
    /// <returns></returns>
    public int CountDrilledPanels() => DrilledPanels.Count();

    /// <summary>
    /// 获取某个熟料料号的钻好的板子的数量
    /// </summary>
    /// <param name="drilledItemCode">熟料料号</param>
    /// <returns></returns>
    public int CountDrilledPanels(string drilledItemCode)
    {
        return GetDrilledPanels(drilledItemCode).Count();
    }

    /// <summary>
    /// 获取某个熟料料号的钻好的板子的数量
    /// </summary>
    /// <param name="drilledItemCodes">熟料料号</param>
    /// <returns></returns>
    public int CountDrilledPanels(IReadOnlyList<string> drilledItemCodes)
    {
        return GetDrilledPanels(drilledItemCodes).Count();
    }

    /// <summary>
    /// 设备是否有空料仓：有料仓，但是是空的
    /// </summary>
    /// <returns></returns>
    public bool IsEmptySiloBox => this.Any() && this.All(x => ProductStatusConstants.EmptySiloBox.Contains(x.ProductStatus));

    /// <summary>
    /// 设备是否有料仓
    /// </summary>
    /// <returns></returns>
    public bool IsEmptyPayload => this.Any() && this.All(x => ProductStatusConstants.EmptyPayload.Contains(x.ProductStatus));

    /// <summary>
    /// 除了指定料号的板子，其他板子为空
    /// </summary>
    /// <param name="itemCode">指定料号的板子</param>
    /// <returns></returns>
    public bool IsEmptySiloBoxExceptItemCode(string itemCode)
    {
        if (string.IsNullOrEmpty(itemCode)) return false;
        return !this.Any(x => x.ItemCode.ToLower() != itemCode.ToLower())
            || this.Where(x => x.ItemCode.ToLower() != itemCode.ToLower()).All(x => ProductStatusConstants.EmptySiloBox.Contains(x.ProductStatus));
    }

    /// <summary>
    /// 是否包含所需要的生料
    /// </summary>
    /// <param name="itemCode">生料料号</param>
    /// <returns></returns>
    public bool ContainsUndrilledItemCode(string itemCode)
    {
        return this.UndrilledPanels.Any(x => x.ItemCode.ToLower() == itemCode.ToLower());
    }

    /// <summary>
    /// 空层数量
    /// </summary>
    public int EmptyLayerCount => this.Count(x => x.EmptySiloBox() && x.Placeable);

    /// <summary>
    /// 统计损坏的料仓层数量
    /// </summary>
    public int BrokenLayerCount => this.Count(x => !x.Placeable);

    /// <summary>
    /// 是否包含所给物料状态
    /// </summary>
    /// <param name="productStatuses"></param>
    /// <returns></returns>
    public bool ContainProductStatuses(IReadOnlyList<ProductStatus> productStatuses)
    {
        return this.Any(x => productStatuses.Contains(x.ProductStatus));
    }

    public bool ContainProductStatus(ProductStatus productStatus)
    {
        return this.Any(x => x.ProductStatus == productStatus);
    }

    /// <summary>
    /// 是否包含其他的熟料料号
    /// </summary>
    /// <param name="itemCode">熟料料号</param>
    /// <returns></returns>
    public bool ContainsOtherDrilledItemCodes(string itemCode)
    {
        return this.DrilledPanels.Any(x => x.ItemCode.ToLower() != itemCode.ToLower());
    }

    /// <summary>
    /// 除了指定料号的板子，其他板子为空
    /// </summary>
    /// <param name="itemCodes">指定料号的板子</param>
    /// <returns></returns>
    public bool IsEmptySiloBoxExceptItemCodes(IReadOnlyList<string> itemCodes)
    {
        if (!this.Any()) return false;
        return !this.Any(x => !itemCodes.Contains(x.ItemCode))
            || this.Where(x => !itemCodes.Contains(x.ItemCode))
            .All(x => ProductStatusConstants.EmptySiloBox.Contains(x.ProductStatus));
    }

    public void SetLocationCode(int position, string locationCode)
    {
        foreach (var item in this.Where(x => x.Position == position))
        {
            item.LocationCode = locationCode;
        }
    }

    public void SetBarcode(int position, string barcode)
    {
        foreach (var item in this.Where(x => x.Position == position))
        {
            item.Barcode = barcode;
        }
    }

    public void SetSiloCode(int position, string siloCode)
    {
        foreach (var item in this.Where(x => x.Position == position))
        {
            item.SiloCode = siloCode;
        }
    }

    public void SetLocationCode(string locationCode)
    {
        foreach (var item in this)
        {
            item.LocationCode = locationCode;
        }
    }

    public void SetSiloCode(string siloCode)
    {
        foreach (var item in this)
        {
            item.SiloCode = siloCode;
        }
    }

    /// <summary>
    /// panel's locationCode will be changed with not-empty <paramref name="locationCode" />, otherwise will keep original value
    /// panel's siloCode will be changed with not-empty <paramref name="siloCode" />, otherwise will keep original value
    /// </summary>
    /// <param name="locationCode"></param>
    /// <param name="siloCode"></param>
    public void SetEmpty(string locationCode = "", string siloCode = "")
    {
        foreach (var item in this)
        {
            item.SetEmpty(locationCode, siloCode);
        }
    }

    public void SetEmpty(int position, string locationCode = "", string siloCode = "")
    {
        foreach (var item in this.Where(x => x.Position == position))
        {
            item.SetEmpty(locationCode, siloCode);
        }
    }

    /// <summary>
    /// panel's locationCode will be changed with not-empty <paramref name="locationCode" />, otherwise will keep original value
    /// </summary>
    /// <param name="locationCode"></param>
    public void SetNoPayload(string locationCode = "")
    {
        foreach (var item in this)
        {
            item.SetNoPayload(locationCode);
        }
    }

    public void SetNoPayload(int position, string locationCode = "")
    {
        foreach (var item in this.Where(x => x.Position == position))
        {
            item.SetNoPayload(locationCode);
        }
    }

    public static PanelList FromList(List<Panel> list)
    {
        var panelList = new PanelList();
        panelList.AddRange(list);
        return panelList;
    }

    public PanelList Clone()
    {
        var panelList = new PanelList();
        foreach (var item in this)
        {
            panelList.Add(item.Clone());
        }

        return panelList;
    }

    /// <summary>
    /// 获取某层的板料
    /// </summary>
    /// <param name="layer">某一层</param>
    /// <returns></returns>
    public Panel[] GetLayerPanels(int layer)
    {
        return this.Where(x => x.Layer == layer).ToArray();
    }

    /// <summary>
    /// 获取某个位置上的板料
    /// </summary>
    /// <param name="position">某个位置</param>
    /// <returns></returns>
    public Panel[] GetPositionPanels(int position)
    {
        return this.Where(x => x.Position == position).ToArray();
    }

    /// <summary>
    /// 获取某个位置上的板料
    /// </summary>
    /// <param name="layer">某一层</param>
    /// <param name="position">某个位置</param>
    /// <returns></returns>
    public Panel[] GetPanels(int layer, int position)
    {
        return this.Where(x => x.Layer == layer && x.Position == position).ToArray();
    }

    /// <summary>
    /// 获取某个位置上的层数数组
    /// </summary>
    /// <param name="position">位置</param>
    /// <returns></returns>
    public int[] GetLayers(int position)
    {
        return this.Where(x => x.Position == position).Select(x => x.Layer).ToArray();
    }

    /// <summary>
    /// 获取位置索引
    /// </summary>
    /// <returns></returns>
    public int[] GetPositions()
    {
        return this.Select(x => x.Position).Distinct().OrderBy(x => x).ToArray();
    }

    public int PanelCount(Predicate<Panel> predicate)
    {
        return this.Count(x => predicate(x));
    }

    public int PanelCount(int position, Predicate<Panel> predicate)
    {
        return this.Count(x => x.Position == position && predicate(x));
    }

    public int PanelCount(int position, ProductStatus productStatus)
    {
        return this.Count(x => x.Position == position && x.ProductStatus == productStatus);
    }

    public int PanelCount(ProductStatus productStatus)
    {
        return this.Count(x => x.ProductStatus == productStatus);
    }

    public int PanelCount(IReadOnlyCollection<ProductStatus> productStatuses)
    {
        return this.Count(x => productStatuses.Contains(x.ProductStatus));
    }

    /// <summary>
    /// Note: position not suitable for drill, because one drill has multiple position;
    /// </summary>
    /// <param name="position"></param>
    /// <param name="productStatuses"></param>
    /// <returns></returns>
    public int PanelCount(int position, IReadOnlyCollection<ProductStatus> productStatuses)
    {
        return this.Count(x => x.Position == position && productStatuses.Contains(x.ProductStatus));
    }

    public int UndrilledPanelCount()
    {
        return UndrilledPanels.Count();
    }

    public int UndrilledPanelCount(int position)
    {
        return UndrilledPanels.Count(x => x.Position == position);
    }

    public int DrilledPanelCount()
    {
        return DrilledPanels.Count();
    }

    public int DrilledPanelCount(int position)
    {
        return DrilledPanels.Count(x => x.Position == position);
    }

    /// <summary>
    /// 获取某个位置上的板料集合
    /// </summary>
    /// <param name="position">位置</param>
    /// <returns></returns>
    public PanelList GetPanelList(int position)
    {
        return FromList(this.Where(x => x.Position == position).ToList());
    }

    /// <summary>
    /// 更新板料状态
    /// </summary>
    /// <param name="predicate">条件</param>
    /// <param name="productStatus">板料状态</param>
    /// <returns></returns>
    public PanelList UpdateProductStatus(Predicate<Panel> predicate, ProductStatus productStatus)
    {
        foreach (var item in this.Where(x => predicate(x)))
        {
            item.ProductStatus = productStatus;
        }

        return this;
    }

    /// <summary>
    /// 更新板料物料代码
    /// </summary>
    /// <param name="predicate">条件</param>
    /// <param name="itemCode">物料代码</param>
    /// <returns></returns>
    public PanelList UpdateItemCode(Predicate<Panel> predicate, string itemCode)
    {
        foreach (var item in this.Where(x => predicate(x)))
        {
            item.ItemCode = itemCode;
        }

        return this;
    }

    /// <summary>
    /// 更新板料料仓编码
    /// </summary>
    /// <param name="predicate">条件</param>
    /// <param name="siloCode">料仓编码</param>
    /// <returns></returns>
    public PanelList UpdateSiloCode(Predicate<Panel> predicate, string siloCode)
    {
        foreach (var item in this.Where(x => predicate(x)))
        {
            item.SiloCode = siloCode;
        }

        return this;
    }

    /// <summary>
    /// 更新板料库位号
    /// </summary>
    /// <param name="predicate">条件</param>
    /// <param name="locationCode">库位号</param>
    /// <returns></returns>
    public PanelList UpdateLocationCode(Predicate<Panel> predicate, string locationCode)
    {
        foreach (var item in this.Where(x => predicate(x)))
        {
            item.LocationCode = locationCode;
        }

        return this;
    }

    /// <summary>
    /// 更新板料二维码
    /// </summary>
    /// <param name="predicate">条件</param>
    /// <param name="barcode">二维码</param>
    /// <returns></returns>
    public PanelList UpdateBarcode(Predicate<Panel> predicate, string barcode)
    {
        foreach (var item in this.Where(x => predicate(x)))
        {
            item.Barcode = barcode;
        }

        return this;
    }

    #region UpdatePanelInfo

    /// <summary>
    /// 更新板料状态
    /// </summary>
    /// <param name="productStatus">板料状态</param>
    public void UpdatePanelInfo(ProductStatus productStatus)
    {
        foreach (var item in this)
        {
            item.ProductStatus = productStatus;
        }
    }

    /// <summary>
    /// 更新板料状态
    /// </summary>
    /// <param name="productStatus">板料状态</param>
    /// <param name="layers">哪几层</param>
    public void UpdatePanelInfo(ProductStatus productStatus, int[] layers)
    {
        foreach (var item in this)
        {
            if (layers.Contains(item.Layer))
            {
                item.ProductStatus = productStatus;
            }
        }
    }

    /// <summary>
    /// 更新板料状态
    /// </summary>
    /// <param name="productStatus">板料状态</param>
    /// <param name="itemCode">料号</param>
    public void UpdatePanelInfo(ProductStatus productStatus, string itemCode)
    {
        foreach (var item in this)
        {
            item.ProductStatus = productStatus;
            item.ItemCode = itemCode;
        }
    }

    /// <summary>
    /// 更新板料状态
    /// </summary>
    /// <param name="productStatus">板料状态</param>
    /// <param name="itemCode">料号</param>
    /// <param name="siloCode">料仓号</param>
    public void UpdatePanelInfo(ProductStatus productStatus, string itemCode, string siloCode)
    {
        foreach (var item in this)
        {
            item.ProductStatus = productStatus;
            item.ItemCode = itemCode;
            item.SiloCode = siloCode;
        }
    }

    /// <summary>
    /// 更新板料状态
    /// </summary>
    /// <param name="productStatus">板料状态</param>
    /// <param name="itemCode">料号</param>
    /// <param name="locationCode">库位号</param>
    /// <param name="siloCode">料仓号</param>
    public void UpdatePanelInfo(ProductStatus productStatus, string itemCode, string locationCode, string siloCode)
    {
        foreach (var item in this)
        {
            item.ProductStatus = productStatus;
            item.ItemCode = itemCode;
            item.SiloCode = siloCode;
            item.LocationCode = locationCode;
        }
    }

    /// <summary>
    /// 更新某个位置上的板料状态
    /// </summary>
    /// <param name="position">位置</param>
    /// <param name="productStatus">板料状态</param>
    public void UpdatePanelInfo(int position, ProductStatus productStatus)
    {
        var panelsOfPosition = this.Where(x => x.Position == position).ToList();
        foreach (var item in panelsOfPosition)
        {
            item.ProductStatus = productStatus;
        }
    }

    /// <summary>
    /// 更新某个位置上，某几层的板料状态
    /// </summary>
    /// <param name="position">位置</param>
    /// <param name="layers">哪几层</param>
    /// <param name="productStatus">板料状态</param>
    public void UpdatePanelInfo(int position, int[] layers, ProductStatus productStatus)
    {
        var panelsOfPosition = this.Where(x => x.Position == position).ToList();
        foreach (var item in panelsOfPosition)
        {
            if (layers.Contains(item.Layer))
            {
                item.ProductStatus = productStatus;
            }
        }
    }

    /// <summary>
    /// 更新某个位置上，某几层的板料状态，料号
    /// </summary>
    /// <param name="position">位置</param>
    /// <param name="layers">哪几层</param>
    /// <param name="productStatus">板料状态</param>
    /// <param name="itemCode">料号</param>
    public void UpdatePanelInfo(int position, int[] layers, ProductStatus productStatus, string itemCode)
    {
        var panelsOfPosition = this.Where(x => x.Position == position).ToList();
        foreach (var item in panelsOfPosition)
        {
            if (layers.Contains(item.Layer))
            {
                item.ProductStatus = productStatus;
                item.ItemCode = itemCode;
            }
        }
    }

    /// <summary>
    /// 更新某个位置上，某几层的板料状态，料号
    /// </summary>
    /// <param name="position">位置</param>
    /// <param name="startLayer">从第几层开始</param>
    /// <param name="productStatus">板料状态</param>
    /// <param name="itemCode">料号</param>
    public void UpdatePanelInfo(int position, int startLayer, ProductStatus productStatus, string itemCode)
    {
        var panelsOfPosition = this.Where(x => x.Position == position).ToList();
        foreach (var item in panelsOfPosition)
        {
            if (item.Layer >= startLayer)
            {
                item.ProductStatus = productStatus;
                item.ItemCode = itemCode;
            }
        }
    }

    /// <summary>
    /// 更新某个位置上，某几层的板料状态
    /// </summary>
    /// <param name="position">位置</param>
    /// <param name="startLayer">从第几层开始</param>
    /// <param name="productStatus">板料状态</param>
    public void UpdatePanelInfo(int position, int startLayer, ProductStatus productStatus)
    {
        var panelsOfPosition = this.Where(x => x.Position == position).ToList();
        foreach (var item in panelsOfPosition)
        {
            if (item.Layer >= startLayer)
            {
                item.ProductStatus = productStatus;
            }
        }
    }

    /// <summary>
    /// 更新某个位置上，某几层是否可用
    /// </summary>
    /// <param name="position">位置</param>
    /// <param name="layers">哪几层</param>
    /// <param name="placeable">是否可用</param>
    public void UpdatePanelInfo(int position, int[] layers, bool placeable)
    {
        var panelsOfPosition = this.Where(x => x.Position == position).ToList();
        foreach (var item in panelsOfPosition)
        {
            if (layers.Contains(item.Layer))
            {
                item.Placeable = placeable;
            }
        }
    }

    /// <summary>
    /// 更新某个位置连续几层板料信息
    /// </summary>
    /// <param name="position">位置</param>
    /// <param name="layer">共几层</param>
    /// <param name="itemCode">物料号</param>
    /// <param name="externalLotNo">外部物料号</param>
    /// <param name="pcs">叠数</param>
    /// <param name="productStatus">板料状态</param>
    public void UpdatePanelInfo(int position, int layer, string itemCode, string externalLotNo, int pcs, ProductStatus productStatus)
    {
        var panelsOfPosition = this.Where(x => x.Position == position).ToList();
        foreach (var item in panelsOfPosition)
        {
            if (item.Layer < layer)
            {
                item.ItemCode = itemCode;
                item.InternalLotNo = itemCode;
                item.ExternalLotNo = externalLotNo;
                item.ProductStatus = productStatus;
                item.Pcs = pcs;
            }
        }
    }

    /// <summary>
    /// 更新板料信息通用方法
    /// </summary>
    /// <param name="predicate">条件</param>
    /// <param name="action">动作</param>
    public void UpdatePanelInfo(Predicate<Panel> predicate, Action<Panel> action)
    {
        this.ForEach(x =>
        {
            if (predicate(x))
            {
                action.Invoke(x);
            }
        });
    }

    #endregion UpdatePanelInfo

    /// <summary>
    /// 包含首件板
    /// </summary>
    /// <returns></returns>
    public bool ContainsFirst() => FirstPanels.Any();

    /// <summary>
    /// 包含熟料板，不包括首件
    /// </summary>
    /// <returns></returns>
    public bool ContainsDrilled() => DrilledPanels.Any();

    /// <summary>
    /// 包含生料板
    /// </summary>
    /// <returns></returns>
    public bool ContainsUndrilled() => UndrilledPanels.Any();

    public override string ToString()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
        });
    }
}
