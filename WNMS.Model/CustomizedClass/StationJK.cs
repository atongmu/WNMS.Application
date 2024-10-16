using MongoDBHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WNMS.Model.CustomizedClass;

namespace WNMS.Model.CustomizedClass
{
    public class StationJK 
    {
        /// <summary>
        /// 泵站ID
        /// </summary>
        public int StationID { get; set; }

        /// <summary>
        /// 泵站名称
        /// </summary>
        public string StationName { get; set; }

        /// <summary>
        /// 经度
        /// </summary>
        public double Lng { get; set; }

        /// <summary>
        /// 纬度
        /// </summary>
        public double Lat { get; set; }

        /// <summary>
        /// 是否关注
        /// </summary>
        public bool FocusOn { get; set; }



        /// <summary>
        /// 是否报警
        /// </summary>
        public bool IsAlarm { get; set; }

        /// <summary>
        /// 是否离线
        /// </summary>
        public bool IsOffline { get; set; }

        /// <summary>
        /// 设备监控信息
        /// </summary>
        public List<DeviceJK> lstDeviceJK { get; set; }


        /// <summary>
        /// 报警设备监控信息
        /// </summary>
        public List<AlarmDeviceInfo> lstAlarmDeviceInfo { get; set; }
    }


    /// <summary>
    /// 手机端首页使用
    /// </summary>
    public class StationJK_SY
    {
        /// <summary>
        /// 泵站ID
        /// </summary>
        public int StationID { get; set; }

        /// <summary>
        /// 泵站名称
        /// </summary>
        public string StationName { get; set; }

        /// <summary>
        /// 是否关注
        /// </summary>
        public bool FocusOn { get; set; }


        /// <summary>
        /// 设备监控信息
        /// </summary>
        public List<DeviceJK_SY> lstDeviceJK { get; set; }

    }

    /// <summary>
    /// 手机端首页使用,报警
    /// </summary>
    public class StationJK_Alarm
    {
        /// <summary>
        /// 泵站ID
        /// </summary>
        public int StationID { get; set; }

        /// <summary>
        /// 泵站名称
        /// </summary>
        public string StationName { get; set; }

        /// <summary>
        /// 是否关注
        /// </summary>
        public bool FocusOn { get; set; }

        /// <summary>
        /// 报警设备监控信息
        /// </summary>
        public List<AlarmDeviceInfo_SS> lstAlarmDeviceInfo { get; set; }

    }

}
