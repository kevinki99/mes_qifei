/*
 *Author：COCO
 *代码由框架生成,此处任何更改都可能导致被代码生成器覆盖
 *所有业务编写全部应在Partial文件夹下Base_PrintCatalogService与IBase_PrintCatalogService中编写
 */
using iMES.Custom.IRepositories;
using iMES.Custom.IServices;
using iMES.Core.BaseProvider;
using iMES.Core.Extensions.AutofacManager;
using iMES.Entity.DomainModels;

namespace iMES.Custom.Services
{
    public partial class Base_PrintCatalogService : ServiceBase<Base_PrintCatalog, IBase_PrintCatalogRepository>
    , IBase_PrintCatalogService, IDependency
    {
    public Base_PrintCatalogService(IBase_PrintCatalogRepository repository)
    : base(repository)
    {
    Init(repository);
    }
    public static IBase_PrintCatalogService Instance
    {
      get { return AutofacContainerModule.GetService<IBase_PrintCatalogService>(); } }
    }
 }
