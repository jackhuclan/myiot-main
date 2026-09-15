using System.Text.Json;

namespace VgAutoDrill.Fundation.Iot.Models;

/// <summary>
/// 刀盘
/// 1个刀盘可以放置4个刀盒
/// </summary>
public class CutterTray : IComparable<CutterTray>
{
    /// <summary>
    /// 料仓二维码
    /// </summary>
    public string SiloCode { get; set; } = string.Empty;
    /// <summary>
    /// 刀盘二维码
    /// </summary>
    public string TrayCode { get; set; } = string.Empty;
    public string ItemCode { get; set; } = string.Empty;
    /// <summary>
    /// 刀盘在刀盘上的顺序位置，从1~6
    /// </summary>
    public int IndexOnLayer { get; set; } = 1;
    /// <summary>
    /// 最多可以放几盒
    /// </summary>
    public int BoxLimit { get; set; } = 4;
    /// <summary>
    /// 最多可以放几盘
    /// </summary>
    public int TrayLimit { get; set; } = 6;
    public CutterTrayStatus Status { get; set; }
    /// <summary>
    /// 以左下角为原点，水平坐标，表示第几行
    /// </summary>
    public int X { get; set; } = 0;
    /// <summary>
    /// 以左下角为原点，深度坐标，表示第几列
    /// </summary>
    public int Y { get; set; } = 0;
    /// <summary>
    /// 以左下角为原点，高度坐标，表示第几层
    /// </summary>
    public int Z { get; set; } = 0;

    public void SetEmpty()
    {
        ItemCode = string.Empty;
        TrayCode = string.Empty;
        Status = CutterTrayStatus.NoTray;
    }
    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
    /// <summary>
    /// 初始化一个刀具料仓的数据，如初始化一个有6层，3行，2列的料仓
    /// InitializeCutterSilo(6,3,2)
    /// </summary>
    /// <param name="layer">有几层</param>
    /// <param name="rows">有几行</param>
    /// <param name="columns">有几列</param>
    /// <returns></returns>
    public static List<CutterTray> InitializeCutterSilo(int layer, int rows, int columns)
    {
        if (layer <= 0) throw new ArgumentOutOfRangeException(nameof(layer));
        if (rows <= 0) throw new ArgumentOutOfRangeException(nameof(rows));
        if (columns <= 0) throw new ArgumentOutOfRangeException(nameof(columns));
        var trayLimit = rows * columns;

        var list = new List<CutterTray>();
        for (int i = 0; i < layer; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                for (int k = 0; k < columns; k++)
                {
                    var tray = new CutterTray();
                    tray.Y = j;
                    tray.Z = i;
                    tray.X = k;
                    tray.TrayLimit = trayLimit;
                    list.Add(tray);
                }
            }
        }

        list.Sort();

        for (int i = 0; i < list.Count; i++)
        {
            list[i].IndexOnLayer = i % trayLimit + 1;
        }

        return list;
    }

    public int CompareTo(CutterTray? other)
    {
        if (this.Z == other.Z)
        {
            if (this.X == other.X)
                return this.X % 2 == 0 ? this.Y - other.Y : other.Y - this.Y;
            else
                return other.X - this.X;
        }
        else
            return this.Z - other.Z;
    }
}
