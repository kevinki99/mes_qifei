/*
 *代码由框架生成,任何更改都可能导致被代码生成器覆盖
 *如果要增加方法请在当前目录下Partial文件夹Base_ExcelTemplateController编写
 */
using Microsoft.AspNetCore.Mvc;
using iMES.Core.Controllers.Basic;
using iMES.Entity.AttributeManager;
using iMES.Custom.IServices;
namespace iMES.Custom.Controllers
{
    [Route("api/Base_ExcelTemplate")]
    [PermissionTable(Name = "Base_ExcelTemplate")]
    public partial class Base_ExcelTemplateController : ApiBaseController<IBase_ExcelTemplateService>
    {
        public Base_ExcelTemplateController(IBase_ExcelTemplateService service)
        : base(service)
        {
        }
    }
}

