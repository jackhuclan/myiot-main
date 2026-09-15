namespace VgAutoDrill.Central.Core.Mes.Model
{
    public class CheckSZKWSNRequest
    {
        public string ContainerName { get; set; }
    }

    public class CheckSNResponse
    {
        public string Message { get; set; }

        public string[] PnlCode { get; set; }
        public string ReadingSwitch { get; set; }
    }

}
