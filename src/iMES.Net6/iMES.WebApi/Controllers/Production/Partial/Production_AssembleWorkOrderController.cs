/*
 *接口编写处...
*如果接口需要做Action的权限验证，请在Action上使用属性
*如: [ApiActionPermission("Production_AssembleWorkOrder",Enums.ActionPermissionOptions.Search)]
 */
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using iMES.Entity.DomainModels;
using iMES.Production.IServices;
using iMES.Production.IRepositories;
using Microsoft.EntityFrameworkCore;
using iMES.Core.DBManager;
using iMES.Custom.IRepositories;
using System.Linq;
using iMES.Core.Configuration;
using iMES.Core.Extensions;
using MiniExcelLibs;
using iMES.Core.Filters;
using iMES.Core.Enums;

namespace iMES.Production.Controllers
{
    public partial class Production_AssembleWorkOrderController
    {
        private readonly IProduction_AssembleWorkOrderService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IProduction_AssembleWorkOrderListRepository _assembleListRepository;
        private readonly IProduction_AssembleWorkOrderRepository _assembleRepository;
        private readonly IBase_ExcelTemplateRepository _templateRepository;
        private readonly IBase_PrintCatalogRepository _templateCatalogRepository;

        [ActivatorUtilitiesConstructor]
        public Production_AssembleWorkOrderController(
            IProduction_AssembleWorkOrderService service,
            IHttpContextAccessor httpContextAccessor,
            IProduction_AssembleWorkOrderListRepository assembleListRepository,
            IProduction_AssembleWorkOrderRepository assembleRepository,
            IBase_ExcelTemplateRepository templateRepository,
            IBase_PrintCatalogRepository templateCatalogRepository
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
            _assembleListRepository = assembleListRepository;
            _assembleRepository = assembleRepository;
            _templateRepository = templateRepository;
            _templateCatalogRepository = templateCatalogRepository;
        }

        /// <summary>
        /// 获取装配订单产品明细列表
        /// </summary>
        /// <param name="SalesOrder_Id">装配订单ID</param>
        /// <returns></returns>
        [Route("getDetailRows"), HttpGet]
        [ApiActionPermission(ActionPermissionOptions.Search)]
        public async Task<IActionResult> GetDetailRows(int AssembleWorkOrder_Id)
        {
            var rows = await _assembleListRepository.FindAsIQueryable(x => x.AssembleWorkOrder_Id == AssembleWorkOrder_Id)
                  .ToListAsync();
            string woSql = " select * from Production_WorkOrder ";
            List<Production_WorkOrder> list = DBServerProvider.SqlDapper.QueryList<Production_WorkOrder>(woSql, new { });
            for (int i = 0; i < rows.Count; i++)
            {
                if (list.Exists(x => x.WorkOrderCode == rows[i].WorkOrderCode))
                {
                    rows[i].FinishQty = list.Find(x => x.WorkOrderCode == rows[i].WorkOrderCode).GoodQty;
                    rows[i].BadQty = list.Find(x => x.WorkOrderCode == rows[i].WorkOrderCode).NoGoodQty;
                }
                else
                {
                    rows[i].FinishQty = 0;
                    rows[i].BadQty = 0;
                }
            }
            //获取当前库存数量
            return JsonNormal(rows);
        }
        /// <summary>
        /// 导出Excel模版数据
        /// </summary>
        /// <param name="SalesOrder_Id">装配工单</param>
        ///    /// <param name="cat">单据类型</param>
        /// <returns></returns>
        [Route("exportExcelTemplate"), HttpGet]
        [ApiActionPermission(ActionPermissionOptions.Search)]
        public string ExportExcelTemplate(int AssembleWorkOrder_Id, string cat)
        {
            Guid catalogId = _templateCatalogRepository.FindAsIQueryable(x => x.CatalogCode == cat)
                 .OrderByDescending(x => x.CreateDate)
                 .Select(s => s.CatalogId)
                 .FirstOrDefault();
            var content = _templateRepository.FindAsIQueryable(x => x.CatalogId == catalogId && x.StatusFlag == 1)
                         .OrderByDescending(x => x.CreateDate)
                         .Select(s => s.TemplateContent)
                         .FirstOrDefault();
            if (string.IsNullOrEmpty(content))
                return "";
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";
            string path = AppSetting.GetSettingString("ExportExcelPath") + fileName;
            string templatePath = ("wwwroot/" + content).MapPath();
            var main = _assembleRepository.FindAsIQueryable(x => x.AssembleWorkOrder_Id == AssembleWorkOrder_Id)
                  .OrderByDescending(x => x.CreateDate)
                  .Select(s => new Production_AssembleWorkOrder
                  {
                      AssembleWorkOrderCode = s.AssembleWorkOrderCode,
                      WorkOrderQty = s.WorkOrderQty,
                      Remark = s.Remark,
                      CreateDate = s.CreateDate
                  })
                  .FirstOrDefault();
            var detail = _assembleListRepository.FindAsIQueryable(x => x.AssembleWorkOrder_Id == AssembleWorkOrder_Id)
                        .ToList();
            var value = new Dictionary<string, object>()
            {
                ["AssembleWorkOrderCode"] = main.AssembleWorkOrderCode,
#pragma warning disable CS8601 // 可能的 null 引用赋值。
                ["WorkOrderQty"] = main.WorkOrderQty,
#pragma warning restore CS8601 // 可能的 null 引用赋值。
                ["Remark"] = main.Remark,
#pragma warning disable CS8601 // 可能的 null 引用赋值。
                ["CreateDate"] = main.CreateDate,
#pragma warning restore CS8601 // 可能的 null 引用赋值。
                ["detail"] = detail,
                ["Total"] = detail.Sum(s => s.Qty),
            };
            MiniExcel.SaveAsByTemplate(path, templatePath, value);
            return fileName;
        }
    }
}
