/*
 *接口编写处...
*如果接口需要做Action的权限验证，请在Action上使用属性
*如: [ApiActionPermission("Bi_db_source",Enums.ActionPermissionOptions.Search)]
 */
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using iMES.Entity.DomainModels;
using iMES.Bi.IServices;
using Newtonsoft.Json.Linq;
using System.Reflection;
using Newtonsoft.Json;

namespace iMES.Bi.Controllers
{
    public partial class Bi_db_sourceController
    {
        private readonly IBi_db_sourceService _service;//访问业务代码
        private readonly IHttpContextAccessor _httpContextAccessor;

        [ActivatorUtilitiesConstructor]
        public Bi_db_sourceController(
            IBi_db_sourceService service,
            IHttpContextAccessor httpContextAccessor
        )
        : base(service)
        {
            _service = service;
            _httpContextAccessor = httpContextAccessor;
        }
        ///// <summary>
        ///// 执行业务接口
        ///// </summary>
        ///// <param name="Action"></param>
        ///// <param name="PostData"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public ActionResult<string> ExeAction([FromBody] JObject JsonData)
        //{
        //    string P1 = JsonData["P1"] == null ? "" : JsonData["P1"].ToString();
        //    string Action = JsonData["Action"] == null ? "" : JsonData["Action"].ToString();
        //    string P2 = JsonData["P2"] == null ? "" : JsonData["P2"].ToString();
        //    try
        //    {
        //        // 1.Load(命名空间名称)，GetType(命名空间.类名)
        //        Type type = Assembly.Load("iMES.Bi.API").GetType("iMES.Bi.API." + Action.Split('_')[0].ToUpper() + "Manage");
        //        //2.GetMethod(需要调用的方法名称)
        //        MethodInfo method = type.GetMethod(Action.Split('_')[1].ToUpper());
        //        // 3.调用的实例化方法（非静态方法）需要创建类型的一个实例
        //        object obj = Activator.CreateInstance(type);
        //        //4.方法需要传入的参数
        //        object[] parameters = new object[] { JsonData, Model, P1, P2 };
        //        method.Invoke(obj, parameters);


        //            var tt = JsonConvert.DeserializeObject<Bi_db_source>(P1);
        //            var db = new DBFactory(tt.DBType, tt.DBIP, tt.Port, tt.DBName, tt.DBUser, tt.DBPwd);
        //            if (db.TestConn())
        //            {
        //                msg.Result = "1"; //1：代表连接成功
        //            }
        //            else
        //            {
        //                msg.ErrorMsg = "连接失败";
        //            }
        //    }
        //    catch (Exception ex)
        //    {
        //        Model.ErrorMsg = "接口调用失败,请检查日志" + ex.StackTrace.ToString();
        //        Model.Result = ex.ToString();
        //    }
        //    return JsonNormal(Model);
        //}
    }
}
