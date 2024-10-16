using System;
using System.Collections.Generic;
using System.Text;
using WNMS.IService;
using WNMS.Model.DataModels;
using Microsoft.Data.SqlClient;
using WNMS.Utility;
using System.Linq;
using WNMS.Model.CustomizedClass;

namespace WNMS.Service
{
    public partial class Sws_GPSModuleService : BaseService, ISws_GPSModuleService
    {
        public int CreateGPSTable(SwsGpsmodule sgp, double lng, double lat)
        {

            int year = DateTime.Now.Year;
            string tablename = sgp.GpSnumber;
            string updateTime = DateTime.Now.ToString("yyyy-MM-dd HH:ss:mm");
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@year", year),
                new SqlParameter("@tablename",tablename),
                new SqlParameter("@lng",lng),
                new SqlParameter("@lat",lat),
                new SqlParameter("@updateTime",updateTime)

            };
            //string sql = @"USE [WNMS_Gps"+ @year + "]"+
            //                @" CREATE TABLE [dbo].["+ @tablename + "]("+
            //                @"[UpdateTime] [datetime] NOT NULL,
            //                [Lng] [float] NULL,
            //                [Lat] [float] NULL,
            //                PRIMARY KEY CLUSTERED 
            //                (
            //                [UpdateTime] ASC
            //                )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
            //                ) ON [PRIMARY]

            //                 insert into [dbo].[RealTimePosition] values ('"+ @tablename + "','"+ @updateTime + "',"+ @lng + ","+ @lat + ")";

            string sql = @"USE [WNMS_Gps" + @year + "]" +
                            @" CREATE TABLE [dbo].[" + @tablename + "](" +
                            @"[UpdateTime] [datetime] NOT NULL,
                            [Lng] [float] NULL,
                            [Lat] [float] NULL,
                            PRIMARY KEY CLUSTERED 
                            (
                            [UpdateTime] ASC
                            )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
                            ) ON [PRIMARY]

                             insert into [dbo].[RealTimePosition] values (@tablename,@updateTime,@lng,@lat)";
            var query = this.Context.Database.InsertData(sql, sp);
            return query;
        }

        #region 轨迹跟踪
        public IEnumerable<UserGPS> GetGpsUser(string sid)
        {
            var query = (from s in Context.Set<SwsGpsborrowing>()
                         join st in Context.Set<SysUser>() on s.UserId equals st.UserId into str
                         from strs in str.DefaultIfEmpty()
                         where s.SerialNumber == sid
                         orderby s.BorrowTime descending
                         select new UserGPS
                         {
                             UserName = strs.NickName,
                             BorrowTime = s.BorrowTime,
                             RemandTime = s.RemandTime,
                             SerialNumber = s.SerialNumber
                         }).ToList();
            return query;
        }
        #endregion

        #region 人员定位
        public List<UserPosition> GetGpsDevices(string SerialNumber)
        {
            string sql = "";
            List<UserPosition> userlist = new List<UserPosition>();
            int year = DateTime.Now.Year;
            if (string.IsNullOrEmpty(SerialNumber))
            {
                SqlParameter[] sqlparameter = new SqlParameter[] {
                new SqlParameter("@year",year)
            };
                sql = @"SELECT '' Name,rt.SerialNumber,rt.Lng,rt.Lat,CONVERT(VARCHAR(20),rt.UpdateTime,120)  UpdateTime  FROM dbo.Sws_GPSModule sg left join [WNMS_Gps" + @year + "].[dbo].RealTimePosition rt on sg.GpSNumber = rt.SerialNumber WHERE  rt.SerialNumber IS NOT NULL";
                userlist = this.Context.Database.SqlQuery<UserPosition>(sql, sqlparameter).ToList();

            }
            else
            {
                SqlParameter[] sqlparameter = new SqlParameter[] {
                new SqlParameter("@year",year),
                new SqlParameter("@SerialNumber",SerialNumber)
            };
                sql = @"SELECT '' Name,rt.SerialNumber,rt.Lng,rt.Lat,CONVERT(VARCHAR(20),rt.UpdateTime,120)  UpdateTime  FROM dbo.Sws_GPSModule sg left join [WNMS_Gps" + @year + "].[dbo].RealTimePosition rt on sg.GpSNumber = rt.SerialNumber WHERE  rt.SerialNumber= @SerialNumber";


                userlist = this.Context.Database.SqlQuery<UserPosition>(sql, sqlparameter).ToList();



            }
            return userlist;
        }
        public string GetUserName(string SerialNumber, string UpdateTime)
        {
            string sql = "";

            int year = DateTime.Now.Year;
            if (string.IsNullOrEmpty(SerialNumber))
            {
                return "";
            }
            else
            {
                SqlParameter[] sqlparameter = new SqlParameter[] {
                new SqlParameter("@UpdateTime",UpdateTime),
                new SqlParameter("@SerialNumber",SerialNumber)
            };
                sql = @" SELECT TOP 1 su.NickName FROM dbo.Sws_Gpsborrowing gps LEFT JOIN dbo.Sys_User su  ON su.UserID = gps.UserID WHERE gps.BorrowTime<=CAST(@UpdateTime  AS DATETIME) AND (gps.RemandTime>=CAST(@UpdateTime  AS DATETIME) OR gps.RemandTime IS NULL) AND su.SerialNumber=@SerialNumber ORDER BY gps.RemandTime DESC";


                var name = this.Context.Database.SqlQuery(sql, sqlparameter);
                if (name != null && name.Rows.Count > 0)
                {
                    return name.Rows[0][0].ToString();
                }
                else
                { return ""; }


            }

        }

        #endregion
        #region 手机人员定位
        public List<UserPositionPhone> GetPhoneGpsDevices(long userid)
        {
            string sql = "";
            List<UserPositionPhone> userlist = new List<UserPositionPhone>();
            int year = DateTime.Now.Year;
            if (userid == 0)
            {
                SqlParameter[] sqlparameter = new SqlParameter[] {
                new SqlParameter("@year",year)
            };
                sql = @"SELECT  sg.NickName,sg.UserID,rt.Lng,rt.Lat,CONVERT(VARCHAR(20),rt.UpdateTime,120)  UpdateTime  FROM dbo.Sys_User sg left join [WNMS_PhoneGps" + @year + "].[dbo].RealTimePosition rt on sg.UserID = rt.UserID";
                userlist = this.Context.Database.SqlQuery<UserPositionPhone>(sql, sqlparameter).ToList();

            }
            else
            {
                SqlParameter[] sqlparameter = new SqlParameter[] {
                new SqlParameter("@year",year),
                new SqlParameter("@userid",userid)
            };
                sql = @"SELECT  sg.NickName,sg.UserID,rt.Lng,rt.Lat,CONVERT(VARCHAR(20),rt.UpdateTime,120)  UpdateTime  FROM dbo.Sys_User sg left join [WNMS_PhoneGps" + @year + "].[dbo].RealTimePosition rt on sg.UserID = rt.UserID WHERE  rt.UserID= @userid";


                userlist = this.Context.Database.SqlQuery<UserPositionPhone>(sql, sqlparameter).ToList();
            }
            return userlist;
        }
        public string GetUserNamePhone(long userid, string UpdateTime)
        {
            string sql = "";

            int year = DateTime.Now.Year;
            if (userid == 0u)
            {
                return "";
            }
            else
            {
                SqlParameter[] sqlparameter = new SqlParameter[] {
                new SqlParameter("@UpdateTime",UpdateTime),
                new SqlParameter("@userid",userid)
            };
                sql = @" SELECT TOP 1 su.NickName FROM dbo.Sws_Gpsborrowing gps LEFT JOIN dbo.Sys_User su  ON su.UserID = gps.UserID WHERE gps.BorrowTime<=CAST(@UpdateTime  AS DATETIME) AND (gps.RemandTime>=CAST(@UpdateTime  AS DATETIME) OR gps.RemandTime IS NULL) AND su.SerialNumber=@SerialNumber ORDER BY gps.RemandTime DESC";
                 
                var name = this.Context.Database.SqlQuery(sql, sqlparameter);
                if (name != null && name.Rows.Count > 0)
                {
                    return name.Rows[0][0].ToString();
                }
                else
                { return ""; }


            }

        }

        #endregion

        #region 移动端实时位置更新
        public int UpdatePosition(long userid, double lng, double lat)
        {
            int year = DateTime.Now.Year;
            DateTime updateTime = DateTime.Now;
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@userid", userid),
                new SqlParameter("@year", year),
                new SqlParameter("@lng",lng),
                new SqlParameter("@lat",lat),
                new SqlParameter("@updateTime",updateTime)
            };
            string sql = @" update  [WNMS_PhoneGps" + @year + "].[dbo].[RealTimePosition] set updateTime = @updateTime,Lng =@lng,Lat=@lat where UserID = @userid";
            var query = this.Context.Database.InsertData(sql, sp);
            string sqlinsert = @" INSERT INTO  [WNMS_PhoneGps" + @year + "].[dbo].[HistoryPosition] ([UserID] ,[UpdateTime] ,[Lng]  ,[Lat]) VALUES (@userid  ,@updateTime  ,@lng  ,@lat)";
            var queryinsert = this.Context.Database.InsertData(sqlinsert, sp);
            return query;
        }
        #endregion

    }
}
