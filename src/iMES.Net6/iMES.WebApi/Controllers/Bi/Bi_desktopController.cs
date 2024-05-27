/*
 *代码由框架生成,任何更改都可能导致被代码生成器覆盖
 *如果要增加方法请在当前目录下Partial文件夹Bi_desktopController编写
 */
using Microsoft.AspNetCore.Mvc;
using iMES.Core.Controllers.Basic;
using iMES.Entity.AttributeManager;
using iMES.Bi.IServices;
namespace iMES.Bi.Controllers
{
    [Route("api/Bi_desktop")]
    [PermissionTable(Name = "Bi_desktop")]
    public partial class Bi_desktopController : ApiBaseController<IBi_desktopService>
    {
        public Bi_desktopController(IBi_desktopService service)
        : base(service)
        {
        }
    }
}

