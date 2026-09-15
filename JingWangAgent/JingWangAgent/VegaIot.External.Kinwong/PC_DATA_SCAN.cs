using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VegaIot.External.Kinwong
{
    [Table("PC_DATA_SCAN")]
    public class PC_DATA_SCAN
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public string CONTAINER { get; set; }

        /// <summary>
        /// 码类型
        /// </summary>
        public string PARENTID { get; set; }

        /// <summary>
        /// 父级ID
        /// </summary>
        public string DRCTYPE { get; set; }

        /// <summary>
        /// 板码（PNL码)
        /// </summary>
        public string PNLIDORSETID { get; set; }
        public string CREATEUSER { get; set; }
        /// <summary>
        /// 时间:yy-mm-dd hh:mm:ss
        /// </summary>
        public DateTime CREATETIME { get; set; }
    }
}
