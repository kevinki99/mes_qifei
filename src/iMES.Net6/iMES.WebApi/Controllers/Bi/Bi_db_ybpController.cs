/*
 *代码由框架生成,任何更改都可能导致被代码生成器覆盖
 *如果要增加方法请在当前目录下Partial文件夹Bi_db_ybpController编写
 */
using Microsoft.AspNetCore.Mvc;
using iMES.Core.Controllers.Basic;
using iMES.Entity.AttributeManager;
using iMES.Bi.IServices;
namespace iMES.Bi.Controllers
{
    [Route("api/Bi_db_ybp")]
    [PermissionTable(Name = "Bi_db_ybp")]
    public partial class Bi_db_ybpController : ApiBaseController<IBi_db_ybpService>
    {
        public Bi_db_ybpController(IBi_db_ybpService service)
        : base(service)
        {
        }
    }
}

