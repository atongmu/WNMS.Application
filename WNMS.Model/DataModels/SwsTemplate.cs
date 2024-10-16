using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class SwsTemplate
    {
        public int Id { get; set; }
        public string TemplateName { get; set; }
        public string DataId { get; set; }
        public long UserId { get; set; }
        public int? Classify { get; set; }
        public byte DeviceType { get; set; }
        public bool FocusOn { get; set; }
    }
}
