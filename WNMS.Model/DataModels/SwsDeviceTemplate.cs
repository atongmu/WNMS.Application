using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class SwsDeviceTemplate
    {
        public long DeviceId { get; set; }
        public int TemplateId { get; set; }
        public byte DeviceType { get; set; }
    }
}
