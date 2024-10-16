using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class WoFeedBackInfo
    {
        public int Id { get; set; }
        public string FeedBackName { get; set; }
        public string Unit { get; set; }
        public int? Type { get; set; }
    }
}
