using System;
using System.Collections.Generic;

namespace iMES.Bi.Data
{
    public partial class BI_DB_Tablefiled
    {
        public int ID { get; set; }
        public int? TableID { get; set; }
        public string TableName { get; set; }
        public string PropertyName { get; set; }
        public string ColumnDescription { get; set; }
        public string DataType { get; set; }
        public string IsPrimarykey { get; set; }
        public string CRUser { get; set; }
        public DateTime? CRDate { get; set; }
        public string DbColumnName { get; set; }
        public string IsIdentity { get; set; }
        public string IsNullable { get; set; }
        public string isPkey { get; set; }
        public string DecimalDigits { get; set; }
        public int Length { get; set; }
        public string DefaultValue { get; set; }
        public string Scale { get; set; }



    }
}
