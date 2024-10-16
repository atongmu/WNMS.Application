

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WNMS.Utility;
using System.Linq;
using WNMS.Model.CustomizedClass;
using WNMS.Model.DataModels;

namespace WNMS.Service
{
    public partial class Sws_EventHistoryService : BaseService, IService.ISws_EventHistoryService
    {
        public IEnumerable<AlarmDeviceInfo> LoadEventHistoryInfo(long EquipID)
        {
            SqlParameter[] sp = new SqlParameter[]
             {
                new SqlParameter("@EquipID",EquipID)
             };

            var query = this.Context.Database.SqlQuery<AlarmDeviceInfo>("exec GetEventHistoryInfoByEquipID @EquipID", sp);
            return query;
        }
        public IEnumerable<dynamic> LoadEventHistoryInfoByPage(long EquipID, int startindex, int pageSize, ref int Totalcount)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@startindex",startindex),
                new SqlParameter("@pageSize",pageSize),
                new SqlParameter("@filterString",EquipID),
                new SqlParameter("@Totalcount",Totalcount){Direction=System.Data.ParameterDirection.Output,DbType=System.Data.DbType.Int32}
            };
            var query = this.Context.Database.SqlQuery_Dic("exec GetEventHistoryInfoByPage @startindex,@pageSize,@filterString,@Totalcount Output", sp).ToList();
            Totalcount = Convert.ToInt32(sp[3].Value);
            return query;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="EquipID"></param>
        /// <returns></returns>
        public IEnumerable<EventDateCounts> LoadEventDateCountsByEquipID(long EquipID, DateTime BeginDate, DateTime EndDate)
        {
            SqlParameter[] sp = new SqlParameter[]
             {
                new SqlParameter("@EquipID",EquipID),
                new SqlParameter("@BeginDate",BeginDate),
                new SqlParameter("@EndDate",EndDate)
             };

            var query = this.Context.Database.SqlQuery<EventDateCounts>("exec GetEventDateCountsByEquipID @EquipID,@BeginDate,@EndDate", sp);
            return query;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="EquipID"></param>
        /// <returns></returns>
        public IEnumerable<EventMonthCounts> LoadEventMonthCountsByEquipID(long EquipID, DateTime BeginDate, DateTime EndDate)
        {
            SqlParameter[] sp = new SqlParameter[]
             {
                new SqlParameter("@EquipID",EquipID),
                new SqlParameter("@BeginDate",BeginDate),
                new SqlParameter("@EndDate",EndDate)
             };

            var query = this.Context.Database.SqlQuery<EventMonthCounts>("exec GetEventMonthCountsByEquipID @EquipID,@BeginDate,@EndDate", sp);
            return query;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="EquipID"></param>
        /// <returns></returns>
        public IEnumerable<EventYearCounts> LoadEventYearCountsByEquipID(long EquipID, DateTime BeginDate, DateTime EndDate)
        {
            SqlParameter[] sp = new SqlParameter[]
             {
                new SqlParameter("@EquipID",EquipID),
                new SqlParameter("@BeginDate",BeginDate),
                new SqlParameter("@EndDate",EndDate)
             };

            var query = this.Context.Database.SqlQuery<EventYearCounts>("exec GetEventYearCountsByEquipID @EquipID,@BeginDate,@EndDate", sp);
            return query;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="EquipID"></param>
        /// <returns></returns>
        public IEnumerable<EventLevelCounts> LoadEventLevelCountsByEquipID(long EquipID, DateTime BeginDate, DateTime EndDate)
        {
            SqlParameter[] sp = new SqlParameter[]
             {
                new SqlParameter("@EquipID",EquipID),
                new SqlParameter("@BeginDate",BeginDate),
                new SqlParameter("@EndDate",EndDate)
             };

            var query = this.Context.Database.SqlQuery<EventLevelCounts>("exec GetEventLevelCountsByEquipID @EquipID,@BeginDate,@EndDate", sp);
            return query;
        }
        #region 报警历史数据

        public IEnumerable<dynamic> QueryEventTable(bool IsAdmin, int pageindex, int pagesize, string beginTime, string endTime, string type, string sortName, string order, int userid, ref int Totalcount)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@IsAdmin",IsAdmin),
                new SqlParameter("@beginTime",beginTime),
                new SqlParameter("@endTime",endTime),
                new SqlParameter("@type",type),
                new SqlParameter("@startindex",pageindex),
                new SqlParameter("@pageSize",pagesize),
                new SqlParameter("@sortName",sortName),
                new SqlParameter("@orderItems",order),
                new SqlParameter("@userid",userid),
                new SqlParameter("@Totalcount",Totalcount){Direction=System.Data.ParameterDirection.Output,DbType=System.Data.DbType.Int32}
            };
            var query = this.Context.Database.SqlQuery_Dic("exec QueryNewEventHistory @IsAdmin,@beginTime,@endTime,@type,@startindex,@pageSize,@sortName,@orderItems,@userid,@Totalcount Output", sp).ToList();

            Totalcount = Convert.ToInt32(sp[9].Value);
            return query;
        }
        #endregion
        #region  报警分析

        //查询排行榜数据
        public IEnumerable<EventRanking> GetRankingData(DateTime beginDate, DateTime endDate, List<int> rtuIDs, int pageSize, int pageIndex, ref int totalCount)
        {
            var query = (from r in
                             from e in Context.Set<SwsEventHistory>()
                             where e.EventTime > beginDate && e.EventTime < endDate && rtuIDs.Contains(e.Rtuid)
                             //join r in Context.Set<SwsRtuinfo>() on e.Rtuid equals r.Rtuid into er
                             //from era in er.DefaultIfEmpty()
                             group e by e.Rtuid into g
                             select new { Rtuid = g.Key, Count = g.Count() }
                         join st in Context.Set<SwsRtuinfo>() on r.Rtuid equals st.Rtuid into str
                         from stra in str.DefaultIfEmpty()
                         join s in Context.Set<SwsStation>() on stra.StationId equals s.StationId into rs
                         from rsa in rs.DefaultIfEmpty()
                         orderby r.Count descending
                         select new EventRanking
                         {
                             Rtuid = r.Rtuid,
                             StationName = rsa.StationName,
                             Count = r.Count
                         }).ToList();
            totalCount = query.Count();
            query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize).Distinct().ToList();
            return query;
        }

        //查询品牌报警数据
        public IEnumerable<Eventmau> GetPinPaiData(DateTime beginDate, DateTime endDate, List<int> rtuIDs, int itemid)
        {
            var query = (
                             from e in Context.Set<SwsEventHistory>()
                             where e.EventTime > beginDate && e.EventTime < endDate && rtuIDs.Contains(e.Rtuid) && e.EventLevel != 0
                             join r in Context.Set<SwsDeviceInfo01>() on e.Rtuid equals r.Rtuid into er
                             from era in er.DefaultIfEmpty()
                             join d in Context.Set<SysDataItemDetail>().Where(r => r.FItemId == itemid) on era.Manufacturer.ToString() equals d.ItemValue into dr
                             from dra in dr.DefaultIfEmpty()
                             select new
                             {
                                 ID = e.Id,
                                 Manufacturer = era.Manufacturer,
                                 Name = dra.ItemName,
                                 EventTime = e.EventTime
                             }).Distinct().GroupBy(r => r.Name).Select(g => new Eventmau { Name = g.Key, Count = g.Count() });
            return query;
        }

        //设备类型分析
        public IEnumerable<Eventmau> GetDeviceTypeData(DateTime beginDate, DateTime endDate, List<int> rtuIDs, int itemid)
        {
            var query = (
                             from e in Context.Set<SwsEventHistory>()
                             where e.EventTime > beginDate && e.EventTime < endDate && rtuIDs.Contains(e.Rtuid)
                             join r in Context.Set<SwsDeviceInfo01>() on e.Rtuid equals r.Rtuid into er
                             from era in er.DefaultIfEmpty()
                             join d in Context.Set<SysDataItemDetail>().Where(r => r.FItemId == itemid) on era.DeviceType.ToString() equals d.ItemValue into dr
                             from dra in dr.DefaultIfEmpty()
                             select new
                             {
                                 ID = e.Id,
                                 Name = dra.ItemName,
                                 EventTime = e.EventTime
                             }).Distinct().GroupBy(r => r.Name).Select(g => new Eventmau { Name = g.Key, Count = g.Count() });
            return query;
        }

        //查询表格数据
        public IEnumerable<EventStation> GetEventTableData(DateTime beginDate, DateTime endDate, List<int> rtuIDs, int pageSize, int pageIndex, ref int totalCount)
        {
            var query = from e in Context.Set<SwsEventHistory>()
                        where e.EventTime > beginDate && e.EventTime < endDate && rtuIDs.Contains(e.Rtuid)
                        join r in Context.Set<SwsRtuinfo>() on e.Rtuid equals r.Rtuid into er
                        from era in er.DefaultIfEmpty()
                        join s in Context.Set<SwsStation>() on era.StationId equals s.StationId into rs
                        from rsa in rs.DefaultIfEmpty()
                        orderby e.EventTime descending
                        select new EventStation
                        {
                            Rtuid = e.Rtuid,
                            StationName = rsa.StationName,
                            State = e.State,
                            EventTime = e.EventTime.ToString("yyyy-MM-dd HH:mm:ss"),
                            EndDate = e.EndDate,
                            EventLevel = e.EventLevel,
                            EventMessage = e.EventMessage,
                            EventType = e.EventType,
                            CurrentValue = e.CurrentValue,
                            LimitValue = e.LimitValue,
                            Manufacturer = "",
                            DeviceType = "",
                            Duration = ""
                        };
            totalCount = query.Count();
            var list = query.Skip(pageSize * (pageIndex - 1)).Take(pageSize);
            return list;
        }

        //查询表格数据 2021.07.14
        public IEnumerable<EventStation> GetEventDetailData(DateTime beginDate, DateTime endDate, List<int> rtuIDs, int pageSize, int pageIndex)
        {
            var lxid = (int)Model.CustomizedClass.Enum.设备类型;
            var ppid = (int)Model.CustomizedClass.Enum.设备品牌;
            var query = from e in (from se in Context.Set<SwsEventHistory>()
                                   where se.EventTime > beginDate && se.EventTime < endDate && rtuIDs.Contains(se.Rtuid)
                                   orderby se.EventTime descending
                                   select se).Skip(pageSize * (pageIndex - 1)).Take(pageSize)
                        join r in (from s in Context.Set<SwsDeviceInfo01>()
                                   select new
                                   {
                                       s.DeviceType,
                                       s.Manufacturer,
                                       s.Rtuid,
                                       s.StationId
                                   }).Distinct() on e.Rtuid equals r.Rtuid into er
                        from era in er.DefaultIfEmpty()
                        join d in Context.Set<SysDataItemDetail>().Where(r => r.FItemId == lxid) on era.DeviceType.ToString() equals d.ItemValue into dr
                        from dra in dr.DefaultIfEmpty()
                        join t in Context.Set<SysDataItemDetail>().Where(r => r.FItemId == ppid) on era.Manufacturer.ToString() equals t.ItemValue into tr
                        from tra in tr.DefaultIfEmpty()
                        join s in Context.Set<SwsStation>() on era.StationId equals s.StationId into rs
                        from rsa in rs.DefaultIfEmpty()
                        orderby e.EventTime descending
                        select new EventStation
                        {
                            Rtuid = e.Rtuid,
                            StationName = rsa.StationName,
                            Manufacturer = tra.ItemName,
                            DeviceType = dra.ItemName,
                            State = e.State,
                            EventTime = e.EventTime.ToString("yyyy-MM-dd HH:mm:ss"),
                            EndDate = e.EndDate,
                            EventLevel = e.EventLevel,
                            EventMessage = e.EventMessage,
                            EventType = e.EventType,
                            CurrentValue = e.CurrentValue,
                            LimitValue = e.LimitValue,
                            Duration = e.EndDate == null ? "--" : ((TimeSpan)(e.EndDate - e.EventTime)).Days + "天" + ((TimeSpan)(e.EndDate - e.EventTime)).Hours + "时" + ((TimeSpan)(e.EndDate - e.EventTime)).Minutes + "分"
                        };
            return query;
        }

        //查询表格数据 2021.07.14
        public IEnumerable<EventStation> GetDetailData(DateTime beginDate, DateTime endDate, List<int> rtuIDs, int pageSize, int pageIndex, int detailType, string content, ref int totalCount)
        {
            var lxid = (int)Model.CustomizedClass.Enum.设备类型;
            var ppid = (int)Model.CustomizedClass.Enum.设备品牌;
            var query = from se in Context.Set<SwsEventHistory>()
                        where se.EventTime > beginDate && se.EventTime < endDate && rtuIDs.Contains(se.Rtuid)
                        select se;
            if (detailType == 1 && !string.IsNullOrEmpty(content))
            {
                query = query.Where(r => r.EventMessage == content);
            }
            if (detailType == 2 && !string.IsNullOrEmpty(content))
            {
                query = query.Where(r => r.Rtuid == int.Parse(content));
            }
            totalCount = query.Count();
            query = query.OrderByDescending(r => r.EventTime).Skip((pageIndex - 1) * pageSize).Take(pageSize);

            var list = from e in query
                       join r in (from s in Context.Set<SwsDeviceInfo01>()
                                  select new
                                  {
                                      s.DeviceType,
                                      s.Manufacturer,
                                      s.Rtuid,
                                      s.StationId
                                  }).Distinct() on e.Rtuid equals r.Rtuid into er
                       from era in er.DefaultIfEmpty()
                       join d in Context.Set<SysDataItemDetail>().Where(r => r.FItemId == lxid) on era.DeviceType.ToString() equals d.ItemValue into dr
                       from dra in dr.DefaultIfEmpty()
                       join t in Context.Set<SysDataItemDetail>().Where(r => r.FItemId == ppid) on era.Manufacturer.ToString() equals t.ItemValue into tr
                       from tra in tr.DefaultIfEmpty()
                       join s in Context.Set<SwsStation>() on era.StationId equals s.StationId into rs
                       from rsa in rs.DefaultIfEmpty()
                       orderby e.EventTime descending
                       select new EventStation
                       {
                           Rtuid = e.Rtuid,
                           StationName = rsa.StationName,
                           Manufacturer = tra.ItemName,
                           DeviceType = dra.ItemName,
                           State = e.State,
                           EventTime = e.EventTime.ToString("yyyy-MM-dd HH:mm:ss"),
                           EndDate = e.EndDate,
                           EventLevel = e.EventLevel,
                           EventMessage = e.EventMessage,
                           EventType = e.EventType,
                           CurrentValue = e.CurrentValue,
                           LimitValue = e.LimitValue,
                           Duration = e.EndDate == null ? "--" : ((TimeSpan)(e.EndDate - e.EventTime)).Days + "天" + ((TimeSpan)(e.EndDate - e.EventTime)).Hours + "时" + ((TimeSpan)(e.EndDate - e.EventTime)).Minutes + "分"
                       };
            return list;
        }
        #endregion
        /// <summary>
        /// 查询泵房报警排名
        /// </summary>
        /// <param name="beginDate"></param>
        /// <param name="endDate"></param>
        /// <param name="rtuIDs"></param>
        /// <returns></returns>
        public IEnumerable<StationEventRank> LoadStationRank(DateTime beginDate, DateTime endDate, string rtuIDs)
        {
            SqlParameter[] sqlparameter = new SqlParameter[] {
                 new SqlParameter("@beginDate",beginDate),
                new SqlParameter("@endDate",endDate),
                new SqlParameter("@rtuIDs",rtuIDs)
            };
            var sql = @"select ss.StationName,count(se.ID) as EventCount from Sws_Station ss 
                        left join Sws_RTUInfo sd on sd.StationID = ss.StationID
                        left join Sws_EventHistory se on se.Rtuid = sd.rtuid
                        where  CHARINDEX(','+LTRIM(sd.rtuid)+',',','+@rtuIDs+',')>0  and se.EventLevel !=0 and  se.EventTime between @beginDate and @endDate   ";
            sql += " group by StationName ";
            var query = this.Context.Database.SqlQuery<StationEventRank>(sql, sqlparameter);
            return query;
        }
    }

}
