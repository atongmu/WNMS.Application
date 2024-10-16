using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WNMS.Model.CustomizedClass;
using WNMS.Utility;

namespace WNMS.Service
{
    public partial class Sws_UserStationService : BaseService, IService.ISws_UserStationService
    {
        /// <summary>
        /// 加载泵房相关信息
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public IEnumerable<UserStationInfo> LoadUserStationInfo(int UserID)
        {
            SqlParameter[] sp = new SqlParameter[]
             {
                new SqlParameter("@UserID",UserID)
             };

            var query = this.Context.Database.SqlQuery<UserStationInfo>("exec GetUserStationInfo @UserID", sp);
            return query;
        }

        /// <summary>
        /// 加载泵房设备信息
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public IEnumerable<UserStationAndDeviceInfo> LoadUserStationAndDeviceInfo(int UserID)
        {
            SqlParameter[] sp = new SqlParameter[]
             {
                new SqlParameter("@UserID",UserID)
             };

            var query = this.Context.Database.SqlQuery<UserStationAndDeviceInfo>("exec GetUserStationAndDeviceInfo @UserID", sp);
            return query;
        }


        //加载带视频监控的泵房信息
        public IEnumerable<dynamic> LoadStationAndCameraInfo(int UserID)
        {
            SqlParameter[] sp = new SqlParameter[]
             {
                new SqlParameter("@UserID",UserID)
             };

            var query = this.Context.Database.SqlQuery_Dic("exec GetStationAndCameraInfo @UserID", sp);
            return query;
        }
    }
}
