using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Fundation.Iot.Extensions;

public static class PanelExtensions
{
    public static bool IsNull(this Panel? panel)
    {
        return panel == null || string.IsNullOrWhiteSpace(panel.PanelCode)
            || panel.ProductStatus == ProductStatus.Noop
            || panel.ProductStatus == ProductStatus.EmptyPayload
            || panel.ProductStatus == ProductStatus.EmptySiloBox;
    }

    public static bool EmptySiloBox(this Panel panel)
    {
        if (panel is null)
        {
            throw new ArgumentNullException(nameof(panel));
        }

        return string.IsNullOrEmpty(panel.ItemCode) && panel.ProductStatus == ProductStatus.EmptySiloBox;
    }

    public static bool EmptyPayload(this Panel panel)
    {
        if (panel is null)
        {
            throw new ArgumentNullException(nameof(panel));
        }

        return string.IsNullOrEmpty(panel.ItemCode) && panel.ProductStatus == ProductStatus.EmptyPayload;
    }
}
