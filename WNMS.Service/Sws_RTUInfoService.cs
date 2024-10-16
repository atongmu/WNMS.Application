using MongoDB.Driver;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using WNMS.Model.CustomizedClass;
using WNMS.Utility;
using MongoDBHelper;
using System.Linq;
using WNMS.Model.DataModels;

namespace WNMS.Service
{
    public partial class Sws_RTUInfoService : BaseService, IService.ISws_RTUInfoService
    {
        /// <summary>
        /// 获取设备监控信息
        /// </summary>
        /// <param name="StationID"></param>
        /// <returns></returns>
        public IEnumerable<DeviceAndRtuinfo> LoadDeviceAndRtuinfo(int StationID)
        {
            SqlParameter[] sp = new SqlParameter[]
             {
                new SqlParameter("@StationID",StationID)
             };

            var query = this.Context.Database.SqlQuery<DeviceAndRtuinfo>("exec GetDeviceAndRTUInfo @StationID", sp);
            return query;
        }

        /// <summary>
        /// 获取发生报警的设备信息
        /// </summary>
        /// <param name="StationID"></param>
        /// <returns></returns>
        public IEnumerable<AlarmDeviceInfo> LoadAlarmDeviceInfo(int StationID)
        {
            SqlParameter[] sp = new SqlParameter[]
             {
                new SqlParameter("@StationID",StationID)
             };

            var query = this.Context.Database.SqlQuery<AlarmDeviceInfo>("exec GetAlarmDeviceInfo @StationID", sp);
            return query;
        }

        public DeviceJK GetMongoJKData(int RTUID)
        {
            DeviceJK listResulr = new DeviceJK();
            MongoDBHelper<DeviceJK> mongoHelper = new MongoDBHelper<DeviceJK>(DateTime.Now.Year.ToString());
            const string proJson = "{ $project: {'_id' : 1,'RTUID':1 ,'AnalogValues':1,'DigitalValues':1,'UpdateTime':1}}";
            string macJson = "{$match:  {'RTUID': " + RTUID + "}}"; ;
            PipelineStageDefinition<DeviceJK, DeviceJK> prostage = new JsonPipelineStageDefinition<DeviceJK, DeviceJK>(proJson);
            PipelineStageDefinition<DeviceJK, DeviceJK> macstage = new JsonPipelineStageDefinition<DeviceJK, DeviceJK>(macJson);
            IList<IPipelineStageDefinition> stages = new List<IPipelineStageDefinition>();
            stages.Add(prostage);
            stages.Add(macstage);
            PipelineDefinition<DeviceJK, DeviceJK> pipeline = new PipelineStagePipelineDefinition<DeviceJK, DeviceJK>(stages);
            var result = mongoHelper.Aggregate(pipeline, "Sws_DeviceJKInfo");
            listResulr = result.FirstOrDefault();
            return listResulr;
        }


        /// <summary>
        /// 获取单条历史数据
        /// </summary>
        /// <param name="year">年份</param>
        /// <param name="beginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="group">分组字符串</param>
        /// <param name="project">筛选字段字符串</param>
        /// <param name="collectName">集合名称</param>
        /// <returns></returns>
        public DeviceJK GetMongoHistoryJKData(string year, string beginDate, string endDate, string collectName)
        {
            DeviceJK listResulr = new DeviceJK();
            //定义mongodb
            MongoDBHelper<DeviceJK> mongoHelper = new MongoDBHelper<DeviceJK>(year);

            //定义参数
            string match = "{$match:{'UpdateTime' : { '$gte' : ISODate('" + Convert.ToDateTime(beginDate).ToString("yyyy-MM-dd'T'HH:mm:ss'Z'") + "'), '$lt' : ISODate('" + Convert.ToDateTime(endDate).ToString("yyyy-MM-dd'T'HH:mm:ss'Z'") + "') }}}";
            string sort = "{$sort:{'Time':1}}";
            PipelineStageDefinition<DeviceJK, DeviceJK> matchjson = new JsonPipelineStageDefinition<DeviceJK, DeviceJK>(match);
            PipelineStageDefinition<DeviceJK, DeviceJK> sortjson = new JsonPipelineStageDefinition<DeviceJK, DeviceJK>(sort);
            IList<IPipelineStageDefinition> stages = new List<IPipelineStageDefinition>();
            stages.Add(matchjson);
            stages.Add(sortjson);

            //查询
            PipelineDefinition<DeviceJK, DeviceJK> pipeline = new PipelineStagePipelineDefinition<DeviceJK, DeviceJK>(stages);
            var result = mongoHelper.Aggregate(pipeline, collectName);
            listResulr = result.FirstOrDefault();
            return listResulr;
        }

        /// <summary>
        /// 获取一段时间内的历史数据
        /// </summary>
        /// <param name="year">年份</param>
        /// <param name="beginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="group">分组字符串</param>
        /// <param name="project">筛选字段字符串</param>
        /// <param name="collectName">集合名称</param>
        /// <returns></returns>
        public List<DeviceJK> GetMoreMongoHistoryJKData(string year, string beginDate, string endDate, string collectName)
        {
            List<DeviceJK> listResulr = new List<DeviceJK>();
            //定义mongodb
            MongoDBHelper<DeviceJK> mongoHelper = new MongoDBHelper<DeviceJK>(year);

            //定义参数
            string match = "{$match:{'UpdateTime' : { '$gte' : ISODate('" + Convert.ToDateTime(beginDate).ToString("yyyy-MM-dd'T'HH:mm:ss'Z'") + "'), '$lt' : ISODate('" + Convert.ToDateTime(endDate).ToString("yyyy-MM-dd'T'HH:mm:ss'Z'") + "') }}}";
            string sort = "{$sort:{'Time':1}}";
            PipelineStageDefinition<DeviceJK, DeviceJK> matchjson = new JsonPipelineStageDefinition<DeviceJK, DeviceJK>(match);
            PipelineStageDefinition<DeviceJK, DeviceJK> sortjson = new JsonPipelineStageDefinition<DeviceJK, DeviceJK>(sort);
            IList<IPipelineStageDefinition> stages = new List<IPipelineStageDefinition>();
            stages.Add(matchjson);
            stages.Add(sortjson);

            //查询
            PipelineDefinition<DeviceJK, DeviceJK> pipeline = new PipelineStagePipelineDefinition<DeviceJK, DeviceJK>(stages);
            var result = mongoHelper.Aggregate(pipeline, collectName);
            listResulr = result.ToList();
            return listResulr;
        }



        /// <summary>
        /// 获取用水量报表数据
        /// </summary>
        /// <param name="year">年份</param>
        /// <param name="beginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="group">分组字符串</param>
        /// <param name="project">筛选字段字符串</param>
        /// <param name="collectName">集合名称</param>
        /// <returns></returns>
        public IEnumerable<FLowReport> GetFlowReportData(string year, string beginDate, string endDate, string group, string project, string collectName)
        {
            //定义mongodb
            MongoDBHelper<FLowReport> mongoHelper = new MongoDBHelper<FLowReport>(year);

            //定义参数
            string match = "{$match:{'UpdateTime' : { '$gte' : ISODate('" + Convert.ToDateTime(beginDate).ToString("yyyy-MM-dd'T'HH:mm:ss'Z'") + "'), '$lt' : ISODate('" + Convert.ToDateTime(endDate).ToString("yyyy-MM-dd'T'HH:mm:ss'Z'") + "') }}}";
            string sort = "{$sort:{'Time':1}}";
            PipelineStageDefinition<FLowReport, FLowReport> matchjson = new JsonPipelineStageDefinition<FLowReport, FLowReport>(match);
            PipelineStageDefinition<FLowReport, FLowReport> projectjson = new JsonPipelineStageDefinition<FLowReport, FLowReport>(project);
            PipelineStageDefinition<FLowReport, FLowReport> groupjson = new JsonPipelineStageDefinition<FLowReport, FLowReport>(group);
            PipelineStageDefinition<FLowReport, FLowReport> sortjson = new JsonPipelineStageDefinition<FLowReport, FLowReport>(sort);
            IList<IPipelineStageDefinition> stages = new List<IPipelineStageDefinition>();
            stages.Add(matchjson);
            stages.Add(groupjson);
            stages.Add(projectjson);
            stages.Add(sortjson);

            //查询
            PipelineDefinition<FLowReport, FLowReport> pipeline = new PipelineStagePipelineDefinition<FLowReport, FLowReport>(stages);
            var result = mongoHelper.Aggregate(pipeline, collectName);
            return result.ToList();
        }
        #region 通讯接入
        //删除数据
        public int DeleteDevHistoryInfo(string begindate, string enddate, int rtuid)
        {
            string year = Convert.ToDateTime(begindate).Year.ToString();
            MongoDBHelper<HistoryJKData> mongoHelper = new MongoDBHelper<HistoryJKData>(year);
            begindate = string.Format("{0:yyyy-MM-dd'T'HH:mm:ss'Z'}", Convert.ToDateTime(begindate));
            enddate = string.Format("{0:yyyy-MM-dd'T'HH:mm:ss'Z'}", Convert.ToDateTime(enddate));

            FilterDefinition<HistoryJKData> filter = "{'UpdateTime':{$gte:ISODate('" + begindate + "'), $lt:ISODate('" + enddate + "')}}";
            var model = mongoHelper.FindSingle(rtuid.ToString(), filter);
            int count = 0;
            if (model != null)
            {
                count = Convert.ToInt32(mongoHelper.Delete(rtuid.ToString(), filter));
            }

            return count;

        }
        public IEnumerable<dynamic> GetSwsRtuInfoTable(int startindex, int pageSize, string orderItems, string filterString, ref int Totalcount)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@startindex",startindex),
                new SqlParameter("@pageSize",pageSize),
                new SqlParameter("@orderItems",orderItems),
                new SqlParameter("@filterString",filterString),
                new SqlParameter("@Totalcount",Totalcount){Direction=System.Data.ParameterDirection.Output,DbType=System.Data.DbType.Int32}
            };
            var query = this.Context.Database.SqlQuery_Dic("exec QuerySws_RTUInfoTable  @startindex,@pageSize,@orderItems,@filterString,@Totalcount Output", sp).ToList();

            Totalcount = Convert.ToInt32(sp[4].Value);
            return query;
        }
        //挂接通讯左侧泵房树
        public IEnumerable<dynamic> GetRtu_StationTree(int stationid, string stationName)
        {
            stationName = stationName ?? "";
            SqlParameter[] sp = new SqlParameter[] {
            new SqlParameter("@stationid",stationid),
            new SqlParameter("@stationName","%"+stationName+"%")
            };
            string sql = @"with t as(  select  StationID from [dbo].[Sws_DeviceInfo01] where RTUID is null or RTUID=0
  union select StationID from [dbo].[Sws_DeviceInfo02] where RTUID is null or RTUID=0)
  select s.StationID,StationName,InType,case when s.StationID=@stationid then 1 else 0 end check1 from [dbo].[Sws_Station] s
  left join t on s.StationID=t.StationID
  where (t.StationID is not null or s.StationID=@stationid)";
            if (!string.IsNullOrEmpty(stationName))
            {
                sql += " and StationName like  @stationName";
            }
            var query = this.Context.Database.SqlQuery_Dic(sql, sp);
            return query;
        }
        //挂接通讯右侧设备列表
        public IEnumerable<dynamic> GetRtu_StationDevice(int stationid, int rtuid, string tablename, int f_itemid, string deviceids)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@stationid",stationid),
                new SqlParameter("@rtuid",rtuid),
                new SqlParameter("@tablename",tablename),
                new SqlParameter("@f_itemid",f_itemid),
                new SqlParameter("@deviceids",deviceids)
           };

            string sql = @"select d.DeviceID,d.DeviceName,d.RTUID,ItemName as partionname,case when g.DeviceID is not null then 1 else  0 end hasCheck from [dbo].[" + @tablename + "] d " +
  @"left join (select * from  [dbo].[Sys_DataItemDetail] where F_ItemId=@f_itemid and IsEnable=1) t on d.Partition=t.ItemValue
left join (select * from [dbo].[" + @tablename + "] where CHARINDEX(','+LTRIM(DeviceID)+',',',' + @deviceids + ',')>0 ) g on d.DeviceID=g.DeviceID " +
   @"where d.StationID=@stationid and (d.RTUID is null or d.RTUID=@rtuid or d.RTUID=0)";
            var query = this.Context.Database.SqlQuery_Dic(sql, sp);
            return query;
        }
        //根据通讯id 查设备
        public IEnumerable<dynamic> GetDeviceByRtuId(int rtuid, string tablename, int f_itemid)
        {
            SqlParameter[] sp = new SqlParameter[] {
                 new SqlParameter("@rtuid",rtuid),
                 new SqlParameter("@tablename",tablename),
                 new SqlParameter("@f_itemid",f_itemid)
            };
            string sql = @"select d.DeviceID,d.DeviceName,d.RTUID,ItemName as partionname from [dbo].[" + @tablename + "] d " +
  @"left join (select * from  [dbo].[Sys_DataItemDetail] where F_ItemId=@f_itemid and IsEnable=1) t on d.Partition=t.ItemValue 
   where RTUID=@rtuid";
            var query = this.Context.Database.SqlQuery_Dic(sql, sp);
            return query;
        }
        //通讯信息添加
        public int AddRtuInfo(SwsRtuinfo r, List<long> Deviceids)
        {
            var station = this.Context.Set<SwsStation>().Where(a => a.StationId == r.StationId).FirstOrDefault();
            if (station != null)
            {
                if (station.InType == 1)//供水泵房
                {
                    var deviceList = this.Context.Set<SwsDeviceInfo01>().Where(r => Deviceids.Contains(r.DeviceId));
                    foreach (var item in deviceList)
                    {
                        item.Rtuid = r.Rtuid;
                        this.Context.Set<SwsDeviceInfo01>().Update(item);
                    }
                }
                else if (station.InType == 2)//直饮水泵房
                {
                    var deviceList = this.Context.Set<SwsDeviceInfo02>().Where(r => Deviceids.Contains(r.DeviceId));
                    foreach (var item in deviceList)
                    {
                        item.Rtuid = r.Rtuid;
                        this.Context.Set<SwsDeviceInfo02>().Update(item);
                    }
                }
            }
            this.Context.Set<SwsRtuinfo>().Add(r);
            return this.Context.SaveChanges();
        }
        //通讯信息修改
        public int EditeRtuInfo(SwsRtuinfo r, List<long> Deviceids)
        {
            //需要把之前该通讯挂接的设备的通讯id清0
            var device1 = this.Context.Set<SwsDeviceInfo01>().Where(a => a.Rtuid == r.Rtuid);
            foreach (var item in device1)
            {
                item.Rtuid = 0;
                this.Context.Set<SwsDeviceInfo01>().Update(item);
            }
            var device2 = this.Context.Set<SwsDeviceInfo02>().Where(a => a.Rtuid == r.Rtuid);
            foreach (var item in device2)
            {
                item.Rtuid = 0;
                this.Context.Set<SwsDeviceInfo02>().Update(item);
            }


            var station = this.Context.Set<SwsStation>().Where(a => a.StationId == r.StationId).FirstOrDefault();
            if (station != null)
            {
                if (station.InType == 1)//供水泵房
                {
                    var deviceList = this.Context.Set<SwsDeviceInfo01>().Where(r => Deviceids.Contains(r.DeviceId));
                    foreach (var item in deviceList)
                    {
                        item.Rtuid = r.Rtuid;
                        this.Context.Set<SwsDeviceInfo01>().Update(item);
                    }
                }
                else if (station.InType == 2)//直饮水泵房
                {
                    var deviceList = this.Context.Set<SwsDeviceInfo02>().Where(r => Deviceids.Contains(r.DeviceId));
                    foreach (var item in deviceList)
                    {
                        item.Rtuid = r.Rtuid;
                        this.Context.Set<SwsDeviceInfo02>().Update(item);
                    }
                }
            }
            this.Context.Set<SwsRtuinfo>().Update(r);
            return this.Context.SaveChanges();
        }
        //通讯信息删除
        public int DeleteRtuInfo(List<int> rtuids)
        {
            var rtuinfos = this.Context.Set<SwsRtuinfo>().Where(a => rtuids.Contains(a.Rtuid));
            this.Context.Set<SwsRtuinfo>().RemoveRange(rtuinfos);
            var device1 = this.Context.Set<SwsDeviceInfo01>().Where(a => rtuids.Contains((int)a.Rtuid));
            foreach (var item in device1)
            {
                item.Rtuid = 0;
                this.Context.Set<SwsDeviceInfo01>().Update(item);

            }
            var device2 = this.Context.Set<SwsDeviceInfo02>().Where(a => rtuids.Contains((int)a.Rtuid));
            foreach (var item in device2)
            {
                item.Rtuid = 0;
                this.Context.Set<SwsDeviceInfo02>().Update(item);
            }
            //删除通讯的时候  删除通讯下的报警信息
            var eventInfo = this.Context.Set<SwsEventInfo>().Where(a => rtuids.Contains((int)a.Rtuid));
            foreach (var item in eventInfo)
            {
                item.Rtuid = 0;
                this.Context.Set<SwsEventInfo>().Remove(item);
            }
            var eventHistory = this.Context.Set<SwsEventHistory>().Where(a => rtuids.Contains((int)a.Rtuid));
            foreach (var item in eventHistory)
            {
                item.Rtuid = 0;
                this.Context.Set<SwsEventHistory>().Remove(item);
            }
            var eventHandle = this.Context.Set<SwsEventHandle>().Where(a => rtuids.Contains((int)a.Rtuid));
            foreach (var item in eventHandle)
            {
                item.Rtuid = 0;
                this.Context.Set<SwsEventHandle>().Remove(item);
            }
            return this.Context.SaveChanges();
        }
        #endregion
        #region 大屏能耗分析
        //左侧树
        public IEnumerable<dynamic> GetDeviceTree_Big(string stationName, int userid, bool isadmin)
        {
            stationName = stationName ?? "";
            var f_itemid = (int)Model.CustomizedClass.Enum.设备分区;
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@stationName","%"+stationName+"%"),
                new SqlParameter("@f_itemid",f_itemid),
                new SqlParameter("@userid",userid)
           };
            string sql = "";
            if (isadmin == true)
            {
                sql = @" with  base as(select * from [dbo].[Sws_Station] where StationName like @stationName)," +
    @"t as ( select DeviceID as ID,d.StationID as Parent,ItemName as Name,1 as type,DeviceName as extraName,Partition from [dbo].[Sws_DeviceInfo01] d
  left join (select * from [dbo].[Sys_DataItemDetail] where F_ItemId=@f_itemid and IsEnable=1) g on d.Partition=g.ItemValue
  left join base on d.StationID=base.StationID
   where d.StationID !=0 and base.StationID is not null),
  tt as (
  select s.StationID as ID,0 as Parent,StationName as Name,0 as type,StationName as extraName,0 as Partition from base s
  left join (select distinct Parent from t) g on s.StationID=g.Parent where g.Parent is not null),
  r as (
  select * from tt 
  union select * from t) select * from r order by Partition";
            }
            else
            {
                sql = @" with  base as(select s.* from [dbo].[Sws_Station] s
left join [dbo].[Sws_UserStation] u on s.StationID=u.StationID
where StationName like @stationName and UserID=@userid)," +
       @"t as ( select DeviceID as ID,d.StationID as Parent,ItemName as Name,1 as type,DeviceName as extraName,Partition from [dbo].[Sws_DeviceInfo01] d
  left join (select * from [dbo].[Sys_DataItemDetail] where F_ItemId=@f_itemid and IsEnable=1) g on d.Partition=g.ItemValue
  left join base on d.StationID=base.StationID
   where d.StationID !=0 and base.StationID is not null),
  tt as (
  select s.StationID as ID,0 as Parent,StationName as Name,0 as type,StationName as extraName,0 as Partition from base s
  left join (select distinct Parent from t) g on s.StationID=g.Parent where g.Parent is not null),
  r as (select * from tt 
  union select * from t) select * from r order by Partition";
            }
            var query = this.Context.Database.SqlQuery_Dic(sql, sp);
            return query;
        }
        //左侧树,过滤掉智能泵房
        public IEnumerable<dynamic> GetDeviceTree_BigFilter(string stationName, int userid, bool isadmin)
        {
            var f_itemid = (int)Model.CustomizedClass.Enum.设备分区;
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@stationName","%"+stationName+"%"),
                new SqlParameter("@f_itemid",f_itemid),
                new SqlParameter("@userid",userid)
           };
            string sql = "";
            if (isadmin == true)
            {
                sql = @" with  base as(select * from [dbo].[Sws_Station] where StationName like @stationName)," +
    @"t as ( select DeviceID as ID,d.StationID as Parent,ItemName as Name,1 as type,DeviceName as extraName,Partition from [dbo].[Sws_DeviceInfo01] d
  left join (select * from [dbo].[Sys_DataItemDetail] where F_ItemId=@f_itemid and IsEnable=1) g on d.Partition=g.ItemValue
  left join base on d.StationID=base.StationID
   where d.StationID !=0 and base.StationID is not null and Partition !=6),
  tt as (
  select s.StationID as ID,0 as Parent,StationName as Name,0 as type,StationName as extraName,0 as Partition from base s
  left join (select distinct Parent from t) g on s.StationID=g.Parent where g.Parent is not null),
  r as (
  select * from tt 
  union select * from t) select * from r order by Partition";
            }
            else
            {
                sql = @" with  base as(select s.* from [dbo].[Sws_Station] s
left join [dbo].[Sws_UserStation] u on s.StationID=u.StationID
where StationName like @stationName and UserID=@userid)," +
       @"t as ( select DeviceID as ID,d.StationID as Parent,ItemName as Name,1 as type,DeviceName as extraName,Partition from [dbo].[Sws_DeviceInfo01] d
  left join (select * from [dbo].[Sys_DataItemDetail] where F_ItemId=@f_itemid and IsEnable=1) g on d.Partition=g.ItemValue
  left join base on d.StationID=base.StationID
   where d.StationID !=0 and base.StationID is not null and Partition !=6),
  tt as (
  select s.StationID as ID,0 as Parent,StationName as Name,0 as type,StationName as extraName,0 as Partition from base s
  left join (select distinct Parent from t) g on s.StationID=g.Parent where g.Parent is not null),
  r as (select * from tt 
  union select * from t) select * from r order by Partition";
            }
            var query = this.Context.Database.SqlQuery_Dic(sql, sp);
            return query;
        }
        //查询当前月能耗
        public IEnumerable<DmonthQuartZ01> GetTodayCompData(long DeviceID, string begindate, string enddate)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@DeviceID",DeviceID),
                new SqlParameter("@begindate",begindate),
                new SqlParameter("@enddate",enddate)
            };
            var sql = @"select 0 as ID,DeviceID,sum(EnergyCon) as EnergyCon,sum(FlowCon) as FlowCon,cast(@begindate as datetime)  as UpdateTime  from [dbo].[DDayQuartZ01] 
where DeviceID=@DeviceID and UpdateTime>=@begindate and UpdateTime<@enddate group by DeviceID ";
            var query = this.Context.Database.SqlQuery<DmonthQuartZ01>(sql, sp);
            return query;
        }
        //获取当前天能耗数据
        public IEnumerable<DdayQuartZ01> GetThisDayCompData(string DeviceID, string begindate, string enddate)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@DeviceID",DeviceID),
                new SqlParameter("@begindate",begindate),
                new SqlParameter("@enddate",enddate)
            };
            var sql = @"select 0 as ID,DeviceID,sum(EnergyCon) as EnergyCon,sum(FlowCon) as FlowCon,cast(@begindate as datetime)  as UpdateTime  from [dbo].[DHourQuartZ01] 
where CHARINDEX(','+LTRIM(DeviceID)+',',','+ @DeviceID + ',')>0 and UpdateTime>=''+@begindate+'' and UpdateTime<=''+@enddate+'' group by DeviceID ";
            var query = this.Context.Database.SqlQuery<DdayQuartZ01>(sql, sp);
            return query;
        }
        /// <summary>
        /// 获取历史数据
        /// </summary>
        /// <param name="year">年份</param>
        /// <param name="beginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="group">分组字符串</param>
        /// <param name="project">筛选字段字符串</param>
        /// <param name="collectName">集合名称</param>
        /// <returns></returns>
        public IEnumerable<DetailFlowData> GetDetailFlowData(string year, string beginDate, string endDate, string group, string project, string collectName)
        {
            //定义mongodb
            MongoDBHelper<DetailFlowData> mongoHelper = new MongoDBHelper<DetailFlowData>(year);

            //定义参数
            string match = "{$match:{'UpdateTime' : { '$gte' : ISODate('" + Convert.ToDateTime(beginDate).ToString("yyyy-MM-dd'T'HH:mm:ss'Z'") + "'), '$lt' : ISODate('" + Convert.ToDateTime(endDate).ToString("yyyy-MM-dd'T'HH:mm:ss'Z'") + "') }}}";
            string sort = "{$sort:{'Time':1}}";
            PipelineStageDefinition<DetailFlowData, DetailFlowData> matchjson = new JsonPipelineStageDefinition<DetailFlowData, DetailFlowData>(match);
            PipelineStageDefinition<DetailFlowData, DetailFlowData> projectjson = new JsonPipelineStageDefinition<DetailFlowData, DetailFlowData>(project);
            PipelineStageDefinition<DetailFlowData, DetailFlowData> groupjson = new JsonPipelineStageDefinition<DetailFlowData, DetailFlowData>(group);
            PipelineStageDefinition<DetailFlowData, DetailFlowData> sortjson = new JsonPipelineStageDefinition<DetailFlowData, DetailFlowData>(sort);
            IList<IPipelineStageDefinition> stages = new List<IPipelineStageDefinition>();
            stages.Add(matchjson);
            stages.Add(groupjson);
            stages.Add(projectjson);
            stages.Add(sortjson);

            //查询
            PipelineDefinition<DetailFlowData, DetailFlowData> pipeline = new PipelineStagePipelineDefinition<DetailFlowData, DetailFlowData>(stages);
            var result = mongoHelper.Aggregate(pipeline, collectName);
            return result.ToList();
        }
        #endregion

        #region GPS设备信息
        /// <summary>
        /// 查询GPS设备借还信息
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <param name="orderby"></param>
        /// <param name="filter"></param>
        /// <param name="Totalcount"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> GetGpsborrowingTable(int pageindex, int pagesize, string orderby, string filter, ref int Totalcount)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@startindex",pageindex),
                new SqlParameter("@pageSize",pagesize),
                new SqlParameter("@orderItems",orderby),
                new SqlParameter("@filterString",filter),
                new SqlParameter("@Totalcount",Totalcount){Direction=System.Data.ParameterDirection.Output,DbType=System.Data.DbType.Int32}
            };
            var query = this.Context.Database.SqlQuery_Dic("exec QueryGpsborrowing @startindex,@pageSize,@orderItems,@filterString,@Totalcount Output", sp).ToList();
            Totalcount = Convert.ToInt32(sp[4].Value);
            return query;
        }
        /// <summary>
        /// 查询分区客服人员
        /// </summary>
        /// <returns></returns>
        public IEnumerable<dynamic> LoadKF()
        {
            SqlParameter[] sp = new SqlParameter[] {

           };
            string sql = @" select u.UserID,NickName,SerialNumber  from [dbo].[Sys_User] u
                left join [dbo].[WO_TeamUser] tu on u.UserID=tu.UserID ";
            var query = this.Context.Database.SqlQuery_Dic(sql, sp);
            return query;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public int DeleteGpsborrowing(List<int> ids)
        {
            var ids_Info = this.Context.Set<SwsGpsborrowing>().Where(r => ids.Contains(r.Id)).ToList();
            foreach (var item in ids_Info)
            {
                this.Context.Set<SwsGpsborrowing>().RemoveRange(item);
            }
            return this.Context.SaveChanges();
        }
        #endregion
    }
}
