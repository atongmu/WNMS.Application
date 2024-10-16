using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class PartitionInfo
    {
        /// <summary>
        /// 低区
        /// </summary>
        [DisplayName("低区"), DefaultValue("2000,5000")]
        public string LPartition { get; set; }

        /// <summary>
        /// 中区
        /// </summary>
        [DisplayName("中区"), DefaultValue("2500,5500")]
        public string MPartition { get; set; }

        /// <summary>
        /// 高区
        /// </summary>
        [DisplayName("高区"), DefaultValue("3000,6000")]
        public string HPartition { get; set; }

        /// <summary>
        /// 超高区
        /// </summary>
        [DisplayName("超高区"), DefaultValue("3500,6500")]
        public string CHPartition { get; set; }

        /// <summary>
        /// 超超高区
        /// </summary>
        [DisplayName("超超高区"), DefaultValue("4000,7000")]
        public string CCHPartition { get; set; }

    }
}
