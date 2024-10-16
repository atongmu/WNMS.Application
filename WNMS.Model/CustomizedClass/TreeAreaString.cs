using System;
using System.Collections.Generic;
using System.Text;

namespace WNMS.Model.CustomizedClass
{
    public class TreeAreaString
    {
        public string id { get; set; }
        public string pId { get; set; }
        public string name { get; set; }
        public bool @checked { get; set; }
        public string icon { get; set; }
        public string type { get; set; }
        public long? Director { get; set; }
        public string DirectorName { get; set; }
    }
}
