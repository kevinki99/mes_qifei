using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using iMES.Bi.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace iMES.Bi.API
{
    public class CommonHelp
    {

        public static T DeepCopyByReflect<T>(T obj)
        {
            //如果是字符串或值类型则直接返回
            if (obj is string || obj.GetType().IsValueType) return obj;

            object retval = Activator.CreateInstance(obj.GetType());
            FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            foreach (FieldInfo field in fields)
            {
                try { field.SetValue(retval, DeepCopyByReflect(field.GetValue(obj))); }
                catch { }
            }
            return (T)retval;
        }
        /// <summary>
        /// 从html中提取纯文本
        /// </summary>
        /// <param name="strHtml"></param>
        /// <returns></returns>
        public string StripHT(string strHtml)  //从html中提取纯文本
        {
            Regex regex = new Regex("<.+?>", RegexOptions.IgnoreCase);
            string strOutput = regex.Replace(strHtml, "");//替换掉"<"和">"之间的内容
            strOutput = strOutput.Replace("<", "");
            strOutput = strOutput.Replace(">", "");
            strOutput = strOutput.Replace("&nbsp;", "");
            return strOutput;
        }


        /// <summary>
        /// 移除html标签
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string RemoveHtml(string html)
        {
            if (string.IsNullOrEmpty(html)) return html;

            Regex regex = new Regex("<.+?>");
            var matches = regex.Matches(html);

            foreach (Match match in matches)
            {
                html = html.Replace(match.Value, "");
            }
            return html;
        }


  






        public static string PostWebRequest(string postUrl, string paramData, Encoding dataEncode)
        {
            string ret = string.Empty;
            try
            {
                byte[] byteArray = dataEncode.GetBytes(paramData); //转化
                HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(new Uri(postUrl));
                webReq.Method = "POST";
                webReq.ContentType = "application/json; charset=UTF-8";

                webReq.ContentLength = byteArray.Length;
                Stream newStream = webReq.GetRequestStream();
                newStream.Write(byteArray, 0, byteArray.Length);//写入参数
                newStream.Close();
                HttpWebResponse response = (HttpWebResponse)webReq.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default);
                ret = sr.ReadToEnd();
                sr.Close();
                response.Close();
                newStream.Close();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return ret;
        }

        public static HttpWebResponse CreateHttpResponse(string url, IDictionary<string, string> parameters, int timeout, string userAgent, CookieCollection cookies, string strType = "POST")
        {
            HttpWebRequest request = null;
            //如果是发送HTTPS请求  
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                request = WebRequest.Create(url) as HttpWebRequest;
                //request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            request.Method = strType;
            request.ContentType = "application/x-www-form-urlencoded";

            //设置代理UserAgent和超时
            //request.UserAgent = userAgent;
            //request.Timeout = timeout; 

            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }
            //发送POST数据  
            if (!(parameters == null || parameters.Count == 0))
            {
                StringBuilder buffer = new StringBuilder();
                int i = 0;
                foreach (string key in parameters.Keys)
                {
                    if (i > 0)
                    {
                        buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                    }
                    else
                    {
                        buffer.AppendFormat("{0}={1}", key, parameters[key]);
                        i++;
                    }
                }
                byte[] data = Encoding.UTF8.GetBytes(buffer.ToString());
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            string[] values = request.Headers.GetValues("Content-Type");
            return request.GetResponse() as HttpWebResponse;
        }

        /// <summary>  
        /// 创建POST方式的HTTP请求  
        /// </summary>  
        public static HttpWebResponse CreatePostHttpResponse(string url, IDictionary<string, string> parameters, int timeout, string userAgent, CookieCollection cookies, string strType = "POST")
        {
            HttpWebRequest request = null;
            //如果是发送HTTPS请求  
            if (url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                request = WebRequest.Create(url) as HttpWebRequest;
                //request.ProtocolVersion = HttpVersion.Version10;
            }
            else
            {
                request = WebRequest.Create(url) as HttpWebRequest;
            }
            request.Method = strType;
            request.ContentType = "application/x-www-form-urlencoded";

            //设置代理UserAgent和超时
            //request.UserAgent = userAgent;
            //request.Timeout = timeout; 

            if (cookies != null)
            {
                request.CookieContainer = new CookieContainer();
                request.CookieContainer.Add(cookies);
            }
            //发送POST数据  
            if (!(parameters == null || parameters.Count == 0))
            {
                StringBuilder buffer = new StringBuilder();
                int i = 0;
                foreach (string key in parameters.Keys)
                {
                    if (i > 0)
                    {
                        buffer.AppendFormat("&{0}={1}", key, parameters[key]);
                    }
                    else
                    {
                        buffer.AppendFormat("{0}={1}", key, parameters[key]);
                        i++;
                    }
                }
                byte[] data = Encoding.UTF8.GetBytes(buffer.ToString());
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            string[] values = request.Headers.GetValues("Content-Type");
            return request.GetResponse() as HttpWebResponse;
        }

        /// <summary>
        /// 获取请求的数据
        /// </summary>
        public static string GetResponseString(HttpWebResponse webresponse)
        {
            using (Stream s = webresponse.GetResponseStream())
            {
                StreamReader reader = new StreamReader(s, Encoding.Default);
                return reader.ReadToEnd();

            }
        }
        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="table"></param>
        /// <param name="fileName"></param>
        public static MemoryStream RenderToExcel(DataTable table)
        {
            MemoryStream ms = new MemoryStream();

            using (table)
            {
                IWorkbook workbook = new HSSFWorkbook();
                ISheet sheet = workbook.CreateSheet();
                IRow headerRow = sheet.CreateRow(0);

                // handling header.
                foreach (DataColumn column in table.Columns)
                    headerRow.CreateCell(column.Ordinal).SetCellValue(column.Caption);//If Caption not set, returns the ColumnName value

                // handling value.
                int rowIndex = 1;

                foreach (DataRow row in table.Rows)
                {
                    IRow dataRow = sheet.CreateRow(rowIndex);

                    foreach (DataColumn column in table.Columns)
                    {
                        dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                    }

                    rowIndex++;
                }

                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;
            }

            //using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            //{
            //    byte[] data = ms.ToArray();

            //    fs.Write(data, 0, data.Length);
            //    fs.Flush();
            //    data = null;
            //}
            return ms;
        }

        //
        public static bool HasData(Stream excelFileStream)
        {
            using (excelFileStream)
            {
                IWorkbook workbook = new HSSFWorkbook(excelFileStream);
                if (workbook.NumberOfSheets > 0)
                {
                    ISheet sheet = workbook.GetSheetAt(0);
                    return sheet.PhysicalNumberOfRows > 0;
                }
            }
            return false;
        }

        public static string SendDX(string Mobile, string Content, string SendTime)
        {
            try
            {
                string returnData = "";
                // string url = CommonHelp.GetConfig("DXURL") + "&Mobile=" + Mobile + "&Content=" + Content;
                if (CommonHelp.GetConfig("DXURL") != "")
                {
                    string url = CommonHelp.GetConfig("DXURL") + "&phone=" + Mobile + "&msg=" + Content;
                    CommonHelp.WriteLOG(url);
                    WebClient WC = new WebClient();
                    WC.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                    int p = url.IndexOf("?");
                    string sData = url.Substring(p + 1);
                    url = url.Substring(0, p);
                    byte[] postData = Encoding.GetEncoding("gb2312").GetBytes(sData);
                    byte[] responseData = WC.UploadData(url, "POST", postData);
                    returnData = Encoding.GetEncoding("gb2312").GetString(responseData);
                }
                return returnData;


            }
            catch (Exception Ex)
            {
                return Ex.Message;

            }

        }

        public static HSSFWorkbook ExportToExcel(DataTable dt)
        {




            HSSFWorkbook workbook = new HSSFWorkbook();
            if (dt.Rows.Count > 0)
            {
                ISheet sheet = workbook.CreateSheet("Sheet1");

                ICellStyle HeadercellStyle = workbook.CreateCellStyle();
                HeadercellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                HeadercellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                HeadercellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                HeadercellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                HeadercellStyle.Alignment = HorizontalAlignment.Center;
                HeadercellStyle.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.SkyBlue.Index;
                HeadercellStyle.FillPattern = FillPattern.SolidForeground;
                HeadercellStyle.FillBackgroundColor = NPOI.HSSF.Util.HSSFColor.SkyBlue.Index;

                //字体
                NPOI.SS.UserModel.IFont headerfont = workbook.CreateFont();
                headerfont.Boldweight = (short)FontBoldWeight.Bold;
                headerfont.FontHeightInPoints = 12;
                HeadercellStyle.SetFont(headerfont);


                //用column name 作为列名
                int icolIndex = 0;
                IRow headerRow = sheet.CreateRow(0);
                foreach (DataColumn dc in dt.Columns)
                {
                    ICell cell = headerRow.CreateCell(icolIndex);
                    cell.SetCellValue(dc.ColumnName);
                    cell.CellStyle = HeadercellStyle;
                    icolIndex++;
                }

                ICellStyle cellStyle = workbook.CreateCellStyle();

                //为避免日期格式被Excel自动替换，所以设定 format 为 『@』 表示一率当成text來看
                cellStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("@");
                cellStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                cellStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                cellStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                cellStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;


                NPOI.SS.UserModel.IFont cellfont = workbook.CreateFont();
                cellfont.Boldweight = (short)FontBoldWeight.Normal;
                cellStyle.SetFont(cellfont);

                //建立内容行
                int iRowIndex = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    int iCellIndex = 0;
                    IRow irow = sheet.CreateRow(iRowIndex + 1);
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        string strsj = string.Empty;
                        if (dr[i] != null)
                        {
                            strsj = dr[i].ToString();
                        }
                        ICell cell = irow.CreateCell(iCellIndex);
                        cell.SetCellValue(strsj);
                        cell.CellStyle = cellStyle;
                        iCellIndex++;
                    }
                    iRowIndex++;
                }

                //自适应列宽度
                for (int i = 0; i < icolIndex; i++)
                {
                    sheet.AutoSizeColumn(i);
                }

                //using (MemoryStream ms = new MemoryStream())
                //{
                //    workbook.Write(ms);

                //    HttpContext curContext = HttpContext.Current;


                //    // 设置编码和附件格式
                //    curContext.Response.ContentType = "application/vnd.ms-excel";
                //    curContext.Response.ContentEncoding = Encoding.UTF8;
                //    curContext.Response.Charset = "";
                //    curContext.Response.AppendHeader("Content-Disposition",
                //        "attachment;filename=" + HttpUtility.UrlEncode(Name + "_导出文件_" + DateTime.Now.Ticks + ".xls", Encoding.UTF8));

                //    curContext.Response.BinaryWrite(ms.GetBuffer());

                //    workbook = null;
                //    ms.Close();
                //    ms.Dispose();

                //    curContext.Response.End();
                //}
            }
            return workbook;

        }
        public DataTable ExcelToTable(Stream stream, int headrow, string suffix)
        {
            DataTable dt = new DataTable();

            IWorkbook workbook = null;
            if (suffix == "xlsx") // 2007版本
            {
                workbook = new XSSFWorkbook(stream);
            }
            else if (suffix == "xls") // 2003版本
            {
                workbook = new HSSFWorkbook(stream);
            }

            //获取excel的第一个sheet
            ISheet sheet = workbook.GetSheetAt(0);

            //获取sheet的第一行
            IRow headerRow = sheet.GetRow(headrow);

            //一行最后一个方格的编号 即总的列数
            int cellCount = headerRow.LastCellNum;
            //最后一列的标号  即总的行数
            int rowCount = sheet.LastRowNum;
            //列名
            for (int i = 0; i < cellCount; i++)
            {
                dt.Columns.Add(headerRow.GetCell(i).ToString());
            }

            for (int i = (sheet.FirstRowNum + headrow + 1); i <= sheet.LastRowNum; i++)
            {
                DataRow dr = dt.NewRow();

                IRow row = sheet.GetRow(i);
                for (int j = row.FirstCellNum; j < cellCount; j++)
                {
                    if (row.GetCell(j) != null)
                    {
                        dr[j] = row.GetCell(j).ToString();
                    }
                }

                dt.Rows.Add(dr);
            }

            sheet = null;
            workbook = null;

            return dt;
        }

        public static string HttpGet(string Url)
        {

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
            string retString = myStreamReader.ReadToEnd();
            myStreamReader.Close();
            myResponseStream.Close();

            return retString;
        }
        public static string GetAPIData(string strUrl)
        {
            string strReturn = "";
            string strHost = GetConfig("APITX");
            strHost = strHost.Substring(0, strHost.IndexOf("/api"));
            strReturn = CommonHelp.HttpGet(strUrl.Replace("$API_HOST", strHost));
            return strReturn;
        }

        private int rep = 0;

        /// <summary>
        /// 生成随机不重复的字符串（分享码用）
        /// </summary>
        /// <param name="codeCount"></param>
        /// <returns></returns>
        public string GenerateCheckCode(int codeCount)
        {
            string str = string.Empty;
            long num2 = DateTime.Now.Ticks + this.rep;
            this.rep++;
            Random random = new Random(((int)(((ulong)num2) & 0xffffffffL)) | ((int)(num2 >> this.rep)));
            for (int i = 0; i < codeCount; i++)
            {
                char ch;
                int num = random.Next();
                if ((num % 2) == 0)
                {
                    ch = (char)(0x30 + ((ushort)(num % 10)));
                }
                else
                {
                    ch = (char)(0x41 + ((ushort)(num % 0x1a)));
                }
                str = str + ch.ToString();
            }
            return str;
        }
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string GetMD5(string content)
        {
            using (var md5 = MD5.Create())
            {
                var result = md5.ComputeHash(Encoding.ASCII.GetBytes(content));
                var strResult = BitConverter.ToString(result);
                return strResult.Replace("-", "");
            }
        }



        public static string GetConfig(string strKey, string strDefault = "")
        {
            return Appsettings.app(strKey) ?? strDefault;
        }

        /// <summary>
        /// 获取数字验证码
        /// </summary>
        /// <param name="codenum"></param>
        /// <returns></returns>
        public static string numcode(int codenum)
        {
            string Vchar = "0,1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9";
            string[] VcArray = Vchar.Split(',');
            string[] stray = new string[codenum];
            Random random = new Random();
            for (int i = 0; i < codenum; i++)
            {
                int iNum = 0;
                while ((iNum = Convert.ToInt32(VcArray.Length * random.NextDouble())) == VcArray.Length)
                {
                    iNum = Convert.ToInt32(VcArray.Length * random.NextDouble());
                }
                stray[i] = VcArray[iNum];
            }

            string identifycode = string.Empty;
            foreach (string s in stray)
            {
                identifycode += s;
            }
            return identifycode;
        }
        /// <summary>
        /// 登录验证码
        /// </summary>
        /// <param name="codenum"></param>
        /// <returns></returns>
        public static string yzmcode(int codenum)
        {
            string Vchar = "0,1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,0,1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,W,X,Y,Z";
            string[] VcArray = Vchar.Split(',');
            string[] stray = new string[codenum];
            Random random = new Random();
            for (int i = 0; i < codenum; i++)
            {
                int iNum = 0;
                while ((iNum = Convert.ToInt32(VcArray.Length * random.NextDouble())) == VcArray.Length)
                {
                    iNum = Convert.ToInt32(VcArray.Length * random.NextDouble());
                }
                stray[i] = VcArray[iNum];
            }

            string identifycode = string.Empty;
            foreach (string s in stray)
            {
                identifycode += s;
            }
            return identifycode;
        }

        private static bool IsIPAddress(string str1)
        {
            if (str1 == null || str1 == string.Empty || str1.Length < 7 || str1.Length > 15) return false;

            string regformat = @"^\d{1,3}[\.]\d{1,3}[\.]\d{1,3}[\.]\d{1,3}$";

            Regex regex = new Regex(regformat, RegexOptions.IgnoreCase);
            return regex.IsMatch(str1);
        }




        public static void WriteLOG(string err)
        {
            try
            {

                string path = AppContext.BaseDirectory;// .NET CORE
                //string path = HttpContext.Current.Request.MapPath("/");

                if (!Directory.Exists(path + "/log/"))
                {
                    Directory.CreateDirectory(path + "/log/");
                }

                string name = DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
                if (!File.Exists(path + "/log/" + name))
                {
                    FileInfo myfile = new FileInfo(path + "/log/" + name);
                    FileStream fs = myfile.Create();
                    fs.Close();
                }

                StreamWriter sw = File.AppendText(path + "/log/" + name);
                sw.WriteLine(err + "\r\n" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                sw.Flush();
                sw.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }








        public static bool ProcessSqlStr(string Str, int type)
        {
            string SqlStr = "";
            if (type == 1)  //Post方法提交  
            {
                SqlStr = "script|iframe|xp_loginconfig|xp_fixeddrives|Xp_regremovemultistring|Xp_regread|Xp_regwrite|xp_cmdshell|xp_dirtree|count(|substring(|mid(|master|truncate|char(|declare|replace(|varchar(|cast(";
            }
            else if (type == 2) //Get方法提交  
            {
                SqlStr = "'|script|iframe|xp_loginconfig|xp_fixeddrives|Xp_regremovemultistring|Xp_regread|Xp_regwrite|xp_cmdshell|xp_dirtree|count(|*|asc(|chr(|substring(|mid(|master|truncate|char(|declare|replace(|;|varchar(|cast(";
            }
            else if (type == 3) //Cookie提交  
            {
                SqlStr = "script|iframe|xp_loginconfig|xp_fixeddrives|Xp_regremovemultistring|Xp_regread|Xp_regwrite|xp_cmdshell|xp_dirtree|count(|asc(|chr(|substring(|mid(|master|truncate|char(|declare";
            }
            else  //默认Post方法提交  
            {
                SqlStr = "script|iframe|xp_loginconfig|xp_fixeddrives|Xp_regremovemultistring|Xp_regread|Xp_regwrite|xp_cmdshell|xp_dirtree|count(|asc(|chr(|substring(|mid(|master|truncate|char(|declare|replace(";
            }

            bool ReturnValue = true;
            try
            {
                if (Str != "")
                {
                    string[] anySqlStr = SqlStr.ToUpper().Split('|'); ;
                    foreach (string ss in anySqlStr)
                    {
                        if (Str.ToUpper().IndexOf(ss) >= 0)
                        {
                            ReturnValue = false;
                        }
                    }
                }
            }
            catch
            {
                ReturnValue = false;
            }
            return ReturnValue;
        }


        public static string Filter(string str)
        {
            string[] pattern = { "insert ", "delete", "count\\(", "drop table", "update", "truncate", "xp_cmdshell", "exec   master", "netlocalgroup administrators", "net use " };
            for (int i = 0; i < pattern.Length; i++)
            {
                str = str.Replace(pattern[i].ToString(), "");
            }
            return str;
        }





        public static string CreateqQsql(string strQFiled, string strQtype, string strQvalue)
        {
            string strSQL = " AND ";
            strSQL = strSQL + strQFiled;
            if (strQtype == "0")
            {
                strSQL = strSQL + " = ";

            }
            strSQL = strSQL + "'" + strQvalue + "'";
            return strSQL.FilterSpecial();

        }



        /// <summary>
        /// 生成流水号格式：8位日期加3位顺序号，如20100302001。
        /// </summary>
        public static string GetWFNumber(string serialNumber, string ywcode, string strWFQZ)
        {
            if (serialNumber.Length < 13)
            {
                strWFQZ = "1";
                //没法子,只能曲线救国
            }
            string strLSTemp = strWFQZ == "0" ? "yyyyMMdd" : "yyyy";
            if (serialNumber != "0" && serialNumber != "")
            {
                string headDate = serialNumber.Substring(ywcode.Length + 1, strLSTemp.Length);
                int lastNumber = int.Parse(serialNumber.Substring(ywcode.Length + 1 + strLSTemp.Length));
                //如果数据库最大值流水号中日期和生成日期在同一天，则顺序号加1
                if (headDate == DateTime.Now.ToString(strLSTemp))
                {
                    lastNumber++;
                    return ywcode + "-" + headDate + lastNumber.ToString("000");
                }
            }
            return ywcode + "-" + DateTime.Now.ToString(strLSTemp) + "001";
        }

        /// <summary>
        /// 转化时分秒
        /// </summary>
        /// <param name="strSFM"></param>
        /// <returns></returns>
        public static int GetSencond(string strSFM)
        {
            int[] ListTemp = strSFM.SplitTOInt(':');

            return ListTemp[0] * 3600 + ListTemp[1] * 60 + ListTemp[2];
        }



        /// <summary>
        /// 在DataTable末尾添加合计行，最后一个参数设置需要合计的列，Tools.dbDataTableSumRowsWithColList(dt,"车号",new string[]{"个人产值","节油（元）","超油（元）"});
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="nColHeJi">需要显示‘合计’字段的列</param>
        /// <param name="colsHeJi">需要合计的列</param>
        public static DataRow dbDataTableSumRowsWithColList(DataTable dt, string sColHeJi, string[] colsHeJi)
        {
            DataRow dr = dt.NewRow();
            dr[sColHeJi] = "合计";
            dt.Rows.Add(dr);
            //初始化合计数组
            decimal[] arrDec = new decimal[colsHeJi.Length];
            for (int i = 0; i < colsHeJi.Length; i++)
            {
                arrDec[i] = decimal.Zero;
            }
            //合计
            for (int i = 0; i < dt.Rows.Count - 1; i++)
            {
                for (int j = 0; j < colsHeJi.Length; j++)
                {
                    string cName = colsHeJi[j];
                    if (Convert.IsDBNull(dt.Rows[i][cName])) continue;
                    arrDec[j] += decimal.Parse(dt.Rows[i][cName] + "");
                }
            }
            //赋值
            for (int i = 0; i < colsHeJi.Length; i++)
            {
                string cName = colsHeJi[i];
                if (arrDec[i] == decimal.Zero) dr[cName] = "0";
                else dr[cName] = arrDec[i];
            }
            return dr;
        }



        /// <summary>
        /// 分类小计，并有合计行，
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="nColHeJi">需要显示‘合计’字段的列</param>
        /// <param name="colsHeJi">需要合计的列</param>
        public static DataRow dbDataTableSubSumRowsWithColList(DataTable dt, string sColHeJi, string strGroupName, string[] colsHeJi)
        {
            DataRow dr = dt.NewRow();
            dr[sColHeJi] = "合计";
            dt.Rows.Add(dr);
            //初始化合计数组
            decimal[] arrDec = new decimal[colsHeJi.Length];
            for (int i = 0; i < colsHeJi.Length; i++)
            {
                arrDec[i] = decimal.Zero;
            }
            //合计
            for (int i = 0; i < dt.Rows.Count - 1; i++)
            {
                for (int j = 0; j < colsHeJi.Length; j++)
                {
                    string cName = colsHeJi[j];
                    if (Convert.IsDBNull(dt.Rows[i][cName])) continue;
                    arrDec[j] += decimal.Parse(dt.Rows[i][cName] + "");
                }
            }
            for (int i = 0; i < colsHeJi.Length; i++)
            {
                string cName = colsHeJi[i];
                if (arrDec[i] == decimal.Zero) dr[cName] = 0;
                else dr[cName] = arrDec[i];
            }
            if (dt.Rows.Count <= 1) return dr;
            //小计
            string sRate = "";
            int currSubSumCol = dt.Rows.Count - 2;
            ArrayList ar = new ArrayList();
            for (int i = dt.Rows.Count - 2; i >= 0; i--)
            {
                string currRate = dt.Rows[i][strGroupName] + "";
                if (sRate != currRate)
                {
                    if (i != dt.Rows.Count - 2)
                    {
                        dr = dt.NewRow();
                        dr[sColHeJi] = "小计";
                        for (int j = 0; j < colsHeJi.Length; j++)
                        {
                            string cName = colsHeJi[j];
                            if (arrDec[j] == decimal.Zero) dr[cName] = 0;
                            else dr[cName] = arrDec[j];
                        }
                        dt.Rows.InsertAt(dr, currSubSumCol + 1);
                    }
                    currSubSumCol = i;
                    sRate = currRate;
                    for (int j = 0; j < colsHeJi.Length; j++)
                    { //归零
                        arrDec[j] = decimal.Zero;
                    }
                }
                for (int j = 0; j < colsHeJi.Length; j++)
                {
                    string cName = colsHeJi[j];
                    if (Convert.IsDBNull(dt.Rows[i][cName])) continue;
                    arrDec[j] += decimal.Parse(dt.Rows[i][cName] + "");
                }
                if (i == 0)
                {
                    dr = dt.NewRow();
                    dr[sColHeJi] = "小计";
                    for (int j = 0; j < colsHeJi.Length; j++)
                    {
                        string cName = colsHeJi[j];
                        if (arrDec[j] == decimal.Zero) dr[cName] = 0;
                        else dr[cName] = arrDec[j];
                    }
                    dt.Rows.InsertAt(dr, currSubSumCol + 1);
                }
            }

            return dr;
        }





    }






}