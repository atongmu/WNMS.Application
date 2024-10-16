using System;
using System.Collections.Generic;
using System.Text;

namespace WNMS.Model.CustomizedClass
{
   public class StationMapState
    {
        public int StationID { get; set; }
        public string StationName { get; set; } 
        public string State { get; set; }//泵房状态 
        public string UpdateTime { get; set; }//更新时间 
        public double Lng { get; set; }//经度
        public double Lat { get; set; }//纬度
        public double allInFlow { get; set; }//总进水瞬时流量
        public List<mapmontorJK> deviceJKs { get; set; }//监控数据集合
    }
}
