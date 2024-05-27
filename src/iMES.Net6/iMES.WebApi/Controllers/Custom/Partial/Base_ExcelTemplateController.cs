/*
 *接口编写处...
*如果接口需要做Action的权限验证，请在Action上使用属性
*如: [ApiActionPermission("Base_ExcelTemplate",Enums.ActionPermissionOptions.Search)]
 */
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using iMES.Entity.DomainModels;
using iMES.Custom.IServices;
using iMES.Custom.IRepositories;
using System.Linq;
using iMES.Core.Filters;
using iMES.Core.Enums;

namespace iMES.Custom.Controllers
{
    public partial class Base_ExcelTemplateController
    {
        private readonly IBase_ExcelTemplateService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBase_ExcelTemplateRepository _templateRepository;

        [ActivatorUtilitiesConstructor]
        public Base_ExcelTemplateController(
            IBase_ExcelTemplateService service,
            IBase_ExcelTemplateRepository templateRepository,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _templateRepository = templateRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        [Route("updateStatus"), HttpGet]
        [ApiActionPermission(ActionPermissionOptions.Update)]
        public IActionResult UpdateStatus(Guid templateId, int statusFlag)
        {
            var catalogId = _templateRepository.FindAsIQueryable(x => x.ExcelTemplateId == templateId)
                              .Select(s => s.CatalogId)
                              .FirstOrDefault();
            var list = _templateRepository.FindAsIQueryable(x => x.StatusFlag == 1 && x.CatalogId == catalogId && x.ExcelTemplateId != templateId)
                               .ToList();
            for (int i = 0; i < list.Count; i++)
            {
                list[i].StatusFlag = 0;
            }
            _templateRepository.UpdateRange(list, true);
            Base_ExcelTemplate printTemplate = new Base_ExcelTemplate()
            {
                ExcelTemplateId = templateId,
                StatusFlag = statusFlag
            };
            _templateRepository.Update(printTemplate, x => new { x.StatusFlag }, true);
            return Content("修改成功");
        }
    }
}
