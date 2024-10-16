using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class SysModule
    {
        public int ModuleNum { get; set; }
        public string ModuleName { get; set; }
        public string Url { get; set; }
        public string HttpMethod { get; set; }
        public int Pnum { get; set; }
        public string Icon { get; set; }
        public bool IsEnable { get; set; }
        public int Sort { get; set; }
        public int Target { get; set; }
        public bool IsMenu { get; set; }
    }
}
