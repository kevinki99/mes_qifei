namespace iMES.Bi.Data
{
    public class SqlHelp
    {
        /// <summary>
        /// 将SQLserver的连接字符串语法转为Mysql的语法
        /// </summary>
        /// <param name="objs"></param>
        /// <returns></returns>
        public static string concat(string strConSQL)
        {
            string strReturn = strConSQL;
            string strDbType = Appsettings.app("DBType");
            if (strDbType == "0")
            {
                strReturn = "CONCAT(" + strReturn.Replace('+', ',') + ")";
            }
            return strReturn;
        }

        /// <summary>
        /// 解析日期格式
        /// </summary>
        /// <param name="objs"></param>
        /// <returns></returns>
        public static string TADate(string strWDColumCode, string strDbType, ref string strGroup)
        {
            string strFiledName = strWDColumCode.Split('_')[0];
            string Format = strWDColumCode.Split('_')[1];

            string strReturn = "";
            if (strDbType == "MYSQL")
            {
                switch (Format)
                {
                    case "YYYY":
                        strGroup = " DATE_FORMAT(" + strFiledName + ",'%Y')   ";
                        strReturn = " DATE_FORMAT(" + strFiledName + ",'%Y')   " + strWDColumCode;
                        break;
                    case "YYYYMM":
                        strGroup = " DATE_FORMAT(" + strFiledName + ",'%Y%m')   ";
                        strReturn = " DATE_FORMAT(" + strFiledName + ",'%Y%m')   " + strWDColumCode;
                        break;
                    case "YYYYMMDD":
                        strGroup = " DATE_FORMAT(" + strFiledName + ",'%Y%m%d')   ";
                        strReturn = " DATE_FORMAT(" + strFiledName + ",'%Y%m%d')   " + strWDColumCode;
                        break;
                    case "YYYYMMDDHH":
                        strGroup = " DATE_FORMAT(" + strFiledName + ",'%Y%m%d%h')   ";
                        strReturn = " DATE_FORMAT(" + strFiledName + ",'%Y%m%d%h')   " + strWDColumCode;
                        break;
                }
            }
            if (strDbType == "SQLSERVER")
            {
                switch (Format)
                {
                    case "YYYY":
                        strGroup = " LEFT(CONVERT(varchar(100)," + strFiledName + ",112),4) ";
                        strReturn = " LEFT(CONVERT(varchar(100)," + strFiledName + ",112),4) AS '" + strWDColumCode + "'";
                        break;
                    case "YYYYMM":
                        strGroup = " LEFT(CONVERT(varchar(100)," + strFiledName + ",112),6) ";
                        strReturn = " LEFT(CONVERT(varchar(100)," + strFiledName + ",112),6) AS '" + strWDColumCode + "'";
                        break;
                    case "YYYYMMDD":
                        strGroup = " LEFT(CONVERT(varchar(100)," + strFiledName + ",112),8) ";
                        strReturn = " LEFT(CONVERT(varchar(100)," + strFiledName + ",112),8) AS '" + strWDColumCode + "'";
                        break;
                    case "YYYYMMDDHH":
                        strGroup = " (LEFT(CONVERT(varchar(100)," + strFiledName + ",112),8)+RIGHT('00'+cast(DATEPART(hh," + strFiledName + ") as VARCHAR),2 ) ) ";
                        strReturn = " (LEFT(CONVERT(varchar(100)," + strFiledName + ",112),8)+RIGHT('00'+cast(DATEPART(hh," + strFiledName + ") as VARCHAR),2 ) ) AS '" + strWDColumCode + "'";
                        break;
                }
            }
            return strReturn;
        }


        /// <summary>
        /// 处理链接字符串废弃
        /// </summary>
        /// <returns></returns>
        public static string concatold(params string[] objs)
        {
            string strReturn = "";
            string strDbType = Appsettings.app("DBType");
            if (strDbType == "0")
            {
                for (int i = 0; i < objs.Length; i++)
                {
                    strReturn = strReturn + ",";
                }
                strReturn = " CONCAT(" + strReturn.TrimEnd(',') + " ) ";
            }
            if (strDbType == "1")
            {
                for (int i = 0; i < objs.Length; i++)
                {
                    strReturn = objs[i].ToString() + "+";
                }
                strReturn.TrimEnd('+');
            }

            return strReturn;
        }
    }
}
