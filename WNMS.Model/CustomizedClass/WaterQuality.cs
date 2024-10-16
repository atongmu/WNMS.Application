using MongoDBHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class WaterQuality
    {
        /// <summary>
        /// 浊度
        /// </summary>
        public double Turbidity { get; set; }

        /// <summary>
        /// 余氯
        /// </summary>
        public double CL { get; set; }

        /// <summary>
        /// PH值
        /// </summary>
        public double PH { get; set; }

        /// <summary>
        /// 温度
        /// </summary>
        public double Temperature { get; set; }

        /// <summary>
        /// 湿度
        /// </summary>
        public double Humidity { get; set; }

        /// <summary>
        /// 噪音
        /// </summary>
        public double Noise { get; set; }
    }

}
