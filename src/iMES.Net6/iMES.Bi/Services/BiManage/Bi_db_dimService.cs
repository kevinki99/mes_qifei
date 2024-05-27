/*
 *Author：COCO
 *代码由框架生成,此处任何更改都可能导致被代码生成器覆盖
 *所有业务编写全部应在Partial文件夹下Bi_db_dimService与IBi_db_dimService中编写
 */
using iMES.Bi.IRepositories;
using iMES.Bi.IServices;
using iMES.Core.BaseProvider;
using iMES.Core.Extensions.AutofacManager;
using iMES.Entity.DomainModels;

namespace iMES.Bi.Services
{
    public partial class Bi_db_dimService : ServiceBase<Bi_db_dim, IBi_db_dimRepository>
    , IBi_db_dimService, IDependency
    {
    public Bi_db_dimService(IBi_db_dimRepository repository)
    : base(repository)
    {
    Init(repository);
    }
    public static IBi_db_dimService Instance
    {
      get { return AutofacContainerModule.GetService<IBi_db_dimService>(); } }
    }
 }
