using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using WNMS.Model.CustomizedClass;
using System.Text;
using WNMS.Utility;
using Microsoft.Data.SqlClient;
using System.Linq.Expressions;
using WNMS.IService;
using WNMS.Model.DataModels;
using System.Linq;

namespace WNMS.Service
{
    public class SysUserService : BaseService, IService.ISysUserService
    {
        public SysUserService(DbContext content) : base(content)
        {

        }
        public int QueryID(string PK, string TableName)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@PK",PK),
                new SqlParameter("@TableName",TableName)
            };

            var query = this.Context.Database.SqlQuery<QueryIDNum>("exec QueryID @PK,@TableName", sp)[0].Num;
            return query;
        }
        class QueryIDNum
        {
            public int Num { get; set; }

        }


        //获取用户信息  dynamic 测试
        public IEnumerable<dynamic> GetUserInfos()
        {
            string sql = @"select UserId,Account,NickName,Gender,Department,Phone,IsEnable,CreateTime,u.Email,Remark,d.DptName as DptName from Sys_User u left join Sys_DepartMent d on u.Department=d.DepartmentID";
            SqlParameter[] sqlparameter = new SqlParameter[] { };
            var users = this.Context.Database.SqlQuery_Dic(sql, sqlparameter);
            return users;
        }

        /// <summary>
        /// 页面显示  获取用户信息
        /// </summary>
        /// <param name="funcWhere">查询条件</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="funcOrderby">排序</param>
        /// <param name="order">正序倒序</param>
        /// <returns></returns>
        public PageResult<DptUser> GetUserData(Expression<Func<DptUser, bool>> funcWhere, int pageSize, int pageIndex, string sortName, string order)
        {
            string sql = @"select UserId,Account,NickName,Gender,SerialNumber,Department,Phone,IsEnable,CreateTime,u.Email,Remark,d.DptName as DptName from Sys_User u left join Sys_DepartMent d on u.Department=d.DepartmentID";
            Microsoft.Data.SqlClient.SqlParameter[] sqlparameter = new Microsoft.Data.SqlClient.SqlParameter[] { };

            bool flag = true;
            if (order == "desc") flag = false;
            if (string.IsNullOrWhiteSpace(sortName)) sortName = "CreateTime";

            var userDatas = this.ExcuteQueryPage(funcWhere, pageSize, pageIndex, sortName, sql, sqlparameter, flag);
            return userDatas;
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns></returns>
        public int DeleteUser(int id)
        {
            SysUser user = this.Context.Find<SysUser>(id);
            if (user == null)
            {
                throw new Exception("用户不存在");
            }
            else
            {
                //删除用户角色中间表
                IQueryable<SysUserRole> userRole = this.Query<SysUserRole>(r => r.UserId == id);
                if (userRole != null)
                {
                    foreach (var ur in userRole)
                    {
                        this.Context.Attach(ur);
                        //if (ur.Role == 2)//删除gps表里的数据
                        //{
                        //    int year = DateTime.Now.Year;
                        //    SqlParameter[] sp = new SqlParameter[] {
                        //                        new SqlParameter("@userid", id),
                        //                        new SqlParameter("@year", year),
                        //    }; 
                        //    string sqlinsert = @" DELETE FROM [WNMS_PhoneGps" + @year + "].[dbo].[RealTimePosition] WHERE UserID= @userid";
                        //    var queryinsert = this.Context.Database.InsertData(sqlinsert, sp);
                        //}
                    }
                    this.Context.RemoveRange(userRole);
                }

                //删除用户权限中间表
                IQueryable<SysUserModule> userModule = this.Query<SysUserModule>(m => m.UserId == id);
                if (userModule != null)
                {
                    foreach (var um in userModule)
                    {
                        this.Context.Attach(um);
                    }
                    this.Context.RemoveRange(userModule);
                }

                //删除用户泵房关联表
                IQueryable<SwsUserStation> userStation = this.Query<SwsUserStation>(m => m.UserId == id);
                if (userStation != null)
                {
                    foreach (var us in userStation)
                    {
                        this.Context.Attach(us);
                    }
                    this.Context.RemoveRange(userStation);
                }

                //修改泵房表中的泵房负责人
                IQueryable<SwsStation> station = this.Query<SwsStation>(s => s.Manager == id);
                if (station != null)
                {
                    foreach(var item in station)
              
                    {
                        item.Manager = null;
                        this.Context.Attach(item);
                    }
                    this.Context.UpdateRange(station);
                }

                //删除用户模板关联表

                //删除用户
                this.Context.Attach(user);
                this.Context.Remove(user);
                 

                return this.Context.SaveChanges();
            }
        }

        //分配权限
        public int SetModule(List<SysUserModule> list, int userID)
        {
            List<SysUserModule> userm = this.Query<SysUserModule>(r => r.UserId == userID).AsNoTracking().ToList();
            if (userm != null && userm.Count > 0)
            {
                foreach (var item in userm)
                {
                    this.Context.Attach(item);
                }
                this.Context.RemoveRange(userm);
            }
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    this.Context.Attach(item);
                }
                this.Context.AddRange(list);
            }

            return this.Context.SaveChanges();
        }

        public IEnumerable<AllotStation> GetStation(int id)
        {
            var allList = from s in Query<SwsStation>(s => true)
                          join u in Query<SwsUserStation>(u => u.UserId == id) on s.StationId equals u.StationId into su
                          from sut in su.DefaultIfEmpty()
                          select new AllotStation
                          {
                              Flag = sut.StationId == s.StationId ? true : false,
                              StationID = s.StationId,
                              StationName = s.StationName
                          };
            return allList;
        }
        /// <summary>
        /// 查询用户的定位信息
        /// </summary>
        /// <param name="fliter">过滤条件</param>
        /// <returns></returns>
        public IEnumerable<dynamic> LoadUserPositionInfo(string fliter)
        {
            var year = DateTime.Now.Year;
            string sql = @"select su.UserID,su.NickName,su.SerialNumber,rt.Lng,rt.Lat,CONVERT(VARCHAR(20),rt.UpdateTime,120)  UpdateTime ,su.Phone from Sys_User su
                           left join [WNMS_Gps" + year + "].[dbo].RealTimePosition rt on su.SerialNumber = rt.SerialNumber where su.SerialNumber is not null" + fliter;
            SqlParameter[] sqlparameter = new SqlParameter[] { };
            var users = this.Context.Database.SqlQuery_Dic(sql, sqlparameter);
            return users;
        }
        //查询日志信息
        public IEnumerable<dynamic> GetLogInfoTable(int pageindex, int pagesize, string orderby, string filter, ref int Totalcount)
        {
            SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@startindex",pageindex),
                new SqlParameter("@pageSize",pagesize),
                new SqlParameter("@orderItems",orderby),
                new SqlParameter("@filterString",filter),
                new SqlParameter("@Totalcount",Totalcount){Direction=System.Data.ParameterDirection.Output,DbType=System.Data.DbType.Int32}
            };
            var query = this.Context.Database.SqlQuery_Dic("exec QueryLogInfo @startindex,@pageSize,@orderItems,@filterString,@Totalcount Output", sp).ToList();
            Totalcount = Convert.ToInt32(sp[4].Value);
            return query;
        }
        //查询客服人员
        public IEnumerable<TeamUser> GetCusUser(string nickname)
        {
            if (string.IsNullOrEmpty(nickname))
            {
                var data = from u in Query<SysUser>(s => true)
                           join r in Query<SysUserRole>(r => true) on u.UserId equals r.UserId
                           where r.Role == 2
                           select new TeamUser
                           {
                               NickName = u.NickName,
                               UserID = u.UserId
                           };
                return data;
            }
            else
            {
                var data = from u in Query<SysUser>(s => s.NickName.Contains(nickname))
                           join r in Query<SysUserRole>(r => true) on u.UserId equals r.UserId
                           where r.Role == 2
                           select new TeamUser
                           {
                               NickName = u.NickName,
                               UserID = u.UserId
                           };
                return data;
            }
        }

       //查询客服角色关联的用户
       public IEnumerable<SysUser> GetRoleUserData()
        {
            var query = from r in Context.Set<SysRole>().Where(r => r.RoleName.Contains("客服"))
                        join ur in Context.Set<SysUserRole>() on r.RoleId equals ur.Role into urs
                        from ursa in urs.DefaultIfEmpty()
                        join u in Context.Set<SysUser>() on ursa.UserId equals u.UserId into us
                        from usa in us.DefaultIfEmpty()
                        select new SysUser
                        {
                            UserId = usa.UserId,
                            Account = usa.Account,
                            NickName = usa.NickName,
                            IsAdmin = usa.IsAdmin
                        };
            return query;
        }
    }
}
