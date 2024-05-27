/*
 *代码由框架生成,任何更改都可能导致被代码生成器覆盖
 *Repository提供数据库操作，如果要增加数据库操作请在当前目录下Partial文件夹Bi_desktopRepository编写代码
 */
using iMES.Bi.IRepositories;
using iMES.Core.BaseProvider;
using iMES.Core.EFDbContext;
using iMES.Core.Extensions.AutofacManager;
using iMES.Entity.DomainModels;

namespace iMES.Bi.Repositories
{
    public partial class Bi_desktopRepository : RepositoryBase<Bi_desktop> , IBi_desktopRepository
    {
    public Bi_desktopRepository(SysDbContext dbContext)
    : base(dbContext)
    {

    }
    public static IBi_desktopRepository Instance
    {
      get {  return AutofacContainerModule.GetService<IBi_desktopRepository>(); } }
    }
}
