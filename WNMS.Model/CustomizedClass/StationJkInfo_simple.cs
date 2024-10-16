using System;
using System.Collections.Generic;
using System.Text;

namespace WNMS.Model.CustomizedClass
{
   public  class StationJkInfo_simple
    {
        public int StationID { get; set; }
        public string StationName { get; set; }
       
        public string State { get; set; }//泵房状态
        public bool Attention { get; set; }//泵房是否被关注
    }
}
