using MongoDBHelper;
using System;
using System.Collections.Generic;
using System.Text;

namespace WNMS.Model.CustomizedClass
{
    public class RtuJKInfo: BaseEntity
    {
      
        /// <summary>
        /// 获取通讯ID
        /// </summary>
        public int RTUID { get; set; }
        /// <summary>
        /// 设备分类
        /// </summary>
      
        /// <summary>
        /// 获取 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 获取 模拟量
        /// </summary>
        public Dictionary<string, object> AnalogValues { get; set; }

        /// <summary>
        /// 获取 开关量
        /// </summary>
        public Dictionary<string, object> DigitalValues { get; set; }
        public bool EventState { get; set; }


    }
}
