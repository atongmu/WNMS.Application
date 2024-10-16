using System;
using System.Collections.Generic;
using System.Text;
using WNMS.Model;
using WNMS.Model.CustomizedClass;

namespace WNMS.IService
{
   public partial interface ISws_AlarmReportsService : IBaseService
    {
        IEnumerable<AlarmRep> GetAlarmDataByDeviceID(string begindate, string enddate, string deviceid, string type);

        IEnumerable<AlarmDetail> GetAlarmDatas(string deviceid, string type, string time, string itemvalue);
    }
}
