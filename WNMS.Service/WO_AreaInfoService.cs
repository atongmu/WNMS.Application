using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using WNMS.IService;
using WNMS.Model.DataModels;
using WNMS.Utility;

namespace WNMS.Service
{
    public partial class WO_AreaInfoService : BaseService, IWO_AreaInfoService
    {

        //添加、修改区域
        public int SetAreaInfo(int areaID, int pid, string areaName, string fillColor, string points, int type)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@points",points),
                new SqlParameter("@areaID",areaID),
                new SqlParameter("@type",type),
                new SqlParameter("@pid",pid),
                new SqlParameter("@areaName",areaName),
                new SqlParameter("@fillColor",fillColor)
           };
            var query = this.Context.Database.InsertData("exec GetRtuOfArea  @points,@areaID,@type,@pid,@areaName,@fillColor", sp);
            return query;
        }

        //根据区域id获取区域信息以及设备数量
        public IEnumerable<dynamic> GetAreaInfo(string areaIDs)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@areaIDs",areaIDs)
            };
            var sql = @"select ID, AreaName, GISPoints.ToString() as GISPoints, PID, FillColor ,case when countnum is null then 0 else countnum end countnum from  [dbo].[WO_AreaInfo] a
 left join (select AreaID,count(*) as countnum from [dbo].[WO_AreaRTU] ar
left join [dbo].[Sws_Station] rtu on ar.StationID=rtu.StationID where rtu.StationID is not null group by AreaID) r on a.ID=r.AreaID
where  CHARINDEX(','+LTRIM(a.ID)+',',','+@areaIDs+ ',')>0";
            var query = this.Context.Database.SqlQuery_Dic(sql, sp).ToList();
            return query;
        }

        //查询区域内的设备
        public IEnumerable<dynamic> GetRtuInfoOfArea(int areaID, string deviceName)
        {
            SqlParameter[] sp = new SqlParameter[] {
                 new SqlParameter("@areaID",areaID),
                 new SqlParameter("@deviceName","%"+deviceName+"%")
            };
            string str = "";
            if (!string.IsNullOrEmpty(deviceName))
            {
                str = " and DeviceName like @deviceName ";
            }
            string sql = @"select DeviceID,DeviceName,t.Lng,t.Lat from [dbo].[Sws_DeviceInfo01] d left join [dbo].[Sws_Station] t on d.StationID=t.StationID
                where DeviceID in (select EquipmentID from [dbo].[WO_AreaRTU] where AreaID=@areaID)" + str + " union select " + @" DeviceID,DeviceName,t.Lng,t.Lat from [dbo].[Sws_DeviceInfo02] d left join [dbo].[Sws_Station] t on d.StationID=t.StationID
                where DeviceID in (select EquipmentID from [dbo].[WO_AreaRTU] where AreaID= @areaID) " + str + "";
            var query = this.Context.Database.SqlQuery_Dic(sql, sp).ToList();
            return query;
        }


        //查询区域内设备并分页
        public IEnumerable<dynamic> GetRtuInfoOfAreaPage(int areaID, string sort, int pageSize, int pageIndex, string devicename, ref int TotalCount)
        {

            var query = (from a in Query<WoAreaRtu>(s => s.AreaId == areaID)
                         join r in Query<SwsDeviceInfo01>(s => true) on a.EquipmentId equals r.DeviceId into rr
                         from rrr in rr.DefaultIfEmpty()
                         join d in Query<SwsStation>(s => true) on rrr.StationId equals d.StationId into rs
                         from rst in rs.DefaultIfEmpty()
                         where rrr != null
                         select new
                         {
                             DeviceID = rrr.DeviceId,
                             DeviceName = rrr.DeviceName,
                             Lng = rst.Lng,
                             Lat = rst.Lat
                         }).Union(from a in Query<WoAreaRtu>(s => s.AreaId == areaID)
                                  join r in Query<SwsDeviceInfo02>(s => true) on a.EquipmentId equals r.DeviceId into rr
                                  from rrr in rr.DefaultIfEmpty()
                                  join d in Query<SwsStation>(s => true) on rrr.StationId equals d.StationId into rs
                                  from rst in rs.DefaultIfEmpty()
                                  where rrr != null
                                  select new
                                  {
                                      DeviceID = rrr.DeviceId,
                                      DeviceName = rrr.DeviceName,
                                      Lng = rst.Lng,
                                      Lat = rst.Lat
                                  });
            if (!string.IsNullOrEmpty(devicename))
            {
                query = query.Where(r => r.DeviceName.Contains(devicename));
            }
            TotalCount = query.Count();
            if (sort == "asc")
            {
                query = query.OrderBy(r => r.DeviceName);
            }
            else
            {
                query = query.OrderByDescending(r => r.DeviceName);
            }
            query = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
            return query;
        }


        //区域删除
        public bool DeleteAreaInfo(int id, ref List<string> areanames)
        {
            //需要判断该区域与其下的区域是否被用到，用到的不能删，子区域若被用其父区域也不能删
            var query = this.Context.Set<WoAreaInfo>().Where(r => r.Id == id || r.Pid == id).ToList();
            var areaids = query.Select(s => s.Id).ToList();
            //判断区域以及子区域是否有被占用的
            var listOccupy = this.Context.Set<WoInspectionPlan>().Where(r => areaids.Contains(r.Dmaid)).Select(s => s.Dmaid).ToList();

            if (listOccupy.Count() > 0)
            {

                IEnumerable<WoAreaInfo> datas = query.Where(r => listOccupy.Contains(r.Id));
                foreach (var item in datas)
                {
                    areanames.Add(item.AreaName);
                }
            }
            else
            {

                //foreach (var item in query)
                //{
                this.Context.Set<WoAreaInfo>().RemoveRange(query);
                //}
                var areaRtus = this.Context.Set<WoAreaRtu>().Where(r => areaids.Contains(r.AreaId)).ToList();
                //foreach (var item in areaRtus)
                //{
                //    this.Context.Set<WoAreaRtu>().RemoveRange(item);
                //}
                this.Context.Set<WoAreaRtu>().RemoveRange(areaRtus);
            }
            int res = this.Context.SaveChanges();
            if (res > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public IEnumerable<dynamic> SearchAreaTree(string areaName)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@areaName","%"+areaName+"%")
            };
            var sql = @"  with t as (select ID,PID from [dbo].[WO_AreaInfo] where AreaName like @areaName) " +
                      @"select distinct ID, AreaName, PID, FillColor from [dbo].[WO_AreaInfo] where ID in (select ID from t) 
   or ID in (select PID from t) or PID in (select ID FROM t ) ";
            var query = this.Context.Database.SqlQuery_Dic(sql, sp);
            return query;
        }
    }
}
