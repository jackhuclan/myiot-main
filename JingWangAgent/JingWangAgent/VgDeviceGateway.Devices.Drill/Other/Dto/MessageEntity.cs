namespace VgDeviceGateway.Devices.Drill.Other.Dto
{
    public class MessageEntity
    {
        public object Data { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsSuccessful { get; set; }
        public string Information { get; set; }
        public string AtpFile { get; set; } = string.Empty;
    }
}
