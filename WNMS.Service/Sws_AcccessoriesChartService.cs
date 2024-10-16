using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using WNMS.IService;
using WNMS.Model.CustomizedClass;
using WNMS.Model.DataModels;
using WNMS.Utility;

namespace WNMS.Service
{
    public partial class Sws_AcccessoriesChartService : BaseService, ISws_AcccessoriesChartService
    {
        #region 器件生命周期 查询设备树（泵房+设备）
        /// <summary>
        /// 查询设备树（供水设备+直饮水设备）
        /// </summary>
        /// <param name="user">登录用户信息</param>
        /// <param name="stationName">泵房名称</param>
        /// <returns></returns>
        public IEnumerable<WNMS.Model.CustomizedClass.StationDevice> LoadZtreeInfo(SysUser user, string stationName)
        {
            string sql = "";
            int userID = user.UserId;
            List<SqlParameter> splist = new List<SqlParameter>();
            stationName = stationName ?? "";
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@stationName", "%" + stationName + "%"),
                new SqlParameter("@userID",userID)
            };
            if (user.IsAdmin)
            {
                sql = @"select d.DeviceId,d.DeviceName,d.Partition,s.StationId,s.StationName,2 as EquType from Sws_DeviceInfo02 d left join Sws_Station s on d.StationID=s.StationID where s.StationId>0 union
                  select d.DeviceId,d.DeviceName,d.Partition,s.StationId,s.StationName,1 as EquType from Sws_DeviceInfo01 d left join Sws_Station s on d.StationID=s.StationID where s.StationId>0";
            }
            else
            {
                sql = @"select d.DeviceId,d.DeviceName,d.Partition,s.StationId,s.StationName,2 as EquType from Sws_DeviceInfo02 d left join Sws_Station s on d.StationID=s.StationID
                        where d.StationID in (select StationID from Sws_UserStation where UserID=@userID) and s.StationId>0
                  union select d.DeviceId,d.DeviceName,d.Partition,s.StationId,s.StationName,1 as EquType from Sws_DeviceInfo01 d left join Sws_Station s on d.StationID=s.StationID
                        where d.StationID in (select StationID from Sws_UserStation where UserID=@userID) and s.StationId>0";
            }

            if (!string.IsNullOrEmpty(stationName))
            {
                sql = @"with t as (" + sql + "), g as(select EquipmentID,EquType,count(ID) as IDCount from [dbo].[Sws_AccessoriesEqu] where EndDate is null group by EquipmentID,EquType) select t.DeviceId,t.DeviceName,t.StationId,t.StationName,t.EquType,isnull(g.IDCount, 0) as Total from t left join g on t.DeviceID = g.EquipmentID and t.EquType = g.EquType  where g.IDCount>0 and t.StationName like @stationName";
            }
            else
            {
                sql = @"with t as (" + sql + "), g as(select EquipmentID,EquType,count(ID) as IDCount from [dbo].[Sws_AccessoriesEqu] where EndDate is null group by EquipmentID,EquType) select t.DeviceId,t.DeviceName,t.StationId,t.StationName,t.EquType,isnull(g.IDCount, 0) as Total from t left join g on t.DeviceID = g.EquipmentID and t.EquType = g.EquType where g.IDCount>0";
            }
            sql = sql + @" order by t.Partition,t.EquType";
            var query = this.Context.Database.SqlQuery<WNMS.Model.CustomizedClass.StationDevice>(sql, sp);
            return query;
        }


        /// <summary>
        /// 查询器件树
        /// </summary>
        /// <param name="id">设备id</param>
        /// <param name="type">设备类型</param>
        /// <returns></returns>
        public IEnumerable<WNMS.Model.CustomizedClass.AccessEquInfo> LoadAccZtree(long id, byte type)
        {
            string sql = "";
            List<SqlParameter> splist = new List<SqlParameter>();
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@id", id),
                new SqlParameter("@type",type)
            };

            sql = @"select a.ID,a.AccessoriesID,a.EquipmentID,b.Name from Sws_AccessoriesEqu a left join Sws_Accessories b on a.AccessoriesID=b.ID where a.EquipmentID=@id and a.EquType=@type and a.EndDate is null order by a.ID";

            var query = this.Context.Database.SqlQuery<WNMS.Model.CustomizedClass.AccessEquInfo>(sql, sp);
            return query;
        }
        #endregion

        #region 器件生命周期 查询器件曲线数据
        /// <summary>
        /// 器件曲线数据
        /// </summary>
        /// <param name="id">器件ID</param>
        /// <returns></returns>
        public IEnumerable<AccChartData> LoadAccChartData(long id)
        {
            var query = from t in Context.Set<SwsAccessoriesEqu>()
                        join a in Context.Set<SwsAcccessoriesChart>() on t.AccessoriesId equals a.AccessoriesId into ta
                        from tar in ta.DefaultIfEmpty()
                        where t.Id == id
                        select new AccChartData
                        {
                            AccessoriesId = t.AccessoriesId,
                            ChartData = tar.ChartData,
                            DetailID = t.Id
                        };
            return query.ToList();
        }
        #endregion

        #region 批量导入
        /// <summary>
        /// 批量导入
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public int ChartDataImport(List<SwsAcccessoriesChart> list)
        {
            try
            {
                foreach (var item in list)
                {
                    this.Context.Add<SwsAcccessoriesChart>(item);
                }
                this.Context.SaveChanges();
            }
            catch (Exception ex)
            {
                return -1;
            }
            return 1;
        }
        #endregion

        #region 模板导出
        public IEnumerable<AccessoriesDatas> ImportChartsData()
        {
            var query = from t in Context.Set<SwsAccessories>()
                        join a in Context.Set<SwsAcccessoriesChart>() on t.Id equals a.AccessoriesId into ta
                        from tar in ta.DefaultIfEmpty()
                        where tar.ChartData == null
                        select new AccessoriesDatas
                        {
                            Id = t.Id,
                            Name = t.Name,
                            ChartData = tar.ChartData
                        };
            return query.ToList();
        }
        #endregion
    }
}
