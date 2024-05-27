using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Newtonsoft.Json.Linq;
using iMES.Bi.API;
using iMES.Bi.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using iMES.Bi;
using iMES.Core.Controllers.Basic;
using iMES.Bi.IServices;

namespace iMES.Bi.Controllers
{
    /// <summary>
    /// 业务功能模块接口
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BllController : ApiBaseController<IBi_db_ybpService>
    {
        private IHttpContextAccessor _accessor;

        [Obsolete]
        private IHostingEnvironment hostingEnv { get; set; }
        private IContentTypeProvider contentTypeProvider { get; set; }


        Msg_Result Model = new Msg_Result() { Action = "", ErrorMsg = "" };

        [Obsolete]
        public BllController(IHttpContextAccessor accessor, IHostingEnvironment env)
        {
            _accessor = accessor;
            this.hostingEnv = env;
        }
        /// <summary>
        /// 执行业务接口
        /// </summary>
        /// <param name="Action"></param>
        /// <param name="PostData"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public ActionResult<string> ExeAction([FromBody] JObject JsonData)
        {
            var context = _accessor.HttpContext;
            string P1 = JsonData["P1"] == null ? "" : JsonData["P1"].ToString();
            string Action = Model.Action = JsonData["Action"] == null ? "" : JsonData["Action"].ToString();
            string P2 = JsonData["P2"] == null ? "" : JsonData["P2"].ToString();
            JsonData.Add("zid", context.Request.Cookies["zid"] ?? "");

            try
            {
                // 1.Load(命名空间名称)，GetType(命名空间.类名)
                Type type = Assembly.Load("iMES.Bi.API").GetType("iMES.Bi.API." + Action.Split('_')[0].ToUpper() + "Manage");
                //2.GetMethod(需要调用的方法名称)
                MethodInfo method = type.GetMethod(Action.Split('_')[1].ToUpper());
                // 3.调用的实例化方法（非静态方法）需要创建类型的一个实例
                object obj = Activator.CreateInstance(type);
                //4.方法需要传入的参数
                object[] parameters = new object[] { JsonData, Model, P1, P2 };
                method.Invoke(obj, parameters);
            }
            catch (Exception ex)
            {
                Model.ErrorMsg = "接口调用失败,请检查日志" + ex.StackTrace.ToString();
                Model.Result = ex.ToString();
            }
            return JsonNormal(Model);
        }
    }
}