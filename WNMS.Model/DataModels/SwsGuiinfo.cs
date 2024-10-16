using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class SwsGuiinfo
    {
        public int Num { get; set; }
        public string Guiname { get; set; }
        public byte PumpNum { get; set; }
        public string PageUrl { get; set; }
        public string ImageUrl { get; set; }
        public byte DeviceType { get; set; }
        public byte EquipType { get; set; }
        public byte IsSum { get; set; }
    }
}
