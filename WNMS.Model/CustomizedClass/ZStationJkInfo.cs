using System;
using System.Collections.Generic;
using System.Text;

namespace WNMS.Model.CustomizedClass
{
    public class ZStationJkInfo
    {
        public int StationID { get; set; }
        public string StationName { get; set; }
        public List<zmontorJK> deviceJKs { get; set; }//监控数据集合
        public string State { get; set; }//泵房状态
        public bool Attention { get; set; }//泵房是否被关注 
        public List<Eventinfo> eventlist { get; set; }//事件集合
        public bool CameraMonitor { get; set; }//视频
        public bool ControlMonitor { get; set; }//控制
    }
    public class zmontorJK
    {
        public String devicename { get; set; }
        public string datetime { get; set; }
        public long DeviceID { get; set; }
        //public int Partition { get; set; }
        public int RtuID { get; set; }
        public string State { get; set; }//状态
        public KeyValuePair<string, bool> JPressOut { get; set; } //净水出水压力
        public KeyValuePair<string, bool> YPressOut { get; set; }//原水出水压力
        public string GPumpState { get; set; }//高压泵状态
        public string PH { get; set; }//PH
        public string CL { get; set; }//余氯
        public string Turbidity { get; set; }//浊度
        public List<ZPumpData> pumpdatas { get; set; }//泵数据集合（频率、流量、运行时间）
        public List<string> pumpState { get; set; }//泵状态，是否旋转
        public string YLevel { get; set; }//原水箱液位
        public string JLevel { get; set; }//净水箱液位
        public string JSetpressure { get; set; }//净设定压力
        public string Conductivity { get; set; }//电导率
        public string Orp { get; set; }//ORP
        public string Salinity { get; set; }//盐度
        public string Oxygen { get; set; }//溶氧量


    }
    public class ZPumpData
    {

        public KeyValuePair<string, bool> frequency { get; set; }//频率
        public KeyValuePair<string, bool> eletric { get; set; }//电流

        public KeyValuePair<string, bool> runtime { get; set; }//运行时间
    }

    //工艺图数据
    public class montorZGuiJK
    {
        public String devicename { get; set; }
        public string datetime { get; set; }
        public long DeviceID { get; set; }
        //public int Partition { get; set; }
        public int RtuID { get; set; }
        public string State { get; set; }//状态
        public KeyValuePair<string, bool> JPressOut { get; set; } //净水出水压力
        public KeyValuePair<string, bool> YPressOut { get; set; }//原水出水压力
        public string GPumpState { get; set; }//高压泵状态
        public string PH { get; set; }//PH
        public string CL { get; set; }//余氯
        public string Turbidity { get; set; }//浊度
        public List<ZPumpData> pumpdatas { get; set; }//泵数据集合（频率、流量、运行时间）
        public List<string> pumpState { get; set; }//泵状态，是否旋转
        public string YLevel { get; set; }//原水箱液位
        public string JLevel { get; set; }//净水箱液位
        public string JSetpressure { get; set; }//净设定压力
        public string Conductivity { get; set; }//电导率
        public string Orp { get; set; }//ORP
        public string Salinity { get; set; }//盐度
        public string Oxygen { get; set; }//溶氧量
        public string  Hardness { get; set; }//硬度

    }
}
