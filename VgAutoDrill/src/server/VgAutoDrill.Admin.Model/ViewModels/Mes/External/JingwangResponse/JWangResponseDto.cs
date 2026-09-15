namespace VgAutoDrill.Admin.Model.ViewModels.Mes.External.JingwangResponse
{
    public class JwangResponseDto
    {
        /// <summary>
        /// 0-成功
        /// 默认返回500-异常
        /// </summary>
        public string ResultCode { get; set; } = "500";

        /// <summary>
        /// 返回信息
        /// </summary>
        public string ResultMsg { set; get; } = "异常";
    }
}
