/*
 *接口编写处...
*如果接口需要做Action的权限验证，请在Action上使用属性
*如: [ApiActionPermission("Sys_WorkFlow",Enums.ActionPermissionOptions.Search)]
 */
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using iMES.Entity.DomainModels;
using iMES.System.IServices;
using iMES.Core.WorkFlow;
using iMES.System.IRepositories;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using iMES.Core.ManageUser;
using iMES.Core.Services;
using iMES.Core.Infrastructure;

namespace iMES.System.Controllers
{
    public partial class Sys_WorkFlowController
    {
        private readonly ISys_WorkFlowService _service;//访问业务代码
        private readonly ISys_UserRepository _userRepository;
        private readonly ISys_RoleRepository _roleRepository;
        private readonly ISys_DepartmentRepository _departmentRepository;
        private readonly ISys_WorkFlowRepository _workFlowRepository;
        private readonly ISys_WorkFlowTableRepository _workFlowTableRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public Sys_WorkFlowController(
            ISys_WorkFlowService service,
            ISys_UserRepository userRepository,
            ISys_RoleRepository roleRepository,
            ISys_WorkFlowRepository workFlowRepository,
            ISys_WorkFlowTableRepository workFlowTableRepository,
            IHttpContextAccessor httpContextAccessor,
            ISys_DepartmentRepository departmentRepository
        )
        : base(service)
        {
            _service = service;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _departmentRepository = departmentRepository;
            _workFlowRepository = workFlowRepository;
            _workFlowTableRepository = workFlowTableRepository;
            _httpContextAccessor = httpContextAccessor;

        }
        /// <summary>
        /// 获取工作流程表数据源
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("getTableInfo")]
        public IActionResult GetTableInfo()
        {
            return Json(WorkFlowContainer.GetDic());
        }

        /// <summary>
        /// 获取流程节点数据源(用户与角色)
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("getNodeDic")]
        public async Task<IActionResult> GetNodeDic()
        {
            var data = new
            {
                users = await _userRepository.FindAsIQueryable(x => true).Select(s => new { key = s.User_Id, value = s.UserTrueName }).Take(5000).ToListAsync(),
                roles = await _roleRepository.FindAsIQueryable(x => true).Select(s => new { key = s.Role_Id, value = s.RoleName }).ToListAsync(),
                dept = await _departmentRepository.FindAsIQueryable(x => true).Select(s => new { key = s.DepartmentId, value = s.DepartmentName }).ToListAsync(),
            };
            return Json(data);
        }
        /// <summary>
        /// 获取单据的审批流程进度
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, Route("getSteps")]
        public async Task<IActionResult> GetSteps([FromBody] List<string> ids, string tableName)
        {
            var flows = await _workFlowTableRepository.FindAsIQueryable(x => x.WorkTable == tableName && ids.Contains(x.WorkTableKey))
                               .Include(x => x.Sys_WorkFlowTableStep)
                               .ToListAsync();
            //不在审核中的数据
            if (flows.Count == 0)
            {
                return Json(new { status = true });
            }
            if (flows.Count > 1 || flows.Count != ids.Count)
            {
                return Json(new { status = false, message = "只能选择一条数据进行审核" });
            }
            var flow = flows[0];
            var user = UserContext.Current.UserInfo;

            var auditUsers = flow.Sys_WorkFlowTableStep.Where(x => x.AuditId != null && x.StepType == (int)AuditType.用户审批).Select(s => s.AuditId).ToArray();
            var users = await _userRepository.FindAsIQueryable(x => auditUsers.Contains(x.User_Id))
                                        .Select(u => new { u.User_Id, u.UserTrueName }).ToListAsync();

            var log = await _workFlowTableRepository.DbContext.Set<Sys_WorkFlowTableAuditLog>()
                  .Where(x => x.WorkFlowTable_Id == flow.WorkFlowTable_Id)
                  .OrderBy(x=>x.CreateDate)
                  .ToListAsync();

            var steps = flow.Sys_WorkFlowTableStep
                    .Select(c => new
                    {
                        c.AuditId,
                        //这里没显示具体是哪个用户审核
                        Auditor = c.Auditor ?? users.Where(us => us.User_Id == c.AuditId).Select(us => us.UserTrueName).FirstOrDefault(),
                        c.AuditDate,
                        c.AuditStatus,
                        c.Remark,
                        c.StepValue,
                        c.StepName,
                        c.OrderId,
                        c.Enable,
                        c.StepId,
                        c.StepAttrType,
                        c.CreateDate,
                        c.Creator,
                        //判断是按角色审批 还是用户帐号审批
                        isCurrentUser = (c.AuditStatus == null || c.AuditStatus == (int)AuditStatus.审核中 || c.AuditStatus == (int)AuditStatus.待审核)
                                        && c.StepId == flow.CurrentStepId && GetAuditStepValue(c),
                        isCurrent = c.StepId == flow.CurrentStepId && c.AuditStatus != (int)AuditStatus.审核通过
                    }).OrderBy(o => o.OrderId);
            var data = new
            {
                status = true,
                step = flow.CurrentStepId,
                flow.AuditStatus,
                auditDic = DictionaryManager.GetDictionary("audit")?.Sys_DictionaryList?.Select(s => new { key = s.DicValue, value = s.DicName }),
                list = steps.OrderBy(x =>x.OrderId ).ToList(),
                log = log,
            };

            return Json(data);
        }
        [HttpPost, Route("getFields")]
        public async Task<IActionResult> GetFields(string table)
        {
            var query = _workFlowTableRepository.DbContext.Set<Sys_TableColumn>().Where(c => c.TableName == table);
            var fields = WorkFlowContainer.GetFilterFields(table);
            if (fields != null && fields.Length > 0)
            {
                query = query.Where(x => fields.Contains(x.ColumnName));
            }
            else
            {
                query = query.Where(x => x.IsDisplay == 1);
            }
            var columns = await query.OrderByDescending(c => c.OrderNo)
                 .Select(s => new
                 {
                     field = s.ColumnName,
                     name = s.ColumnCnName,
                     dicNo = s.DropNo,
                     s.ColumnType
                 }).ToListAsync();


            var data = columns.Select(s => new
            {
                s.field,
                s.name,
                s.dicNo,
                columnType = s.ColumnType,
                data = string.IsNullOrEmpty(s.dicNo)
                ? null
                : DictionaryManager.GetDictionary(s.dicNo)?.Sys_DictionaryList?.Select(c => new { key = c.DicValue, value = c.DicName })?.ToList()
            }).ToList();
            return JsonNormal(data);

        }

        private bool GetAuditStepValue(Sys_WorkFlowTableStep flow)
        {
            if (flow.StepType == (int)AuditType.角色审批)
            {
                return UserContext.Current.RoleId.ToString() == flow.StepValue;
            }
            //按部门审批
            if (flow.StepType == (int)AuditType.部门审批)
            {
                return UserContext.Current.UserInfo.DeptIds.Select(s => s.ToString()).Contains(flow.StepValue);
            }
            //按用户审批
            return UserContext.Current.UserId.ToString() == flow.StepValue;

        }
        [Route("getOptions"), HttpGet]
        public async Task<IActionResult> GetOptions(Guid id)
        {
            var data = await _workFlowRepository.FindAsIQueryable(x => x.WorkFlow_Id == id)
                .Include(c => c.Sys_WorkFlowStep)
                .FirstOrDefaultAsync();
            return JsonNormal(data);
        }
    }
}
