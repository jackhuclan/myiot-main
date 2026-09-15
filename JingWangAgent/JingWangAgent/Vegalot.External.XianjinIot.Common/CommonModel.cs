// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Vegalot.External.XianjinIot.Common
{
    public class CommonModel
    {
        /// <summary>
        /// 公钥(注册时，用来加密会话密钥CommonModel.Key)
        /// </summary>
        public static string PublicKey { get; set; } = string.Empty;

        /// <summary>
        /// sn
        /// </summary>
        public static string sn { get; set; } = string.Empty;

        /// <summary>
        /// 会话密钥(注册完成以后，用来签名body)
        /// </summary>
        public static string Key { get; set; } = string.Empty;

        public static string DrillTaskCode { get; set; } = string.Empty;
    }
}
