using System;
using System.Collections.Generic;
using System.Text;

namespace WNMS.Model.CustomizedClass
{
    public class GUIInfo
    {
        public int Num { get; set; }
        public string GUIName { get; set; }
        public byte PumpNum { get; set; }
        public string PageURL { get; set; }
        public string ImageURL { get; set; }
        public byte DeviceType { get; set; }
        public byte EquipType { get; set; }
        public string DeviceTypeName { get; set; }
    }
}
