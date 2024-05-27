/*
 *Author：COCO
 *代码由框架生成,此处任何更改都可能导致被代码生成器覆盖
 *所有业务编写全部应在Partial文件夹下Bi_desktopService与IBi_desktopService中编写
 */
using iMES.Bi.IRepositories;
using iMES.Bi.IServices;
using iMES.Core.BaseProvider;
using iMES.Core.Extensions.AutofacManager;
using iMES.Entity.DomainModels;

namespace iMES.Bi.Services
{
    public partial class Bi_desktopService : ServiceBase<Bi_desktop, IBi_desktopRepository>
    , IBi_desktopService, IDependency
    {
    public Bi_desktopService(IBi_desktopRepository repository)
    : base(repository)
    {
    Init(repository);
    }
    public static IBi_desktopService Instance
    {
      get { return AutofacContainerModule.GetService<IBi_desktopService>(); } }
    }
 }
