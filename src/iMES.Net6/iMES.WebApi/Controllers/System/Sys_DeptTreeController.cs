/*
 *代码由框架生成,任何更改都可能导致被代码生成器覆盖
 *如果要增加方法请在当前目录下Partial文件夹Sys_DeptTreeController编写
 */
using Microsoft.AspNetCore.Mvc;
using iMES.Core.Controllers.Basic;
using iMES.Entity.AttributeManager;
using iMES.System.IServices;
namespace iMES.System.Controllers
{
    [Route("api/Sys_DeptTree")]
    [PermissionTable(Name = "Sys_DeptTree")]
    public partial class Sys_DeptTreeController : ApiBaseController<ISys_DeptTreeService>
    {
        public Sys_DeptTreeController(ISys_DeptTreeService service)
        : base(service)
        {
        }
    }
}

