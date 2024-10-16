using MongoDBHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WNMS.Model.CustomizedClass
{
    /// <summary>
    /// 用水量分析报表数据类
    /// </summary>
    public class FLowReport: BaseEntity
    {
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime Time { get; set; }
        /// <summary>
        /// 时间段内最小值
        /// </summary>
        public Nullable<double> Mindata { get; set; }
        /// <summary>
        /// 时间段内最大值
        /// </summary>
        public Nullable<double> Maxdata { get; set; }
        /// <summary>
        /// 时间段内差值
        /// </summary>
        public Nullable<double> Data { get; set; }
  
    }


    /// <summary>
    /// 用水量分析报表数据类
    /// </summary>
    public class FLowReport_Last
    {
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime Time { get; set; }
     
        /// <summary>
        /// 时间段内差值
        /// </summary>
        public Nullable<double> Data { get; set; }

    }
}
