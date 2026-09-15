using System.ComponentModel.DataAnnotations;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.Rack
{
    public class UnBindRackAndSiolReq
    {
        /// <summary>
        /// 料架编码
        /// </summary>
        [Required]
        public virtual string? RackCode { get; set; }
    }
}
