/*
 *代码由框架生成,任何更改都可能导致被代码生成器覆盖
 *如果要增加方法请在当前目录下Partial文件夹Bi_db_dimController编写
 */
using Microsoft.AspNetCore.Mvc;
using iMES.Core.Controllers.Basic;
using iMES.Entity.AttributeManager;
using iMES.Bi.IServices;
namespace iMES.Bi.Controllers
{
    [Route("api/Bi_db_dim")]
    [PermissionTable(Name = "Bi_db_dim")]
    public partial class Bi_db_dimController : ApiBaseController<IBi_db_dimService>
    {
        public Bi_db_dimController(IBi_db_dimService service)
        : base(service)
        {
        }
    }
}

