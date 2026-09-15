namespace VgAutoDrill.Fundation.Iot.Models;

public static class ProductStatusConstants
{
    /// <summary>
    /// 生料状态
    /// </summary>
    public static IReadOnlyList<ProductStatus> Finished_PIN = new List<ProductStatus>() {
        ProductStatus.Finished_PIN,
        ProductStatus.PRE_PRE_BUFFER_TRANSFER_AGV_OUTPUT_1,
        ProductStatus.Finished_PRE_BUFFER,
        ProductStatus.PRE_DRILL_TRANSFER_AGV_OUTPUT_1,
        ProductStatus.WaitingForDrill,
    }.AsReadOnly();

    /// <summary>
    /// 熟料状态
    /// </summary>
    public static IReadOnlyList<ProductStatus> Finished_DRILL = new List<ProductStatus>() {
        ProductStatus.Finished_DRILL,
        ProductStatus.PRE_POST_BUFFER_TRANSFER_AGV_OUTPUT_1,
        ProductStatus.Finished_POST_BUFFER,
        ProductStatus.PRE_UNPIN_TRANSFER_AGV_OUTPUT_1,
    }.AsReadOnly();

    public static IReadOnlyList<ProductStatus> EmptySiloBox = new List<ProductStatus>() {
        ProductStatus.Finished_UNPIN,
        ProductStatus.EmptySiloBox,
    }.AsReadOnly();

    public static IReadOnlyList<ProductStatus> EmptyPayload = new List<ProductStatus>() {
         ProductStatus.EmptyPayload,
    }.AsReadOnly();

    public static ProductStatus SimplifyProductStatus(ProductStatus productStatus)
    {
        if (Finished_PIN.Contains(productStatus)) return ProductStatus.Finished_PIN;
        if (Finished_DRILL.Contains(productStatus)) return ProductStatus.Finished_DRILL;
        if (EmptySiloBox.Contains(productStatus)) return ProductStatus.EmptySiloBox;
        if (EmptyPayload.Contains(productStatus)) return ProductStatus.EmptyPayload;
        return productStatus;
    }

    public static string SimplifyProductStatusName(ProductStatus productStatus)
    {
        if (Finished_PIN.Contains(productStatus)) return "生料";
        if (Finished_DRILL.Contains(productStatus)) return "熟料";
        if (EmptySiloBox.Contains(productStatus)) return "空";
        if (EmptyPayload.Contains(productStatus)) return "无料仓";
        if (productStatus == ProductStatus.Drilling) return "正在钻孔";
        return productStatus.ToString();
    }
}
