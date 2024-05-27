/*
 *代码由框架生成,任何更改都可能导致被代码生成器覆盖
 *如果要增加方法请在当前目录下Partial文件夹Sys_DepartmentController编写
 */
using Microsoft.AspNetCore.Mvc;
using iMES.Core.Controllers.Basic;
using iMES.Entity.AttributeManager;
using iMES.System.IServices;
namespace iMES.System.Controllers
{
    [Route("api/Sys_Department")]
    [PermissionTable(Name = "Sys_Department")]
    public partial class Sys_DepartmentController : ApiBaseController<ISys_DepartmentService>
    {
        public Sys_DepartmentController(ISys_DepartmentService service)
        : base(service)
        {
        }
    }
}

