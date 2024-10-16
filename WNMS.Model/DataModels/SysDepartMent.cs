using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class SysDepartMent
    {
        public int DepartmentId { get; set; }
        public int Pid { get; set; }
        public string DptName { get; set; }
        public int Type { get; set; }
        public string Manager { get; set; }
        public string OuterPhone { get; set; }
        public string InnerPhone { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
        public int Sort { get; set; }
        public string Reamrk { get; set; }
    }
}
