using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class WoFbtemplate
    {
        public int TemplateId { get; set; }
        public string TemplateName { get; set; }
        public string DataId { get; set; }
        public byte InspectObject { get; set; }
        public bool IsTemplate { get; set; }
    }
}
