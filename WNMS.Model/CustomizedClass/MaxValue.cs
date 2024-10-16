using System;
using System.Collections.Generic;
using System.Text;

namespace WNMS.Model.CustomizedClass
{
    public class MaxValue
    {
        public double maxFlow { get; set; }//最大流量
        public double minFlow { get; set; }//最小流量
        public double maxPower { get; set; }//最大能耗
        public double minPower { get; set; }//最小能耗
        public double consump { get; set; }//单吨能耗
        public double FlucNum { get; set; }//波动次数
        public double HighHour { get; set; }//高频运行时间
        public double LowHour { get; set; }//低频运行时间
    }
}
