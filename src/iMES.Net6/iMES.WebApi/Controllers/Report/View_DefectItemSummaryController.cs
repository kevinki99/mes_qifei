/*
 *代码由框架生成,任何更改都可能导致被代码生成器覆盖
 *如果要增加方法请在当前目录下Partial文件夹View_DefectItemSummaryController编写
 */
using Microsoft.AspNetCore.Mvc;
using iMES.Core.Controllers.Basic;
using iMES.Entity.AttributeManager;
using iMES.Report.IServices;
namespace iMES.Report.Controllers
{
    [Route("api/View_DefectItemSummary")]
    [PermissionTable(Name = "View_DefectItemSummary")]
    public partial class View_DefectItemSummaryController : ApiBaseController<IView_DefectItemSummaryService>
    {
        public View_DefectItemSummaryController(IView_DefectItemSummaryService service)
        : base(service)
        {
        }
    }
}

