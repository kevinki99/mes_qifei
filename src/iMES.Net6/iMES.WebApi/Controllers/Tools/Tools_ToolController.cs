/*
 *代码由框架生成,任何更改都可能导致被代码生成器覆盖
 *如果要增加方法请在当前目录下Partial文件夹Tools_ToolController编写
 */
using Microsoft.AspNetCore.Mvc;
using iMES.Core.Controllers.Basic;
using iMES.Entity.AttributeManager;
using iMES.Tools.IServices;
namespace iMES.Tools.Controllers
{
    [Route("api/Tools_Tool")]
    [PermissionTable(Name = "Tools_Tool")]
    public partial class Tools_ToolController : ApiBaseController<ITools_ToolService>
    {
        public Tools_ToolController(ITools_ToolService service)
        : base(service)
        {
        }
    }
}

