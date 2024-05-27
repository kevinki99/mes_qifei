/*
 *代码由框架生成,任何更改都可能导致被代码生成器覆盖
 *Repository提供数据库操作，如果要增加数据库操作请在当前目录下Partial文件夹Bi_db_setRepository编写代码
 */
using iMES.Bi.IRepositories;
using iMES.Core.BaseProvider;
using iMES.Core.EFDbContext;
using iMES.Core.Extensions.AutofacManager;
using iMES.Entity.DomainModels;

namespace iMES.Bi.Repositories
{
    public partial class Bi_db_setRepository : RepositoryBase<Bi_db_set> , IBi_db_setRepository
    {
    public Bi_db_setRepository(SysDbContext dbContext)
    : base(dbContext)
    {

    }
    public static IBi_db_setRepository Instance
    {
      get {  return AutofacContainerModule.GetService<IBi_db_setRepository>(); } }
    }
}
