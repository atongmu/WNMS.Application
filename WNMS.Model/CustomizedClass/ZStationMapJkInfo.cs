using System;
using System.Collections.Generic;
using System.Text;

namespace WNMS.Model.CustomizedClass
{
    public class ZStationMapJkInfo
    {
        public int StationID { get; set; }
        public string StationName { get; set; }
        public List<zmapmontorJK> deviceJKs { get; set; }//监控数据集合
        public string State { get; set; }//泵房状态 
        public double Lng { get; set; }//经度
        public double Lat { get; set; }//纬度
        public string PH { get; set; }//PH
        public string CL { get; set; }//余氯
        public string Turbidity { get; set; }//浊度
        public string UpdateTime { get; set; }//更新时间 
    }
    public class zmapmontorJK
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
