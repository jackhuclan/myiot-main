using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VgMySqlStore.Plugin.Models;

[Table("PC_DATA_EMP")]
public class PC_DATA_EMP
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ID { get; set; }
    public string EMPNO { get; set; }
    public string EMPNAME { get; set; }
    public string EMPCODE { get; set; }
    public string EMPDES { get; set; }
    public DateTime EMPTIME { get; set; }
    public DateTime CREATETIME { get; set; }

}
