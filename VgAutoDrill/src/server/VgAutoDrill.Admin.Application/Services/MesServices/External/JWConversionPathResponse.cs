namespace VgAutoDrill.Admin.Application.Services.MesServices.External
{
    public class JWConversionPathResponse
    {
        /// <summary>
        /// "stateDescriptions": [
            ///    "Submitted",       //已提交转换请求
            ///    "Pending",         //排队中
            ///    "Processing",      //转换处理中
            ///    "Retrying",        //转换重试中,前几次转换失败。
            ///    "ConvFail",        //转换失败
            ///    "ConvSucc",        //转换成功
            ///    "VerifyNG",        //审核失败
            ///    "VerifyTimeOut",   //审核超时
            ///    "VerifyOK"         //审核成功
            ///],
        /// </summary>
        public string state { get; set; }

        public string stateInfo { get; set; }

        public bool success { get; set; }

        /// <summary>
        /// 转换码，1成功
        /// </summary>
        public int convCode { get; set; }

        public int tryConvCounts { get; set; }

        public string rawPath { get; set; }

        public string convPath { get; set; }

        public string verifyPath { get; set; }
    }
}
