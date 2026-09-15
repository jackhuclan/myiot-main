namespace VgAutoDrill.Admin.Model.ViewModels.Mes.External
{
    public class ExternaCutterlTaskDto
    {
        public string? GroupNo { get; set; }
        public string? ItemName { get; set; }
        public string? DrillNo { get; set; }
        public string? DrillName { get; set; }
        public List<CutterGroupDetails> CutterGroupInfo { get; set; } = new List<CutterGroupDetails>();
        public DateTime PlannedTime { get; set; }
        public int RoundNum { get; set; }
        public int AxisCount { get; set; }
        public int EndAxisNum { get; set; }
        public int CutterBoxNum { get; set; }
        public DateTime GroupDate { get; set; }
    }

    public class CutterGroupDetails
    {
        public string? ItemCode { get; set; }
        public int? Count { get; set; }
    }
}
