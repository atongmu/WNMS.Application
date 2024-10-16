using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class SysModuleButton
    {
        public int ModuleButtonId { get; set; }
        public int ModuleId { get; set; }
        public int Pid { get; set; }
        public string ButtonName { get; set; }
        public string ButtonClass { get; set; }
        public string ButtionIcon { get; set; }
        public string ButtonId { get; set; }
        public bool IsEnable { get; set; }
        public int Sort { get; set; }
        public string Url { get; set; }
        public string HttpMethod { get; set; }
    }
}
