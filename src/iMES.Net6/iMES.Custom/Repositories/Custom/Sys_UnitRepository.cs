/*
 *代码由框架生成,任何更改都可能导致被代码生成器覆盖
 *Repository提供数据库操作，如果要增加数据库操作请在当前目录下Partial文件夹Sys_UnitRepository编写代码
 */
using iMES.Custom.IRepositories;
using iMES.Core.BaseProvider;
using iMES.Core.EFDbContext;
using iMES.Core.Extensions.AutofacManager;
using iMES.Entity.DomainModels;

namespace iMES.Custom.Repositories
{
    public partial class Sys_UnitRepository : RepositoryBase<Sys_Unit> , ISys_UnitRepository
    {
    public Sys_UnitRepository(SysDbContext dbContext)
    : base(dbContext)
    {

    }
    public static ISys_UnitRepository Instance
    {
      get {  return AutofacContainerModule.GetService<ISys_UnitRepository>(); } }
    }
}