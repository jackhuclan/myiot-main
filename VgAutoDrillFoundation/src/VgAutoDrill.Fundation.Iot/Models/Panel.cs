using System.Diagnostics;
using System.Text.Json;

namespace VgAutoDrill.Fundation.Iot.Models;

/// <summary>
/// 板料信息
/// </summary>
[DebuggerDisplay("SiloCode={SiloCode},ItemCode={ItemCode},Position={Position},Layer={Layer},ProductStatus={ProductStatus}")]
public class Panel
{
    /// <summary>
    /// 板料码
    /// </summary>
    public string PanelCode { get; set; } = string.Empty;

    /// <summary>
    /// 板料长度
    /// </summary>
    public float PanelLength { get; set; }

    /// <summary>
    /// 板料二维码
    /// </summary>
    public string Barcode { get; set; } = string.Empty;

    /// <summary>
    /// 单个板料厚度
    /// </summary>
    public float PanelThickness { get; set; }

    /// <summary>
    /// 叠数
    /// </summary>
    public int Pcs { get; set; }

    /// <summary>
    /// 板料所在库位
    /// </summary>
    public string LocationCode { get; set; } = string.Empty;

    /// <summary>
    /// 板料宽度
    /// </summary>
    public float PanelWidth { get; set; }

    /// <summary>
    /// 板料销钉距中心偏移量
    /// </summary>
    public float PinOffset { get; set; }

    /// <summary>
    /// 料仓二维码
    /// </summary>
    public string SiloCode { get; set; } = string.Empty;

    /// <summary>
    /// 料号
    /// </summary>
    public string ItemCode { get; set; } = string.Empty;

    /// <summary>
    /// Lot二维码
    /// </summary>
    public string LotId { get; set; } = string.Empty;

    public string InternalLotNo { get; set; } = string.Empty;
    public string ExternalLotNo { get; set; } = string.Empty;

    /// <summary>
    /// 工单生产任务号
    /// </summary>
    public string TaskCode { get; set; } = string.Empty;

    /// <summary>
    /// 批次号
    /// </summary>
    public string BatchCode { get; set; } = string.Empty;

    /// <summary>
    /// 放在第几层
    /// </summary>
    public int Layer { get; set; } = 0;

    /// <summary>
    /// 放在哪个位置
    /// 从1开始
    /// 六轴设备上，分别对应1~6
    /// </summary>
    public int Position { get; set; } = 1;

    /// <summary>
    /// 成品状态
    /// </summary>
    public ProductStatus ProductStatus { get; set; } = ProductStatus.EmptyPayload;

    /// <summary>
    /// 是否首件
    /// </summary>
    public bool IsFirst { get; set; } = false;

    /// <summary>
    /// 可放置板料，默认true
    /// </summary>
    public bool Placeable { get; set; } = true;

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }

    /// <summary>
    /// panel's locationCode will be changed with not-empty <paramref name="locationCode" />, otherwise will keep original value
    /// panel's siloCode will be changed with not-empty <paramref name="siloCode" />, otherwise will keep original value
    /// position,layer, placeable keep old value
    /// </summary>
    /// <param name="locationCode"></param>
    /// <param name="siloCode"></param>
    public void SetEmpty(string locationCode = "", string siloCode = "")
    {
        PanelWidth = 0;
        PanelLength = 0;
        PinOffset = 0;
        PanelThickness = 0;
        Pcs = 0;

        LotId = string.Empty;
        InternalLotNo = string.Empty;
        ExternalLotNo = string.Empty;
        BatchCode = string.Empty;
        ItemCode = string.Empty;
        Barcode = string.Empty;
        TaskCode = string.Empty;
        PanelCode = string.Empty;
        SiloCode = string.Empty;
        IsFirst = false;
        SiloCode = !string.IsNullOrWhiteSpace(siloCode) ? siloCode : SiloCode;
        LocationCode = !string.IsNullOrWhiteSpace(locationCode) ? locationCode : LocationCode;
        ProductStatus = ProductStatus.EmptySiloBox;
    }

    /// <summary>
    /// panel's locationCode will be changed with not-empty <paramref name="locationCode" />, otherwise will keep original value
    /// position,layer keep old value
    /// </summary>
    /// <param name="locationCode"></param>
    public void SetNoPayload(string locationCode = "")
    {
        PanelWidth = 0;
        PanelLength = 0;
        PinOffset = 0;
        PanelThickness = 0;
        Pcs = 0;

        LotId = string.Empty;
        InternalLotNo = string.Empty;
        ExternalLotNo = string.Empty;
        BatchCode = string.Empty;
        ItemCode = string.Empty;
        Barcode = string.Empty;
        TaskCode = string.Empty;
        PanelCode = string.Empty;
        SiloCode = string.Empty;
        IsFirst = false;
        Placeable = true;
        LocationCode = !string.IsNullOrWhiteSpace(locationCode) ? locationCode : LocationCode;
        ProductStatus = ProductStatus.EmptyPayload;
    }

    public Panel Clone()
    {
        return new Panel
        {
            PanelCode = this.PanelCode,
            PanelLength = this.PanelLength,
            Barcode = this.Barcode,
            PanelThickness = this.PanelThickness,
            Pcs = this.Pcs,
            LocationCode = this.LocationCode,
            PanelWidth = this.PanelWidth,
            PinOffset = this.PinOffset,
            SiloCode = this.SiloCode,
            ItemCode = this.ItemCode,
            LotId = this.LotId,
            InternalLotNo = this.InternalLotNo,
            ExternalLotNo = this.ExternalLotNo,
            BatchCode = this.BatchCode,
            TaskCode = this.TaskCode,
            Layer = this.Layer,
            Position = this.Position,
            ProductStatus = this.ProductStatus,
            IsFirst = this.IsFirst,
            Placeable = this.Placeable,
        };
    }

    /// <summary>
    /// 无料仓情况下初始化板料
    /// </summary>
    public static class NoSilo
    {
        /// <summary>
        /// 初始化没有料仓的板料情况
        /// 如position=1,layer=0,layerLimit=18
        /// </summary>
        /// <param name="position">从1开始的轴位置，或者是料仓的序号</param>
        /// <param name="layer">从0开始的层号</param>
        /// <param name="layerLimit">初始化几层</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static PanelList PanelForSingleSpindle(int position, int layer, int layerLimit)
        {
            if (position < 1) throw new ArgumentOutOfRangeException(nameof(position));
            if (layer < 0) throw new ArgumentOutOfRangeException(nameof(layer));
            if (layerLimit < 1) throw new ArgumentOutOfRangeException(nameof(layerLimit));

            var panels = new PanelList();
            for (int i = layer; i < layer + layerLimit; i++)
            {
                panels.Add(new Panel()
                {
                    Position = position,
                    Layer = i,
                    ProductStatus = ProductStatus.EmptyPayload
                });
            }

            return panels;
        }

        /// <summary>
        /// 初始化没有料仓的板料情况，返回的list中position从1开始，layer从0开始
        /// </summary>
        /// <param name="panelCount">一共多少板</param>
        /// <param name="layerLimit">每个轴位置或者是料仓最多放置多少层板</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static PanelList PanelForSpindleFirst(int panelCount, int layerLimit)
        {
            if (panelCount <= 0) throw new ArgumentOutOfRangeException(nameof(panelCount));
            if (layerLimit <= 0) throw new ArgumentOutOfRangeException(nameof(layerLimit));

            var panels = new PanelList();
            var spindle = (panelCount - 1) / layerLimit + 1;
            for (int i = 1; i <= spindle; i++)
            {
                for (int j = 0; j < layerLimit && (i - 1) * layerLimit + j < panelCount; j++)
                {
                    panels.Add(new Panel()
                    {
                        Position = i,
                        Layer = j,
                        ProductStatus = ProductStatus.EmptyPayload
                    });
                }
            }

            return panels;
        }

        /// <summary>
        /// 无料仓的情况下，用指定物料状态的板料，初始化设备上的板料料仓
        /// </summary>
        /// <param name="spindleLimit">设备上的轴数</param>
        /// <param name="layerLimit">层数限制</param>
        /// <param name="productStatus">板料的物料状态</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static PanelList PanelForLayerFirst(int spindleLimit, int layerLimit, ProductStatus productStatus)
        {
            if (layerLimit <= 0) throw new ArgumentOutOfRangeException(nameof(layerLimit));
            if (spindleLimit <= 0) throw new ArgumentOutOfRangeException(nameof(spindleLimit));

            var panels = new PanelList();

            for (var layer = 0; layer < layerLimit; layer++)
            {
                for (int i = 1; i <= spindleLimit; i++)
                {
                    panels.Add(new Panel()
                    {
                        Position = i,
                        Layer = layer,
                        ProductStatus = productStatus
                    });
                }
            }
            return panels;
        }
    }

    /// <summary>
    /// 有料仓情况下初始化板料
    /// </summary>
    public static class HasSilo
    {
        /// <summary>
        /// 初始化单个位置上的板料料仓，但是里面没有板料情况，
        /// 如position=1,layer=0,layerLimit=18
        /// </summary>
        /// <param name="siloCode">料仓的设备code,必须为实际的料仓</param>
        /// <param name="position">从1开始的轴位置，或者是料仓的序号</param>
        /// <param name="layer">从0开始的层号</param>
        /// <param name="layerLimit">初始化几层</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static PanelList NoPanelForSingleSpindle(string siloCode, int position, int layer, int layerLimit)
        {
            if (position < 1) throw new ArgumentOutOfRangeException(nameof(position));
            if (layer < 0) throw new ArgumentOutOfRangeException(nameof(layer));
            if (layerLimit < 1) throw new ArgumentOutOfRangeException(nameof(layerLimit));

            var panels = new PanelList();
            for (var i = layer; i < layer + layerLimit; i++)
            {
                panels.Add(new Panel()
                {
                    SiloCode = siloCode,
                    Position = position,
                    Layer = i,
                    ProductStatus = ProductStatus.EmptySiloBox
                });
            }

            return panels;
        }

        /// <summary>
        /// 初始化有料仓，但是里面没有板料情况，返回的list中position从1开始，layer从0开始
        /// </summary>
        /// <param name="siloCode">料仓的设备code,必须为实际的料仓</param>
        /// <param name="panelCount">一共多少板</param>
        /// <param name="layerLimit">每个轴位置或者是料仓最多放置多少层板</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static PanelList NoPanelForSpindleFirst(string siloCode, int panelCount, int layerLimit)
        {
            return HasPanelForSpindleFirst(siloCode, panelCount, layerLimit, ProductStatus.EmptySiloBox);
        }

        /// <summary>
        /// 用指定物料状态的板料，初始化有料仓，返回的list中position从1开始，layer从0开始
        /// </summary>
        /// <param name="siloCode">料仓的设备code,必须为实际的料仓</param>
        /// <param name="panelCount">一共多少板</param>
        /// <param name="layerLimit">每个轴位置或者是料仓最多放置多少层板</param>
        /// <param name="productStatus">板料的物料状态</param>
        /// <returns></returns>
        public static PanelList HasPanelForSpindleFirst(string siloCode, int panelCount, int layerLimit, ProductStatus productStatus)
        {
            if (panelCount <= 0) throw new ArgumentOutOfRangeException(nameof(panelCount));
            if (layerLimit <= 0) throw new ArgumentOutOfRangeException(nameof(layerLimit));

            PanelList panels = new PanelList();
            var spindleLimit = (panelCount - 1) / layerLimit + 1;
            for (var i = 1; i <= spindleLimit; i++)
            {
                for (var j = 0; j < layerLimit && (i - 1) * layerLimit + j < panelCount; j++)
                {
                    panels.Add(new Panel()
                    {
                        Position = i,
                        Layer = j,
                        SiloCode = siloCode,
                        ProductStatus = productStatus
                    });
                }
            }

            return panels;
        }

        /// <summary>
        /// 用指定物料状态的板料，初始化有料仓，返回的list中spindleLimit从1开始，layer从0开始
        /// </summary>
        /// <param name="siloCode">料仓的设备code,必须为实际的料仓</param>
        /// <param name="position">位置</param>
        /// <param name="layerLimit">layer从0开始</param>
        /// <param name="productStatus">板料的物料状态</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static PanelList HasPanelForSingleSpindle(string siloCode, int position, int layerLimit, ProductStatus productStatus)
        {
            if (string.IsNullOrWhiteSpace(siloCode)) throw new ArgumentException(nameof(siloCode));
            if (position <= 0) throw new ArgumentOutOfRangeException(nameof(position));
            if (layerLimit <= 0) throw new ArgumentOutOfRangeException(nameof(layerLimit));

            PanelList panels = new PanelList();
            for (var j = 0; j < layerLimit; j++)
            {
                panels.Add(new Panel()
                {
                    Position = position,
                    Layer = j,
                    ProductStatus = productStatus,
                    SiloCode = siloCode
                });
            }

            return panels;
        }

        /// <summary>
        /// 初始化设备上的板料料仓
        /// </summary>
        /// <param name="siloCode">料仓的设备code,必须为实际的料仓</param>
        /// <param name="spindleLimit">设备上的轴数</param>
        /// <param name="layerLimit">层数限制</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static PanelList NoPanelForLayerFirst(string siloCode, int spindleLimit, int layerLimit)
        {
            return HasPanelForLayerFirst(siloCode, spindleLimit, layerLimit, ProductStatus.EmptySiloBox);
        }

        /// <summary>
        /// 用指定物料状态的板料，初始化设备上的板料料仓
        /// </summary>
        /// <param name="siloCode">料仓的设备code,必须为实际的料仓</param>
        /// <param name="spindleLimit">设备上的轴数</param>
        /// <param name="layerLimit">层数限制</param>
        /// <param name="productStatus">板料的物料状态</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static PanelList HasPanelForLayerFirst(string siloCode, int spindleLimit, int layerLimit, ProductStatus productStatus)
        {
            if (string.IsNullOrWhiteSpace(siloCode)) throw new ArgumentException(nameof(siloCode));
            if (layerLimit <= 0) throw new ArgumentOutOfRangeException(nameof(layerLimit));
            if (spindleLimit <= 0) throw new ArgumentOutOfRangeException(nameof(spindleLimit));

            var panels = new PanelList();

            for (var layer = 0; layer < layerLimit; layer++)
            {
                for (int i = 1; i <= spindleLimit; i++)
                {
                    panels.Add(new Panel()
                    {
                        Position = i,
                        Layer = layer,
                        SiloCode = siloCode,
                        ProductStatus = productStatus
                    });
                }
            }
            return panels;
        }

        /// <summary>
        /// 初始化单层板料为EmptySiloBox
        /// </summary>
        /// <param name="siloCode">料仓的设备code,必须为实际的料仓</param>
        /// <param name="spindleLimit">设备上的轴数</param>
        /// <param name="layer">指定层数</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static PanelList NoPanelForSingleLayer(string siloCode, int spindleLimit, int layer)
        {
            return HasPanelForSingleLayer(siloCode, spindleLimit, layer, ProductStatus.EmptySiloBox);
        }

        /// <summary>
        /// 初始化单层板料为指定的productStatus
        /// </summary>
        /// <param name="siloCode">料仓的设备code,必须为实际的料仓</param>
        /// <param name="spindleLimit">设备上的轴数</param>
        /// <param name="layer">指定层数</param>
        /// <param name="productStatus">板料的物料状态</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static PanelList HasPanelForSingleLayer(string siloCode, int spindleLimit, int layer, ProductStatus productStatus)
        {
            if (layer < 0) throw new ArgumentOutOfRangeException(nameof(layer));
            if (spindleLimit <= 0) throw new ArgumentOutOfRangeException(nameof(spindleLimit));

            var panels = new PanelList();

            for (int i = 1; i <= spindleLimit; i++)
            {
                panels.Add(new Panel()
                {
                    Position = i,
                    Layer = layer,
                    SiloCode = siloCode,
                    ProductStatus = productStatus
                });
            }
            return panels;
        }
    }
}
