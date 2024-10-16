using System;
using System.Collections.Generic;
using System.Text;

namespace WNMS.Model.CustomizedClass
{
    public class TaskPush
    {
        public long UserID { get; set; }
        public long PlanID { get; set; }
        public string Content { get; set; }
    }
}
