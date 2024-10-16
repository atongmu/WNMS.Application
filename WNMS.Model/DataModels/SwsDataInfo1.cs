using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class SwsDataInfo1
    {
        public int Id { get; set; }
        public short DataId { get; set; }
        public string Cnname { get; set; }
        public string Enname { get; set; }
        public string Unit { get; set; }
        public byte? DataType { get; set; }
        public double DataRatio { get; set; }
        public int DeviceType { get; set; }
    }
}
