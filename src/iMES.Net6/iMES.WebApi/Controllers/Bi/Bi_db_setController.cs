/*
 *代码由框架生成,任何更改都可能导致被代码生成器覆盖
 *如果要增加方法请在当前目录下Partial文件夹Bi_db_setController编写
 */
using Microsoft.AspNetCore.Mvc;
using iMES.Core.Controllers.Basic;
using iMES.Entity.AttributeManager;
using iMES.Bi.IServices;
namespace iMES.Bi.Controllers
{
    [Route("api/Bi_db_set")]
    [PermissionTable(Name = "Bi_db_set")]
    public partial class Bi_db_setController : ApiBaseController<IBi_db_setService>
    {
        public Bi_db_setController(IBi_db_setService service)
        : base(service)
        {
        }
    }
}

