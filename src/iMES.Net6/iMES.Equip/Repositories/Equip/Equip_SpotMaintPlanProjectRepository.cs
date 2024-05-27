/*
 *代码由框架生成,任何更改都可能导致被代码生成器覆盖
 *Repository提供数据库操作，如果要增加数据库操作请在当前目录下Partial文件夹Equip_SpotMaintPlanProjectRepository编写代码
 */
using iMES.Equip.IRepositories;
using iMES.Core.BaseProvider;
using iMES.Core.EFDbContext;
using iMES.Core.Extensions.AutofacManager;
using iMES.Entity.DomainModels;

namespace iMES.Equip.Repositories
{
    public partial class Equip_SpotMaintPlanProjectRepository : RepositoryBase<Equip_SpotMaintPlanProject> , IEquip_SpotMaintPlanProjectRepository
    {
    public Equip_SpotMaintPlanProjectRepository(SysDbContext dbContext)
    : base(dbContext)
    {

    }
    public static IEquip_SpotMaintPlanProjectRepository Instance
    {
      get {  return AutofacContainerModule.GetService<IEquip_SpotMaintPlanProjectRepository>(); } }
    }
}
