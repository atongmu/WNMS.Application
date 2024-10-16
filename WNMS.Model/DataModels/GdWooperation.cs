using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class GdWooperation
    {
        public long Id { get; set; }
        public int UserId { get; set; }
        public DateTime OperationTime { get; set; }
        public string Description { get; set; }
        public long Pid { get; set; }
        public short Type { get; set; }
        public short State { get; set; }
    }
}
