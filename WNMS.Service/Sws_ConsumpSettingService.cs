using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WNMS.IService;
using Microsoft.Data.SqlClient;
using WNMS.Model.CustomizedClass;
using WNMS.Utility;
using WNMS.Model.DataModels;
using System.Linq;
using System.Linq.Expressions;
using MongoDB.Driver;

namespace WNMS.Service
{
    public partial class Sws_ConsumpSettingService : BaseService, ISws_ConsumpSettingService
    {
        //public Sws_ConsumpSettingService(DbContext content) : base(content)
        //{

        //}
        public IEnumerable<dynamic> GetDeviceTree(bool isadmin, int userid, string searchtex, short type)
        {
            var manufacterid = (int)Extra_enum.厂商;
            SqlParameter[] sp = new SqlParameter[] {

               new SqlParameter("@userid",userid),
               new SqlParameter("@searchtex","%" + searchtex + "%"),
               new SqlParameter("@type",type),
               new SqlParameter("@manufacterid",manufacterid)
            };
            string sql = "";
            if (isadmin == true)
            {
                sql = @" with t as ( select DeviceName,DeviceID,s.StationID,StationName,m.ItemName as ManufacturerName from [dbo].[Sws_DeviceInfo01] d
  inner join [dbo].[Sws_Station] s on d.StationID=s.StationID
 left join (select * from  [dbo].[Sys_DataItemDetail] where F_ItemId=@manufacterid and IsEnable=1) m on d.Manufacturer=m.ItemValue)
  select t.*,case when c.UserID is null then 0 else 1 end ischeck from t
   left join [dbo].[Sws_ConsumpSetting] c on t.DeviceID = c.DeviceID and c.UserID = @userid and c.Type = @type 
where StationName like @searchtex or DeviceName like @searchtex";
            }
            else
            {
                sql = @" with t as ( select DeviceName,DeviceID,s.StationID,StationName,m.ItemName as ManufacturerName from [dbo].[Sws_DeviceInfo01] d
  inner join [dbo].[Sws_Station] s on d.StationID=s.StationID
  inner join [dbo].[Sws_UserStation] u on d.StationID=u.StationID
 left join (select * from  [dbo].[Sys_DataItemDetail] where F_ItemId=@manufacterid and IsEnable=1) m on d.Manufacturer=m.ItemValue
  where u.UserID=@userid
  )
  select t.*,case when c.UserID is null then 0 else 1 end ischeck from t
   left join [dbo].[Sws_ConsumpSetting] c on t.DeviceID = c.DeviceID and c.UserID = @userid and c.Type = @type 
where StationName like @searchtex or DeviceName like @searchtex";
            }

            var query = this.Context.Database.SqlQuery_Dic(sql, sp);

            return query;
        }
        //设备插入
        public int DeviceSetting(int userid, short type, List<SwsConsumpSetting> datalist)
        {
            var oldlist = this.Context.Set<SwsConsumpSetting>().Where(r => r.UserId == userid && r.Type == type).ToList();
            if (oldlist.Count > 0)
            {
                this.Context.Set<SwsConsumpSetting>().RemoveRange(oldlist);
            }
            this.Context.Set<SwsConsumpSetting>().AddRange(datalist);
            return this.Context.SaveChanges();
        }
        //获取单吨能耗
        public IEnumerable<dynamic> QueryNH_pre(string deviceids, string tablename, DateTime begindate, DateTime enddate)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@deviceids",deviceids),
                new SqlParameter("@tablename",tablename),
                new SqlParameter("@begindate",begindate),
                new SqlParameter("@enddate",enddate)
          };
            string sql = @"  select t.DeviceID,EnergyCon,FlowCon,DeviceName,UpdateTime from [dbo].[" + @tablename + "] t" +
 @" left join [dbo].[Sws_DeviceInfo01] d on t.DeviceID=d.DeviceID
  where CHARINDEX(','+LTRIM(t.DeviceID)+',',','+@deviceids+',')>0  and UpdateTime>= @begindate and UpdateTime< @enddate";
            var query = this.Context.Database.SqlQuery_Dic(sql.ToString(), sp);
            return query;
        }
        //获取吨水能耗
        public IEnumerable<dynamic> QueryNH_manufacter(string deviceids, DateTime begindate, DateTime enddate)
        {
            var manufacterid = (int)Extra_enum.厂商;
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@deviceids",deviceids),
                new SqlParameter("@manufacterid",manufacterid),
                new SqlParameter("@begindate",begindate),
                new SqlParameter("@enddate",enddate)
             };
            string sql = @" with t as ( select d.*,s.Manufacturer from [dbo].[Sws_DeviceInfo01] s 
  left join [dbo].[DDayQuartZ01] d on d.DeviceID=s.DeviceID
  where CHARINDEX(','+LTRIM(s.DeviceID)+',',','+@deviceids+',')>0  and UpdateTime>=@begindate and UpdateTime<@enddate" +
  @"),
  r as (
  select Manufacturer,sum(EnergyCon) as EnergyCon,sum(FlowCon) as FlowCon from t group by Manufacturer)
  select case when FlowCon=0 then 0 else EnergyCon/FlowCon end nh,d.ItemName as ManufacturerName from r 
  left join (select * from [dbo].[Sys_DataItemDetail] where F_ItemId=@manufacterid and IsEnable=1) d on r.Manufacturer=d.ItemValue";
            var query = this.Context.Database.SqlQuery_Dic(sql.ToString(), sp);
            return query;
        }
        //泵房能耗总量
        public IEnumerable<dynamic> QueryNH_station(string deviceids, DateTime begindate, DateTime enddate)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@deviceids",deviceids),
                new SqlParameter("@begindate",begindate),
                new SqlParameter("@enddate",enddate)
             };
          
            string sql = @" with t as ( select d.*,s.StationID from [dbo].[Sws_DeviceInfo01] s 
  left join [dbo].[DDayQuartZ01] d on d.DeviceID=s.DeviceID
  where CHARINDEX(','+LTRIM(s.DeviceID)+',',','+@deviceids+',')>0  and UpdateTime>=@begindate and UpdateTime<@enddate" +
            @"),
  r as (
  select StationID,sum(EnergyCon) as EnergyCon,sum(FlowCon) as FlowCon from t group by StationID)
  select EnergyCon as  nh,s.StationName  from r 
  left join [dbo].[Sws_Station] s on r.StationID=s.StationID";
            var query = this.Context.Database.SqlQuery_Dic(sql.ToString(), sp);
            
            return query;
        }
        //获取设备和分区
        public IEnumerable<dynamic> QueryAreaDevice(string deviceids)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@deviceids",deviceids)
             };
            string sql = @"     select d.DeviceID,d.DeviceName,s.StationName,d.StationID from [dbo].[Sws_DeviceInfo01] d
   left join [dbo].[Sws_Station] s on d.StationID=s.StationID
  where CHARINDEX(','+LTRIM(d.DeviceID)+',',','+@deviceids+',')>0 ";
            var query = this.Context.Database.SqlQuery_Dic(sql.ToString(), sp);
            return query;
        }
        //初始化，选取默认设备
        public IEnumerable<dynamic> GetDeviceid_init(int userid, bool isadmin)
        {
            SqlParameter[] sp = new SqlParameter[] {
            new SqlParameter("@userid",userid)
            };
            StringBuilder sql = new StringBuilder();
            if (isadmin == true)
            {
                sql.Append("select top 6 DeviceID from [dbo].[Sws_DeviceInfo01]");
            }
            else
            {
                sql.Append(@"select top 6 DeviceID from [dbo].[Sws_DeviceInfo01] d
left join[dbo].[Sws_UserStation] s on d.StationID = s.StationID
where UserID = @userid");
            }
            var query = this.Context.Database.SqlQuery_Dic(sql.ToString(), sp);
            return query;
        }
        //厂商设备类型能耗排名
        public IEnumerable<dynamic> GetMaDeviceRate(int userid, bool isadmin, string begindate, string enddate)
        {
            var manufacterid = (int)Extra_enum.厂商;
            var devicetype = (int)Extra_enum.设备型号;
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@userid",userid),
                new SqlParameter("@begindate",begindate),
                new SqlParameter("@enddate",enddate),
                new SqlParameter("@manufacterid",manufacterid),
                new SqlParameter("@devicetype",devicetype)
            };
            StringBuilder sql = new StringBuilder();
            if (isadmin == true)
            {
                sql.Append(@"with t as (
  select DeviceType,Manufacturer,EnergyCon,FlowCon from [dbo].[Sws_DeviceInfo01] d
  left join [dbo].[DDayQuartZ01] q on d.DeviceID=q.DeviceID
  where UpdateTime>=@begindate and UpdateTime<@enddate),");
            }
            else
            {
                sql.Append(@"with t as (
  select DeviceType,Manufacturer,EnergyCon,FlowCon from [dbo].[Sws_DeviceInfo01] d
  left join [dbo].[DDayQuartZ01] q on d.DeviceID=q.DeviceID
  left join [dbo].[Sws_UserStation] u on d.StationID=u.StationID
  where UpdateTime>=@begindate and UpdateTime<@enddate and UserID=@userid),");
            }
            sql.Append(@"a as(
  select DeviceType,Manufacturer,sum(EnergyCon) as EnergyCon,sum(FlowCon) as FlowCon from t group by DeviceType,Manufacturer),
  m as(
  select a.*,case when FlowCon=0 then 0 else EnergyCon/FlowCon end nh  from a)
  select d1.ItemName as DeviceTypeName,d2.ItemName as ManufacterName,EnergyCon,FlowCon,cast(nh as decimal(10,2)) as nh from m
  left join (select * from [dbo].[Sys_DataItemDetail] where F_ItemId=@devicetype and IsEnable=1) d1 on m.DeviceType=d1.ItemValue
  left join (select * from [dbo].[Sys_DataItemDetail] where F_ItemId=@manufacterid and IsEnable=1) d2 on m.Manufacturer=d2.ItemValue
  order by nh");
            var query = this.Context.Database.SqlQuery_Dic(sql.ToString(), sp);
            return query;

        }
        //千吨水百米扬程能耗
        public IEnumerable<dynamic> GetMilliLiftNH(int userid, bool isadmin, string begindate, string enddate)
        {
            var manufacterid = (int)Extra_enum.厂商;
            var devicetype = (int)Extra_enum.设备型号;
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@userid",userid),
                new SqlParameter("@begindate",begindate),
                new SqlParameter("@enddate",enddate),
                new SqlParameter("@manufacterid",manufacterid),
                new SqlParameter("@devicetype",devicetype)
            };
            StringBuilder sql = new StringBuilder();
            if (isadmin == true)
            {
                sql.Append(@"with t as (
  select DeviceType,Manufacturer,EnergyCon,FlowCon from [dbo].[Sws_DeviceInfo01] d
  left join [dbo].[DDayQuartZ01] q on d.DeviceID=q.DeviceID
  where UpdateTime>=@begindate and UpdateTime<@enddate),");
            }
            else
            {
                sql.Append(@"with t as (
  select DeviceType,Manufacturer,EnergyCon,FlowCon from [dbo].[Sws_DeviceInfo01] d
  left join [dbo].[DDayQuartZ01] q on d.DeviceID=q.DeviceID
  left join [dbo].[Sws_UserStation] u on d.StationID=u.StationID
  where UpdateTime>=@begindate and UpdateTime<@enddate and UserID=@userid),");
            }
            sql.Append(@"a as(
  select DeviceType,Manufacturer,sum(EnergyCon) as EnergyCon,sum(FlowCon) as FlowCon from t group by DeviceType,Manufacturer),
  m as(
  select a.*,case when FlowCon=0 then 0 else EnergyCon/FlowCon end nh  from a)
  select d1.ItemName as DeviceTypeName,d2.ItemName as ManufacterName,cast(nh*10/SQRT(b.HighLift) as decimal(10,2)) as nh,b.HighLift,b.MotorEff,b.PumpEff,FlowCon from m
  left join (select * from [dbo].[Sys_DataItemDetail] where F_ItemId=@devicetype and IsEnable=1) d1 on m.DeviceType=d1.ItemValue
  left join (select * from [dbo].[Sys_DataItemDetail] where F_ItemId=@manufacterid and IsEnable=1) d2 on m.Manufacturer=d2.ItemValue
  left join [dbo].[Sws_MaDeBaseInfo] b on m.Manufacturer=b.Manufacturer and m.DeviceType=b.DeviceType
  order by nh");
            var query = this.Context.Database.SqlQuery_Dic(sql.ToString(), sp);
            return query;

        }
        //千吨水百米扬程-厂商信息
        public IEnumerable<dynamic> GetMilliLiftNH_base()
        {
            var manufacterid = (int)Extra_enum.厂商;
            var pumpBrandType = (int)Extra_enum.水泵品牌;
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@manufacterid",manufacterid),
                new SqlParameter("@pumpBrandType",pumpBrandType)
            };
            string sql = @" select b.*,d2.ItemName as ManufacterName,d1.ItemName as PumpBrandName from [dbo].[Sws_MaDeBaseInfo] b
  left join (select * from [dbo].[Sys_DataItemDetail] where F_ItemId=@pumpBrandType and IsEnable=1) d1 on d1.ItemValue=b.PumpBrand
  left join (select * from [dbo].[Sys_DataItemDetail] where F_ItemId=@manufacterid and IsEnable=1) d2 on d2.ItemValue=b.Manufacturer
  order by ValueData asc";
            var query = this.Context.Database.SqlQuery_Dic(sql.ToString(), sp);
            return query;

        }
        #region 厂商设备基础信息

        public PageResult<ManuDeviceBase> GetMaDeviceBaseTable(Expression<Func<ManuDeviceBase, bool>> funcWhere, int pageSize, int pageIndex, string funcOrderby, bool isAsc = true)
        {
            var manufacterid = (int)Extra_enum.厂商;
            var pumpbrandid = (int)Extra_enum.水泵品牌;
            SqlParameter[] sqlparameter = new SqlParameter[] {
                new SqlParameter("@manufacterid",manufacterid),
                new SqlParameter("@pumpbrandid",pumpbrandid)
            };
            string sql = @"  select m.ItemName as ManufacterName,d.ItemName  as PumpBrandName,b.* from [dbo].[Sws_MaDeBaseInfo] b
  left join (select * from [dbo].[Sys_DataItemDetail] where F_ItemId=@manufacterid and IsEnable=1) m on b.Manufacturer=m.ItemValue
  left join (select * from [dbo].[Sys_DataItemDetail] where F_ItemId=@pumpbrandid and IsEnable=1) d on b.PumpBrand=d.ItemValue";

            PageResult<ManuDeviceBase> presult = new PageResult<ManuDeviceBase>();
            presult = this.ExcuteQueryPage(funcWhere, pageSize, pageIndex, funcOrderby, sql, sqlparameter, isAsc);
            return presult;
           
        }
        public int AddInfo(SwsMaDeBaseInfo b)
        {
            this.Context.Set<SwsMaDeBaseInfo>().Add(b);
            return this.Context.SaveChanges();
        }
        public int EditeInfo(SwsMaDeBaseInfo b)//测试
        {
            this.Context.Set<SwsMaDeBaseInfo>().Update(b);
            return this.Context.SaveChanges();
        }
        public int DeleteInfo(List<int> ids)
        {
            var list = this.Context.Set<SwsMaDeBaseInfo>().Where(r => ids.Contains(r.Id));
            if (list.Count() > 0)
            {
                this.Context.Set<SwsMaDeBaseInfo>().RemoveRange(list);
            }
            return this.Context.SaveChanges();
        }
        #endregion
        #region 人员轨迹
        public PageResult<GpsData> GetPersonLocus(string num,int pageindex,int pagesize,string begindate,string enddate)
        {
            var beginyear = Convert.ToDateTime(begindate).Year;
            var year = beginyear;
            var endyear = Convert.ToDateTime(enddate).Year;
            SqlParameter[] sqlparameter = new SqlParameter[] {
                new SqlParameter("@year",year),
                new SqlParameter("@num",num),
                new SqlParameter("@begindate",begindate),
                new SqlParameter("@enddate",enddate)
            };
            StringBuilder sql =new StringBuilder(@"select * from [WNMS_Gps"+ @year + "].[dbo].["+ @num + "] where Lng!=0 and UpdateTime between @begindate and @enddate");
           
            if (beginyear != endyear)
            {
                beginyear = beginyear + 1;
                while (beginyear <= endyear)
                {
                    sql.Append(@" union select * from [WNMS_Gps" + beginyear + "].[dbo].[" + @num + "] where Lng!=0 and UpdateTime between @begindate  and  @enddate");
                    beginyear = beginyear + 1;
                }

            }

            PageResult<GpsData> presult = new PageResult<GpsData>();
            Expression<Func<GpsData, bool>> funcWhere = r => true;
            
            presult = this.ExcuteQueryPage(funcWhere, pagesize, pageindex, "UpdateTime", sql.ToString(), sqlparameter, true);
            return presult;
        }
        public IEnumerable<dynamic> PersonLocusTree(string text)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@text","%"+ text + "%")
           };
            string sql = @" with t as ( select u.UserID,NickName,SerialNumber,case when a.ID is null then -1 else a.ID end ID, case when a.AreaName is null then '未知区域' else a.AreaName end AreaName,case when a.PID is null then 0 else a.PID END PID from [dbo].[Sys_User] u
  left join [dbo].[WO_TeamUser] tu on u.UserID=tu.UserID
  left join [dbo].[WO_TeamInfo] t on tu.TeamID=t.TeamID
  left join [dbo].[WO_AreaInfo] a on t.RegionID=a.ID
  where SerialNumber is not null),
  area as(
  select * from t where AreaName like @text)," +
  @"parentd as(
 select ID,PID,AreaName from area
    union all
    select d.ID,d.PID,d.AreaName from [dbo].[WO_AreaInfo] d join parentd as d2 on d.ID=d2.PID
),
chd as (
select ID,PID,AreaName from area
    union all
    select d.ID,d.PID,d.AreaName from [dbo].[WO_AreaInfo] d join chd as d2 on d.PID=d2.ID
),
users as (
select ID,PID,AreaName, UserID,NickName,SerialNumber from t where NickName like @text" +
@"),
udepart as (
select ID,PID,AreaName from users 
    union all
    select d.ID,d.PID,d.AreaName from [dbo].[WO_AreaInfo] d join udepart as d2 on d.ID=d2.PID
),
r as(
select ID,PID,AreaName,'' as  NickName,1  as type,'' as SerialNumber,0  as UserID from parentd 
union all(
select ID,PID,AreaName,NickName,2 as type,SerialNumber,UserID from t where ID in (select ID  from chd)
)
union all(
select ID,PID,AreaName,'' as  NickName,1  as type,'' as SerialNumber,0  as UserID from udepart
)
union all(
select ID,PID,AreaName,NickName,2 as type,SerialNumber,UserID from users))select distinct * from r

";
            var query = this.Context.Database.SqlQuery_Dic(sql, sp);
            return query;
        }
        //人员列表
        public PageResult<LocusUser> LocusUserList(string userName,string searchTime, int pagesize,int pageindex,string sort,bool flag)
        {

            string filter = "";
            if (!string.IsNullOrEmpty(searchTime))
            {
                filter += " and (CONVERT(varchar(100), BorrowTime, 23)<=@searchTime and CONVERT(varchar(100), RemandTime, 23)>=@searchTime)";
            }
            if (!string.IsNullOrEmpty(userName))
            {
                filter += " and NickName like @userName";
            }
            if (filter != "")
            {
                filter = filter.Substring(4);
            }
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@userName","%"+ userName + "%"),
                new SqlParameter("@searchTime",searchTime)
            };
            string sql = @" select b.UserID,NickName,DptName,b.SerialNumber,BorrowTime,RemandTime,Phone from [dbo].[Sws_Gpsborrowing] b
  inner join [dbo].[Sys_User] u on b.UserID=u.UserID
  inner join [dbo].[Sys_DepartMent] d on u.Department=d.DepartmentID";
            if (!string.IsNullOrEmpty(filter))
            {
                sql += " where " + @filter + "";
            }
            
            Expression<Func<LocusUser, bool>> funcWhere = null;
            PageResult<LocusUser> presult = this.ExcuteQueryPage(funcWhere, pagesize, pageindex, sort, sql, sp, flag);
            return presult;
        }
        #endregion
        #region 人员轨迹调整2022.8.11
        public PageResult<GpsData> GetPersonLocusData(string num, int pageindex, int pagesize, string begindate, string enddate)
        {
            var beginyear = Convert.ToDateTime(begindate).Year;
            var year = beginyear;
            var endyear = Convert.ToDateTime(enddate).Year;
            SqlParameter[] sqlparameter = new SqlParameter[] {
                new SqlParameter("@year",year),
                new SqlParameter("@num",num),
                new SqlParameter("@begindate",begindate),
                new SqlParameter("@enddate",enddate)
            };
            StringBuilder sql = new StringBuilder(@"select * from [WNMS_PhoneGps" + @year + "].[dbo].[HistoryPosition] where Lng!=0 and UpdateTime between @begindate and @enddate and UserID=@num");

            if (beginyear != endyear)
            {
                beginyear = beginyear + 1;
                while (beginyear <= endyear)
                {
                    sql.Append(@" union select * from [WNMS_PhoneGps" + beginyear + "].[dbo].[HistoryPosition] where Lng!=0 and UpdateTime between @begindate  and  @enddate and UserID=@num");
                    beginyear = beginyear + 1;
                }

            }

            PageResult<GpsData> presult = new PageResult<GpsData>();
            Expression<Func<GpsData, bool>> funcWhere = r => true;

            presult = this.ExcuteQueryPage(funcWhere, pagesize, pageindex, "UpdateTime", sql.ToString(), sqlparameter, true);
            return presult;
        }
        public IEnumerable<dynamic> PersonLocusTreeData(string text)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@text","%"+ text + "%")
           };
            string sql = @" with t as ( select u.UserID,NickName,SerialNumber,case when a.ID is null then -1 else a.ID end ID, case when a.AreaName is null then '未知区域' else a.AreaName end AreaName,case when a.PID is null then 0 else a.PID END PID from [dbo].[Sys_User] u
  left join [dbo].[WO_TeamUser] tu on u.UserID=tu.UserID
  left join [dbo].[WO_TeamInfo] t on tu.TeamID=t.TeamID
  left join [dbo].[WO_AreaInfo] a on t.RegionID=a.ID
  ),
  area as(
  select * from t where AreaName like @text)," +
  @"parentd as(
 select ID,PID,AreaName from area
    union all
    select d.ID,d.PID,d.AreaName from [dbo].[WO_AreaInfo] d join parentd as d2 on d.ID=d2.PID
),
chd as (
select ID,PID,AreaName from area
    union all
    select d.ID,d.PID,d.AreaName from [dbo].[WO_AreaInfo] d join chd as d2 on d.PID=d2.ID
),
users as (
select ID,PID,AreaName, UserID,NickName,SerialNumber from t where NickName like @text" +
@"),
udepart as (
select ID,PID,AreaName from users 
    union all
    select d.ID,d.PID,d.AreaName from [dbo].[WO_AreaInfo] d join udepart as d2 on d.ID=d2.PID
),
r as(
select ID,PID,AreaName,'' as  NickName,1  as type,'' as SerialNumber,0  as UserID from parentd 
union all(
select ID,PID,AreaName,NickName,2 as type,SerialNumber,UserID from t where ID in (select ID  from chd)
)
union all(
select ID,PID,AreaName,'' as  NickName,1  as type,'' as SerialNumber,0  as UserID from udepart
)
union all(
select ID,PID,AreaName,NickName,2 as type,SerialNumber,UserID from users))select distinct * from r

";
            var query = this.Context.Database.SqlQuery_Dic(sql, sp);
            return query;
        }
        #endregion
        #region 曲线汇总
        public IEnumerable<dynamic> GetDeviceBySetting(int userid,int type)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@userid",userid),
                new SqlParameter("@type",type)
            };
            string sql = @"  select d.DeviceID,d.DeviceName from [dbo].[Sws_ConsumpSetting] c
  inner join [dbo].[Sws_DeviceInfo01] d on c.DeviceID=d.DeviceID
  where UserID=@userid and c.Type=@type ";
            var query = this.Context.Database.SqlQuery_Dic(sql, sp);
            return query;
        }
        //初始化，选取默认设备
        public IEnumerable<dynamic> GetDevice_init(int userid, bool isadmin)
        {
            SqlParameter[] sp = new SqlParameter[] {
            new SqlParameter("@userid",userid)
            };
            StringBuilder sql = new StringBuilder();
            if (isadmin == true)
            {
                sql.Append("select top 2 DeviceID,DeviceName from [dbo].[Sws_DeviceInfo01]");
            }
            else
            {
                sql.Append(@"select top 2 DeviceID,DeviceName from [dbo].[Sws_DeviceInfo01] d
left join [dbo].[Sws_UserStation] s on d.StationID = s.StationID
where UserID = @userid");
            }
            var query = this.Context.Database.SqlQuery_Dic(sql.ToString(), sp);
            return query;
        }

        //初始化，选取默认设备,过滤掉智能泵房
        public IEnumerable<dynamic> GetDevice_initFilter(int userid, bool isadmin)
        {
            SqlParameter[] sp = new SqlParameter[] {
            new SqlParameter("@userid",userid)
            };
            StringBuilder sql = new StringBuilder();
            if (isadmin == true)
            {
                sql.Append(@"select top 2 DeviceID,DeviceName from [dbo].[Sws_DeviceInfo01] d
 inner join [dbo].[Sws_Station] s on d.StationID=s.StationID where Partition!=6");
            }
            else
            {
                sql.Append(@"select top 2 DeviceID,DeviceName from [dbo].[Sws_DeviceInfo01] d
inner join [dbo].[Sws_Station] s1 on d.StationID=s1.StationID 
left join [dbo].[Sws_UserStation] s on d.StationID = s.StationID
where UserID = @userid and Partition!=6");
            }
            var query = this.Context.Database.SqlQuery_Dic(sql.ToString(), sp);
            return query;
        }
        //设备树
        public IEnumerable<dynamic> QueryDeviceTree(string searchTxt,int userid,bool isadmin)
        {
            SqlParameter[] sp = new SqlParameter[] { 
              new SqlParameter("@searchTxt","%" + searchTxt + "%"),
              new SqlParameter("@userid",userid)
            };
            string sql = "";
            if (isadmin == true)
            {
                sql = @" with t as ( select DeviceName,DeviceID,s.StationID,StationName,Partition from [dbo].[Sws_DeviceInfo01] d
  inner join [dbo].[Sws_Station] s on d.StationID=s.StationID where Partition!=6)
  select * from t where StationName like @searchTxt or DeviceName like @searchTxt";
            }
            else
            {
                sql = @" with t as ( select DeviceName,DeviceID,s.StationID,StationName,Partition from [dbo].[Sws_DeviceInfo01] d
  inner join [dbo].[Sws_Station] s on d.StationID=s.StationID
  inner join [dbo].[Sws_UserStation] u on d.StationID=u.StationID
  where u.UserID=@userid and Partition!=6
  )
  select * from t where StationName like @searchTxt or DeviceName like @searchTxt";
            }
            var query = this.Context.Database.SqlQuery_Dic(sql, sp);
            return query;
        }
        //获取月能耗数据
        public IEnumerable<dynamic> GetMonthNHData(string deviceids,string begindate,string enddate)
        {
            SqlParameter[] sp = new SqlParameter[] { 
               new SqlParameter("@deviceids",deviceids),
               new SqlParameter("@begindate",begindate),
               new SqlParameter("@enddate",enddate)
            };
            string sql = @"  select  DeviceID, EnergyCon, FlowCon, UpdateTime from [dbo].[DMonthQuartZ01] 
  where UpdateTime between  @begindate  and  @enddate  and CHARINDEX(','+LTRIM(DeviceID)+',',','+@deviceids+',')>0";
            var query = this.Context.Database.SqlQuery_Dic(sql, sp);
            return query;
        }
        //获取本月的能耗数据
        public IEnumerable<dynamic> GetThisMonthNH(string deviceids, string begindate, string enddate)
        {
            SqlParameter[] sp = new SqlParameter[] {
               new SqlParameter("@deviceids",deviceids),
               new SqlParameter("@begindate",begindate),
               new SqlParameter("@enddate",enddate)
            };
            string sql = @"select DeviceID,sum(EnergyCon) as EnergyCon,sum(FlowCon) as FlowCon,cast(@begindate  as datetime)  as UpdateTime  from [dbo].[DDayQuartZ01] "+
"where CHARINDEX(','+LTRIM(DeviceID)+',',','+@deviceids+',')>0 and UpdateTime>=@begindate  and UpdateTime<@enddate  group by DeviceID ";
            var query = this.Context.Database.SqlQuery_Dic(sql, sp);
            return query;
        }
        public IEnumerable<dynamic> HistoryChartData(DateTime begindate, DateTime enddate, string project, string collectName)
        {
            //定义mongodb
            string _conntionString = StaticConstraint.MongoDBConn;
            string _dbName = StaticConstraint.MongoDBName;
            var client = new MongoClient(_conntionString);
            var database = client.GetDatabase(_dbName + begindate.Year);

            //定义参数
            string match = @"{'$match':{ 'UpdateTime': { '$gte': ISODate('" + begindate.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'") + "'),'$lt':ISODate('" + enddate.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'") + "')}" +
                            " }" +
                           "}";


            string sort = "{'$sort': {" +
                         "'UpdateTime': 1" +
                         "}}";
            PipelineStageDefinition<dynamic, dynamic> matchjson = new JsonPipelineStageDefinition<dynamic, dynamic>(match);
            PipelineStageDefinition<dynamic, dynamic> projectjson = new JsonPipelineStageDefinition<dynamic, dynamic>(project);
            PipelineStageDefinition<dynamic, dynamic> sortjson = new JsonPipelineStageDefinition<dynamic, dynamic>(sort);
            IList<IPipelineStageDefinition> stages = new List<IPipelineStageDefinition>();
            stages.Add(matchjson);
            stages.Add(projectjson);
            stages.Add(sortjson);

            //查询
            PipelineDefinition<dynamic, dynamic> pipeline = new PipelineStagePipelineDefinition<dynamic, dynamic>(stages);
            var result = database.GetCollection<dynamic>(collectName).Aggregate(pipeline);
            return result.ToList();
        }
        public IEnumerable<dynamic> GetPressFlucNum(string year, string beginDate, string endDate,string match, string group, string project, string collectName)
        {
            //定义mongodb
            string _conntionString = StaticConstraint.MongoDBConn;
            string _dbName = StaticConstraint.MongoDBName;
            //string connectstring = _conntionString + "/" + _dbName ;
            var client = new MongoClient(_conntionString);
            var database = client.GetDatabase(_dbName + year);

            PipelineStageDefinition<dynamic, dynamic> matchjson = new JsonPipelineStageDefinition<dynamic, dynamic>(match);
            PipelineStageDefinition<dynamic, dynamic> projectjson = new JsonPipelineStageDefinition<dynamic, dynamic>(project);
            PipelineStageDefinition<dynamic, dynamic> groupjson = new JsonPipelineStageDefinition<dynamic, dynamic>(group);
           
            IList<IPipelineStageDefinition> stages = new List<IPipelineStageDefinition>();
            stages.Add(projectjson);
            stages.Add(matchjson);
            stages.Add(groupjson);
           
           

            //查询
            PipelineDefinition<dynamic, dynamic> pipeline = new PipelineStagePipelineDefinition<dynamic, dynamic>(stages);
            var result = database.GetCollection<dynamic>(collectName).Aggregate(pipeline);
            return result.ToList();
        }
        //今天波动次数
        public IEnumerable<dynamic> GetPressFlucNum_today(string project, string match, string group, string collectName, DateTime begindate, DateTime enddate)
        {
            //定义参数
            string matchstart = @"{'$match':{ 'UpdateTime': { '$gte': ISODate('" + begindate.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'") + "'),'$lt':ISODate('" + enddate.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'") + "')}" +
                           " }" +
                          "}";

            //定义mongodb
            string _conntionString = StaticConstraint.MongoDBConn;
            string _dbName = StaticConstraint.MongoDBName;
            //string connectstring = _conntionString + "/" + _dbName ;
            var client = new MongoClient(_conntionString);
            var database = client.GetDatabase(_dbName + begindate.Year);
            PipelineStageDefinition<dynamic, dynamic> matchjsonStart = new JsonPipelineStageDefinition<dynamic, dynamic>(matchstart);
            PipelineStageDefinition<dynamic, dynamic> matchjson = new JsonPipelineStageDefinition<dynamic, dynamic>(match);
            PipelineStageDefinition<dynamic, dynamic> projectjson = new JsonPipelineStageDefinition<dynamic, dynamic>(project);
            PipelineStageDefinition<dynamic, dynamic> groupjson = new JsonPipelineStageDefinition<dynamic, dynamic>(group);

            IList<IPipelineStageDefinition> stages = new List<IPipelineStageDefinition>();
            stages.Add(matchjsonStart);
            stages.Add(projectjson);
            stages.Add(matchjson);
            stages.Add(groupjson);



            //查询
            PipelineDefinition<dynamic, dynamic> pipeline = new PipelineStagePipelineDefinition<dynamic, dynamic>(stages);
            var result = database.GetCollection<dynamic>(collectName).Aggregate(pipeline);
            return result.ToList();
        }
        //今天最大最小流量、能耗、单吨能耗
        public IEnumerable<dynamic> GetMaxWaterData(DateTime begindate, DateTime enddate, string deviceids)
        {
            
            SqlParameter[] sp = new SqlParameter[]{
               
                new SqlParameter("@deviceids",deviceids),
                new SqlParameter("@begindate",begindate),
                new SqlParameter("@enddate",enddate)
            };
            string sql = @" with t as ( select UpdateTime,DeviceID,EnergyCon,FlowCon from [dbo].[DHourQuartZ01] " +
 @"where UpdateTime  between @begindate  and  @enddate  and CHARINDEX(','+LTRIM(DeviceID)+',',','+@deviceids+',')>0)," +
 @"data as(
  select DeviceID,sum(EnergyCon) as allEnergy,sum(FlowCon) as allFlow,max(EnergyCon) as maxEnergy,
  min(EnergyCon) as minEnergy,max(FlowCon) as maxFlow,min(FlowCon) as minFlow from t group by DeviceID)
  select DeviceID,case when allFlow=0 then 0 else  cast( allEnergy/allFlow as decimal(18,2)) end consump,
  maxEnergy,minEnergy,maxFlow,minFlow from data ";
            var query = this.Context.Database.SqlQuery_Dic(sql, sp);
            return query;
        }
        #endregion
        #region 安防事件
        public IEnumerable<dynamic> GetSerialNum(int userid)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@userid",userid)
          };
            string sql = @"  select distinct SerialNum from [dbo].[Sws_Camera] c
  left join [dbo].[Sws_UserStation] u on c.StationID=u.StationID
   where SerialNum is not null and UserID=@userid";
            var query = this.Context.Database.SqlQuery_Dic(sql, sp);
            return query;
        }
        #endregion
        #region 移动端历史曲线
        public IEnumerable<dynamic> HistroyChartData(string project, string sort, string group, string collectName, DateTime begindate, DateTime enddate)
        {
            //定义参数
            string matchstart = @"{'$match':{ 'UpdateTime': { '$gte': ISODate('" + begindate.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'") + "'),'$lt':ISODate('" + enddate.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'") + "')}" +
                           " }" +
                          "}";

            //定义mongodb
            string _conntionString = StaticConstraint.MongoDBConn;
            string _dbName = StaticConstraint.MongoDBName;
            //string connectstring = _conntionString + "/" + _dbName ;
            var client = new MongoClient(_conntionString);
            var database = client.GetDatabase(_dbName + begindate.Year);
           
            PipelineStageDefinition<dynamic, dynamic> matchjson = new JsonPipelineStageDefinition<dynamic, dynamic>(matchstart);
            PipelineStageDefinition<dynamic, dynamic> projectjson = new JsonPipelineStageDefinition<dynamic, dynamic>(project);
            PipelineStageDefinition<dynamic, dynamic> groupjson = new JsonPipelineStageDefinition<dynamic, dynamic>(group);
            PipelineStageDefinition<dynamic, dynamic> sortjson = new JsonPipelineStageDefinition<dynamic, dynamic>(sort);

            IList<IPipelineStageDefinition> stages = new List<IPipelineStageDefinition>();
            stages.Add(matchjson);
            stages.Add(groupjson);
            stages.Add(projectjson);
            stages.Add(sortjson);

            //查询
            PipelineDefinition<dynamic, dynamic> pipeline = new PipelineStagePipelineDefinition<dynamic, dynamic>(stages);
            var result = database.GetCollection<dynamic>(collectName).Aggregate(pipeline);
            return result.ToList();
        }
        #endregion
    }
}
