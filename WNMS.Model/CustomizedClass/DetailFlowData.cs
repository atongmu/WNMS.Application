using MongoDBHelper;
using System;
using System.Collections.Generic;
using System.Text;

namespace WNMS.Model.CustomizedClass
{
    public class DetailFlowData: BaseEntity
    {
        public Nullable<double> Data { get; set; }
        public Nullable<double> Data1 { get; set; }
        public Nullable<double> Data2 { get; set; }
        public Nullable<double> Data3 { get; set; }
        public Nullable<double> Data4 { get; set; }
        public DateTime Time { get; set; }
    }
}
