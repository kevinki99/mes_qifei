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
    [Entity(TableCnName = "实体扩展字段",TableName = "Sys_Table_Extend",DBServer = "SysDbContext")]
    public partial class Sys_Table_Extend:SysEntity
    {
        /// <summary>
       ///扩展表主键ID
       /// </summary>
       [Key]
       [Display(Name ="扩展表主键ID")]
       [Column(TypeName="int")]
       [Required(AllowEmptyStrings=false)]
       public int TableEx_Id { get; set; }

       /// <summary>
       ///表名
       /// </summary>
       [Display(Name ="表名")]
       [MaxLength(200)]
       [Column(TypeName="nvarchar(200)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string TableName { get; set; }

       /// <summary>
       ///字段名称
       /// </summary>
       [Display(Name ="字段名称")]
       [MaxLength(200)]
       [Column(TypeName="nvarchar(200)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string FieldName { get; set; }

       /// <summary>
       ///字段类型
       /// </summary>
       [Display(Name ="字段类型")]
       [MaxLength(200)]
       [Column(TypeName="nvarchar(200)")]
       [Editable(true)]
       [Required(AllowEmptyStrings=false)]
       public string FieldType { get; set; }

       /// <summary>
       ///字段编码
       /// </summary>
       [Display(Name ="字段编码")]
       [MaxLength(200)]
       [Column(TypeName="nvarchar(200)")]
       public string FieldCode { get; set; }

       /// <summary>
       ///数据字典
       /// </summary>
       [Display(Name ="数据字典")]
       [MaxLength(200)]
       [Column(TypeName="nvarchar(200)")]
       [Editable(true)]
       public string DataDic { get; set; }

       /// <summary>
       ///字段属性
       /// </summary>
       [Display(Name ="字段属性")]
       [MaxLength(200)]
       [Column(TypeName="nvarchar(200)")]
       [Editable(true)]
       public string FieldAttr { get; set; }

       /// <summary>
       ///引导文字
       /// </summary>
       [Display(Name ="引导文字")]
       [MaxLength(200)]
       [Column(TypeName="nvarchar(200)")]
       [Editable(true)]
       public string GuideWords { get; set; }

       /// <summary>
       ///创建时间
       /// </summary>
       [Display(Name ="创建时间")]
       [Column(TypeName="datetime")]
       public DateTime? CreateDate { get; set; }

       /// <summary>
       ///创建人编号
       /// </summary>
       [Display(Name ="创建人编号")]
       [Column(TypeName="int")]
       public int? CreateID { get; set; }

       /// <summary>
       ///创建人
       /// </summary>
       [Display(Name ="创建人")]
       [MaxLength(200)]
       [Column(TypeName="nvarchar(200)")]
       public string Creator { get; set; }

       /// <summary>
       ///修改人
       /// </summary>
       [Display(Name ="修改人")]
       [MaxLength(200)]
       [Column(TypeName="nvarchar(200)")]
       public string Modifier { get; set; }

       /// <summary>
       ///修改时间
       /// </summary>
       [Display(Name ="修改时间")]
       [Column(TypeName="datetime")]
       public DateTime? ModifyDate { get; set; }

       /// <summary>
       ///修改人编号
       /// </summary>
       [Display(Name ="修改人编号")]
       [Column(TypeName="int")]
       public int? ModifyID { get; set; }

       /// <summary>
       ///默认值
       /// </summary>
       [Display(Name ="默认值")]
       [MaxLength(200)]
       [Column(TypeName="nvarchar(200)")]
       [Editable(true)]
       public string DefaultValue { get; set; }

       
    }
}