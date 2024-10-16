using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class SysArea
    {
        public int Id { get; set; }
        public string AreaName { get; set; }
        public int Parents { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? Isdeleted { get; set; }
    }
}
