using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Admin.Application.Helper
{
    internal static class ProductStatusExtension
    {
        internal static string GetProductStatusDesc(this ProductStatus ProductStatus)
        {
            string result = "【未知】";
            switch (ProductStatus)
            {
                case ProductStatus.EmptyPayload:
                    result = "【空位】";
                    break;

                case ProductStatus.EmptySiloBox:
                    result = "【空仓】";
                    break;

                case ProductStatus.Finished_PIN:
                    result = "【生料】完成pin包装";
                    break;

                case ProductStatus.Finished_PRE_BUFFER:
                    result = "【生料】已放置中转位料架";
                    break;

                case ProductStatus.PRE_DRILL_TRANSFER_AGV_OUTPUT_1:
                    result = "【生料】agv已装载板料";
                    break;

                case ProductStatus.WaitingForDrill:
                    result = "【生料】等待钻孔";
                    break;

                case ProductStatus.Drilling:
                    result = "【生料】正在钻孔";
                    break;

                case ProductStatus.Finished_DRILL:
                    result = "【熟料】完成钻孔";
                    break;

                case ProductStatus.PRE_POST_BUFFER_TRANSFER_AGV_OUTPUT_1:
                    result = "【熟料】agv已装载板料";
                    break;

                case ProductStatus.Finished_POST_BUFFER:
                    result = "【熟料】已放置中转位料架";
                    break;

                case ProductStatus.Finished_UNPIN:
                    result = "【熟料】进入拆pin等待出货";
                    break;

                default:
                    result = "【未知】";
                    break;
            }
            return result;
        }
    }
}
