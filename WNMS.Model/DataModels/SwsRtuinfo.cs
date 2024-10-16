using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class SwsRtuinfo
    {
        public int Rtuid { get; set; }
        public string DeviceId { get; set; }
        public byte ComAddress { get; set; }
        public string ComType { get; set; }
        public string DeviceType { get; set; }
        public string Priority { get; set; }
        public byte[] PluginFile { get; set; }
        public bool ActivelySent { get; set; }
        public int? Ipport { get; set; }
        public int StationId { get; set; }
    }
}
