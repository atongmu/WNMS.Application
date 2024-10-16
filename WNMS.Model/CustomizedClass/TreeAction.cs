using System;
using System.Collections.Generic;
using System.Text;

namespace WNMS.Model.CustomizedClass
{
    public class TreeAction
    {
        public long id { get; set; }
        public long pId { get; set; }
        public string name { get; set; }
        public string icon { get; set; }
        public bool @checked { get; set; }
        //是否隐藏节点
        public bool isHidden { get; set; }
        //是否保存 权限
        public bool isSave { get; set; }
        public int type { get; set; }
        //是否设备
        public bool isDevice { get; set; }

        //checkbox是否禁用
        public bool chkDisabled { get; set; }
        //是否显示checkbox
        public bool nocheck { get; set; }
        public bool isParent { get; set; }
        //分区
        public byte Partition { get; set; }
        //通讯ID
        public int rtuid { get; set; }
        //其他值
        public string value { get; set; }

    }
}
