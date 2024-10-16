using MongoDBHelper;
using System;
using System.Collections.Generic;
using System.Text;

namespace WNMS.Model.CustomizedClass
{
    /// <summary>
    /// 历史数据
    /// </summary>
    public class HistoryJKData : BaseEntity
    {
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// 模拟量
        /// </summary>
        public Dictionary<string, object> AnalogValues { get; set; }
        /// <summary>
        /// 开关量
        /// </summary>
        public Dictionary<string, object> DigitalValues { get; set; }
    }
}
