using System.ComponentModel.DataAnnotations;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.Silo
{
    public class ExternalSiloUnBindReq
    {
        public List<UnBindSiloDetail> UnBindSiloDetails { get; set; } = new List<UnBindSiloDetail>();

    }

    public class UnBindSiloDetail
    {
        [Required]
        public virtual int? FloorNum { get; set; }
        [Required]
        public virtual string? LocationCode { get; set; }
    }

    public class ExternalSiloAllUnBindReq
    {
        [Required]
        public virtual string? SiloCode { get; set; }

    }
    public class ExternalSetSiloStatusReq
    {
        [Required]
        public virtual string? SiloCode { get; set; }

    }
}
