/*
 *Author：COCO
 *代码由框架生成,此处任何更改都可能导致被代码生成器覆盖
 *所有业务编写全部应在Partial文件夹下Bi_db_setService与IBi_db_setService中编写
 */
using iMES.Bi.IRepositories;
using iMES.Bi.IServices;
using iMES.Core.BaseProvider;
using iMES.Core.Extensions.AutofacManager;
using iMES.Entity.DomainModels;

namespace iMES.Bi.Services
{
    public partial class Bi_db_setService : ServiceBase<Bi_db_set, IBi_db_setRepository>
    , IBi_db_setService, IDependency
    {
    public Bi_db_setService(IBi_db_setRepository repository)
    : base(repository)
    {
    Init(repository);
    }
    public static IBi_db_setService Instance
    {
      get { return AutofacContainerModule.GetService<IBi_db_setService>(); } }
    }
 }
