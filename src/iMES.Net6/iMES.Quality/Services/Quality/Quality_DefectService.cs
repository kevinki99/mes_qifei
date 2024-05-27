/*
 *Author：COCO
 *代码由框架生成,此处任何更改都可能导致被代码生成器覆盖
 *所有业务编写全部应在Partial文件夹下Quality_DefectService与IQuality_DefectService中编写
 */
using iMES.Quality.IRepositories;
using iMES.Quality.IServices;
using iMES.Core.BaseProvider;
using iMES.Core.Extensions.AutofacManager;
using iMES.Entity.DomainModels;

namespace iMES.Quality.Services
{
    public partial class Quality_DefectService : ServiceBase<Quality_Defect, IQuality_DefectRepository>
    , IQuality_DefectService, IDependency
    {
    public Quality_DefectService(IQuality_DefectRepository repository)
    : base(repository)
    {
    Init(repository);
    }
    public static IQuality_DefectService Instance
    {
      get { return AutofacContainerModule.GetService<IQuality_DefectService>(); } }
    }
 }
