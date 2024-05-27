/*
 *代码由框架生成,任何更改都可能导致被代码生成器覆盖
 *如果数据库字段发生变化，请在代码生器重新生成此Model
 */
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iMES.Entity.SystemModels;

namespace iMES.Entity.DomainModels
{
    [Entity(TableCnName = "仪表盘",TableName = "Bi_db_ybp",DBServer = "SysDbContext")]
    public partial class Bi_db_ybp:SysEntity
    {
        /// <summary>
       ///
       /// </summary>
       [Key]
       [Display(Name ="ID")]
       [Column(TypeName="int")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public int ID { get; set; }

       /// <summary>
       ///数据集ID
       /// </summary>
       [Display(Name ="数据集ID")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? DimID { get; set; }

       /// <summary>
       ///字段名称
       /// </summary>
       [Display(Name ="字段名称")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       [Editable(true)]
       public string Name { get; set; }

       /// <summary>
       ///字段别名
       /// </summary>
       [Display(Name ="字段别名")]
       [Column(TypeName="nvarchar(max)")]
       [Editable(true)]
       public string YBContent { get; set; }

       /// <summary>
       ///字段类型
       /// </summary>
       [Display(Name ="字段类型")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       [Editable(true)]
       public string YBType { get; set; }

       /// <summary>
       ///1维度，2度量
       /// </summary>
       [Display(Name ="1维度，2度量")]
       [Column(TypeName="nvarchar(max)")]
       [Editable(true)]
       public string YBOption { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="CRUser")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       [Editable(true)]
       public string CRUser { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="CRDate")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       public DateTime? CRDate { get; set; }

       /// <summary>
       ///原字段
       /// </summary>
       [Display(Name ="原字段")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       [Editable(true)]
       public string Remark { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="YBSet")]
       [Column(TypeName="varchar(max)")]
       [Editable(true)]
       public string YBSet { get; set; }

       
    }
}