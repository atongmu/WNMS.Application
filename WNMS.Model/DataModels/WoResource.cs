using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class WoResource
    {
        public long Id { get; set; }
        public long Pid { get; set; }
        public short Type { get; set; }
        public string Path { get; set; }
        public short ResourceType { get; set; }
        public string Suffix { get; set; }
        public string FileName { get; set; }
    }
}
