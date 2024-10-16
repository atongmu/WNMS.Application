using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Text;
using WNMS.IService;
using WNMS.Utility;
using System.Linq;
using WNMS.Model.DataModels;

namespace WNMS.Service
{
    public partial class Sws_AccessoriesMaintenanceService : BaseService, ISws_AccessoriesMaintenanceService
    {
        public IEnumerable<dynamic> QueryMaintenanceTable(bool IsAdmin, int UserID, int pageindex, int pagesize, string order, string filter, ref int Totalcount)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@IsAdmin",IsAdmin),
                new SqlParameter("@UserID",UserID),
                new SqlParameter("@startindex",pageindex),
                new SqlParameter("@pageSize",pagesize),
                new SqlParameter("@orderItems",order),
                new SqlParameter("@filterString",filter),
                new SqlParameter("@Totalcount",Totalcount){Direction=System.Data.ParameterDirection.Output,DbType=System.Data.DbType.Int32}
            };
            var query = this.Context.Database.SqlQuery_Dic("exec QueryMaintenanceTable @IsAdmin,@UserID,@startindex,@pageSize,@orderItems,@filterString,@Totalcount Output", sp).ToList();

            Totalcount = Convert.ToInt32(sp[6].Value);
            return query;
        }

        //删除 
        public int DeleteMaintenances(List<int> ids)
        {
            var ids_Info = this.Context.Set<SwsAccessoriesMaintenance>().Where(r => ids.Contains(r.Id)).ToList();
            foreach (var item in ids_Info)
            {
                this.Context.Set<SwsAccessoriesMaintenance>().RemoveRange(item);
            }
            return this.Context.SaveChanges();
        }

        //查询报警提醒
        public IEnumerable<dynamic> QueryMaintenanceRtTable(bool IsAdmin, int UserID, int pageindex, int pagesize, string order, string filter, ref int Totalcount)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@IsAdmin",IsAdmin),
                new SqlParameter("@UserID",UserID),
                new SqlParameter("@startindex",pageindex),
                new SqlParameter("@pageSize",pagesize),
                new SqlParameter("@orderItems",order),
                new SqlParameter("@filterString",filter),
                new SqlParameter("@Totalcount",Totalcount){Direction=System.Data.ParameterDirection.Output,DbType=System.Data.DbType.Int32}
            };
            var query = this.Context.Database.SqlQuery_Dic("exec QueryMaintenanceRtTable @IsAdmin,@UserID,@startindex,@pageSize,@orderItems,@filterString,@Totalcount Output", sp).ToList();

            Totalcount = Convert.ToInt32(sp[6].Value);
            return query;
        }

        //标记已读
        public int ReadMaintenances(List<long> ids)
        {
            var ids_Info = this.Context.Set<SwsAccessoriesRtMaintenance>().Where(r => ids.Contains(r.AccessoriesId)).ToList();
            foreach (var item in ids_Info)
            {
                this.Context.Set<SwsAccessoriesRtMaintenance>().RemoveRange(item);
            }
            return this.Context.SaveChanges();
        }
        //添加保养及更新器件表
        public int SetMaintenances(SwsAccessoriesMaintenance swsAccessoriesMaintenance, int isInsert)
        {
            if (isInsert == 0)
            {
                this.Context.AddRange(swsAccessoriesMaintenance);
            }
            else
            {
                this.Context.UpdateRange(swsAccessoriesMaintenance);
            }
            //更新器件表的保养时间
            var ac_info = this.Context.Set<SwsAccessoriesEqu>().Where(r => r.Id == swsAccessoriesMaintenance.AccessoriesDetailId).FirstOrDefault();
            if (ac_info != null)
            {
                ac_info.MaintenanceDate = swsAccessoriesMaintenance.MaintenanceDate;
                this.Context.UpdateRange(ac_info);
            }

            //删除提醒表的数据
            var rt_info = this.Context.Set<SwsAccessoriesRtMaintenance>().Where(r => r.AccessoriesId == swsAccessoriesMaintenance.AccessoriesDetailId).FirstOrDefault();
            if (rt_info != null)
            {
                this.Context.RemoveRange(rt_info);
            }

            return this.Context.SaveChanges();
        }
        class QueryIDNum
        {
            public int Num { get; set; }
        }
        //查询保养数量
        public int GetMaintenanceRtCount(int userid, bool isadmin)
        {
            StringBuilder sb = new StringBuilder();
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@userid",userid)

            };
            string countsql = "";
            if (isadmin == true)
            {
                sb.Append(@" with t as(select DeviceID,DeviceName from [dbo].[Sws_DeviceInfo01]
  union  select DeviceID,DeviceName from [dbo].[Sws_DeviceInfo02]),
  g as ( select distinct a.*,c.Name, t.DeviceName from [dbo].[Sws_AccessoriesRtMaintenance] a
  left join Sws_AccessoriesEqu as b on a.AccessoriesID = b.id
  left join Sws_Accessories c on b.AccessoriesID = c.ID
  left join t on t.DeviceID=b.EquipmentID
  WHERE t.DeviceID is not null )");
            }
            else
            {
                sb.Append(@"  with t1 as(select DeviceID,DeviceName,StationID from [dbo].[Sws_DeviceInfo01]
  union  select DeviceID,DeviceName,StationID from [dbo].[Sws_DeviceInfo02]),
  t as(select t1.DeviceID,t1.DeviceName from t1
  left join [dbo].[Sws_UserStation] us on t1.StationID=us.StationID where UserID=@userid),
  g as( select distinct a.*,c.Name, t.DeviceName  from [dbo].[Sws_AccessoriesRtMaintenance] a
  left join Sws_AccessoriesEqu as b on a.AccessoriesID = b.id
  left join Sws_Accessories c on b.AccessoriesID = c.ID 
  left join t on t.DeviceID=b.EquipmentID
  WHERE t.DeviceID is not null )");
            }
            countsql = sb.ToString() + " select count(*) as Num from g";
            var TotalCount = this.Context.Database.SqlQuery<QueryIDNum>(countsql, sp)[0].Num;
            return TotalCount;

        }

        /// <summary>
        /// 批量导入
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public int MaintenanceImport(List<SwsAccessoriesMaintenance> list)
        {
            try
            {
                foreach (var item in list)
                {
                    this.Context.Add<SwsAccessoriesMaintenance>(item);
                }
                this.Context.SaveChanges();
            }
            catch (Exception ex)
            {
                return -1;
            }
            return 1;
        }
    }
}
