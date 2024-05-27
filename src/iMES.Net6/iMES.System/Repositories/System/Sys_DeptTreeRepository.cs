/*
 *代码由框架生成,任何更改都可能导致被代码生成器覆盖
 *Repository提供数据库操作，如果要增加数据库操作请在当前目录下Partial文件夹Sys_DeptTreeRepository编写代码
 */
using iMES.System.IRepositories;
using iMES.Core.BaseProvider;
using iMES.Core.EFDbContext;
using iMES.Core.Extensions.AutofacManager;
using iMES.Entity.DomainModels;

namespace iMES.System.Repositories
{
    public partial class Sys_DeptTreeRepository : RepositoryBase<Sys_DeptTree> , ISys_DeptTreeRepository
    {
    public Sys_DeptTreeRepository(SysDbContext dbContext)
    : base(dbContext)
    {

    }
    public static ISys_DeptTreeRepository Instance
    {
      get {  return AutofacContainerModule.GetService<ISys_DeptTreeRepository>(); } }
    }
}
