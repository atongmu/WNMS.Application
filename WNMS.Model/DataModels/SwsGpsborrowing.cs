using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class SwsGpsborrowing
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public DateTime? BorrowTime { get; set; }
        public DateTime? RemandTime { get; set; }
        public string Remark { get; set; }
        public string SerialNumber { get; set; }
    }
}
