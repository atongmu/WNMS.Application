using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using WNMS.IService;
using WNMS.Model.CustomizedClass;
using WNMS.Model.DataModels;
using WNMS.Utility;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace WNMS.Service
{
    public partial class Sws_CameraService : BaseService, ISws_CameraService
    {
        /// <summary>
        /// 获取摄像头信息
        /// </summary>
        /// <param name="funcWhere">筛选条件</param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="funcOrderby">排序</param>
        /// <param name="userID">用户名</param>
        /// <param name="ispage">是否分页</param>
        /// <param name="isAsc">是否为倒序</param>
        /// <returns></returns>
        public PageResult<CameraInfo> LoadInfoList(Expression<Func<CameraInfo, bool>> funcWhere, int pageSize, int pageIndex, string funcOrderby, int userID, bool ispage = true, bool isAsc = true)
        {
            SysUser user = this.Find<SysUser>(userID);

            SqlParameter[] sqlparameter = new SqlParameter[] {
                new SqlParameter("@userID",userID)
            };
            string sql = @"select c.CameraID,c.CameraName,c.CameraType,c.ChannelNum,c.IP,c.Port,s.StationID,s.StationName from [dbo].[Sws_Camera] c left join [dbo].[Sws_Station] s on c.StationID=s.StationID";

            if (!user.IsAdmin)     //非admin查询
            {
                sql += " where c.StationID in (select StationID from Sws_UserStation where UserID=@userID) or c.StationID=0";
            }

            PageResult<CameraInfo> presult = new PageResult<CameraInfo>();
            if (ispage)   //分页查询
            {
                presult = this.ExcuteQueryPage(funcWhere, pageSize, pageIndex, funcOrderby, sql, sqlparameter, isAsc);
            }
            else     //查询所有
            {
                List<CameraInfo> list = this.Context.Database.SqlQuery<CameraInfo>(sql, sqlparameter);
                presult.DataList = list;
            }
            return presult;
        }

        //添加摄像头
        public int AddCameraEntity(SwsCamera camera)
        {
            SwsStation station = this.Context.Find<SwsStation>(camera.StationId);
            if (station != null)
            {
                station.CameraMonitor = true;
                this.Context.Attach(station);
                this.Context.Update(station);
            }
            this.Context.Attach(camera);
            this.Context.Add(camera);
            return this.Context.SaveChanges();
        }

        //编辑摄像头
        public int EditCameraEntity(SwsCamera camera, int id)
        {
            this.Context.Attach(camera);
            if (id != camera.StationId)
            {
                List<SwsCamera> stationcamera = this.Query<SwsCamera>(r => r.StationId == id).ToList();
                if (stationcamera != null && stationcamera.Count == 1)
                {
                    SwsStation st = this.Context.Find<SwsStation>(id);
                    if (st != null)
                    {
                        st.CameraMonitor = false;
                        this.Context.Attach(st);
                        this.Context.Update(st);
                    }
                }
                SwsStation stnew = this.Context.Find<SwsStation>(camera.StationId);
                if (stnew != null)
                {
                    stnew.CameraMonitor = true;
                    this.Context.Attach(stnew);
                    this.Context.Update(stnew);
                }
            }

            this.Context.Update(camera);
            return this.Context.SaveChanges();
        }

        //编辑摄像头
        public int EditCameraEntitys(List<SwsCamera> cameralist, int id)
        {
            List<int> camIDs = cameralist.Select(r => r.CameraId).ToList();
            var cameralist_old = this.Query<SwsCamera>(r => r.StationId == id && !camIDs.Contains(r.CameraId)).AsNoTracking();
            //var cameralist_old = this.Context.Set<SwsCamera>().Where(r => r.StationId == id);
            if (cameralist_old.Count() > 0)
            {
                foreach (var item in cameralist_old)
                {
                    item.StationId = 0;
                    this.Context.Set<SwsCamera>().Update(item);
                }
            }
            if (cameralist.Count() > 0)
            {
                foreach (var item in cameralist)
                {
                    this.Context.Attach(item);
                }
                this.Context.UpdateRange(cameralist);
                SwsStation stnew = this.Query<SwsStation>(r => r.StationId == id).AsNoTracking().FirstOrDefault();
                if (stnew != null)
                {
                    stnew.CameraMonitor = true;
                    this.Context.Attach(stnew);
                    this.Context.Update(stnew);
                }
            }
            else
            {
                SwsStation stnew = this.Query<SwsStation>(r => r.StationId == id).AsNoTracking().FirstOrDefault();
                if (stnew != null)
                {
                    stnew.CameraMonitor = false;
                    this.Context.Attach(stnew);
                    this.Context.Update(stnew);
                }
            }
            return this.Context.SaveChanges();
        }
        //删除摄像头
        public int DeleteCameraEntity(List<int> ids)
        {
            List<SwsCamera> cameraList = this.Query<SwsCamera>(r => ids.Contains(r.CameraId)).ToList();
            foreach (var item in cameraList)
            {
                this.Context.Attach(item);
                List<SwsCamera> list = this.Query<SwsCamera>(r => r.StationId == item.StationId).ToList();
                if (list == null || list.Count == 1)
                {
                    SwsStation st = this.Find<SwsStation>(item.StationId);
                    if (st != null)
                    {
                        st.CameraMonitor = false;
                        this.Context.Attach(st);
                        this.Context.Update(st);
                    }
                }
            }
            this.Context.RemoveRange(cameraList);
            return this.Context.SaveChanges();
        }

        #region 视频接入
        public IEnumerable<CameraInfo> LoadCameraInfo(string condition, int userID)
        {
            SysUser user = this.Find<SysUser>(userID);

            SqlParameter[] sqlparameter = new SqlParameter[] {
                new SqlParameter("@userID",userID)
            };
            string sql = @"select c.CameraID,c.CameraName,c.CameraType,c.ChannelNum,c.IP,c.Port,s.StationID,s.StationName from [dbo].[Sws_Camera] c left join [dbo].[Sws_Station] s on c.StationID=s.StationID";

            if (!user.IsAdmin)     //非admin查询
            {
                sql += " where c.StationID in (select StationID from Sws_UserStation where UserID=@userID) " + condition + "";
            }
            List<CameraInfo> list = this.Context.Database.SqlQuery<CameraInfo>(sql, sqlparameter);
            return list;
        }
        #endregion

        #region 视频录像列表获取
        public IEnumerable<dynamic> GetCameraStationData(bool isadmin, int userid, string name, int innertype)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@userid",userid),
                new SqlParameter("@innertype",innertype),
                new SqlParameter("@name","%"+name+"%")
            };
            string sql = "";
            if (isadmin == true)
            {
                sql = @"select c.CameraID,c.CameraName,c.StationID,s.StationName from [dbo].[Sws_Camera] c
                        left join Sws_Station  s  on s.StationID=c.StationID  where s.InType=@innertype ";
            }
            else
            {
                sql = @"select c.CameraID,c.CameraName,c.StationID,s.StationName from [dbo].[Sws_Camera] c
                        left join Sws_Station s  on s.StationID = c.StationID
                        left join[dbo].[Sws_UserStation] u on c.StationID = u.StationID
                        where s.InType = @innertype and u.UserID = @userid";
            }
            if (!string.IsNullOrEmpty(name))
            {
                sql += " and c.CameraName like @name or s.StationName like @name";
            }
            var query = this.Context.Database.SqlQuery_Dic(sql, sp).ToList();
            return query;
        }
        #endregion
    }
}
