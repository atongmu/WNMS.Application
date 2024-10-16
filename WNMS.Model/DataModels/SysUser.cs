using System;
using System.Collections.Generic;

namespace WNMS.Model.DataModels
{
    public partial class SysUser
    {
        public int UserId { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public string NickName { get; set; }
        public string HeadIcon { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsLock { get; set; }
        public int Department { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string WeChatKey { get; set; }
        public long LitmitTime { get; set; }
        public byte ErrorTimes { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public string Imei { get; set; }
        public DateTime? CreateTime { get; set; }
        public bool Gender { get; set; }
        public bool? IsEnable { get; set; }
        public string Remark { get; set; }
        public string SerialNumber { get; set; }
        public string Token { get; set; }
    }
}
