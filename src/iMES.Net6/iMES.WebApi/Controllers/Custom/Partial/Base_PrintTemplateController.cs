/*
 *接口编写处...
*如果接口需要做Action的权限验证，请在Action上使用属性
*如: [ApiActionPermission("Base_PrintTemplate",Enums.ActionPermissionOptions.Search)]
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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Newtonsoft.Json.Linq;
using iMES.Core.DBManager;
using Newtonsoft.Json;
using iMES.Core.Utilities;
using iMES.Core.Filters;
using iMES.Core.Enums;

namespace iMES.Custom.Controllers
{
    public partial class Base_PrintTemplateController
    {
        private readonly IBase_PrintTemplateService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBase_PrintTemplateRepository _templateRepository;
        private readonly IBase_PrintCatalogRepository _templateCatalogRepository;

        [ActivatorUtilitiesConstructor]
        public Base_PrintTemplateController(
            IBase_PrintTemplateService service,
            IHttpContextAccessor httpContextAccessor,
            IBase_PrintTemplateRepository templateRepository,
            IBase_PrintCatalogRepository templateCatalogRepository
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
            _templateRepository = templateRepository;
            _templateCatalogRepository = templateCatalogRepository;
        }

        [Route("updateStatus"), HttpGet]
        [ApiActionPermission(ActionPermissionOptions.Update)]
        public IActionResult UpdateStatus(Guid templateId, int statusFlag)
        {
            var catalogId = _templateRepository.FindAsIQueryable(x => x.PrintTemplateId == templateId)
                              .Select(s => s.CatalogId)
                              .FirstOrDefault();
            var list =  _templateRepository.FindAsIQueryable(x => x.StatusFlag == 1 && x.CatalogId == catalogId && x.PrintTemplateId != templateId)
                               .ToList();
            for (int i = 0; i < list.Count; i++)
            {
                list[i].StatusFlag = 0;
            }
            _templateRepository.UpdateRange(list, true);
            Base_PrintTemplate printTemplate = new Base_PrintTemplate()
            {
                PrintTemplateId = templateId,
                StatusFlag = statusFlag
            };
            _templateRepository.Update(printTemplate, x => new { x.StatusFlag }, true);
            return Content("修改成功");
        }

        [HttpPost, HttpGet, Route("saveOrUpdateData"), AllowAnonymous]
        [AcceptVerbs("GET", "POST")]
        public IActionResult saveOrUpdateData(string stage,string id,string cat,string token)
        {
            PrintOutput print = new PrintOutput();
            if (!VerificationToken(token))
            {
                print.message = "Token校验失败";
                print.status = 1;
                print.success = false;
                return JsonNormal(print);
            }
            int? isDefault = _templateRepository.FindAsIQueryable(x => x.PrintTemplateId == new Guid(id))
                              .OrderByDescending(x => x.CreateDate)
                              .Select(s =>s.isDefault)
                              .FirstOrDefault();
            if (isDefault == 1)
            {
                print.message = "系统内置模版不允许编辑";
                print.data = (JObject)JsonConvert.DeserializeObject("{}");
                print.status = 1;
                print.success = false;
                return JsonNormal(print);
            }
            else
            {
                Base_PrintTemplate printTemplate = new Base_PrintTemplate()
                {
                    PrintTemplateId = new Guid(id),
                    TemplateContent = stage
                };
                _templateRepository.Update(printTemplate, x => new { x.TemplateContent }, true);
                print.message = "成功";
                print.data = (JObject)JsonConvert.DeserializeObject("{}");
                print.status = 1;
                print.success = true;
                return JsonNormal(print);
            }
        }

        [HttpPost, HttpGet, Route("getResourceByCatId"), AllowAnonymous]
        public IActionResult getResourceByCatId(string id,string cat,string token)
        {
            PrintOutput printOp = new PrintOutput();
            if (!VerificationToken(token))
            {
                printOp.message = "Token校验失败";
                printOp.status = 1;
                printOp.success = false;
                return JsonNormal(printOp);
            }
            string tableNameMain = string.Empty; 
            string tableNameDetail = string.Empty;
            switch (cat)
            {
                case "Production_SalesOrder":
                    tableNameMain = "Production_SalesOrder";
                    tableNameDetail = "Production_SalesOrderList";
                    break;
                case "Production_ProductPlan":
                    tableNameMain = "Production_ProductPlan";
                    tableNameDetail = "Production_ProductPlanList";
                    break; 
                case "Production_AssembleWorkOrder":
                    tableNameMain = "Production_AssembleWorkOrder";
                    tableNameDetail = "Production_AssembleWorkOrderList";
                    break; 
                case "Ware_OutWareHouseBill":
                    tableNameMain = "Ware_OutWareHouseBill";
                    tableNameDetail = "Ware_OutWareHouseBillList";
                    break;
                case "Ware_WareHouseBill":
                    tableNameMain = "Ware_WareHouseBill";
                    tableNameDetail = "Ware_WareHouseBillList";
                    break;
                case "Equip_Device":
                    tableNameMain = "Equip_Device";
                    tableNameDetail = "Equip_Device";
                    break;
                default:
                    tableNameMain = "Production_SalesOrder";
                    tableNameDetail = "Production_SalesOrderList";
                    break;
            }
            string sqlMain = @"SELECT 
                                  a.name [key],
                                  isnull(g.[value], '') name
                                FROM
                                  syscolumns a
                                  inner join sysobjects d on a.id = d.id
                                  and d.xtype = 'U'
                                  and d.name <> 'dtproperties'
                                  left join sys.extended_properties g on a.id = G.major_id
                                  and a.colid = g.minor_id
                                where
                                  d.name = @tableNameMain
                                order by
                                  a.id, 
                                  a.colorder";
            string sqlDetail = @"SELECT 
                                  a.name [key],
                                  isnull(g.[value], '') name
                                FROM
                                  syscolumns a
                                  inner join sysobjects d on a.id = d.id
                                  and d.xtype = 'U'
                                  and d.name <> 'dtproperties'
                                  left join sys.extended_properties g on a.id = G.major_id
                                  and a.colid = g.minor_id
                                where
                                  d.name = @tableNameDetail
                                order by
                                  a.id, 
                                  a.colorder";
            //与原生dapper使用方式基本一致，更多使用方法参照dapper文档
            List<filedDetail> main = DBServerProvider.SqlDapper.QueryList<filedDetail>(sqlMain, new { tableNameMain });
            List<filedDetail> detail = DBServerProvider.SqlDapper.QueryList<filedDetail>(sqlDetail, new { tableNameDetail });

            PrintFieldOutput print = new PrintFieldOutput();
            List<filed> list = new List<filed>();
            if (tableNameMain != "Equip_Device")
            {
                list.Add(new filed
                {
                    id = "bill",
                    name = "单据头",
                    tag = "dataBill",
                    fields = main
                });
            }
            list.Add(new filed
                {
                    id = "detail",
                    name = "单据体",
                    tag = "dataDetail",
                    fields = detail
                });
            print.data = list;
            print.message = "成功";
            print.status = 1;
            print.success = true;
            return JsonNormal(print);
        }
        [HttpPost, HttpGet, Route("getDataById"), AllowAnonymous]
        public IActionResult getDataById(string id, string cat,string token)
        {
            PrintOutput print = new PrintOutput();
            if (!VerificationToken(token))
            {
                print.message = "Token校验失败";
                print.status = 1;
                print.success = false;
                return JsonNormal(print);
            }
            string templateContent = string.Empty;
            if (string.IsNullOrWhiteSpace(id))
            {
                Guid catalogId = _templateCatalogRepository.FindAsIQueryable(x => x.CatalogCode == cat)
                               .OrderByDescending(x => x.CreateDate)
                               .Select(s => s.CatalogId)
                               .FirstOrDefault();
                templateContent = _templateRepository.FindAsIQueryable(x => x.CatalogId == catalogId && x.StatusFlag == 1)
                             .OrderByDescending(x => x.CreateDate)
                             .Select(s => s.TemplateContent)
                             .FirstOrDefault();
            }
            else
            {
                templateContent = _templateRepository.FindAsIQueryable(x => x.PrintTemplateId == new Guid(id))
                                   .OrderByDescending(x => x.CreateDate)
                                   .Select(s => s.TemplateContent)
                                   .FirstOrDefault();
            }
            JObject jo = new JObject();
            if (templateContent != null)
            {
                jo = (JObject)JsonConvert.DeserializeObject(templateContent);
            }
            print.message = "成功";
            print.data = jo;
            print.status = 1;
            print.success = true;
            return JsonNormal(print);
        }
        private bool VerificationToken(string token)
        {
            string requestToken = token?.Replace("Bearer ", "");
            int userId = JwtHelper.GetUserId(requestToken);
            if (string.IsNullOrWhiteSpace(token) || userId <= 0)
            {
                return false;
            }
            else {
                return true;
            };
        }

        [HttpPost, HttpGet, Route("getDataByIdPrint"), AllowAnonymous]
        public IActionResult getDataByIdPrint(string dataId, string cat, string token)
        {
            PrintOutput print = new PrintOutput();
            if (!VerificationToken(token))
            {
                print.message = "Token校验失败";
                print.status = 1;
                print.success = false;
                return JsonNormal(print);
            }
            string templateContent = string.Empty;
            Guid catalogId = _templateCatalogRepository.FindAsIQueryable(x => x.CatalogCode == cat)
                           .OrderByDescending(x => x.CreateDate)
                           .Select(s => s.CatalogId)
                           .FirstOrDefault();
            templateContent = _templateRepository.FindAsIQueryable(x => x.CatalogId == catalogId && x.StatusFlag == 1)
                         .OrderByDescending(x => x.CreateDate)
                         .Select(s => s.TemplateContent)
                         .FirstOrDefault();

            JObject jo = (JObject)JsonConvert.DeserializeObject(templateContent);
            print.message = "成功";
            print.data = jo;
            print.status = 1;
            print.success = true;
            return JsonNormal(print);
        }
        [HttpPost, HttpGet, Route("getBillData"), AllowAnonymous]
        public IActionResult getBillData(string id, string cat,string token)
        {
            PrintOutput print = new PrintOutput();
            if (!VerificationToken(token))
            {
                print.message = "Token校验失败";
                print.status = 1;
                print.success = false;
                return JsonNormal(print);
            }
            string tableNameMain = string.Empty;
            string sql = string.Empty;
            string jsonstr = string.Empty;
            switch (cat)
            {
                case "Production_SalesOrder":
                    tableNameMain = "Production_SalesOrder";
                    sql = "select * from " + tableNameMain + " where SalesOrder_Id=@salesOrder_Id ";
                    string salesOrder_Id = id;
                    //与原生dapper使用方式基本一致，更多使用方法参照dapper文档
                    Production_SalesOrder entity = DBServerProvider.SqlDapper.QueryList<Production_SalesOrder>(sql, new { salesOrder_Id })[0];
                    jsonstr = JsonConvert.SerializeObject(entity);
                    break;
                case "Production_ProductPlan":
                    tableNameMain = "Production_ProductPlan";
                    sql = "select * from " + tableNameMain + " where ProductPlan_Id=@productPlan_Id ";
                    string productPlan_Id = id;
                    Production_ProductPlan entityProductPlan = DBServerProvider.SqlDapper.QueryList<Production_ProductPlan>(sql, new { productPlan_Id })[0];
                    jsonstr = JsonConvert.SerializeObject(entityProductPlan);
                    break;
                case "Production_AssembleWorkOrder":
                    tableNameMain = "Production_AssembleWorkOrder";
                    sql = "select * from " + tableNameMain + " where AssembleWorkOrder_Id=@assembleWorkOrder_Id ";
                    string assembleWorkOrder_Id = id;
                    Production_AssembleWorkOrder entityAssembleWorkOrder = DBServerProvider.SqlDapper.QueryList<Production_AssembleWorkOrder>(sql, new { assembleWorkOrder_Id })[0];
                    jsonstr = JsonConvert.SerializeObject(entityAssembleWorkOrder);
                    break;
                case "Ware_OutWareHouseBill":
                    tableNameMain = "Ware_OutWareHouseBill";
                    sql = "select * from " + tableNameMain + " where OutWareHouseBill_Id=@outWareHouseBill_Id ";
                    string outWareHouseBill_Id = id;
                    Ware_OutWareHouseBill entityOutWareHouseBill = DBServerProvider.SqlDapper.QueryList<Ware_OutWareHouseBill>(sql, new { outWareHouseBill_Id })[0];
                    jsonstr = JsonConvert.SerializeObject(entityOutWareHouseBill);
                    break;
                case "Ware_WareHouseBill":
                    tableNameMain = "Ware_WareHouseBill";
                    sql = "select * from " + tableNameMain + " where WareHouseBill_Id=@wareHouseBill_Id ";
                    string wareHouseBill_Id = id;
                    Ware_WareHouseBill entityWareHouseBill = DBServerProvider.SqlDapper.QueryList<Ware_WareHouseBill>(sql, new { wareHouseBill_Id })[0];
                    jsonstr = JsonConvert.SerializeObject(entityWareHouseBill);
                    break;
                case "Equip_Device":
                    tableNameMain = "Equip_Device";
                    sql = "select * from " + tableNameMain + " where DeviceId in (" + id + ") ";
                    List<Equip_Device> entityDevice = DBServerProvider.SqlDapper.QueryList<Equip_Device>(sql, new { });
                    jsonstr = JsonConvert.SerializeObject(entityDevice);
                    break;
                default:
                    tableNameMain = "Production_SalesOrder";
                    sql = "select * from " + tableNameMain + " where SalesOrder_Id=@salesOrder_Id ";
                    string salesOrder_Id2 = id;
                    //与原生dapper使用方式基本一致，更多使用方法参照dapper文档
                    Production_SalesOrder entity2 = DBServerProvider.SqlDapper.QueryList<Production_SalesOrder>(sql, new { salesOrder_Id2 })[0];
                    jsonstr = JsonConvert.SerializeObject(entity2);
                    break;
            }
            JObject jo = (JObject)JsonConvert.DeserializeObject(jsonstr);
            print.message = "成功";
            print.data = jo;
            print.status = 1;
            print.success = true;
            return JsonNormal(print);
        }
        [HttpPost, HttpGet, Route("getReviewBillData"), AllowAnonymous]
        public IActionResult getReviewBillData(string id, string cat,string token)
        {
            PrintOutput print = new PrintOutput();
            if (!VerificationToken(token))
            {
                print.message = "Token校验失败";
                print.status = 1;
                print.success = false;
                return JsonNormal(print);
            }
            string tableNameMain = string.Empty;
            string sql = string.Empty;
            string jsonstr = string.Empty;
            switch (cat)
            {
                case "Production_SalesOrder":
                    tableNameMain = "Production_SalesOrder";
                    sql = "select top 1 * from " + tableNameMain;
                    string salesOrder_Id = id;
                    //与原生dapper使用方式基本一致，更多使用方法参照dapper文档
                    Production_SalesOrder  entity = DBServerProvider.SqlDapper.QueryList<Production_SalesOrder>(sql, new { salesOrder_Id })[0];
                    jsonstr = JsonConvert.SerializeObject(entity);
                    break;
                case "Production_ProductPlan":
                    tableNameMain = "Production_ProductPlan";
                    sql = "select top 1  * from " + tableNameMain;
                    string productPlan_Id = id;
                    Production_ProductPlan entityProductPlan = DBServerProvider.SqlDapper.QueryList<Production_ProductPlan>(sql, new { productPlan_Id })[0];
                    jsonstr = JsonConvert.SerializeObject(entityProductPlan);
                    break;
                case "Production_AssembleWorkOrder":
                    tableNameMain = "Production_AssembleWorkOrder";
                    sql = "select top 1  * from " + tableNameMain ;
                    string assembleWorkOrder_Id = id;
                    Production_AssembleWorkOrder entityAssembleWorkOrder = DBServerProvider.SqlDapper.QueryList<Production_AssembleWorkOrder>(sql, new { assembleWorkOrder_Id })[0];
                    jsonstr = JsonConvert.SerializeObject(entityAssembleWorkOrder);
                    break;
                case "Ware_OutWareHouseBill":
                    tableNameMain = "Ware_OutWareHouseBill";
                    sql = "select top 1 * from " + tableNameMain;
                    string outWareHouseBill_Id = id;
                    Ware_OutWareHouseBill entityOutWareHouseBill = DBServerProvider.SqlDapper.QueryList<Ware_OutWareHouseBill>(sql, new { outWareHouseBill_Id })[0];
                    jsonstr = JsonConvert.SerializeObject(entityOutWareHouseBill);
                    break;
                case "Ware_WareHouseBill":
                    tableNameMain = "Ware_WareHouseBill";
                    sql = "select top 1 * from " + tableNameMain;
                    string wareHouseBill_Id = id;
                    Ware_WareHouseBill entityWareHouseBill = DBServerProvider.SqlDapper.QueryList<Ware_WareHouseBill>(sql, new { wareHouseBill_Id })[0];
                    jsonstr = JsonConvert.SerializeObject(entityWareHouseBill);
                    break;
                case "Equip_Device":
                    tableNameMain = "Equip_Device";
                    sql = "select top 1 * from " + tableNameMain;
                    Equip_Device entityDevice = DBServerProvider.SqlDapper.QueryList<Equip_Device>(sql, new { })[0];
                    jsonstr = JsonConvert.SerializeObject(entityDevice);
                    break;
                default:
                    tableNameMain = "Production_SalesOrder";
                    sql = "select top 1  * from " + tableNameMain;
                    string salesOrder_Id2 = id;
                    //与原生dapper使用方式基本一致，更多使用方法参照dapper文档
                    Production_SalesOrder entity2 = DBServerProvider.SqlDapper.QueryList<Production_SalesOrder>(sql, new { salesOrder_Id2 })[0];
                    jsonstr = JsonConvert.SerializeObject(entity2);
                    break;
            }
            JObject jo = (JObject)JsonConvert.DeserializeObject(jsonstr);
            print.message = "成功";
            print.data = jo;
            print.status = 1;
            print.success = true;
            return JsonNormal(print);
        }
        [HttpPost, HttpGet, Route("getReviewDetailData"), AllowAnonymous]
        public IActionResult getReviewDetailData(string id, string cat,string token)
        {
            PrintOutput printOp = new PrintOutput();
            if (!VerificationToken(token))
            {
                printOp.message = "Token校验失败";
                printOp.status = 1;
                printOp.success = false;
                return JsonNormal(printOp);
            }
            string tableNameDetail = string.Empty;
            string sql = string.Empty;
            string jsonstr = string.Empty;
            switch (cat)
            {
                case "Production_SalesOrder":
                    tableNameDetail = "Production_SalesOrderList";
                    string salesOrder_Id = id;
                    sql = "select top 3 * from " + tableNameDetail;
                    List<Production_SalesOrderList> entityList = DBServerProvider.SqlDapper.QueryList<Production_SalesOrderList>(sql, new { salesOrder_Id });
                    jsonstr = JsonConvert.SerializeObject(entityList);
                    break;
                case "Production_ProductPlan":
                    tableNameDetail = "Production_ProductPlanList";
                    string productPlan_Id = id;
                    sql = "select  top 3 * from " + tableNameDetail;
                    List<Production_ProductPlanList> entityListProductPlan = DBServerProvider.SqlDapper.QueryList<Production_ProductPlanList>(sql, new { productPlan_Id });
                    jsonstr = JsonConvert.SerializeObject(entityListProductPlan);
                    break;
                case "Production_AssembleWorkOrder":
                    tableNameDetail = "Production_AssembleWorkOrderList";
                    string assembleWorkOrder_Id = id;
                    sql = "select  top 3 * from " + tableNameDetail;
                    List<Production_AssembleWorkOrderList> entityListAssembleWorkOrder = DBServerProvider.SqlDapper.QueryList<Production_AssembleWorkOrderList>(sql, new { assembleWorkOrder_Id });
                    jsonstr = JsonConvert.SerializeObject(entityListAssembleWorkOrder);
                    break;
                case "Ware_OutWareHouseBill":
                    tableNameDetail = "Ware_OutWareHouseBillList";
                    string outWareHouseBill_Id = id;
                    sql = "select   top 3 * from " + tableNameDetail;
                    List<Ware_OutWareHouseBillList> entityListOutWareHouseBill = DBServerProvider.SqlDapper.QueryList<Ware_OutWareHouseBillList>(sql, new { outWareHouseBill_Id });
                    jsonstr = JsonConvert.SerializeObject(entityListOutWareHouseBill);
                    break;
                case "Ware_WareHouseBill":
                    tableNameDetail = "Ware_WareHouseBillList";
                    string wareHouseBill_Id = id;
                    sql = "select  top 3 * from " + tableNameDetail;
                    List<Ware_WareHouseBillList> entityListWareHouseBill = DBServerProvider.SqlDapper.QueryList<Ware_WareHouseBillList>(sql, new { wareHouseBill_Id });
                    jsonstr = JsonConvert.SerializeObject(entityListWareHouseBill);
                    break;
                case "Equip_Device":
                    tableNameDetail = "Equip_Device";
                    sql = "select  top 3 * from " + tableNameDetail;
                    List<Equip_Device> entityDevice = DBServerProvider.SqlDapper.QueryList<Equip_Device>(sql, new {  });
                    jsonstr = JsonConvert.SerializeObject(entityDevice);
                    break;
                default:
                    tableNameDetail = "Production_SalesOrderList";
                    string salesOrder_Id2 = id;
                    sql = "select  top 3 * from " + tableNameDetail;
                    List<Production_SalesOrderList> entityList2 = DBServerProvider.SqlDapper.QueryList<Production_SalesOrderList>(sql, new { salesOrder_Id2 });
                    jsonstr = JsonConvert.SerializeObject(entityList2);
                    break;
            }
            PrintOutputArray print = new PrintOutputArray();
            JArray jo = (JArray)JsonConvert.DeserializeObject(jsonstr);
            print.message = "成功";
            print.data = jo;
            print.status = 1;
            print.success = true;
            return JsonNormal(print);
        }
        [HttpPost, HttpGet, Route("getDetailData"), AllowAnonymous]
        public IActionResult getDetailData(string id, string cat,string token)
        {
            PrintOutput printOp = new PrintOutput();
            if (!VerificationToken(token))
            {
                printOp.message = "Token校验失败";
                printOp.status = 1;
                printOp.success = false;
                return JsonNormal(printOp);
            }
            string tableNameDetail = string.Empty;
            string sql = string.Empty;
            string jsonstr = string.Empty;
            switch (cat)
            {
                case "Production_SalesOrder":
                    tableNameDetail = "Production_SalesOrderList";
                    string salesOrder_Id = id;
                    sql = "select * from " + tableNameDetail + " where SalesOrder_Id=@salesOrder_Id ";
                    List<Production_SalesOrderList> entityList = DBServerProvider.SqlDapper.QueryList<Production_SalesOrderList>(sql, new { salesOrder_Id });
                    jsonstr = JsonConvert.SerializeObject(entityList);
                    break;
                case "Production_ProductPlan":
                    tableNameDetail = "Production_ProductPlanList";
                    string productPlan_Id = id;
                    sql = "select * from " + tableNameDetail + " where ProductPlan_Id=@productPlan_Id ";
                    List<Production_ProductPlanList> entityListProductPlan = DBServerProvider.SqlDapper.QueryList<Production_ProductPlanList>(sql, new { productPlan_Id });
                    jsonstr = JsonConvert.SerializeObject(entityListProductPlan);
                    break;
                case "Production_AssembleWorkOrder":
                    tableNameDetail = "Production_AssembleWorkOrderList";
                    string assembleWorkOrder_Id = id;
                    sql = "select * from " + tableNameDetail + " where AssembleWorkOrder_Id=@assembleWorkOrder_Id  ";
                    List<Production_AssembleWorkOrderList> entityListAssembleWorkOrder = DBServerProvider.SqlDapper.QueryList<Production_AssembleWorkOrderList>(sql, new { assembleWorkOrder_Id });
                    jsonstr = JsonConvert.SerializeObject(entityListAssembleWorkOrder);
                    break;
                case "Ware_OutWareHouseBill":
                    tableNameDetail = "Ware_OutWareHouseBillList";
                    string outWareHouseBill_Id = id;
                    sql = "select * from " + tableNameDetail + " where OutWareHouseBill_Id=@outWareHouseBill_Id ";
                    List<Ware_OutWareHouseBillList> entityListOutWareHouseBill = DBServerProvider.SqlDapper.QueryList<Ware_OutWareHouseBillList>(sql, new { outWareHouseBill_Id });
                    jsonstr = JsonConvert.SerializeObject(entityListOutWareHouseBill);
                    break;
                case "Ware_WareHouseBill":
                    tableNameDetail = "Ware_WareHouseBillList";
                    string wareHouseBill_Id = id;
                    sql = "select * from " + tableNameDetail + " where WareHouseBill_Id=@wareHouseBill_Id ";
                    List<Ware_WareHouseBillList> entityListWareHouseBill = DBServerProvider.SqlDapper.QueryList<Ware_WareHouseBillList>(sql, new { wareHouseBill_Id });
                    jsonstr = JsonConvert.SerializeObject(entityListWareHouseBill);
                    break;
                case "Equip_Device":
                    tableNameDetail = "Equip_Device";
                    sql = "select * from " + tableNameDetail + " where DeviceId in ("+ id + ") ";
                    List<Equip_Device> entityDevice = DBServerProvider.SqlDapper.QueryList<Equip_Device>(sql, new {  });
                    jsonstr = JsonConvert.SerializeObject(entityDevice);
                    break;
                default:
                    tableNameDetail = "Production_SalesOrderList";
                    string salesOrder_Id2 = id;
                    sql = "select * from " + tableNameDetail + " where SalesOrder_Id=@salesOrder_Id ";
                    List<Production_SalesOrderList> entityList2 = DBServerProvider.SqlDapper.QueryList<Production_SalesOrderList>(sql, new { salesOrder_Id2 });
                    jsonstr = JsonConvert.SerializeObject(entityList2);
                    break;
            }
            PrintOutputArray print = new PrintOutputArray();
            JArray jo = (JArray)JsonConvert.DeserializeObject(jsonstr);
            print.message = "成功";
            print.data = jo;
            print.status = 1;
            print.success = true;
            return JsonNormal(print);
        }
    }
}
