// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VegaIot.External.Kinwong
{
    [Table("PC_SIGNAL_STATUS")]
    public class PC_SIGNAL_STATUS
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        /// <summary>
        /// 计数
        /// </summary>
        public string SIGNALCODE { get; set; }

        public string SIGNALDES { get; set; }
        public string SIGNALVALUE { get; set; }

        /// <summary>
        /// 时间:yy-mm-dd hh:mm:ss
        /// </summary>
        public DateTime CREATETIME { get; set; }
    }
}
