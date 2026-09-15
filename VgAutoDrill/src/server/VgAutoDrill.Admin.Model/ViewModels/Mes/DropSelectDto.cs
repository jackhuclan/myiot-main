namespace VgAutoDrill.Admin.Model.ViewModels.Mes
{
    public class DropSelectDto
    {
        /// <summary>
        /// id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// code(名称)
        /// </summary>
        public string? Label { get; set; }

        public string? PartitionCode { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string? Code { get; set; }

        public string? Name { get; set; }
    }
}
