using System;
using System.Collections.Generic;
using System.Text;

namespace WNMS.Model.CustomizedClass
{
    public class StationMarkerData
    {
        public int StationID { get; set; }
        public string StationName { get; set; }
        public List<mapmontorJK> deviceJKs { get; set; }//监控数据集合
        public double Lng { get; set; }
        public double Lat { get; set; }
        public string State { get; set; }//泵房状态 
        public string PH { get; set; }//PH
        public string CL { get; set; }//余氯
        public string Turbidity { get; set; }//浊度
        public double allInFlow { get; set; }//累计电量
    }
    public class mapmontorJK
    {
        public String devicename { get; set; }
        public string datetime { get; set; }
        public long DeviceID { get; set; }

        public byte Partition { get; set; }
        public int RtuID { get; set; }
        public string State { get; set; }//状态
        public KeyValuePair<string, bool> PressIN { get; set; }//进水压力
        public KeyValuePair<string, bool> PressOut { get; set; }//出水压力
        public string inflow { get; set; }//瞬时流量  

    }

    public class shuixiangjk
    {
        public double sx1 { get; set; }
        public double sx2 { get; set; }
        public double sx3 { get; set; } 
        public double sx4 { get; set; } 
        public double sx5 { get; set; } 
    }
}
