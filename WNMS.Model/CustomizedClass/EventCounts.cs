using System;
using System.Collections.Generic;

namespace WNMS.Model.CustomizedClass
{
    public partial class EventDateCounts
    {
        public long EquipID { get; set; }
    
        public string EventDate { get; set; }
        public int EventCount { get; set; }
      
    }

    public partial class EventMonthCounts
    {
        public long EquipID { get; set; }

        public int EventMonth { get; set; }
        public int EventCount { get; set; }

    }

    public partial class EventYearCounts
    {
        public long EquipID { get; set; }

        public int EventYear { get; set; }
        public int EventCount { get; set; }

    }


    public partial class EventLevelCounts
    {
        public long EquipID { get; set; }

        public byte EventLevel { get; set; }
        public int EventCount { get; set; }

    }
}
