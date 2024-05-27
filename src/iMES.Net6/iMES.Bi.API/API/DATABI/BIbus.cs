using iMES.Bi.Data;
using SqlSugar;
using System.Collections.Generic;
using System.Data;

namespace iMES.Bi.API
{
















    #region 数据BI模块

    public class BI_DB_DimB : BaseEFDao<BI_DB_Dim> { }
    public class BI_DB_SetB : BaseEFDao<BI_DB_Set>
    {


        /// <summary>
        /// 获取Data得经纬度
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public List<BI_DB_Dim> getCType(DataTable dt)
        {

            List<BI_DB_Dim> ListDIM = new List<BI_DB_Dim>();
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                BI_DB_Dim DIM = new BI_DB_Dim();
                DIM.Name = dt.Columns[i].ColumnName;
                DIM.ColumnName = dt.Columns[i].ColumnName;
                DIM.ColumnSource = "0";
                string strType = dt.Columns[i].DataType.Name.ToLower();
                if (strType.Contains("int"))
                {
                    DIM.ColumnType = "Num";
                    DIM.Dimension = "2";
                }
                else if (strType.Contains("decimal") || strType.Contains("float") || strType.Contains("Double"))
                {
                    DIM.ColumnType = "float";
                    DIM.Dimension = "2";

                }
                else if (strType.Contains("datetime"))
                {
                    DIM.ColumnType = "Date";
                    DIM.Dimension = "1";
                }
                else
                {
                    DIM.ColumnType = "Str";
                    DIM.Dimension = "1";

                }
                ListDIM.Add(DIM);
            }
            return ListDIM;
        }
    }
    public class BI_DB_SourceB : BaseEFDao<BI_DB_Source>
    {

        /// <summary>
        /// 获取数据源
        /// </summary>
        /// <param name="intSID"></param>
        /// <returns></returns>
        public DBFactory GetDB(int intSID)
        {
            DBFactory db = new DBFactory();
            if (intSID == 0)//本地数据库
            {
                string strCon = new BI_DB_SetB().GetDBString();
                db = new DBFactory(strCon);
            }
            else
            {
                var tt = new BI_DB_SourceB().GetEntity(p => p.ID == intSID);
                if (tt != null)
                {
                    db = new DBFactory(tt.DBType, tt.DBIP, tt.Port, tt.DBName, tt.DBUser, tt.DBPwd);
                }
            }
            return db;

        }
    }
    public class BI_DB_YBPB : BaseEFDao<BI_DB_YBP>
    {


    }

    public class BI_DB_TableB : BaseEFDao<BI_DB_Table>
    {
        public bool ReTabName(int tabid, string strNewName)
        {

            BI_DB_Table model = new BI_DB_TableB().GetEntity(D => D.ID == tabid);
            DBFactory dbccgc = new BI_DB_SourceB().GetDB(model.DSID);
            dbccgc.GetDBClient().DbMaintenance.RenameTable(model.TableName, strNewName);
            model.TableName = strNewName;
            return new BI_DB_TableB().Update(model);
        }
        public List<DbColumnInfo> CreateLogicTable(BI_DB_Table model)
        {
            string strTableName = model.TableName;
            DBFactory dbccgc = new BI_DB_SourceB().GetDB(model.DSID);
            List<DbColumnInfo> ListC = new List<DbColumnInfo>();
            DbColumnInfo colID = new DbColumnInfo();
            colID.DataType = "int";
            colID.DbColumnName = "ID";
            colID.IsPrimarykey = true;
            colID.IsIdentity = true;
            colID.IsNullable = false;
            ListC.Add(colID);

            DbColumnInfo colCRUser = new DbColumnInfo();
            colCRUser.DataType = "varchar";
            colCRUser.DbColumnName = "CRUser";
            colCRUser.ColumnDescription = "创建人";
            colCRUser.Length = 255;
            colCRUser.IsNullable = true;
            ListC.Add(colCRUser);


            DbColumnInfo colDCode = new DbColumnInfo();
            colDCode.DataType = "varchar";
            colDCode.DbColumnName = "DCode";
            colDCode.ColumnDescription = "机构代码";
            colDCode.Length = 255;
            colDCode.IsNullable = true;
            ListC.Add(colDCode);

            DbColumnInfo colDName = new DbColumnInfo();
            colDName.DataType = "varchar";
            colDName.DbColumnName = "DName";
            colDName.ColumnDescription = "机构名称";
            colDName.Length = 255;
            colDName.IsNullable = true;
            ListC.Add(colDName);

            DbColumnInfo colCRUserName = new DbColumnInfo();
            colCRUserName.DataType = "varchar";
            colCRUserName.DbColumnName = "CRUserName";
            colCRUserName.ColumnDescription = "真实姓名";
            colCRUserName.Length = 255;
            colCRUserName.IsNullable = true;
            ListC.Add(colCRUserName);

            DbColumnInfo colCRDate = new DbColumnInfo();
            colCRDate.DataType = "datetime";
            colCRDate.DbColumnName = "CRDate";
            colCRDate.IsNullable = true;

            ListC.Add(colCRDate);


            DbColumnInfo colINPID = new DbColumnInfo();
            colINPID.DataType = "int";
            colINPID.DbColumnName = "intProcessStanceid";
            colINPID.ColumnDescription = "流程ID";
            colINPID.IsNullable = true;
            colINPID.DefaultValue = "0";

            ListC.Add(colINPID);

            DbColumnInfo colComID = new DbColumnInfo();
            colComID.DataType = "int";
            colComID.DbColumnName = "ComID";
            colComID.IsNullable = true;
            ListC.Add(colComID);

            dbccgc.GetDBClient().DbMaintenance.CreateTable(strTableName, ListC);
            return ListC;
        }

        public void DelLogicTable(int tabid)
        {

            BI_DB_Table model = this.GetEntity(D => D.ID == tabid);
            DBFactory dbccgc = new BI_DB_SourceB().GetDB(model.DSID);
            string strTableName = model.TableName;
            dbccgc.GetDBClient().DbMaintenance.DropTable(strTableName);

            var result = this.Db.Ado.UseTran(() =>
            {
                new BI_DB_TableB().Delete(D => D.ID == tabid);
                new BI_DB_TablefiledB().Delete(D => D.TableID == tabid);

            });


        }
    }


    public class BI_DB_TablefiledB : BaseEFDao<BI_DB_Tablefiled>
    {
        public void AddCol(BI_DB_Tablefiled col)
        {

            BI_DB_Table model = new BI_DB_TableB().GetEntity(D => D.ID == col.TableID);
            DBFactory dbccgc = new BI_DB_SourceB().GetDB(model.DSID);
            DbColumnInfo filed = new DbColumnInfo();
            filed.DataType = col.DataType;
            filed.DbColumnName = col.DbColumnName;
            filed.IsPrimarykey = false;
            filed.IsIdentity = false;
            filed.ColumnDescription = col.ColumnDescription;
            filed.IsNullable = col.IsNullable == "0" ? true : false;
            filed.DefaultValue = col.DefaultValue;
            if (dbccgc.GetDBType() == "SqlServer" && col.DataType == "int")
            {
                filed.Length = 0;
            }
            else
            {
                filed.Length = col.Length == -1 ? int.MaxValue : col.Length;

            }
            filed.DecimalDigits = int.Parse(col.DecimalDigits);

            dbccgc.GetDBClient().DbMaintenance.AddColumn(model.TableName, filed);
        }

        public bool upCol(BI_DB_Tablefiled col)
        {

            BI_DB_Table model = new BI_DB_TableB().GetEntity(D => D.ID == col.TableID);
            DBFactory dbccgc = new BI_DB_SourceB().GetDB(model.DSID);
            DbColumnInfo filed = new DbColumnInfo();
            filed.DataType = col.DataType;
            filed.DbColumnName = col.DbColumnName;
            filed.ColumnDescription = col.ColumnDescription;
            filed.IsNullable = col.IsNullable == "0" ? true : false;
            filed.DefaultValue = col.DefaultValue;
            if (dbccgc.GetDBType() == "SqlServer" && col.DataType == "int")
            {
                filed.Length = 0;
            }
            else
            {
                filed.Length = col.Length;

            }
            filed.DecimalDigits = int.Parse(col.DecimalDigits);

            return dbccgc.GetDBClient().DbMaintenance.UpdateColumn(model.TableName, filed);
        }

        public bool DelCol(BI_DB_Tablefiled col)
        {

            BI_DB_Table model = new BI_DB_TableB().GetEntity(D => D.ID == col.TableID);
            DBFactory dbccgc = new BI_DB_SourceB().GetDB(model.DSID);
            return dbccgc.GetDBClient().DbMaintenance.DropColumn(model.TableName, col.DbColumnName);
        }


        public bool ReColName(BI_DB_Tablefiled col, string strOldName)
        {

            BI_DB_Table model = new BI_DB_TableB().GetEntity(D => D.ID == col.TableID);
            DBFactory dbccgc = new BI_DB_SourceB().GetDB(model.DSID);
            return dbccgc.GetDBClient().DbMaintenance.RenameColumn(model.TableName, strOldName, col.DbColumnName);
        }
    }



    #endregion











}
