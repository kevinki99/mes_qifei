using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using iMES.Bi.Data;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using iMES.Core.ManageUser;

namespace iMES.Bi.API
{
    public class DATABIManage
    {
        #region DataSet

        //获取数据集
        public void GETBIDBSETLIST(JObject context, Msg_Result msg, string P1, string P2)
        {
            string userName = UserContext.Current.UserName;
            string strWhere = " 1=1 ";

            int page = 0;
            int pagecount = 8;
            int.TryParse(context.Request("p") ?? "1", out page);
            int.TryParse(context.Request("pagecount") ?? "80", out pagecount);//页数
            page = page == 0 ? 1 : page;
            int total = 0;
            DataTable dt = new BI_DB_SetB().GetDataPager(" BI_DB_Set T left join BI_DB_Source S on T.SID=S.ID ", " T.*, ISNULL(S.Name,'本地数据源')  AS  SJY,  ISNULL(S.DBType,'本地数据源')  AS DBType  ", pagecount, page, " CRDate desc ", strWhere, ref total);


            msg.Result = dt;
            msg.Result1 = total;
            msg.Result2 = new BI_DB_SourceB().GetALLEntities();

        }

        //添加修改数据集
        public void ADDBIDBSET(JObject context, Msg_Result msg, string P1, string P2)
        {
            var tt = JsonConvert.DeserializeObject<BI_DB_Set>(P1);
            if (tt.ID == 0)
            {
                tt.CRUser = UserContext.Current.UserName;
                tt.CRDate = DateTime.Now;
                new BI_DB_SetB().Insert(tt);
            }
            else
            {
                tt.UPDate = DateTime.Now;
                new BI_DB_SetB().Update(tt);
            }


            msg.Result = tt;

        }

        //删除数据集
        public void DELBIDBSET(JObject context, Msg_Result msg, string P1, string P2)
        {
            try
            {
                int ID = int.Parse(P1);
                new BI_DB_SetB().Delete(d => d.ID == ID);
                new BI_DB_DimB().Delete(d => d.STID == ID);
            }
            catch (Exception ex)
            {
                msg.ErrorMsg = ex.Message;
            }

        }


        public void GETBIDBSET(JObject context, Msg_Result msg, string P1, string P2)
        {
            try
            {
                int ID = int.Parse(P1);
                msg.Result = new BI_DB_SetB().GetEntity(d => d.ID == ID);
                msg.Result1 = new BI_DB_DimB().GetEntities(d => d.STID == ID && d.Dimension == "1");
                msg.Result2 = new BI_DB_DimB().GetEntities(d => d.STID == ID && d.Dimension == "2");

            }
            catch (Exception ex)
            {
                msg.ErrorMsg = ex.Message;
            }

        }

        public void UPBIDSET(JObject context, Msg_Result msg, string P1, string P2)
        {
            try
            {
                string WD = context["WD"] == null ? "" : context["WD"].ToString();
                string DL = context["DL"] == null ? "" : context["DL"].ToString();
                var tt = JsonConvert.DeserializeObject<BI_DB_Set>(P1);
                tt.UPDate = DateTime.Now;
                new BI_DB_SetB().Update(tt);

                List<BI_DB_Dim> ListWD = JsonConvert.DeserializeObject<List<BI_DB_Dim>>(WD);
                List<BI_DB_Dim> ListDL = JsonConvert.DeserializeObject<List<BI_DB_Dim>>(DL);
                new BI_DB_DimB().Delete(D => D.STID == tt.ID);

                ListWD.ForEach(D => D.CRDate = DateTime.Now);
                ListWD.ForEach(D => D.ColumnSource = "0");
                ListWD.ForEach(D => D.CRUser = UserContext.Current.UserName);

                ListDL.ForEach(D => D.CRDate = DateTime.Now);
                ListDL.ForEach(D => D.ColumnSource = "0");
                ListDL.ForEach(D => D.CRUser = UserContext.Current.UserName);

                new BI_DB_DimB().Insert(ListWD);
                new BI_DB_DimB().Insert(ListDL);


            }
            catch (Exception ex)
            {
                msg.ErrorMsg = ex.Message;
            }

        }


        /// <summary>
        /// 仪表盘页面使用数据集数据
        /// </summary>
        /// <param name="context"></param>
        /// <param name="msg"></param>
        /// <param name="P1"></param>
        /// <param name="P2"></param>
        /// <param name="UserInfo"></param>
        public void GETYBDATASET(JObject context, Msg_Result msg, string P1, string P2)
        {
            try
            {
                DataTable dt = new BI_DB_SetB().GetDTByCommand("select ID,Name AS DName,Name AS DValue,'数据集' AS dtype  from BI_DB_Set UNION ALL select  ID,TableDesc  AS DName, TableName  AS DValue, '数据表' AS dtype  from bi_db_table ");
                dt.Columns.Add("wd", Type.GetType("System.Object"));
                dt.Columns.Add("dl", Type.GetType("System.Object"));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["dtype"].ToString() == "数据集")
                    {
                        dt.Rows[i]["wd"] = new BI_DB_SetB().GetDTByCommand("select Name,ColumnName,ColumnType,Dimension from BI_DB_Dim WHERE STID='" + dt.Rows[i]["ID"].ToString() + "' AND Dimension='1' AND ColumnSource='0' ORDER BY  ColumnName");
                        dt.Rows[i]["dl"] = new BI_DB_SetB().GetDTByCommand("select Name,ColumnName,ColumnType,Dimension from BI_DB_Dim WHERE STID='" + dt.Rows[i]["ID"].ToString() + "' AND Dimension='2' AND ColumnSource='0' ORDER BY  ColumnName");
                    }
                    else
                    {
                        DataTable dtWD = new BI_DB_DimB().GetDTByCommand("select DbColumnName AS ColumnName, ISNULL(ColumnDescription, DbColumnName) AS Name ,DataType AS ColumnType,'1' AS Dimension FROM bi_db_tablefiled WHERE  DATATYPE not IN ('int','decimal','float','Double') AND tableid='" + dt.Rows[i]["ID"].ToString() + "'");
                        DataTable dtDL = new BI_DB_DimB().GetDTByCommand("select DbColumnName AS ColumnName, ISNULL(ColumnDescription, DbColumnName) AS Name,DataType AS ColumnType,'2' AS Dimension FROM bi_db_tablefiled WHERE  DATATYPE IN ('int','decimal','float','Double') AND tableid='" + dt.Rows[i]["ID"].ToString() + "' ");

                        for (int m = 0; m < dtWD.Rows.Count; m++)
                        {
                            dtWD.Rows[m]["ColumnType"] = getfiletype(dtWD.Rows[m]["ColumnType"].ToString());
                            if (dtWD.Rows[m]["Name"].ToString() == "")
                            {
                                dtWD.Rows[m]["Name"] = dtWD.Rows[m]["ColumnName"].ToString();
                            }
                        }
                        for (int m = 0; m < dtDL.Rows.Count; m++)
                        {
                            dtDL.Rows[m]["ColumnType"] = getfiletype(dtDL.Rows[m]["ColumnType"].ToString());
                            if (dtDL.Rows[m]["Name"].ToString() == "")
                            {
                                dtDL.Rows[m]["Name"] = dtDL.Rows[m]["ColumnName"].ToString();
                            }
                        }
                        dt.Rows[i]["wd"] = dtWD;
                        dt.Rows[i]["dl"] = dtDL;

                    }
                }
                msg.Result = dt;
            }
            catch (Exception ex)
            {
                msg.ErrorMsg = ex.Message;
            }

        }



        public void JXSQL(JObject context, Msg_Result msg, string P1, string P2)
        {
            try
            {
                int ID = int.Parse(P1);
                BI_DB_Set DS = new BI_DB_SetB().GetEntity(d => d.ID == ID);
                DBFactory db = new BI_DB_SourceB().GetDB(DS.SID.Value);
                DataTable dt = new DataTable();
                dt = db.GetSQL(CommonHelp.Filter(P2));
                List<BI_DB_Dim> ListDIM = new BI_DB_SetB().getCType(dt);
                ListDIM.ForEach(D => D.STID = ID);
                msg.Result = ListDIM.Where(D => D.Dimension == "1");
                msg.Result1 = ListDIM.Where(D => D.Dimension == "2");

            }
            catch (Exception ex)
            {
                msg.ErrorMsg = ex.Message;
            }

        }




        /// <summary>
        /// 获取数据集数据,可作为外部接口使用
        /// </summary>
        /// <param name="context"></param>
        /// <param name="msg"></param>
        /// <param name="P1"></param>
        /// <param name="P2"></param>
        /// <param name="UserInfo"></param>
        public void GETDATASETDATA(JObject context, Msg_Result msg, string P1, string P2)
        {
            msg.DataLength = 0;
            BI_DB_Set DS = new BI_DB_SetB().GetEntities(d => d.Name == P1).FirstOrDefault();
            if (DS != null)
            {
                JObject wigdata = JObject.Parse(P2);
                string strWhere = " 1=1 ";
                DBFactory db = new BI_DB_SourceB().GetDB(DS.SID.Value);


                JObject orderdata = (JObject)wigdata["dataorder"];

                string ordersql = "";
                string strPageCount = (string)wigdata["pagecount"] ?? "0";
                int pageNo = int.Parse((string)wigdata["pageNo"] ?? "1");
                int pageSize = int.Parse((string)wigdata["pageSize"] ?? "0");
                JArray wdlist = (JArray)wigdata["wdlist"];
                JArray dllist = (JArray)wigdata["dllist"];




                string strWD = "";
                string strWDGroup = "";//处理MYSQL GROUP别名问题
                foreach (JObject item in wdlist)
                {
                    string strWDColumCode = (string)item["colid"];

                    string strTempGroup = strWDColumCode;
                    strWD = strWD + strWDColumCode + ",";
                    strWDGroup = strWDGroup + strTempGroup + ",";


                    //获取维度字段筛选条件
                    JArray querylist = (JArray)item["querydata"];
                    foreach (JObject queryitem in querylist)
                    {
                        string strcal = (string)queryitem["cal"];
                        string strglval = (string)queryitem["glval"];
                        if (!string.IsNullOrEmpty(strglval))
                        {
                            switch (strcal)
                            {
                                case "0": strWhere += " and " + strTempGroup + " ='" + strglval + "'"; break;
                                case "1": strWhere += " and " + strTempGroup + " <'" + strglval + "'"; break;
                                case "2": strWhere += " and " + strTempGroup + " >'" + strglval + "'"; break;
                                case "3": strWhere += " and " + strTempGroup + " !='" + strglval + "'"; break;
                                case "4": strWhere += " and " + strWDColumCode + "  LIKE  '%" + strglval.ToFormatLike() + "%'"; break;
                                case "5": strWhere += " and " + strWDColumCode + "  BETWEEN '" + strglval.Split(',')[0] + " 00:00:00' AND '" + strglval.Split(',')[1] + " 00:00:00 '"; break;
                                case "6": strWhere += " and " + strWDColumCode + "  IN  ('" + strglval.ToFormatLike() + "')"; break;


                            }
                        }
                    }
                    //处理排序
                    if (orderdata["order"] != null && (string)orderdata["order"].ToString() != "")
                    {
                        ordersql = strWDColumCode + " " + (string)orderdata["order"].ToString();
                    }

                }
                strWD = strWD.TrimEnd(',');
                strWDGroup = strWDGroup.TrimEnd(',');

                string strDL = "";
                string strHaving = "HAVING";
                foreach (JObject item in dllist)
                {
                    strDL = strDL + " " + (string)item["caltype"] + "(" + (string)item["colid"] + ") AS " + (string)item["colid"] + ",";
                    string strTJFiled = (string)item["caltype"] + "(" + (string)item["colid"] + ")  ";
                    //获取统计字段筛选条件
                    JArray querylist = (JArray)item["querydata"];
                    foreach (JObject queryitem in querylist)
                    {
                        string strcal = (string)queryitem["cal"];
                        string strglval = (string)queryitem["glval"];
                        if (!string.IsNullOrEmpty(strglval))
                        {
                            string strPre = strHaving == "HAVING" ? " " : " and ";
                            switch (strcal)
                            {
                                case "0":
                                    strHaving += strPre + strTJFiled + " ='" + strglval + "'";
                                    break;
                                case "1":
                                    strHaving += strPre + strTJFiled + " <'" + strglval + "'";
                                    break;
                                case "2":
                                    strHaving += strPre + strTJFiled + " >'" + strglval + "'";
                                    break;
                                case "3":
                                    strHaving += strPre + strTJFiled + " !='" + strglval + "'";
                                    break;
                                case "4":
                                    strHaving += strPre + strTJFiled + "  LIKE  '%" + strglval.ToFormatLike() + "%'";
                                    break;
                                case "6":
                                    strHaving += strPre + strTJFiled + "  IN  ('" + strglval.ToFormatLike() + "')";
                                    break;
                            }
                        }


                    }
                    //处理组件筛选
                    //string bindwig = (string)item["bindwig"];
                    //getqwig(strquerydata, strTJFiled, bindwig, ref strHaving);

                    //处理排序
                    if ((string)orderdata["prop"].ToString() == (string)item["colid"])
                    {
                        ordersql = strTJFiled + " " + (string)orderdata["order"].ToString();
                    }

                }
                strDL = strDL.TrimEnd(',');
                strHaving = strHaving == "HAVING" ? "" : strHaving;
                strHaving = strHaving.Replace("HAVING", "");


                int recordTotal = 0;
                string strRSQL = "";
                DataTable dt = db.GetYBData(DS.DSQL, strWD, strDL, strPageCount, strWhere, ordersql, pageNo, pageSize, strWDGroup, strHaving, ref recordTotal, ref strRSQL);
                if (dt.Rows.Count > 8000)
                {
                    //msg.ErrorMsg = "返回数据量太大,超过8000,服务器只返回前8000条数据";
                    dt = dt.SplitDataTable(1, 8000);
                }
                msg.Result = dt;
                msg.DataLength = recordTotal;

            }
        }



        #endregion

        #region DataSource
        //测试数据源连接
        public void TESTBIDBSOURCE(JObject context, Msg_Result msg, string P1, string P2)
        {
            var tt = JsonConvert.DeserializeObject<BI_DB_Source>(P1);
            var db = new DBFactory(tt.DBType, tt.DBIP, tt.Port, tt.DBName, tt.DBUser, tt.DBPwd);
            if (db.TestConn())
            {
                msg.Result = "1"; //1：代表连接成功
            }
            else
            {
                msg.ErrorMsg = "连接失败";
            }

        }

        //获取数据源
        public void GETBIDBSOURCELIST(JObject context, Msg_Result msg, string P1, string P2)
        {
            string userName = UserContext.Current.UserName;
            string strWhere = "1=1";
            int page = 0;
            int pagecount = 8;
            int.TryParse(context.Request("p") ?? "1", out page);
            int.TryParse(context.Request("pagecount") ?? "8", out pagecount);//页数
            page = page == 0 ? 1 : page;
            int total = 0;
            List<BI_DB_Source> dt = new BI_DB_SourceB().Db.Queryable<BI_DB_Source>().Where(strWhere).OrderBy(it => it.CRDate, OrderByType.Desc).ToPageList(page, pagecount, ref total);


            //foreach (var tt in dt)
            //{
            //    var db = new DBFactory(tt.DBType, tt.DBIP, tt.Port, tt.DBName, tt.DBUser, tt.DBPwd);
            //    tt.Attach = "1";//不可用
            //}

            msg.Result = dt;
            msg.Result1 = total;
        }

        //添加修改数据源
        public void ADDBIDBSOURCE(JObject context, Msg_Result msg, string P1, string P2)
        {
            var tt = JsonConvert.DeserializeObject<BI_DB_Source>(P1);
            if (tt.ID == 0)
            {
                tt.CRUser = UserContext.Current.UserName;
                tt.CRDate = DateTime.Now;
                new BI_DB_SourceB().Insert(tt);
            }
            else
            {
                new BI_DB_SourceB().Update(tt);
            }
            msg.Result = tt;

        }

        //删除数据源
        public void DELBIDBSOURCE(JObject context, Msg_Result msg, string P1, string P2)
        {
            try
            {
                int ID = int.Parse(P1);
                new BI_DB_SourceB().Delete(d => d.ID == ID);
                new BI_DB_SetB().Delete(d => d.SID == ID);

            }
            catch (Exception ex)
            {
                msg.ErrorMsg = ex.Message;
            }

        }


        //获取数据集表名和视图名
        public void GETBIDBSOURCEVIEWLIST(JObject context, Msg_Result msg, string P1, string P2)
        {
            int ID = Int32.Parse(P1);
            DBFactory db = new BI_DB_SourceB().GetDB(ID);
            msg.Result = db.GetDBTables();

        }


        /// <summary>
        /// 生成数据集
        /// </summary>
        /// <param name="context"></param>
        /// <param name="msg"></param>
        /// <param name="P1"></param>
        /// <param name="P2"></param>
        /// <param name="UserInfo"></param>

        public void ADDBISETLIST(JObject context, Msg_Result msg, string P1, string P2)
        {
            int ID = Int32.Parse(P1);
            DBFactory db = new BI_DB_SourceB().GetDB(ID);
            string strTableName = P2;
            string strDataSetName = context.Request("DsetName") ?? "1";





            BI_DB_Set DS = new BI_DB_Set();
            DS.Name = strDataSetName;
            DS.SID = ID;
            DS.SName = strTableName;
            DS.CRDate = DateTime.Now;
            DS.CRUser = UserContext.Current.UserName;
            DS.Type = "SQL";
            DS.DSQL = "SELECT  * FROM " + strTableName;
            new BI_DB_SetB().Insert(DS);




            DataTable dt = db.GetDBClient().SqlQueryable<Object>(CommonHelp.Filter("SELECT  * FROM " + strTableName)).ToDataTablePage(1, 1);

            List<BI_DB_Dim> ListDIM = new BI_DB_SetB().getCType(dt);
            ListDIM.ForEach(D => D.STID = DS.ID);
            ListDIM.ForEach(D => D.CRDate = DateTime.Now);
            ListDIM.ForEach(D => D.CRUser = UserContext.Current.UserName);

            new BI_DB_DimB().Insert(ListDIM);




        }
        #endregion


        #region YBP
        public void GETYBLISTDATA(JObject context, Msg_Result msg, string P1, string P2)
        {
            msg.Result = new BI_DB_YBPB().GetALLEntities();

        }


        public void SAVEDATA(JObject context, Msg_Result msg, string P1, string P2)
        {
            BI_DB_YBP model = new BI_DB_YBP();
            model.Name = P1;
            model.YBType = P2;
            model.DimID = int.Parse(context.Request("dim") ?? "0");
            //model.CRUser = UserContext.Current.UserName;
            model.CRDate = DateTime.Now;
            model.Remark = new CommonHelp().GenerateCheckCode(12);
            new BI_DB_YBPB().Insert(model);
            msg.Result = model;
        }

        public void UPYBDATA(JObject context, Msg_Result msg, string P1, string P2)
        {

            string strFormName = context["FormName"] == null ? "" : context["FormName"].ToString();
            string strFB = context["ISFB"] == null ? "N" : context["ISFB"].ToString();



            int ID = Int32.Parse(P1);
            BI_DB_YBP model = new BI_DB_YBPB().GetEntities(d => d.ID == ID).FirstOrDefault();
            model.YBContent = P2;
            if (strFormName != "")
            {
                model.Name = strFormName;
            }
            if (strFB == "Y")
            {
                model.YBOption = P2;
            }
            if (string.IsNullOrEmpty(model.Remark))
            {
                model.Remark = new CommonHelp().GenerateCheckCode(12);
            }
            new BI_DB_YBPB().Update(model);
            msg.Result = model;
        }



        public void DELYBDATA(JObject context, Msg_Result msg, string P1, string P2)
        {
            int ID = Int32.Parse(P1);
            new BI_DB_YBPB().Delete(D => D.ID == ID);
            new BI_DB_DimB().Delete(D => D.STID == ID);
        }


        public void GETYBBYID(JObject context, Msg_Result msg, string P1, string P2)
        {
            int ID = Int32.Parse(P1);
            BI_DB_YBP model = new BI_DB_YBPB().GetEntities(d => d.ID == ID).FirstOrDefault();
            msg.Result = model;
        }


        /// <summary>
        /// 获取仪表盘数据接口
        /// </summary>
        /// <param name="context"></param>
        /// <param name="msg"></param>
        /// <param name="P1"></param>
        /// <param name="P2"></param>
        /// <param name="UserInfo"></param>
        public void GETYBDATA(JObject context, Msg_Result msg, string P1, string P2)
        {
            try
            {
                msg.DataLength = 0;
                JObject wigdata = JObject.Parse(P1);



                string datatype = (string)wigdata["datatype"];//数据来源类型0:SQL,1:API
                if (datatype == "0")//SQL取数据
                {
                    string strWigdetType = (string)wigdata["wigdetype"];
                    string strDateSetName = (string)wigdata["datasetname"];
                    int sid = 0;
                    string dsql = "";
                    if (!strDateSetName.Contains("qj_"))
                    {
                        //看是否包含qj_，不包含就是数据集
                        BI_DB_Set DS = new BI_DB_SetB().GetEntities(d => d.Name == strDateSetName).FirstOrDefault();
                        sid = DS.SID.Value;
                        dsql = DS.DSQL;
                    }
                    else
                    {
                        BI_DB_Table DS = new BI_DB_TableB().GetEntities(d => d.TableName == strDateSetName).FirstOrDefault();
                        sid = DS.DSID;
                        dsql = "select * from " + DS.TableName;
                    }

                    JObject orderdata = (JObject)wigdata["dataorder"];

                    string ordersql = "";
                    string strPageCount = context["pagecount"] == null ? "10" : context["pagecount"].ToString();
                    string strWhere = " 1=1 ";
                    DBFactory db = new BI_DB_SourceB().GetDB(sid);
                    if (strWigdetType == "dwig")
                    {
                        JArray wdlist = (JArray)wigdata["wdlist"];
                        JArray dllist = (JArray)wigdata["dllist"];
                        JArray filist = (JArray)wigdata["filist"];

                        string strWD = "";
                        string strWDGroup = "";//处理MYSQL GROUP别名问题
                        foreach (JObject item in filist)
                        {
                            string strWDType = (string)item["coltype"];
                            string strWDColumCode = (string)item["colid"];

                            //获取维度字段筛选条件
                            JArray querylist = (JArray)item["querydata"];
                            foreach (JObject queryitem in querylist)
                            {
                                string strcal = (string)queryitem["cal"];
                                string strglval = (string)queryitem["glval"];
                                if (!string.IsNullOrEmpty(strglval))
                                {
                                    switch (strcal)
                                    {
                                        case "0": strWhere += " and " + strWDColumCode + " ='" + strglval + "'"; break;
                                        case "1": strWhere += " and " + strWDColumCode + " <'" + strglval + "'"; break;
                                        case "2": strWhere += " and " + strWDColumCode + " >'" + strglval + "'"; break;
                                        case "3": strWhere += " and " + strWDColumCode + " !='" + strglval + "'"; break;
                                        case "4": strWhere += " and " + strWDColumCode + "  LIKE  '%" + strglval.ToFormatLike() + "%'"; break;
                                        case "5": strWhere += " and " + strWDColumCode + "  BETWEEN '" + strglval.Split(',')[0] + " 00:00:00' AND '" + strglval.Split(',')[1] + " 00:00:00 '"; break;
                                        case "6": strWhere += " and " + strWDColumCode + "  IN  ('" + strglval.ToFormatLike() + "')"; break;


                                    }
                                }
                            }

                            //处理组件筛选
                            //string bindwig = (string)item["bindwig"];
                            //getqwig(strquerydata, strWDColumCode, bindwig, ref strWhere);

                        }
                        foreach (JObject item in wdlist)
                        {
                            string strWDType = (string)item["coltype"];
                            string strWDColumCode = (string)item["colid"];


                            string strTempGroup = strWDColumCode;

                            if (strWDType == "TA")//分析字段
                            {
                                string strFiled = strWDColumCode.Split('_')[0];
                                string strForMat = strWDColumCode.Split('_')[1];

                                strWDColumCode = SqlHelp.TADate(strWDColumCode, db.GetDBType().ToUpper(), ref strTempGroup);
                            }

                            strWD = strWD + strWDColumCode + ",";
                            strWDGroup = strWDGroup + strTempGroup + ",";


                            //获取维度字段筛选条件
                            JArray querylist = (JArray)item["querydata"];
                            foreach (JObject queryitem in querylist)
                            {
                                string strcal = (string)queryitem["cal"];
                                string strglval = (string)queryitem["glval"];
                                if (!string.IsNullOrEmpty(strglval))
                                {
                                    switch (strcal)
                                    {
                                        case "0": strWhere += " and " + strTempGroup + " ='" + strglval + "'"; break;
                                        case "1": strWhere += " and " + strTempGroup + " <'" + strglval + "'"; break;
                                        case "2": strWhere += " and " + strTempGroup + " >'" + strglval + "'"; break;
                                        case "3": strWhere += " and " + strTempGroup + " !='" + strglval + "'"; break;
                                        case "4": strWhere += " and " + strWDColumCode + "  LIKE  '%" + strglval.ToFormatLike() + "%'"; break;
                                        case "5": strWhere += " and " + strWDColumCode + "  BETWEEN '" + strglval.Split(',')[0] + " 00:00:00' AND '" + strglval.Split(',')[1] + " 00:00:00 '"; break;
                                        case "6": strWhere += " and " + strWDColumCode + "  IN  ('" + strglval.ToFormatLike() + "')"; break;


                                    }
                                }
                            }

                            //处理组件筛选
                            //string bindwig = (string)item["bindwig"];
                            //getqwig(strquerydata, strWDColumCode, bindwig, ref strWhere);


                            //处理排序
                            if ((string)orderdata["prop"].ToString() == strWDColumCode)
                            {
                                ordersql = strWDColumCode + " " + (string)orderdata["order"].ToString();
                            }

                        }
                        strWD = strWD.TrimEnd(',');
                        strWDGroup = strWDGroup.TrimEnd(',');

                        string strDL = "";
                        string strHaving = "HAVING";
                        foreach (JObject item in dllist)
                        {
                            strDL = strDL + " " + (string)item["caltype"] + "(" + (string)item["colid"] + ") AS " + (string)item["colid"] + ",";
                            string strTJFiled = (string)item["caltype"] + "(" + (string)item["colid"] + ")  ";
                            //获取统计字段筛选条件
                            JArray querylist = (JArray)item["querydata"];
                            foreach (JObject queryitem in querylist)
                            {
                                string strcal = (string)queryitem["cal"];
                                string strglval = (string)queryitem["glval"];
                                if (!string.IsNullOrEmpty(strglval))
                                {
                                    string strPre = strHaving == "HAVING" ? " " : " and ";
                                    switch (strcal)
                                    {
                                        case "0":
                                            strHaving += strPre + strTJFiled + " ='" + strglval + "'";
                                            break;
                                        case "1":
                                            strHaving += strPre + strTJFiled + " <'" + strglval + "'";
                                            break;
                                        case "2":
                                            strHaving += strPre + strTJFiled + " >'" + strglval + "'";
                                            break;
                                        case "3":
                                            strHaving += strPre + strTJFiled + " !='" + strglval + "'";
                                            break;
                                        case "4":
                                            strHaving += strPre + strTJFiled + "  LIKE  '%" + strglval.ToFormatLike() + "%'";
                                            break;
                                        case "6":
                                            strHaving += strPre + strTJFiled + "  IN  ('" + strglval.ToFormatLike() + "')";
                                            break;
                                    }
                                }


                            }
                            //处理组件筛选
                            //string bindwig = (string)item["bindwig"];
                            //getqwig(strquerydata, strTJFiled, bindwig, ref strHaving);

                            //处理排序
                            if ((string)orderdata["prop"].ToString() == (string)item["colid"])
                            {
                                ordersql = strTJFiled + " " + (string)orderdata["order"].ToString();
                            }

                        }
                        strDL = strDL.TrimEnd(',');
                        strHaving = strHaving == "HAVING" ? "" : strHaving;
                        strHaving = strHaving.Replace("HAVING", "");

                        int pageNo = int.Parse(context["pageNo"] == null ? "1" : context["pageNo"].ToString());
                        int pageSize = int.Parse(context["pageSize"] == null ? "0" : context["pageSize"].ToString());
                        int recordTotal = 0;
                        string strRSQL = "";
                        DataTable dt = db.GetYBData(dsql, strWD, strDL, strPageCount, strWhere, ordersql, pageNo, pageSize, strWDGroup, strHaving, ref recordTotal, ref strRSQL);
                        if (dt.Rows.Count > 8000)
                        {
                            //msg.ErrorMsg = "返回数据量太大,超过8000,服务器只返回前8000条数据";
                            dt = dt.SplitDataTable(1, 8000);
                        }
                        msg.Result = dt;
                        msg.DataLength = recordTotal;
                        // msg.Result1 = strRSQL;
                    }
                }
                else if (datatype == "3")//存储过程
                {
                    //string strAPIUrl = (string)wigdata["apiurl"] + "&szhlcode=" + UserInfo.User.pccode;
                    //string str = CommonHelp.GetAPIData(strAPIUrl);
                    List<SugarParameter> ListP = new List<SugarParameter>();
                    string strProname = (string)wigdata["proname"];
                    JArray prlist = (JArray)wigdata["proqdata"];
                    foreach (var item in prlist)
                    {
                        string pname = (string)item["pname"];
                        string pvalue = (string)item["pvalue"];
                        ListP.Add(new SugarParameter(pname, pvalue));
                    }
                    DBFactory dbccgc = new BI_DB_SourceB().GetDB(1);
                    DataTable dt = dbccgc.GetDBClient().Ado.UseStoredProcedure().GetDataTable(strProname, ListP);
                    msg.Result = dt;
                }
                else
                {

                    string strAPIUrl = (string)wigdata["apiurl"];
                    JArray prlist = (JArray)wigdata["proqdata"];
                    string pr = "?1=1";
                    foreach (var item in prlist)
                    {
                        string pname = (string)item["pname"];
                        string pvalue = (string)item["pvalue"];
                        pr = pr + "&" + pname + "=" + pvalue;
                        //if (pname == "szhlcode")
                        //{
                        //    pvalue = EncrpytHelper.UnEscape(pvalue);
                        //}

                    }
                    string str = CommonHelp.HttpGet(strAPIUrl.Replace("//", "/").Replace(":/", "://") + pr);
                    msg.Result = str;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                msg.ErrorMsg = "获取数据错误";
            }

        }
        /// <summary>
        /// 获取仪表盘可用Token
        /// </summary>
        /// <param name="context"></param>
        /// <param name="msg"></param>
        /// <param name="P1"></param>
        /// <param name="P2"></param>
        /// <param name="UserInfo"></param>
        public void GETYBPTOKEN(JObject context, Msg_Result msg, string P1, string P2)
        {
            new DATABIManage().GETYBDATA(context, msg, P1, P2);
        }
        /// <summary>
        /// 根据关联查询组件获取查询条件语句(废弃)
        /// </summary>
        /// <param name="strquerydata"></param>
        /// <param name="strcolid"></param>
        /// <param name="strwigcode"></param>
        public void getqwig(string strquerydata, string strcolid, string strwigcode, ref string strWhere)
        {
            JArray categories = JArray.Parse(strquerydata);
            foreach (JObject item in categories)
            {

                string FiledName = strcolid;
                if (strwigcode == (string)item["wigdetcode"])
                {
                    string ColumnType = (string)item["ColumnType"];
                    string eltype = (string)item["component"];
                    if (eltype == "qjInput")
                    {
                        string strValue = (string)item["value"];
                        if (!string.IsNullOrEmpty(strValue))
                        {
                            string strSQL = string.Format(" AND {0} LIKE ('%{1}%')", FiledName.Replace(',', '+'), strValue);
                            strWhere = strWhere + strSQL;
                        }
                    }
                    else
                    if (eltype == "qjSelect")
                    {
                        string strValue = (string)item["value"];
                        if (!string.IsNullOrEmpty(strValue))
                        {
                            string strSQL = string.Format(" AND {0} IN ('{1}')", FiledName, strValue);
                            strWhere = strWhere + strSQL;
                        }
                    }
                    else
                    if (eltype == "qjMonth" || eltype == "qjDate")
                    {
                        if (item["value"] != null && item["value"].ToString() != "")
                        {
                            string strval = item["value"].ToString();
                            string sDate = strval.Split(',')[0].ToString();
                            string eDate = strval.Split(',')[1].ToString();
                            string strSQL = string.Format(" AND {0} BETWEEN '{1} 00:00' AND '{2} 23:59' ", FiledName, sDate, eDate);
                            strWhere = strWhere + strSQL;
                        }

                    }
                    else
                    {
                        string strValue = (string)item["value"];
                        if (!string.IsNullOrEmpty(strValue))
                        {
                            string strSQL = string.Format(" AND {0} IN ('{1}')", FiledName, strValue);
                            strWhere = strWhere + strSQL;
                        }
                    }
                }


            }

        }



        /// <summary>
        /// 验证API数据接口
        /// </summary>
        /// <param name="context"></param>
        /// <param name="msg"></param>
        /// <param name="P1"></param>
        /// <param name="P2"></param>
        /// <param name="UserInfo"></param>
        public void YZAPIDATA(JObject context, Msg_Result msg, string P1, string P2)
        {
            try
            {
                //string strAPIUrl = P1 + "&szhlcode=" + UserInfo.User.pccode;
                //msg.Result = CommonHelp.GetAPIData(strAPIUrl);
            }
            catch (Exception ex)
            {
                msg.ErrorMsg = ex.Message;
            }

        }

        public void GETSQLDATA(JObject context, Msg_Result msg, string P1, string P2)
        {
            string SQL = CommonHelp.Filter(P1);
            DBFactory db = new BI_DB_SourceB().GetDB(0);
            DataTable dt = db.GetSQL(SQL);
            msg.Result = dt;
        }

        #endregion

        #region Table


        public void GETTABLEDATA(JObject context, Msg_Result msg, string P1, string P2)
        {

            msg.Result = new BI_DB_TableB().GetEntity(D => D.ID.ToString() == P1);
            msg.Result1 = new BI_DB_TablefiledB().GetEntities(D => D.TableID.ToString() == P1 && D.isPkey == "0");//
            msg.Result2 = new BI_DB_TablefiledB().GetEntities(D => D.TableID.ToString() == P1 && D.isPkey == "1");//
        }

        /// <summary>
        /// 修改Table名称
        /// </summary>
        /// <param name="context"></param>
        /// <param name="msg"></param>
        /// <param name="P1"></param>
        /// <param name="P2"></param>
        /// <param name="UserInfo"></param>
        public void RETABNAME(JObject context, Msg_Result msg, string P1, string P2)
        {

            new BI_DB_TableB().ReTabName(int.Parse(P1), P2);


        }
        /// <summary>
        /// 更新表单数据,已经有的创建,没有得更新
        /// </summary>
        /// <param name="context"></param>
        /// <param name="msg"></param>
        /// <param name="P1"></param>
        /// <param name="P2"></param>
        /// <param name="UserInfo"></param>
        public void UPBDCOLDATA(JObject context, Msg_Result msg, string P1, string P2)
        {


            BI_DB_Tablefiled model = JsonConvert.DeserializeObject<BI_DB_Tablefiled>(P1);
            if (model.ID == 0)
            {
                model.CRDate = DateTime.Now;
                model.CRUser = UserContext.Current.UserName;
                new BI_DB_TablefiledB().Insert(model);
                new BI_DB_TablefiledB().AddCol(model);

            }
            else
            {
                BI_DB_Tablefiled oldmodel = new BI_DB_TablefiledB().GetEntity(D => D.ID == model.ID);
                new BI_DB_TablefiledB().Update(model);
                if (oldmodel.DbColumnName != model.DbColumnName)
                {
                    new BI_DB_TablefiledB().ReColName(model, oldmodel.DbColumnName);
                }
                new BI_DB_TablefiledB().upCol(model);

            }

            msg.Result = model;

        }
        public void DELTABLEDATA(JObject context, Msg_Result msg, string P1, string P2)
        {
            int ID = Int32.Parse(P1);

            new BI_DB_TableB().DelLogicTable(ID);

        }
        public void DELCOL(JObject context, Msg_Result msg, string P1, string P2)
        {
            int COLID = Int32.Parse(P1);
            BI_DB_Tablefiled COL = new BI_DB_TablefiledB().GetEntity(D => D.ID == COLID);
            new BI_DB_TablefiledB().Delete(D => D.ID == COLID);
            new BI_DB_TablefiledB().DelCol(COL);

        }


        #region 通用List组件页面


        /// <summary>
        /// 给通用List组件页面用
        /// </summary>
        /// <param name="context"></param>
        /// <param name="msg"></param>
        /// <param name="P1"></param>
        /// <param name="P2"></param>
        /// <param name="UserInfo"></param>
        public void GETVUELISTDATA(JObject context, Msg_Result msg, string P1, string P2)
        {

            string strDataType = P1.Split(',')[0];
            string strDataID = P1.Split(',')[1];

            string strWDFiled = "";
            string strWD = context.Request("WD");
            string strQDATA = context.Request("qdata");

            if (strWD != null)
            {
                JArray wdlist = JsonConvert.DeserializeObject(strWD) as JArray;
                foreach (var item in wdlist)
                {
                    strWDFiled = strWDFiled + (string)item["colid"] + ",";
                }

            }
            else
            {
                strWDFiled = "*";
            }

            string strQWhere = "";
            JArray querylist = JsonConvert.DeserializeObject(strQDATA) as JArray;
            foreach (JObject queryitem in querylist)
            {
                string strcxzd = (string)queryitem["colid"];
                string strcal = (string)queryitem["cal"];
                string strglval = (string)queryitem["val"];
                if (!string.IsNullOrEmpty(strglval))
                {
                    switch (strcal)
                    {
                        case "0":
                            strQWhere += strQWhere + " and " + strcxzd + " ='" + strglval + "'";
                            break;
                        case "1":
                            strQWhere += strQWhere + " and " + strcxzd + "  LIKE  '%" + strglval.ToFormatLike() + "%'";
                            break;
                        case "2":
                            strQWhere += strQWhere + " and " + strcxzd + "  IN  ('" + strglval.ToFormatLike() + "')";
                            break;
                    }
                }
            }
        }

        public void GETTABANDSET(JObject context, Msg_Result msg, string P1, string P2)
        {
            var datatable = new BI_DB_TableB().Db.Queryable<BI_DB_Table>()
                     .Select(f => new
                     {
                         value = f.ID,
                         label = f.TableDesc
                     }).ToList();
            var dataset = new BI_DB_SetB().Db.Queryable<BI_DB_Set>()
                  .Select(f => new
                  {
                      value = f.ID,
                      label = f.Name
                  }).ToList();
            msg.Result = datatable;
            msg.Result1 = dataset;

        }

        public void GETFIELDDATA(JObject context, Msg_Result msg, string P1, string P2)
        {
            int dataid = int.Parse(P2);
            if (P1 == "0")
            {
                var fileds = new BI_DB_TablefiledB().Db.Queryable<BI_DB_Tablefiled>().Where(D => D.TableID == dataid)
                  .Select(f => new
                  {
                      colid = f.DbColumnName,
                      coltype = f.DataType,
                      colname = f.ColumnDescription,
                      isshow = true
                  }).ToList();
                msg.Result = fileds;

            }
            else
            {
                var fileds = new BI_DB_DimB().Db.Queryable<BI_DB_Dim>().Where(D => D.STID == dataid)
              .Select(f => new
              {
                  colid = f.ColumnName,
                  coltype = f.ColumnType,
                  colname = f.Name,
                  isshow = true
              }).ToList();
                msg.Result = fileds;
            }


        }

        private string getfiletype(string strType)
        {
            string strReturn = "";
            if (strType.Contains("int"))
            {
                strReturn = "Num";
            }
            else if (strType.Contains("decimal") || strType.Contains("float") || strType.Contains("Double"))
            {
                strReturn = "float";
            }
            else if (strType.Contains("datetime"))
            {
                strReturn = "Date";
            }
            else
            {
                strReturn = "Str";
            }
            return strReturn;
        }



        #endregion




        /// <summary>
        /// 数据集默认查询API
        /// </summary>
        /// <param name="context"></param>
        /// <param name="msg"></param>
        /// <param name="P1"></param>
        /// <param name="P2"></param>
        /// <param name="UserInfo"></param>
        public void GETQJDATASETDATA(JObject context, Msg_Result msg, string P1, string P2)
        {
            try
            {
                msg.DataLength = 0;
                string strTableID = P1;
                string ordersql = "";
                string strPageCount = context.Request("pagecount") ?? "0";
                string strWhere = " 1=1 ";
                string strWD = "";
                string strWDGroup = "";//处理MYSQL GROUP别名问题

                BI_DB_Set DS = new BI_DB_SetB().GetEntities(d => d.ID.ToString() == strTableID).FirstOrDefault();
                DBFactory db = new BI_DB_SourceB().GetDB(DS.SID.Value);

                if (!string.IsNullOrEmpty(P2))
                {
                    JObject wigdata = JObject.Parse(P2);
                    ordersql = (string)wigdata["dataorder"] ?? "";
                    string wdlist = (string)wigdata["wdlist"];
                    strWD = wdlist.TrimEnd(',');
                    JArray querylist = (JArray)wigdata["querydata"];
                    if (querylist != null)
                    {
                        foreach (JObject queryitem in querylist)
                        {
                            string strQFiled = (string)queryitem["qfiled"];
                            string strcal = (string)queryitem["cal"];
                            string strglval = (string)queryitem["glval"];
                            if (!string.IsNullOrEmpty(strglval))
                            {
                                switch (strcal)
                                {
                                    case "0": strWhere += " and " + strQFiled + " ='" + strglval + "'"; break;
                                    case "1": strWhere += " and " + strQFiled + " <'" + strglval + "'"; break;
                                    case "2": strWhere += " and " + strQFiled + " >'" + strglval + "'"; break;
                                    case "3": strWhere += " and " + strQFiled + " !='" + strglval + "'"; break;
                                    case "4": strWhere += " and " + strQFiled + "  LIKE  '%" + strglval.ToFormatLike() + "%'"; break;
                                    case "5": strWhere += " and " + strQFiled + "  BETWEEN '" + strglval.Split(',')[0] + " 00:00:00' AND '" + strglval.Split(',')[1] + " 00:00:00 '"; break;
                                    case "6": strWhere += " and " + strQFiled + "  IN  ('" + strglval.ToFormatLike() + "')"; break;


                                }
                            }
                        }
                    }


                }


                int pageNo = int.Parse(context.Request("pageNo") ?? "1");
                int pageSize = int.Parse(context.Request("pageSize") ?? "0");
                int recordTotal = 0;
                string strRSQL = "";
                DataTable dt = db.GetYBData(DS.DSQL, strWD, "", strPageCount, strWhere, ordersql, pageNo, pageSize, strWDGroup, "", ref recordTotal, ref strRSQL);
                if (dt.Rows.Count > 8000)
                {
                    //msg.ErrorMsg = "返回数据量太大,超过8000,服务器只返回前8000条数据";
                    dt = dt.SplitDataTable(1, 8000);
                }
                msg.Result = dt;
                msg.DataLength = recordTotal;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                msg.ErrorMsg = "获取数据错误";
            }

        }

        /// <summary>
        /// 数据表默认查询API
        /// </summary>
        /// <param name="context"></param>
        /// <param name="msg"></param>
        /// <param name="P1"></param>
        /// <param name="P2"></param>
        /// <param name="UserInfo"></param>
        public void GETQJTABLEDATA(JObject context, Msg_Result msg, string P1, string P2)
        {
            try
            {
                msg.DataLength = 0;
                string strTableID = P1;
                string ordersql = "";
                string strPageCount = context.Request("pagecount") ?? "0";
                string strWhere = " 1=1 ";
                string strWD = "";
                string strWDGroup = "";//处理MYSQL GROUP别名问题

                BI_DB_Table DS = new BI_DB_TableB().GetEntities(d => d.ID.ToString() == strTableID).FirstOrDefault();
                DBFactory db = new BI_DB_SourceB().GetDB(DS.DSID);

                if (!string.IsNullOrEmpty(P2))
                {
                    JObject wigdata = JObject.Parse(P2);
                    ordersql = (string)wigdata["dataorder"];
                    string wdlist = (string)wigdata["wdlist"];
                    strWD = wdlist.TrimEnd(',');
                    JArray querylist = (JArray)wigdata["querydata"];
                    if (querylist != null)
                    {
                        foreach (JObject queryitem in querylist)
                        {
                            string strQFiled = (string)queryitem["qfiled"];
                            string strcal = (string)queryitem["cal"];
                            string strglval = (string)queryitem["glval"];
                            if (!string.IsNullOrEmpty(strglval))
                            {
                                switch (strcal)
                                {
                                    case "0": strWhere += " and " + strQFiled + " ='" + strglval + "'"; break;
                                    case "1": strWhere += " and " + strQFiled + " <'" + strglval + "'"; break;
                                    case "2": strWhere += " and " + strQFiled + " >'" + strglval + "'"; break;
                                    case "3": strWhere += " and " + strQFiled + " !='" + strglval + "'"; break;
                                    case "4": strWhere += " and " + strQFiled + "  LIKE  '%" + strglval.ToFormatLike() + "%'"; break;
                                    case "5": strWhere += " and " + strQFiled + "  BETWEEN '" + strglval.Split(',')[0] + " 00:00:00' AND '" + strglval.Split(',')[1] + " 00:00:00 '"; break;
                                    case "6": strWhere += " and " + strQFiled + "  IN  ('" + strglval.ToFormatLike() + "')"; break;


                                }
                            }
                        }
                    }
                }
                int pageNo = int.Parse(context.Request("pageNo") ?? "1");
                int pageSize = int.Parse(context.Request("pageSize") ?? "0");
                int recordTotal = 0;
                string strRSQL = "";

                string strDSSQL = "SELECT * FROM  " + DS.TableName;
                DataTable dt = db.GetYBData(strDSSQL, strWD, "", strPageCount, strWhere, ordersql, pageNo, pageSize, strWDGroup, "", ref recordTotal, ref strRSQL);
                if (dt.Rows.Count > 8000)
                {
                    //msg.ErrorMsg = "返回数据量太大,超过8000,服务器只返回前8000条数据";
                    dt = dt.SplitDataTable(1, 8000);
                }
                msg.Result = dt;
                msg.DataLength = recordTotal;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                msg.ErrorMsg = "获取数据错误";
            }
        }
        #endregion
    }
}