using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class SwsAccessControl
    {
        public int DoorId { get; set; }
        public byte Brand { get; set; }
        public string Num { get; set; }
        public string AccessName { get; set; }
        public string Ip { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public int StationId { get; set; }
        public string AppKey { get; set; }
        public string Secret { get; set; }
    }
}
