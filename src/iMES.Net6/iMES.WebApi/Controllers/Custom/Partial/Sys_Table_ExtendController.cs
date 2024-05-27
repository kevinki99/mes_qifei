/*
 *接口编写处...
*如果接口需要做Action的权限验证，请在Action上使用属性
*如: [ApiActionPermission("Sys_Table_Extend",Enums.ActionPermissionOptions.Search)]
 */
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using iMES.Entity.DomainModels;
using iMES.Custom.IServices;
using iMES.Core.Enums;
using iMES.Core.Filters;

namespace iMES.Custom.Controllers
{
    public partial class Sys_Table_ExtendController
    {
        private readonly ISys_Table_ExtendService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public Sys_Table_ExtendController(
            ISys_Table_ExtendService service,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet, HttpPost, Route("getTableExtendField")]
        [ApiActionPermission(ActionPermissionOptions.Search)]
        public async Task<IActionResult> GetTableExtendFieldList(string tableName)
        {
            return Json(await _service.GetTableExtendFieldList(tableName));
        }
    }
}
