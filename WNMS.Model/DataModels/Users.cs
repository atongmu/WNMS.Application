using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class Users
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public string UserFullName { get; set; }
        public string UserEmail { get; set; }
        public bool IsAccountLocked { get; set; }
        public bool? IsAdministrator { get; set; }
        public int? UserLevel { get; set; }
        public DateTime? DateCreated { get; set; }
        public string Describe { get; set; }
        public string Region { get; set; }
        public string Hp { get; set; }
        public string Imei { get; set; }
        public string WeChatId { get; set; }
        public string Phone { get; set; }
    }
}
