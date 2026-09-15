namespace VgAutoDrill.Admin.Model.ViewModels.Mes.External.BominResponse
{
    public class DrillWithPanelsResponseDto
    {
        public Data? data { get; set; }

        public string? checkType { get; set; }

        public string? checkCode { get; set; }

        public string? severName { get; set; }

        public int? code { get; set; }

        public string? success { get; set; }

        public Message? message { get; set; }

        public string? serverDatetime { get; set; }
    }

    public class Data
    {
        /// <summary>
        ///true成功，false失败
        /// </summary>
        public string? Success { get; set; }

        /// <summary>
        ///失败说明
        /// </summary>
        public string? Content { get; set; }
    }

    public class Message
    {
        public string? code { get; set; }

        public string? content { get; set; }

        public string? msg { get; set; }

        public string? stackTrace { get; set; }
    }
}