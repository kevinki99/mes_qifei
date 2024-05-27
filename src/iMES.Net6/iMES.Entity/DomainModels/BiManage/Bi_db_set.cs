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
    [Entity(TableCnName = "数据集管理",TableName = "Bi_db_set",DBServer = "SysDbContext")]
    public partial class Bi_db_set:SysEntity
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
       ///数据源
       /// </summary>
       [Display(Name ="数据源")]
       [Column(TypeName="int")]
       [Editable(true)]
       public int? SID { get; set; }

       /// <summary>
       ///数据集名称
       /// </summary>
       [Display(Name ="数据集名称")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       [Editable(true)]
       public string Name { get; set; }

       /// <summary>
       ///对应数据源表名
       /// </summary>
       [Display(Name ="对应数据源表名")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       [Editable(true)]
       public string SName { get; set; }

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
       ///
       /// </summary>
       [Display(Name ="UPDate")]
       [Column(TypeName="datetime")]
       [Editable(true)]
       public DateTime? UPDate { get; set; }

       /// <summary>
       ///数据集类型
       /// </summary>
       [Display(Name ="数据集类型")]
       [MaxLength(255)]
       [Column(TypeName="nvarchar(255)")]
       [Editable(true)]
       public string Type { get; set; }

       /// <summary>
       ///数据集配置
       /// </summary>
       [Display(Name ="数据集配置")]
       [Column(TypeName="nvarchar(max)")]
       [Editable(true)]
       public string Option { get; set; }

       /// <summary>
       ///SQL语句
       /// </summary>
       [Display(Name ="SQL语句")]
       [Column(TypeName="nvarchar(max)")]
       [Editable(true)]
       public string DSQL { get; set; }

       
    }
}