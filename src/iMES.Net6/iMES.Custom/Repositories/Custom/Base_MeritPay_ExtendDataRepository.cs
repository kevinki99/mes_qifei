/*
 *代码由框架生成,任何更改都可能导致被代码生成器覆盖
 *Repository提供数据库操作，如果要增加数据库操作请在当前目录下Partial文件夹Base_MeritPay_ExtendDataRepository编写代码
 */
using iMES.Custom.IRepositories;
using iMES.Core.BaseProvider;
using iMES.Core.EFDbContext;
using iMES.Core.Extensions.AutofacManager;
using iMES.Entity.DomainModels;

namespace iMES.Custom.Repositories
{
    public partial class Base_MeritPay_ExtendDataRepository : RepositoryBase<Base_MeritPay_ExtendData> , IBase_MeritPay_ExtendDataRepository
    {
    public Base_MeritPay_ExtendDataRepository(SysDbContext dbContext)
    : base(dbContext)
    {

    }
    public static IBase_MeritPay_ExtendDataRepository Instance
    {
      get {  return AutofacContainerModule.GetService<IBase_MeritPay_ExtendDataRepository>(); } }
    }
}
