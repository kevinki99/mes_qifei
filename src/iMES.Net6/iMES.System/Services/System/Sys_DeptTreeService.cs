/*
 *Author：COCO
 *代码由框架生成,此处任何更改都可能导致被代码生成器覆盖
 *所有业务编写全部应在Partial文件夹下Sys_DeptTreeService与ISys_DeptTreeService中编写
 */
using iMES.System.IRepositories;
using iMES.System.IServices;
using iMES.Core.BaseProvider;
using iMES.Core.Extensions.AutofacManager;
using iMES.Entity.DomainModels;

namespace iMES.System.Services
{
    public partial class Sys_DeptTreeService : ServiceBase<Sys_DeptTree, ISys_DeptTreeRepository>
    , ISys_DeptTreeService, IDependency
    {
    public Sys_DeptTreeService(ISys_DeptTreeRepository repository)
    : base(repository)
    {
    Init(repository);
    }
    public static ISys_DeptTreeService Instance
    {
      get { return AutofacContainerModule.GetService<ISys_DeptTreeService>(); } }
    }
 }
