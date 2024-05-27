/*
 *接口编写处...
*如果接口需要做Action的权限验证，请在Action上使用属性
*如: [ApiActionPermission("Base_MaterialDetailTree",Enums.ActionPermissionOptions.Search)]
 */
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using iMES.Entity.DomainModels;
using iMES.Custom.IServices;
using iMES.Core.Filters;
using iMES.Core.Enums;
using iMES.Custom.Repositories;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using iMES.Core.Extensions;

namespace iMES.Custom.Controllers
{
    public partial class Base_MaterialDetailTreeController
    {
        private readonly IBase_MaterialDetailTreeService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public Base_MaterialDetailTreeController(
            IBase_MaterialDetailTreeService service,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }
        public override ActionResult GetPageData([FromBody] PageDataOptions loadData)
        {
            //没有查询条件显示所有一级节点数据
            if (loadData.Value.GetInt() == 1)
            {
                return GetCatalogRootData(loadData);
            }
            //有查询条件使用框架默认的查询方法
            return base.GetPageData(loadData);
        }

        /// treetable 获取根节点数据
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("getCatalogRootData")]
        [ApiActionPermission(ActionPermissionOptions.Search)]
        public ActionResult GetCatalogRootData([FromBody] PageDataOptions options)
        {
            //页面加载(一级)根节点数据条件x => x.ParentId==null,自己根据需要设置
            var query = Base_MaterialDetailTreeRepository.Instance.FindAsIQueryable(x => x.ParentId == null);

            var rows = query.TakeOrderByPage(options.Page, options.Rows)
                .OrderBy(x => x.ProductName).Select(s => new
                {
                    s.MaterialDetailTree_Id,
                    s.ProductCode,
                    s.ProductName,
                    s.ProductStandard,
                    s.Unit_Id,
                    s.QuantityPer,
                    s.ParentId,
                    s.CreateID,
                    s.Creator,
                    s.CreateDate,
                    s.ModifyID,
                    s.Modifier,
                    s.ModifyDate,
                    hasChildren = true
                }).ToList();
            return JsonNormal(new { total = query.Count(), rows });
        }

        /// <summary>
        ///treetable 获取子节点数据
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("getChildrenData")]
        [ApiActionPermission(ActionPermissionOptions.Search)]
        public async Task<ActionResult> GetChildrenData(Guid materialDetailTreeId)
        {
            //点击节点时，加载子节点数据
            var roleRepository = Base_MaterialDetailTreeRepository.Instance.FindAsIQueryable(x => 1 == 1);

            var rows = await roleRepository.Where(x => x.ParentId == materialDetailTreeId)
                .Select(s => new
                {
                    s.MaterialDetailTree_Id,
                    s.ProductCode,
                    s.ProductName,
                    s.ProductStandard,
                    s.Unit_Id,
                    s.QuantityPer,
                    s.ParentId,
                    s.CreateID,
                    s.Creator,
                    s.CreateDate,
                    s.ModifyID,
                    s.Modifier,
                    s.ModifyDate,
                    hasChildren = roleRepository.Any(x => x.ParentId == s.MaterialDetailTree_Id)
                }).ToListAsync();
            return JsonNormal(new { rows });
        }
    }
}
