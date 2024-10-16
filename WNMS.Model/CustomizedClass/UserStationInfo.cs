using System;
using System.Collections.Generic;

namespace WNMS.Model.CustomizedClass
{
    public partial class UserStationInfo
    {
        public int UserID { get; set; }
        public string StationName { get; set; }
        public int StationID { get; set; }
        public bool FocusOn { get; set; }
        public double Lng { get; set; }
        public double Lat { get; set; }

    }

    public partial class UserStationAndDeviceInfo
    {
        public int UserID { get; set; }
        public string StationName { get; set; }
        public int StationID { get; set; }
        public bool FocusOn { get; set; }

        /// <summary>
        /// 获取设备ID
        /// </summary>
        public int RTUID { get; set; }
        /// <summary>
        /// 通讯编号
        /// </summary>
        public string DeviceID { get; set; }
        /// <summary>
        /// 获取设备名称
        /// </summary>
        public string DeviceName { get; set; }

    }
}
