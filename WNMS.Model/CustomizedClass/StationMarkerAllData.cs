using System;
using System.Collections.Generic;
using System.Text;

namespace WNMS.Model.CustomizedClass
{
  public  class StationMarkerAllData
    {
        public int StationID { get; set; }
        public string StationName { get; set; }
        public List<allmapmontorJK> deviceJKs { get; set; }//二供监控数据集合
        public List<zallmapmontorJK> zdeviceJKs { get; set; }//直饮水监控数据集合
        public double Lng { get; set; }
        public double Lat { get; set; }
        public int InType { get; set; }
        public string State { get; set; }//泵房状态 
    }
    public class allmapmontorJK
    {
        public string devicename { get; set; }
        public string datetime { get; set; }
        public long DeviceID { get; set; }

        public byte Partition { get; set; }
        public int RtuID { get; set; }
        public string State { get; set; }//状态
        public KeyValuePair<string, bool> PressIN { get; set; }//进水压力
        public KeyValuePair<string, bool> PressOut { get; set; }//出水压力
        public string inflow { get; set; }//瞬时流量  

    }
    public class zallmapmontorJK
    {
        public string devicename { get; set; }
        public string datetime { get; set; }
        public int Partition { get; set; }
        public int RtuID { get; set; }
        public string State { get; set; }//状态
        public KeyValuePair<string, bool> JPressOut { get; set; } //净水出水压力
        public KeyValuePair<string, bool> YPressOut { get; set; }//原水出水压力

        public string PH { get; set; }//PH
        public string CL { get; set; }//余氯
        public string Turbidity { get; set; }//浊度

    }
}
