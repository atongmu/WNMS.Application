using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class GdResource
    {
        public long Id { get; set; }
        public long Pid { get; set; }
        public byte Type { get; set; }
        public string Path { get; set; }
        public byte ResourceType { get; set; }
        public string Suffix { get; set; }
        public string FileName { get; set; }
    }
}
