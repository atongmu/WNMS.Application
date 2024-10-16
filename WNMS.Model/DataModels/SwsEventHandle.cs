using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class SwsEventHandle
    {
        public long Id { get; set; }
        public int UserId { get; set; }
        public string FeedBackInfo { get; set; }
        public bool IsConvertOrder { get; set; }
        public DateTime EventTime { get; set; }
        public int Rtuid { get; set; }
        public short EventSource { get; set; }
    }
}
