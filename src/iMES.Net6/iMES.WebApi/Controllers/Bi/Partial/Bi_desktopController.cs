/*
 *接口编写处...
*如果接口需要做Action的权限验证，请在Action上使用属性
*如: [ApiActionPermission("Bi_desktop",Enums.ActionPermissionOptions.Search)]
 */
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using iMES.Entity.DomainModels;
using iMES.Bi.IServices;
using iMES.Bi.IRepositories;
using System.Linq;
using iMES.Core.ManageUser;
using Microsoft.EntityFrameworkCore;

namespace iMES.Bi.Controllers
{
    public partial class Bi_desktopController
    {
        private readonly IBi_desktopService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBi_desktopRepository _desktopRepository;

        [ActivatorUtilitiesConstructor]
        public Bi_desktopController(
            IBi_desktopService service,
            IHttpContextAccessor httpContextAccessor,
            IBi_desktopRepository desktopRepository
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
            _desktopRepository = desktopRepository;
        }

        [Route("updateStatus"), HttpGet]
        public IActionResult UpdateStatus(int desktopId, int statusFlag)
        {
            Bi_desktop desktop = new Bi_desktop()
            {
                DesktopId = desktopId,
                IsDefault = statusFlag
            };
            _desktopRepository.Update(desktop, x => new { x.IsDefault }, true);
            return Content("修改成功");
        }

        /// <summary>
        /// 获取主页数据
        /// </summary>
        /// <returns></returns>
        [Route("getDesktop"), HttpGet]
        public async Task<IActionResult> GetDesktop()
        {
            var rows = await _desktopRepository.FindAsIQueryable(x => x.IsDefault == 1 && x.CreateID == UserContext.Current.UserId)
                  .ToListAsync();
            return JsonNormal(rows);
        }
    }
}
