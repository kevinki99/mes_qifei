/*
 *Author：COCO
 *代码由框架生成,此处任何更改都可能导致被代码生成器覆盖
 *所有业务编写全部应在Partial文件夹下Bi_db_ybpService与IBi_db_ybpService中编写
 */
using iMES.Bi.IRepositories;
using iMES.Bi.IServices;
using iMES.Core.BaseProvider;
using iMES.Core.Extensions.AutofacManager;
using iMES.Entity.DomainModels;

namespace iMES.Bi.Services
{
    public partial class Bi_db_ybpService : ServiceBase<Bi_db_ybp, IBi_db_ybpRepository>
    , IBi_db_ybpService, IDependency
    {
    public Bi_db_ybpService(IBi_db_ybpRepository repository)
    : base(repository)
    {
    Init(repository);
    }
    public static IBi_db_ybpService Instance
    {
      get { return AutofacContainerModule.GetService<IBi_db_ybpService>(); } }
    }
 }
