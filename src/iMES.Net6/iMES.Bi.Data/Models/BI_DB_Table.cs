﻿using System;
using System.Collections.Generic;

namespace iMES.Bi.Data
{
    public partial class BI_DB_Table
    {
        public int ID { get; set; }
        public string TableName { get; set; }
        public string TableDesc { get; set; }
        public string CRUser { get; set; }
        public DateTime? CRDate { get; set; }
        public int ModelID { get; set; }
        public string Remark { get; set; }
        public int DSID { get; set; }

        
    }
}
