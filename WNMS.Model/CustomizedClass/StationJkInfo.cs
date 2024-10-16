using System;
using System.Collections.Generic;
using System.Text;
using WNMS.Model.DataModels;

namespace WNMS.Model.CustomizedClass
{
    public class StationJkInfo
    {
        public int StationID { get; set; }
        public string StationName { get; set; }
        public List<montorJK> deviceJKs { get; set; }//监控数据集合
        public string State { get; set; }//泵房状态
        public bool Attention { get; set; }//泵房是否被关注
        public double allinflow { get; set; }//总瞬时流量
        public List<Eventinfo> eventlist { get; set; }//事件集合
        public bool CameraMonitor { get; set; }//视频
        public bool ControlMonitor { get; set; }//控制
    }
    public class montorJK
    {
        public String devicename { get; set; }
        public string datetime { get; set; }
        public long DeviceID { get; set; }
        //public int Partition { get; set; }
        public int RtuID { get; set; }
        public string State { get; set; }//状态
        public KeyValuePair<string, bool> PressSet { get; set; }//设定压力
        public KeyValuePair<string, bool> PressIN { get; set; }//进水压力
        public KeyValuePair<string, bool> PressOut { get; set; }//出水压力
        public string inflow { get; set; }//瞬时流量
        public List<string> pumpState { get; set; }//泵状态，是否旋转
        public string totalflow { get; set; }//累计流量
        public string totalpower { get; set; }//累计电量 
        public List<PumpData> pumpdatas { get; set; }//泵数据集合（频率、流量、运行时间）

    }
    public class PumpData {

        public KeyValuePair<string, bool> frequency { get; set; }//频率
        public KeyValuePair<string, bool> eletric { get; set; }//电流 
        public KeyValuePair<string, bool> runtime { get; set; }//运行时间  
    }

    public class Eventinfo
    {
        public int Rtuid { get; set; }
        public int ID { get; set; }
        public string EventMessage { get; set; }
        public DateTime EventTime { get; set; }
        public float CurrentValue { get; set; }
        public float LimitValue { get; set; }
        public string Unit { get; set; }
        public byte? DataType { get; set; }
        public short EventSource { get; set; }
        public string RegionName { get; set; }
        public int EventLevel { get; set; }
       
    }
    //工艺图数据
    public class montorGuiJK
    {
        public String devicename { get; set; }
        public string datetime { get; set; }
        public long DeviceID { get; set; }
        //public int Partition { get; set; }
        public int RtuID { get; set; }
        public string State { get; set; }//状态 
        public KeyValuePair<string, bool> PressIN { get; set; }//进水压力
        public KeyValuePair<string, bool> PressOut { get; set; }//出水压力
        public string inflow { get; set; }//瞬时流量
        public List<string> pumpState { get; set; }//泵状态，是否旋转
        public string totalflow { get; set; }//累计流量
        public string totalpower { get; set; }//累计电量 
        public List<PumpData> pumpdatas { get; set; }//泵数据集合（频率、流量、运行时间）
        public string PH { get; set; }//PH
        public string CL { get; set; }//余氯
        public string Turbidity { get; set; }//浊度
        public string Humidity { get; set; }//湿度
        public string Noise { get; set; }//噪音
        public string PressureSet { get; set; }//设定压力
        public string LiquidHight { get; set; }//液位高度L_Temperature
        public string Temperature { get; set; }//温度
        public string BWCWWD1 { get; set; }//保温层外温度
        public string BWCNWD2 { get; set; }//保温层内温度
        public string RDXNWD { get; set; }//弱电箱内温度
        #region 新加7泵参数
        public List<string> ValveFault { get; set; }//电动阀状态
        public List<string> ValveValue { get; set; }//电动阀开度
        public KeyValuePair<string, bool> FPressIN { get; set; }//进水压力
        public KeyValuePair<string, bool> FPressOut { get; set; }//出水压力
        public string FPressSet { get; set; }//设定压力
        public string ztotalflow { get; set; }//中站累计流量
        public string zinflow { get; set; }//中站瞬时流量 
        public string ftotalflow { get; set; }//峰林累计流量
        public string finflow { get; set; }//峰林瞬时流量 

        public List<string> voltage { get; set; }//泵的电压
        #endregion


    }

}
