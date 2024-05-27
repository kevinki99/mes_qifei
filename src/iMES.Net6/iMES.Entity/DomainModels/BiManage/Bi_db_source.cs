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
    [Entity(TableCnName = "数据源管理",TableName = "Bi_db_source",DBServer = "SysDbContext")]
    public partial class Bi_db_source:SysEntity
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
       ///名称
       /// </summary>
       [Display(Name ="名称")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       [Editable(true)]
       public string Name { get; set; }

       /// <summary>
       ///数据连接类型（SQLSERVER，MYSQL
       /// </summary>
       [Display(Name ="数据连接类型（SQLSERVER，MYSQL")]
       [MaxLength(20)]
       [Column(TypeName="nvarchar(20)")]
       [Editable(true)]
       public string DBType { get; set; }

       /// <summary>
       ///数据库地址
       /// </summary>
       [Display(Name ="数据库地址")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       [Editable(true)]
       public string DBIP { get; set; }

       /// <summary>
       ///端口
       /// </summary>
       [Display(Name ="端口")]
       [MaxLength(10)]
       [Column(TypeName="nvarchar(10)")]
       [Editable(true)]
       public string Port { get; set; }

       /// <summary>
       ///数据库
       /// </summary>
       [Display(Name ="数据库")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       [Editable(true)]
       public string DBName { get; set; }

       /// <summary>
       ///架构
       /// </summary>
       [Display(Name ="架构")]
       [MaxLength(10)]
       [Column(TypeName="nvarchar(10)")]
       [Editable(true)]
       public string Schema { get; set; }

       /// <summary>
       ///用户名
       /// </summary>
       [Display(Name ="用户名")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       [Editable(true)]
       public string DBUser { get; set; }

       /// <summary>
       ///密码
       /// </summary>
       [Display(Name ="密码")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       [Editable(true)]
       public string DBPwd { get; set; }

       /// <summary>
       ///
       /// </summary>
       [Display(Name ="Attach")]
       [MaxLength(50)]
       [Column(TypeName="nvarchar(50)")]
       [Editable(true)]
       public string Attach { get; set; }

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

       
    }
}