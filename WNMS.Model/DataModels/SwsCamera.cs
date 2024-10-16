using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class SwsCamera
    {
        public int CameraId { get; set; }
        public string CameraName { get; set; }
        public byte CameraType { get; set; }
        public string Ip { get; set; }
        public int? Port { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string SerialNum { get; set; }
        public string AppKey { get; set; }
        public string AppSecret { get; set; }
        public string Token { get; set; }
        public long? LimitTime { get; set; }
        public int? DoorId { get; set; }
        public int? ChannelNum { get; set; }
        public string Pid { get; set; }
        public string Numbering { get; set; }
        public int StationId { get; set; }
        public string Url { get; set; }
    }
}
