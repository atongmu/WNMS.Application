using System;
using System.Collections.Generic;

namespace WNMS.Model.CustomizedClass
{
    public partial class Pump
    {
        /// <summary>
        /// 运行方式
        /// </summary>
        public string PState { get; set; }
        /// <summary>
        /// 运行状态
        /// </summary>
        public string PFault { get; set; }
        /// <summary>
        /// 电流
        /// </summary>
        public double Electric { get; set; }

        /// <summary>
        /// 频率
        /// </summary>
        public double Frequency { get; set; }
        /// <summary>
        /// 泵的名称
        /// </summary>
        public string PumpName { get; set; }

    }

}
