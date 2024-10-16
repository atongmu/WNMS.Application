using System;
using System.Collections.Generic;
using System.Text;

namespace WNMS.Model.CustomizedClass
{
    public class CameraInfo
    {
        public int CameraID { get; set; }
        public string CameraName { get; set; }
        public byte CameraType { get; set; }
        public string IP { get; set; }
        public int? Port { get; set; }
        public int? ChannelNum { get; set; }
        public int StationID { get; set; }
        public string StationName { get; set; }
    }
}
