using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Admin.Application.Helper
{
    /// <summary>
    /// ProductStatus 扩展方法
    /// </summary>
    internal static class ProductStatusExtensions
    {
        /// <summary>
        /// 获取简化的产品状态名称
        /// </summary>
        /// <param name="productStatus">产品状态</param>
        /// <returns>简化的状态名称</returns>
        public static string SimplifyProductStatusName(this ProductStatus productStatus)
        {
            if (ProductStatusConstants.Finished_PIN.Contains(productStatus))
            {
                return "生料";
            }

            if (ProductStatusConstants.Finished_DRILL.Contains(productStatus))
            {
                return "熟料";
            }

            if (ProductStatusConstants.EmptySiloBox.Contains(productStatus))
            {
                return "空";
            }

            if (ProductStatusConstants.EmptyPayload.Contains(productStatus))
            {
                return "无料仓";
            }

            if (productStatus == ProductStatus.Drilling)
            {
                return "正在钻孔";
            }

            return productStatus.ToString();
        }
    }
}

