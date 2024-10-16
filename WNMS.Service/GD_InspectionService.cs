using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using WNMS.IService;
using WNMS.Utility;
using System.Linq;
using WNMS.Model.DataModels;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace WNMS.Service
{
    public partial class GD_InspectionService : BaseService, IGD_InspectionService
    {
        //提交巡检信息和事件信息
        public int AddEventIn(GdInspection gdInspection, GdEvents gdEvents)
        {
            try
            {
                this.Context.Add(gdInspection);
                this.Context.Add(gdEvents);
                this.Context.SaveChanges();
            }
            catch (Exception ex)
            {
                return -1;
            }
            return 1;

        }
        //提交保养信息和事件信息
        public int AddEventMa(GdMaintain gdMaintain, GdEvents gdEvents)
        {
            try
            {
                this.Context.Add(gdMaintain);
                this.Context.Add(gdEvents);
                this.Context.SaveChanges();
            }
            catch (Exception ex)
            {
                return -1;
            }
            return 1;

        }
        //提交维修信息和事件信息
        public int AddEventRe(GdRepair gdRepair, GdEvents gdEvents)
        {
            try
            {
                this.Context.Add(gdRepair);
                this.Context.Add(gdEvents);
                this.Context.SaveChanges();
            }
            catch (Exception ex)
            {
                return -1;
            }
            return 1;

        }
        #region 巡检反馈信息
        public IEnumerable<dynamic> GetInspectionTable(bool isadmin, int pageindex, int pagesize, string orderby, string filter, ref int Totalcount)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@isadmin",isadmin),
                new SqlParameter("@startindex",pageindex),
                new SqlParameter("@pageSize",pagesize),
                new SqlParameter("@orderItems",orderby),
                new SqlParameter("@filterString",filter),
                new SqlParameter("@Totalcount",Totalcount){Direction=System.Data.ParameterDirection.Output,DbType=System.Data.DbType.Int32}
            };
            var query = this.Context.Database.SqlQuery_Dic("exec QueryGD_InspectionTable @isadmin,@startindex,@pageSize,@orderItems,@filterString,@Totalcount Output", sp).ToList();
            Totalcount = Convert.ToInt32(sp[5].Value);
            return query;
        }
        //班组人员树
        public IEnumerable<dynamic> GetTeamUserTree()
        {
            SqlParameter[] sp = new SqlParameter[] { };
            var sql = @"  select tu.*,TeamName,Account from [dbo].[GD_TeamUser] tu
  left join [dbo].[Sys_User] u on tu.UserID=u.UserID
  left join [dbo].[GD_TeamInfo] t on tu.TeamID=t.TeamID";
            var query = this.Context.Database.SqlQuery_Dic(sql, sp);
            return query;
        }
        //删除巡检反馈信息
        public int DeleteXJFeedBack(List<int> XJIDs)
        {
            var xjlist = this.Context.Set<GdInspection>().Where(r => XJIDs.Contains(r.InspectionId)).AsNoTracking();
            foreach (var item in xjlist)
            {
                GdInspection p = new GdInspection();
                p.InspectionId = item.InspectionId;
                p.CreateTime = item.CreateTime;
                p.Num = item.Num;
                p.TaskDescription = item.TaskDescription;
                p.StationId = item.StationId;
                this.Context.Set<GdInspection>().Update(p);
            }
            return this.Context.SaveChanges();
        }
        //泵房选择
        public IEnumerable<dynamic> GetGD_SelectStationTable(bool isadmin, int stationid, int f_typeItemID, int startindex, int pageSize, string orderItems, string filterString, ref int Totalcount)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@isadmin",isadmin),
                new SqlParameter("@stationid",stationid),
                new SqlParameter("@f_typeItemID",f_typeItemID),
                new SqlParameter("@startindex",startindex),
                new SqlParameter("@pageSize",pageSize),
                new SqlParameter("@orderItems",orderItems),
                new SqlParameter("@filterString",filterString),
                new SqlParameter("@Totalcount",Totalcount){Direction=System.Data.ParameterDirection.Output,DbType=System.Data.DbType.Int32}
                 };
            var query = this.Context.Database.SqlQuery_Dic("exec QueryGD_SelectStationTable @isadmin,@stationid,@f_typeItemID,@startindex,@pageSize,@orderItems,@filterString,@Totalcount Output", sp).ToList();

            Totalcount = Convert.ToInt32(sp[7].Value);
            return query;
        }
        #endregion
        #region 维修反馈信息
        public IEnumerable<dynamic> GetRepairTable(bool isadmin, int pageindex, int pagesize, string orderby, string filter, ref int Totalcount)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@isadmin",isadmin),
                new SqlParameter("@startindex",pageindex),
                new SqlParameter("@pageSize",pagesize),
                new SqlParameter("@orderItems",orderby),
                new SqlParameter("@filterString",filter),
                new SqlParameter("@Totalcount",Totalcount){Direction=System.Data.ParameterDirection.Output,DbType=System.Data.DbType.Int32}
            };
            var query = this.Context.Database.SqlQuery_Dic("exec QueryGD_RepairTable @isadmin,@startindex,@pageSize,@orderItems,@filterString,@Totalcount Output", sp).ToList();
            Totalcount = Convert.ToInt32(sp[5].Value);
            return query;
        }
        //根据泵房id查询设备
        public IEnumerable<dynamic> GetDeviceList_ByStationID(string tablename, int stationid, long deviceid)
        {
            var f_itemid = (int)WNMS.Model.CustomizedClass.Enum.设备分区;
            SqlParameter[] sp = new SqlParameter[] {
                 new SqlParameter("@f_itemid",f_itemid),
                 new SqlParameter("@tablename",tablename),
                 new SqlParameter("@stationid",stationid),
                 new SqlParameter("@deviceid",deviceid)
            };
            var sql = @"  select DeviceID,ItemName as deviceName from [dbo].[" + @tablename + "] d " +
  @"left join (select * from [dbo].[Sys_DataItemDetail] where F_ItemId=@f_itemid and IsEnable=1) g on d.Partition=g.ItemValue
  where StationID=@stationid";
            if (deviceid != 0)
            {
                sql += " and DeviceID=@deviceid";
            }
            var query = this.Context.Database.SqlQuery_Dic(sql, sp);
            return query;
        }
        //添加,编辑
        public int UpdateRepairInfo(GdRepair p, List<GdResource> resourceList)
        {
            this.Context.Set<GdRepair>().Update(p);
            //查询之前的附件
            //var oldresource = this.Context.Set<GdResource>().Where(r => r.Pid == p.RepairId && r.ResourceType == 3);
            //if (oldresource.Count() > 0)
            //{
            //    this.Context.Set<GdResource>().RemoveRange(oldresource);
            //}
            if (resourceList.Count() > 0)
            {
                this.Context.Set<GdResource>().AddRange(resourceList);
            }
            return this.Context.SaveChanges();
        }
        //删除维修反馈信息
        public int DeleteWXFeedBack(List<long> WXIDs, string RootPath)
        {
            var wxlist = this.Context.Set<GdRepair>().Where(r => WXIDs.Contains(r.RepairId)).AsNoTracking();
            foreach (var item in wxlist)
            {
                GdRepair p = new GdRepair();
                p.RepairId = item.RepairId;
                p.CreateTime = item.CreateTime;
                p.Num = item.Num;
                p.StationId = item.StationId;
                p.FaultDescription = item.FaultDescription;
                p.FaultContent = item.FaultContent;

                this.Context.Set<GdRepair>().Update(p);
            }
            var resouce = this.Context.Set<GdResource>().Where(r => WXIDs.Contains(r.Pid) && r.ResourceType == 3);
            foreach (var item in resouce)
            {
                var indexlast = item.Path.LastIndexOf("\\");
                string localPath = Path.Combine(RootPath, item.Path.Substring(1, indexlast - 1));
                //删除文件
                if (Directory.Exists(localPath))
                {
                    Directory.Delete(localPath, true);
                }
                this.Context.Set<GdResource>().Remove(item);
            }
            return this.Context.SaveChanges();
        }
        #endregion
        #region 保养反馈信息
        public IEnumerable<dynamic> GetMainTainTable(bool isamin, int pageindex, int pagesize, string orderby, string filter, ref int Totalcount)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@isadmin",isamin),
                new SqlParameter("@startindex",pageindex),
                new SqlParameter("@pageSize",pagesize),
                new SqlParameter("@orderItems",orderby),
                new SqlParameter("@filterString",filter),
                new SqlParameter("@Totalcount",Totalcount){Direction=System.Data.ParameterDirection.Output,DbType=System.Data.DbType.Int32}
            };
            var query = this.Context.Database.SqlQuery_Dic("exec QueryMainTainTable @isadmin,@startindex,@pageSize,@orderItems,@filterString,@Totalcount Output", sp).ToList();
            Totalcount = Convert.ToInt32(sp[5].Value);
            return query;
        }
        //添加,编辑
        public int UpdateMainTainInfo(GdMaintain p, List<GdResource> resourceList)
        {
            this.Context.Set<GdMaintain>().Update(p);
            ////查询之前的附件
            //var oldresource = this.Context.Set<GdResource>().Where(r => r.Pid == p.MaintainId && r.Type == 4);
            //if (oldresource.Count() > 0)
            //{
            //    this.Context.Set<GdResource>().RemoveRange(oldresource);
            //}
            if (resourceList.Count() > 0)
            {
                this.Context.Set<GdResource>().AddRange(resourceList);
            }
            return this.Context.SaveChanges();
        }
        //删除维修反馈信息
        public int DeleteBYFeedBack(List<long> WXIDs, string RootPath)
        {
            var wxlist = this.Context.Set<GdMaintain>().Where(r => WXIDs.Contains(r.MaintainId)).AsNoTracking();
            foreach (var item in wxlist)
            {
                GdMaintain p = new GdMaintain();
                p.MaintainId = item.MaintainId;
                p.CreateTime = item.CreateTime;
                p.Num = item.Num;
                p.StationId = item.StationId;
                p.TaskDescription = item.TaskDescription;


                this.Context.Set<GdMaintain>().Update(p);
            }
            var resouce = this.Context.Set<GdResource>().Where(r => WXIDs.Contains(r.Pid) && r.ResourceType == 4);
            foreach (var item in resouce)
            {
                var indexlast = item.Path.LastIndexOf("\\");
                string localPath = Path.Combine(RootPath, item.Path.Substring(1, indexlast - 1));
                //删除文件
                if (Directory.Exists(localPath))
                {
                    Directory.Delete(localPath, true);
                }
                this.Context.Set<GdResource>().Remove(item);
            }
            return this.Context.SaveChanges();
        }
        #endregion
        #region 业务反馈相关接口
        public GdEvents GetGdEventByWorkID(long workid)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@workid",workid)
            };
            string sql = @"
  select e.* from [dbo].[GD_Events] e
  left join [dbo].[GD_WorkOrder] w on e.IncidentID=w.EventID
  where WOID=@workid
";
            var query = this.Context.Database.SqlQuery<GdEvents>(sql, sp).FirstOrDefault();
            return query;
        }
        public IEnumerable<dynamic> GetModelByID(int type, int id)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@id",id)
          };
            string sql = "";
            if (type == 0)//巡检
            {
                sql = @"select p.*,s.StationName,u.Account  from [dbo].[GD_Inspection] p
  left join [dbo].[Sws_Station] s on p.StationID=s.StationID
  left join [dbo].[Sys_User] u on p.InspectionUser=u.UserID where InspectionID=@id";
            }
            else if (type == 1)//保养
            {
                sql = @"  select m.*,s.StationName,u.Account from [dbo].[GD_Maintain] m
  left join [dbo].[Sws_Station] s on m.StationID=s.StationID
  left join [dbo].[Sys_User] u on m.MaintainUser=u.UserID where MaintainID=@id";
            }
            else//维修
            {
                sql = @"  select r.*,s.StationName,u.Account from [dbo].[GD_Repair] r
 left join [dbo].[Sws_Station] s on r.StationID=s.StationID
  left join [dbo].[Sys_User] u on r.RepairUser=u.UserID where RepairID=@id";
            }
            var query = this.Context.Database.SqlQuery_Dic(sql, sp);
            return query;
        }
        #endregion
    }
}
