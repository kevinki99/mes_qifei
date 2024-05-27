/*
 *所有关于Bi_db_ybp类的业务代码应在此处编写
*可使用repository.调用常用方法，获取EF/Dapper等信息
*如果需要事务请使用repository.DbContextBeginTransaction
*也可使用DBServerProvider.手动获取数据库相关信息
*用户信息、权限、角色等使用UserContext.Current操作
*Bi_db_ybpService对增、删、改查、导入、导出、审核业务代码扩展参照ServiceFunFilter
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
using iMES.Bi.IRepositories;
using System;
using iMES.Core.ManageUser;

namespace iMES.Bi.Services
{
    public partial class Bi_db_ybpService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBi_db_ybpRepository _repository;//访问数据库

        [ActivatorUtilitiesConstructor]
        public Bi_db_ybpService(
            IBi_db_ybpRepository dbRepository,
            IHttpContextAccessor httpContextAccessor
            )
        : base(dbRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _repository = dbRepository;
            //多租户会用到这init代码，其他情况可以不用
            //base.Init(dbRepository);
        }
        WebResponseContent webResponse = new WebResponseContent();
        /// <summary>
        /// 新建
        /// </summary>
        /// <param name="saveDataModel"></param>
        /// <returns></returns>
        public override WebResponseContent Add(SaveModel saveDataModel)
        {
            //此处saveModel是从前台提交的原生数据，可对数据进修改过滤
            AddOnExecuting = (Bi_db_ybp ybp, object list) =>
            {
                ybp.CRDate = DateTime.Now;
                ybp.CRUser = UserContext.Current.UserName;
                return webResponse.OK();
            };
            return base.Add(saveDataModel);
        }
    }
}
