/*
 *代码由框架生成,此处任何更改都可能导致被代码生成器覆盖
 *所有业务编写全部应在Partial文件夹下Sys_WorkFlowTableAuditLogService与ISys_WorkFlowTableAuditLogService中编写
 */
using iMES.System.IRepositories;
using iMES.System.IServices;
using iMES.Core.BaseProvider;
using iMES.Core.Extensions.AutofacManager;
using iMES.Entity.DomainModels;

namespace iMES.System.Services
{
    public partial class Sys_WorkFlowTableAuditLogService : ServiceBase<Sys_WorkFlowTableAuditLog, ISys_WorkFlowTableAuditLogRepository>
    , ISys_WorkFlowTableAuditLogService, IDependency
    {
    public Sys_WorkFlowTableAuditLogService(ISys_WorkFlowTableAuditLogRepository repository)
    : base(repository)
    {
    Init(repository);
    }
    public static ISys_WorkFlowTableAuditLogService Instance
    {
      get { return AutofacContainerModule.GetService<ISys_WorkFlowTableAuditLogService>(); } }
    }
 }
