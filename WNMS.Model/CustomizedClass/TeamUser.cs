using System;
using System.Collections.Generic;
using System.Text;

namespace WNMS.Model.CustomizedClass
{
    public class TeamUser
    {
        public int TeamID { get; set; }
        public int UserID { get; set; }
        public string TeamName { get; set; }
        public string Account { get; set; }
        public string NickName { get; set; }
    }
}
