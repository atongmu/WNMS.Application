using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WNMS.Utility;
using System.Linq;
using WNMS.Model.DataModels;
using WNMS.Model.CustomizedClass;
using System.IO;
using System.Data;
using WNMS.Model;

namespace WNMS.Service
{
  public partial class Sws_AlarmReportsService : BaseService, IService.ISws_AlarmReportsService
    {
        public Sws_AlarmReportsService(DbContext content) : base(content)
        {

        }
        public IEnumerable<AlarmRep> GetAlarmDataByDeviceID(string begindate, string enddate, string deviceid,string type)
        {
            string sql = "";
           SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@deviceid",deviceid),
                new SqlParameter("@begindate",begindate),
                new SqlParameter("@enddate",enddate) 
        };
            if (type == "day")
            {
                sql = @" SELECT CONVERT(VARCHAR(13),e.EventTime,120)+':00:00' AS EventTime,COUNT(e.ID) Num,sa.DeviceID,sa.DeviceName,sd.ItemValue,sd.ItemName  from [dbo].[Sws_EventHistory] e
  left join [dbo].[Sws_RTUInfo] r on e.RTUID=r.RTUID left join Sys_DataItemDetail sd on e.EventLevel = sd.ItemValue and sd.F_ItemId = 3 left join (select RtuID,StationID,DeviceName,DeviceID  from [dbo].[Sws_DeviceInfo02]  union all (select RtuID,StationID,DeviceName,DeviceID  from [dbo].[Sws_DeviceInfo01])) sa  on sa.RTUID = e.RTUID
  where  sa.DeviceID=@deviceid AND e.EventTime>=@begindate and e.EventTime<@enddate GROUP BY  CONVERT(varchar(13),e.EventTime,120), sa.DeviceID,sa.DeviceName,sd.ItemValue,sd.ItemName  ORDER BY CONVERT(VARCHAR(13),e.EventTime,120),sd.ItemValue";
            }
            else if (type == "month")
            {
                sql = @" SELECT CONVERT(VARCHAR(10),e.EventTime,120) AS EventTime,COUNT(e.ID) Num,sa.DeviceID,sa.DeviceName,sd.ItemValue,sd.ItemName  from [dbo].[Sws_EventHistory] e
  left join [dbo].[Sws_RTUInfo] r on e.RTUID=r.RTUID left join Sys_DataItemDetail sd on e.EventLevel = sd.ItemValue and sd.F_ItemId = 3 left join (select RtuID,StationID,DeviceName,DeviceID  from [dbo].[Sws_DeviceInfo02]  union all (select RtuID,StationID,DeviceName,DeviceID  from [dbo].[Sws_DeviceInfo01])) sa  on sa.RTUID = e.RTUID
  where  sa.DeviceID=@deviceid AND e.EventTime>=@begindate and e.EventTime<@enddate GROUP BY  CONVERT(varchar(10),e.EventTime,120), sa.DeviceID,sa.DeviceName,sd.ItemValue,sd.ItemName  ORDER BY CONVERT(VARCHAR(10),e.EventTime,120),sd.ItemValue";
            }
            else if (type == "year")
            {
                sql = @" SELECT CONVERT(VARCHAR(7),e.EventTime,120) AS EventTime,COUNT(e.ID) Num,sa.DeviceID,sa.DeviceName,sd.ItemValue,sd.ItemName  from [dbo].[Sws_EventHistory] e
  left join [dbo].[Sws_RTUInfo] r on e.RTUID=r.RTUID left join Sys_DataItemDetail sd on e.EventLevel = sd.ItemValue and sd.F_ItemId = 3 left join (select RtuID,StationID,DeviceName,DeviceID  from [dbo].[Sws_DeviceInfo02]  union all (select RtuID,StationID,DeviceName,DeviceID  from [dbo].[Sws_DeviceInfo01])) sa  on sa.RTUID = e.RTUID
  where  sa.DeviceID=@deviceid AND e.EventTime>=@begindate and e.EventTime<@enddate GROUP BY  CONVERT(varchar(7),e.EventTime,120), sa.DeviceID,sa.DeviceName,sd.ItemValue,sd.ItemName  ORDER BY CONVERT(VARCHAR(7),e.EventTime,120),sd.ItemValue";
            }
            else
            { 
                sql = @" SELECT CONVERT(VARCHAR(10),e.EventTime,120) AS EventTime,COUNT(e.ID) Num,sa.DeviceID,sa.DeviceName,sd.ItemValue,sd.ItemName from [dbo].[Sws_EventHistory] e
  left join [dbo].[Sws_RTUInfo] r on e.RTUID=r.RTUID left join Sys_DataItemDetail sd on e.EventLevel = sd.ItemValue and sd.F_ItemId = 3 left join (select RtuID,StationID,DeviceName,DeviceID  from [dbo].[Sws_DeviceInfo02]  union all (select RtuID,StationID,DeviceName,DeviceID  from [dbo].[Sws_DeviceInfo01])) sa  on sa.RTUID = e.RTUID
  where  sa.DeviceID=@deviceid AND e.EventTime>=@begindate and e.EventTime<@enddate GROUP BY  CONVERT(varchar(10),e.EventTime,120), sa.DeviceID,sa.DeviceName,sd.ItemValue,sd.ItemName  ORDER BY CONVERT(VARCHAR(10),e.EventTime,120),sd.ItemValue"; 
            }

            var query = this.Context.Database.SqlQuery<AlarmRep>(sql, sp);
            return query;
        }

        public IEnumerable<AlarmDetail> GetAlarmDatas(string deviceid, string type, string time, string itemvalue)
        {
            string sql = "";
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@deviceid",deviceid),
                 new SqlParameter("@time",time),
                new SqlParameter("@itemvalue",itemvalue)
        };
            if (type == "day")
            {
                sql = @"   SELECT e.RTUID, CONVERT(VARCHAR(20),e.EventTime,120) EventTime,e.EventMessage,e.EventType,e.State,e.CurrentValue,e.LimitValue,CONVERT(VARCHAR(20),e.EndDate,120) EndDate , CASE WHEN e.EndDate IS NULL THEN '--' ELSE CAST(CAST(DATEDIFF(ss, e.EventTime, e.EndDate)/ (60*60*24) AS INT) AS VARCHAR(20)) +'天'+CAST(CAST(DATEDIFF(ss, e.EventTime,e.EndDate)%86400/3600 AS INT) AS VARCHAR(20)) +'时'+CAST(CAST(DATEDIFF(ss, e.EventTime,e.EndDate)%3600/60 AS INT) AS VARCHAR(20))+'分'+CAST(CAST(DATEDIFF(ss, e.EventTime,e.EndDate)%60 AS INT) AS VARCHAR(20))+'秒' end  Duration,sa.DeviceName,r.DeviceType FROM [dbo].[Sws_EventHistory] e
  left join [dbo].[Sws_RTUInfo] r on e.RTUID=r.RTUID left join Sys_DataItemDetail sd on e.EventLevel = sd.ItemValue and sd.F_ItemId = 3 left join (select RtuID,StationID,DeviceName,DeviceID  from [dbo].[Sws_DeviceInfo02]  union all (select RtuID,StationID,DeviceName,DeviceID  from [dbo].[Sws_DeviceInfo01])) sa  on sa.RTUID = e.RTUID
  where  sa.DeviceID = @deviceid AND CONVERT(VARCHAR(13),e.EventTime,120)=CONVERT(VARCHAR(13),@time ,120) and sd.ItemValue=@itemvalue  ";
            }
            else if (type == "month")
            {
                sql = @"  SELECT e.RTUID, CONVERT(VARCHAR(20),e.EventTime,120) EventTime,e.EventMessage,e.EventType,e.State,e.CurrentValue,e.LimitValue,CONVERT(VARCHAR(20),e.EndDate,120) EndDate , CASE WHEN e.EndDate IS NULL THEN '--' ELSE CAST(CAST(DATEDIFF(ss, e.EventTime, e.EndDate)/ (60*60*24) AS INT) AS VARCHAR(20)) +'天'+CAST(CAST(DATEDIFF(ss, e.EventTime,e.EndDate)%86400/3600 AS INT) AS VARCHAR(20)) +'时'+CAST(CAST(DATEDIFF(ss, e.EventTime,e.EndDate)%3600/60 AS INT) AS VARCHAR(20))+'分' +CAST(CAST(DATEDIFF(ss, e.EventTime,e.EndDate)%60 AS INT) AS VARCHAR(20))+'秒' end  Duration,sa.DeviceName,r.DeviceType FROM [dbo].[Sws_EventHistory] e
  left join [dbo].[Sws_RTUInfo] r on e.RTUID=r.RTUID left join Sys_DataItemDetail sd on e.EventLevel = sd.ItemValue and sd.F_ItemId = 3 left join (select RtuID,StationID,DeviceName,DeviceID  from [dbo].[Sws_DeviceInfo02]  union all (select RtuID,StationID,DeviceName,DeviceID  from [dbo].[Sws_DeviceInfo01])) sa  on sa.RTUID = e.RTUID
  where  sa.DeviceID = @deviceid AND CONVERT(VARCHAR(10),e.EventTime,120)=@time and sd.ItemValue=@itemvalue  ";
            }
            else if (type == "year")
            {
                sql = @"  SELECT e.RTUID, CONVERT(VARCHAR(20),e.EventTime,120) EventTime,e.EventMessage,e.EventType,e.State,e.CurrentValue,e.LimitValue,CONVERT(VARCHAR(20),e.EndDate,120) EndDate , CASE WHEN e.EndDate IS NULL THEN '--' ELSE CAST(CAST(DATEDIFF(ss, e.EventTime, e.EndDate)/ (60*60*24) AS INT) AS VARCHAR(20)) +'天'+CAST(CAST(DATEDIFF(ss, e.EventTime,e.EndDate)%86400/3600 AS INT) AS VARCHAR(20)) +'时'+CAST(CAST(DATEDIFF(ss, e.EventTime,e.EndDate)%3600/60 AS INT) AS VARCHAR(20))+'分' +CAST(CAST(DATEDIFF(ss, e.EventTime,e.EndDate)%60 AS INT) AS VARCHAR(20))+'秒' end  Duration,sa.DeviceName,r.DeviceType FROM [dbo].[Sws_EventHistory] e
  left join [dbo].[Sws_RTUInfo] r on e.RTUID=r.RTUID left join Sys_DataItemDetail sd on e.EventLevel = sd.ItemValue and sd.F_ItemId = 3 left join (select RtuID,StationID,DeviceName,DeviceID  from [dbo].[Sws_DeviceInfo02]  union all (select RtuID,StationID,DeviceName,DeviceID  from [dbo].[Sws_DeviceInfo01])) sa  on sa.RTUID = e.RTUID
  where sa.DeviceID = @deviceid AND CONVERT(VARCHAR(7),e.EventTime,120)=@time and sd.ItemValue=@itemvalue  ";
            }
            else
            {
                sql = @"   SELECT e.RTUID, CONVERT(VARCHAR(20),e.EventTime,120) EventTime,e.EventMessage,e.EventType,e.State,e.CurrentValue,e.LimitValue,CONVERT(VARCHAR(20),e.EndDate,120) EndDate , CASE WHEN e.EndDate IS NULL THEN '--' ELSE CAST(CAST(DATEDIFF(ss, e.EventTime, e.EndDate)/ (60*60*24) AS INT) AS VARCHAR(20)) +'天'+CAST(CAST(DATEDIFF(ss, e.EventTime,e.EndDate)%86400/3600 AS INT) AS VARCHAR(20)) +'时'+CAST(CAST(DATEDIFF(ss, e.EventTime,e.EndDate)%3600/60 AS INT) AS VARCHAR(20))+'分' +CAST(CAST(DATEDIFF(ss, e.EventTime,e.EndDate)%60 AS INT) AS VARCHAR(20))+'秒' end  Duration,sa.DeviceName,r.DeviceType FROM [dbo].[Sws_EventHistory] e
  left join [dbo].[Sws_RTUInfo] r on e.RTUID=r.RTUID left join Sys_DataItemDetail sd on e.EventLevel = sd.ItemValue and sd.F_ItemId = 3 left join (select RtuID,StationID,DeviceName,DeviceID  from [dbo].[Sws_DeviceInfo02]  union all (select RtuID,StationID,DeviceName,DeviceID  from [dbo].[Sws_DeviceInfo01])) sa  on sa.RTUID = e.RTUID
  where  sa.DeviceID = @deviceid AND CONVERT(VARCHAR(10),e.EventTime,120)=@time and sd.ItemValue=@itemvalue  ";
            }

            var query = this.Context.Database.SqlQuery<AlarmDetail>(sql, sp);
            return query;
        }
    }
}
