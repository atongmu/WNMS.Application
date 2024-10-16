using System;
using System.Collections.Generic;
using System.Text;
using WNMS.IService;
using WNMS.Utility;
using Microsoft.Data.SqlClient;
using System.Linq;
using WNMS.Model.DataModels;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace WNMS.Service
{
    public partial class Sws_AccessControlService : BaseService, ISws_AccessControlService
    {
        //门禁监控左侧泵房状态
        public IEnumerable<dynamic> GetStationTreeOfState(string goodRtuids,bool isadmin,int Userid,string stationname)
        {
            List<int> ids_rtuonline = new List<int>();
            if (!string.IsNullOrEmpty(goodRtuids))
            {
                ids_rtuonline = new List<string>(goodRtuids.Split(",")).ConvertAll(r => int.Parse(r));
            }
            DataTable tvpDt = new DataTable();
            tvpDt.Columns.Add("RTUID", typeof(int));
            foreach (var item in ids_rtuonline)
            {
                tvpDt.Rows.Add(item);
            }
            SqlParameter[] sp = new SqlParameter[] {
              
                new SqlParameter("@Userid",Userid),
                new SqlParameter("@stationname","%" + stationname + "%"),
                new SqlParameter("@onlineRtu", SqlDbType.Structured) { Value = tvpDt, TypeName = "OnlineRtuIDTVP" }
            };
            StringBuilder sb = new StringBuilder();
           
            if (isadmin == true)
            {
                //基础数据
                sb.Append(@"with t as( select StationID,StationName,CameraMonitor,DoorInsert as HasDoor from [dbo].[Sws_Station] 
  where StationName like @stationname),");
            }
            else
            {
                //基础数据
                sb.Append(@" with t as( select s.StationID,StationName,s.CameraMonitor,DoorInsert as HasDoor from [dbo].[Sws_Station] s
 left join [dbo].[Sws_UserStation] u on s.StationID=u.StationID
 where UserID=@Userid and StationName like @stationname),");
            }
            //报警
            sb.Append(@"a1 as(
 select distinct StationID from [dbo].[Sws_EventInfo] e
 left join [dbo].[Sws_RTUInfo] r on e.RTUID=r.RTUID where e.EventLevel!=0
 and e.RTUID in (select * from @onlineRtu)),
 alarm as(
 select t.*,'报警' as state from t
 left join a1 on t.StationID=a1.StationID where a1.StationID is not null),");
            //在线
            sb.Append(@"online1 as (
 select distinct StationID from [dbo].[Sws_RTUInfo] where RTUID in (select * from @onlineRtu))," +
 @"online as(
 select t.*,'在线' as state from  t
 left join online1 on t.StationID=online1.StationID
 left join a1 on t.StationID=a1.StationID
 where a1.StationID is null and online1.StationID is not null),");
            //离线
            sb.Append(@" alarmOnline as(
 select * from alarm
 union select * from online
 ),offline as(
 select t.*,'离线' as type from t 
 left join alarmOnline on t.StationID=alarmOnline.StationID
 where alarmOnline.StationID is null)");
            //最终数据
            sb.Append(@"select * from alarm
 union select * from online
 union select * from offline order by HasDoor desc");
            string sql = sb.ToString();
            var query = this.Context.Database.SqlQuery_Dic(sql, sp);
            return query;
        }
        //门禁监控历史数据
        public IEnumerable<dynamic> GetAccessHistory(int stationid,int pageindex,int pagesize,string filter,string orderby,ref int Totalcount)
        {
            SqlParameter[] sp = new SqlParameter[]
            {
                new SqlParameter("@stationid",stationid),
                new SqlParameter("@startindex",pageindex),
                new SqlParameter("@pageSize",pagesize),
                new SqlParameter("@orderItems",orderby),
                new SqlParameter("@filterString",filter),
                new SqlParameter("@Totalcount",Totalcount){Direction=System.Data.ParameterDirection.Output,DbType=System.Data.DbType.Int32}
            };
            var query = this.Context.Database.SqlQuery_Dic("exec QueryAccessHistaory @stationid,@startindex,@pageSize,@orderItems,@filterString,@Totalcount Output", sp).ToList();

            Totalcount = Convert.ToInt32(sp[5].Value);
            return query;

        }
        //门禁信息表格数据查询
        public IEnumerable<dynamic> GetAcessControlTable(int f_BrandItemID,int startindex,int pageSize,string orderItems,string filterString,ref int Totalcount)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@f_BrandItemID",f_BrandItemID),
                new SqlParameter("@startindex",startindex),
                new SqlParameter("@pageSize",pageSize),
                new SqlParameter("@orderItems",orderItems),
                new SqlParameter("@filterString",filterString),
                new SqlParameter("@Totalcount",Totalcount){Direction=System.Data.ParameterDirection.Output,DbType=System.Data.DbType.Int32}
            };
            var query = this.Context.Database.SqlQuery_Dic("exec QueryAccessCtrolTable @f_BrandItemID,@startindex,@pageSize,@orderItems,@filterString,@Totalcount Output", sp).ToList();

            Totalcount = Convert.ToInt32(sp[5].Value);
            return query;
        }
        //删除门禁信息
        public int DeleteAccessControl(List<int> doorids)
        {

            var deleteAccessControl = this.Context.Set<SwsAccessControl>().Where(r => doorids.Contains(r.DoorId));
            var stationIDList = deleteAccessControl.Select(r => r.StationId).Distinct();
            var noDelAccessControl= this.Context.Set<SwsAccessControl>().Where(r => !doorids.Contains(r.DoorId));
            if (noDelAccessControl.Count() > 0)
            {
                var noOperateStationids = noDelAccessControl.Select(r => r.StationId).Distinct();
                stationIDList = stationIDList.Except(noOperateStationids);//需要更新的泵房集合
            }


            //查询相关的泵房
            var swsSations= this.Context.Set<SwsStation>().Where(r => stationIDList.Contains(r.StationId));
            foreach (var item in swsSations)
            {
                item.DoorInsert = false;
                this.Context.Set<SwsStation>().Update(item);//删除门禁的时候，相关的泵房门禁清0
            }
            
            //删除门禁
            
            this.Context.Set<SwsAccessControl>().RemoveRange(deleteAccessControl);
            return this.Context.SaveChanges();
        }
        //门禁选择泵房、泵房列表
        public IEnumerable<dynamic> GetAC_StationTable(int stationid,int f_typeItemID,int startindex,int pageSize,string orderItems,string filterString,ref int Totalcount)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@stationid",stationid),
                new SqlParameter("@f_typeItemID",f_typeItemID),
                new SqlParameter("@startindex",startindex),
                new SqlParameter("@pageSize",pageSize),
                new SqlParameter("@orderItems",orderItems),
                new SqlParameter("@filterString",filterString),
                new SqlParameter("@Totalcount",Totalcount){Direction=System.Data.ParameterDirection.Output,DbType=System.Data.DbType.Int32}
                 };
             var query = this.Context.Database.SqlQuery_Dic("exec QueryAccess_StationTable @stationid,@f_typeItemID,@startindex,@pageSize,@orderItems,@filterString,@Totalcount Output", sp).ToList();

            Totalcount = Convert.ToInt32(sp[6].Value);
            return query;
        }
        //门禁添加
        public int InsertAccessControl(SwsAccessControl a)
        {
            this.Context.Set<SwsAccessControl>().Add(a);
            var station = this.Context.Set<SwsStation>().Where(r => r.StationId == a.StationId).FirstOrDefault();
            if (station != null)
            {
                if (station.DoorInsert == false)
                {
                    station.DoorInsert = true;
                    this.Context.Set<SwsStation>().Update(station);
                }
            }
            return this.Context.SaveChanges();
        }
        //门禁修改
        public int UpdateAccessControl(SwsAccessControl a)
        {
            //首先判断所选泵房是否变化

            //先判断之前的门禁对应的泵房，若对应的泵房只有一个门禁，则将泵房表的DoorInsert设为0 ，否则不置0；
            //再判断目前门禁对应的泵房，DoorInsert是否为0，若为0则置1，否则不更新

            var oldAccess = this.Context.Set<SwsAccessControl>().Where(r => r.DoorId == a.DoorId).AsNoTracking().FirstOrDefault();
            if (a.StationId == oldAccess.StationId)//未更改泵房
            {

            }
            else//更改泵房
            {
                //之前选中的泵房拥有的（除了这条）门禁
                var oldStationOwnAccess = this.Context.Set<SwsAccessControl>().Where(r => r.StationId == oldAccess.StationId && r.DoorId != a.DoorId).AsNoTracking().ToList();
                if (oldStationOwnAccess.Count == 0)
                {
                    var oldStation = this.Context.Set<SwsStation>().Where(r => r.StationId == oldAccess.StationId).FirstOrDefault();
                    if (oldStation != null)
                    {
                        oldStation.DoorInsert = false;
                        this.Context.Set<SwsStation>().Update(oldStation);
                    }
                }
                //当前泵房
                var newStation= this.Context.Set<SwsStation>().Where(r => r.StationId == a.StationId).FirstOrDefault();
                if (newStation != null)
                {
                    if (newStation.DoorInsert == false)
                    {
                        newStation.DoorInsert = true;
                        this.Context.Set<SwsStation>().Update(newStation);
                    }
                }
            }

            //var station = this.Context.Set<SwsStation>().Where(r => r.StationId == a.StationId).FirstOrDefault();

            //if (a.DoorId != station.DoorId)//判断是否更改泵房
            //{
            //    station.DoorId = a.DoorId;
            //    this.Context.Set<SwsStation>().Update(station);//将新选的泵房的doorid设置成doorid
            //    var oldstation = this.Context.Set<SwsStation>().Where(r => r.DoorId == a.DoorId).FirstOrDefault();
            //    oldstation.DoorId = 0;
            //    this.Context.Set<SwsStation>().Update(oldstation);//将此doorid对应的泵房的doorid设置为0

            //}
            this.Context.Set<SwsAccessControl>().Update(a);
            return this.Context.SaveChanges();

        }
    }
}
