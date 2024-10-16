using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class SysLog
    {
        public int Id { get; set; }
        public DateTime LogDate { get; set; }
        public string LogLevel { get; set; }
        public string LogLogger { get; set; }
        public string LogAction { get; set; }
        public byte LogType { get; set; }
        public string LogMessage { get; set; }
        public string LogUrl { get; set; }
        public string UserName { get; set; }
        public string Ip { get; set; }
    }
}
