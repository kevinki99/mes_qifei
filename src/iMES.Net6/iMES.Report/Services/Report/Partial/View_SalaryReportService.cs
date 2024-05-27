/*
 *所有关于View_SalaryReport类的业务代码应在此处编写
*可使用repository.调用常用方法，获取EF/Dapper等信息
*如果需要事务请使用repository.DbContextBeginTransaction
*也可使用DBServerProvider.手动获取数据库相关信息
*用户信息、权限、角色等使用UserContext.Current操作
*View_SalaryReportService对增、删、改查、导入、导出、审核业务代码扩展参照ServiceFunFilter
*/
using iMES.Core.BaseProvider;
using iMES.Core.Extensions.AutofacManager;
using iMES.Entity.DomainModels;
using System.Linq;
using iMES.Core.Utilities;
using System.Linq.Expressions;
using iMES.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using iMES.Report.IRepositories;
using System.Collections.Generic;

namespace iMES.Report.Services
{
    public partial class View_SalaryReportService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IView_SalaryReportRepository _repository;//访问数据库

        [ActivatorUtilitiesConstructor]
        public View_SalaryReportService(
            IView_SalaryReportRepository dbRepository,
            IHttpContextAccessor httpContextAccessor
            )
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            //多租户会用到这init代码，其他情况可以不用
            //base.Init(dbRepository);
        }
        public override PageGridData<View_SalaryReport> GetPageData(PageDataOptions options)
        {
            //查询table界面显示求和
            SummaryExpress = (IQueryable<View_SalaryReport> queryable) =>
            {
                return queryable.GroupBy(x => 1).Select(x => new
                {
                    ReportAll = x.Sum(o => o.ReportAll).ToString("f2"),
                    Salary = x.Sum(o => o.Salary).ToString("f2"),
                    AlreadyAppNumber = x.Sum(o => o.AlreadyAppNumber).ToString("f2"),
                })
                .FirstOrDefault();
            };
            return base.GetPageData(options);
        }
    }
}
